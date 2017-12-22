using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Rhyous.WebFramework.HeaderValidators
{
    /// <summary>
    /// This is the primary token validator used by Entity Anywhere framework. It uses the Token entity to maintain logged in state.
    /// In the web.config add a TokenTimeToLive value in the AppSettings section to specify the time to live of the token in seconds.
    ///     &lt;add key="TokenTimeToLive" value="86400" /&gt;
    /// </summary>
    public class TokenHeaderValidator : IHeaderValidator
    {
        /// <inheritdoc />
        public long UserId { get; set; }

        internal static long OneWeekInSeconds = 604800L;

        internal static TokenCache Cache = new TokenCache();

        /// <summary>
        /// Time to live of the token in seconds.
        /// Default token TimeToLive value: 1 week
        /// </summary>
        internal static long TimeToLive { get { return ConfigurationManager.AppSettings.Get("TokenTimeToLive", OneWeekInSeconds); } }

        /// <inheritdoc />
        public bool IsValid(NameValueCollection headers)
        {
            var tokenText = headers["Token"];
            if (string.IsNullOrWhiteSpace(tokenText))
                return false;
            Token token;
            if (!Cache.TryGetValue(tokenText, out token))            
            {
                token = TaskRunner.RunSynchonously(TokenService.GetAsync, tokenText)?.Object;
                if (token == null || IsExpired(token))
                    return false;
                Cache.Add(tokenText, token);
            }
            UserId = token.UserId;
            return true;
        }

        /// <inheritdoc />
        internal bool IsExpired(IToken token)
        {
            return token.CreateDate.AddSeconds(TimeToLive) < DateTime.Now;
        }

        #region Injectable
        internal EntityClientAsync<Token, long> TokenService
        {
            get { return _TokenService ?? (_TokenService = new EntityClientAdminAsync<Token, long>(true)); }
            set { _TokenService = value; }
        } private EntityClientAsync<Token, long> _TokenService;
        #endregion
    }
}
