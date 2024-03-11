using log4net.Config;
using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Loggers.SingleFileLogger
{
    public class Logger : ILogger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Logger()
        {
            XmlConfigurator.Configure();
        }

        public void Debug(string msg, string callingMethod = "",
            string callingFilePath = "",
            int callingFileLineNumber = 0)
        {
            log.Debug($"{callingFilePath} - {callingMethod} - {callingFileLineNumber}: {msg}");
        }

        public void Write(string msg, string callingMethod = "",
            string callingFilePath = "",
             int callingFileLineNumber = 0)
        {
            log.Info($"{callingFilePath} - {callingMethod} - {callingFileLineNumber}: {msg}");
        }

        public void Write(Exception msg, string callingMethod = "",
            string callingFilePath = "",
            int callingFileLineNumber = 0)
        {
            log.Error($"{callingFilePath} - {callingMethod} - {callingFileLineNumber}: {msg}");
        }
    }
}
