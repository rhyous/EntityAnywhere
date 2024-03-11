using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// If an Entity has a unique key property other than Id, for example, Name. Use this attribute to specify such unique key property.
    /// Currently, this only supports one per class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AlternateKeyAttribute : EntityAttribute
    {
        public AlternateKeyAttribute(string altKeyProperty) { KeyProperty = altKeyProperty; }

        /// <summary>
        /// The property name of the second unique key property.
        /// </summary>
        public string KeyProperty
        {
            get { return _KeyProperty; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("The KeyProperty must be specified.", nameof(KeyProperty));
                _KeyProperty = value;
            }
        } private string _KeyProperty;
    }
}
