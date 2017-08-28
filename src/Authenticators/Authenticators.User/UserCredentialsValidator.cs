using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System.Configuration;

namespace Rhyous.WebFramework.Authenticators
{
    public class UserCredentialsValidator : ICredentialsValidator, ITokenBuilder
    {
        /// <summary>
        /// If this is true, users cannot login using this plugin.
        /// If this is false, users can login either with this plugin or with another plugin.
        /// </summary>
        public static bool ForceExternalUsersToAuthenticateExternally { get { return ConfigurationManager.AppSettings.Get("ForceExternalUsersToAuthenticateExternally", true); } }

        public IToken Build(ICredentials creds, long userId)
        {
            return TokenGenerator.Build(creds, userId);
        }

        public bool IsValid(ICredentials creds, out IToken token)
        {
            token = null;
            var user = Service.Get(creds.User)?.Object;
            if (user == null)
                return false;
            if (user.ExternalAuth && ForceExternalUsersToAuthenticateExternally)
                return false; // This will allow a different authenticator plugin to attempt authentication
            bool result = (user.IsHashed) ? Hash.Compare(creds.Password, user.Salt, user.Password, Hash.DefaultHashType, Hash.DefaultEncoding)
                                          : creds.Password == user.Password;

            token = result ? Build(creds, user.Id) : null;
            return result;
        }

        #region Injectable
        public EntityClient<User, int> Service
        {
            get { return _Service ?? (_Service = new EntityClient<User, int>()); }
            set { _Service = value; }
        } private EntityClient<User, int> _Service;

        public TokenGenerator TokenGenerator
        {
            get { return _TokenGenerator ?? (_TokenGenerator = new TokenGenerator()); }
            set { _TokenGenerator = value; }
        } private TokenGenerator _TokenGenerator;

        #endregion
    }
}
