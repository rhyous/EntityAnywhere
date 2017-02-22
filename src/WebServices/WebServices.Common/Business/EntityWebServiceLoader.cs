using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.IO;

namespace Rhyous.WebFramework.WebServices
{

    public class EntityWebServiceLoader<Ts, T, Tinterface, Tid, TService> : PluginLoaderBase<Ts>
        where Ts : class, new()
        where T : class, Tinterface, new()
        where Tinterface : IEntity<Tid>
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
        where TService : class, IServiceCommon<T, Tinterface, Tid>, new()
    {
        public override bool ThrowExceptionIfNoPluginFound => false;
        public override string PluginSubFolder => Path.Combine("WebServices", typeof(T).Name);
    }

}