using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    class HeadersUpdater : IHeadersUpdater
    {
        /// <summary>
        /// Thiis updates the headers to values from token to prevent a bad actor from trying to input invalid headers
        /// </summary>
        public void Update(IToken token, IHeadersContainer headers)
        {
            var userId = token.GetClaimValue("User", "Id");
            var userName = token.GetClaimValue("User", "Username");
            headers.UpdateValue("UserId", userId);
            headers.UpdateValue("Username", userName);
        }
    }
}