using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.UserToUserType;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IUserToUserTypeWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserToUserTypes", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetAll();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserToUserTypes({id})", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<Entity> Get(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserToUserTypes/Ids", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByIds(List<int> ids);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserToUserTypes({id})/{property}", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserToUserTypes", ResponseFormat = WebMessageFormat.Json)]
        List<Entity> Post(List<Entity> entity);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "UserToUserTypes({id})", ResponseFormat = WebMessageFormat.Json)]
        Entity Put(string id, Entity entity);

        [OperationContract]
        [WebInvoke(Method = "PATCH", UriTemplate = "UserToUserTypes({id})", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Entity Patch(string id, Entity entity, List<string> changedProperties);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "UserToUserTypes({id})", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);


        #region Mapping Table Specific
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Users({id})/UserTypes", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByPrimaryEntityId(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserTypes({id})/Users", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetBySecondaryEntityId(string id);
        #endregion
    }
}
