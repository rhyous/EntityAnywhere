using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices.Interfaces
{
    [ServiceContract]
    public interface IOneToManyEntityWebService<TEntity, TId> : IEntityWebService<TEntity, TId>
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<TEntity>> GetByRelatedEntityId(string id);
    }
}
