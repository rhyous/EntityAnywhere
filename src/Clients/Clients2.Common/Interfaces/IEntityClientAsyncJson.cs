using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    /// <summary>
    /// A common class that any client can implement to talk to any entity's web services asynchronously.
    /// It has all the same methods that exist on the common webservice are available to all client implementations.
    /// This interface expects all the returns to be streams of the serialized object.
    /// </summary>
    public interface IEntityClientAsync : IEntityClientBase
    {
        /// <summary>
        /// Gets the metadata about the entity. Call is asynchonous.
        /// </summary>
        /// <returns>The metadata about the entity.</returns>
        Task<CsdlEntity> GetMetadataAsync(bool forwardExceptions = true);

        /// <summary>
        /// Gets all entities. Call is asynchonous.
        /// </summary>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>All Entities</returns>
        Task<string> GetAllAsync(bool forwardExceptions = true);

        /// <summary>
        /// This provides an additional option to make a get call with query parameters. Call is asynchonous.
        /// </summary>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>A list of entities that match the query parameters.</returns>
        Task<string> GetByQueryParametersAsync(string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets all entities with the provided ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of entity ids.</param>
        /// <returns>All entities with the provided ids.</returns>
        Task<string> GetByIdsAsync(IEnumerable<string> ids, bool forwardExceptions = true);

        /// <summary>
        /// Gets all entities with the provided ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of entity ids.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>All entities with the provided ids.</returns>
        Task<string> GetByIdsAsync(IEnumerable<string> ids, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets all entities with the provided values of a given property.
        /// </summary>
        /// <param name="values">A list of values.</param>
        /// <returns>All entities with the provided values of a given property.</returns>
        Task<string> GetByPropertyValuesAsync(string property, IEnumerable<string> values, bool forwardExceptions = true);

        /// <summary>
        /// Gets all entities with the provided values of a given property.
        /// </summary>
        /// <param name="values">A list of values.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns>All entities with the provided values of a given property.</returns>
        Task<string> GetByPropertyValuesAsync(string property, IEnumerable<string> values, string urlParameters, bool forwardExceptions = false);

        /// <summary>
        /// Gets an entity by a specific id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>The entity with the specified id.</returns>
        Task<string> GetAsync(string id, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by a specific id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>The entity with the specified id.</returns>
        Task<string> GetAsync(string id, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by the AlternateKey. This is only a valid call for Entities
        /// with the AlternateKey attribute. Call is asynchonous.
        /// </summary>
        /// <param name="altKey"></param>
        /// <returns>The entity with the specified alternate key.</returns>
        Task<string> GetByAlternateKeyAsync(string altKey, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by the AlternateKey. This is only a valid call for Entities
        /// with the AlternateKey attribute. Call is asynchonous.
        /// </summary>
        /// <param name="altKey"></param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>The entity with the specified alternate key.</returns>
        Task<string> GetByAlternateKeyAsync(string altKey, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by an Alternate Id. Call is asynchonous.
        /// </summary>
        /// <param name="altId">The alternate Id</param>
        /// <returns>The entity with the specified alternate key.</returns>
        Task<string> GetByAlternateIdAsync(string altId, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by an Alternate Id. Call is asynchonous.
        /// </summary>
        /// <param name="altId">The alternate Id</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>The entity with the specified alternate key.</returns>
        Task<string> GetByAlternateIdAsync(string altId, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity's property value by a specific id and property name. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="property">The property name.</param>
        /// <returns>The value of the specific property of the specific entity.</returns>
        Task<string> GetPropertyAsync(string id, string property, bool forwardExceptions = true);

        /// <summary>
        /// Updates a property on the entity with the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="property">The name of the property to update.</param>
        /// <param name="value">The new value.</param>
        /// <returns>The actual value after it is changed.</returns>
        Task<string> UpdatePropertyAsync(string id, string property, string value, bool forwardExceptions = true);

        /// <summary>
        /// Adds an entity. Call is asynchonous.
        /// </summary>
        /// <param name="entity">The content to post, which is usually an entity to add.</param>
        /// <returns>The added entity.</returns>
        Task<string> PostAsync(HttpContent content, bool forwardExceptions = true);

        /// <summary>
        /// Adds an entity. Call is asynchonous.
        /// </summary>
        /// <param name="content">The content to post, which is usually an entity to add.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns>The added entity.</returns>
        /// 
        Task<string> PostAsync(HttpContent content, string urlParameters, bool forwardExceptions = false);

        /// <summary>
        /// Adds an entity. Call is asynchonous.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        Task<string> PostAsync(object entity, bool forwardExceptions = true);

        /// <summary>
        /// Adds an entity. Call is asynchonous.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>The added entity.</returns>
        Task<string> PostAsync(object entity, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Replaces an entity at the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="entity">The entity to put, json serialized as a StringContent.</param>
        /// <returns>The replaced entity.</returns>
        Task<string> PutAsync(string id, HttpContent content, bool forwardExceptions = true);

        /// <summary>
        /// Replaces an entity at the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="entity">The entity to put, json serialized as a StringContent.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns>The replaced entity.</returns>
        Task<string> PutAsync(string id, HttpContent content, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Replaces an entity at the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="entity">The entity to put.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns>The replaced entity.</returns>
        Task<string> PutAsync(string id, object entity, bool forwardExceptions = true);

        /// <summary>
        /// Replaces an entity at the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="entity">The entity to put.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns>The replaced entity.</returns>
        Task<string> PutAsync(string id, object entity, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Used to update multiple properties of an existing entity without first getting the entity. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="patchedEntity">An HttpContent object that should be a JSON serialized version of PatchedEntity{TEntity, TId}</TEntity>.</param>
        /// <returns>The patched entity, fetched completely after update.</returns>
        Task<string> PatchAsync(string id, HttpContent content, bool forwardExceptions = true);

        /// <summary>
        /// Used to update multiple properties of an existing entity without first getting the entity. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="content">An HttpContent object that should be a JSON serialized version of PatchedEntity{TEntity, TId}</TEntity>.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns>The patched entity, fetched completely after update.</returns>
        Task<string> PatchAsync(string id, HttpContent content, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Used to update multiple properties of an existing entity without first getting the entity. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="patchedEntity">An object that should be a PatchedEntity{TEntity, TId}.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns>The patched entity, fetched completely after update.</returns>

        Task<string> PatchAsync(string id, object obj, bool forwardExceptions = true);

        /// <summary>
        /// Used to update multiple properties of an existing entity without first getting the entity. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="patchedEntity">An object that should be a PatchedEntity{TEntity, TId}.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns>The patched entity, fetched completely after update.</returns>
        Task<string> PatchAsync(string id, object patchedEntity, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Used to update multiple properties of an existing entity without first getting the entity. Call is asynchonous.
        /// </summary>
        /// <param name="content">An HttpContent object that should be a JSON serialized version of a PatchedEntityCollection{TEntity, TId}</TEntity>.</param>
        /// <returns>The patched entity, fetched completely after update.</returns>
        Task<string> PatchManyAsync(HttpContent content, bool forwardExceptions = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">An HttpContent object that should be a JSON serialized version of a PatchedEntityCollection{TEntity, TId}</TEntity>.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns></returns>
        Task<string> PatchManyAsync(HttpContent content, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patchedEntityCollection">An object that should be a PatchedEntityCollection{TEntity, TId}.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns></returns>
        Task<string> PatchManyAsync(object patchedEntityCollection, bool forwardExceptions = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patchedEntityCollection">An object that should be a PatchedEntityCollection{TEntity, TId}.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns></returns>
        Task<string> PatchManyAsync(object pachedEntityCollection, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Deletes the entity by the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>True if deleted or if already deleted. False otherwise.</returns>
        Task<bool> DeleteAsync(string id, bool forwardExceptions = true);

        /// <summary>
        /// Deletes the entity by the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="ids">The ids of the entities to delete.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns>A collection of results showing which entities deleted and which didn't.</returns>
        Task<Dictionary<TId, bool>> DeleteManyAsync<TId>(IEnumerable<TId> ids, bool forwardExceptions = true);

        /// <summary>
        /// Deletes the entity by the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="ids">The ids of the entities to delete.</param>
        /// <param name="forwardExceptions">If the call fails do you want to forward the exceptions? If not, an empty response is provided.</param>
        /// <returns>A collection of results showing which entities deleted and which didn't.</returns>
        Task<Dictionary<string, bool>> DeleteManyAsync(IEnumerable<string> ids, bool forwardExceptions = true);

        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hsotname/path/EntityService.svc/.</param>
        /// <returns>A list of entities returned by the custom service.</returns>
        Task<string> GetByCustomUrlAsync(string url, bool forwardExceptions = true);

        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <remarks>This overload specifically is used for HttpClient actions other than GET, such as POST, that has content.</remarks>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hostname/path/EntityService.svc/.</param>
        /// <param name="httpMethod">The HttpMethod, such as GET, POST, PATCH, PUT, DELETE, etc.</param>
        /// <param name="content">The content in object form, it will be converted to JSON.</param>
        /// <returns>A list of entities returned by the custom service.</returns>
        Task<string> CallByCustomUrlAsync(string urlPart, HttpMethod httpMethod, object content, bool forwardExceptions = true);

        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hostname/path/EntityService.svc/.</param>
        /// <param name="httpMethod">The HttpMethod, such as GET, POST, PATCH, PUT, DELETE, etc.</param>
        /// <param name="content">The content in object form, it will be converted to JSON.</param>
        /// <returns>A single entity returned by the custom service. The service must wrap it in an OdataObject.</returns>
        Task<string> GetObjectByCustomUrlAsync(string urlPart, HttpMethod httpMethod, object content, bool forwardExceptions = true);

        /// <summary>
        /// Queries for the count of entities.
        /// </summary>
        /// <returns>The count of entities</returns>
        Task<int> GetCountAsync(bool forwardExceptions = true);

        /// <summary>
        /// Queries for the count of entities.
        /// </summary>
        /// <param name="urlParameters">Url parameters other than $count. This lets you filter.</param>
        /// <returns>The count of entities</returns>
        Task<int> GetCountAsync(string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// The ability to add an extension entity (i.e. property value pair) to any entity
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="extensionEntity">The singular name of the extension entity. Usually Addendum or AlternateId.</param>
        /// <param name="content">The content as an httpContent object.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns></returns>
        Task<string> PostExtensionAsync(string id, string extensionEntity, HttpContent content, bool forwardExceptions = true);

        /// <summary>
        /// The ability to add an extension entity (i.e. property value pair) to any entity
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="extensionEntity">The singular name of the extension entity. Usually Addendum or AlternateId.</param>
        /// <param name="content">The content as an httpContent object.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns></returns>
        Task<string> PostExtensionAsync(string id, string extensionEntity, HttpContent content, string urlParameters, bool forwardExceptions = false);

        /// <summary>
        /// The ability to add an extension entity (i.e. property value pair) to any entity
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="extensionEntity">The singular name of the extension entity. Usually Addendum or AlternateId.</param>
        /// <param name="obj">The content as an object.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns></returns>
        Task<string> PostExtensionAsync(string id, string extensionEntity, object obj, bool forwardExceptions = true);

        /// <summary>
        /// The ability to add an extension entity (i.e. property value pair) to any entity
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="extensionEntity">The singular name of the extension entity. Usually Addendum or AlternateId.</param>
        /// <param name="obj">The content as an object.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns></returns>
        Task<string> PostExtensionAsync(string id, string extensionEntity, object obj, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// This will ask the repository to generate whatever it needs to exist. 
        /// For example, if the repo is SQL and Entity Framework, it would create a table.
        /// It does not create the database.
        /// </summary>
        /// <returns>RepositoryGenerationResult</returns>
        Task<RepositoryGenerationResult> GenerateRepositoryAsync(bool forwardExceptions = true);

        /// <summary>
        /// This will check if an entity has seed data and if so, insert it into the repository.
        /// </summary>
        /// <returns>RepositorySeedResult</returns>
        Task<RepositorySeedResult> InsertSeedDataAsync(bool forwardExceptions = true);
    }
}