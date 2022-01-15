namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// An attribute to set the default values for an Entity Property.
    /// </summary>
    public class EntityPropertyAttribute : EntityAttribute
    {
        /// <summary>
        /// This property allows an entity property to have a default description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// This property allows an entity property to have a default sort order. This is useful
        /// for UI purposes.
        /// </summary>
        public int Order { get; set; }
    }
}

