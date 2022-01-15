namespace Rhyous.EntityAnywhere.Repositories
{
    public interface IUpdateDbContext<TEntity> : IBaseDbContext<TEntity>
        where TEntity : class
    {
    }
}
