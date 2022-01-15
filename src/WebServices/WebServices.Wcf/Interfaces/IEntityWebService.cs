using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    [ServiceContract]
    public interface IEntityWebService<TEntity, TId> : IEntityWebServiceReadOnly<TEntity, TId>, IGenerateRespository
        where TEntity : class, IId<TId>, new()
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
        Task<string> UpdatePropertyAsync(string id, string property, string value);

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        Task<OdataObjectCollection<TEntity, TId>> PostAsync(List<TEntity> entity);

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        [OperationContract(IsOneWay = true)]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        Task PostOneWayAsync(List<TEntity> entity);

        /// <summary>
        /// Replaces an entity at the specified id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The replaced entity.</returns>
        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json)]
        Task<OdataObject<TEntity, TId>> PutAsync(string id, TEntity entity);

        /// <summary>
        /// Used to update multiple properties of an existing entity without first getting the entity.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="patchedEntity">An object that contains a stub entity instance with the only properties set being the ones that will change. It also has a list of changed properties.</param>
        /// <returns>The patched entity, fetched completely after update.</returns>
        [OperationContract]
        [WebInvoke(Method = "PATCH", ResponseFormat = WebMessageFormat.Json)]
        Task<OdataObject<TEntity, TId>> PatchAsync(string id, PatchedEntity<TEntity, TId> patchedEntity);

        /// <summary>
        /// Used to update multiple properties of an existing entity without first getting the entity.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="patchedEntity">An object that contains a stub entity instance with the only properties set being the ones that will change. It also has a list of changed properties.</param>
        /// <returns>The patched entity, fetched completely after update.</returns>
        [OperationContract]
        [WebInvoke(Method = "PATCH", ResponseFormat = WebMessageFormat.Json)]
        Task<OdataObjectCollection<TEntity, TId>> PatchManyAsync(PatchedEntityCollection<TEntity, TId> patchedEntityCollection);

        /// <summary>
        /// Deletes the entity by the specified id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>True if deleted or if already deleted. False otherwise.</returns>
        [OperationContract]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);

        /// <summary>
        /// Deletes the entity by the specified id.
        /// </summary>
        /// <param name="ids">The ids of the entities to delete.</param>
        /// <returns>True if deleted or if already deleted. False otherwise.</returns>
        [OperationContract]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json)]
        Task<Dictionary<TId, bool>> DeleteManyAsync(IEnumerable<string> ids);

        /// <summary>
        /// The ability to add an extension entity to a given entity using this entities endpoints instead of the extension entity's endpoints.
        /// </summary>
        /// <param name="id">The id of this entity.</param>
        /// <param name="extensionEntity">The name of the extension entity, usually "AlternateId" or "Addendum"</param>
        /// <param name="propertyValue">The posted object, a property value pair.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        Task<OdataObject<TEntity, TId>> PostExtensionAsync(string id, string extensionEntity, PropertyValue propertyValue);

        /// <summary>
        /// The ability to add an extension entity to a given entity using this entities endpoints instead of the extension entity's endpoints.
        /// </summary>
        /// <param name="id">The id of this entity.</param>
        /// <param name="extensionEntity">The name of the extension entity, usually "AlternateId" or "Addendum"</param>
        /// <param name="propertyValue">The posted object, a property value pair.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json)]
        Task<string> UpdateExtensionValueAsync(string id, string extensionEntity, PropertyValue propertyValue);
    }
}