using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    /// <summary>
    /// A common class that any client can implement to talk to any entity's web services asynchronously.
    /// It inherits from IEntityWebService so all the same methods that exist on the common webservice are available to all client implementations.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TId">The entity id type</typeparam>
    public interface IEntityClientAsync<TEntity, TId> : IEntityClientBase
        where TEntity : class, IId<TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        /// <summary>
        /// Gets the metadata about the entity. Call is asynchonous.
        /// </summary>
        /// <returns>The metadata about the entity.</returns>
        Task<CsdlEntity> GetMetadataAsync(bool forwardExceptions = true);

        /// <summary>
        /// Gets all entities. Call is asynchonous.
        /// </summary>
        /// <returns>All Entities</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetAllAsync(bool forwardExceptions = true);

        /// <summary>
        /// This provides an additional option to make a get call with query parameters. Call is asynchonous.
        /// </summary>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>A list of entities that match the query parameters.</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetByQueryParametersAsync(string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets all entities with the provided ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of entity ids.</param>
        /// <returns>All entities with the provided ids.</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetByIdsAsync(IEnumerable<TId> ids, bool forwardExceptions = true);

        /// <summary>
        /// Gets all entities with the provided ids. Call is asynchonous.
        /// </summary>
        /// <param name="ids">A list of entity ids.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>All entities with the provided ids.</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetByIdsAsync(IEnumerable<TId> ids, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets all entities with the provided values of a given property.
        /// </summary>
        /// <param name="values">A list of values.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>All entities with the provided values of a given property.</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetByPropertyValuesAsync(string property, IEnumerable<string> values, bool forwardExceptions = true);

        /// <summary>
        /// Gets all entities with the provided values of a given property.
        /// </summary>
        /// <param name="values">A list of values.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>All entities with the provided values of a given property.</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetByPropertyValuesAsync(string property, IEnumerable<string> values, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by a specific id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity as a string.</param>
        /// <returns>The entity with the specified id.</returns>
        Task<OdataObject<TEntity, TId>> GetAsync(string id, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by a specific id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity as the correct type.</param>
        /// <returns>The entity with the specified id.</returns>
        Task<OdataObject<TEntity, TId>> GetAsync(TId id, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by a specific id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity as the correct type.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>The entity with the specified id.</returns>
        Task<OdataObject<TEntity, TId>> GetAsync(TId id, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by a specific id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>The entity with the specified id.</returns>
        Task<OdataObject<TEntity, TId>> GetAsync(string id, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by the AlternateKey. This is only a valid call for Entities
        /// with the AlternateKey attribute. Call is asynchonous.
        /// </summary>
        /// <param name="altKey"></param>
        /// <returns>The entity with the specified alternate key.</returns>
        Task<OdataObject<TEntity, TId>> GetByAlternateKeyAsync(string altKey, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by the AlternateKey. This is only a valid call for Entities
        /// with the AlternateKey attribute. Call is asynchonous.
        /// </summary>
        /// <param name="altKey"></param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>The entity with the specified alternate key.</returns>
        Task<OdataObject<TEntity, TId>> GetByAlternateKeyAsync(string altKey, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by an Alternate Id. Call is asynchonous.
        /// </summary>
        /// <param name="altId">The alternate Id</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>The entity with the specified alternate key.</returns>
        Task<OdataObject<TEntity, TId>> GetByAlternateIdAsync(string altId, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity by an Alternate Id. Call is asynchonous.
        /// </summary>
        /// <param name="altId">The alternate Id</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>The entity with the specified alternate key.</returns>
        Task<OdataObject<TEntity, TId>> GetByAlternateIdAsync(string altId, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Gets an entity's property value by a specific id and property name. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="property">The property name.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>The value of the specific property of the specific entity.</returns>
        Task<string> GetPropertyAsync(string id, string property, bool forwardExceptions = true);

        /// <summary>
        /// Updates a property on the entity with the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="property">The name of the property to update.</param>
        /// <param name="value">The new value.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>The actual value after it is changed.</returns>
        Task<string> UpdatePropertyAsync(string id, string property, string value, bool forwardExceptions = true);

        /// <summary>
        /// Adds an entity. Call is asynchonous.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>The added entity.</returns>
        Task<OdataObjectCollection<TEntity, TId>> PostAsync(IEnumerable<TEntity> entity, bool forwardExceptions = true);

        /// <summary>
        /// Adds an entity. Call is asynchonous.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>The added entity.</returns>
        Task<OdataObjectCollection<TEntity, TId>> PostAsync(IEnumerable<TEntity> entity, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Replaces an entity at the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The replaced entity.</returns>
        Task<OdataObject<TEntity, TId>> PutAsync(string id, TEntity entity, bool forwardExceptions = true);

        /// <summary>
        /// Replaces an entity at the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="entity">The entity to add.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>The replaced entity.</returns>
        Task<OdataObject<TEntity, TId>> PutAsync(string id, TEntity entity, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Used to update multiple properties of an existing entity. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="patchedEntity">An object that contains a stub entity instance with the only properties set being the ones that will change. It also has a list of changed properties.</param>
        /// <param name="forwardExceptions">If the call fails do you want to forward the exceptions? If not, an empty response is provided.</param>
        /// <returns>The patched entity, fetched completely after update.</returns>
        Task<OdataObject<TEntity, TId>> PatchAsync(string id, PatchedEntity<TEntity, TId> patchedEntity, bool forwardExceptions = true);

        /// <summary>
        /// Used to update multiple properties of an existing entity. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="patchedEntity">An object that contains a stub entity instance with the only properties set being the ones that will change. It also has a list of changed properties.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">If the call fails do you want to forward the exceptions? If not, an empty response is provided.</param>
        /// <returns>The patched entity, fetched completely after update.</returns>
        Task<OdataObject<TEntity, TId>> PatchAsync(string id, PatchedEntity<TEntity, TId> patchedEntity, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Used to update multiple properties of multiple existing entities. Call is asynchonous.
        /// </summary>
        /// <param name="patchedEntityCollection">An collection of objects that each contains a stub entity instance with the only properties set being the ones that will change. It also has a list of changed properties.</param>
        /// <param name="forwardExceptions">If the call fails do you want to forward the exceptions? If not, an empty response is provided.</param>
        /// <returns></returns>
        Task<OdataObject<TEntity, TId>> PatchManyAsync(PatchedEntityCollection<TEntity, TId> patchedEntityCollection, bool forwardExceptions = true);

        /// <summary>
        /// Used to update multiple properties of multiple existing entities. Call is asynchonous.
        /// </summary>
        /// <param name="patchedEntityCollection">An collection of objects that each contains a stub entity instance with the only properties set being the ones that will change. It also has a list of changed properties.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">If the call fails do you want to forward the exceptions? If not, an empty response is provided.</param>
        /// <returns></returns>
        Task<OdataObject<TEntity, TId>> PatchManyAsync(PatchedEntityCollection<TEntity, TId> patchedEntityCollection, string urlParameters, bool forwardExceptions = true);

        /// <summary>
        /// Deletes the entity by the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="forwardExceptions">If the call fails do you want to forward the exceptions? If not, an empty response is provided.</param>
        /// <returns>True if deleted or if already deleted. False otherwise.</returns>
        Task<bool> DeleteAsync(TId id, bool forwardExceptions = true);

        /// <summary>
        /// Deletes the entity by the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="forwardExceptions">If the call fails do you want to forward the exceptions? If not, an empty response is provided.</param>
        /// <returns>True if deleted or if already deleted. False otherwise.</returns>
        Task<bool> DeleteAsync(string id, bool forwardExceptions = true);

        /// <summary>
        /// Deletes the entity by the specified id. Call is asynchonous.
        /// </summary>
        /// <param name="ids">The ids of the entities to delete.</param>
        /// <param name="forwardExceptions">If the call fails do you want to forward the exceptions? If not, an empty response is provided.</param>
        /// <returns>True if deleted or if already deleted. False otherwise.</returns>
        Task<Dictionary<TId, bool>> DeleteManyAsync(IEnumerable<TId> ids, bool forwardExceptions = true);

        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hsotname/path/EntityService/.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>A list of entities returned by the custom service.</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetByCustomUrlAsync(string url, bool forwardExceptions = true);

        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <remarks>This overload specifically is used for HttpClient actions other than GET, such as POST, that has content.</remarks>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hostname/path/EntityService/.</param>
        /// <param name="httpMethod">The HttpMethod, such as GET, POST, PATCH, PUT, DELETE, etc.</param>
        /// <param name="content">The content in object form, it will be converted to JSON.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message (true) or to be treated as an empty result set (false).</param>
        /// <returns>A list of entities returned by the custom service.</returns>
        Task<OdataObjectCollection<TEntity, TId>> CallByCustomUrlAsync(string urlPart, HttpMethod httpMethod, object content, bool forwardExceptions = true);

        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hostname/path/EntityService/.</param>
        /// <param name="httpMethod">The HttpMethod, such as GET, POST, PATCH, PUT, DELETE, etc.</param>
        /// <param name="content">The content in object form, it will be converted to JSON.</param>
        /// <returns>A single entity returned by the custom service. The service must wrap it in an OdataObject.</returns>
        Task<OdataObject<TEntity, TId>> GetObjectByCustomUrlAsync(string urlPart, HttpMethod httpMethod, object content, bool forwardExceptions = true);

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
        /// <returns>The entity with the posted extensions as relations.</returns>
        Task<OdataObject<TEntity, TId>> PostExtensionAsync(string id, string extensionEntity, PropertyValue propertyValue, bool forwardExceptions = true);

        /// <summary>
        /// The ability to add an extension entity (i.e. property value pair) to any entity
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="extensionEntity">The singular name of the extension entity. Usually Addendum or AlternateId.</param>
        /// <param name="content">The content as an httpContent object.</param>
        /// <param name="urlParameters">A string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns>The entity with the posted extensions as relations.</returns>
        Task<OdataObject<TEntity, TId>> PostExtensionAsync(string id, string extensionEntity, PropertyValue propertyValue, string urlParameters, bool forwardExceptions = true);
        
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

        /// <summary>
        /// The ability to delete an extension entity to a given entity using this entities endpoints instead of the extension entity's endpoints.
        /// </summary>
        /// <param name="id">The id of this entity.</param>
        /// <param name="extensionEntity">The name of the extension entity, usually "AlternateId" or "Addendum". If "All" then delete all extension entities </param>
        /// <returns>True if deleted or if already deleted. False otherwise.</returns>
        Task<Dictionary<TId, bool>> DeleteAllExtensionsAsync(string id, string extensionEntity, bool forwardExceptions = true);

        /// <summary>
        /// The ability to delete an extension entity to a given entity using this entities endpoints instead of the extension entity's endpoints.
        /// </summary>
        /// <param name="id">The id of this entity.</param>
        /// <param name="extensionEntity">The name of the extension entity, usually "AlternateId" or "Addendum".</param>
        /// <param name="ids">The ids of the extension entities to delete.</param>
        /// <returns>True if deleted or if already deleted. False otherwise.</returns>
        Task<Dictionary<TId, bool>> DeleteExtensionsAsync(string id, string extensionEntity, IEnumerable<string> extensionEntityIds, bool forwardExceptions = true);

        /// <summary>
        /// Takes in a Entity and gets all the distinct values for the given property.
        /// </summary>
        /// <param name="propertyName">The name of the Property.</param>
        /// <returns>all the distinct values for the given property.</returns>
        Task<List<string>> GetDistinctPropertyValuesAsync(string propertyName, bool forwardExceptions = true);
    }
}
