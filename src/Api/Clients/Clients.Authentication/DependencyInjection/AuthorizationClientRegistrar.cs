using Autofac;
using Rhyous.SimplePluginLoader.DependencyInjection;

namespace Rhyous.EntityAnywhere.Clients2.DependencyInjection
{
    internal class AuthorizationClientRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<AuthorizationClientModule>();
        }
    }
}