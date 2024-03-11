using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface ICustomCustomerRoleAuthorization
    {
        bool IsAuthorized(IHeadersContainer headers, int userRoleId);
    }
}