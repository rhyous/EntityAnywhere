using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IMappingEntityList
    {
        List<Type> Entities { get; }
        IEnumerable<string> EntityNames { get; }
    }
}
