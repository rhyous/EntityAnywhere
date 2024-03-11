using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IAdminClaimsProvider
    {
        List<ClaimDomain> ClaimDomains { get; }
    }
}