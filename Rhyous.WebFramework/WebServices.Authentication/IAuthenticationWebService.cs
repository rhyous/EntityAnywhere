using System.ServiceModel;
using System.ServiceModel.Web;
using Rhyous.WebFramework.Services;

namespace  Rhyous.WebFramework.WebServices
{
    [ServiceContract]

    public interface IAuthenticationWebService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Token Authenticate(Credentials creds);
    }
}
