using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public class UserService : ServiceCommon<User, IUser>, ISearchableServiceCommon<User,IUser>
    {
        public IUser Get(string name)
        {
            return Repo.Get(name, u => u.Username);
        }

        public List<IUser> Search(string name)
        {
            return Repo.Search(name, u => u.Username);
        }

        public override List<IUser> Add(IList<IUser> users)
        {
            var duplicateUsernames = new List<string>();
            foreach (User user in users)
            {
                if (Get(user.Username) != null)
                {
                    duplicateUsernames.Add(user.Username);
                    continue;
                }
                if (string.IsNullOrWhiteSpace(user.Salt))
                {
                    user.Salt = Hash.Get(user.Username);
                }
                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    // Todo: Password notification email here. Maybe plugins for handling password (Creation, Resetting, etc.)
                    user.Password = Hash.Get(CryptoRandomString.GetCryptoRandomAlphaNumericString(10), user.Salt);
                }
                user.Password = Hash.Get(user.Password, user.Salt);
            }
            if (duplicateUsernames.Count > 0)
                throw new Exception("Duplicate username(s) detected: " + string.Join(", ", duplicateUsernames));
            return Repo.Create(users);
        }
    }
}