using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    class HeadersUpdater : IHeadersUpdater
    {
        /// <summary>
        /// Thiis updates the headers to values from token to prevent a bad actor from trying to input invalid headers
        /// </summary>
        public void Update(IToken token, NameValueCollection headers)
        {
            var userId = token.GetClaimValue("User", "Id");
            var userName = token.GetClaimValue("User", "Username");
            var orgId = token.GetClaimValue("Organization", "Id");
            var sapId = token.GetClaimValue("Organization", "SapId");
            headers.UpdateValue("OrganizationId", orgId);
            headers.UpdateValue("UserId", userId);
            headers.UpdateValue("Username", userName);
            headers.UpdateValue("SapId", sapId);
        }
    }
}