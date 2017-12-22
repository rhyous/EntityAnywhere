using Rhyous.WebFramework.Services;
using System;
using System.IO;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This class loads the custom entity service layer plugins or the common entity service if no custom one exists.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TService">The custom entity service layer plugin type. To use the common entity web service and have a custom entity service, the custom service must inherit ServiceCommon{T, Tinterface, Tid}.</typeparam>
    public class EntityServiceLoader<TEntity, TService> : PluginLoaderBase<TService>
        where TService : class, new()
    {
        /// <inheritdoc />
        public override bool ThrowExceptionIfNoPluginFound => false;
        /// <inheritdoc />
        /// <remarks>The default Services plugin directory is cleverly named: Services</remarks>
        public override string PluginSubFolder => Path.Combine("Services", typeof(TEntity).Name);

        /// <summary>
        /// This method loads the custom entity service layer plugin or the common entity service if no custom one exists.
        /// </summary>
        /// <returns>The custom or common service.</returns>
        public TService LoadPluginOrCommon()
        {
            if (PluginCollection != null && PluginCollection.Any())
            {
                if (Plugins == null || !Plugins.Any())
                    throw new Exception("Custom entity service plugin found but failed to load.");
                return Plugins[0];
            }
            return new TService();
        }
    }
}