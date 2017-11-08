using Rhyous.Odata;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Clients
{
    /// <summary>
    /// The interface for a Mapping Entity client.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TE1Id">The Entity1 id type. Entity1 should always be the entity with less instances.</typeparam>
    /// <typeparam name="TE2Id">The Entity2 id type. Entity2 should always be the entity with more instances.</typeparam>
    public interface IMappingEntityClient<TEntity, TId, TE1Id, TE2Id> : IMappingEntityWebService<TEntity, TId, TE1Id, TE2Id>, IEntityClient<TEntity, TId>
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        /// <summary>
        /// The name of the first mapped entity.
        /// </summary>
        string Entity1 { get; }
        /// <summary>
        /// The name of the first mapped entity pluralized.
        /// </summary>
        string Entity1Pluralized { get; }
        /// <summary>
        /// The name of the property that maps to Entity1.
        /// By default,this mapped entity property should be: Entity1 + "Id".
        /// </summary>
        string Entity1Property { get; }

        /// <summary>
        /// The name of the second mapped entity.
        /// </summary>
        string Entity2 { get; }
        /// <summary>
        /// The name of the second mapped entity pluralized.
        /// </summary>
        string Entity2Pluralized { get; }
        /// <summary>
        /// The name of the property that maps to Entity2.
        /// By default, this mapped entity property should be: Entity2 + "Id".
        /// </summary>
        string Entity2Property { get; }

        /// <summary>
        /// Gets the mapped entity by a list of Entity1 ids.
        /// </summary>
        /// <param name="ids">A list of Entity1 ids.</param>
        /// <returns>A list of mapped entities.</returns>
        OdataObjectCollection<TEntity, TId> GetByE1Ids(IEnumerable<TE1Id> ids);
        /// <summary>
        /// Gets the mapped entity by a list of Entity2 ids.
        /// </summary>
        /// <param name="ids">A list of Entity2 ids.</param>
        /// <returns>A list of mapped entities.</returns>
        OdataObjectCollection<TEntity, TId> GetByE2Ids(IEnumerable<TE2Id> ids);
    }
}