using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface ICustomCustomerRoleAuthorization
    {
        bool IsAuthorized(NameValueCollection headers, int userRoleId);
    }
}