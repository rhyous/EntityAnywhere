using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    public class PluginLoaderLoggerWrapper : IPluginLoaderLogger
    {
        private readonly ILogger _Logger;

        public PluginLoaderLoggerWrapper(ILogger logger)
        {
            // We aren't ready to enforce DI yet. We need to move legacy services to custom EAF serivces.
            _Logger = logger;// ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Log(Exception e)
        {
            _Logger?.Write(e);
        }

        public void Write(PluginLoaderLogLevel level, string msg)
        {
            if (level == PluginLoaderLogLevel.Debug)
                _Logger?.Debug(msg);
            if (level == PluginLoaderLogLevel.Info)
                _Logger?.Write(msg);
        }

        public void WriteLine(PluginLoaderLogLevel level, string msg)
        {
            if (!msg.EndsWith(Environment.NewLine))
                msg += Environment.NewLine;
            Write(level, msg);
        }

        public void WriteLines(PluginLoaderLogLevel level, params string[] msgLines)
        {
            foreach (var msgLine in msgLines)
                WriteLine(level, msgLine);
        }
    }
}
