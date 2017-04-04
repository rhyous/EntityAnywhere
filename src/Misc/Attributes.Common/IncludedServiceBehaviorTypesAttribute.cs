using System;

namespace Rhyous.WebFramework.Attributes
{
    /// <summary>
    /// If this attribute exists, only included service behaviors will be added.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IncludedServiceBehaviorTypesAttribute : Attribute
    {
        public IncludedServiceBehaviorTypesAttribute(params ServiceBehaviorType[] types)
        {
            Types = types;
        }

        public ServiceBehaviorType[] Types { get; set; }
    }
}
