using Rhyous.Odata;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    [ServiceContract]
    interface ICustom1WebService : ICustomWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "NewEndpoint", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool NewEndpoint();
    }
}
