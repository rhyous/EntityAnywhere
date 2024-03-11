using Autofac;

namespace Rhyous.EntityAnywhere.WebApi
{
    public class WebApiScopeContainerBuilder : IWebApiScopeContainerBuilder
    {

        public WebApiScopeContainerBuilder(ContainerBuilder containerBuilder)
        {
            ContainerBuilder = containerBuilder;
        }

        public ContainerBuilder ContainerBuilder { get; }

        public void Register(Type type)
        {
            ContainerBuilder.RegisterType(type);
        }
    }
}
