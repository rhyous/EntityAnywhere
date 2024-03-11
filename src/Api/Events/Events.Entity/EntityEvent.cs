using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Events
{
    /// <summary>
    /// An event plugin for the Entity entity.
    /// </summary>
    public class EntityEvent : IEntityEventAfter<Entity, int>
        
    {
        private readonly IMetadataCache _MetadataCache;
        private readonly IEntitySettingsCache _EntitySettingsCache;
        private readonly ILogger _Logger;

        public EntityEvent(IMetadataCache metadataCache,
                           IEntitySettingsCache entitySettingsCache,
                           ILogger logger)
        {
            _MetadataCache = metadataCache ?? throw new System.ArgumentNullException(nameof(metadataCache));
            _EntitySettingsCache = entitySettingsCache;
            _Logger = logger;
        }


        public void AfterDelete(Entity entity, bool wasDeleted)
        {
            ClearEntityMetadata();
        }

        public void AfterDeleteMany(IEnumerable<Entity> entities, Dictionary<int, bool> wasDeleted)
        {
            ClearEntityMetadata();
        }

        public void AfterPatch(PatchedEntityComparison<Entity, int> patchedEntityComparison)
        {
            ClearEntityMetadata();
        }

        public void AfterPatchMany(IEnumerable<PatchedEntityComparison<Entity, int>> patchedEntityComparisons)
        {
            ClearEntityMetadata();
        }

        public void AfterPost(IEnumerable<Entity> postedItems)
        {
            ClearEntityMetadata();
        }

        public void AfterPut(Entity newEntity, Entity priorEntity)
        {
            ClearEntityMetadata();
        }

        public void AfterUpdateProperty(string property, object newValue, object existingValue)
        {
            ClearEntityMetadata();
        }

        internal void ClearEntityMetadata()
        {
            _Logger.Debug("Entity configuration data changed. Clearing metadata and entitysettings.");
            _MetadataCache.ProvideAsync(true);
            _EntitySettingsCache.ProvideAsync(true);
        }
    }
}