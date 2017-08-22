using Rhyous.WebFramework.Entities;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IMappingEntityWebService<T, Tid, E1Tid, E2Tid> : IEntityWebService<T, Tid>
        where T : class, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
        where E1Tid : IComparable, IComparable<E1Tid>, IEquatable<E1Tid>
        where E2Tid : IComparable, IComparable<E2Tid>, IEquatable<E2Tid>
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<T>> GetByE1Ids(List<E1Tid> ids);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        List<OdataObject<T>> GetByE2Ids(List<E2Tid> ids);
    }
}
