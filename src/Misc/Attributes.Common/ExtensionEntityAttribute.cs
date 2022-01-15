namespace Rhyous.EntityAnywhere.Attributes
{
    public class ExtensionEntityAttribute :  EntityAttribute
    {
        /// <summary>
        /// For future use. In the future, we should have $expand use this instead of a static list of Extension Entities to expand.
        /// See RelatedEntityExtensions.cs in Services.Common.
        /// </summary>
        public bool AutoExpand { get; set; }
    }
}