using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.Addendum;
using ItType = System.Int64;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IAddendumWebService
    {

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Addenda", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetAll();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Addenda/Ids", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByIds(List<ItType> ids);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Addenda({idOrName})", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<Entity> Get(string idOrName);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Addenda({id})/{property}", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Addenda", ResponseFormat = WebMessageFormat.Json)]
        List<Entity> Post(List<Entity> entity);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "Addenda({id})", ResponseFormat = WebMessageFormat.Json)]
        Entity Put(string id, Entity entity);

        [OperationContract]
        [WebInvoke(Method = "PATCH", UriTemplate = "Addenda({id})", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Entity Patch(string id, Entity entity, List<string> changedProperties);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "Addenda({id})", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Users({relatedEntityId})/Properties", ResponseFormat = WebMessageFormat.Json)]
        List<Entity> GetAddenda(string relatedEntityId);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Users({relatedEntityId})/Properties({name})", ResponseFormat = WebMessageFormat.Json)]
        Entity GetAddendaByName(string relatedEntityId, string name);
    }
}