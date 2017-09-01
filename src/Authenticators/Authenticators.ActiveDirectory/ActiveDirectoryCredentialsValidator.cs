﻿using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Configuration;
using System.Net;
using ICredentials = Rhyous.WebFramework.Interfaces.ICredentials;

namespace Rhyous.WebFramework.Authenticators
{
    /// <summary>
    /// A credentials validator that takes in a username and password and connects to Active Directory to see if it is valid.
    /// This requires some AppSettings in the web.config to work.
    ///     &lt;add key="Domain" value="domain.tld" /&gt;
    ///     &lt;add key="DomainGroup" value="AdGroup1" /&gt;
    /// </summary>
    public class ActiveDirectoryCredentialsValidator : ICredentialsValidator, ITokenBuilder
    {
        const string Domain = "Domain";
        const string DomainGroup = "DomainGroup";

        /// <inheritdoc />
        public IToken Build(ICredentials creds, long userId)
        {
            return TokenGenerator.Build(creds, userId);
        }

        /// <inheritdoc />
        public bool IsValid(ICredentials creds, out IToken token)
        {
            token = null;
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
                var user = UserService.Get(creds.User);
                if (user == null)
                    user = StoreUser(creds);
                token = Build(creds, user.Id);
                return true;
            }
            return false;
        }

        private IUser StoreUser(ICredentials creds)
        {
            IUser user = null;
            var users = UserService.Add(
                new[] {
                    new User {
                        Username = creds.User,
                        Password = creds.Password,
                        ExternalAuth = true,
                        Enabled = true
                    }
                }
            );
            if (users?.Count > 0)
                user = users[0];
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

        internal UserService UserService
        {
            get { return _UserService ?? (_UserService = new UserService()); }
            set { _UserService = value; }
        } private UserService _UserService;

        internal TokenGenerator TokenGenerator
        {
            get { return _TokenGenerator ?? (_TokenGenerator = new TokenGenerator()); }
            set { _TokenGenerator = value; }
        } private TokenGenerator _TokenGenerator;

        #endregion
    }
}
