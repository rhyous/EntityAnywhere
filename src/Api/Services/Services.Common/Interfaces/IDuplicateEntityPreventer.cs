using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IDuplicateEntityPreventer<TEntity, TInterface, TId, TAltKey>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        Task CheckAsync(IEnumerable<TInterface> entities);
        void RemoveTracked(IEnumerable<TInterface> entities);
    }
}