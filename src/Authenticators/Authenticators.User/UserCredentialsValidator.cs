using Rhyous.Odata;
using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Authenticators
{
    /// <summary>
    /// This is the primary login method as part of Entity Anywhere framework. This logs in using the User entity.
    /// </summary>
    public class UserCredentialsValidator : ICredentialsValidatorAsync, ITokenBuilder
    {
        /// <summary>
        /// If this is true, external users cannot login using this plugin.
        /// If this is false, users can login either with this plugin or with another plugin.
        /// </summary>
        public static bool ForceExternalUsersToAuthenticateExternally { get { return ConfigurationManager.AppSettings.Get("ForceExternalUsersToAuthenticateExternally", true); } }

        /// <inheritdoc />
        public async Task<IToken> BuildAsync(ICredentials creds, IUser user, List<RelatedEntityCollection> relatedEntityCollections, WebOperationContext context)
        {
            return await TokenGenerator.BuildAsync(creds, user, relatedEntityCollections, context);
        }

        /// <inheritdoc />
        public async Task<IToken> IsValidAsync(ICredentials creds, WebOperationContext context)
        {
            var userClient = ClientsCache.Generic.GetValueOrNew<EntityClientAsync<User, long>, WebOperationContext>(typeof(User).Name, context);
            var odataUser = await userClient.GetByAlternateKeyAsync(creds.User);
            var user = odataUser?.Object;
            if (user == null)
                return null;
            if (user.ExternalAuth && ForceExternalUsersToAuthenticateExternally)
                return null; // This will allow a different authenticator plugin to attempt authentication
            bool result = (user.IsHashed) ? Hash.Compare(creds.Password, user.Salt, user.Password, Hash.DefaultHashType, Hash.DefaultEncoding)
                                          : creds.Password == user.Password;

            var token = result ? await BuildAsync(creds, user, odataUser.RelatedEntities, context) : null;
            return token;
        }

        #region Injectable
        /// <summary>
        /// Used for both caching and reusing existing clients and is also used for dependency injection, for example, mocking in unit tests.
        /// </summary>
        internal IEntityClientCache ClientsCache
        {
            get { return _ClientsCache ?? (_ClientsCache = new EntityClientCache()); }
            set { _ClientsCache = value; }
        } private IEntityClientCache _ClientsCache;

        public ITokenBuilder TokenGenerator
        {
            get { return _TokenGenerator ?? (_TokenGenerator = new TokenGenerator()); }
            set { _TokenGenerator = value; }
        } private ITokenBuilder _TokenGenerator;

        #endregion
    }
}
