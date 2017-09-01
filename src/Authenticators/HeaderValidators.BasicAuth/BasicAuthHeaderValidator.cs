using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System.Collections.Specialized;

namespace Rhyous.WebFramework.HeaderValidators
{
    /// <summary>
    /// This token validator will validate Basic Auth headers. With this header validator, you can authenticate and call the service in one call.
    /// </summary>
    public class TokenValidator : IHeaderValidator
    {
        /// <inheritdoc />
        public long UserId { get; set; }

        /// <inheritdoc />
        public bool IsValid(NameValueCollection headers)
        {
            var basicAuthHeader = headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(basicAuthHeader))
            {
                var token = AuthService.Authenticate(new BasicAuth(basicAuthHeader).Creds);
                UserId = token.UserId;
                return true;
            }
            return false;
        }

        #region Injectable
        internal AuthenticationService AuthService
        {
            get { return _AuthService ?? (_AuthService = new AuthenticationService()); }
            set { _AuthService = value; }
        } private AuthenticationService _AuthService;
        #endregion
    }
}
