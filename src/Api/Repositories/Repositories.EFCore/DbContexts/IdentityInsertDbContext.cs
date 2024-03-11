using Microsoft.EntityFrameworkCore;
using Rhyous.EntityAnywhere.Interfaces;

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
                               IUserDetails userDetails)
            : base(provider, auditablesHandler, settings, userDetails)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TEntity>()
              .Property(a => a.Id);
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
                
                var tableName = this.GetTableName();
                var identityInsertBaseSqlQuery = $"SET IDENTITY_INSERT {tableName}";

                using (var transaction = Database.BeginTransaction())
                {
                    Database.ExecuteSqlRaw($"{identityInsertBaseSqlQuery} ON");
                    successfulInsertCount = base.SaveChanges();
                    Database.ExecuteSqlRaw($"{identityInsertBaseSqlQuery} OFF");
                    transaction.Commit();
                }

                return successfulInsertCount;
            }
            catch (DbUpdateException)
            {
                throw;
            }

        }
    }
}