using System;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// An abstract base class for all entities.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class AuditableEntityBase<TId> : EntityBase<TId>, IAuditable
    {
        /// <inheritdoc />
        public DateTime CreateDate { get; set; }
        /// <inheritdoc />
        public DateTime? LastUpdated { get; set; }
        /// <inheritdoc />
        public int CreatedBy { get; set; }
        /// <inheritdoc />
        public int? LastUpdatedBy { get; set; }
    }
}
