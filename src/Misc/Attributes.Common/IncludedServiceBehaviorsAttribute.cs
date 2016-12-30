using System;

namespace Rhyous.WebFramework.Attributes
{
    /// <summary>
    /// If this attribute exists, only included service behaviors will be added.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IncludedServiceBehaviorsAttribute : Attribute
    {
        public IncludedServiceBehaviorsAttribute(params string[] includedServiceBehaviors)
        {
            _ServiceBehaviors = includedServiceBehaviors;
        }

        public string[] ServiceBehaviors => _ServiceBehaviors;
        private string[] _ServiceBehaviors;
    }
}
