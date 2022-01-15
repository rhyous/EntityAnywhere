using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    public class UserRoleProvider : IUserRoleProvider
    {
        internal const string DefaultUserRole = "Customer";
        internal const string DefaultUserRoleSetting = "DefaultUserRole";

        private readonly IUserRoleEntityDataCache _UserRoleEntityDataCache;
        private readonly IAppSettings _AppSettings;

        public UserRoleProvider(IUserRoleEntityDataCache userRoleEntityDataCache,
                                IAppSettings appSettings)
        {
            _UserRoleEntityDataCache = userRoleEntityDataCache;
            _AppSettings = appSettings;
        }

        public int GetDefaultRoleId()
        {
            var roleName = _AppSettings.Collection.Get(DefaultUserRoleSetting, DefaultUserRole);
            var role = _UserRoleEntityDataCache.UserRoleIds.TryGetValue(roleName, out int roleId)
                     ? roleId
                     : -1;
            return (role > 0) ? role: throw new Exception($"The role {roleName} could not be found.");
        }
    }
}