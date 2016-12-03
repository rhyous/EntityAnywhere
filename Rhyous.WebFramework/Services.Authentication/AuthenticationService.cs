using Rhyous.WebFramework.Interfaces;
using System.Security.Authentication;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.Services
{
    public class AuthenticationService
    {
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
        public ICredentialsValidator CredentialsValidator
        {
            get { return _CredentialsValidator ?? (_CredentialsValidator = new PluginCredentialsValidator()); }
            internal set { _CredentialsValidator = value; }
        } private ICredentialsValidator _CredentialsValidator;
        #endregion
    }
}
