using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// By default, all ServiceBehaviors plugins are applied to all Web Services. 
    /// If this attribute exists, all attributes are applied except behaviors with a ServiceBehaviorType in the list of excluded types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExcludedServiceBehaviorTypesAttribute : Attribute
    {
        /// <summary>
        /// Add the ServiceBehaviorTypes to exclude in this constructor.
        /// </summary>
        /// <param name="types">The ServiceBehaviorTypes to exclude.</param>
        public ExcludedServiceBehaviorTypesAttribute(params ServiceBehaviorType[] types)
        {
            Types = types;
        }

        /// <summary>
        /// The ServiceBehaviorTypes to exclude.
        /// </summary>
        public ServiceBehaviorType[] Types { get; set; }
    }
}
