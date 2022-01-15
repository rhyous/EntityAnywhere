using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// A generic DBContext for any single entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class UpdateDbContext<TEntity> : BaseDbContext<TEntity>, IUpdateDbContext<TEntity>
        where TEntity : class
    {
        public UpdateDbContext(IEntityConnectionStringNameProvider<TEntity> provider,
                               IAuditablesHandler auditablesHandler,
                               ISettings<TEntity> settings,
                               IUserDetails userDetails,
                               IMigrationsConfigurationContainer<TEntity> migrationsConfigurationContainer)
            : base(provider, auditablesHandler, settings, userDetails, migrationsConfigurationContainer)
        {
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