using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.BusinessRules
{
    [DataContract]
    [Serializable]
    public class BusinessRuleResult
    {
        [DataMember]
        public bool Result { get; set; }
        [DataMember]
        public List<object> FailedObjects
        {
            get { return _FailedObjects ?? (_FailedObjects = new List<object>()); }
            set { _FailedObjects = value; }
        } private List<object> _FailedObjects;
    }
}
