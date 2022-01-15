using Newtonsoft.Json;
using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{

    /// <summary>
    /// This class is used to process properties decorated with the [RelatedEntityForeignAttribute].
    /// </summary>
    /// <typeparam name="TEntity">The Entity type.</typeparam>
    /// <typeparam name="TInterface">The Entity Interface</typeparam>
    /// <typeparam name="TId">The Entity's Id type.</typeparam>
    public class RelatedEntityOneToMany<TEntity, TInterface, TId> 
        : IRelatedEntityOneToMany<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly INamedFactory<IEntityClientAsync> _EntityClientAsyncFactory;
        private readonly AttributeEvaluator _AttributeEvaluator;
        private IRelatedEntitySorterHelper<TInterface, TId> _Sorter;

        public RelatedEntityOneToMany(INamedFactory<IEntityClientAsync> entityClientAsyncFactory,
                                      AttributeEvaluator attributeEvaluator,
                                      IRelatedEntitySorterHelper<TInterface, TId> sorter)
        {
            _EntityClientAsyncFactory = entityClientAsyncFactory;
            _AttributeEvaluator = attributeEvaluator;
            _Sorter = sorter;
        }

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(TInterface entity, IEnumerable<ExpandPath> expandPaths = null)
        {
            return await GetRelatedEntitiesAsync(new[] { entity }, expandPaths);
        }

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<ExpandPath> expandPaths = null)
        {
            var list = new List<RelatedEntityCollection>();
            var attributes = _AttributeEvaluator.GetAttributesToExpand<RelatedEntityForeignAttribute>(typeof(TEntity), expandPaths?.Select(ep => ep.Entity));
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
                var relatedEntity = string.IsNullOrWhiteSpace(a.RelatedEntityAlias) ? a.RelatedEntity : a.RelatedEntityAlias;
                var expandPath = expandPaths.FirstOrDefault(ep => a.RelatedEntity == ep.Entity || a.RelatedEntityAlias == ep.Entity);
                RelatedEntityCollection relatedEntities = await GetRelatedEntities(entities, a.RelatedEntity, a.EntityKeyProperty, a.ForeignKeyProperty, expandPath?.Parenthesis); // Cast is intentional
                if (relatedEntities == null || !relatedEntities.Any())
                    continue;
                relatedEntities.RelatedEntity = relatedEntity;
                var sortDetails = new SortDetails(typeof(TEntity).Name, relatedEntity, RelatedEntity.Type.OneToMany) { EntityToRelatedEntityProperty = a.ForeignKeyProperty };
                if (!string.IsNullOrWhiteSpace(a.EntityProperty))
                    sortDetails.EntityProperty = a.EntityProperty;
                _Sorter.Sort(entities, relatedEntities, sortDetails, list);
            }
            return list;
        }

        internal async Task<OdataObjectCollection> GetRelatedEntities(IEnumerable<TInterface> entities, string entity, string entityKeyProperty, string foreignKeyProperty, string urlParams = null)
        {
            var client = _EntityClientAsyncFactory.Create(entity);
            var relatedEntityValues = entities.Select(e => e.GetPropertyValue<TId>(entityKeyProperty).ToString()).ToList();
            var json = await client.GetByPropertyValuesAsync(foreignKeyProperty, relatedEntityValues, urlParams);
            if (string.IsNullOrWhiteSpace(json) || json.Equals("null", StringComparison.OrdinalIgnoreCase))
                return null;
            var relatedEntityCollection = JsonConvert.DeserializeObject<OdataObjectCollection>(json);
            return relatedEntityCollection;
        }
    }
}