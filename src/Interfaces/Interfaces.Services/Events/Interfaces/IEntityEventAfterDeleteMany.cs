using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfterDeleteMany<TEntity, TId> : IEntityEvent<TEntity>
        where TEntity : IId<TId>
    {
        void AfterDeleteMany(IEnumerable<TEntity> entities, Dictionary<TId, bool> wasDeleted);
    }
}