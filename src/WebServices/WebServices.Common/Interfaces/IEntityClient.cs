using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Clients
{
    public interface IEntityClient<T, Tid> : IEntityWebService<T, Tid>
        where T : class, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
    {
        string ServiceUrl { get; set; }

        string Entity { get; }

        IHttpContextProvider HttpContextProvider { get; }
        List<OdataObject<T>> GetAll(string queryParameters);
        List<OdataObject<T>> GetByIds(IEnumerable<Tid> ids);
        List<OdataObject<T>> GetByCustomUrl(string url);
    }
}
