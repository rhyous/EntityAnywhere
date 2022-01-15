using Rhyous.Collections;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Tools;
using Rhyous.Wrappers;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Web;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class HeaderValidationInspector : IHeaderValidationInspector
    {
        private readonly IPluginHeaderValidator _HeaderValidator;
        private readonly IAccessController _AccessController;
        private readonly ILogger Logger;

        public HeaderValidationInspector(IPluginHeaderValidator headerValidator,
                                         IAccessController accessController,
                                         ILogger logger)
        {
            _HeaderValidator = headerValidator;
            _AccessController = accessController;
            Logger = logger;
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // Return BadRequest if request is null
            if (WebOperationContext.Current == null) { throw new RestException(HttpStatusCode.BadRequest); }
            var headers = Append(WebOperationContext.Current?.IncomingRequest?.Headers, HttpContext.Current?.Request?.Headers);
            var webOperationContext = new WebOperationContextWrapper(WebOperationContext.Current);
            return AfterReceiveRequest(ref request, channel, instanceContext, webOperationContext, headers);
       }

        internal object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext, IWebOperationContext context, NameValueCollection headers)
        {
            var urlAbsolutePath = request.Headers?.To.AbsolutePath;
            // There are many ways to get headers, but this one works in One Way where others didn't
            headers.Add("AbsolutePath", urlAbsolutePath);
            var pathAndQuery = request.Headers?.To.PathAndQuery;
            headers.Add("PathAndQuery", pathAndQuery);
            headers.Add("HttpMethod", context.IncomingRequest.Method);

            if (_AccessController.IsSystemAdmin(headers, context) || _AccessController.IsAnonymousAllowed(urlAbsolutePath))
            {
                return null;
            }

            // At this point we need to verify so we will get it and verify
            if (TaskRunner.RunSynchonously(_HeaderValidator.IsValidAsync, headers))
            {
                CallContext.LogicalSetData("UserId", _HeaderValidator.UserId);
                return null;
            }

            Logger?.Debug($"The following url is forbidden: {request.Headers?.To.AbsoluteUri}. Token: {headers.Get("Token","No token provided")}");
            throw new RestException(request.Headers?.To.AbsoluteUri, HttpStatusCode.Forbidden);
        }

        public void BeforeSendReply(ref Message reply, object correlationState) {}

        internal NameValueCollection Append(params NameValueCollection[] args)
        {
            if (args == null || args.Length == 0)
            {
                return new NameValueCollection();
            }
            var mergedNvc = args[0] ?? new NameValueCollection();
            for (int i = 1; i < args.Length; i++)
            {
                var currentNvc = args[i];
                if (currentNvc != null && currentNvc.Count > 0)
                {
                    foreach (string key in currentNvc)
                    {
                        var existingValue = mergedNvc[key];
                        var newValue = currentNvc[key];
                        if (existingValue == newValue || (existingValue != null && existingValue.Split(',').Any(s=> s == newValue)))
                            continue;
                        mergedNvc.Add(key, currentNvc[key]);
                    }
                }
            }
            return mergedNvc;
        }
    }
}