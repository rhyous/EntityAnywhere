using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    public partial class UserService : ServiceCommonAlternateKey<User, IUser, long>
    {
        public override Expression<Func<User, string>> PropertyExpression => e => e.Username;

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