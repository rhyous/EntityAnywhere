using System.ServiceModel;
using Rhyous.WebFramework.Models;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IUserRoleWebService : IEntityWebService<UserRole, int>
    {
    }
}