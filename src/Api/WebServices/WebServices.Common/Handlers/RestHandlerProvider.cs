using Autofac;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class RestHandlerProvider : IRestHandlerProvider
    {
        private readonly ILifetimeScope _Container;

        public RestHandlerProvider(ILifetimeScope container)
        {
            _Container = container;
        }

        public T Provide<T>() => _Container.Resolve<T>();
    }
}