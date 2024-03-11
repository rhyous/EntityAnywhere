using Autofac;
using Rhyous.SimplePluginLoader.DependencyInjection;

namespace Rhyous.EntityAnywhere.Clients2.DependencyInjection
{
    internal class AuthenticationClientRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<AuthenticationClientModule>();
        }
    }
}