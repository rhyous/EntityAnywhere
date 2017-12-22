using System;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// An abstract base class for all entities.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class AuditableEntity<TId> : Entity<TId>, IAuditable
    {
        /// <inheritdoc />
        public virtual DateTime CreateDate { get; set; }
        /// <inheritdoc />
        public virtual DateTime? LastUpdated { get; set; }
        /// <inheritdoc />
        public virtual int CreatedBy { get; set; }
        /// <inheritdoc />
        public virtual int? LastUpdatedBy { get; set; }
    }
}
