using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Rhyous.WebFramework.Interfaces
{
    public class Claim : IParent<ClaimDomain>
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ClaimDomain Domain { get; set; }

        public string Subject
        {
            get { return _Subject ?? (_Subject = Domain?.Subject); }
            set { _Subject = value; }
        }
        private string _Subject;

        public string Issuer
        {
            get { return _Issuer ?? (_Issuer = Domain?.Issuer); }
            set { _Issuer = value; }
        } private string _Issuer;

        ClaimDomain IParent<ClaimDomain>.Parent { get { return Domain; } set { Domain = value; } }        
    }
}
