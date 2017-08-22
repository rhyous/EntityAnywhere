using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;

namespace Rhyous.WebFramework.WebServices
{
    [CustomWebService("UserWebService", typeof(IUserWebService), typeof(User))]    
    public class UserWebService : EntityWebServiceAlternateKey<User, IUser, long, ServiceCommonAlternateKey<User, IUser, long>>, IUserWebService
    {
        public string Test()
        {
            return "Test worked!";
        }
    }
}
