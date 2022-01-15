namespace Rhyous.EntityAnywhere.Repositories
{
    public interface IEntityConnectionStringNameProvider<TEntity>
    {
        string Provide();
    }
}