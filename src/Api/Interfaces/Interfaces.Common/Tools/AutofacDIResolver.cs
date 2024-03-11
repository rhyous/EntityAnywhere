using Autofac;
using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>An Autofac implementation of a dependency injection resolver.</summary>
    [ExcludeFromCodeCoverage]
    public class AutofacDIResolver : IDependencyInjectionResolver
    {
        private readonly ILifetimeScope _Scope;

        public AutofacDIResolver(ILifetimeScope scope)
        {
            _Scope = scope;
        }
        public T Resolve<T>()
        {
            return _Scope.Resolve<T>();
        }
    }
}