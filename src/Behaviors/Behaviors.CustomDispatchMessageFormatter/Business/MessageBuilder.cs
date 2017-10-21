using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Behaviors
{
    public class MessageBuilder : IMessageBuilder
    {

        const string Json = "application/json";

        public Message Build(MessageVersion messageVersion, string action, RawBodyWriter body)
        {
            Message replyMessage = Message.CreateMessage(messageVersion, action, body);
            replyMessage.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Raw));
            HttpResponseMessageProperty respProp = new HttpResponseMessageProperty();
            respProp.Headers[HttpResponseHeader.ContentType] = Json;
            replyMessage.Properties.Add(HttpResponseMessageProperty.Name, respProp);
            return replyMessage;
        }
    }
}
