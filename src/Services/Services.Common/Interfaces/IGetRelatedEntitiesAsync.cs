using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    public interface IGetRelatedEntitiesAsync<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(TInterface entity, IEnumerable<ExpandPath> expandPaths = null);
        Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<ExpandPath> expandPaths = null);
    }
}
