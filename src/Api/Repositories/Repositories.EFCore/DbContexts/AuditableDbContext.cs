using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// This class is used to automatically populate auditable properties:
    /// - CreatedBy
    /// - LastUpdatedBy
    /// - CreateDate
    /// - LastUpdated
    /// </summary>
    public abstract class AuditableDbContext : DbContext, IAuditableDbContext
    {
        private readonly IAuditablesHandler _AuditablesHandler;

        #region Constructors
        protected AuditableDbContext(IAuditablesHandler auditablesHandler)
            : base()
        {
            _AuditablesHandler = auditablesHandler;
        }

        #endregion

        /// <summary>
        /// Set to true to allow dates to not be changed to a current value.
        /// Use this for ETL.
        /// </summary>
        public bool IsHistorical { get; set; }

        /// <inheritdoc />
        /// <remarks>This will handle setting auditables properties.</remarks>
        public override int SaveChanges()
        {
            HandleAuditables();
            return base.SaveChanges();
        }

        /// <summary>
        /// This method must get the UserId.
        /// </summary>
        /// <returns>The id of the current user.</returns>
        public abstract long UserId { get; }

        /// <summary>
        /// This method uses AuditablesHandler to check if an Entity implements an Auditable interface, and if so, populate the property.
        /// </summary>
        /// <param name="userId">The UserId of the logged in user. This is different than the user that may be in the connection string.</param>
        public virtual void HandleAuditables()
        {
            if (!IsHistorical)
            {
                _AuditablesHandler.HandleAuditableCreatedBy(ChangeTracker, UserId);
                _AuditablesHandler.HandleAuditableLastUpdatedBy(ChangeTracker, UserId);
                _AuditablesHandler.HandleAuditableCreateDate(ChangeTracker);
                _AuditablesHandler.HandleAuditableLastUpdated(ChangeTracker);
            }
        }
    }
}
