using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Security;
using Rhyous.EntityAnywhere.WebServices;
using Rhyous.Odata;

namespace Rhyous.EntityAnywhere.WebApi
{
    /// <summary>
    /// This allows for creating a generic entity controller.
    /// All generic entity controllers will use this path:
    ///   {host}/Api/{entity}Service/{entityPluralize}
    /// Example for a User entity:
    ///   {host}/Api/UserService/Users
    /// Then the HTTP verb (GET, POST, DELETE, PUT, PATCH) determines which method will be hit
    /// </summary>
    /// <typeparam name="TEntity">The Entity Type</typeparam>
    /// <typeparam name="TInterface">The Entity Interface Type</typeparam>
    /// <typeparam name="TId">The entity</typeparam>
    [ApiController]
    [Route("api/[controller]")]
    [EntityGenericController]
    [Authorize(AuthenticationSchemes = TokenAuthenticationSchemeOptions.Name)]
    public class EntityController<TEntity, TInterface, TId> : EntityControllerReadOnly<TEntity, TInterface, TId>,
        IEntityWebService<TEntity, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        /// <summary>The cosntructor</summary>
        /// <param name="restHandlerProvider">A provider to provide the handler the rest call.</param>
        public EntityController(IRestHandlerProvider restHandlerProvider)
            : base(restHandlerProvider)
        {
        }

        #region Deletes
        /// <summary>
        /// Deletes the entity at the given id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>true if the entity could be deleted, false otherwise.</returns>
        [HttpDelete("[action]({id})")]
        [EntityActionName("{EntityPluralized}")]
        public virtual bool Delete(string id)
            => _RestHandlerProvider.Provide<IDeleteHandler<TEntity, TInterface, TId>>()
                                   .Handle(id);

        /// <summary>
        /// Deletes all instances of an extension entity for a given entity id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="extensionEntity">The entities to delete.</param>
        /// <returns>A dictionary with a key of the extension entity id adn the value of true or false,
        /// indicating which extension entities were deleted.</returns>
        [HttpDelete("[action]({id})/{extensionEntity}/All")]
        [EntityActionName("{EntityPluralized}")]
        public async Task<Dictionary<long, bool>> DeleteAllExtensionsAsync(string id, string extensionEntity)
            => await _RestHandlerProvider.Provide<IDeleteExtensionHandler<TEntity, TInterface, TId>>()
                                         .HandleAsync(id, extensionEntity);

        /// <summary>
        /// Deletes all instances of an extension entity matching the provided ids for a given entity id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="extensionEntity">The entities to delete.</param>
        /// <param name="ids">The ids of the extension entities to delete.</param>
        /// <returns>A dictionary with a key of the extension entity id adn the value of true or false,
        /// indicating which extension entities were deleted.</returns>
        [HttpDelete("[action]({id})/{extensionEntity}")]
        [EntityActionName("{EntityPluralized}")]
        public async Task<Dictionary<long, bool>> DeleteExtensionsAsync(string id, string extensionEntity, IEnumerable<string> ids)
            => await _RestHandlerProvider.Provide<IDeleteExtensionHandler<TEntity, TInterface, TId>>()
                                         .HandleAsync(id, extensionEntity, ids);

        /// <summary>
        /// Deletes the entity at the given id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>true if the entity could be deleted, false otherwise.</returns>
        [HttpDelete("[action]/Bulk")]
        [EntityActionName("{EntityPluralized}")]
        public virtual async Task<Dictionary<TId, bool>> DeleteManyAsync(IEnumerable<string> ids)
            => await _RestHandlerProvider.Provide<IDeleteManyHandler<TEntity, TInterface, TId>>()
                                         .HandleAsync(ids);

        #endregion

        #region Setup and Configuration
        /// <summary>Should generate the repository.</summary>
        /// <returns>A RepositoryGenerationResult indicating success or failure.</returns>
        [HttpGet("$Generate")]
        public virtual RepositoryGenerationResult GenerateRepository()
                    => _RestHandlerProvider.Provide<IGenerateRepositoryHandler<TEntity, TInterface, TId>>()
                                           .GenerateRepository();

        /// <summary>Should insert seed data the repository.</summary>
        /// <returns>A RepositorySeedResult indicating success or failure.</returns>
        [HttpGet("$Seed")]
        public virtual RepositorySeedResult InsertSeedData()
            => _RestHandlerProvider.Provide<IInsertSeedDataHandler<TEntity, TInterface, TId>>()
                                   .InsertSeedData();
        #endregion

