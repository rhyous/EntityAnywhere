using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IUpdateHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        TInterface Update(TId id, PatchedEntity<TInterface, TId> patchedEntity);
        List<TInterface> Update(PatchedEntityCollection<TInterface, TId> patchedEntityCollection);
        List<TInterface> Update(IEnumerable<TInterface> entities, params string[] changedProperties);
        string UpdateProperty(TId id, string property, string value);
        TInterface Replace(TId id, TInterface entity);
    }
}