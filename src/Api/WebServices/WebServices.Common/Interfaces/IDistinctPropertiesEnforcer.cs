using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IDistinctPropertiesEnforcer<TEntity, TInterface, TId>
    {
        /// <summary>
        /// Enforce related entities, this overlad is used for POSTS
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entities"></param>
        Task Enforce(IEnumerable<TEntity> entities, ChangeType changeType);
    }
}