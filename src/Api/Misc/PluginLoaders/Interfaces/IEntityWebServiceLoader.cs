using Rhyous.SimplePluginLoader;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityWebServiceLoader<TEntity, TWebService> : IRuntimePluginLoader<TWebService>
         where TWebService : class
    {
    }
}