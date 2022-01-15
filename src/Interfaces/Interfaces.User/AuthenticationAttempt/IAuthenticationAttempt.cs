namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IAuthenticationAttempt : IBaseEntity<long>, IAuditable
    {
        /// <summary>
        /// The attempted username.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// The result of the login attempt:
        /// Allowed values: 
        /// Success - Any authentication attempt with valid creds.
        /// Failure - Any authentication attempt with invalid creds.
        /// Unknown - An exception occurred that had nothing to do with whether the creds are valid or invalid.
        /// </summary>
        string Result { get; set; }

        /// <summary>
        /// The IpAddress from which the authentication attempt originated.
        /// </summary>
        string IpAddress { get; set; }

        /// <summary>
        /// Allows for an admin to immediately unlock an account by marking failed attempts with the ignore flag.
        /// </summary>
        bool Ignore { get; set; }

        /// <summary>
        /// A place holder for an error message. Success messages will be blank, but failures can be logged.
        /// </summary>
        string Message { get; set; }
    }
}