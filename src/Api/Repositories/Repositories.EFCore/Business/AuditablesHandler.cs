using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// This class with set the value of an auditable property on an entity whenever it is created or updated.
    /// </summary>
    public class AuditablesHandler : IAuditablesHandler
    {
        private const string UseProvidedAuditablesSetting = "UseProvidedAuditables";
        private const bool UseProvidedAuditablesDefault = false;
        private readonly IUrlParameters _UrlParameters;

        public AuditablesHandler(IUrlParameters urlParameters)
        {
            _UrlParameters = urlParameters;
        }

        /// <summary>
        /// Enables proxy creation in the dbcontext.
        /// </summary>
        private bool UseProvidedAuditables => _UseProvidedAuditables ?? (_UseProvidedAuditables = _UrlParameters.Collection.Get(UseProvidedAuditablesSetting, UseProvidedAuditablesDefault)).Value;
        private bool? _UseProvidedAuditables;

        /// <summary>
        /// Sets the CreatedBy property value to be the logged in user's Id.
        /// </summary>
        /// <param name="tracker">The DbChangeTracker from the DbContext.</param>
        /// <param name="userId">The UserId of the logged in user. This is different than the user that may be in the connection string.</param>
        public void HandleAuditableCreatedBy(ChangeTracker tracker, long userId)
        {
            foreach (var auditableEntity in tracker.Entries<IAuditableCreatedBy>().Where(auditableEntity => auditableEntity.State == EntityState.Added))
            {
                if (UseProvidedAuditables)
                    continue;
                auditableEntity.Entity.CreatedBy = userId;
            }
        }

        /// <summary>
        /// Sets the LastUpdatedBy property value to be the logged in user's Id.
        /// </summary>
        /// <param name="tracker">The DbChangeTracker from the DbContext.</param>
        /// <param name="userId">The UserId of the logged in user. This is different than the user that may be in the connection string.</param>
        public void HandleAuditableLastUpdatedBy(ChangeTracker tracker, long userId)
        {
            foreach (var auditableEntity in tracker.Entries<IAuditableLastUpdatedBy>().Where(auditableEntity => auditableEntity.State == EntityState.Modified))
            {
                if (UseProvidedAuditables)
                    continue;
                auditableEntity.Entity.LastUpdatedBy = userId;
            }
        }

        /// <summary>
        /// Sets CreateDate to DateTimeOffset.Now.
        /// </summary>
        /// <param name="tracker">The DbChangeTracker from the DbContext.</param>
        public void HandleAuditableCreateDate(ChangeTracker tracker)
        {
            foreach (var auditableEntity in tracker.Entries<IAuditableCreateDate>().Where(auditableEntity => auditableEntity.State == EntityState.Added))
            {
                if (UseProvidedAuditables)
                    continue;
                auditableEntity.Entity.CreateDate = DateTimeOffset.Now;
            }
        }

        /// <summary>
        /// Sets LastUpdated to DateTimeOffset.Now.
        /// </summary>
        /// <param name="tracker">The DbChangeTracker from the DbContext.</param>
        public void HandleAuditableLastUpdated(ChangeTracker tracker)
        {
            foreach (var auditableEntity in tracker.Entries<IAuditableLastUpdatedDate>().Where(auditableEntity => auditableEntity.State == EntityState.Modified))
            {
                if (UseProvidedAuditables)
                     continue;
                auditableEntity.Entity.LastUpdated = DateTimeOffset.Now;
            }
        }
    }
}
