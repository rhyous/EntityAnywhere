namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// This entity provides a custom value to any entity given an entity name and an entity id.
    /// </summary>
    public partial interface IAddendum : IEntity<long>, IAuditable
    {
        /// <summary>
        /// The name of the Entity this addendum is for
        /// </summary>
        string Entity { get; set; }

        /// <summary>
        /// The id specifying the entity instance that this addendum is for.
        /// </summary>
        string EntityId { get; set; }

        /// <summary>
        /// The name that identifies the custom value that this addendum represents.
        /// </summary>
        string Property { get; set; }

        /// <summary>
        /// The custom value to be added to any entity instance.
        /// </summary>
        string Value { get; set; }
    }
}
