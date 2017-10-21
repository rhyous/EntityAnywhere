using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IEntityWebService<TEntity, TId> : IEntityWebServiceReadOnly<TEntity, TId>, IEntityWebServiceAddenda
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        /// <summary>
        /// Updates a property on the entity with the specified id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="property">The name of the property to update.</param>
        /// <param name="value">The new value.</param>
        /// <returns>The actual value after it is changed.</returns>
        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json)]
        string UpdateProperty(string id, string property, string value);

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<TEntity, TId>> Post(List<TEntity> entity);

        /// <summary>
        /// Replaces an entity at the specified id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The replaced entity.</returns>
        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<TEntity, TId> Put(string id, TEntity entity);

        /// <summary>
        /// Used to update multiple properties of an existing entity without first getting the entity.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="patchedEntity">An object that contains a stub entity instance with the only properties set being the ones that will change. It also has a list of changed properties.</param>
        /// <returns>The patched entity, fetched completely after update.</returns>
        [OperationContract]
        [WebInvoke(Method = "PATCH", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<TEntity, TId> Patch(string id, PatchedEntity<TEntity> patchedEntity);

        /// <summary>
        /// Deletes the entity by the specified id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>True if deleted or if already deleted. False otherwise.</returns>
        [OperationContract]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);
    }
}
