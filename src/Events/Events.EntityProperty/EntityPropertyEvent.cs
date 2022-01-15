using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Events
{
    /// <summary>
    /// An event plugin for the EntityProperty entity.
    /// </summary>
    public class EntityPropertyEvent : IEntityEventAfter<EntityProperty, int>
    {
        private readonly IMetadataCache _MetadataCache;
        private readonly ILogger _Logger;

        public EntityPropertyEvent(IMetadataCache metadataCache, ILogger logger)
        {
            _MetadataCache = metadataCache ?? throw new System.ArgumentNullException(nameof(metadataCache));
            _Logger = logger;
        }

        public void AfterDelete(EntityProperty entity, bool wasDeleted)
        {
            ClearEntityMetadata();
        }

        public void AfterDeleteMany(IEnumerable<EntityProperty> entities, Dictionary<int, bool> wasDeleted)
        {
            ClearEntityMetadata();
        }

        public void AfterPatch(PatchedEntityComparison<EntityProperty, int> patchedEntityComparison)
        {
            ClearEntityMetadata();
        }

        public void AfterPatchMany(IEnumerable<PatchedEntityComparison<EntityProperty, int>> patchedEntityComparison)
        {
            ClearEntityMetadata();
        }

        public void AfterPost(IEnumerable<EntityProperty> postedItems)
        {
            ClearEntityMetadata();
        }

        public void AfterPut(EntityProperty newEntity, EntityProperty priorEntity)
        {
            ClearEntityMetadata();
        }

        public void AfterUpdateProperty(string property, object newValue, object existingValue)
        {
            ClearEntityMetadata();
        }

        public void ClearEntityMetadata()
        {
            _Logger.Debug("EntityProperty configuration data changed. Clearing metadata.");
            _MetadataCache.EntityMetadata.Clear();
        }
    }
}
