using Rhyous.WebFramework.Interfaces;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Rhyous.WebFramework.Repositories
{
    /// <summary>
    /// This class with set the value of an auditable property on an entity whenever it is created or updated.
    /// </summary>
    public class AuditablesHandler
    {
        /// <summary>
        /// Sets the CreatedBy property value to be the logged in user's Id.
        /// </summary>
        /// <param name="tracker">The DbChangeTracker from the DbContext.</param>
        /// <param name="userId">The UserId of the logged in user. This is different than the user that may be in the connection string.</param>
        public static void HandleAuditableCreatedBy(DbChangeTracker tracker, int userId)
        {
            foreach (var auditableEntity in tracker.Entries<IAuditableCreatedBy>().Where(auditableEntity => auditableEntity.State == EntityState.Added))
            {
                auditableEntity.Entity.CreatedBy = userId;
            }
        }

        /// <summary>
        /// Sets the LastUpdatedBy property value to be the logged in user's Id.
        /// </summary>
        /// <param name="tracker">The DbChangeTracker from the DbContext.</param>
        /// <param name="userId">The UserId of the logged in user. This is different than the user that may be in the connection string.</param>
        public static void HandleAuditableLastUpdatedBy(DbChangeTracker tracker, int userId)
        {
            foreach (var auditableEntity in tracker.Entries<IAuditableLastUpdatedBy>().Where(auditableEntity => auditableEntity.State == EntityState.Modified))
            {
                auditableEntity.Entity.LastUpdatedBy = userId;
            }
        }

        /// <summary>
        /// Sets CreateDate to DateTime.Now.
        /// </summary>
        /// <param name="tracker">The DbChangeTracker from the DbContext.</param>
        public static void HandleAuditableCreateDate(DbChangeTracker tracker)
        {
            foreach (var auditableEntity in tracker.Entries<IAuditableCreateDate>().Where(auditableEntity => auditableEntity.State == EntityState.Added))
            {
                auditableEntity.Entity.CreateDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Sets LastUpdated to DateTime.Now.
        /// </summary>
        /// <param name="tracker">The DbChangeTracker from the DbContext.</param>
        public static void HandleAuditableLastUpdated(DbChangeTracker tracker)
        {
            foreach (var auditableEntity in tracker.Entries<IAuditableLastUpdatedDate>().Where(auditableEntity => auditableEntity.State == EntityState.Modified))
            {
                auditableEntity.Entity.LastUpdated = DateTime.Now;
            }
        }
    }
}
