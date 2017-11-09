using Rhyous.Odata;
using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using ICredentials = Rhyous.WebFramework.Interfaces.ICredentials;

namespace Rhyous.WebFramework.Authenticators
{
    /// <summary>
    /// A credentials validator that takes in a username and password and connects to Active Directory to see if it is valid.
    /// This requires some AppSettings in the web.config to work.
    ///     &lt;add key="Domain" value="domain.tld" /&gt;
    ///     &lt;add key="DomainGroup" value="AdGroup1" /&gt;
    /// </summary>
    public class ActiveDirectoryCredentialsValidator : ICredentialsValidatorAsync, ITokenBuilder
    {
        const string Domain = "Domain";
        const string DomainGroup = "DomainGroup";

        /// <inheritdoc />
        public async Task<IToken> BuildAsync(ICredentials creds, IUser user, List<RelatedEntityCollection> relatedEntityCollections)
        {
            return await TokenGenerator.BuildAsync(creds, user, relatedEntityCollections);
        }

        /// <inheritdoc />
        public async Task<IToken> IsValidAsync(ICredentials creds)
        {
            var domain = ConfigurationManager.AppSettings.Get<string>("Domain", null);
            if (string.IsNullOrWhiteSpace(domain))
                throw new Exception("The 'Domain' appsetting value in the web.config must be populated.");

            var group = ConfigurationManager.AppSettings.Get<string>("DomainGroup", null);
            if (string.IsNullOrWhiteSpace(group))
                throw new Exception("The 'DomainGroup' appsetting value in the web.config must be populated.");

            var userDomain = GetDomain(creds.User) ?? domain;
            var netCreds = new NetworkCredential(GetUserName(creds.User), creds.Password, userDomain);
            if (ADService.ValidateCredentialsAgainstDomain(netCreds) && ADService.IsUserInGroup(netCreds, domain, group))
            {
                var userClient = ClientsCache.Generic.GetValueOrNew<EntityClientAsync<User, long>>(typeof(User).Name);
                var odataUser = await userClient.GetAsync(creds.User);
                IUser user = odataUser?.Object;
                if (user == null)
                    user = await StoreUser(creds);
                return await BuildAsync(creds, user, odataUser.RelatedEntities);
            }
            return null;
        }

        internal async Task<IUser> StoreUser(ICredentials creds)
        {
            IUser user = null;
            var userClient = ClientsCache.Generic.GetValueOrNew<EntityClientAsync<User, long>>(typeof(User).Name);
            var users = await userClient.PostAsync(
            // Do not store the password at all.            
            new List<User> { new User { Username = creds.User, ExternalAuth = true, Enabled = true } });
            if (users?.Count > 0)
                user = users[0].Object;
            return user;
        }

        internal string GetDomain(string username)
        {
            if (username.Contains(@"\"))
                return username.Substring(0, username.IndexOf(@"\"));
            return ConfigurationManager.AppSettings["Domain"];
        }

        internal string GetUserName(string username)
        {
            if (username.Contains(@"\"))
                return username.Substring(username.IndexOf(@"\") + 1);
            if (username.Contains("@"))
                return username.Substring(0, username.IndexOf("@"));
            return username;
        }

        #region Lazy Injectables that can be mocked or replaced for unit tests
        internal IActiveDirectoryService ADService
        {
            get { return _ADService ?? (_ADService = new ActiveDirectoryService()); }
            set { _ADService = value; }
        } private IActiveDirectoryService _ADService;
        
        /// <summary>
        /// Used for both caching and reusing existing clients and is also used for dependency injection, for example, mocking in unit tests.
        /// </summary>
        internal IEntityClientCache ClientsCache
        {
            get { return _ClientsCache ?? (_ClientsCache = new EntityClientCache()); }
            set { _ClientsCache = value; }
        } private IEntityClientCache _ClientsCache;

        internal ITokenBuilder TokenGenerator
        {
            get { return _TokenGenerator ?? (_TokenGenerator = new TokenGenerator()); }
            set { _TokenGenerator = value; }
        } private ITokenBuilder _TokenGenerator;
        #endregion
    }
}