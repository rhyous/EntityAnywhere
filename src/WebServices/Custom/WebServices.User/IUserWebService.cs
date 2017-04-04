using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Entities;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    [CustomWebService(Entity = typeof(User))]
    public interface IUserWebService : IEntityWebService<User, long>, ICustomWebService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "Test", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string Test();
    }
}
