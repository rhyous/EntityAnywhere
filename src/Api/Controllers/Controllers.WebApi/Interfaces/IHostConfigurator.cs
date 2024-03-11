
using Autofac;
using Microsoft.AspNetCore.Builder;

namespace Rhyous.EntityAnywhere.WebApi
{
    public interface IHostConfigurator
    {
        void Configure(ConfigureHostBuilder builder, ILifetimeScope wepApiParentScope);
    }
}