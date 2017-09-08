using Rhyous.WebFramework.Interfaces;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Reflection;

namespace Rhyous.WebFramework.Repositories
{
    /// <summary>
    /// A generic DBContext for any single entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class BaseDbContext<TEntity> : AuditableDbContext
        where TEntity : class
    {
        internal static string DefaultSqlConnectionStringName = "SqlRepository";

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
            : this($"name={DefaultSqlConnectionStringName}")
        {
            SetConfig(proxyCreationEnabled, lazyLoadingEnabled, asNoTracking);
        }

        protected BaseDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            if (ConfigSettings.UseEntityFrameworkDatabaseManagement)
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<BaseDbContext<TEntity>, Configuration<TEntity>>(DefaultSqlConnectionStringName));
            else
                Database.SetInitializer<BaseDbContext<TEntity>>(null); 
        }
        #endregion

        /// <summary>
        /// The UserId of the logged in user. This is different than the user that may be in the connection string
        /// </summary>
        public int UserId { get; set; }

        internal ISettings ConfigSettings
        {
            get { return _Settings ?? (_Settings = Settings.Instance); }
            set { _Settings = value; }
        } private ISettings _Settings;

        /// <summary>
        /// This method tries to get the UserId and if it is null, changes to the SystemUsers.Unknown user, id 2.
        /// In the database: user Id 1 must be system, user Id 2 must be Unknown.
        /// </summary>
        /// <returns>The user id</returns>
        public override int GetCurrentUserId()
        {
            return UserId > 0 ? UserId : (int)SystemUsers.Unknown;
        }

        /// <summary>
        /// This is the DbSet for the Entity.
        /// </summary>
        public virtual DbSet<TEntity> Entities { get; set; }

        /// <summary>
        /// This is a configuration setting option to enable or disable proxy creation, lazy loading, and no tracking.
        /// </summary>
        /// <param name="proxyCreationEnabled">bool</param>
        /// <param name="lazyLoadingEnabled">bool</param>
        /// <param name="asNoTracking">bool</param>
        public void SetConfig(bool proxyCreationEnabled = false, bool lazyLoadingEnabled = false, bool asNoTracking = true)
        {
            Configuration.ProxyCreationEnabled = proxyCreationEnabled;
            Configuration.LazyLoadingEnabled = lazyLoadingEnabled;
            if (asNoTracking)
                GloballySetAsNoTracking();
        }

        /// <inheritdoc />
        /// <remarks>This override method makes the exception more useable by puttting the entity validation errors into the message.</remarks>
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

        /// <summary>
        /// This might not be doing anything. AsNoTracking may be per call, and so setting this might not actually do anything for subsquent calls.
        /// This needs to be tested.
        /// </summary>
        private void GloballySetAsNoTracking()
        {
            var dbSetProperties = GetType().GetProperties();
            foreach (PropertyInfo pi in dbSetProperties)
            {
                var obj = pi.GetValue(this, null);
                if (obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition() == typeof(DbSet<>))
                {
                    var mi = obj.GetType().GetMethod("AsNoTracking");
                    try { mi.Invoke(obj, null); }
                    catch (Exception e) { throw e; }
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            if (typeof(TEntity).IsAssignableFrom(typeof(IAuditableCreateDate)))
            {
                modelBuilder.Entity<TEntity>()
                    .Property(f => (f as IAuditableCreateDate).CreateDate)
                    .HasColumnType("datetime2");
            }
            if (typeof(TEntity).IsAssignableFrom(typeof(IAuditableLastUpdatedDate)))
            {
                modelBuilder.Entity<TEntity>()
                    .Property(f => (f as IAuditableLastUpdatedDate).LastUpdated)
                    .HasColumnType("datetime2");
            }
        }
    }
}
