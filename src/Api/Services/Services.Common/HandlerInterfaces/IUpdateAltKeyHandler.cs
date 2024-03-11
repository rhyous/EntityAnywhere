using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IUpdateAltKeyHandler<TEntity, TInterface, TId, TAltKey>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
           where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        TInterface Update(TId id, PatchedEntity<TInterface, TId> patchedEntity);
        List<TInterface> Update(PatchedEntityCollection<TInterface, TId> patchedEntityCollection);
    }
}