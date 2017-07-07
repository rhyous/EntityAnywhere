using System.Net;

namespace Rhyous.WebFramework.Authenticators
{
    public interface IActiveDirectoryService
    {
        bool ValidateCredentialsAgainstDomain(NetworkCredential credentials);
        bool IsUserInGroup(NetworkCredential credentials, string domain, string group);
    }
}