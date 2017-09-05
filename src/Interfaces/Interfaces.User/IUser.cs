﻿namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The interface for a User entity.
    /// </summary>
    public partial interface IUser : IEntity<long>, IAuditable, IEnabled
    {
        /// <summary>
        /// The username used to login as this user.
        /// </summary>
        string Username { get; set; }
        /// <summary>
        /// The password used to login as this user.
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// The salt that is appended to the password before it is hashed.
        /// </summary>
        string Salt { get; set; }
        /// <summary>
        /// A bool value. If true, the password is salted and hashed. If false, the password is in clear text.
        /// </summary>
        bool IsHashed { get; set; }
        /// <summary>
        /// If true, this user was created by using an Authenticator plugin other than <see cref="Rhyous.WebFramework.Authenticators.User.UserCredentialsValidator"/> which authenticates using the User entity.
        /// If this is true, the password and salt should be null or empty and IsHashed should be ignored.
        /// However, a user might want to login using mutiple methods. Once a user is created, if ForceExternalUsersToAuthenticateExternally is false, then a user might be enabled to set their password, and log in externally or internally.
        /// </summary>
        bool ExternalAuth { get; set; }
    }
}
