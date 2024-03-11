using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class ImpersonationHandler : IImpersonationHandler
    {
        private readonly IEntityClientAsync<UserRole, int> _UserRoleClient;
        private readonly IHeaders _Headers;
        private readonly IJWTToken _JwtToken;
        private readonly ITokenDecoder _TokenDecoder;

        public ImpersonationHandler(IEntityClientAsync<UserRole, int> userRoleClient,
                                    IHeaders headers,
                                    IJWTToken jwtToken,
                                    ITokenDecoder tokenDecoder)
        {
            _UserRoleClient = userRoleClient;
            _Headers = headers;
            _JwtToken = jwtToken;
            _TokenDecoder = tokenDecoder;
        }

        public async Task<Token> HandleAsync(int roleId)
        {
            if (roleId < 1)
                throw new RestException($"{nameof(roleId)} must be valid", HttpStatusCode.BadRequest);

            var tokenText = _Headers.Collection["Token"];
            if (string.IsNullOrWhiteSpace(tokenText))
                throw new RestException("Request contains no token", HttpStatusCode.BadRequest);

            var token = _TokenDecoder.Decode(tokenText);

            var newRole = (await _UserRoleClient.GetAsync(roleId))?.Object;
            var userRoleDomain = token.ClaimDomains.First(c => c.Subject == "UserRole");
            userRoleDomain.Claims.First(c => c.Name == "Role").Value = newRole.Name;

            if (userRoleDomain.Claims.Any(c => c.Name == "PortalLandingPageType"))
            {
                var plpt = (int)newRole.LandingPageId;
                userRoleDomain.Claims.First(c => c.Name == "PortalLandingPageType").Value = plpt.ToString();
            }

            token.Text = _JwtToken.GetTokenText(token.ClaimDomains);
            token.CreateDate = DateTimeOffset.Now;
            return token.ToConcrete<Token>();
        }
    }
}
