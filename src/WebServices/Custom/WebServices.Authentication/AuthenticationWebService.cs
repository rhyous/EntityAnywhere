using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// A custom web service, not related to an entity, for authentication. This web service forwards to the Authentication Service which loads the Authenticator plugins
    /// and attemps to authenticate to each of them.
    /// </summary>
    [ExcludedServiceBehaviorTypes(ServiceBehaviorType.Authenticator)]
    [CustomWebService("AuthenticationWebService", typeof(IAuthenticationWebService), null, "AuthenticationService.svc")]
    public class AuthenticationWebService : IAuthenticationWebService, ICustomWebService
    {
        /// <inheritdoc />
        public Token Authenticate(Credentials creds)
        {
            return Service.Authenticate(creds).ToConcrete<Token>();
        }

        /// <inheritdoc />
        public Token AuthenticateInQuery(string user, string password)
        {
            return Service.Authenticate(new Credentials() { User = user, Password = password }).ToConcrete<Token>();
        }

        #region Injectable
        internal AuthenticationService Service
        {
            get { return _Service ?? (_Service = new AuthenticationService()); }
            set { _Service = value; }
        } private AuthenticationService _Service;
        #endregion
    }
}