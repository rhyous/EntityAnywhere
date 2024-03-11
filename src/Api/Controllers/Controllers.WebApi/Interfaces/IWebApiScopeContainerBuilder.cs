using Autofac;

namespace Rhyous.EntityAnywhere.WebApi
{
    public interface IWebApiScopeContainerBuilder
    {
        public ContainerBuilder ContainerBuilder { get; }
        public void Register(Type type);
    }
}