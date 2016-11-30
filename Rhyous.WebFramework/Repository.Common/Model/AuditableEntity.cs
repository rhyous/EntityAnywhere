using Rhyous.WebFramework.Interfaces;
using System;

namespace Rhyous.WebFramework.Repositories
{
    public abstract class AuditableEntity : IAuditable
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
