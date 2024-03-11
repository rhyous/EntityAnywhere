using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Rhyous.EntityAnywhere.PluginLoaders
{
    /// <summary>
    /// This class loads the Loggers and passes them around.
    /// This is the EAF logger, so this one plugin loader uses
    /// a default PluginLoaderLogger. Once this is loaded all 
    /// further logging should use plugin loggers.
    /// </summary>
    /// <remarks>It is both an ILogger and a logger plugin loader.</remarks>
    public class LoggerLoader : RuntimePluginLoaderBase<ILogger>, ILogger
    {
        public LoggerLoader(IAppDomain appDomain,
                            IPluginLoaderSettings settings,
                            IPluginLoaderFactory<ILogger> pluginLoaderFactory,
                            IPluginObjectCreator<ILogger> pluginObjectCreator,
                            IPluginPaths pluginPaths)
            : base(appDomain, settings, pluginLoaderFactory, pluginObjectCreator, pluginPaths, PluginLoaderLogger.Instance)
        {
        }

        /// <inheritdoc />
        /// <remarks>The subfolder for loggers is cleverly named: Loggers</remarks>
        public override string PluginSubFolder => "Loggers";

        public IList<ILogger> Plugins
        {
            get { return _Plugins ?? (_Plugins = CreatePluginObjects()); }
            set { _Plugins = value; }
        } private IList<ILogger> _Plugins;

        public void Debug(string msg, [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFilePath = "",
            [CallerLineNumber] int callingFileLineNumber = 0)
        {
            if (Plugins == null || !Plugins.Any())
                return;
            foreach (var logger in Plugins)
            {
                logger.Debug(msg, callingMethod, callingFilePath, callingFileLineNumber);
            }
        }

        public void Write(string msg, [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFilePath = "",
            [CallerLineNumber] int callingFileLineNumber = 0)
        {
            if (Plugins == null || !Plugins.Any())
                return;
            foreach (var logger in Plugins)
            {
                logger.Write(msg, callingMethod, callingFilePath, callingFileLineNumber);
            }
        }

        public void Write(Exception exception, [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFilePath = "",
            [CallerLineNumber] int callingFileLineNumber = 0)
        {
            if (Plugins == null || !Plugins.Any())
                return;
            foreach (var logger in Plugins)
            {
                logger.Write(exception, callingMethod, callingFilePath, callingFileLineNumber);
            }
        }
    }
}