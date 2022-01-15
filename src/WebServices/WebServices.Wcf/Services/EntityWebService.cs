using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// A common entity web service for all entities. If no custom entity web service is provided, this one is used.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TService">The entity service type.</typeparam>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class EntityWebService<TEntity, TInterface, TId>
               : EntityWebServiceReadOnly<TEntity, TInterface, TId>, 
                 IEntityWebService<TEntity, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        protected readonly IRestHandlerProvider<TEntity, TInterface, TId> _RestHandlerProvider;

        public EntityWebService(IRestHandlerProvider<TEntity, TInterface, TId> restHandlerProvider) 
            : base (restHandlerProvider)
        {
            _RestHandlerProvider = restHandlerProvider;
        }

        /// <summary>
        /// Updates a single properties value
        /// </summary>
        /// <param name="id">The entity's Id.</param>
        /// <param name="property">The property name to update.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual async Task<string> UpdatePropertyAsync(string id, string property, string value) 
                  => await _RestHandlerProvider.UpdatePropertyHandler.Handle(id, property, value);

        /// <summary>
        /// Creates one or more (an array) of entities. 
        /// </summary>
        /// <param name="entities">The list of entities to create</param>
        /// <returns>The created entities.</returns>
        public virtual async Task<OdataObjectCollection<TEntity, TId>> PostAsync(List<TEntity> entities) 
                           => await _RestHandlerProvider.PostHandler.HandleAsync(entities);

        /// <summary>
        /// Creates one or more (an array) of entities. However, different than POST this immediately returns a 202.
        /// </summary>
        /// <param name="entities">The list of entities to create</param>
        /// <returns>The created entities.</returns>
        public virtual async Task PostOneWayAsync(List<TEntity> entities) => await PostAsync(entities);

        /// <summary>
        /// Updates one ore more properties of an entity.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="patchedEntity">A composite object including the current entity and changed properties.</param>
        /// <returns>The changed entity.</returns>
        public virtual async Task<OdataObject<TEntity, TId>> PatchAsync(string id, PatchedEntity<TEntity, TId> patchedEntity) 
                           => await _RestHandlerProvider.PatchHandler.HandleAsync(id, patchedEntity);

        /// <summary>
        /// Updates a list of entities
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="patchedEntity">A list of composite objects, each including the current entity and it's changed properties.</param>
        /// <returns>The changed entity.</returns>
        public virtual async Task<OdataObjectCollection<TEntity, TId>> PatchManyAsync(PatchedEntityCollection<TEntity, TId> patchedEntityCollection)
                           => await _RestHandlerProvider.PatchHandler.Handle(patchedEntityCollection);

        /// <summary>
        /// Replace the entity stored with the provided entity.
        /// </summary>
        /// <param name="id">The entity id to replace.</param>
        /// <param name="entity">The new entity.</param>
        /// <returns>The new entity.</returns>
        public virtual async Task<OdataObject<TEntity, TId>> PutAsync(string id, TEntity entity) 
                           => await _RestHandlerProvider.PutHandler.Handle(id, entity);

        /// <summary>
        /// Deletes the entity at the given id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>true if the entity could be deleted, false otherwise.</returns>
        public virtual bool Delete(string id) => _RestHandlerProvider.DeleteHandler.Handle(id);

        /// <summary>
        /// Deletes the entity at the given id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>true if the entity could be deleted, false otherwise.</returns>
        public virtual async Task<Dictionary<TId, bool>> DeleteManyAsync(IEnumerable<string> ids)
            => await _RestHandlerProvider.DeleteManyHandler.HandleAsync(ids);

        public virtual RepositoryGenerationResult GenerateRepository() => _RestHandlerProvider.GenerateRepositoryHandler.GenerateRepository();

        public virtual RepositorySeedResult InsertSeedData() => _RestHandlerProvider.InsertSeedDataHandler.InsertSeedData();

        /// <inheritdoc />
        public async Task<OdataObject<TEntity, TId>> PostExtensionAsync(string id, string extensionEntity, PropertyValue propertyValue)
            => await _RestHandlerProvider.PostExtensionHandler.HandleAsync(id, extensionEntity, propertyValue);

        /// <inheritdoc />
        public async Task<string> UpdateExtensionValueAsync(string id, string extensionEntity, PropertyValue propertyValue)
            => await _RestHandlerProvider.UpdateExtensionValueHandler.HandleAsync(id, extensionEntity, propertyValue);
    }
}