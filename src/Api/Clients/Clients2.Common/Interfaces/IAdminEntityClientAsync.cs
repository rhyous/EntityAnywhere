using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Clients2
{
    public interface IAdminEntityClientAsync : IEntityClientAsync { }
    public interface IAdminEntityClientAsync<TEntity, TId> : IEntityClientAsync<TEntity, TId>
        where TEntity : class, IId<TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
    }
}
