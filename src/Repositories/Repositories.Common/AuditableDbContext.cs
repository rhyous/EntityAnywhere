using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace Rhyous.WebFramework.Repositories
{
    public abstract class AuditableDbContext : DbContext
    {

        #region Constructors

        protected AuditableDbContext()
        {
        }

        protected AuditableDbContext(DbCompiledModel model) : base(model)
        {
        }

        protected AuditableDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        protected AuditableDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {
        }

        protected AuditableDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        protected AuditableDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {
        }

        protected AuditableDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext)
        {
        }

        #endregion

        /// <summary>
        /// Set to true to allow dates to not be changed to a current value.
        /// Use this for ETL.
        /// </summary>
        public bool IsHistorical { get; set; }

        public override int SaveChanges()
        {
            return SaveChanges(GetCurrentUserId());
        }

        public int SaveChanges(int userId)
        {
            HandleAuditables(userId);
            return base.SaveChanges();
        }

        /// <summary>
        /// This method must get the UserId.
        /// </summary>
        /// <returns>The id of the current user.</returns>
        public abstract int GetCurrentUserId();

        public virtual void HandleAuditables(int userId)
        {
            userId = userId > 0 ? userId : GetCurrentUserId();
            if (!IsHistorical)
            {
                AuditablesHandler.HandleAuditableCreatedBy(ChangeTracker, userId);
                AuditablesHandler.HandleAuditableLastUpdatedBy(ChangeTracker, userId);
                AuditablesHandler.HandleAuditableCreateDate(ChangeTracker);
                AuditablesHandler.HandleAuditableLastUpdated(ChangeTracker);
            }
        }
    }
}
