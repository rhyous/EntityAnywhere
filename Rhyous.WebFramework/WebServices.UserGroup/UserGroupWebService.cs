using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Models;
using Rhyous.WebFramework.Services;

namespace Rhyous.WebFramework.WebServices
{
    public class UserGroupWebService : SearchableEntityWebService<UserGroup, IUserGroup, int, ServiceCommonSearchable<UserGroup, IUserGroup, int>>, IUserGroupWebService
    {
        public UserGroupWebService()
        {
            Service = new ServiceCommonSearchable<UserGroup, IUserGroup, int>(x => x.Name);
        }
    }
}
