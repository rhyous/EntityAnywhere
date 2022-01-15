using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Behaviors.Tests
{
    internal class Data
    {
        public static IToken token = new Token
        {
            ClaimDomains = new System.Collections.Generic.List<ClaimDomain>(),
            CreateDate = DateTimeOffset.Now,            
            Text = "TestText",
            CredentialEntityId = 1233421
        };
    }
}
