using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    /// This class is used to process properties decorated with the [RelatedEntityAttribute].
    /// </summary>
    /// <typeparam name="TEntity">The Entity type.</typeparam>
    /// <typeparam name="TInterface">The Entity Interface</typeparam>
    /// <typeparam name="TId">The Entity's Id type.</typeparam>
    public class RelatedEntityManyToOne<TEntity, TInterface, TId> 
        : IRelatedEntityManyToOne<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public delegate RelatedEntityManyToOne<TEntity, TInterface, TId> Factory(AttributeEvaluator attributeEvaluator);

        private readonly INamedFactory<IEntityClientAsync> _EntityClientAsyncFactory;
        private readonly AttributeEvaluator AttributeEvaluator;
        private readonly IRelatedEntitySorterHelper<TInterface, TId> _Sorter;

        public RelatedEntityManyToOne(INamedFactory<IEntityClientAsync> entityClientAsyncFactory,
                                      AttributeEvaluator attributeEvaluator,
                                      IRelatedEntitySorterHelper<TInterface, TId> sorter)
        {
            _EntityClientAsyncFactory = entityClientAsyncFactory;
            AttributeEvaluator = attributeEvaluator;
            _Sorter = sorter;
        }

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(TInterface entity, IEnumerable<ExpandPath> expandPaths = null)
        {
            return await GetRelatedEntitiesAsync(new[] { entity }, expandPaths);
        }

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<ExpandPath> expandPaths = null)
        {
            var list = new List<RelatedEntityCollection>();
            var attributes = AttributeEvaluator.GetAttributesToExpand<RelatedEntityAttribute>(typeof(TEntity), expandPaths?.Select(ep => ep.Entity));
            var relatedEntities = await GetRelatedEntitiesAsync(entities, attributes, expandPaths);
            if (relatedEntities != null && relatedEntities.Any())
                list.AddRange(relatedEntities);
            return list;
        }

        internal async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<RelatedEntityAttribute> attributes, IEnumerable<ExpandPath> expandPaths)
        {
            if (entities == null || !entities.Any())
                return null;
            var list = new List<RelatedEntityCollection>();
            foreach (RelatedEntityAttribute a in attributes)
            {
                var relatedEntity = string.IsNullOrWhiteSpace(a.RelatedEntityAlias) ? a.RelatedEntity : a.RelatedEntityAlias;
                var expandPath = expandPaths.FirstOrDefault(ep => a.RelatedEntity == ep.Entity || a.RelatedEntityAlias == ep.Entity);
                RelatedEntityCollection relatedEntities = await GetRelatedEntities(entities, a.RelatedEntity, a.Property, a.ForeignKeyProperty, expandPath?.Parenthesis); // Cast is intentional
                if (a.AllowedNonExistentValue != null)
                {
                    var obj = new { Id = a.AllowedNonExistentValue.ToString(), Name = a.AllowedNonExistentValueName };
                    var objJson = JsonConvert.SerializeObject(obj);
                    relatedEntities.Add(new OdataObject { Id = a.AllowedNonExistentValue.ToString(), Object = new JRaw(objJson) });
                }
                if (relatedEntities == null || !relatedEntities.Any())
                    continue;
                relatedEntities.RelatedEntity = relatedEntity;
                var sortDetails = new SortDetails(typeof(TEntity).Name, relatedEntity, RelatedEntity.Type.ManyToOne)
                {
                    EntityToRelatedEntityProperty = a.Property,
                    RelatedEntityIdProperty = a.ForeignKeyProperty
                };
                _Sorter.Sort(entities, relatedEntities, sortDetails, list);
            }
            return list;
        }

        internal async Task<OdataObjectCollection> GetRelatedEntities(IEnumerable<TInterface> entities, string entity, string entityIdProperty, string foreignProperty = "Id", string urlParams = null)
        {
            var client = _EntityClientAsyncFactory.Create(entity);
            var relatedEntityIds = entities.Select(e => e.GetPropertyValue(entityIdProperty)?.ToString()).Where(e => e != null);
            if (!relatedEntityIds.Any())
                return null;
            var json = foreignProperty == "Id"
                ? await client.GetByIdsAsync(relatedEntityIds, urlParams)
                : await client.GetByPropertyValuesAsync(foreignProperty, relatedEntityIds, urlParams);
            if (string.IsNullOrWhiteSpace(json) || json.Equals("null", StringComparison.OrdinalIgnoreCase))
                return null;
            var relatedEntityCollection = JsonConvert.DeserializeObject<OdataObjectCollection>(json);
            return relatedEntityCollection;
        }
    }
}