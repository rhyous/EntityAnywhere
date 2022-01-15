using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// By default, all ServiceBehaviors plugins are applied to all Web Services. 
    /// If this attribute exists, only included service behaviors will be added.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IncludedServiceBehaviorsAttribute : Attribute
    {
        /// <summary>
        /// Specify the Service Behavior plugins to include here.
        /// </summary>
        /// <param name="includedServiceBehaviors">The services behaviors to include.</param>
        public IncludedServiceBehaviorsAttribute(params string[] includedServiceBehaviors)
        {
            ServiceBehaviors = includedServiceBehaviors;
        }

        /// <summary>
        /// The Service Behabiors plugins to include
        /// </summary>
        public string[] ServiceBehaviors { get; }
    }
}
