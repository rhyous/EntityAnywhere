using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface ICustomCustomerRoleAuthorization
    {
        bool IsAuthorized(IAccessToken token, NameValueCollection headers);
    }
}