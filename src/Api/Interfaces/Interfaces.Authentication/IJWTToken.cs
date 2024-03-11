using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IJWTToken
    {
        string GetTokenText(IEnumerable<ClaimDomain> claimDomains);
        string CreateToken(IEnumerable<ClaimDomain> claimDomains, string privateRsaKey);
        string DecodeToken(string token);
        List<ClaimDomain> GetClaimDomains(string decodedToken);
    }
}