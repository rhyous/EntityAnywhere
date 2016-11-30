using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.UserType;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IUserTypeWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserTypes", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetAll();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserTypes/Ids", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByIds(List<int> ids);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserTypes({idOrName})", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<Entity> Get(string idOrName);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserTypes({id})/{property}", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserTypes", ResponseFormat = WebMessageFormat.Json)]
        List<Entity> Post(List<Entity> entity);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "UserTypes({id})", ResponseFormat = WebMessageFormat.Json)]
        Entity Put(string id, Entity entity);

        [OperationContract]
        [WebInvoke(Method = "PATCH", UriTemplate = "UserTypes({id})", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Entity Patch(string id, Entity entity, List<string> changedProperties);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "UserTypes({id})", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);
    }
}