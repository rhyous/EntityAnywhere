using Rhyous.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Interfaces
{
    [ExcludeFromCodeCoverage]
    public class ClaimsList : ParentedList<Claim, ClaimDomain>
    {
        public ClaimsList() { }
        public ClaimsList(ClaimDomain parent) : base(parent) { }
    }
}
