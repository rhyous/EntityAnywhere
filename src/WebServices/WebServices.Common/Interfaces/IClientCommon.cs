using Rhyous.WebFramework.WebServices;
using System;

namespace Rhyous.WebFramework.Clients
{
    public interface IClientCommon<T, Tid> : IEntityWebService<T, Tid>
        where T : class, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
    {
        string ServiceUrl { get; set; }
    }
}
