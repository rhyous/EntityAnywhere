using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;

namespace Rhyous.Wrappers
{
    public interface IOperationContext
    {
        OperationContext Context { get; }
        ICollection<SupportingTokenSpecification> SupportingTokens { get; }
        string SessionId { get; }
        ServiceSecurityContext ServiceSecurityContext { get; }
        IRequestContext RequestContext { get; set; }
        InstanceContext InstanceContext { get; }
        MessageVersion IncomingMessageVersion { get; }
        MessageProperties IncomingMessageProperties { get; }
        MessageHeaders IncomingMessageHeaders { get; }
        MessageProperties OutgoingMessageProperties { get; }
        MessageHeaders OutgoingMessageHeaders { get; }
        ServiceHostBase Host { get; }
        bool HasSupportingTokens { get; }
        IExtensionCollection<OperationContext> Extensions { get; }
        bool IsUserContext { get; }
        EndpointDispatcher EndpointDispatcher { get; set; }
        IContextChannel Channel { get; }
        ClaimsPrincipal ClaimsPrincipal { get; }

        event EventHandler OperationCompleted;

        T GetCallbackChannel<T>();
        void SetTransactionComplete();
    }
}
