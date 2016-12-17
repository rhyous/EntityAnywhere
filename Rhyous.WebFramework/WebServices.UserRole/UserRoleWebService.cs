using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Models;
using Rhyous.WebFramework.Services;

namespace Rhyous.WebFramework.WebServices
{
    public class UserRoleWebService : SearchableEntityWebService<UserRole, IUserRole, int, ServiceCommonSearchable<UserRole, IUserRole, int>>, IUserRoleWebService
    {
        public UserRoleWebService()
        {
            Service = new ServiceCommonSearchable<UserRole, IUserRole, int>(x => x.Name);
        }
    }
}
