using System;
using System.Data.Entity.Infrastructure;

namespace Rhyous.EntityAnywhere.Repositories
{
    public interface IAuditableDbContext : IDisposable
    {
        bool IsHistorical { get; set; }

        long UserId { get; }
        void HandleAuditables();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        int SaveChanges();
    }
}