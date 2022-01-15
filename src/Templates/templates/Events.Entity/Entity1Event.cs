using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Entity1Event : IEntityEventAll<Entity1, long>
    {
        private readonly ILogger _Logger;

        public Entity1Event(ILogger logger)
        {
            _Logger = logger;
        }

        #region Before
        public void BeforeDelete(Entity1 entity)
        {
        }

        public void BeforeDeleteMany(IEnumerable<Entity1> entities)
        {
        }

        public void BeforePatch(PatchedEntityComparison<Entity1, long> patchedEntityComparison)
        {
        }

        public void BeforePatchMany(IEnumerable<PatchedEntityComparison<Entity1, long>> patchedEntityComparisons)
        {
        }

        public void BeforePost(IEnumerable<Entity1> postedItems)
        {
        }

        public void BeforePut(Entity1 newEntity, Entity1 existingEntity)
        {
        }

        public void BeforeUpdateProperty(string property, object newValue, object existingValue)
        {
        }
        #endregion

        #region After
        public void AfterDelete(Entity1 Entity1, bool wasDeleted)
        {
        }

        public void AfterDeleteMany(IEnumerable<Entity1> entities, Dictionary<long, bool> wasDeleted)
        {

        }

        public void AfterPatch(PatchedEntityComparison<Entity1, long> patchedEntityComparison)
        {
        }

        public void AfterPatchMany(IEnumerable<PatchedEntityComparison<Entity1, long>> patchedEntityComparisons)
        {
        }

        public void AfterPost(IEnumerable<Entity1> postedItems)
        {
        }

        public void AfterPut(Entity1 newEntity, Entity1 priorEntity)
        {
        }

        public void AfterUpdateProperty(string property, object newValue, object existingValue)
        {
        }
        #endregion
    }
}