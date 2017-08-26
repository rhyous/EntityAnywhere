using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Entities;
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
                user = UserService.Get(creds.User).Object;
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
        public EntityClient<User, int> UserService
        {
            get { return _UserService ?? (_UserService = new EntityClient<User, int>()); }
            set { _UserService = value; }
        } private EntityClient<User, int> _UserService;

        public ServiceCommonAlternateKey<Token, IToken, long> TokenService
        {
            get { return _TokenService ?? (_TokenService = new ServiceCommonAlternateKey<Token, IToken, long>(x => x.Text)); }
            set { _TokenService = value; }
        } private ServiceCommonAlternateKey<Token, IToken, long> _TokenService;
        #endregion
    }
}
