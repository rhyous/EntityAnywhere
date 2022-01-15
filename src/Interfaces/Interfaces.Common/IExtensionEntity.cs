namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IExtensionEntity : IBaseEntity<long>, IAuditable
    {
        /// <summary>
        /// The name of the Entity this extension is for
        /// </summary>
        string Entity { get; set; }

        /// <summary>
        /// The id specifying the entity instance that this extension is for.
        /// </summary>
        string EntityId { get; set; }

        /// <summary>
        /// The name that identifies the custom value that this extension represents.
        /// </summary>
        string Property { get; set; }

        /// <summary>
        /// The custom value to be added to any entity instance.
        /// </summary>
        string Value { get; set; }
    }
}
