using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;

namespace Rhyous.WebFramework.WebServices
{
    [CustomWebService("UserWebService", typeof(IUserWebService), typeof(User))]    
    public class UserWebService : EntityWebServiceAltId<User, IUser, long, ServiceCommonAltId<User, IUser, long>>, IUserWebService
    {
        public string Test()
        {
            return "Test worked!";
        }
    }
}
