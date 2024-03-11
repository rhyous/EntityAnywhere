namespace Rhyous.EntityAnywhere.Attributes
{
    public class ExtensionEntityAttribute :  EntitySettingsAttribute
    {
        internal const string ExtensionEntityGroup = "Extension Entities";
        /// <summary>
        /// For future use. In the future, we should have $expand use this instead of a static list of Extension Entities to expand.
        /// See RelatedEntityExtensions.cs in Services.Common.
        /// </summary>
        public bool AutoExpand { get; set; }

        public override string Group { get; set; } = ExtensionEntityGroup;
    }
}