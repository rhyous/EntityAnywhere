using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// By default, all ServiceBehaviors plugins are applied to all Web Services. 
    /// If this attribute exists, all attributes are applied except behaviors in the list.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExcludedServiceBehaviorsAttribute : Attribute
    {
        /// <summary>
        /// Specify the Service Behavior plugins to exclude here.
        /// </summary>
        /// <param name="excludedServiceBehaviors">The services behaviors to exclude.</param>
        public ExcludedServiceBehaviorsAttribute(params string[] excludedServiceBehaviors)
        {
            ServiceBehaviors = excludedServiceBehaviors;
        }

        /// <summary>
        /// The services behaviors to exclude.
        /// </summary>
        public string[] ServiceBehaviors { get; }
    }
}
