using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Rhyous.EntityAnywhere.Repositories
{
    public interface IAuditableDbContext : IDisposable
    {
        bool IsHistorical { get; set; }

        long UserId { get; }
        void HandleAuditables();

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        int SaveChanges();
    }
}