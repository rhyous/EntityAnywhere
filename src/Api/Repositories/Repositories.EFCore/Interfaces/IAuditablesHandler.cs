using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Rhyous.EntityAnywhere.Repositories
{
    public interface IAuditablesHandler
    {
        void HandleAuditableCreateDate(ChangeTracker tracker);
        void HandleAuditableCreatedBy(ChangeTracker tracker, long userId);
        void HandleAuditableLastUpdated(ChangeTracker tracker);
        void HandleAuditableLastUpdatedBy(ChangeTracker tracker, long userId);
    }
}