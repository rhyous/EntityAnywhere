using Rhyous.Odata;
using Rhyous.WebFramework.Entities;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    interface IUserWebService : IEntityWebService<User, long>, ICustomWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Users/Filter", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Task<OdataObjectCollection<User, long>> FilterAsync();
    }
}
