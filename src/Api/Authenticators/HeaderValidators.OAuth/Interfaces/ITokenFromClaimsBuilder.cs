using System.Security.Claims;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface ITokenFromClaimsBuilder { IAccessToken Build(ClaimsPrincipal claims); }
}
