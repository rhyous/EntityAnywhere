using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IAddAltKeyHandler<TEntity, TInterface, TId, TAltKey>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
           where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        Task<List<TInterface>> AddAsync(IEnumerable<TInterface> entities);
    }
}