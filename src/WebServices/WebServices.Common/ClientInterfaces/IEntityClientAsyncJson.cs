using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    /// <summary>
    /// A common class that any client can implement to talk to any entity's web services asynchronously.
    /// It has all the same methods that exist on the common webservice are available to all client implementations.
    /// This interface expects all the returns to be streams of hte serialized object.
    /// </summary>
    public interface IEntityClientAsync : IEntityClientBase
    {
        #region Async
        /// <summary>
        /// Gets the metadata about the entity. Call is asynchonous.
        /// </summary>
        /// <returns>The metadata about the entity.</returns>
        Task<string> GetMetadataAsync();

        /// <summary>
        /// Gets all entities. Call is asynchonous.
        /// </summary>
        /// <returns>All Entities</returns>
        Task<string> GetAllAsync(string urlParameters = null);

        /// <summary>
        /// This provides an additional option to make a get call with query parameters. Call is asynchonous.
        /// </summary>
        /// <param name="queryParameters">a string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>A list of entities that match the query parameters.</returns>
        Task<string> GetByQueryParametersAsync(string queryParameters);
        
        /// <summary>
        /// Gets all entities with the provided ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of entity ids.</param>
        /// <returns>All entities with the provided ids.</returns>
        Task<string> GetByIdsAsync(IEnumerable<string> ids, string urlParameters = null);

        /// <summary>
        /// Gets all entities with the provided values of a given property.
        /// </summary>
        /// <param name="values">A list of values.</param>
        /// <returns>All entities with the provided values of a given property.</returns>
        Task<string> GetByPropertyValuesAsync(string property, List<string> values, string urlParameters = null);

        /// <summary>
        /// Gets an entity be a specific id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>The entity with the specified id.</returns>
        Task<string> GetAsync(string id, string urlParameters = null);

        /// <summary>
        /// Gets an entity's property value by a specific id and property name. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="property">The property name.</param>
        /// <returns>The value of the specific property of the specific entity.</returns>
        Task<string> GetPropertyAsync(string id, string property);

        /// <summary>
        /// Updates a property on the entity with the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="property">The name of the property to update.</param>
        /// <param name="value">The new value.</param>
        /// <returns>The actual value after it is changed.</returns>
        Task<string> UpdatePropertyAsync(string id, string property, string value);

        /// <summary>
        /// Adds an entity. Call is asynchonous.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        Task<string> PostAsync(HttpContent content);

        /// <summary>
        /// Replaces an entity at the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The replaced entity.</returns>
        Task<string> PutAsync(string id, HttpContent content);

        /// <summary>
        /// Used to update multiple properties of an existing entity without first getting the entity. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="patchedEntity">An object that contains a stub entity instance with the only properties set being the ones that will change. It also has a list of changed properties.</param>
        /// <returns>The patched entity, fetched completely after update.</returns>
        Task<string> PatchAsync(string id, HttpContent content);

        /// <summary>
        /// Deletes the entity by the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>True if deleted or if already deleted. False otherwise.</returns>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Gets addenda from the entity using the specified entity id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity to get addenda for. This is not the addendum entity id.</param>
        /// <returns>The addenda for an entity instance.</returns>
        Task<string> GetAddendaAsync(string id);

        /// <summary>
        /// Gets addenda from the entity using the specified entity ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of entity ids. These are not addendum entity ids.</param>
        /// <returns>The addenda for an entity instances.</returns>
        Task<string> GetAddendaByEntityIdsAsync(List<string> ids);

        /// <summary>
        /// Gets a specific addendum for a specific entity. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity to get addenda for. This is not the addendum entity id.</param>
        /// <param name="name">The name of the addendum to get.</param>
        /// <returns>A specific addendum for a specific entity</returns>
        Task<string> GetAddendaByNameAsync(string id, string name);

        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hsotname/path/EntityService.svc/.</param>
        /// <returns>A list of entities returned by the custom service.</returns>
        Task<string> GetByCustomUrlAsync(string url);

        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <remarks>This overload specifically is used for HttpClient actions other than GET, such as POST, that has content.</remarks>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hostname/path/EntityService.svc/.</param>
        /// <param name="httpMethod"></param>
        /// <param name="content">The HttpContent form. It must already be in the correct format.</param>
        /// <returns>A list of entities returned by the custom service.</returns>
        Task<string> GetByCustomUrlAsync(string urlPart, Func<string, HttpContent, Task<HttpResponseMessage>> httpMethod, HttpContent content);
        
        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <remarks>This overload specifically is used for HttpClient actions other than GET, such as POST, that has content.</remarks>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hostname/path/EntityService.svc/.</param>
        /// <param name="httpMethod">The HttpClient method to call.</param>
        /// <param name="content">The content in object form, it will be converted to JSON.</param>
        /// <returns>A list of entities returned by the custom service.</returns>
        Task<string> GetByCustomUrlAsync(string urlPart, Func<string, HttpContent, Task<HttpResponseMessage>> httpMethod, object content);
        #endregion
    }
}