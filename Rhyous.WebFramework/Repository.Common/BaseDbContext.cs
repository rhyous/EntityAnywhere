using Rhyous.Db.Auditable;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Reflection;

namespace Rhyous.WebFramework.Repositories
{
    public class BaseDbContext<T> : AuditableDbContext
        where T : class
    {
        #region Constructors
        public BaseDbContext()
            : this(0)            
        {
        }

        public BaseDbContext(int userId)
            : this(false)
        {
            UserId = userId;
        }

        public BaseDbContext(bool proxyCreationEnabled, bool lazyLoadingEnabled = false, bool asNoTracking = true)
            : base("name=SqlRepository")
        {
            SetConfig(proxyCreationEnabled, lazyLoadingEnabled, asNoTracking);
        }
        #endregion

        public int UserId { get; set; }

        public override int GetCurrentUserId()
        {
            return UserId > 0 ? UserId : (int)SystemUsers.Unknown;
        }

        public virtual DbSet<T> Entities { get; set; }

        public void SetConfig(bool proxyCreationEnabled = false, bool lazyLoadingEnabled = false, bool asNoTracking = true)
        {
            Configuration.ProxyCreationEnabled = proxyCreationEnabled;
            Configuration.LazyLoadingEnabled = lazyLoadingEnabled;
            if (asNoTracking)
                GloballySetAsNoTracking();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var errorMsg = e.Message + Environment.NewLine + e.GetValidationResultErrorsAsString();
                // Use the same exception type to avoid downstream bugs with code that catches this
                throw new DbEntityValidationException(errorMsg, e.EntityValidationErrors, e);
            }
        }

        private void GloballySetAsNoTracking()
        {
            var dbSetProperties = GetType().GetProperties();
            foreach (PropertyInfo pi in dbSetProperties)
            {
                var obj = pi.GetValue(this, null);
                if (obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition() == typeof(DbSet<>))
                {
                    var mi = obj.GetType().GetMethod("AsNoTracking");
                    mi.Invoke(obj, null);
                }
            }
        }
    }
}
