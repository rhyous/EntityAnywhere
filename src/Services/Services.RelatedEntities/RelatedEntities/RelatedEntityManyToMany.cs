using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    /// <summary>
    /// This class is used to process properties decorated with the [RelatedEntityMappingAttribute].
    /// </summary>
    /// <typeparam name="TEntity">The Entity type.</typeparam>
    /// <typeparam name="TInterface">The Entity Interface</typeparam>
    /// <typeparam name="TId">The Entity's Id type.</typeparam>
    public class RelatedEntityManyToMany<TEntity, TInterface, TId> 
          : IRelatedEntityManyToMany<TEntity, TInterface, TId> 
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRelatedEntityOneToMany<TEntity, TInterface, TId> _RelatedEntityOneToMany;
        private readonly AttributeEvaluator _AttributeEvaluator;

        public RelatedEntityManyToMany(
            IRelatedEntityOneToMany<TEntity, TInterface, TId> relatedEntityOneToMany,
            AttributeEvaluator attributeEvaluator)
        {
            _RelatedEntityOneToMany = relatedEntityOneToMany;
            _AttributeEvaluator = attributeEvaluator;
        }

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(TInterface entity, IEnumerable<ExpandPath> expandPaths = null)
        {
            return await GetRelatedEntitiesAsync(new[] { entity }, expandPaths);
        }

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<ExpandPath> expandPaths = null)
        {
            var list = new List<RelatedEntityCollection>();
            var attributes = _AttributeEvaluator.GetAttributesToExpand<RelatedEntityMappingAttribute>(typeof(TEntity), expandPaths?.Select(ep => ep.Entity));
            if (attributes == null || !attributes.Any())
                return null;
            var subExpandPaths = new Dictionary<string, string>();
            foreach (var path in expandPaths)
            {
                if (!string.IsNullOrWhiteSpace(path.Parenthesis))
                    subExpandPaths.Add(path.Entity, path.Parenthesis);
            }
            foreach (var a in attributes)
            {
                var entity = string.IsNullOrWhiteSpace(a.EntityAlias) ? a.Entity : a.EntityAlias;
                var relatedEntity = string.IsNullOrWhiteSpace(a.RelatedEntityAlias) ? a.RelatedEntity : a.RelatedEntityAlias;
                var mappingEntity = string.IsNullOrWhiteSpace(a.MappingEntityAlias) ? a.MappingEntity : a.MappingEntityAlias;
                string InnerUrlParams = "";
                if (subExpandPaths.TryGetValue(relatedEntity, out string subExpandPath))
                    InnerUrlParams += $"({subExpandPath})";
                var relatedEntityCollections = await _RelatedEntityOneToMany.GetRelatedEntitiesAsync(entities, new[] { new ExpandPath { Entity = mappingEntity, Parenthesis = $"$expand={relatedEntity}{InnerUrlParams}" } });
                foreach (RelatedEntityCollection rec in relatedEntityCollections)
                {
                    Dictionary<string, RelatedEntityCollection> tmpCollections = new Dictionary<string, RelatedEntityCollection>();
                    foreach (RelatedEntity re in rec)
                    {
                        foreach (var subRec in re.RelatedEntityCollection)
                        {
                            subRec.Entity = entity;
                            subRec.EntityId = rec.EntityId;
                            if (tmpCollections.TryGetValue(subRec.Entity + subRec.EntityId, out RelatedEntityCollection existingCollection))
                            {
                                existingCollection.AddRange(subRec);
                                continue;
                            }
                            subRec.Parent = null;
                            tmpCollections.Add(subRec.Entity + subRec.EntityId, subRec);
                        }
                    }
                    list.AddRange(tmpCollections.Values);
                }
            }
            return list;
        }
    }
}