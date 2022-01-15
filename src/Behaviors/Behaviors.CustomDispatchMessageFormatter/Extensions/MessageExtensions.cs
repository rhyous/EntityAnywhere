using System.ServiceModel.Channels;
using System.Xml;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public static class MessageExtensions
    {
        public static WebContentFormat GetWebContentFormat(this Message message)
        {
            if (!message.Properties.TryGetValue(WebBodyFormatMessageProperty.Name, out object bodyFormatProperty))
                return WebContentFormat.Default;
            return (bodyFormatProperty as WebBodyFormatMessageProperty).Format;                
        }

        public static byte[] GetRawBody(this Message message)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            bodyReader.ReadStartElement("Binary");
            byte[] rawBody = bodyReader.ReadContentAsBase64();
            return rawBody;
        }
    }
}
