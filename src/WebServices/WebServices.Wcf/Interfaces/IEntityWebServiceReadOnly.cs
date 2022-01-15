using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    [ServiceContract]
    public interface IEntityWebServiceReadOnly<TEntity, TId> : IMetadataWebService<CsdlEntity>, IDisposable
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        /// <summary>
        /// Gets count of entities.
        /// </summary>
        /// <returns>All Entities</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Task<int> GetCountAsync();

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>All Entities</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Task<OdataObjectCollection<TEntity, TId>> GetAllAsync();

        /// <summary>
        /// Gets all entities with the provided ids.
        /// </summary>
        /// <param name="ids">A list of entity ids.</param>
        /// <returns>All entities with the provided ids.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        Task<OdataObjectCollection<TEntity, TId>> GetByIdsAsync(List<TId> ids);

        /// <summary>
        /// Gets all entities with the provided values of a given property.
        /// </summary>
        /// <param name="collection">A list of values.</param>
        /// <returns>All entities with the provided values of a given property.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        Task<OdataObjectCollection<TEntity, TId>> GetByPropertyValuesAsync(string property, List<string> values);

        /// <summary>
        /// Gets an entity be a specific id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>The entity with the specified id.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Task<OdataObject<TEntity, TId>> GetAsync(string id);

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
