using Newtonsoft.Json.Linq;
using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.RelatedEntities;
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
        public async Task<RelatedEntityCollection> GetRelatedEntitiesAsync(TInterface entity, IEnumerable<string> entitiesToExpand = null)
        {
            var type = typeof(TEntity);
            var attributes = GetAttributes(entitiesToExpand, type);
            var list = new RelatedEntityCollection();
            foreach (RelatedEntityAttribute a in attributes)
            {
                var client = new EntityClientAsync(a.Entity);
                var relatedEntityKeyValue = entity.GetPropertyValue(a.Property).ToString();
                var relatedEntityJson = await client.GetAsync(relatedEntityKeyValue);
                var jObject = JObject.Parse(relatedEntityJson);
                var relatedEntity = new RelatedEntity {Id = jObject.GetValue(a.ForeignKey).ToString(), Json = new JRaw(relatedEntityJson) };
                list.Entities.Add(relatedEntity);
            }
            return list;
        }

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<string> entitiesToExpand = null)
        {
            if (entities == null || !entities.Any())
                return null;
            var type = entities.First().GetType();
            var attributes = GetAttributes(entitiesToExpand, type);
            var list = new List<RelatedEntityCollection>();
            foreach (RelatedEntityAttribute a in attributes)
            {
                var client = new EntityClientAsync(a.Entity);
                var relatedEntityKeyValues = entities.Select(e => e.GetPropertyValue(a.Property).ToString());
                var relatedEntities = await client.GetByIdsAsync(relatedEntityKeyValues);
                var jArray = JArray.Parse(relatedEntities);
                foreach (var entity in entities)
                {
                    var collection = new RelatedEntityCollection { Entity = a.Entity };
                    foreach (var jToken in jArray)
                    {
                        var id = jToken[a.ForeignKey].ToString();
                        if (entity.GetPropertyValue(a.Property).ToString() == id)
                        {
                            collection.Entities.Add(new RelatedEntity { Id = id, Json = new JRaw(jToken) });
                            list.Add(collection);
                        }
                    }
                }
            }
            return list;
        }

        internal IEnumerable<RelatedEntityAttribute> GetAttributes(IEnumerable<string> entitiesToExpand, Type type)
        {
            return type.GetProperties()
                                 .Select(p => p.GetCustomAttribute<RelatedEntityAttribute>(true))
                                 .Where(a => a != null && ((a.AutoExpand && (entitiesToExpand == null || !entitiesToExpand.Any())) || entitiesToExpand.Contains(a.Entity)));
        }
    }
}
