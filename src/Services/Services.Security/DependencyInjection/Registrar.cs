using Autofac;
using Rhyous.SimplePluginLoader.DependencyInjection;

namespace Rhyous.WebFramework.Services.Security.DependencyInjection
{
    /// <summary>
    /// Responsible for providing the <see cref="ActivationCredentialWebServiceModule"/>
    /// </summary>
    internal class Registrar : IDependencyRegistrar<ContainerBuilder>
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<SecurityModule>();
        }
    }
}
