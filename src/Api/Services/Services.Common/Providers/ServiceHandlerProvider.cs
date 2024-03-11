using Autofac;

namespace Rhyous.EntityAnywhere.Services
{
    public class ServiceHandlerProvider : IServiceHandlerProvider
    {
        private readonly ILifetimeScope _Container;

        public ServiceHandlerProvider(ILifetimeScope container)
        {
            _Container = container;
        }

        public T Provide<T>() => _Container.Resolve<T>();
    }
}
