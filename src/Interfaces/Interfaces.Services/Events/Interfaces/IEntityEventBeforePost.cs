using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBeforePost<TEntity> : IEntityEvent<TEntity>
    {
        void BeforePost(IEnumerable<TEntity> postedItems);
    }
}
