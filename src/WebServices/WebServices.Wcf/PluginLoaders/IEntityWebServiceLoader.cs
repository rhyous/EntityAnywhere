using Rhyous.SimplePluginLoader;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IEntityWebServiceLoader<TEntity, TWebService> : IRuntimePluginLoader<TWebService>
         where TWebService : class
    {
    }
}