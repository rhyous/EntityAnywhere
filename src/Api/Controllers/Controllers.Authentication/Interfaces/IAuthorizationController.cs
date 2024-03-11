using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IAuthorizationController
    {
        /// <summary>
        /// Returns the User's role data
        /// </summary>
        /// <returns>UserRoleEntityData</returns>
        //[WebInvoke(Method = "GET", UriTemplate = "MyRoleData", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Task<IUserRoleEntityData> MyRoleDataAsync();
    }
}