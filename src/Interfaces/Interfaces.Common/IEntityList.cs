using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityList
    {
        List<Type> Entities { get; }
        IEnumerable<string> EntityNames { get; }
    }
}
