using System;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface ITypeInfoResolver
    {
        ITypeInfo Resolve(Type type);
        ITypeInfo<T> Resolve<T>();
    }
}