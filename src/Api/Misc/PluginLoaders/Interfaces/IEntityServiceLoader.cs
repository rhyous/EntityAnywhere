namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityServiceLoader<TServiceInterface, TEntity, TInterface, TId>
        where TServiceInterface : IServiceCommon<TEntity, TInterface, TId>
        where TEntity : class, TInterface
        where TInterface : IId<TId>
    {
        TServiceInterface LoadPlugin();
    }

    public interface IEntityServiceLoaderCommon<TServiceInterface, TEntity, TInterface, TId>
    where TServiceInterface : IServiceCommon<TEntity, TInterface, TId>
    where TEntity : class, TInterface
    where TInterface : IId<TId>
    {
        TServiceInterface LoadPlugin();
    }
}