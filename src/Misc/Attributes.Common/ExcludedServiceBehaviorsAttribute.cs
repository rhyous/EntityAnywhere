using System;

namespace Rhyous.WebFramework.Attributes
{
    /// <summary>
    /// By default, all ServiceBehaviors plugins are applied to all Web Services. 
    /// If this attribute exists, all attributes are applied except behaviors in the list.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExcludedServiceBehaviorsAttribute : Attribute
    {
        public ExcludedServiceBehaviorsAttribute(params string[] excludedServiceBehaviors)
        {
            _ServiceBehaviors = excludedServiceBehaviors;
        }

        public string[] ServiceBehaviors => _ServiceBehaviors;
        private string[] _ServiceBehaviors;
    }
}
