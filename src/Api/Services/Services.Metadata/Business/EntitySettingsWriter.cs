using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// This class writes default data to Entity, EntityGroup, and EntityProperty. 
    /// The data is based on the Entity plugin.
    /// </summary>
    /// <remarks>When a new Entity is created, zero manually work should be required
    /// beyond the creation of the entity. So this needs to happen automatically.</remarks>
    public class EntitySettingsWriter : IEntitySettingsWriter
    {
        private readonly IAdminEntityClientAsync<Entity, int> _EntityClient;
        private readonly IAdminEntityClientAsync<EntityProperty, int> _EntityPropertyClient;
        private readonly IAdminEntityClientAsync<EntityGroup, int> _EntityGroupClient;
        private readonly ILogger _Logger;

        public EntitySettingsWriter(IAdminEntityClientAsync<Entity, int> entityClient,
                                    IAdminEntityClientAsync<EntityProperty, int> entityPropertyClient,
                                    IAdminEntityClientAsync<EntityGroup, int> entityGroupClient,
                                    ILogger logger)
        {
            _EntityClient = entityClient;
            _EntityPropertyClient = entityPropertyClient;
            _EntityGroupClient = entityGroupClient;
            _Logger = logger;
        }

        public async Task Write(MissingEntitySettings settings)
        {
            await WriteGroups(settings);
            await WriteEntities(settings);
            await WriteEntityProperties(settings);
        }

        #region Group
        internal async Task WriteGroups(MissingEntitySettings settings)
        {
            if (settings.EntityGroups != null && settings.EntityGroups.Any())
            {   // Add any groups that are missing
                var groups = settings.EntityGroups.Values.Where(e => e.IsMissing).Select(m => m.Object);
                await Write(settings, groups);
                // Update any entities to know the GroupId
                UpdateEntityGroupIds(settings);
            }
        }

        internal async Task Write(MissingEntitySettings settings, IEnumerable<EntityGroup> entityGroups)
        {
            var posted = await _EntityGroupClient.PostAsync(entityGroups);
            foreach (var odataGroup in posted)
                settings.EntityGroups[odataGroup.Object.Name] = odataGroup.Object;
        }

        internal void UpdateEntityGroupIds(MissingEntitySettings settings)
        {
            var entities = settings.Entities.Values.Where(e => e.IsMissing);
            foreach (var entity in entities)
            {
                if (settings.EntityGroups.TryGetValue(entity.Object.EntityGroup.Name, out Missing<EntityGroup> mGroup))
                    entity.Object.Entity.EntityGroupId = mGroup.Object.Id;
            }
        }
        #endregion

        #region Entity
        internal async Task WriteEntities(MissingEntitySettings settings)
        {
            var missingEntities = settings.Entities?.Values
                                          .Where(e => e.IsMissing)
                                          .Select(m => m.Object);
            if (missingEntities != null && missingEntities.Any())
            {   // Add any Entities that are missing
                await Write(settings, missingEntities.Select(e=>e.Entity));
                // Update any EntityProperties to know the EntityId.
                UpdateEntityProperties(settings);
            }
        }

        internal async Task Write(MissingEntitySettings settings, IEnumerable<Entity> entities)
        {
            var postedEntities = (await _EntityClient.PostAsync(entities))?.Select(o => o.Object);
            foreach (var entity in postedEntities)
                settings.Entities[entity.Name].Object.Entity = entity;
        }

        internal void UpdateEntityProperties(MissingEntitySettings settings)
        {
            foreach (var entity in settings.Entities.Values)
            {
                var entityProps = entity.Object.EntityProperties.Values.Select(m => m.Object);
                foreach (var prop in entityProps)
                    prop.EntityId = entity.Object.Entity.Id;
            }
        }
        #endregion

        #region
        internal async Task WriteEntityProperties(MissingEntitySettings settings)
        {
            var missingProps = settings.Entities.SelectMany(e => e.Value.Object.EntityProperties.Values
                             .Where(p => p.IsMissing)
                             .Select(m => m.Object));
            if (missingProps != null && missingProps.Any())
            // Add any EntityProperties that are missing
            {
                var entityProps = missingProps.ToConcrete<EntityProperty, IEntityProperty>();
                await Write(entityProps);
            }
        }

        internal async Task Write(IEnumerable<EntityProperty> entityProps)
        {
            (await _EntityPropertyClient.PostAsync(entityProps))?
                                        .Select(o => o.Object);
        }
        #endregion
    }
}