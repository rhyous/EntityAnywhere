using Rhyous.WebFramework.Entities;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IEntityWebService<T, Tid>
        where T : class, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        EntityMetadata<T> GetMetadata();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<T>> GetAll();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<T>> GetByIds(List<Tid> ids);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<T> Get(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetProperty(string id, string property);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json)]
        string UpdateProperty(string id, string property, string value);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<T>> Post(List<T> entity);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<T> Put(string id, T entity);

        [OperationContract]
        [WebInvoke(Method = "PATCH", ResponseFormat = WebMessageFormat.Json)]
        OdataObject<T> Patch(string id, PatchedEntity<T> patchedEntity);

        [OperationContract]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<Addendum> GetAddenda(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Addendum GetAddendaByName(string id, string name);
    }
}
