using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class PutHandler<TEntity, TInterface, TId> : IPutHandler<TEntity, TInterface, TId>
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
        private readonly IUrlParameters _UrlParameters;
        private readonly IRequestUri _RequestUri;

        public PutHandler(IRelatedEntityEnforcer<TEntity> relatedEntityEnforcer,
                          IDistinctPropertiesEnforcer<TEntity, TInterface, TId> distinctPropertiesEnforcer,
                          IEntityEventAll<TEntity, TId> entityEvent,
                          IInputValidator<TEntity, TId> inputValidator,
                          IServiceCommon<TEntity, TInterface, TId> service,
                          IRelatedEntityProvider<TEntity, TInterface, TId> relatedEntityProvider,
                          IUrlParameters urlParameters,
                          IRequestUri requestUri)
        {
            _RelatedEntityEnforcer = relatedEntityEnforcer ?? throw new ArgumentNullException(nameof(relatedEntityEnforcer));
            _DistinctPropertiesEnforcer = distinctPropertiesEnforcer ?? throw new ArgumentNullException(nameof(distinctPropertiesEnforcer));
            _EntityEvent = entityEvent ?? throw new ArgumentNullException(nameof(entityEvent));
            _InputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _RelatedEntityProvider = relatedEntityProvider ?? throw new ArgumentNullException(nameof(relatedEntityProvider));
            _UrlParameters = urlParameters ?? throw new ArgumentNullException(nameof(urlParameters));
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        public async Task<OdataObject<TEntity, TId>> Handle(string id, TEntity entity)
        {
            if (!_InputValidator.CleanAndValidate(ref id, entity))
                throw new RestException(HttpStatusCode.BadRequest);
            await _RelatedEntityEnforcer.Enforce(new[] { entity });
            await _DistinctPropertiesEnforcer.Enforce(new[] { entity }, ChangeType.Update);
            var existingEntity = _Service.Get(id.To<TId>())?.ToConcrete<TEntity, TInterface>();
            _EntityEvent?.BeforePut(entity, existingEntity);
            var result = _Service.Replace(id.To<TId>(), entity)
                                 .ToConcrete<TEntity, TInterface>()
                                 .AsOdata<TEntity, TId>(_RequestUri.Uri);
            _EntityEvent?.AfterPut(result.Object, existingEntity);
            await _RelatedEntityProvider.ProvideAsync(new[] { result }, _UrlParameters.Collection);
            return result;
        }
    }
}