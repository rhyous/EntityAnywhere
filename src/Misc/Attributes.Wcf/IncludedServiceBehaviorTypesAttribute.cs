using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// By default, all ServiceBehaviors plugins are applied to all Web Services. 
    /// If this attribute exists, only service behaviors of the included ServiceBehaviorTypes will be added.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IncludedServiceBehaviorTypesAttribute : Attribute
    {
        /// <summary>
        /// Add the ServiceBehaviorTypes to include in this constructor.
        /// </summary>
        /// <param name="types">The ServiceBehaviorTypes to include.</param>
        public IncludedServiceBehaviorTypesAttribute(params ServiceBehaviorType[] types)
        {
            Types = types;
        }

        /// <summary>
        /// The ServiceBehaviorTypes to include.
        /// </summary>
        public ServiceBehaviorType[] Types { get; set; }
    }
}