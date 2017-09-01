using System;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;

namespace Rhyous.WebFramework.Authenticators
{
    /// <summary>
    /// A service class used to communicate with Active Diretory
    /// </summary>
    public class ActiveDirectoryService : IActiveDirectoryService
    {        
        private const int ErrorLogonFailure = 0x31;

        /// <inheritdoc />
        public bool ValidateCredentialsAgainstDomain(NetworkCredential credentials)
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
            }
            return true;
        }

        /// <inheritdoc />
        public bool IsUserInGroup(NetworkCredential credentials, string domain, string group)
        {
            using (var domainContext = new PrincipalContext(ContextType.Domain, domain))
            {
                using (var userDomainContext = (credentials.Domain == domain)
                                             ? domainContext
                                             : new PrincipalContext(ContextType.Domain, credentials.Domain, credentials.UserName, credentials.Password))
                {
                    using (var gp = GroupPrincipal.FindByIdentity(domainContext, group))
                    {
                        if (gp == null)
                            throw new Exception("The specified domain group was not found.");
                        using (var aUser = UserPrincipal.FindByIdentity(userDomainContext, IdentityType.SamAccountName, credentials.UserName))
                        {
                            if (aUser == null)
                                return false;
                            var result = gp.GetMembers(true);
                            return result.Contains(aUser);
                        }
                    }
                }
            }
        }
    }
}