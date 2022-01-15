using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBeforeDeleteMany<TEntity> : IEntityEvent<TEntity>
    {
        void BeforeDeleteMany(IEnumerable<TEntity> entities);
    }
}