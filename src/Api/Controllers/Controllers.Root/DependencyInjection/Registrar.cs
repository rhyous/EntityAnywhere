using Autofac;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Services;

namespace Rhyous.EntityAnywhere.WebServices.DependencyInjection
{
    internal class Registrar : IDependencyRegistrar<ContainerBuilder>
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<RootWebServiceModule>();
            containerBuilder.RegisterModule<MetadataServiceModule>();
        }
    }
}