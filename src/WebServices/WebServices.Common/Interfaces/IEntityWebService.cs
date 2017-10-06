using Rhyous.Odata.Csdl;
using Rhyous.WebFramework.Entities;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IEntityWebService<TEntity, TId>
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        /// <summary>
        /// Gets the metadata about the entity.
        /// </summary>
        /// <returns>The metadata about the entity.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        CsdlEntity<TEntity> GetMetadata();

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>All Entities</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        int GetCount();

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>All Entities</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<TEntity>> GetAll();

        /// <summary>
        /// Gets all entities with the provided ids.
        /// </summary>
        /// <param name="ids">A list of entity ids.</param>
        /// <returns>All entities with the provided ids.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<TEntity>> GetByIds(List<TId> ids);

        /// <summary>
        /// Gets an entity be a specific id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>The entity with the specified id.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<TEntity> Get(string id);
        
        /// <summary>
        /// Gets an entity's property value by a specific id and property name.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="property">The property name.</param>
        /// <returns>The value of the specific property of the specific entity.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

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
        List<OdataObject<TEntity>> Post(List<TEntity> entity);

        /// <summary>
        /// Replaces an entity at the specified id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The replaced entity.</returns>
        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<TEntity> Put(string id, TEntity entity);

        /// <summary>
        /// Used to update multiple properties of an existing entity without first getting the entity.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="patchedEntity">An object that contains a stub entity instance with the only properties set being the ones that will change. It also has a list of changed properties.</param>
        /// <returns>The patched entity, fetched completely after update.</returns>
        [OperationContract]
        [WebInvoke(Method = "PATCH", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<TEntity> Patch(string id, PatchedEntity<TEntity> patchedEntity);

        /// <summary>
        /// Deletes the entity by the specified id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>True if deleted or if already deleted. False otherwise.</returns>
        [OperationContract]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);

        /// <summary>
        /// Gets addenda from the entity using the specified entity id.
        /// </summary>
        /// <param name="id">The id of the entity to get addenda for. This is not the addendum entity id.</param>
        /// <returns>The addenda for an entity instance.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<Addendum> GetAddenda(string id);

        /// <summary>
        /// Gets addenda from the entity using the specified entity ids.
        /// </summary>
        /// <param name="ids">A list of entity ids. These are not addendum entity ids.</param>
        /// <returns>The addenda for an entity instances.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        List<Addendum> GetAddendaByEntityIds(List<string> ids);

        /// <summary>
        /// Gets a specific addendum for a specific entity.
        /// </summary>
        /// <param name="id">The id of the entity to get addenda for. This is not the addendum entity id.</param>
        /// <param name="name">The name of the addendum to get.</param>
        /// <returns>A specific addendum for a specific entity</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Addendum GetAddendaByName(string id, string name);
    }
}
