using System;
using HP.ScalableTest.Framework;
using log4net;
using log4net.Config;
using log4net.Core;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    public class CtcServiceLogger : ISystemTrace
    {
        private readonly Type _loggerType = typeof(Logger);
        private readonly ILog _logger = CreateLogger();

        private static ILog CreateLogger()
        {
            if (!LogManager.GetRepository().Configured)
            {
                XmlConfigurator.Configure();
            }

            return LogManager.GetLogger("HP.ScalableTest");
        }

        public void LogTrace(object message) => Log(Level.Trace, message);
        public void LogDebug(object message) => Log(Level.Debug, message);
        public void LogInfo(object message) => Log(Level.Info, message);
        public void LogNotice(object message) => Log(Level.Notice, message);
        public void LogNotice(object message, Exception ex) => Log(Level.Notice, message, ex);
        public void LogWarn(object message) => Log(Level.Warn, message);
        public void LogWarn(object message, Exception ex) => Log(Level.Warn, message, ex);
        public void LogError(object message) => Log(Level.Error, message);
        public void LogError(object message, Exception ex) => Log(Level.Error, message, ex);

        private void Log(Level level, object message, Exception ex = null)
        {
            ThreadContext.Properties["charlevel"] = level.ToString()[0];
            _logger.Logger.Log(_loggerType, level, message, ex);
        }
    }
}
