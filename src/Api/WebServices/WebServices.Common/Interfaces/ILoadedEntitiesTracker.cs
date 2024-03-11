using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface ILoadedEntitiesTracker
    {
        HashSet<Type> Entities { get; set; }
    }
}