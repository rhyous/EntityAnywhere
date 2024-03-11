using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    //[ExcludedServiceBehaviorTypes(ServiceBehaviorType.Authenticator)]
    [Route("Api/AuthorizationService")]
    public class AuthorizationController : ControllerBase, IAuthorizationController, ICustomWebService
    {
        private readonly IUserRoleEntityDataCache _UserRoleEntityDataCache;
        private readonly IUserDetails _UserDetails;

        public AuthorizationController(IUserRoleEntityDataCache userRoleEntityDataCache,
                                       IUserDetails userDetails)
        {
            _UserRoleEntityDataCache = userRoleEntityDataCache;
            _UserDetails = userDetails;
        }

        [AllowAnonymous]
        [HttpGet("MyRoleData")]
        public Task<IUserRoleEntityData> MyRoleDataAsync()
        {
            _UserRoleEntityDataCache.TryGetValue(_UserDetails.UserRole, out IUserRoleEntityData userRoleEntityData);
            if (userRoleEntityData == null)
                _UserRoleEntityDataCache.TryGetValue("Anonymous", out userRoleEntityData);
            return Task.FromResult(userRoleEntityData)!;
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