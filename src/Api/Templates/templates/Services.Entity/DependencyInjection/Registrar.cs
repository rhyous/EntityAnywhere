using Autofac;
using Rhyous.SimplePluginLoader.DependencyInjection;

namespace Rhyous.EntityAnywhere.Services.DependencyInjection
{
    internal class Registrar : IDependencyRegistrar<ContainerBuilder>
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<Entity1ServiceModule>();
        }
    }
}