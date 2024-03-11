using System.Buffers.Text;
using System.Text;
using System.Security.Cryptography;
using System;
using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Rhyous.EntityAnywhere.Interfaces;
using Claim = System.Security.Claims.Claim;

namespace Rhyous.EntityAnywhere.Security
{
    public class AuthenticationTicketBuilder : IAuthenticationTicketBuilder
    {
        private readonly ITokenDecoder _TokenDecoder;

        public AuthenticationTicketBuilder(ITokenDecoder tokenDecoder)
        {
            _TokenDecoder = tokenDecoder;
        }

        /// <summary></summary>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        public AuthenticationTicket? Build(string tokenStr)
        {
            var token = _TokenDecoder.Decode(tokenStr);
            var userClaimDomain = token.ClaimDomains.FirstOrDefault(c => c.Subject.Equals("User", StringComparison.OrdinalIgnoreCase));
            var userClaim = userClaimDomain.Claims.FirstOrDefault(c => c.Name.Equals("Username", StringComparison.OrdinalIgnoreCase));
            if (userClaim == null)
                return null;
            return BuildByUsername(userClaim.Value);
        }

        /// <summary></summary>
        /// <returns></returns>
        public AuthenticationTicket BuildAdmin()
        {
            return BuildByUsername("Admin");
        }

        private static AuthenticationTicket BuildByUsername(string username)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, username)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, nameof(TokenAuthenticationHandler));
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return new AuthenticationTicket(claimsPrincipal, TokenAuthenticationSchemeOptions.Name);
        }
    }
}