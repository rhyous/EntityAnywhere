using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.IO;

namespace Rhyous.WebFramework.WebServices
{
    public class MappingEntityWebServiceLoader<TWebService, TEntity, TInterface, Tid, TService, E1Tid, E2Tid> : PluginLoaderBase<TWebService>
        where TWebService : class, new()
        where TEntity : class, TInterface, new()
        where TInterface : IEntity<Tid>
        where TService : class, IServiceCommon<TEntity, TInterface, Tid>, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
        where E1Tid : IComparable, IComparable<E1Tid>, IEquatable<E1Tid>
        where E2Tid : IComparable, IComparable<E2Tid>, IEquatable<E2Tid>
    {
        public override bool ThrowExceptionIfNoPluginFound => false;
        public override string PluginSubFolder => Path.Combine("WebServices", typeof(TEntity).Name);

        public TWebService LoadPluginOrCommon()
        {
            return (Plugins != null && Plugins.Count > 0) ? Plugins[0] : new TWebService();
        }
    }

}