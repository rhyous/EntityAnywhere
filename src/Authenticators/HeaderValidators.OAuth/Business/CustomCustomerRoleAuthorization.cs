using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    class CustomCustomerRoleAuthorization : ICustomCustomerRoleAuthorization
    {
        private readonly ITokenSecurityList _TokenSecurityList;
        private readonly IPathNormalizer _PathNormalizer;


        public CustomCustomerRoleAuthorization(ITokenSecurityList tokenSecurityList,
                                               IPathNormalizer pathNormalizer)
        {
            _TokenSecurityList = tokenSecurityList;
            _PathNormalizer = pathNormalizer;
        }

        public bool IsAuthorized(IAccessToken token, NameValueCollection headers)
        {
            if (token is null || token.UserRoleId < 0
             || headers is null || headers.Count == 0)
                return false;

            var absolutePath = headers.Get("AbsolutePath");
            if (string.IsNullOrWhiteSpace(absolutePath))
                return false;
            var httpMethod = headers.Get("HttpMethod");
            if (string.IsNullOrWhiteSpace(httpMethod))
                return false;

            var customerRoles = new HashSet<int> { WellknownUserRoleIds.Customer, WellknownUserRoleIds.InternalCustomer, WellknownUserRoleIds.Activation };
            if (!customerRoles.Contains(token.UserRoleId))
                return false;

            var normalizedAbsolutePath = _PathNormalizer.Normalize(absolutePath);

            if (IsAllowedUrl(normalizedAbsolutePath, httpMethod))
                return true;

            var pathAndQuery = headers.Get("PathAndQuery");
            pathAndQuery = _PathNormalizer.Normalize(pathAndQuery);
            if (!string.IsNullOrWhiteSpace(pathAndQuery))
                return true;

            return false;
        }

        internal bool IsAllowedUrl(string absolutePath, string httpMethod)
        {
            var customerCalls = _TokenSecurityList.GetCustomerCalls();
            return (customerCalls.TryGetValue(absolutePath, out IEnumerable<string> call) || customerCalls.TryGetValue(absolutePath.TrimEnd('/'), out call))
                 && call.Contains(httpMethod, StringComparer.OrdinalIgnoreCase);
        }
    }
}
