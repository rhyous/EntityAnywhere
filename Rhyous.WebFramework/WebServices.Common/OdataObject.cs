using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.WebFramework.WebServices
{
    [DataContract]
    public class OdataObject<T>
    {
        [DataMember(Order = 0)]
        public Uri Uri { get; set; }

        [DataMember(Order = 1)]
        public T @Object { get; set; }

        [DataMember(Order = 2)]
        public List<ODataUri> PropertyUris { get; set; }
    }
}
