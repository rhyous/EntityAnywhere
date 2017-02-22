using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices.Interfaces
{
    [ServiceContract]
    public interface IOneToManyEntityWebService<T, Tid> : IEntityWebService<T, Tid>
        where T : class, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<T>> GetByRelatedEntityId(string id);
    }
}
