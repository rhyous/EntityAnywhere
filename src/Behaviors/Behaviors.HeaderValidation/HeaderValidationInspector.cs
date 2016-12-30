using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.Behaviors
{
    public class HeaderValidationInspector : IDispatchMessageInspector
    {

        public static readonly string AllowAnonymousSvcPages = "AllowAnonymousSvcPages";
        public static readonly string AllowAnonymousSvcHelpPages = "AllowAnonymousSvcHelpPages";

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // Return BadRequest if request is null
            if (WebOperationContext.Current == null) { throw new WebFaultException(HttpStatusCode.BadRequest); }

            if (IsAnonymousAllowed(request.Headers.To.AbsolutePath))
                return null;

            ValidateByHeaders(WebOperationContext.Current.IncomingRequest.Headers);
            return null;
        }

        private static bool IsAnonymousAllowed(string absolutePath)
        {
            return (ConfigurationManager.AppSettings.Get(AllowAnonymousSvcPages, true)
                    && absolutePath.EndsWith(".svc"))
                   || (ConfigurationManager.AppSettings.Get(AllowAnonymousSvcHelpPages, true)
                       && absolutePath.Contains("/help"));
        }

        private static void ValidateByHeaders(NameValueCollection token)
        {
            IHeaderValidator validator = new PluginHeaderValidator();
            if (!validator.IsValid(token))
            {
                throw new WebFaultException(HttpStatusCode.Forbidden);
            }
            // Add Userid to the header so the service has it if needed
            WebOperationContext.Current?.IncomingRequest.Headers.Add("UserId", validator.UserId.ToString());
        }

        private static void ValidateBasicAuthentication()
        {
            var authorization = WebOperationContext.Current?.IncomingRequest.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(authorization))
            {
                throw new WebFaultException(HttpStatusCode.Forbidden);
            }
            var basicAuth = new BasicAuth(authorization);
            var token = AuthService.Authenticate(basicAuth.Creds);
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        #region Injectables
        public static AuthenticationService AuthService
        {
            get { return _AuthService ?? (_AuthService = new AuthenticationService()); }
            internal set { _AuthService = value; }
        } private static AuthenticationService _AuthService;
        #endregion
    }
}
