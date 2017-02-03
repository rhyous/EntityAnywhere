using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.IO;

namespace Rhyous.WebFramework.WebServices
{

    public class EntityServiceLoader<T, Tinterface, Tid, TService> : PluginLoaderBase<ServiceCommon<T, Tinterface, Tid>>
        where T : class, Tinterface, new()
        where Tinterface : IEntity<Tid>
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
        where TService : IServiceCommon<T, Tinterface, Tid>, new()
    {
        public override bool ThrowExceptionIfNoPluginFound => false;
        public override string PluginSubFolder => Path.Combine("Services", typeof(T).Name);

        public IServiceCommon<T, Tinterface, Tid> LoadPluginOrCommon()
        {
            return (Plugins != null && Plugins.Count > 0) ? Plugins[0] as IServiceCommon<T,Tinterface, Tid> : new TService();
        }
    }
}