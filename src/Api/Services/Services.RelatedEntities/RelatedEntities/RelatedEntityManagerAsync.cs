using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    public class RelatedEntityManager<TEntity, TInterface, TId>
        : IRelatedEntityManager<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRelatedEntityAccessors<TEntity, TInterface, TId> _RelatedEntityAccessors;

        public RelatedEntityManager(IRelatedEntityAccessors<TEntity, TInterface, TId> RelatedEntityAccessors)
        {
            _RelatedEntityAccessors = RelatedEntityAccessors;
        }

        public virtual async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(TInterface entity, IEnumerable<ExpandPath> expandPaths = null)
        {
            return await GetRelatedEntitiesAsync(new[] { entity }, expandPaths);
        }

        public virtual async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<ExpandPath> expandPaths = null)
        {
            var list = new List<RelatedEntityCollection>();
            var tasks = new List<Task<List<RelatedEntityCollection>>>();
            foreach (var accessor in _RelatedEntityAccessors.List)
            {
                tasks.Add(accessor.GetRelatedEntitiesAsync(entities, expandPaths));
            }
            while (tasks.Count > 0)
            {
                var t = await Task.WhenAny(tasks);
                var relatedEntities = t.Result;
                if (relatedEntities != null && relatedEntities.Any())
                    list.AddRange(relatedEntities);
                tasks.Remove(t);
            }
            return list;
        }

        /// <inheritdoc />
        public virtual List<RelatedEntityCollection> GetRelatedEntities(TInterface entity, NameValueCollection parameters)
        {
            var expandPaths = new ExpandParser().Parse(parameters);
            return TaskRunner.RunSynchonously(GetRelatedEntitiesAsync, entity, expandPaths);
        }

        /// <inheritdoc />
        public virtual List<RelatedEntityCollection> GetRelatedEntities(IEnumerable<TInterface> entities, NameValueCollection parameters)
        {
            var expandPaths = new ExpandParser().Parse(parameters);
            return TaskRunner.RunSynchonously(GetRelatedEntitiesAsync, entities, expandPaths);
        }
    }
}