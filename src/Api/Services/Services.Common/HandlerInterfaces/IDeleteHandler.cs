using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IDeleteHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        bool Delete(TId id);
        Dictionary<TId, bool> DeleteMany(IEnumerable<TId> ids);
    }
}