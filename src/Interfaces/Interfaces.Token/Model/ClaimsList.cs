namespace Rhyous.WebFramework.Interfaces
{
    public class ClaimsList : ParentedList<Claim, ClaimDomain>
    {
        public ClaimsList() { }
        public ClaimsList(ClaimDomain parent) : base(parent) { }
    }
}
