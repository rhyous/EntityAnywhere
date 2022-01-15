using System.Configuration;
using System.Xml;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class ServiceResponse : ConfigurationElement
    {
        const string AttributeName = "key";
        protected override void DeserializeElement(XmlReader reader, bool s)
        {
            Key = reader.GetAttribute(AttributeName);
            ResponseString = reader.ReadElementContentAs(typeof(string), null) as string;
            ResponseString = ResponseString.Trim();
        }

        [ConfigurationProperty(AttributeName, IsKey = true, IsRequired = true)]
        public string Key { get; set; }

        public string ResponseString { get; set; }
    }
}