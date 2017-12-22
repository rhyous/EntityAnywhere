using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.WebFramework.Clients;
using Newtonsoft.Json;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public class RelatedEntityExtensions<TEntity, TInterface, TId> : IGetRelatedEntitiesAsync<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(TInterface entity, IEnumerable<ExpandPath> expandPaths = null)
        {
            return await GetRelatedEntitiesAsync(new[] { entity }, expandPaths);
        }

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<ExpandPath> expandPaths = null)
        {
            var list = new List<RelatedEntityCollection>();
            var extensionEntitiesToExpand = GetExtensionEntitiesToExpand(expandPaths?.Select(ep => ep.Entity));
            if (extensionEntitiesToExpand == null || !extensionEntitiesToExpand.Any())
                return list;
            var relatedExtensionEntities = await GetRelatedExtensionEntitiesAsync(entities, extensionEntitiesToExpand);
            if (relatedExtensionEntities != null && relatedExtensionEntities.Any())
                list.AddRange(relatedExtensionEntities);
            return list;
        }

        internal static IEnumerable<string> GetExtensionEntitiesToExpand(IEnumerable<string> entitiesToExpand)
        {
            var extensionEntities = new List<string> { "Addendum" /* , "AlternateId" */ };
            var exclusionsAttribute = typeof(TEntity).GetCustomAttributes(typeof(RelatedEntityExclusionsAttribute), false).FirstOrDefault() as RelatedEntityExclusionsAttribute;
            if (exclusionsAttribute != null && exclusionsAttribute.Exclusions != null && exclusionsAttribute.Exclusions.Any())
            {
                foreach (var exclusion in exclusionsAttribute.Exclusions)
                    extensionEntities.Remove(exclusion);
            }
            if (entitiesToExpand == null || !entitiesToExpand.Any())
                return extensionEntities;
            else
                return extensionEntities.Where(ex => entitiesToExpand.Contains(ex) || entitiesToExpand.Contains(ExpandConstants.WildCard));

        }
        internal async Task<List<RelatedEntityCollection>> GetRelatedExtensionEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<string> extensionEntitiesToExpand)
        {
            if (entities == null || !entities.Any())
                return null;
            var list = new List<RelatedEntityCollection>();
            var entity = typeof(TEntity).Name;
            foreach (var extensionEntity in extensionEntitiesToExpand)
            {
                var client = ClientsCache.Json[extensionEntity];
                var entityIdentifiers = entities.Select(e => new EntityIdentifier { Entity = entity, EntityId = e.Id.ToString() }).ToList();
                var json = await client.GetByCustomUrlAsync($"{client.EntityPluralized}/EntityIdentifiers", client.HttpClient.PostAsync, entityIdentifiers);
                var extensionEntities = JsonConvert.DeserializeObject<OdataObjectCollection>(json);
                var sortDetails = new SortDetails(entity, extensionEntity, RelatedEntity.Type.OneToMany) { EntityToRelatedEntityProperty = "EntityId" };
                RelatedEntityCollection collection = extensionEntities;
                var collections = Sorter.Sort(entities, collection, sortDetails);
                if (collections != null && collections.Any())
                    list.AddRange(collections);
            }
            return list;
        }

        /// <summary>
        /// Used for both caching and reusing existing clients and is also used for dependency injection, for example, mocking in unit tests.
        /// </summary>
        internal IEntityClientCache ClientsCache
        {
            get { return _ClientsCache ?? (_ClientsCache = new EntityClientCache()); }
            set { _ClientsCache = value; }
        } private IEntityClientCache _ClientsCache;

        public IRelatedEntitySorter<TInterface> Sorter
        {
            get { return _Sorter ?? (_Sorter = new RelatedEntitySorter<TInterface, TId>()); }
            set { _Sorter = value; }
        } private IRelatedEntitySorter<TInterface> _Sorter;
    }
}