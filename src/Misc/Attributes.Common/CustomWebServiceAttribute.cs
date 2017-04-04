using System;

namespace Rhyous.WebFramework.Attributes
{
    public class CustomWebServiceAttribute : Attribute
    {
        public CustomWebServiceAttribute(string name = null, Type serviceContract = null, Type entity = null, string serviceRoute = null)
        {
            Name = name;
            ServiceContract = serviceContract;
            Entity = entity;
            ServiceRoute = serviceRoute;
        }

        public string Name { get; set; }

        public Type ServiceContract { get; set; }

        public Type Entity { get; set; }

        public string ServiceRoute { get; set; }
    }
}
