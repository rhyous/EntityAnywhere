using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The contract that all header validator plugins must implement. Usually a token is passed in a header to avoid constant authentication.
    /// </summary>
    public interface IHeaderValidator
    {
        /// <summary>
        /// The Id of the logged in user
        /// </summary>
        long UserId { get; set; }

        /// <summary>
        /// A list of headers from a web call.
        /// </summary>
        /// <param name="headers">The NameValueCollection of all headers included in a web call.</param>
        /// <returns>True or the header is valid and not expired. False otherwise.</returns>
        Task<bool> IsValidAsync(IHeadersContainer headers);

        /// <summary>
        /// The headers this validator will validate.
        /// If a call doesn't have any of these headers, then this validator shouldn't be called.
        /// If a HandledHeaders is null or empty, it will be called always.
        /// </summary>
        /// <remarks></remarks>
        IList<string> Headers { get; }
    }
}