using System.Collections.Specialized;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The contract that all header validator plugins must implement. Usually a token is passed in a header to avoid constant authentication.
    /// </summary>
    public interface IHeaderValidator
    {
        /// <summary>
        /// A list of headers from a web call.
        /// </summary>
        /// <param name="headers">The NameValueCollection of all headers included in a web call.</param>
        /// <returns>True or the header is valid and not expired. False otherwise.</returns>
        bool IsValid(NameValueCollection headers);

        /// <summary>
        /// This must be populated by the IsValid method if true is returned. This is the UserId associated with the valid header.
        /// </summary>
        long UserId { get; set; }
    }
}