using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.User;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IUserWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Users", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetAll();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Users/Ids", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByIds(List<int> ids);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Users({idOrName})", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<Entity> Get(string idOrName);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Users({id})/{property}", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Users", ResponseFormat = WebMessageFormat.Json)]
        Entity Post(Entity entity);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "Users({id})", ResponseFormat = WebMessageFormat.Json)]
        Entity Put(string id, Entity entity);

        [OperationContract]
        [WebInvoke(Method = "PATCH", UriTemplate = "Users({id})", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Entity Patch(string id, Entity entity, List<string> changedProperties);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "Users({id})", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);
    }
}
