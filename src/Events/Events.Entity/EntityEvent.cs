using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityEvent : IEntityEventAfter<Entity, int>
        
    {
        private readonly IMetadataCache _MetadataCache;
        private readonly ILogger _Logger;

        public EntityEvent(IMetadataCache metadataCache, ILogger logger)
        {
            _MetadataCache = metadataCache ?? throw new System.ArgumentNullException(nameof(metadataCache));
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

        public void ClearEntityMetadata()
        {
            _Logger.Debug("Entity configuration data changed. Clearing metadata.");
            _MetadataCache.EntityMetadata.Clear();
        }
    }
}