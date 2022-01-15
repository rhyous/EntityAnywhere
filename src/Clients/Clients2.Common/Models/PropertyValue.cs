namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// A property value pair representing an extension entity, where the entity, entity id, and extension entity
    /// are provided by the endpoint.
    /// </summary>
    public class PropertyValue
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }
}
