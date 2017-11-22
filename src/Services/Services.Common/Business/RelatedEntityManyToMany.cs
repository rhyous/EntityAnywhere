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
    public class RelatedEntityManyToMany<TEntity, TInterface, TId> : IGetRelatedEntitiesAsync<TEntity, TInterface, TId>
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
            throw new NotImplementedException();
            //var list = new List<RelatedEntityCollection>();
            //var mappingAttributes = AttributeEvaluator.GetForeignAttributesToExpand(typeof(TEntity), expandPaths?.Select(ep => ep.Entity));
            //var mapList = await GetRelatedMappingEntitiesAsync(entities, mappingAttributes);           
            //if (mapList != null && mapList.Count > 0)
            //    list.AddRange(mapList);
            //return list;
        }

        internal async Task<List<RelatedEntityCollection>> GetRelatedMappingEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<RelatedEntityForeignAttribute> attributes, IEnumerable<ExpandPath> expandPaths = null)
        {
            var list = new List<RelatedEntityCollection>();
            //foreach (RelatedEntityForeignAttribute a in attributes)
            //{
            //    List<RelatedEntity> relatedEntities = await GetRelatedMappingEntitiesByAttribute(entities, a);
            //    foreach (var entity in entities)
            //    {
            //        var sortDetails = new SortDetails(typeof(TEntity).Name, a.RelatedEntity, RelatedEntity.Type.ManyToMany) { EntityToRelatedEntityProperty = a.MappingKey };
            //        var collections = Sorter.Sort(entities, relatedEntities, sortDetails);
            //        list.AddRange(collections);
            //    }
            //}
            return list;
        }

        internal async Task<List<RelatedEntity>> GetRelatedMappingEntitiesByAttribute(IEnumerable<TInterface> entities, RelatedEntityForeignAttribute a)
        {
            //IMappingEntityClientAsync mapClient;
            //var key = $"{a.MappingEntity}:{a.RelatedEntity}:{a.Entity}";
            //if (!MappingClientsCache.TryGetValue(key, out mapClient))
            //{
            //    mapClient = new MappingEntityClientAsync(a.MappingEntity, a.RelatedEntity, a.Entity);
            //    MappingClientsCache.Add(key, mapClient);
            //}
            //var entityIds = entities.Select(e => e.Id.ToString());
            //var mappingsJson = await mapClient.GetByE2IdsAsync(entityIds, $"$expand={a.RelatedEntity}");
            //var mappingsList = JsonConvert.DeserializeObject<List<RelatedEntity>>(mappingsJson);
            //return mappingsList;
            return null;
        }

        /// <summary>
        /// Used for both caching and reusing existing mapping clients and is also used for dependency injection, for example, mocking in unit tests.
        /// </summary>
        internal Dictionary<string, IMappingEntityClientAsync> MappingClientsCache
        {
            get { return _MappingClientsCache ?? (_MappingClientsCache = new Dictionary<string, IMappingEntityClientAsync>()); }
            set { _MappingClientsCache = value; }
        } private Dictionary<string, IMappingEntityClientAsync> _MappingClientsCache;

        public IRelatedEntitySorter<TInterface> Sorter
        {
            get { return _Sorter ?? (_Sorter = new RelatedEntitySorter<TInterface, TId>()); }
            set { _Sorter = value; }
        } private static IRelatedEntitySorter<TInterface> _Sorter;
        
        internal AttributeEvaluator AttributeEvaluator
        {
            get { return _AttributeEvaluator ?? (_AttributeEvaluator = new AttributeEvaluator()); }
            set { _AttributeEvaluator = value; }
        } private AttributeEvaluator _AttributeEvaluator;
    }
}