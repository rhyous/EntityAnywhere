using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    /// <summary>
    /// This class checks that a user (other than Admin) has permissions to an entity.
    /// </summary>
    class EntityPermissionChecker : IEntityPermissionChecker
    {
        private readonly IUserRoleEntityDataCache _UserRoleEntityDataCache;

        /// <summary>
        /// The contructor
        /// </summary>
        /// <param name="userRoleEntityDataCache"></param>
        /// <remarks>Injecting IUserRoleEntityDataCache doesn't create an infinity loop here because UserRoleEntityDataCacheFactory uses the EntityAdminToken, so this
        /// code isn't reached when UserRoleEntityDataCacheFactory calls the UserRole web service.</remarks>
        public EntityPermissionChecker(IUserRoleEntityDataCache userRoleEntityDataCache)
        {
            _UserRoleEntityDataCache = userRoleEntityDataCache;
        }
        public bool HasPermission(string role, string entity)
        {
            return (!string.IsNullOrWhiteSpace(role) && !string.IsNullOrWhiteSpace(entity))
                && _UserRoleEntityDataCache.UserRoleIds.TryGetValue(role, out int roleId)
                && HasPermission(roleId, entity);
        }

        public bool HasPermission(int roleId, string entity)
        {
            if (roleId < 1 || string.IsNullOrWhiteSpace(entity))
                return false;
            if (roleId == WellknownUserRoleIds.Admin)
                return true;
            if (!_UserRoleEntityDataCache.TryGetValue(roleId, out IUserRoleEntityData userRoleEntityData)
             || userRoleEntityData == null
             || !userRoleEntityData.TryGetValue(entity, out EntityPermission entityPermission)
             || entityPermission == null
             || entityPermission.Permissions == null)
                return false;
            return entityPermission.Permissions.Contains(Permissions.Admin);
            // Later we will support other permissions besides admin
        }
    }
}
