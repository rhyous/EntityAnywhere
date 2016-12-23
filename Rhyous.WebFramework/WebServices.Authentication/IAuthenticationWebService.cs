using System.ServiceModel;
using System.ServiceModel.Web;
using Rhyous.WebFramework.Services;
using Rhyous.WebFramework.Entities;

namespace  Rhyous.WebFramework.WebServices
{
    [ServiceContract]

    public interface IAuthenticationWebService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Token Authenticate(Credentials creds);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Authenticate?user={user}&password={password}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Token AuthenticateInQuery(string user, string password);
    }
}
