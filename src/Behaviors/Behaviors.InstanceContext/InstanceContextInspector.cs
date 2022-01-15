using Rhyous.Collections;
using System;
using System.Configuration;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Web;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class InstanceContextInspector : IDispatchMessageInspector
    {
        #region IDispatchMessageInspector interface
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var callGuid = Guid.NewGuid();
            CallContext.LogicalSetData("CallGuid", callGuid);
            CallContext.LogicalSetData("OperationContext", OperationContext.Current);
            CallContext.LogicalSetData("WebOperationContext", WebOperationContext.Current);
            CallContext.LogicalSetData("HttpContext", HttpContext.Current);
            CallContext.LogicalSetData("AbsoluteUri", request.Headers.To.AbsoluteUri);
            var token = WebOperationContext.Current?.IncomingRequest?.Headers.Get("Token");
            if (!string.IsNullOrWhiteSpace(token))
                CallContext.LogicalSetData("Token", WebOperationContext.Current?.IncomingRequest?.Headers.Get("Token"));
            var entityAdminToken = WebOperationContext.Current?.IncomingRequest?.Headers.Get("EntityAdminToken");
            if (!string.IsNullOrWhiteSpace(entityAdminToken)
              && entityAdminToken == ConfigurationManager.AppSettings.Get("EntityAdminToken", ""))
                CallContext.LogicalSetData("IsAdmin", true);
            CallContext.LogicalSetData("IpAddress", (OperationContext.Current?.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty)?.Address);
            return callGuid;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }
        #endregion
    }
}