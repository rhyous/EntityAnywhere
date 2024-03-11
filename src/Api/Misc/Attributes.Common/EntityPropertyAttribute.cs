using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// An attribute to set the default values for an Entity Property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EntityPropertyAttribute : EntityAttribute
    {
        /// <summary>
        /// This property allows an entity property to have a default description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// This property allows an entity property to have a default sort order. This is only useful
        /// for UI purposes.
        /// </summary>
        public int Order { get; set; } = int.MaxValue;

        /// <summary>
        /// This property allows an entity property to be UI searchable. This is only useful
        /// for UI purposes.
        /// </summary>
        public bool Searchable { get; set; }
    }
}

