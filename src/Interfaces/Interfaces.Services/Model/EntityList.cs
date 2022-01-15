using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class EntityList : IEntityList
    {
        public List<Type> Entities
        {
            get { return _Entities ?? (_Entities = new List<Type>()); }
        } private List<Type> _Entities;

        public IEnumerable<string> EntityNames => Entities?.Select(t => t.Name);
    }
}
