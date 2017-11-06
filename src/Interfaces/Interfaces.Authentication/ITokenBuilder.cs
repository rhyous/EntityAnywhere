using Rhyous.Odata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// A token is used in the header of a web call to claim that authentication has already occurred.
    /// If a token must be built by an Authenticator plugin, then that token build should implement this interface.
    /// </summary>
    public interface ITokenBuilder
    {
        /// <summary>
        /// A method to build a Token.
        /// </summary>
        /// <param name="creds">The credentials (username and password) that the token is being built for.</param>
        /// <param name="userId">The id of the user that is requesting the token.</param>
        /// <returns>An IToken implementation. The Token and UserId properties must be set.</returns>
        Task<IToken> BuildAsync(ICredentials creds, IUser user, List<RelatedEntityCollection> relatedEntityCollections);
    }
}
