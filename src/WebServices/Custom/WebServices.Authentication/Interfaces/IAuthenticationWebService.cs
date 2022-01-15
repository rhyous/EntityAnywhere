using Rhyous.EntityAnywhere.Interfaces;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// A service contract for a custom web service, not related to an entity, for authentication.
    /// </summary>
    [ServiceContract]
    public interface IAuthenticationWebService
    {
        /// <summary>
        /// This method takes in a user name and password in a POSTed Credentials object and authenticates with them.
        /// </summary>
        /// <param name="creds">A Credentials object, which contains a user name and password.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<Token> AuthenticateAsync(Credentials creds);

        /// <summary>
        /// This method takes in a username and password from URL parameters in a GET requests and authenticates with them.
        /// This method is not recommended as the password is exposed in the URL.
        /// </summary>
        /// <param name="user">The user as a string.</param>
        /// <param name="password">The password as a string.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Authenticate?user={user}&password={password}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<Token> AuthenticateInQueryAsync(string user, string password);
    }
}
