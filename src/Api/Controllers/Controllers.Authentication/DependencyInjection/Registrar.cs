using Autofac;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Services.DependencyInjection;

namespace Rhyous.EntityAnywhere.WebServices.DependencyInjection
{
    internal class Registrar : IDependencyRegistrar<ContainerBuilder>
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<TokenModule>();
            containerBuilder.RegisterModule<AuthenticationServiceModule>();
            containerBuilder.RegisterModule<AuthenticationWebServiceModule>();
        }
    }
}