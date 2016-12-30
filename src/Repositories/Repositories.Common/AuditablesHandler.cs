using Rhyous.WebFramework.Interfaces;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Rhyous.WebFramework.Repositories
{
    public class AuditablesHandler
    {
        public static void HandleAuditableCreatedBy(DbChangeTracker tracker, int userId)
        {
            foreach (var auditableEntity in tracker.Entries<IAuditableCreatedBy>().Where(auditableEntity => auditableEntity.State == EntityState.Added))
            {
                auditableEntity.Entity.CreatedBy = userId;
            }
        }

        public static void HandleAuditableLastUpdatedBy(DbChangeTracker tracker, int userId)
        {
            foreach (var auditableEntity in tracker.Entries<IAuditableLastUpdatedBy>().Where(auditableEntity => auditableEntity.State == EntityState.Modified))
            {
                auditableEntity.Entity.LastUpdatedBy = userId;
            }
        }


        public static void HandleAuditableCreateDate(DbChangeTracker tracker)
        {
            foreach (var auditableEntity in tracker.Entries<IAuditableCreateDate>().Where(auditableEntity => auditableEntity.State == EntityState.Added))
            {
                auditableEntity.Entity.CreateDate = DateTime.Now;
            }
        }

        public static void HandleAuditableLastUpdated(DbChangeTracker tracker)
        {
            foreach (var auditableEntity in tracker.Entries<IAuditableLastUpdatedDate>().Where(auditableEntity => auditableEntity.State == EntityState.Modified))
            {
                auditableEntity.Entity.LastUpdated = DateTime.Now;
            }
        }
    }
}
