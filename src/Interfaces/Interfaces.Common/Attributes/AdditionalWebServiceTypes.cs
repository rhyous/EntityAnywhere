using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    public class AdditionalWebServiceTypes : Attribute, IAdditionalTypes
    {
        public AdditionalWebServiceTypes(params Type[] types) { _Types = types.ToList(); }
        public List<Type> Types
        {
            get { return _Types ?? (_Types = new List<Type>()); }
            set { _Types = value; }
        } private List<Type> _Types;
    }
}
