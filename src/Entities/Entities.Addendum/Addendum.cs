using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// This entity is used for extending any entity without creating a new property for that entity.
    /// Every entity has the ability to have Addenda, which is a custom property value list.
    /// </summary>
    public partial class Addendum : AuditableEntity<long>, IAddendum
    {
        /// <inheritdoc />
        public string Entity { get; set; }
        /// <inheritdoc />
        public string EntityId { get; set; }
        /// <inheritdoc />
        public string Property { get; set; }
        /// <inheritdoc />
        public string Value { get; set; }
    }
}
