using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityInfo<TEntity>
    {
        IDictionary<string, PropertyInfo> Properties { get; }
    }
}