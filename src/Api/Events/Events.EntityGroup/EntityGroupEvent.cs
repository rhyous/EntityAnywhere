using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityGroupEvent : IEntityEventAfter<EntityGroup, int>
    {
        private readonly IMetadataCache _MetadataCache;
        private readonly IEntitySettingsCache _EntitySettingsCache;
        private readonly IEntityGroupCache _EntityGroupCache;
        private readonly ILogger _Logger;

        public EntityGroupEvent(IMetadataCache metadataCache,
                                IEntitySettingsCache entitySettingsCache,
                                IEntityGroupCache entityGroupCache,
                                ILogger logger)
        {
            _MetadataCache = metadataCache;
            _EntitySettingsCache = entitySettingsCache;
            _EntityGroupCache = entityGroupCache;
            _Logger = logger;
        }

        public void AfterDelete(EntityGroup entity, bool wasDeleted)
        {
            ClearEntityMetadata();
        }

        public void AfterDeleteMany(IEnumerable<EntityGroup> entities, Dictionary<int, bool> wasDeleted)
        {
            ClearEntityMetadata();
        }

        public void AfterPatch(PatchedEntityComparison<EntityGroup, int> patchedEntityComparison)
        {
            ClearEntityMetadata();
        }

        public void AfterPatchMany(IEnumerable<PatchedEntityComparison<EntityGroup, int>> patchedEntityComparisons)
        {
            ClearEntityMetadata();
        }

        public void AfterPost(IEnumerable<EntityGroup> postedItems)
        {
            ClearEntityMetadata();
        }

        public void AfterPut(EntityGroup newEntity, EntityGroup priorEntity)
        {
            ClearEntityMetadata();
        }

        public void AfterUpdateProperty(string property, object newValue, object existingValue)
        {
            ClearEntityMetadata();
        }

        public void ClearEntityMetadata()
        {
            _Logger.Debug("EntityGroup configuration data changed. Clearing metadata.");
            _MetadataCache.ProvideAsync(true);
            _EntitySettingsCache.ProvideAsync(true);
            _EntityGroupCache.ProvideAsync(true);
        }
    }
}