using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Models;
using Rhyous.WebFramework.Services;

namespace Rhyous.WebFramework.WebServices
{
    public class UserWebService : SearchableEntityWebService<User, IUser, long, UserService>, IUserWebService
    {
        public UserWebService()
        {
            Service = new ServiceCommonSearchable<User, IUser, long>(x => x.Username);
        }

    }
}
