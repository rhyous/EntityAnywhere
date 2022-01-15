using Autofac;
using Rhyous.SimplePluginLoader.DependencyInjection;

namespace Rhyous.WebFramework.WebServices.DependencyInjection
{
    internal class Registrar : IDependencyRegistrar<ContainerBuilder>
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<UserWebServiceModule>();
        }
    }
}