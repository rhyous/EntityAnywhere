using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// This class allows for the properties to be accessed from any Type.
    /// </summary>
    /// <typeparam name="Type">The generic type</typeparam>
    /// <remarks>If this is registered as a singleton, then properties are only ever loaded once per app lifetime.</remarks>
    public class EntityInfo<Type> : IEntityInfo<Type>
    {
        public IDictionary<string, PropertyInfo> Properties => _Properties ?? (_Properties = GetProperties());
        private IDictionary<string, PropertyInfo> _Properties;

        private IDictionary<string, PropertyInfo> GetProperties()
        {
            // Entities can't have two properties where the only difference in property name is the case
            return typeof(Type).GetProperties().ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);
        }
    }
}