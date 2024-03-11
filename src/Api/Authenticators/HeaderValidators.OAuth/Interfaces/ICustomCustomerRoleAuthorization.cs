using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface ICustomCustomerRoleAuthorization
    {
        bool IsAuthorized(IAccessToken token, IHeadersContainer headers);
    }
}