using System.Data.Entity.Infrastructure;

namespace Rhyous.EntityAnywhere.Repositories
{
    public interface IAuditablesHandler
    {
        void HandleAuditableCreateDate(DbChangeTracker tracker);
        void HandleAuditableCreatedBy(DbChangeTracker tracker, long userId);
        void HandleAuditableLastUpdated(DbChangeTracker tracker);
        void HandleAuditableLastUpdatedBy(DbChangeTracker tracker, long userId);
    }
}