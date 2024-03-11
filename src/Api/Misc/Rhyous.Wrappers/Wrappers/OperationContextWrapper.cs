using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;

namespace Rhyous.Wrappers
{
    public class OperationContextWrapper : IOperationContext
    {
        public OperationContextWrapper(OperationContext operationContext)
        {
            Context = operationContext;
            RequestContext = new RequestContextWrapper(operationContext.RequestContext);
        }

        public OperationContext Context { get; }

        public ICollection<SupportingTokenSpecification> SupportingTokens => Context.SupportingTokens;

        public string SessionId => Context.SessionId;

        public ServiceSecurityContext ServiceSecurityContext => Context.ServiceSecurityContext;

        public IRequestContext RequestContext { get; set; }

        public InstanceContext InstanceContext => Context.InstanceContext;

        public MessageVersion IncomingMessageVersion => Context.IncomingMessageVersion;

        public MessageProperties IncomingMessageProperties => Context.IncomingMessageProperties;

        public MessageHeaders IncomingMessageHeaders => Context.IncomingMessageHeaders;

        public MessageProperties OutgoingMessageProperties => Context.OutgoingMessageProperties;

        public MessageHeaders OutgoingMessageHeaders => Context.OutgoingMessageHeaders;

        public ServiceHostBase Host => Context.Host;

        public bool HasSupportingTokens => Context.HasSupportingTokens;

        public IExtensionCollection<OperationContext> Extensions => Context.Extensions;

        public bool IsUserContext => Context.IsUserContext;

        public EndpointDispatcher EndpointDispatcher { get => Context.EndpointDispatcher; set => Context.EndpointDispatcher = value; }

        public IContextChannel Channel => Context.Channel;

        public ClaimsPrincipal ClaimsPrincipal => Context.ClaimsPrincipal;

        public event EventHandler OperationCompleted 
        {
            add { Context.OperationCompleted += value; } 
            remove { Context.OperationCompleted -= value; }
        }

        public T GetCallbackChannel<T>()
        {
            return Context.GetCallbackChannel<T>();
        }

        public void SetTransactionComplete()
        {
            Context.SetTransactionComplete();
        }
    }
}
