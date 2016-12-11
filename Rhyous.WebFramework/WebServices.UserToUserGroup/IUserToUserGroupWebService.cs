using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.UserToUserGroup;
using IdType = System.Int64;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IUserToUserGroupWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserToUserGroups", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetAll();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserToUserGroups({id})", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<Entity> Get(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserToUserGroups/Ids", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByIds(List<IdType> ids);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserToUserGroups({id})/{property}", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserToUserGroups", ResponseFormat = WebMessageFormat.Json)]
        List<Entity> Post(List<Entity> entity);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "UserToUserGroups({id})", ResponseFormat = WebMessageFormat.Json)]
        Entity Put(string id, Entity entity);

        [OperationContract]
        [WebInvoke(Method = "PATCH", UriTemplate = "UserToUserGroups({id})", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Entity Patch(string id, Entity entity, List<string> changedProperties);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "UserToUserGroups({id})", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);


        #region Mapping Table Specific
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Users({id})/UserGroups", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByPrimaryEntityId(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserGroups({id})/Users", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetBySecondaryEntityId(string id);
        #endregion
    }
}
