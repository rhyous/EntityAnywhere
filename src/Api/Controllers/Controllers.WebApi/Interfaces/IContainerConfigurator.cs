using Autofac;

namespace Rhyous.EntityAnywhere.WebApi
{
    internal interface IContainerConfigurator
    {
        void Configure(ContainerBuilder builder);
    }
}