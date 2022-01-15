using Rhyous.Collections;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services
{
    public class PasswordManager : IPasswordManager
    {
        private readonly IAppSettings _AppSettings;

        public PasswordManager(IAppSettings appSettings)
        {
            _AppSettings = appSettings;
        }

        public int DefaultPasswordLength => _AppSettings.Collection.Get("DefaultUserPasswordLength", 10);

        public void SetOrHashPassword(IUser user, bool passwordChanged)
        {
            if (user == null)
                return;
            if (user.ExternalAuth && string.IsNullOrWhiteSpace(user.Password))
                return;
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                user.Password = RandomStringCreatorMethod(DefaultPasswordLength, false);
                passwordChanged = true;
            }
            if (passwordChanged && user.IsHashed)
            {
                if (string.IsNullOrWhiteSpace(user.Salt))
                {
                    var unique = RandomStringCreatorMethod(DefaultPasswordLength, false);
                    user.Salt = Hash.Get(user.Username + unique);
                }
                user.Password = Hash.Get(user.Password, user.Salt);
            }
        }

        public void SetOrHashPassword(IEnumerable<IUser> users)
        {
            if (users == null)
                return;
            foreach (IUser user in users)
            {
                SetOrHashPassword(user, true);
            }
        }

        #region Injectables
        /// <summary>This Lazy func is for unit testing purposes.</summary>
        internal Func<int, bool, string> RandomStringCreatorMethod
        {
            get { return _RandomStringCreator ?? (_RandomStringCreator = CryptoRandomString.GetCryptoRandomAlphaNumericString); }
            set { _RandomStringCreator = value; }
        } private Func<int, bool, string> _RandomStringCreator;
        #endregion
    }
}