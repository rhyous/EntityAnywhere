using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Odata;
using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// A class to generate an authentication token.
    /// </summary>
    public partial class TokenGenerator : ITokenBuilder
    {
        /// <summary>
        /// The size of the token string.
        /// </summary>
        public static int TokenSize = 100;

        /// <inheritdoc />
        public virtual async Task<IToken> BuildAsync(ICredentials creds, IUser user, List<RelatedEntityCollection> relatedEntityCollections)
        {
            if (user == null)
            {
                var userClient = ClientsCache.Generic.GetValueOrNew<EntityClientAsync<User, long>>(typeof(User).Name);
                var odataUser = await userClient.GetAsync(creds.User) ?? throw new Exception("User not found.");
                user = odataUser.Object;
                relatedEntityCollections = relatedEntityCollections ?? new List<RelatedEntityCollection>();
                relatedEntityCollections.AddRange(odataUser.RelatedEntities);
            }
            var tokenClient = ClientsCache.Generic.GetValueOrNew<EntityClientAsync<Token, long>, bool>(typeof(Token).Name, true);
            var token = new Token { Text = CryptoRandomString.GetCryptoRandomBase64String(TokenSize), UserId = user.Id };
            var odataToken = await tokenClient.PostAsync(new List<Token> { token });
            var claimConfigClient = ClientsCache.Generic.GetValueOrNew<EntityClientAsync<ClaimConfiguration, int>>(typeof(ClaimConfiguration).Name);
            var claimConfigs = await claimConfigClient.GetAllAsync();
            var claims = await ClaimsBuilder.BuildAsync(user, claimConfigs?.Select(c=>c.Object));
            if (claims != null && claims.Count > 0)
                token.ClaimDomains.AddRange(claims);
            Task.WaitAll();
            return token;
        }

        #region injectables
        internal IEntityClientCache ClientsCache
        {
            get { return _ClientsCache ?? (_ClientsCache = new EntityClientCache()); }
            set { _ClientsCache = value; }
        } private IEntityClientCache _ClientsCache;

        public IClaimsBuilderAsync ClaimsBuilder
        {
            get { return _ClaimsBuilder ?? (_ClaimsBuilder = new ClaimsBuilderAsync(ClientsCache)); }
            set { _ClaimsBuilder = value; }
        } private IClaimsBuilderAsync _ClaimsBuilder;
        #endregion
    }
}