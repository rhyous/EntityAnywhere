namespace Rhyous.EntityAnywhere.Repositories
{
    public interface IEntityConnectionStringProvider<TEntity>
    {
        string Provide();
    }
}