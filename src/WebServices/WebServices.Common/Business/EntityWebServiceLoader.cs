using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.IO;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This class loads the custom entity web service layer plugins or the common entity web service if no custom one exists.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TWebService">The custom entity web service layer plugin type.</typeparam>
    public class EntityWebServiceLoader<TEntity, TWebService> : PluginLoaderBase<TWebService>
        where TWebService : class, new()
    {
        /// <inheritdoc />
        public override bool ThrowExceptionIfNoPluginFound => false;
        /// <inheritdoc />
        /// <remarks>The default WebServices plugin directory is cleverly named: WebServices</remarks>
        public override string PluginSubFolder => Path.Combine("WebServices", typeof(TEntity).Name);

        /// <summary>
        /// This method loads the custom entity web service layer plugin or the common entity web service if no custom one exists.
        /// </summary>
        /// <returns>The custom or common web service.</returns>
        public TWebService LoadPluginOrCommon()
        {
            return (Plugins != null && Plugins.Count > 0) ? Plugins[0] : new TWebService();
        }
    }

}