using System.Configuration;

namespace Rhyous.WebFramework.Behaviors
{
    public class ServiceResponseSection : ConfigurationSection
    {
        [ConfigurationProperty("ServiceResponses")]
        [ConfigurationCollection(typeof(ServiceResponse), AddItemName = "Response")]
        public ServiceResponseCollection ServiceResponses
        {
            get { return this["ServiceResponses"] as ServiceResponseCollection; }
        }
    }
}