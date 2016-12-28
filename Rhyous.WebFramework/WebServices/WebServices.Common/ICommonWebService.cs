using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface ICommonWebService<T>
    {
        [OperationContract]
        List<OdataObject<T>> GetAll();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "T/Ids", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<T>> GetByIds(List<int> ids);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "T({idOrName})", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<T> Get(string idOrName);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "T({id})/{property}", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "T", ResponseFormat = WebMessageFormat.Json)]
        T Post(T entity);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "T({id})", ResponseFormat = WebMessageFormat.Json)]
        T Put(string id, T entity);

        [OperationContract]
        [WebInvoke(Method = "PATCH", UriTemplate = "T({id})", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        T Patch(string id, T entity, List<string> changedProperties);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "T({id})", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);
    }
}
