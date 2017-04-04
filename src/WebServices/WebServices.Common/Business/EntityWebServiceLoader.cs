using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.IO;

namespace Rhyous.WebFramework.WebServices
{

    public class EntityWebServiceLoader<TWebService, TEntity, TInterface, Tid, TService> : PluginLoaderBase<TWebService>
        where TWebService : class, new()
        where TEntity : class, TInterface, new()
        where TInterface : IEntity<Tid>
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
        where TService : class, IServiceCommon<TEntity, TInterface, Tid>, new()
    {
        public override bool ThrowExceptionIfNoPluginFound => false;
        public override string PluginSubFolder => Path.Combine("WebServices", typeof(TEntity).Name);

        public TWebService LoadPluginOrCommon()
        {
            return (Plugins != null && Plugins.Count > 0) ? Plugins[0] : new TWebService();
        }
    }

}