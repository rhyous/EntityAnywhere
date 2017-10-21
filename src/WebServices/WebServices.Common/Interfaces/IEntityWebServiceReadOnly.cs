using Rhyous.Odata.Csdl;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IEntityWebServiceReadOnly<TEntity, TId>
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
        List<OdataObject<TEntity, TId>> GetAll();

        /// <summary>
        /// Gets all entities with the provided ids.
        /// </summary>
        /// <param name="ids">A list of entity ids.</param>
        /// <returns>All entities with the provided ids.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<TEntity, TId>> GetByIds(List<TId> ids);

        /// <summary>
        /// Gets an entity be a specific id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>The entity with the specified id.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<TEntity, TId> Get(string id);

        /// <summary>
        /// Gets an entity's property value by a specific id and property name.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="property">The property name.</param>
        /// <returns>The value of the specific property of the specific entity.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);
    }
}
