using Rhyous.EntityAnywhere.WebServices;

namespace Rhyous.EntityAnywhere.WebApi
{
    class EntityControllerListBuilder : IEntityControllerListBuilder
    {
        private readonly IEntityControllerBuilder _EntityControllerBuilder;
        private readonly ILoadedEntitiesTracker _LoadedEntitiesTracker;
        private readonly IEntityControllerList _EntityControllerCollection;

        public EntityControllerListBuilder(IEntityControllerBuilder entityControllerBuilder,
                                           ILoadedEntitiesTracker loadedEntitiesTracker,
                                           IEntityControllerList entityControllerCollection)
        {
            _EntityControllerBuilder = entityControllerBuilder;
            _LoadedEntitiesTracker = loadedEntitiesTracker;
            _EntityControllerCollection = entityControllerCollection;
        }

        /// <summary>
        /// Builds the endpoints for the provided types.
        /// </summary>
        /// <param name="entityTpes">The Entity types.</param>
        public void Build(IEnumerable<Type> entityTypes)
        {
            if (entityTypes is null) { throw new ArgumentNullException(nameof(entityTypes)); }
            foreach (var entityType in entityTypes)
            {
                if (_LoadedEntitiesTracker.Entities.Contains(entityType))
                    continue; // Usually only happens if the entity's controller was loaded custom
                var type = _EntityControllerBuilder.Build(entityType);
                _EntityControllerCollection.ControllerTypes.Add(type);
            }
        }
    }
}