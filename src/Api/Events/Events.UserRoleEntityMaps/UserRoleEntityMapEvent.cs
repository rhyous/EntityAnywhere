using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;


namespace Rhyous.EntityAnywhere.Entities
{
    public class UserRoleEntityMapEvent : IEntityEventAfter<UserRoleEntityMap, long>
    {
        private readonly IUserRoleEntityDataCache _UserRoleEntityDataCache;
        private readonly ILogger _Logger;

        public UserRoleEntityMapEvent(IUserRoleEntityDataCache userRoleEntityDataCache, ILogger logger)
        {
            _UserRoleEntityDataCache = userRoleEntityDataCache ?? throw new System.ArgumentException(nameof(userRoleEntityDataCache));
            _Logger = logger;
        }

        public void AfterDelete(UserRoleEntityMap entity, bool wasDeleted)
        {
            ClearUserRoleEntityData(entity.UserRoleId, entity.EntityId);
        }

        public void AfterDeleteMany(IEnumerable<UserRoleEntityMap> entities, Dictionary<long, bool> wasDeleted)
        {
            foreach(UserRoleEntityMap entity in entities)
            {
                ClearUserRoleEntityData(entity.UserRoleId, entity.EntityId);
            }
        }

        public void AfterPatch(PatchedEntityComparison<UserRoleEntityMap, long> patchedEntityComparison)
        {
            UpdateUserRoleEntityData(patchedEntityComparison.Entity.UserRoleId);
        }

        public void AfterPatchMany(IEnumerable<PatchedEntityComparison<UserRoleEntityMap, long>> patchedEntityComparisons)
        {
            foreach (PatchedEntityComparison<UserRoleEntityMap, long> patchedEntityComparison in patchedEntityComparisons)
            {
                UpdateUserRoleEntityData(patchedEntityComparison.Entity.UserRoleId);
            }
        }

        public void AfterPost(IEnumerable<UserRoleEntityMap> postedItems)
        {
            foreach(UserRoleEntityMap entity in postedItems)
            {
                UpdateUserRoleEntityData(entity.UserRoleId);
            }
        }

        public void AfterPut(UserRoleEntityMap newEntity, UserRoleEntityMap priorEntity)
        {
            UpdateUserRoleEntityData(newEntity.UserRoleId);
        }

        public void AfterUpdateProperty(string property, object newValue, object existingValue)
        {
            
        }

        internal void ClearUserRoleEntityData(int roleId, int entityId)
        {
            _Logger.Debug("UserRole Entity Map configuration data changed. Updating UserRoleEntityData.");

            if (!_UserRoleEntityDataCache.TryGetValue(roleId, out IUserRoleEntityData userRoleEntityData))
                return;

            if (userRoleEntityData.Count == 1)
            {
                _UserRoleEntityDataCache.Remove(roleId);
            }
            else
            {
                userRoleEntityData.TryRemove(entityId);
            }
        }

        internal void UpdateUserRoleEntityData(int key)
        {
            _Logger.Debug("UserRole Entity Map configuration data changed. Updating UserRoleEntityData.");
            _UserRoleEntityDataCache.UpdateRoleEntityDataAsync(key);
        }
    }
}
