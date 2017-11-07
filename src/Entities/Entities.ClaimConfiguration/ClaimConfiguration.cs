using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// A configuration describing how to get a claim from an Entity.
    /// The entity must have a relation to User or be User.
    /// </summary>
    public class ClaimConfiguration : AuditableEntity<int>, IClaimConfiguration
    {
        /// <inheritdoc />
        public string Domain { get; set; }
        /// <inheritdoc />
        public string Name { get; set; }
        /// <inheritdoc />
        public string Entity { get; set; }
        /// <inheritdoc />
        public string EntityProperty { get; set; }
        /// <inheritdoc />
        public string EntityIdProperty { get; set; }
        /// <inheritdoc />
        public string RelatedEntityIdProperty { get; set; }
    }
}