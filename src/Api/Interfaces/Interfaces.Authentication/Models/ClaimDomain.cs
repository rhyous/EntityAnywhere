using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class ClaimDomain
    {
        public string Subject { get; set; }
        public string Issuer { get; set; }
        [ExcludeFromCodeCoverage]
        public string OriginalIssuer { get; set; }
        public ClaimsList Claims
        {
            get { return _Claims ?? (_Claims = new ClaimsList(this)); }
            set { _Claims = value; }
        } private ClaimsList _Claims;
    }
}