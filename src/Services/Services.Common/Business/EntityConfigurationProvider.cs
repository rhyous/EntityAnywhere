using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// Responsible for retrieving an Entity's configuration via the <see cref="IAdminEntityClientAsync{,}"/> 
    /// <see cref="Entity"/> and <see cref="IAdminEntityClientAsync{,}"/> <see cref="EntityProperty"/>
    /// </summary>
    public class EntityConfigurationProvider : IEntityConfigurationProvider
    {
        private readonly IAdminEntityClientAsync<Entity, int> EntityClientAsync;
        private readonly IAdminEntityClientAsync<EntityProperty, int> EntityPropertyClientAsync;

        public EntityConfigurationProvider(
            IAdminEntityClientAsync<Entity,int> entityClientAsync,
            IAdminEntityClientAsync<EntityProperty, int> entityPropertyClientAsync
        )
        {
            EntityClientAsync = entityClientAsync;
            EntityPropertyClientAsync = entityPropertyClientAsync;
        }

        /// <summary>
        /// Asynchronously retrieve the entity configuration using the Entity EntityClient and the EntityProperty EntityClient
        /// </summary>
        /// <param name="entityType">The entity type to use</param>
        /// <returns>A <see cref="Task{TResult}"/> of <see cref="IEntityConfiguration"/></returns>
        public async Task<IEntityConfiguration> ProvideAsync(Type entityType)
        {
            if (entityType.Name == nameof(Entity) || entityType.Name == nameof(EntityProperty))
                return new EntityConfiguration();

            var entityResult = await EntityClientAsync.GetByAlternateKeyAsync(entityType.Name);
            if (entityResult == null)
                return new EntityConfiguration();

            var propertyId = entityResult.Object.SortByPropertyId;
            var entityPropertyResult = propertyId.HasValue && propertyId.Value != default
                ? await EntityPropertyClientAsync.GetAsync(entityResult.Object.SortByPropertyId.Value, false)
                : null;

            // Adapt the result
            return new EntityConfiguration()
            {
                DefaultSortByProperty = entityPropertyResult?.Object?.Name ?? "Id",
                DefaultSortOrder = entityResult.Object.SortOrder
            };
        }

        /// <summary>
        /// Synchronously retrieve the entity configuration
        /// </summary>
        /// <param name="entityType">The entity type to use</param>
        /// <returns>An <see cref="IEntityConfiguration"/></returns>
        public IEntityConfiguration Provide(Type entityType)
        {
            return TaskRunner.RunSynchonously(() => ProvideAsync(entityType));
        }

    }
}
