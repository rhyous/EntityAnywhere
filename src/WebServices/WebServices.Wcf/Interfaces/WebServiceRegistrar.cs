using Autofac;
using Autofac.Builder;
using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    class WebServiceRegistrar : IWebServiceRegistrar
    {
        private readonly ContainerBuilder _ContainerBuilder;

        public WebServiceRegistrar(ContainerBuilder containerBuilder)
        {
            _ContainerBuilder = containerBuilder;
        }
        public IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType(Type implementationType)
            => _ContainerBuilder.RegisterType(implementationType);
    }
}