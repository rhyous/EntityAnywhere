namespace Rhyous.WebFramework.Interfaces
{
    public class ClaimDomain
    {
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string OriginalIssuer { get; set; }
        public ClaimsList Claims
        {
            get { return _Claims ?? (_Claims = new ClaimsList(this)); }
            set { _Claims = value; }
        } private ClaimsList _Claims;
    }
}