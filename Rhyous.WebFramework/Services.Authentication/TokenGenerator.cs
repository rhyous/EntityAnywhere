using Rhyous.WebFramework.Interfaces;
using System;

namespace Rhyous.WebFramework.Services
{
    public class TokenGenerator : ITokenBuilder
    {
        public static int TokenSize = 100;

        public IToken Build(ICredentials creds, long userId = 0)
        {
            var tokenvalue = CryptoRandomString.GetCryptoRandomBase64String(TokenSize);
            IUser user = null;
            if (userId == 0)
            {
                user = UserService.Get(creds.User);
                if (user == null)
                    return null;
                userId = user.Id;
            }
            var token = new Token
            {
                Text = tokenvalue,
                CreateDate = DateTime.Now,
                UserId = userId
            };
            TokenService.Add(new[] { token });
            return token;
        }

        #region injectables
        public UserService UserService
        {
            get { return _UserService ?? (_UserService = new UserService()); }
            set { _UserService = value; }
        }
        private UserService _UserService;

        public TokenService TokenService
        {
            get { return _TokenService ?? (_TokenService = new TokenService()); }
            set { _TokenService = value; }
        }
        private TokenService _TokenService;
        #endregion
    }
}
