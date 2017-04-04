using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors
{
    public static class TypeExtensions
    {
        public static List<Type> GetInterfaceInheritance(this Type type)
        {
            var interfaces = new List<Type>();
            foreach (Type parentInterface in type.GetInterfaces())
            {
                interfaces.Add(parentInterface);
                interfaces.AddRange(parentInterface.GetInterfaceInheritance());
            }
            return interfaces;
        }
    }
}
