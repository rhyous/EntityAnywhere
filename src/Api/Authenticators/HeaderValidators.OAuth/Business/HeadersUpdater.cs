using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    class HeadersUpdater : IHeadersUpdater
    {
        /// <summary>
        /// Thiis updates the headers to values from token to prevent a bad actor from trying to input invalid headers
        /// </summary>
        public void Update(IAccessToken token, IHeadersContainer headers)
        {
            headers.UpdateValue("UserId", token.UserId.ToString());
            headers.UpdateValue("Username", token.Subject);
        }
    }
}