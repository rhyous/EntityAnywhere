using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Rhyous.EntityAnywhere.Repositories
{
    public interface IBaseDbContext<TEntity> : IAuditableDbContext
        where TEntity : class
    {
        DbSet<TEntity> Entities { get; set; }
        void SetConfig(bool proxyCreationEnabled = false, bool lazyLoadingEnabled = false, bool asNoTracking = true);
        void SetIsModified(TEntity entity, string prop, bool isModified);
        DbContextConfiguration Configuration { get; }
        Database Database { get; }
    }
}