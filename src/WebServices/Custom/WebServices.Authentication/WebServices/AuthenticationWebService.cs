using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// A custom web service, not related to an entity, for authentication. This web service forwards to the Authentication Service which loads the Authenticator plugins
    /// and attemps to authenticate to each of them.
    /// </summary>
    [ExcludedServiceBehaviorTypes(ServiceBehaviorType.Authenticator)]
    [CustomWebService("AuthenticationWebService", typeof(IAuthenticationWebService), null, "AuthenticationService.svc")]
    public class AuthenticationWebService : IAuthenticationWebService, ICustomWebService
    {
        private readonly IAuthenticationService _Service;
        private readonly IRequestSourceIpAddress _RequestSourceIpAddress;
        private readonly ILogger _Logger;

        public AuthenticationWebService(IAuthenticationService service, IRequestSourceIpAddress requestSourceIpAddress, ILogger Logger)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _RequestSourceIpAddress = requestSourceIpAddress;
            _Logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
        }

        /// <inheritdoc />
        public async Task<Token> AuthenticateAsync(Credentials creds)
        {            
            return (await _Service.AuthenticateAsync(creds, _RequestSourceIpAddress.IpAddress))?.ToConcrete<Token>();
        }

        /// <inheritdoc />
        public async Task<Token> AuthenticateInQueryAsync(string user, string password)
        {
            return (await _Service.AuthenticateAsync(new Credentials { User = user, Password = password }, _RequestSourceIpAddress.IpAddress))?.ToConcrete<Token>();
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