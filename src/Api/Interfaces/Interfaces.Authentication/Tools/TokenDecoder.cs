using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// The token decoder.
    /// </summary>
    public class TokenDecoder : ITokenDecoder
    {
        private readonly IJWTToken _JWTToken;
        private readonly IUserRoleEntityDataCache _UserRoleEntityDataCache;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="jWTToken"></param>
        /// <param name="userRoleEntityDataCache"></param>
        /// <remarks>Injecting IUserRoleEntityDataCache doesn't create an infinity loop here because UserRoleEntityDataCacheFactory uses the EntityAdminToken, so this
        /// code isn't reached when UserRoleEntityDataCacheFactory calls the UserRole web service.</remarks>
        public TokenDecoder(IJWTToken jWTToken,
                            IUserRoleEntityDataCache userRoleEntityDataCache)
        {
            _JWTToken = jWTToken;
            _UserRoleEntityDataCache = userRoleEntityDataCache;
        }

        public IToken Decode(string tokenText)
        {
            if (string.IsNullOrWhiteSpace(tokenText))
                throw new ArgumentException(nameof(tokenText));
            var decodedJWTToken = _JWTToken.DecodeToken(tokenText);
            var token = new Token { Text = tokenText, ClaimDomains = _JWTToken.GetClaimDomains(decodedJWTToken) };
            token.CredentialEntityId = token.GetClaimValue<long>("User", "Id");
            token.CreateDate = token.GetClaimValue<DateTimeOffset>("User", "LastAuthenticated");
            token.Role = token.GetClaimValue("UserRole", "Role");
            if (!string.IsNullOrWhiteSpace(token.Role) && _UserRoleEntityDataCache.UserRoleIds.TryGetValue(token.Role, out int userRoleId))
                token.RoleId = userRoleId;
            return token;
        }
    }
}