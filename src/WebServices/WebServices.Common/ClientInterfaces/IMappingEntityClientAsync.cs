using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public interface IMappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id> : IMappingEntityClient<TEntity, TId, TE1Id, TE2Id>, IEntityClient<TEntity, TId>
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {        /// <summary>
             /// Gets the mapped entity by a list of Entity1 ids. Call is asynchonous.
             /// </summary>
             /// <param name="ids">A list of Entity1 ids.</param>
             /// <returns>A list of mapped entities.</returns>
        Task<List<OdataObject<TEntity>>> GetByE1IdsAsync(IEnumerable<TE1Id> ids);
        /// <summary>
        /// Gets the mapped entity by a list of Entity2 ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of Entity2 ids.</param>
        /// <returns>A list of mapped entities.</returns>
        Task<List<OdataObject<TEntity>>> GetByE2IdsAsync(IEnumerable<TE2Id> ids);
    }
}