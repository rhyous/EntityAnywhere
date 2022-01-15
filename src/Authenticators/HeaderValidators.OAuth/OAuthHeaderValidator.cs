//---------------------------------------------------------------------------
// Copyright (C) Ivanti Corporation 2020. All rights reserved.
//---------------------------------------------------------------------------

using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    /// <summary>
    /// This is the primary token validator used by Entity Anywhere framework. It uses the Access Token entity to maintain logged in state.
    /// In the web.config add a TokenTimeToLive value in the AppSettings section to specify the time to live of the token in seconds.
    ///     &lt;add key="TokenTimeToLive" value="86400" /&gt;
    /// </summary>
    public class OAuthHeaderValidator : IHeaderValidator
    {
        private readonly IBearerDecoder _BearerDecoder;
        private readonly IEntityNameProvider _EntityNameProvider;
        private readonly IEntityPermissionChecker _EntityPermissionChecker;
        private readonly IHeadersUpdater _HeadersUpdater;
        private readonly ICustomCustomerRoleAuthorization _CustomCustomerRoleAuthorization;

        public OAuthHeaderValidator(IBearerDecoder bearerDecoder,
                                    IEntityNameProvider entityNameProvider,
                                    IEntityPermissionChecker entityPermissionChecker,
                                    IHeadersUpdater headersUpdater,
                                    ICustomCustomerRoleAuthorization customCustomerRoleAuthorization)
        {
            _BearerDecoder = bearerDecoder;
            _EntityNameProvider = entityNameProvider;
            _EntityPermissionChecker = entityPermissionChecker;
            _HeadersUpdater = headersUpdater;
            _CustomCustomerRoleAuthorization = customCustomerRoleAuthorization;
        }

        public long UserId { get; set; }

        public IList<string> Headers => new List<string> { "Bearer" };

        /// <inheritdoc />
        public async Task<bool> IsValidAsync(NameValueCollection headers)
        {
            var tokenText = headers["Bearer"];
            if (string.IsNullOrWhiteSpace(tokenText))
                return false;

            var token = await _BearerDecoder.DecodeAsync(tokenText);
            if (token == null)
                return false;

            UserId = token.UserId;
            return !IsExpired(token) && IsTokenVerified(token, headers);
        }

        internal bool IsTokenVerified(IAccessToken token, NameValueCollection headers)
        {

            if (token is null || token.UserRoleId < 1 || headers is null)
                return false;

            if (token.UserRoleId == WellknownUserRoleIds.Admin)
                return true;

            var absolutePath = headers.Get("AbsolutePath");
            if (string.IsNullOrWhiteSpace(absolutePath))
                return false;

            // Add values to the header so the service has it if needed
            _HeadersUpdater.Update(token, headers);

            var entityName = _EntityNameProvider.Provide(absolutePath);
            return _EntityPermissionChecker.HasPermission(token.UserRoleId, entityName) // Role-based Authorization - from configuration
                || _CustomCustomerRoleAuthorization.IsAuthorized(token, headers); // Temporary URL security for the customer roles, until we get authorization via roles, then we can remove this
        }

        internal bool IsExpired(IAccessToken token)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var addSeconds = epoch.AddSeconds(token.Expires);
            return DateTime.Compare(addSeconds, DateTime.Now) <= 0;
        }
    }
}