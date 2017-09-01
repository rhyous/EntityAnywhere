using Rhyous.WebFramework.Interfaces;
using System.Security.Authentication;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// A custom service that assists with authentication.
    /// </summary>
    public partial class AuthenticationService
    {
        /// <summary>
        /// Loads the authenticator plugins and tries to authenticate to each of them.
        /// </summary>
        /// <param name="creds">The credentials, user name and password, to authenticate with.</param>
        /// <returns>A token if authenticated.</returns>
        /// <exception>AuthenticationException</exception>
        public IToken Authenticate(Credentials creds)
        {
            if (creds == null && WebOperationContext.Current != null)
            {
                var basicAuthHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (!string.IsNullOrWhiteSpace(basicAuthHeader))
                    creds = new BasicAuth(basicAuthHeader).Creds;
            }
            IToken token;
            if (CredentialsValidator.IsValid(creds, out token))
                return token;
            else
                throw new AuthenticationException("Invalid credentials.");
        }

        #region Injectable
        internal ICredentialsValidator CredentialsValidator
        {
            get { return _CredentialsValidator ?? (_CredentialsValidator = new PluginCredentialsValidator()); }
            set { _CredentialsValidator = value; }
        } private ICredentialsValidator _CredentialsValidator;
        #endregion
    }
}
