using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
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
            var token = Service.Get(tokenText);
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
        public ServiceCommonAltId<Token, IToken, long> Service
        {
            get { return _Service ?? (_Service = new ServiceCommonAltId<Token, IToken, long>(x => x.Text)); }
            set { _Service = value; }
        } private ServiceCommonAltId<Token, IToken, long> _Service;
        #endregion
    }
}
