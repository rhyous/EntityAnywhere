using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;

namespace Rhyous.WebFramework.WebServices
{
    public class UserWebService : EntityWebService<User, IUser, long, UserService>, IUserWebService
    {
    }
}
