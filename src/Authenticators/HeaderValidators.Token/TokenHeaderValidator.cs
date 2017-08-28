using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Rhyous.WebFramework.HeaderValidators
{
    public class TokenHeaderValidator : IHeaderValidator
    {
        public long UserId { get; set; }

        /// <summary>
        /// Time to live of the token in seconds
        /// </summary>
        public long TimeToLive { get { return ConfigurationManager.AppSettings.Get("TokenTimeToLive", 604800L); } }

        public bool IsValid(NameValueCollection headers)
        {
            var tokenText = headers["Token"];
            var token = TokenService.Get(tokenText)?.Object;
            if (token == null || IsExpired(token))
                return false;
            UserId = token.UserId;
            return true;
        }

        internal bool IsExpired(IToken token)
        {
            return token.CreateDate.AddSeconds(TimeToLive) < DateTime.Now;
        }

        #region Injectable
        public EntityClient<Token, long> TokenService
        {
            get { return _TokenService ?? (_TokenService = new EntityClient<Token, long>(true)); }
            set { _TokenService = value; }
        } private EntityClient<Token, long> _TokenService;
        #endregion
    }
}
