using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityProvider<TEntity, TId>
        where TEntity : class, IId<TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        Task<TEntity> Provide(TId id, bool throwIfNotFound = false);
    }
}
