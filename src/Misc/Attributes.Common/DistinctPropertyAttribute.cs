using System;
using System.Runtime.CompilerServices;

namespace Rhyous.EntityAnywhere.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DistinctPropertyAttribute : EntityAttribute
    {
        public DistinctPropertyAttribute([CallerMemberName]string group = null, [CallerMemberName]string property = null)
        {
            Group = group;
            Property = property;
        }

        public string Group { get; }
        public string Property { get; }
    }
}
