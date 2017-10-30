using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public interface IMappingEntityClientAsync : IEntityClientAsync
    {
        /// <summary>
        /// Gets the mapped entity by a list of Entity1 ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of Entity1 ids.</param>
        /// <returns>A list of mapped entities.</returns>
        Task<string> GetByE1IdsAsync(IEnumerable<string> ids, string urlParameters = null);
        /// <summary>
        /// Gets the mapped entity by a list of Entity2 ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of Entity2 ids.</param>
        /// <returns>A list of mapped entities.</returns>
        Task<string> GetByE2IdsAsync(IEnumerable<string> ids, string urlParameters = null);
    }
}