using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    /// <summary>
    /// This is the primary token validator used by Entity Anywhere framework. It uses the Token entity to maintain logged in state.
    /// In the web.config add a TokenTimeToLive value in the AppSettings section to specify the time to live of the token in seconds.
    ///     &lt;add key="TokenTimeToLive" value="86400" /&gt;
    /// </summary>
    public class TokenHeaderValidator : IHeaderValidator
    {
        internal static long OneWeekInSeconds = 604800L;

        private readonly ITokenDecoder _TokenDecoder;
        private readonly IAppSettings _AppSettings;
        private readonly IEntityNameProvider _EntityNameProvider;
        private readonly IEntityPermissionChecker _EntityPermissionChecker;
        private readonly IHeadersUpdater _HeadersUpdater;
        private readonly ICustomCustomerRoleAuthorization _CustomCustomerRoleAuthorization;

        public TokenHeaderValidator(ITokenDecoder tokenDecoder,
                                    IAppSettings appSettings,
                                    IEntityNameProvider entityNameProvider,
                                    IEntityPermissionChecker entityPermissionChecker,
                                    IHeadersUpdater headersUpdater,
                                    ICustomCustomerRoleAuthorization customCustomerRoleAuthorization)
        {
            _TokenDecoder = tokenDecoder;
            _AppSettings = appSettings;
            _EntityNameProvider = entityNameProvider;
            _EntityPermissionChecker = entityPermissionChecker;
            _HeadersUpdater = headersUpdater;
            _CustomCustomerRoleAuthorization = customCustomerRoleAuthorization;
        }

        /// <summary>
        /// Time to live of the token in seconds.
        /// Default token TimeToLive value: 1 week
        /// </summary>
        internal long TimeToLive { get { return _AppSettings.Collection.Get("TokenTimeToLive", OneWeekInSeconds); } }

        public long UserId { get; set; }

        public IList<string> Headers => new List<string> { "Token" };

        /// <inheritdoc />
        public Task<bool> IsValidAsync(IHeadersContainer headers)
        {
            var tokenText = headers.Get("Token", "");
            if (string.IsNullOrWhiteSpace(tokenText))
                return Task.FromResult(false);

            var token = _TokenDecoder.Decode(tokenText);
            if (token == null || string.IsNullOrWhiteSpace(token.Text))
                return Task.FromResult(false);

            UserId = token.CredentialEntityId;
            return Task.FromResult(!IsExpired(token) && IsTokenVerified(token, headers));
        }

        internal bool IsTokenVerified(IToken token, IHeadersContainer headers)
        {
            if (token is null || headers is null)
                return false;

            if (token.RoleId == WellknownUserRoleIds.Admin)
                return true;

            var absolutePath = headers.Get("AbsolutePath", "");
            if (string.IsNullOrWhiteSpace(absolutePath))
                return false;

            // Add values to the header so the service has it if needed
            _HeadersUpdater.Update(token, headers);

            var entityName = _EntityNameProvider.Provide(absolutePath);

            return _EntityPermissionChecker.HasPermission(token.RoleId, entityName) // Role-based Authorization - from configuration
                || _CustomCustomerRoleAuthorization.IsAuthorized(headers, token.RoleId); // Temporary URL security for the customer roles, until we get authorization via roles, then we can remove this
        }

        /// <inheritdoc />
        internal bool IsExpired(IToken token)
        {
            return token.CreateDate.AddSeconds(TimeToLive) < DateTimeOffset.Now;
        }
    }
}