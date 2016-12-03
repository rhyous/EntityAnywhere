using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Configuration;

namespace Rhyous.WebFramework.Authenticators
{
    public class TokenValidator : ITokenValidator
    {
        public IToken Token { get; set; }

        /// <summary>
        /// Time to live of the token in seconds
        /// </summary>
        public long TimeToLive { get { return ConfigurationManager.AppSettings.Get("TokenTimeToLive", 604800L); } }

        public bool IsValid(string tokenText)
        {
            Token = Service.Get(tokenText);
            return Token != null && !IsExpired(Token);
        }

        internal bool IsExpired(IToken token)
        {
            return token.CreateDate.AddSeconds(TimeToLive) < DateTime.Now;
        }

        #region Injectable
        public TokenService Service
        {
            get { return _Service ?? (_Service = new TokenService()); }
            set { _Service = value; }
        } private TokenService _Service;
        #endregion
    }
}
