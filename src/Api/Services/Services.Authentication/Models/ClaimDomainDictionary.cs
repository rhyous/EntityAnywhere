using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services
{
    public class ClaimDomainDictionary : NullSafeDictionary<string, ClaimDomain>
    {
        public override ClaimDomain DefaultValueProvider(string key)
        {
            return this[key] = new ClaimDomain { Subject = key, Issuer = "LOCAL AUTHORITY" };
        }
    }
}