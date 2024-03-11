using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityInfo<TEntity> : ITypeInfo<TEntity> { }

    public interface ITypeInfo<TEntity> : ITypeInfo { }

    public interface ITypeInfo
    {
        IDictionary<string, PropertyInfo> Properties { get; }
    }
}