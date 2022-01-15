using Autofac;
using Autofac.Builder;
using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IWebServiceRegistrar
    {
        IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType(Type implementationType);
    }
}