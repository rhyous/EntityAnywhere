using System;

namespace Rhyous.WebFramework.Attributes
{
    /// <summary>
    /// By default, all ServiceBehaviors plugins are applied to all Web Services. 
    /// If this attribute exists, only service behaviors of the included ServiceBehaviorTypes will be added.
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
