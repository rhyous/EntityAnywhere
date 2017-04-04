using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;

namespace Rhyous.WebFramework.WebServices
{
    [ExcludedServiceBehaviorTypes(ServiceBehaviorType.Authenticator)]
    [CustomWebService("AuthenticationWebService", typeof(IAuthenticationWebService), null, "AuthenticationService.svc")]
    public class AuthenticationWebService : IAuthenticationWebService, ICustomWebService
    {
        public Token Authenticate(Credentials creds)
        {
            return Service.Authenticate(creds).ToConcrete<Token>();
        }

        public Token AuthenticateInQuery(string user, string password)
        {
            return Service.Authenticate(new Credentials() { User = user, Password = password }).ToConcrete<Token>();
        }

        #region Injectable
        public AuthenticationService Service
        {
            get { return _Service ?? (_Service = new AuthenticationService()); }
            set { _Service = value; }
        } private AuthenticationService _Service;
        #endregion
    }
}