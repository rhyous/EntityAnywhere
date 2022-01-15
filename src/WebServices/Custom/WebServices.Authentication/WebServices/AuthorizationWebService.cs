using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    [ExcludedServiceBehaviorTypes(ServiceBehaviorType.Authenticator)]
    [CustomWebService("AuthorizationWebService", typeof(IAuthorizationWebService), null, "AuthorizationService.svc")]
    public class AuthorizationWebService : IAuthorizationWebService, ICustomWebService
    {
        private readonly IUserRoleEntityDataCache _UserRoleEntityDataCache;
        private readonly IUserDetails _UserDetails;

        public AuthorizationWebService(IUserRoleEntityDataCache userRoleEntityDataCache,
                                       IUserDetails userDetails)
        {
            _UserRoleEntityDataCache = userRoleEntityDataCache;
            _UserDetails = userDetails;
        }
        public async Task<UserRoleEntityData> MyRoleDataAsync()
        {
            if (!_UserRoleEntityDataCache.TryGetValue(_UserDetails.UserRole, out IUserRoleEntityData userRoleEntityData))
                return null;
            return userRoleEntityData as UserRoleEntityData;
        }

        #region IDisposable

        internal bool _IsDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_IsDisposed)
            {
                _IsDisposed = true;
                if (disposing)
                {
                }
                // Dispose unmanaged resources
            }
        }

        #endregion
    }
}