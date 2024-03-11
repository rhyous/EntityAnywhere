using Newtonsoft.Json;
using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    /// <summary>
    /// This is for getting related Extension Entities, such as Addendum, AlternateId, Notes, that could go on any entity.
    /// Addenda is expanded by default. If you want Addenda and something else, you need to specify both Addenda and the
    /// other property in the $expand query.
    /// </summary>
    /// <typeparam name="TEntity">The Entity.</typeparam>
    /// <typeparam name="TInterface">The Entity interface.</typeparam>
    /// <typeparam name="TId">The Entity Id type.</typeparam>
    /// <remarks>This is not an C# static extensions class. The name may cause confusion.</remarks>
    public class RelatedEntityExtensions<TEntity, TInterface, TId>
        : IRelatedEntityExtensions<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly INamedFactory<IAdminEntityClientAsync> _EntityClientAsyncFactory;
        private readonly IExtensionEntityList _ExtensionEntityList;
        private readonly IRelatedEntitySorterHelper<TInterface, TId> _Sorter;

        public RelatedEntityExtensions(
            INamedFactory<IAdminEntityClientAsync> entityClientAsyncFactory,
            IExtensionEntityList extensionEntityList,
            IRelatedEntitySorterHelper<TInterface, TId> sorter)
        {
            _EntityClientAsyncFactory = entityClientAsyncFactory;
            _ExtensionEntityList = extensionEntityList;
            _Sorter = sorter;
        }

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(TInterface entity, IEnumerable<ExpandPath> expandPaths = null)
        {
            return await GetRelatedEntitiesAsync(new[] { entity }, expandPaths);
        }

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<ExpandPath> expandPaths = null)
        {
            var list = new List<RelatedEntityCollection>();
            // Don't expand AlternateId or Addendum on mapping entities
            if (typeof(TEntity).GetAttribute<MappingEntityAttribute>() != null)
                return list;
            var extensionEntitiesToExpand = GetExtensionEntitiesToExpand(expandPaths?.Select(ep => ep.Entity));
            if (extensionEntitiesToExpand == null || !extensionEntitiesToExpand.Any())
                return list;
            var relatedExtensionEntities = await GetRelatedExtensionEntitiesAsync(entities, extensionEntitiesToExpand);
            if (relatedExtensionEntities != null && relatedExtensionEntities.Any())
                list.AddRange(relatedExtensionEntities);
            return list;
        }

        internal IEnumerable<string> GetExtensionEntitiesToExpand(IEnumerable<string> entitiesToExpand)
        {
            var allExtensionEntities = _ExtensionEntityList.EntityNames;
            if (entitiesToExpand != null && entitiesToExpand.Any())
            {
                // if * (wildcard) is passed in, return all extension entities whether autoexpand is true or not because * means all.
                if (entitiesToExpand.Contains(ExpandConstants.WildCard))
                    return allExtensionEntities;

                // When calling $Expand with values, only expand what is specified. Don't autoexpand!
                return allExtensionEntities.Where(ex => entitiesToExpand.Contains(ex));
            }

            return GetExtensionEntitiesToAutoExpand();
        }

        internal IEnumerable<string> GetExtensionEntitiesToAutoExpand()
        {
            var toAutoExpand = _ExtensionEntityList.EntityNames
                                                   .Where(e => _ExtensionEntityList.ShouldAutoExpand(e))
                                                   .ToList();

            var exclusionsAttribute = typeof(TEntity).GetAttribute<RelatedEntityExclusionsAttribute>(false);
            if (exclusionsAttribute != null && exclusionsAttribute.Exclusions != null && exclusionsAttribute.Exclusions.Any())
            {
                if (exclusionsAttribute.Exclusions.Any(e => e == ExpandConstants.WildCard))
                    toAutoExpand.Clear();
                else
                    toAutoExpand.RemoveAny(exclusionsAttribute.Exclusions);
            }
            return toAutoExpand;
        }

        internal async Task<List<RelatedEntityCollection>> GetRelatedExtensionEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<string> extensionEntitiesToExpand)
        {
            if (entities == null || !entities.Any())
                return null;
            var list = new List<RelatedEntityCollection>();
            var entity = typeof(TEntity).Name;
            foreach (var extensionEntity in extensionEntitiesToExpand)
            {
                var client = _EntityClientAsyncFactory.Create(extensionEntity);
                var entityIds = entities.Select(e => e.Id.ToString()).ToList();
                var json = await client.CallByCustomUrlAsync($"{client.EntityPluralized}/{entity}/Ids", HttpMethod.Post, entityIds);
                if (string.IsNullOrWhiteSpace(json) || json.Equals("null", StringComparison.OrdinalIgnoreCase))
                    continue;
                var extensionEntities = JsonConvert.DeserializeObject<OdataObjectCollection>(json);
                var sortDetails = new SortDetails(entity, extensionEntity, RelatedEntity.Type.OneToMany) { EntityToRelatedEntityProperty = "EntityId" };
                RelatedEntityCollection collection = extensionEntities;
                _Sorter.Sort(entities, collection, sortDetails, list);
            }
            return list;
        }
    }
}