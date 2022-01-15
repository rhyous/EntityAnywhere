using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class ExtensionEntityList : IExtensionEntityList
    {
        public List<Type> Entities
        {
            get { return _ExtensionEntities ?? (_ExtensionEntities = new List<Type>()); }
        } private List<Type> _ExtensionEntities;

        public IEnumerable<string> EntityNames => Entities?.Select(t => t.Name);
    }
}
