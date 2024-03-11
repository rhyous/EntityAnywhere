using Rhyous.SimplePluginLoader;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface ICustomWebServiceLoader : IRuntimePluginLoader<ICustomWebService>
    {
        void Load();
    }
}