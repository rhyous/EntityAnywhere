using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces.Tests
{
    public class FakeToken : IToken
    {
        public string Text { get; set; }
        public string CredentialEntity { get; set; }
        public long CredentialEntityId { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        public int OrganizationId { get; set; }
        public List<ClaimDomain> ClaimDomains { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}