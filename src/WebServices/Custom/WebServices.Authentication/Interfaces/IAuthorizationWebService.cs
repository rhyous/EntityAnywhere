using Rhyous.EntityAnywhere.Interfaces;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    [ServiceContract]
    public interface IAuthorizationWebService
    {
        /// <summary>
        /// Returns the User's role data
        /// </summary>
        /// <returns>UserRoleEntityData</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "MyRoleData", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<UserRoleEntityData> MyRoleDataAsync();
    }
}