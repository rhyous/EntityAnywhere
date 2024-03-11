using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Rhyous.EntityAnywhere.Repositories
{
    public interface IBaseDbContext<TEntity> : IAuditableDbContext
        where TEntity : class
    {
        DbSet<TEntity> Entities { get; set; }
        //void SetConfig(bool proxyCreationEnabled = false, bool lazyLoadingEnabled = false, bool asNoTracking = true);
        void SetIsModified(TEntity entity, string prop, bool isModified);
        //DbContextConfiguration Configuration { get; }
        DatabaseFacade Database { get; }
        IModel Model { get; }
    }
}