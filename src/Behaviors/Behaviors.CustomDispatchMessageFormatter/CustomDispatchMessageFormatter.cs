using System;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Rhyous.EntityAnywhere.Behaviors
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
            if (message.IsEmpty || parameters.Length == 0)
                ParentFormatter.DeserializeRequest(message, parameters);
            else
                DeserializeMessageWithBody(message, parameters);
        }

        private void DeserializeMessageWithBody(Message message, object[] parameters)
        {
            if (parameters.Length > 1)
            {
                object[] tmpParams = new object[parameters.Length - 1];
                ParentFormatter.DeserializeRequest(message, tmpParams);
                tmpParams.CopyTo(parameters, 0);
            }
            if (message.GetWebContentFormat() != WebContentFormat.Raw)
                throw new InvalidOperationException("Incoming messages must have a body format of Raw.");
            byte[] rawBody = message.GetRawBody();
            var type = OperationDescription.Messages[0].Body.Parts.Last().Type;
            parameters[parameters.Length - 1] = Serializer.Deserialize(rawBody, type);
        }

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object obj)
        {
            if (obj != null && typeof(Stream).IsAssignableFrom(obj.GetType()))
                return ParentFormatter.SerializeReply(messageVersion, parameters, obj);
            var json = Serializer.Json(obj, ContractResolver.Instance);
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