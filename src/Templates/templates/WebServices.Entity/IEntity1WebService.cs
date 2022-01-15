using Rhyous.Odata;
using Rhyous.WebFramework.Entities;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    interface IEntity1WebService : IEntityWebService<Entity1, long>, ICustomWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "NewEndpoint", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Task<OdataObjectCollection<Entity1, long>> NewEndpoint();
    }
}
