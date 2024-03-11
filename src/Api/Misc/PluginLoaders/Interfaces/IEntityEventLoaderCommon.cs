namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventLoaderCommon<TEntity, TId>
        where TEntity : class, IId<TId>
    {
        string PluginSubFolder { get; }

        IEntityEventAll<TEntity, TId> LoadPlugins();
    }
}