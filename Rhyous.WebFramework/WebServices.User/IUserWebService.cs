using Rhyous.WebFramework.Services;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IUserWebService : IEntityWebService<User, long>
    {
    }
}
