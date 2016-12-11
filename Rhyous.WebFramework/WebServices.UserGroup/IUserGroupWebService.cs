using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.UserGroup;
using ItType = System.Int32;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IUserGroupWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserGroups", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetAll();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserGroups/Ids", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByIds(List<ItType> ids);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserGroups({idOrName})", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<Entity> Get(string idOrName);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserGroups({id})/{property}", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserGroups", ResponseFormat = WebMessageFormat.Json)]
        List<Entity> Post(List<Entity> entity);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "UserGroups({id})", ResponseFormat = WebMessageFormat.Json)]
        Entity Put(string id, Entity entity);

        [OperationContract]
        [WebInvoke(Method = "PATCH", UriTemplate = "UserGroups({id})", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Entity Patch(string id, Entity entity, List<string> changedProperties);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "UserGroups({id})", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);
    }
}