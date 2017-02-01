using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.IO;

namespace Rhyous.WebFramework.WebServices
{

    public class EntityWebServiceLoader<T, Tinterface, Tid, TService> : PluginLoaderBase<EntityWebService<T, Tinterface, Tid, TService>>
        where T : class, Tinterface, new()
        where Tinterface : IEntity<Tid>
        where Tid : IComparable, IConvertible, IComparable<Tid>, IEquatable<Tid>
        where TService : class, IServiceCommon<T, Tinterface, Tid>, new()
    {
        public override bool ThrowExceptionIfNoPluginFound => false;
        public override string PluginSubFolder => Path.Combine("WebServices", typeof(T).Name);
    }

}