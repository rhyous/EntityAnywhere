using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// A custom web service, not related to an entity, for authentication. This web service forwards to the Authentication Service which loads the Authenticator plugins
    /// and attemps to authenticate to each of them.
    /// </summary>
    //[ExcludedServiceBehaviorTypes(ServiceBehaviorType.Authenticator)]
    [Route("Api/AuthenticationService")]
    public class AuthenticationController : ControllerBase, IAuthenticationController, ICustomWebService
    {
        private readonly IAuthenticationService _Service;
        private readonly IRequestSourceIpAddress _RequestSourceIpAddress;

        public AuthenticationController(IAuthenticationService service, IRequestSourceIpAddress requestSourceIpAddress)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _RequestSourceIpAddress = requestSourceIpAddress;
        }

        /// <inheritdoc />
        [HttpPost("Authenticate")]
        public async Task<Token> AuthenticateAsync([FromBody]Credentials creds)
        {            
            return (await _Service.AuthenticateAsync(creds, _RequestSourceIpAddress.IpAddress)).ToConcrete<Token>();
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
                    _Service.Dispose();
                }
                // Dispose unmanaged resources
            }
        }

        #endregion
    }
}