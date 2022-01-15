//---------------------------------------------------------------------------
// Copyright (C) Ivanti Corporation 2020. All rights reserved.
//---------------------------------------------------------------------------

using Rhyous.StringLibrary;
using System.Security.Claims;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public class TokenFromClaimsBuilder : ITokenFromClaimsBuilder
    {
        public IAccessToken Build(ClaimsPrincipal claims)
        {
            var accessToken = new AccessToken();
            foreach (var claim in claims.Claims)
            {
                if (claim.Type.Equals("iss"))
                {
                    accessToken.Issuer = claim.Value;
                }
                if (claim.Type.Equals("aud"))
                {
                    accessToken.Audience = claim.Value;
                }
                if (claim.Type.Equals("exp"))
                {
                    accessToken.Expires = long.Parse(claim.Value);
                }
                if (claim.Type.Equals("nbf"))
                {
                    accessToken.NotBefore = long.Parse(claim.Value);
                }
                if (claim.Type.Equals("client_id"))
                {
                    accessToken.ClientId = claim.Value;
                }
                if (claim.Type.Equals("sub"))
                {
                    accessToken.Subject = claim.Value;
                }
                if (claim.Type.Equals("auth_time"))
                {
                    accessToken.AuthTime = claim.Value.To<long>();
                }
                if (claim.Type.Equals("idp"))
                {
                    accessToken.IdentityProvider = claim.Value;
                }
                if (claim.Type.Equals("user_id"))
                {
                    accessToken.UserId = claim.Value.To<long>();
                }
                if (claim.Type.Equals("user_role_id"))
                {
                    accessToken.UserRoleId = claim.Value.To<int>();
                }
            }
            return accessToken;
        }
    }
}
