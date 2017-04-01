using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Configuration;
using System.Net;
using ICredentials = Rhyous.WebFramework.Interfaces.ICredentials;

namespace Rhyous.WebFramework.Authenticators
{
    public class ActiveDirectoryCredentialsValidator : ICredentialsValidator, ITokenBuilder
    {
        const string Domain = "Domain";
        const string DomainGroup = "DomainGroup";

        public IToken Build(ICredentials creds, long userId)
        {
            return TokenGenerator.Build(creds, userId);
        }

        public bool IsValid(ICredentials creds, out IToken token)
        {
            CleanUserName(creds);
            token = null;
            var domain = ConfigurationManager.AppSettings[Domain];
            var domainGroup = ConfigurationManager.AppSettings[DomainGroup];
            var netCreds = new NetworkCredential(creds.User, creds.Password, domain);
            if (ValidateMethod(netCreds, domainGroup))
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
                        Active = true
                    }
                }
            );
            if (users?.Count > 0)
                user = users[0];
            return user;
        }

        public void CleanUserName(ICredentials creds)
        {
            if (creds.User.Contains(@"\"))
                creds.User.Substring(creds.User.IndexOf(@"\") + 1);
            if (creds.User.Contains("@"))
                creds.User.Substring(0, creds.User.IndexOf("@"));
            return;
        }
        
        #region Lazy Injectables for unit tests
        public UserService UserService
        {
            get { return _UserService ?? (_UserService = new UserService()); }
            set { _UserService = value; }
        } private UserService _UserService;

        public TokenGenerator TokenGenerator
        {
            get { return _TokenGenerator ?? (_TokenGenerator = new TokenGenerator()); }
            set { _TokenGenerator = value; }
        } private TokenGenerator _TokenGenerator;


        public Func<NetworkCredential, string, bool> ValidateMethod
        {
            get { return _ValidateMethod ?? (_ValidateMethod = ActiveDirectoryManager.ValidateCredentials); }
            set { _ValidateMethod = value; }
        } private Func<NetworkCredential, string, bool> _ValidateMethod;

        #endregion
    }
}
