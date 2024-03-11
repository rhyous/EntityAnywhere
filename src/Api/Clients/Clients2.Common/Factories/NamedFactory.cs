using Autofac;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class NamedFactory<T> : INamedFactory<T>
    {
        private readonly ILifetimeScope _Container;

        public NamedFactory(ILifetimeScope container)
        {
            _Container = container;
        }

        public T Create(string name)
        {
            return _Container.ResolveNamed<T>(name);
        }
    }
}