using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Authenticators
{
    public interface IClaimsBuilder
    {
        Task<List<ClaimDomain>> BuildAsync(IActivationCredential cred);
        Task<ClaimDomain> BuildOrganizationClaimAsync(int organizationId);
        ClaimDomain BuildRolesClaim();
        ClaimDomain BuildUserClaims(string username, string primaryIdentifier, DateTimeOffset lastAuthenticated);
    }
}