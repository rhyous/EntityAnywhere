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
        [WebInvoke(Method = "GET", UriTemplate = "UserToUserType", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetAll();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserToUserType/Ids", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetByIds(List<int> ids);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "UserToUserType({id})/{property}", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UserToUserType", ResponseFormat = WebMessageFormat.Json)]
        Entity Post(Entity entity);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "UserToUserType({id})", ResponseFormat = WebMessageFormat.Json)]
        Entity Put(string id, Entity entity);

        [OperationContract]
        [WebInvoke(Method = "PATCH", UriTemplate = "UserToUserType({id})", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Entity Patch(string id, Entity entity, List<string> changedProperties);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "UserToUserType({id})", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);


        #region Mapping Table Specific
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "User({id})/{property}", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<Entity>> GetById(string id, string property);
        #endregion
    }
}
