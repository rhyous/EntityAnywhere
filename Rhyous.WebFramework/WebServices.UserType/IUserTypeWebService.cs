using Rhyous.WebFramework.Models;
using System.ServiceModel;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IUserTypeWebService : IEntityWebService<UserType, int>
    {
    }
}