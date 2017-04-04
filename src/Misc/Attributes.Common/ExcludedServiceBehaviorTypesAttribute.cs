using System;

namespace Rhyous.WebFramework.Attributes
{
    /// <summary>
    /// If this attribute exists, all attributes are applied except behaviors in the list.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExcludedServiceBehaviorTypesAttribute : Attribute
    {
        public ExcludedServiceBehaviorTypesAttribute(params ServiceBehaviorType[] types)
        {
            Types = types;
        }

        public ServiceBehaviorType[] Types { get; set; }
    }
}
