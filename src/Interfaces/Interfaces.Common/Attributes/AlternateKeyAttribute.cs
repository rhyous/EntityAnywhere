using System;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// If an Entity has a unique key property other than Id, for example, Name. Use this attribute to specify such unique key property.
    /// Currently, this only supports one per class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AlternateKeyAttribute : EntityAttribute
    {
        public AlternateKeyAttribute() { }
        public AlternateKeyAttribute(string idProperty) { KeyProperty = idProperty; }

        /// <summary>
        /// The property name of the second unique key property.
        /// </summary>
        public string KeyProperty { get; set; }
    }
}
