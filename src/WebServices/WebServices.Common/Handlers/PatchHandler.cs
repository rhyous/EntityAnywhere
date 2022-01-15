using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class PatchHandler<TEntity, TInterface, TId> : IPatchHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRelatedEntityEnforcer<TEntity> _RelatedEntityEnforcer;
        private readonly IDistinctPropertiesEnforcer<TEntity, TInterface, TId> _DistinctPropertiesEnforcer;
        private readonly IEntityEventAll<TEntity, TId> _EntityEvent;
        private readonly IInputValidator<TEntity, TId> _InputValidator;
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;
        private readonly IRelatedEntityProvider<TEntity, TInterface, TId> _RelatedEntityProvider;
        private readonly IEntityInfo<TEntity> _EntityInfo;
        private readonly IUrlParameters _UrlParameters;
        private readonly IRequestUri _RequestUri;

        public PatchHandler(IRelatedEntityEnforcer<TEntity> relatedEntityEnforcer,
                            IDistinctPropertiesEnforcer<TEntity, TInterface, TId> distinctPropertiesEnforcer,
                            IEntityEventAll<TEntity, TId> entityEvent,
                            IInputValidator<TEntity, TId> inputValidator,
                            IServiceCommon<TEntity, TInterface, TId> service,
                            IRelatedEntityProvider<TEntity, TInterface, TId> relatedEntityProvider,
                            IEntityInfo<TEntity> entityInfo,
                            IUrlParameters urlParameters,
                            IRequestUri requestUri)
        {
            _RelatedEntityEnforcer = relatedEntityEnforcer ?? throw new ArgumentNullException(nameof(relatedEntityEnforcer));
            _DistinctPropertiesEnforcer = distinctPropertiesEnforcer ?? throw new ArgumentNullException(nameof(distinctPropertiesEnforcer));
            _EntityEvent = entityEvent ?? throw new ArgumentNullException(nameof(entityEvent));
            _InputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _RelatedEntityProvider = relatedEntityProvider ?? throw new ArgumentNullException(nameof(relatedEntityProvider));
            _EntityInfo = entityInfo;
            _UrlParameters = urlParameters ?? throw new ArgumentNullException(nameof(urlParameters));
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        public async Task<OdataObject<TEntity, TId>> HandleAsync(string id, PatchedEntity<TEntity, TId> patchedEntity)
        {
            if (!_InputValidator.CleanAndValidate(ref id, patchedEntity))
                throw new RestException(HttpStatusCode.BadRequest);
            var existingEntity = _Service.Get(id.To<TId>())?.ToConcrete<TEntity, TInterface>();
            if (existingEntity == null)
                throw new RestException($"{typeof(TEntity).Name} {id}", HttpStatusCode.NotFound);
            var entityWithChanges = existingEntity.ConcreteCopy<TEntity, TInterface>();
            foreach (var property in patchedEntity.ChangedProperties)
            {
                if (!_EntityInfo.Properties.TryGetValue(property, out PropertyInfo pi))
                    throw new RestException($"Entity '{typeof(TEntity).Name}' does not have a '{property}' property.", HttpStatusCode.BadRequest);
                pi.SetValue(entityWithChanges, patchedEntity.Entity.GetPropertyValue(property));
            }

            await _RelatedEntityEnforcer.Enforce(new[] { entityWithChanges }, patchedEntity.ChangedProperties);    // Check to be sure RelatedEntities actually exist
            await _DistinctPropertiesEnforcer.Enforce(new[] { entityWithChanges }, ChangeType.Update);
            patchedEntity.Entity = entityWithChanges;
            patchedEntity.ChangedProperties = patchedEntity.ChangedProperties.Where(cp => !string.IsNullOrWhiteSpace(cp)).ToHashSet();
            var patchedEntityComparison = new PatchedEntityComparison<TEntity, TId> { PatchedEntity = patchedEntity, Entity = existingEntity };
            _EntityEvent?.BeforePatch(patchedEntityComparison);
            var result = _Service.Update(id.To<TId>(), patchedEntity.AsInterface<TEntity, TInterface, TId>())
                                 .ToConcrete<TEntity, TInterface>()
                                 .AsOdata<TEntity, TId>(_RequestUri.Uri);
            patchedEntity.Entity = result.Object;
            _EntityEvent?.AfterPatch(patchedEntityComparison);
            await _RelatedEntityProvider.ProvideAsync(new[] { result }, _UrlParameters.Collection);
            return result;
        }

        public async Task<OdataObjectCollection<TEntity, TId>> Handle(PatchedEntityCollection<TEntity, TId> patchedEntityCollection)
        {
            if (!_InputValidator.CleanAndValidate(patchedEntityCollection))
                throw new RestException(HttpStatusCode.BadRequest);

            var ids = patchedEntityCollection.PatchedEntities.Select(pe => pe.Entity.Id);
            var queryable = (await _Service.GetAsync(ids, _UrlParameters.Collection.Clone(keysToExclude: new[] { "$top", "$skip" })));
            var existingEntitiesMappedById = queryable?.ToDictionary(e => e.Id);
            // Eliminate any empty, whitespace, global properties
            var comparisons = new List<PatchedEntityComparison<TEntity, TId>>();
            var entitiesWithChanges = new List<TEntity>();

            if (patchedEntityCollection.ChangedProperties != null && patchedEntityCollection.ChangedProperties.Any())
                patchedEntityCollection.ChangedProperties = patchedEntityCollection.ChangedProperties
                                                                                   .Where(cp => !string.IsNullOrWhiteSpace(cp))
                                                                                   .ToHashSet();
            var patchedEntityCollection2 = new PatchedEntityCollection<TInterface, TId>(); // No global changed properties
            var allChangedProperties = patchedEntityCollection.ChangedProperties.ToHashSet(); // Used for event only
            foreach (var pe in patchedEntityCollection.PatchedEntities)
            {
                if (!existingEntitiesMappedById.TryGetValue(pe.Entity.Id, out TInterface existingEntity))
                    throw new RestException($"{typeof(TEntity).Name} {pe.Entity.Id}", HttpStatusCode.NotFound);

                // Merge with global changed properties
                if (pe.ChangedProperties == null)
                    pe.ChangedProperties = patchedEntityCollection.ChangedProperties;
                else
                    pe.ChangedProperties.UnionWith(patchedEntityCollection.ChangedProperties);

                var entityWithChanges = existingEntity.ToConcrete<TEntity, TInterface>();
                entitiesWithChanges.Add(entityWithChanges);
                var patchedEntity = new PatchedEntity<TInterface, TId>
                {
                    Entity = entityWithChanges,
                    ChangedProperties = pe.ChangedProperties.Where(cp => !string.IsNullOrWhiteSpace(cp))
                                                            .ToHashSet()
                };
                patchedEntityCollection2.Add(patchedEntity);
                foreach (var property in patchedEntity.ChangedProperties)
                {
                    if (!_EntityInfo.Properties.TryGetValue(property, out PropertyInfo pi))
                        throw new RestException($"Entity '{typeof(TEntity).Name}' does not have a '{property}' property.", HttpStatusCode.BadRequest);
                    pi.SetValue(entityWithChanges, pi.GetValue(pe.Entity));
                }

                var patchedEntityComparison = new PatchedEntityComparison<TEntity, TId>
                {
                    PatchedEntity = patchedEntity.ToConcrete<TEntity, TInterface, TId>(),
                    Entity = existingEntity.ToConcrete<TEntity, TInterface>()
                };
                comparisons.Add(patchedEntityComparison);
                allChangedProperties.UnionWith(pe.ChangedProperties);
            }
            await _RelatedEntityEnforcer.Enforce(entitiesWithChanges, allChangedProperties);    // Check to be sure RelatedEntities actually exist
            await _DistinctPropertiesEnforcer.Enforce(entitiesWithChanges, ChangeType.Update);
            _EntityEvent?.BeforePatchMany(comparisons);
            var result = _Service.Update(patchedEntityCollection2)
                     .ToConcrete<TEntity, TInterface>()
                     .AsOdata<TEntity, TId>(_RequestUri.Uri);
            _EntityEvent?.AfterPatchMany(comparisons);
            await _RelatedEntityProvider.ProvideAsync(result, _UrlParameters.Collection);
            return result;
        }
    }
}