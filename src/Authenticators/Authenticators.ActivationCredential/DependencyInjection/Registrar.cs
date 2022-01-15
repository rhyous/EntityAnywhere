using Autofac;
using Rhyous.SimplePluginLoader.DependencyInjection;

namespace Rhyous.WebFramework.Authenticators.DependencyInjection
{
    internal class Registrar : IDependencyRegistrar<ContainerBuilder>
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<ActivationCredentialsValidatorModule>();
        }
    }
}