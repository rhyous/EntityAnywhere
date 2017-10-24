using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.RelatedEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    public interface IRelatedEntityManager<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        Task<RelatedEntityCollection> GetRelatedEntitiesAsync(TInterface entity, IEnumerable<string> entitiesToExpand = null);
        Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<string> entitiesToExpand = null);
    }
}