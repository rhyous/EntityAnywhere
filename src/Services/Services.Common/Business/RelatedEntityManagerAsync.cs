using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    public class RelatedEntityManager<TEntity, TInterface, TId> : IRelatedEntityManager<TEntity, TInterface, TId>
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
            // ExtensionEntities (Addendum, AlternateId)
            var extensionEntitiesToExpand = GetExtensionEntitiesToExpand(expandPaths?.Select(ep => ep.Entity));
            var relatedExtensionEntities = await GetRelatedExtensionEntitiesAsync(entities, extensionEntitiesToExpand);
            if (relatedExtensionEntities != null && relatedExtensionEntities.Any())
                list.AddRange(relatedExtensionEntities);

            // Related entities
            var attributes = AttributeEvaluator.GetAttributesToExpand(typeof(TEntity), expandPaths?.Select(ep => ep.Entity));
            var relatedEntities = await GetRelatedEntitiesAsync(entities, attributes, expandPaths);
            if (relatedEntities != null && relatedEntities.Any())
                list.AddRange(relatedEntities);
            
            // Related mapping entities
            var mappingAttributes = AttributeEvaluator.GetMappingAttributesToExpand(typeof(TEntity), expandPaths?.Select(ep => ep.Entity));
            var mapList = await GetRelatedMappingEntitiesAsync(entities, mappingAttributes);
            if (mapList != null && mapList.Count > 0)
                list.AddRange(mapList); return list;
        }

        #region injectables
        /// <summary>
        /// Used for both caching and reusing existing clients and is also used for dependency injection, for example, mocking in unit tests.
        /// </summary>
        internal IEntityClientCache ClientsCache
        {
            get { return _ClientsCache ?? (_ClientsCache = new EntityClientCache()); }
            set { _ClientsCache = value; }
        } private IEntityClientCache _ClientsCache;

        /// <summary>
        /// Used for both caching and reusing existing mapping clients and is also used for dependency injection, for example, mocking in unit tests.
        /// </summary>
        internal static Dictionary<string, IMappingEntityClientAsync> MappingClientsCache
        {
            get { return _MappingClientsCache ?? (_MappingClientsCache = new Dictionary<string, IMappingEntityClientAsync>()); }
            set { _MappingClientsCache = value; }
        } private static Dictionary<string, IMappingEntityClientAsync> _MappingClientsCache;

        public IRelatedEntitySorter<TInterface, TId> Sorter
        {
            get { return _Sorter ?? (_Sorter = new RelatedEntitySorter<TInterface, TId>()); }
            set { _Sorter = value; }
        } private IRelatedEntitySorter<TInterface, TId> _Sorter;
        
        public AttributeEvaluator AttributeEvaluator
        {
            get { return _AttributeEvaluator ?? (_AttributeEvaluator = new AttributeEvaluator()); }
            set { _AttributeEvaluator = value; }
        } private AttributeEvaluator _AttributeEvaluator;
        #endregion

        #region internals        
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
                var sortDetails = new SortDetails(entity, extensionEntity, RelatedEntity.Type.OneToMany) { EntityToRelatedEntityProperty = "RelatedId" };
                var relatedEntities = extensionEntities.Select(e => new RelatedEntityOneToMany("EntityId", e));
                var collections = Sorter.Sort(entities, relatedEntities, sortDetails);
                list.AddRange(collections);
            }
            return list;
        }

        internal async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<RelatedEntityAttribute> attributes, IEnumerable<ExpandPath> expandPaths)
        {
            if (entities == null || !entities.Any())
                return null;            
            var list = new List<RelatedEntityCollection>();
            foreach (RelatedEntityAttribute a in attributes)
            {
                List<RelatedEntity> relatedEntities = await GetRelatedEntities(entities, a.RelatedEntity, a.Property);
                var sortDetails = new SortDetails(typeof(TEntity).Name, a.RelatedEntity, RelatedEntity.Type.OneToOne) { EntityToRelatedEntityProperty = a.Property };
                var collections = Sorter.Sort(entities, relatedEntities, sortDetails);
                list.AddRange(collections);
            }
            return list;
        }

        internal async Task<List<RelatedEntityCollection>> GetRelatedMappingEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<RelatedEntityMappingAttribute> attributes, IEnumerable<ExpandPath> expandPaths = null)
        {
            var list = new List<RelatedEntityCollection>();
            foreach (RelatedEntityMappingAttribute a in attributes)
            {
                List<RelatedEntity> relatedEntities = await GetRelatedMappingEntitiesByAttribute(entities, a);
                foreach (var entity in entities)
                {
                    var sortDetails = new SortDetails(typeof(TEntity).Name, a.RelatedEntity, RelatedEntity.Type.ManyToMany) { EntityToRelatedEntityProperty = a.MappingKey };
                    var collections = Sorter.Sort(entities, relatedEntities, sortDetails);
                    list.AddRange(collections);
                }
            }
            return list;
        }

        internal async Task<List<RelatedEntity>> GetRelatedEntities(IEnumerable<TInterface> entities, string entity, string entityIdProperty)
        {
            var client = ClientsCache.Json[entity];
            var relatedEntityIds = entities.Select(e => e.GetPropertyValue(entityIdProperty).ToString());
            var json = await client.GetByIdsAsync(relatedEntityIds);
            var relatedEntities = JsonConvert.DeserializeObject<List<RelatedEntity>>(json);
            return relatedEntities;
        }

        internal static async Task<List<RelatedEntity>> GetRelatedMappingEntitiesByAttribute(IEnumerable<TInterface> entities, RelatedEntityMappingAttribute a)
        {
            IMappingEntityClientAsync mapClient;
            var key = $"{a.MappingEntity}:{a.RelatedEntity}:{a.Entity}";
            if (!MappingClientsCache.TryGetValue(key, out mapClient))
            {
                mapClient = new MappingEntityClientAsync(a.MappingEntity, a.RelatedEntity, a.Entity);
                MappingClientsCache.Add(key, mapClient);
            }
            var entityIds = entities.Select(e => e.Id.ToString());
            var mappingsJson = await mapClient.GetByE2IdsAsync(entityIds, $"$expand={a.RelatedEntity}");
            var mappingsList = JsonConvert.DeserializeObject<List<RelatedEntity>>(mappingsJson);
            return mappingsList;
        }

        internal static IEnumerable<string> GetExtensionEntitiesToExpand(IEnumerable<string> entitiesToExpand)
        {
            var extensionEntities = new List<string> { "Addendum" /* , "AlternateId" */ };
            if (entitiesToExpand == null || !entitiesToExpand.Any())
                return extensionEntities;
            else
                return extensionEntities.Where(ex => entitiesToExpand.Contains(ex) || entitiesToExpand.Contains(ExpandConstants.WildCard));

        }
        #endregion
    }
}