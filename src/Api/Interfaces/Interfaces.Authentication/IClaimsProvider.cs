using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IClaimsProvider
    {
        List<ClaimDomain> ClaimDomains { get; }
        string GetClaim(string claimSubject, string claimName);
        T GetClaim<T>(string claimSubject, string claimName);
    }
}