        #region Patch
        /// <summary>
        /// Updates one ore more properties of an entity.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="patchedEntity">A composite object including the current entity and changed properties.</param>
        /// <returns>The changed entity.</returns>
        [HttpPatch("[action]({id})")]
        [EntityActionName("{EntityPluralized}")]
        public virtual async Task<OdataObject<TEntity, TId>> PatchAsync(string id, PatchedEntity<TEntity, TId> patchedEntity)
            => await _RestHandlerProvider.Provide<IPatchHandler<TEntity, TInterface, TId>>()
                                         .HandleAsync(id, patchedEntity);

        /// <summary>
        /// Updates a list of entities
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="patchedEntity">A list of composite objects, each including the current entity and it's changed properties.</param>
        /// <returns>The changed entity.</returns>
        [HttpPatch("[action]")]
        [EntityActionName("{EntityPluralized}")]
        public virtual async Task<OdataObjectCollection<TEntity, TId>> PatchManyAsync(PatchedEntityCollection<TEntity, TId> patchedEntityCollection)
            => await _RestHandlerProvider.Provide<IPatchHandler<TEntity, TInterface, TId>>()
                                         .Handle(patchedEntityCollection);

        #endregion

        #region Post

        /// <summary>
        /// Creates one or more (an array) of entities. 
        /// </summary>
        /// <param name="entities">The list of entities to create</param>
        /// <returns>The created entities.</returns>
        [HttpPost("[action]")]
        [EntityActionName("{EntityPluralized}")]
        public virtual async Task<OdataObjectCollection<TEntity, TId>> PostAsync(List<TEntity> entities)
                           => await _RestHandlerProvider.Provide<IPostHandler<TEntity, TInterface, TId>>()
                                                        .HandleAsync(entities);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="extensionEntity"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        [HttpPost("[action]({id})/Extension/{extensionEntity}")]
        [EntityActionName("{EntityPluralized}")]
        public async Task<OdataObject<TEntity, TId>> PostExtensionAsync(string id, string extensionEntity, PropertyValue propertyValue)
            => await _RestHandlerProvider.Provide<IPostExtensionHandler<TEntity, TInterface, TId>>()
                                         .HandleAsync(id, extensionEntity, propertyValue);

        #endregion

        #region Put
        /// <summary>
        /// Replace the entity stored with the provided entity.
        /// </summary>
        /// <param name="id">The entity id to replace.</param>
        /// <param name="entity">The new entity.</param>
        /// <returns>The new entity.</returns>
        [HttpPut("[action]({id})")]
        [EntityActionName("{EntityPluralized}")]
        public virtual async Task<OdataObject<TEntity, TId>> PutAsync(string id, TEntity entity)
            => await _RestHandlerProvider.Provide<IPutHandler<TEntity, TInterface, TId>>()
                                         .Handle(id, entity);

        /// <summary>Updates the value of an existing extension entity, found by the entity id and property.</summary>
        /// <param name="id"></param>
        /// <param name="extensionEntity"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        [HttpPut("[action]({id})/Extension/{extensionEntity}")]
        [EntityActionName("{EntityPluralized}")]
        public async Task<string> UpdateExtensionValueAsync(string id, string extensionEntity, PropertyValue propertyValue)
            => await _RestHandlerProvider.Provide<IUpdateExtensionValueHandler<TEntity, TInterface, TId>>()
                                         .HandleAsync(id, extensionEntity, propertyValue);

        /// <summary>Updates the value of an existing extension entity, found by the entity id and property.</summary>
        /// <param name="id">The entity's Id.</param>
        /// <param name="property">The property name to update.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [HttpPut("[action]({id})/{property}")]
        [EntityActionName("{EntityPluralized}")]
        public virtual async Task<string> UpdatePropertyAsync(string id, string property, [FromBody]string value)
                  => await _RestHandlerProvider.Provide<IUpdatePropertyHandler<TEntity, TInterface, TId>>()
                                               .Handle(id, property, value);

        /// <summary>
        /// Updates a single properties value
        /// </summary>
        /// <param name="id">The entity's Id.</param>
        /// <param name="property">The property name to update.</param>
        /// <param name="value">The value as a stream.</param>
        /// <returns>True if updated, false otherwise.</returns>
        [HttpPut("[action]({id}){property}/stream")]
        [EntityActionName("{EntityPluralized}")]
        public virtual async Task<bool> UpdatePropertyStreamAsync(string id, string property, Stream value)
                  => await _RestHandlerProvider.Provide<IUpdatePropertyStreamHandler<TEntity, TInterface, TId>>()
                                               .HandleAsync(id, property, value);
        #endregion
    }
}