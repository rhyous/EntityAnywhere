using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public interface IMappingEntityClientAsync : IEntityClientAsync
    {
        /// <summary>
        /// Gets the mapped entity by a list of Entity1 ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of Entity1 ids.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>A list of mapped entities.</returns>
        Task<string> GetByE1IdsAsync(IEnumerable<string> ids, bool forwardExceptions = true);

        /// <summary>
        /// Gets the mapped entity by a list of Entity1 ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of Entity1 ids.</param>
        /// <param name="urlParameters">The url parameters.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>A list of mapped entities.</returns>
        Task<string> GetByE1IdsAsync(IEnumerable<string> ids, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets the mapped entity by a list of Entity2 ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of Entity2 ids.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>A list of mapped entities.</returns>
        Task<string> GetByE2IdsAsync(IEnumerable<string> ids, bool forwardExceptions = true);

        /// <summary>
        /// Gets the mapped entity by a list of Entity2 ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of Entity2 ids.</param>
        /// <param name="urlParameters">The url parameters.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>A list of mapped entities.</returns>
        Task<string> GetByE2IdsAsync(IEnumerable<string> ids, string urlParameters, bool forwardExceptions = true);
    }

    public interface IAdminMappingEntityClientAsync : IMappingEntityClientAsync
    {
    }
}