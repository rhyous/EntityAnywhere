using System.ComponentModel.DataAnnotations;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Entities
{
    [IdType(IsGenericForWebService = false)]
    public abstract class ExtensionEntity : AuditableEntity<long>, IExtensionEntity
    {        
        /// <inheritdoc />
        [Required]
        [DistinctProperty("ExtensionEntityGroup")]
        [MaxLength(100)]
        public virtual string Entity { get; set; }
        /// <inheritdoc />
        [Required]
        [DistinctProperty("ExtensionEntityGroup")]
        [MaxLength(100)]
        public virtual string EntityId { get; set; }
        /// <inheritdoc />
        [Required]
        [DistinctProperty("ExtensionEntityGroup")]
        [MaxLength(100)]
        public virtual string Property { get; set; }
        /// <inheritdoc />
        [Required]
        [DistinctProperty("ExtensionEntityGroup")]
        public virtual string Value { get; set; }
    }
}