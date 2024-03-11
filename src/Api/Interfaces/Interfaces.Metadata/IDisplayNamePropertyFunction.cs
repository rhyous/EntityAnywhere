using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IDisplayNamePropertyFunction
    {
        IEnumerable<KeyValuePair<string, object>> GetDisplayNameProperty(MemberInfo mi);
    }
}