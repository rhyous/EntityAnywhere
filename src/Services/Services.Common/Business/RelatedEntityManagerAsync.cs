using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    public class RelatedEntityManager<TEntity, TInterface, TId> : IRelatedEntityManager<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public const string WildCard = "*";

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(TInterface entity, IEnumerable<ExpandPath> expandPaths = null)
        {
            return await GetRelatedEntitiesAsync(new[] { entity }, expandPaths);
        }        

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<ExpandPath> expandPaths = null)
        {
            var attributes = GetAttributesToExpand(expandPaths?.Select(ep => ep.Entity));
            var list = await GetRelatedEntitiesAsync(entities, attributes, expandPaths);
            
            var mappingAttributes = GetMappingAttributesToExpand(expandPaths?.Select(ep => ep.Entity));
            var mapList = await GetRelatedMappingEntitiesAsync(entities, mappingAttributes);
            if (mapList != null && mapList.Count > 0)
                list.AddRange(mapList); return list;
        }

        #region injectables
        /// <summary>
        /// Used for both caching and reusing existing clients and is also used for dependency injection, for example, mocking in unit tests.
        /// </summary>
        internal static Dictionary<string, IEntityClientAsync> ClientsCache
        {
            get { return _ClientsCache ?? (_ClientsCache = new Dictionary<string, IEntityClientAsync>()); }
            set { _ClientsCache = value; }
        } private static Dictionary<string, IEntityClientAsync> _ClientsCache;
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
        #endregion

        #region internals
        internal async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<RelatedEntityAttribute> attributes, IEnumerable<ExpandPath> expandPaths)
        {
            if (entities == null || !entities.Any())
                return null;            
            var list = new List<RelatedEntityCollection>();
            foreach (RelatedEntityAttribute a in attributes)
            {
                List<RelatedEntity> relatedEntities = await GetRelatedEntitiesByAttribute(entities, a);
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

        internal async Task<List<RelatedEntity>> GetRelatedEntitiesByAttribute(IEnumerable<TInterface> entities, RelatedEntityAttribute a)
        {
            IEntityClientAsync client;
            if (!ClientsCache.TryGetValue(a.RelatedEntity, out client))
            {
                client = new EntityClientAsync(a.RelatedEntity);
                ClientsCache.Add(a.RelatedEntity, client);
            }
            var relatedEntityIds = entities.Select(e => e.GetPropertyValue(a.Property).ToString());
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

        internal IEnumerable<RelatedEntityAttribute> GetAttributesToExpand(IEnumerable<string> entitiesToExpand = null)
        {
            var attribs = typeof(TEntity).GetProperties().Select(p => p.GetCustomAttribute<RelatedEntityAttribute>(true));
            return GetAttributesToExpand(entitiesToExpand, attribs);
        }

        internal IEnumerable<RelatedEntityMappingAttribute> GetMappingAttributesToExpand(IEnumerable<string> entitiesToExpand = null)
        {
            var attribs = typeof(TEntity).GetCustomAttributes<RelatedEntityMappingAttribute>();
            return GetAttributesToExpand(entitiesToExpand, attribs);
        }

        internal static IEnumerable<T> GetAttributesToExpand<T>(IEnumerable<string> entitiesToExpand, IEnumerable<T> attribs)
            where T : IRelatedEntity
        {
            var safeAttribs = attribs.Where(a => a != null);
            if (entitiesToExpand == null || !entitiesToExpand.Any())
                return safeAttribs.Where(a => a != null && a.AutoExpand);
            else 
                return safeAttribs.Where(a => entitiesToExpand.Contains(a.RelatedEntity) || entitiesToExpand.Contains(WildCard));
        }
        #endregion
    }
}