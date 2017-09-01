using System.Net;

namespace Rhyous.WebFramework.Authenticators
{
    public interface IActiveDirectoryService
    {
        /// <summary>
        /// Validates credentials using type NetworkCredentials against a domain.
        /// </summary>
        /// <param name="credentials">NetworkCredentials containing username password and domain.</param>
        /// <returns>true if the credentials are valid, false otherwise.</returns>
        bool ValidateCredentialsAgainstDomain(NetworkCredential credentials);
        
        /// <summary>
        /// Verifies that the user is in the specified domain group.
        /// </summary>
        /// <param name="credentials">NetworkCredentials containing username password and domain.</param>
        /// <param name="domain">The domain name.</param>
        /// <param name="group">The domain group name.</param>
        /// <returns></returns>
        bool IsUserInGroup(NetworkCredential credentials, string domain, string group);
    }
}