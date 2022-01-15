using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// An abstract base class for all entities.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class AuditableEntity<TId> : BaseEntity<TId>, IAuditable
    {
        /// <inheritdoc />
        [Editable(false)]
        [Display(Description = "The date this entity was created.")]
        public virtual DateTimeOffset CreateDate { get; set; }
        /// <inheritdoc />
        [Editable(false)]
        [Display(Description = "The date this entity was last updated.")]
        public virtual DateTimeOffset? LastUpdated { get; set; }
        /// <inheritdoc />
        [Editable(false)]
        [Display(Description = "The user.Id of the user that created this entity.")]
        public virtual long CreatedBy { get; set; }
        /// <inheritdoc />
        [Editable(false)]
        [Display(Description = "The user.Id of the user that last updated this entity.")]
        public virtual long? LastUpdatedBy { get; set; }
    }
}