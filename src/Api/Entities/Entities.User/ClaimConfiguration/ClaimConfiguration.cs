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
        [StringLength(100)]
        public string Domain { get; set; }
        /// <inheritdoc />
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        /// <inheritdoc />
        [Required]
        [StringLength(100)]
        public string Entity { get; set; }
        /// <inheritdoc />
        [Required]
        [StringLength(100)]
        public string EntityProperty { get; set; }
        /// <inheritdoc />
        [StringLength(100)]
        public string EntityIdProperty { get; set; }
        /// <inheritdoc />
        [StringLength(100)]
        public string RelatedEntityIdProperty { get; set; }
    }
}