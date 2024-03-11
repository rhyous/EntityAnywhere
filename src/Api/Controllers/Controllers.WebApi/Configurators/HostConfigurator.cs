using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Rhyous.EntityAnywhere.WebApi
{
    public class HostConfigurator : IHostConfigurator
    {
        public void Configure(ConfigureHostBuilder hostBuilder, ILifetimeScope wepApiParentScope)
        {
            var webApiChildScope = new AutofacChildLifetimeScopeServiceProviderFactory(wepApiParentScope.BeginLifetimeScope("WebApiChild"));
            hostBuilder.UseServiceProviderFactory(webApiChildScope);

            // Register services directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory.
            var containerConfigurator = wepApiParentScope.Resolve<IContainerConfigurator>();
            hostBuilder.ConfigureContainer<AutofacChildLifetimeScopeConfigurationAdapter>(config =>
            {
                config.Add(builder => containerConfigurator.Configure(builder));
            });

        }
    }
}
