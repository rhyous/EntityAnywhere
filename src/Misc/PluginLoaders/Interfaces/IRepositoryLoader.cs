namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IRepositoryLoader<TRepositoryInterface, TEntity, TInterface, TId>
    {
        TRepositoryInterface LoadPlugin();
    }
    public interface IRepositoryLoaderCommon<TRepositoryInterface, TEntity, TInterface, TId>
    {
        TRepositoryInterface LoadPlugin();
    }
}