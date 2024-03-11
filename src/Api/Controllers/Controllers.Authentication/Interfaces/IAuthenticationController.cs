using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// A service contract for a custom web service, not related to an entity, for authentication.
    /// </summary>
    public interface IAuthenticationController
    {
        /// <summary>
        /// This method takes in a user name and password in a POSTed Credentials object and authenticates with them.
        /// </summary>
        /// <param name="creds">A Credentials object, which contains a user name and password.</param>
        /// <returns></returns>
        Task<Token> AuthenticateAsync(Credentials creds);
    }
}
