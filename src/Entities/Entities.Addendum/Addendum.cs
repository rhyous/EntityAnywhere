using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    public partial class Addendum : AuditableEntityBase<long>, IAddendum
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
