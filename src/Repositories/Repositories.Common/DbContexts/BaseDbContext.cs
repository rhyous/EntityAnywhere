using Rhyous.Collections;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// A generic DBContext for any single entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class BaseDbContext<TEntity> : AuditableDbContext, IBaseDbContext<TEntity>
        where TEntity : class
    {
        internal readonly string NameOrConnectionString;
        private readonly ISettings<TEntity> _Settings;
        private readonly IUserDetails _UserDetails;

        #region Constructors

        public BaseDbContext(IEntityConnectionStringNameProvider<TEntity> provider,
                             IAuditablesHandler auditablesHandler,
                             ISettings<TEntity> settings,
                             IUserDetails userDetails,
                             IMigrationsConfigurationContainer<TEntity> migrationsConfigurationContainer)
            : base($"name={provider.Provide()}", auditablesHandler)
        {
            NameOrConnectionString = provider.Provide();
            _Settings = settings;
            _UserDetails = userDetails;
            SetConfig(_Settings.ProxyCreationEnabled, _Settings.LazyLoadingEnabled);
            if (_Settings.UseEntityFrameworkDatabaseManagement)
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<BaseDbContext<TEntity>, DbMigrationsConfiguration<BaseDbContext<TEntity>>>(true, migrationsConfigurationContainer.Config));
            else
                Database.SetInitializer<BaseDbContext<TEntity>>(null);
        }
        #endregion

        /// <summary>
        /// The UserId of the logged in user. This is different than the user that may be in the connection string
        /// </summary>
        public override long UserId => _UserDetails.UserId;

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

        public void SetIsModified(TEntity entity, string prop, bool isModified)
        {
            var propInfo = entity.GetPropertyInfo(prop);
            var name = propInfo.Name;
            var repositoryProperty = propInfo.GetCustomAttribute<RepositoryRedirectPropertyAttribute>();
            if (repositoryProperty != null)
                name = repositoryProperty.PropertyName;
            Entry(entity).Property(name).IsModified = isModified;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var result = base.ValidateEntity(entityEntry, items);
            var falseErrors = result.ValidationErrors.Where(error =>entityEntry.State == EntityState.Modified);
            foreach (var error in falseErrors.ToArray())
                result.ValidationErrors.Remove(error);
            return result;
        }
    }
}