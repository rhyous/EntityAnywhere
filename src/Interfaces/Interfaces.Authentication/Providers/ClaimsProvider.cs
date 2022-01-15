using Rhyous.Collections;
using Rhyous.StringLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class ClaimsProvider : IClaimsProvider
    {
        private readonly IHeaders _Headers;
        private readonly IJWTToken _JwtToken;
        private readonly IAppSettings _AppSettings;
        private readonly IAdminClaimsProvider _AdminClaimsProvider;

        public ClaimsProvider(IHeaders headers,
                              IJWTToken jwtToken,
                              IAppSettings appSettings,
                              IAdminClaimsProvider adminClaimsProvider)
        {
            _Headers = headers;
            _JwtToken = jwtToken;
            _AppSettings = appSettings;
            _AdminClaimsProvider = adminClaimsProvider;
        }

        public List<ClaimDomain> ClaimDomains { get => _ClaimDomains ?? (_ClaimDomains = GetClaimDomainsMethod()); internal set => _ClaimDomains = value; }
        private List<ClaimDomain> _ClaimDomains;

        internal List<ClaimDomain> GetClaims()
        {
            var token = _Headers.Collection.Get("Token", "");
            if (string.IsNullOrWhiteSpace(token))
            {
                var providedAdminToken = _Headers.Collection.Get("EntityAdminToken", "");
                var configuredAdminToken = _AppSettings.Collection.Get("EntityAdminToken", "");
                return !string.IsNullOrWhiteSpace(providedAdminToken) && !string.IsNullOrWhiteSpace(configuredAdminToken) && providedAdminToken == configuredAdminToken
                    ? _AdminClaimsProvider.ClaimDomains
                    : null;
            } 

            var decoded = _JwtToken.DecodeToken(token);
            if (decoded == null)
                throw new Exception("The decoded token is invalid");

            return _JwtToken.GetClaimDomains(decoded);
        } 

        public string GetClaim(string claimSubject, string claimName)
        {
            var userClaimDomain = ClaimDomains?.FirstOrDefault(x => x.Subject == claimSubject);
            if (userClaimDomain is null || userClaimDomain.Claims is null || !userClaimDomain.Claims.Any())
                throw new Exception($"The token's {claimSubject} data is invalid");
            var usernameClaim = userClaimDomain.Claims.FirstOrDefault(c => c.Name == claimName);
            if (usernameClaim is null)
                throw new Exception($"The token's {claimSubject}.{claimName} data is invalid");
            return usernameClaim.Value;
        }

        public T GetClaim<T>(string claimSubject, string claimName)
        {
            var claimValue = GetClaim(claimSubject, claimName);
            if (string.IsNullOrWhiteSpace(claimValue))
                return default;
            return claimValue.To<T>();
        }

        [ExcludeFromCodeCoverage]
        internal Func<List<ClaimDomain>> GetClaimDomainsMethod
        {
            get { return _GetClaimDomainsMethod ?? (_GetClaimDomainsMethod = GetClaims); }
            set { _GetClaimDomainsMethod = value; }
        } private Func<List<ClaimDomain>> _GetClaimDomainsMethod;

    }
}