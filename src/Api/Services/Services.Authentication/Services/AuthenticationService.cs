using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// A custom service that assists with authentication.
    /// </summary>
    public partial class AuthenticationService : IAuthenticationService
    {
        private readonly ICredentialsValidatorAsync _CredentialsValidator;
        private readonly IAccountLocker _AccountLocker;
        private readonly IBasicAuth _BasicAuth;
        public readonly ILogger Logger;

        public AuthenticationService(ICredentialsValidatorAsync credentialsValidator, 
                                     IAccountLocker accountLocker,
                                     IBasicAuth basicAuth,
                                     ILogger logger)
        {
            _CredentialsValidator = credentialsValidator;
            _AccountLocker = accountLocker;
            _BasicAuth = basicAuth;
            Logger = logger;
        }

        /// <summary>
        /// Loads the authenticator plugins and tries to authenticate to each of them.
        /// </summary>
        /// <param name="creds">The credentials, user name and password, to authenticate with.</param>
        /// <returns>A token if authenticated.</returns>
        /// <exception>AuthenticationException</exception>
        public async Task<IToken> AuthenticateAsync(Credentials creds, string IpAddress)
        {
            if (creds == null  && (creds = _BasicAuth.Credentials) == null)
                throw new AuthenticationException("Invalid credentials. Credentials is null.");
            if (string.IsNullOrEmpty(creds.User) || string.IsNullOrEmpty(creds.Password))
                throw new AuthenticationException("Invalid credentials. Both a Username and password must be provided.");

            var attempt = new AuthenticationAttempt { Username = creds.User, IpAddress = IpAddress, AuthenticationPlugin = creds.AuthenticationPlugin };

            if (await _AccountLocker.IsLocked(creds))
                throw new AuthenticationException($"Account is locked. Please try again later. User: {creds.User}");

            CredentialsValidatorResponse credsValidatorResponse;
            try { credsValidatorResponse = await _CredentialsValidator.IsValidAsync(creds); }
            catch (Exception e)
            {
                // If it isn't an authentication exception, don't mark it as a failed login attempt.
                attempt.Result = "Unknown";
                attempt.Message = e.Message;
                await _AccountLocker.AddAttempt(attempt);
                throw;
            }
            if (credsValidatorResponse == null)
            {
                attempt.Result = "Failure";
                attempt.Message = "Error getting resonse from credential validators.";
                await _AccountLocker.AddAttempt(attempt);
                throw new AuthenticationException($"Invalid credentials. User: {creds.User}");
            }
            if (!credsValidatorResponse.Success)
            {
                attempt.Result = "Failure";
                attempt.Message = credsValidatorResponse.Message;
                await _AccountLocker.AddAttempt(attempt);
                throw new AuthenticationException(credsValidatorResponse.Message);
            }
            attempt.Result = "Success";
            attempt.AuthenticationPlugin = credsValidatorResponse.AuthenticationPlugin;
            await _AccountLocker.AddAttempt(attempt);
            return credsValidatorResponse.Token;
        }

        #region IDisposable

        private bool _IsDisposed;

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
                    // Dispose managed resources
                }
                // Dispose unmanaged resources
            }
        }

        #endregion
    }
}