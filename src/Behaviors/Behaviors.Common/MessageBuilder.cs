using System.Net;
using System.ServiceModel.Channels;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class MessageBuilder : IMessageBuilder
    {
        public const string Json = "application/json";

        public Message Build(MessageVersion messageVersion, string action, RawBodyWriter body, HttpResponseMessageProperty httpResponseMessageProperty = null)
        {
            Message replyMessage = Message.CreateMessage(messageVersion, action, body);
            replyMessage.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Raw));

            if (httpResponseMessageProperty == null)
                httpResponseMessageProperty = new HttpResponseMessageProperty();
            httpResponseMessageProperty.Headers[HttpResponseHeader.ContentType] = Json;

            replyMessage.Properties.Add(HttpResponseMessageProperty.Name, httpResponseMessageProperty);
            return replyMessage;
        }
    }
}