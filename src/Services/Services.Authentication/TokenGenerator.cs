using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// A class to generate an authentication token.
    /// </summary>
    public class TokenGenerator : ITokenBuilder
    {
        /// <summary>
        /// The size of the token string.
        /// </summary>
        public static int TokenSize = 100;

        /// <inheritdoc />
        public IToken Build(ICredentials creds, long userId = 0)
        {
            if (userId < 1)
            {
                userId = UserService.Get(creds.User)?.Object?.Id ?? throw new Exception("User not found.");
            }
            var token = new Token { Text = CryptoRandomString.GetCryptoRandomBase64String(TokenSize), UserId = userId };
            TokenService.Post(new List<Token> { token });
            return token;
        }

        #region injectables
        internal EntityClient<User, int> UserService
        {
            get { return _UserService ?? (_UserService = new EntityClient<User, int>()); }
            set { _UserService = value; }
        } private EntityClient<User, int> _UserService;

        internal EntityClient<Token, long> TokenService
        {
            get { return _TokenService ?? (_TokenService = new EntityClient<Token, long>(true)); }
            set { _TokenService = value; }
        } private EntityClient<Token, long> _TokenService;
        #endregion
    }
}
