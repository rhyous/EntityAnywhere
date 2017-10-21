using Newtonsoft.Json.Linq;
using Rhyous.WebFramework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Rhyous.WebFramework.Behaviors
{
    public class CustomDispatchMessageFormatter : IDispatchMessageFormatter
    {
        internal WebHttpBehavior EndpointBehavior { get; set; }
        internal IDispatchMessageFormatter ParentFormatter { get; set; }
        internal OperationDescription OperationDescription { get; set; }
        
        public CustomDispatchMessageFormatter(WebHttpBehavior behavior, OperationDescription operation, IDispatchMessageFormatter formatter)
        {
            EndpointBehavior = behavior;
            OperationDescription = operation;
            ParentFormatter = formatter;
        }

        public void DeserializeRequest(Message message, object[] parameters)
        {
            ParentFormatter.DeserializeRequest(message, parameters);
        }

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            var json = Serializer.Json(result);
            var body = new RawBodyWriter(json);
            return MessageBuilder.Build(messageVersion, OperationDescription.Messages[1].Action, body);
        }

        public ISerializer Serializer
        {
            get { return _Serializer ?? (_Serializer = new Serializer()); }
            set { _Serializer = value; }
        } private ISerializer _Serializer;

        public IMessageBuilder MessageBuilder
        {
            get { return _MessageBuilder ?? (_MessageBuilder = new MessageBuilder()); }
            set { _MessageBuilder = value; }
        } private IMessageBuilder _MessageBuilder;
    }
}