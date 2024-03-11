using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// DbContext - automatic Entity Id generation option turned off 
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TId">The entity id type</typeparam>
    public class IdentityInsertDbContext<TEntity, TId> : BaseDbContext<TEntity>, IIdentityInsertDbContext<TEntity, TId>
        where TEntity : class, IId<TId>
        where TId : struct
    {
        public IdentityInsertDbContext(IEntityConnectionStringProvider<TEntity> provider,
                               IAuditablesHandler auditablesHandler,
                               ISettings<TEntity> settings,
                               IUserDetails userDetails,
                               IMigrationsConfigurationContainer<TEntity> migrationsConfigurationContainer)
            : base(provider, auditablesHandler, settings, userDetails, migrationsConfigurationContainer)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TEntity>()
              .Property(a => a.Id)
              .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            base.OnModelCreating(modelBuilder);
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method disables auto increment property of fields in TEntity db table.
        /// It inserts seed data within transaction and re-enables auto increment.
        /// </remarks>
        public override int SaveChanges()
        {
            try
            {
                var successfulInsertCount = 0;
                var tableName = this.GetTableName<TEntity>();

                var identityInsertBaseSqlQuery = $"SET IDENTITY_INSERT {tableName}";

                using (var transaction = Database.BeginTransaction())
                {
                    Database.ExecuteSqlCommand($"{identityInsertBaseSqlQuery} ON");
                    successfulInsertCount = base.SaveChanges();
                    Database.ExecuteSqlCommand($"{identityInsertBaseSqlQuery} OFF");
                    transaction.Commit();
                }

                return successfulInsertCount;
            }
            catch (DbEntityValidationException e)
            {
                var errorMsg = e.Message + Environment.NewLine + e.GetValidationResultErrorsAsString();
                throw new DbEntityValidationException(errorMsg, e.EntityValidationErrors, e);
            }

        }
    }
}