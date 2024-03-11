using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBeforeDeleteMany<TEntity, TId> : IEntityEvent<TEntity, TId>
        where TEntity : IId<TId>
    {
        void BeforeDeleteMany(IEnumerable<TEntity> entities);
    }
}