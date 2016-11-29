using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public class UserService : SearchableServiceCommon<User, IUser>
    {
        public override IUser Get(string name)
        {
            return Repo.Get(name, u => u.Username);
        }

        public override List<IUser> Search(string name)
        {
            return Repo.Search(name, u => u.Username);
        }

        public override IUser Add(IUser user)
        {
            if (Get(user.Username) != null)
                throw new Exception("Duplicate name detected.");
            if (string.IsNullOrWhiteSpace(user.Salt))
            {
                user.Salt = Hash.Get(user.Username);
                if (string.IsNullOrWhiteSpace(user.Password))
                    user.Password = Hash.Get(CryptoRandomString.GetCryptoRandomAlphaNumericString(10), user.Salt);
                user.Password = Hash.Get(user.Password, user.Salt);
            }
            return Repo.Create(user);
        }
    }
}