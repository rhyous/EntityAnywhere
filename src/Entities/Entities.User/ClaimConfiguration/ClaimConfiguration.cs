using Rhyous.EntityAnywhere.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// A configuration describing how to get a claim from an Entity.
    /// The entity must have a relation to User or be User.
    /// </summary>
    [ClaimConfigurationSeedData]
    public class ClaimConfiguration : AuditableEntity<int>, IClaimConfiguration
    {
        /// <inheritdoc />
        [Required]
        public string Domain { get; set; }
        /// <inheritdoc />
        [Required]
        public string Name { get; set; }
        /// <inheritdoc />
        [Required]
        public string Entity { get; set; }
        /// <inheritdoc />
        [Required]
        public string EntityProperty { get; set; }
        /// <inheritdoc />
        public string EntityIdProperty { get; set; }
        /// <inheritdoc />
        public string RelatedEntityIdProperty { get; set; }
    }
}