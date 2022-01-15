using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.WebServices
{
    class EntityWebServiceBuilder : IEntityWebServiceBuilder
    {
        private readonly IEndpointBuilder _EndpointBuilder;
        private readonly ILoadedEntitiesTracker _LoadedEntitiesTracker;

        public EntityWebServiceBuilder(IEndpointBuilder endpointBuilder,
                                       ILoadedEntitiesTracker loadedEntitiesTracker)
        {
            _EndpointBuilder = endpointBuilder;
            _LoadedEntitiesTracker = loadedEntitiesTracker;
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
                    continue;
                _EndpointBuilder.BuildEntityEndpoint(entityType);
            }
        }
    }
}