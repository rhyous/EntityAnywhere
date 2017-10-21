using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// A service contract for a Mapping Entity. This inherits the service contract for a regular entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TE1Id">The Entity1 id type. Entity1 should always be the entity with less instances.</typeparam>
    /// <typeparam name="TE2Id">The Entity2 id type. Entity2 should always be the entity with more instances.</typeparam>
    [ServiceContract]
    public interface IMappingEntityWebService<TEntity, TId, TE1Id, TE2Id> : IEntityWebService<TEntity, TId>
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        /// <summary>
        /// Gets the mapped entity by a list of Entity1 ids.
        /// </summary>
        /// <param name="ids">A list of Entity1 ids.</param>
        /// <returns>A list of mapped entities.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<TEntity, TId>> GetByE1Ids(List<TE1Id> ids);

        /// <summary>
        /// Gets the mapped entity by a list of Entity2 ids.
        /// </summary>
        /// <param name="ids">A list of Entity2 ids.</param>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<TEntity, TId>> GetByE2Ids(List<TE2Id> ids);
    }
}
