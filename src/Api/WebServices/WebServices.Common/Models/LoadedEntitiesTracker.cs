using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class LoadedEntitiesTracker : ILoadedEntitiesTracker
    {
        public HashSet<Type> Entities
        {
            get { return _Entities ?? (_Entities = new HashSet<Type>()); }
            set { _Entities = value; }
        } private HashSet<Type> _Entities;
    }
}