using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IDeleteManyHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        Task<Dictionary<TId, bool>> HandleAsync(IEnumerable<string> ids);
    }
}