//---------------------------------------------------------------------------
// Copyright (C) Ivanti Corporation 2020. All rights reserved.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Jose;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    /// <summary>
    /// JWTValidator
    /// </summary>
    /// <remarks>We could break this up into individual validators for easier unit testing.</remarks>
    public class JWTValidator : IJWTValidator
    {
        public async Task<ClaimsPrincipal> ValidateAsync(string jwt)
        {
            var token = new JwtSecurityToken(jwt);

            ValidateIssuerUrl(token.Issuer);

            WebClient client = new WebClient();
            string response = await client.DownloadStringTaskAsync(token.Issuer + "/.well-known/openid-configuration");

            JObject config = JObject.Parse(response);
            //JsonReaderException
            var url = (string)config["jwks_uri"];
            if (url == null)
                throw new NullReferenceException(string.Format(CultureInfo.InvariantCulture, "URL must not be null"));

            response = await client.DownloadStringTaskAsync(url);

            var jwks = JObject.Parse(response);
            if (jwks == null)
                throw new NullReferenceException(string.Format(CultureInfo.InvariantCulture, "JWKS must not be null"));

            var keys = new List<SecurityKey>();
            var jkeys = jwks["keys"];
            if (jkeys == null)
                throw new NullReferenceException(string.Format(CultureInfo.InvariantCulture, "JKeys must not be null"));

            var jkey = jkeys.Where(x => (string)x["kid"] == token.Header.Kid);
            foreach (var k in jkey)
            {
                var e = Base64Url.Decode((string)k["e"]);
                var n = Base64Url.Decode((string)k["n"]);

                var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
                {
                    KeyId = (string)k["kid"]
                };

                keys.Add(key);
            }

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKeys = keys,
                RequireSignedTokens = true
            };

            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();

            return handler.ValidateToken(jwt, parameters, out var _);
        }

        /// <summary>
        /// Validate issuer came from ivanticloud.com or ivanticlouddev.com.  Add ISM domain to domain white list when needed
        /// </summary>
        /// <param name="issuer">th </param>
        private static void ValidateIssuerUrl(string issuer)
        {
            var domains = new List<string> { "ivanticloud.com", "ivanticlouddev.com", "http://localhost:5000" };
            var parts = issuer.Split('.');
            var revParts = parts.Reverse().Take(2).Reverse();
            var issuerDomain = string.Join(".", revParts);
            if (!domains.Contains(issuerDomain))
            {
                throw new SecurityTokenInvalidIssuerException($"Invalid issuer: {issuer}");
            }
        }
    }
}
