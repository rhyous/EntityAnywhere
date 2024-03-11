using Newtonsoft.Json;
using Rhyous.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class Claim : IParent<ClaimDomain>
    {
        public string Name { get; set; }
        public string Value { get; set; }
        [ExcludeFromCodeCoverage]
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

        [ExcludeFromCodeCoverage]
        ClaimDomain IParent<ClaimDomain>.Parent { get { return Domain; } set { Domain = value; } }        
    }
}
