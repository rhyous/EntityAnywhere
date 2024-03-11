using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IExtensionEntityList
    {
        List<Type> Entities { get; }
        IEnumerable<string> EntityNames { get; }

        bool ShouldAutoExpand(string name);
    }
}
