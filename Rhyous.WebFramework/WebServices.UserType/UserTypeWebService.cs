using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using Rhyous.WebFramework.Models;

namespace Rhyous.WebFramework.WebServices
{
    public class UserTypeWebService : SearchableEntityWebService<UserType, IUserType, int, ServiceCommonSearchable<UserType, IUserType, int>>, IUserTypeWebService
    {
    }
}
