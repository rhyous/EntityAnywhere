using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.UserToUserRole;
using IdType = System.Int64;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IUserToUserRoleWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserToUserRoles", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetAll();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserToUserRoles({id})", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<Entity> Get(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserToUserRoles/Ids", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByIds(List<IdType> ids);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserToUserRoles({id})/{property}", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserToUserRoles", ResponseFormat = WebMessageFormat.Json)]
        List<Entity> Post(List<Entity> entity);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "UserToUserRoles({id})", ResponseFormat = WebMessageFormat.Json)]
        Entity Put(string id, Entity entity);

        [OperationContract]
        [WebInvoke(Method = "PATCH", UriTemplate = "UserToUserRoles({id})", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Entity Patch(string id, Entity entity, List<string> changedProperties);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "UserToUserRoles({id})", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);


        #region Mapping Table Specific
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Users({id})/UserRoles", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByPrimaryEntityId(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserRoles({id})/Users", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetBySecondaryEntityId(string id);
        #endregion
    }
}
