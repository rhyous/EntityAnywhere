using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public interface IMappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id> : IEntityClientAsync<TEntity, TId>
        where TEntity : class, IId<TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        /// <summary>
        /// Gets the mapped entity by a list of Entity1 ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of Entity1 ids.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>A list of mapped entities.</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetByE1IdsAsync(IEnumerable<TE1Id> ids, bool forwardExceptions = true);

        /// <summary>
        /// Gets the mapped entity by a list of Entity1 ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of Entity1 ids.</param>
        /// <param name="urlParameters">Url parameters as a string.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>A list of mapped entities.</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetByE1IdsAsync(IEnumerable<TE1Id> ids, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets the mapped entity by a list of Entity2 ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of Entity2 ids.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>A list of mapped entities.</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetByE2IdsAsync(IEnumerable<TE2Id> ids, bool forwardExceptions = true);

        /// <summary>
        /// Gets the mapped entity by a list of Entity2 ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of Entity2 ids.</param>
        /// <param name="urlParameters">Url parameters as a string.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>A list of mapped entities.</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetByE2IdsAsync(IEnumerable<TE2Id> ids, string urlParameters, bool forwardExceptions = true);
    }

    public interface IAdminMappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id> : IMappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id>
        where TEntity : class, IId<TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
    }
}