using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Tools
{
    public class TypeInfoResolver : ITypeInfoResolver
    {
        private readonly IDependencyInjectionResolver _DIResolver;

        public TypeInfoResolver(IDependencyInjectionResolver diResolver)
        {
            _DIResolver = diResolver;
        }

        public ITypeInfo Resolve(Type type)
        {
            var mi = GetType().GetMethod(nameof(ITypeInfoResolver.Resolve), new Type[] { });
            var gmi = mi.MakeGenericMethod(type);
            var entityInfo = gmi.Invoke(this, null) as ITypeInfo;
            return entityInfo;
        }

        public ITypeInfo<T> Resolve<T>()
        {
            return _DIResolver.Resolve<ITypeInfo<T>>();
        }
    }
}