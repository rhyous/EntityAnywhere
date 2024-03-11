using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
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
        private readonly IEntityConnectionStringProvider<TEntity> _Provider;
        private readonly ISettings<TEntity> _Settings;
        private readonly IUserDetails _UserDetails;

        #region Constructors

        public BaseDbContext(IEntityConnectionStringProvider<TEntity> provider,
                             IAuditablesHandler auditablesHandler,
                             ISettings<TEntity> settings,
                             IUserDetails userDetails)
            : base(auditablesHandler)
        {
            NameOrConnectionString = provider.Provide();
            _Provider = provider;
            _Settings = settings;
            _UserDetails = userDetails;
        }
        #endregion

        /// <summary>
        /// The UserId of the logged in user. This is different than the user that may be in the connection string
        /// </summary>
        public override long UserId => _UserDetails.UserId;

        /// <summary>
        /// This is the DbSet for the Entity.
        /// </summary>
        public virtual DbSet<TEntity> Entities { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseExceptionProcessor();
            optionsBuilder.UseSqlServer(_Provider.Provide());
        }

        /// <inheritdoc />
        /// <remarks>This override method makes the exception more useable by puttting the entity validation errors into the message.</remarks>
        public override int SaveChanges()
        {
            return base.SaveChanges();
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
    }
}