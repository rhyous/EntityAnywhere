using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.IO;

namespace Rhyous.WebFramework.WebServices
{

    public class EntityServiceLoader<T, Tinterface, Tid> : PluginLoaderBase<ServiceCommon<T, Tinterface, Tid>>
        where T : class, Tinterface, new()
        where Tinterface : IEntity<Tid>
        where Tid : struct, IComparable, IConvertible, IComparable<Tid>, IEquatable<Tid>
    {
        public override bool ThrowExceptionIfNoPluginFound => false;
        public override string PluginSubFolder => Path.Combine("WebServices", typeof(T).Name);

        public ServiceCommon<T, Tinterface, Tid> LoadPluginOrCommon()
        {
            return (Plugins != null && Plugins.Count > 0) ? Plugins[0] : new ServiceCommon<T, Tinterface, Tid>();
        }
    }

}