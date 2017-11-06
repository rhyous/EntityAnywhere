using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public class ClaimDomainDictionary : NullSafeDictionary<string, ClaimDomain>
    {
        public override ClaimDomain DefaultValueProvider(string key)
        {
            return this[key] = new ClaimDomain { Subject = key, Issuer = "LOCAL AUTHORITY" }; ;
        }
    }
}