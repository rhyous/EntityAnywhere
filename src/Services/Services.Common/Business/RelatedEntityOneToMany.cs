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
    public class RelatedEntityOneToMany<TEntity, TInterface, TId> : IGetRelatedEntitiesAsync<TEntity, TInterface, TId>
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
            var attributes = AttributeEvaluator.GetForeignAttributesToExpand(typeof(TEntity), expandPaths?.Select(ep => ep.Entity));
            var relatedEntities = await GetRelatedEntitiesAsync(entities, attributes, expandPaths);
            if (relatedEntities != null && relatedEntities.Any())
                list.AddRange(relatedEntities);
            return list;
        }

        internal async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<RelatedEntityForeignAttribute> attributes, IEnumerable<ExpandPath> expandPaths)
        {
            if (entities == null || !entities.Any())
                return null;
            var list = new List<RelatedEntityCollection>();
            foreach (RelatedEntityForeignAttribute a in attributes)
            {
                var expandPath = expandPaths.FirstOrDefault(ep=> a.RelatedEntity == ep.Entity);
                RelatedEntityCollection relatedEntities = await GetRelatedEntities(entities, a.RelatedEntity, a.EntityKeyProperty, a.ForeignKeyProperty, expandPath?.Parenthesis); // Cast is intentional
                var sortDetails = new SortDetails(typeof(TEntity).Name, a.RelatedEntity, RelatedEntity.Type.OneToMany) { EntityToRelatedEntityProperty = a.ForeignKeyProperty };
                var collections = Sorter.Sort(entities, relatedEntities, sortDetails);
                list.AddRange(collections);
            }
            return list;
        }

        internal async Task<OdataObjectCollection> GetRelatedEntities(IEnumerable<TInterface> entities, string entity, string entityKeyProperty, string foreignKeyProperty, string urlParams = null)
        {
            var client = ClientsCache.Json[entity];
            var relatedEntityValues = entities.Select(e => e.GetPropertyValue(entityKeyProperty).ToString()).ToList();
            var json = await client.GetByPropertyValuesAsync(foreignKeyProperty, relatedEntityValues, urlParams);
            var relatedEntityCollection = JsonConvert.DeserializeObject<OdataObjectCollection>(json);
            return relatedEntityCollection;
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

        public AttributeEvaluator AttributeEvaluator
        {
            get { return _AttributeEvaluator ?? (_AttributeEvaluator = new AttributeEvaluator()); }
            set { _AttributeEvaluator = value; }
        } private AttributeEvaluator _AttributeEvaluator;
    }
}