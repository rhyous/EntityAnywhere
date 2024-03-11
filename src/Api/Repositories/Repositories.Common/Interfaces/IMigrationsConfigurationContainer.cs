using System.Data.Entity.Migrations;

namespace Rhyous.EntityAnywhere.Repositories
{
    public interface IMigrationsConfigurationContainer<TEntity> where TEntity : class
    {
        DbMigrationsConfiguration<BaseDbContext<TEntity>> Config { get; }
    }
}