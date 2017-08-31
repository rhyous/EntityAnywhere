using System;

namespace Rhyous.WebFramework.Attributes
{
    /// <summary>
    /// By default, all ServiceBehaviors plugins are applied to all Web Services. 
    /// If this attribute exists, all attributes are applied except behaviors with a ServiceBehaviorType in the list of excluded types.
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
