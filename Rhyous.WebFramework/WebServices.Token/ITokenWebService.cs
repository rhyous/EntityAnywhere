using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.Token;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface ITokenWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Tokens", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetAll();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Tokens/Ids", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByIds(List<int> ids);
        
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Tokens({id})/{property}", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Tokens", ResponseFormat = WebMessageFormat.Json)]
        List<Entity> Post(List<Entity> entity);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "Tokens({id})", ResponseFormat = WebMessageFormat.Json)]
        Entity Put(string id, Entity entity);

        [OperationContract]
        [WebInvoke(Method = "PATCH", UriTemplate = "Tokens({id})", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Entity Patch(string id, Entity entity, List<string> changedProperties);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "Tokens({id})", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);

        #region One to Many Method
        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "Users({id})/Token", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByRelatedEntityId(string id);
        #endregion
    }
}
