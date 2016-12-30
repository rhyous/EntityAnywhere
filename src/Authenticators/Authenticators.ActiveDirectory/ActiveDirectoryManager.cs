using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;

namespace Rhyous.WebFramework.Authenticators
{
    class ActiveDirectoryManager
    {
        private const int ErrorLogonFailure = 0x31;

        public static bool ValidateCredentials(NetworkCredential credentials, string domainGroup)
        {
            var id = new LdapDirectoryIdentifier(credentials.Domain);
            using (var connection = new LdapConnection(id, credentials, AuthType.Kerberos))
            {
                connection.SessionOptions.Sealing = true;
                connection.SessionOptions.Signing = true;
                try
                {
                    connection.Bind();
                }
                catch (LdapException lEx)
                {
                    if (ErrorLogonFailure == lEx.ErrorCode)
                    {
                        return false;
                    }
                    throw;
                }
                if (string.IsNullOrWhiteSpace(domainGroup))
                {
                    return IsUserInGroup(credentials, domainGroup);
                }
            }
            return true;
        }

        internal static bool IsUserInGroup(NetworkCredential credentials, string domainGroup)
        {
            using (var domainContext = new PrincipalContext(ContextType.Domain, credentials.Domain, credentials.UserName, credentials.Password))
            {
                using (var gp = GroupPrincipal.FindByIdentity(domainContext, domainGroup))
                {
                    using (var aUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, credentials.UserName))
                    {
                        if (gp != null)
                        {
                            var groupMembers = gp.GetMembers(true);
                            return groupMembers.Contains(aUser);
                        }
                        return false; // Group doesn't exist
                    }
                }
            }
        }
    }
}
