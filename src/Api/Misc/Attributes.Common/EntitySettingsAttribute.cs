namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// All attributes that can be added to an Entity should inherit this attribute.
    /// </summary>
    public class EntitySettingsAttribute : EntityAttribute
    {
        /// <summary>
        /// This property allows an entity to have a default description.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// The default group this entity is in.
        /// </summary>
        public virtual string Group { get; set; }

        /// <summary>
        /// The default description for this default group.
        /// </summary>
        public virtual string GroupDescription { get; set; }
    }
}