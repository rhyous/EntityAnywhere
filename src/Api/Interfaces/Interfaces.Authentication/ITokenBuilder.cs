using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// A token is used in the header of a web call to claim that authentication has already occurred.
    /// If a token must be built by an Authenticator plugin, then that token build should implement this interface.
    /// </summary>
    public interface ITokenBuilder<ICredentialEntity>
    {
        /// <summary>
        /// A method to build a Token.
        /// </summary>
        /// <param name="creds">The credentials (username and password) that the token is being built for.</param>
        /// <param name="credentialEntity">The credential entity (usually a user) that is requesting the token.</param>
        /// <returns>An IToken implementation. The Token and UserId properties must be set.</returns>
        Task<IToken> BuildAsync(ICredentials creds, ICredentialEntity credentialEntity);
    }
}
