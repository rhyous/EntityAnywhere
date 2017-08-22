using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Interfaces
{
    public class AdditionalServiceTypes : Attribute, IAdditionalTypes
    {
        public AdditionalServiceTypes(params Type[] types) { }
        public List<Type> Types
        {
            get { return _Types ?? (_Types = new List<Type>()); }
            set { _Types = value; }
        } private List<Type> _Types;
    }
}
