namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// This class is used by the custom Addendum Web Service to identify which addendums to fetch. An addendum is mapped to an entity and an id.
    /// </summary>
    public class EntityIdentifier
    {
        /// <summary>
        /// The Entity name.
        /// </summary>
        public string Entity { get; set; }
        /// <summary>
        /// The Entity Id.
        /// </summary>
        public string EntityId { get; set; }
    }
}
