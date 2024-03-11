using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface ITokenSecurityList
    {
        Dictionary<string, IEnumerable<string>> GetCustomerCalls();
    }
}