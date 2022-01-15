using Rhyous.Collections;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class MessageHandler : IMessageHandler
    {
        public IMessageProperty GetHttpRequestMessageProperty(Message message, string name)
        {
            return (IMessageProperty)message.Properties[name];
        }

        public WebContentFormat GetMessageContentFormat(Message message)
        {
            WebContentFormat format = WebContentFormat.Default;
            if (message.Properties.ContainsKey(WebBodyFormatMessageProperty.Name))
            {
                WebBodyFormatMessageProperty bodyFormat;
                bodyFormat = (WebBodyFormatMessageProperty)message.Properties[WebBodyFormatMessageProperty.Name];
                format = bodyFormat.Format;
            }

            return format;
        }

        public string MessageToString(ref Message message)
        {
            WebContentFormat messageFormat = GetMessageContentFormat(message);
            MemoryStream ms = new MemoryStream();
            switch (messageFormat)
            {
                case WebContentFormat.Default:
                case WebContentFormat.Xml:
                    return ReadAndReplaceBody(ref message, messageFormat, ms, XmlDictionaryWriter.CreateTextWriter(ms));
                case WebContentFormat.Json:
                    return ReadAndReplaceBody(ref message, messageFormat, ms, JsonReaderWriterFactory.CreateJsonWriter(ms));
                case WebContentFormat.Raw:
                    return ReadRawBody(ref message);
            }
            return null;
        }

        private static string ReadAndReplaceBody(ref Message message, WebContentFormat messageFormat, MemoryStream ms, XmlDictionaryWriter writer)
        {
            message.WriteMessage(writer);
            writer.Flush();
            string messageBody = Encoding.UTF8.GetString(ms.ToArray());

            // now that the message was read, it needs to be recreated.
            ms.Position = 0;

            XmlDictionaryReader reader = messageFormat == WebContentFormat.Json
                                       ? JsonReaderWriterFactory.CreateJsonReader(ms, XmlDictionaryReaderQuotas.Max)
                                       : XmlDictionaryReader.CreateTextReader(ms, XmlDictionaryReaderQuotas.Max);

            Message newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            foreach (MessageHeader header in message.Headers)
            {
                newMessage.Headers.Add(header);
            } 
            message = newMessage;
            return messageBody;
        }

        private string ReadRawBody(ref Message message)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            bodyReader.ReadStartElement("Binary");
            byte[] bodyBytes = bodyReader.ReadContentAsBase64();
            string messageBody = Encoding.UTF8.GetString(bodyBytes);

            // Now to recreate the message
            MemoryStream ms = new MemoryStream();
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateBinaryWriter(ms);
            writer.WriteStartElement("Binary");
            writer.WriteBase64(bodyBytes, 0, bodyBytes.Length);
            writer.WriteEndElement();
            writer.Flush();
            ms.Position = 0;
            XmlDictionaryReader reader = XmlDictionaryReader.CreateBinaryReader(ms, XmlDictionaryReaderQuotas.Max);
            Message newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            message = newMessage;

            return messageBody;
        }

        public bool LogRequestMessage => ConfigurationManager.AppSettings.Get("LogRequestMessage", false);

        public bool LogResponseMessage => ConfigurationManager.AppSettings.Get("LogResponseMessage", false);
    }
}