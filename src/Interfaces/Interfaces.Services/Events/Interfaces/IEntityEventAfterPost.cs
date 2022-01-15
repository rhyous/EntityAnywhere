using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfterPost<TEntity> : IEntityEvent<TEntity>
    {
        void AfterPost(IEnumerable<TEntity> postedItems);
    }
}
