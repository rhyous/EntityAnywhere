using Rhyous.Odata.Filter;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public class AccountLocker : IAccountLocker
    {
        private readonly IAdminEntityClientAsync<AuthenticationAttempt, long> AuthenticationAttemptClient;
        private readonly IAuthenticationSettings AuthenticationSettings;

        public AccountLocker(
            IAdminEntityClientAsync<AuthenticationAttempt, long> authenticationAttemptClient,
            IAuthenticationSettings authenticationSettings
        )
        {
            AuthenticationAttemptClient = authenticationAttemptClient;
            AuthenticationSettings = authenticationSettings;
        }

        public async Task<bool> IsLocked(Credentials creds)
        {
            var sinceDate = Uri.EscapeDataString(AuthenticationSettings.Start.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))).EnforceConstant<AuthenticationAttempt>();
            var user = Uri.EscapeDataString(creds.User.EnforceConstant<AuthenticationAttempt>());
            var queryParams = $"$filter=Username eq '{user}' and Result eq Failure and Ignore eq 0 and CreateDate gt \"{sinceDate}\"";
            var priorAttempts = (await AuthenticationAttemptClient.GetByQueryParametersAsync(queryParams))?.Select(o => o.Object);
            var recentPastAttemptCount = priorAttempts?.Count() ?? 0;
            return recentPastAttemptCount >= AuthenticationSettings.MaxFailedAttempts;
        }

        public async Task AddAttempt(AuthenticationAttempt attempt)
        {
            await AuthenticationAttemptClient.PostAsync(new[] { attempt }, false);
        }
    }
}
