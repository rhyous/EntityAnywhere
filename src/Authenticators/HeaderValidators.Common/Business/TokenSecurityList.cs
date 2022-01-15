using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public class TokenSecurityList : ITokenSecurityList
    {
        public Dictionary<string, IEnumerable<string>> GetCustomerCalls()
        {
            var dict = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase);

            return dict;
        }
    }
}