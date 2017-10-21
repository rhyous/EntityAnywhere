using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors.Dictionaries
{
    public class TypeSerializerDictionary : Dictionary<Type,Func<object, byte[]>>
    {
        public TypeSerializerDictionary()
        {
            Add(typeof(List<>), (a) => { return null; });
        }
    }
}
