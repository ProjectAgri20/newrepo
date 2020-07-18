using System;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.IO;
using HP.ScalableTest;

namespace HP.SolutionTest.Install
{
    internal class SystemTrace : MarshalByRefObject
    {
        private readonly Logger _logger;

        private readonly static SystemTrace _instance = new SystemTrace();

        SystemTrace()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = "%date{yyyy-MM-dd HH:mm:ss.fff} %message%newline";
            layout.ActivateOptions();

            RollingFileAppender appender = new RollingFileAppender();
            appender.AppendToFile = false;
            appender.File = Path.Combine(Path.GetTempPath(), "StbInstaller-{0}.log".FormatWith(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            appender.Layout = layout;
            appender.MaxSizeRollBackups = 5;
            appender.MaximumFileSize = "1MB";
            appender.RollingStyle = RollingFileAppender.RollingMode.Size;
            appender.StaticLogFileName = true;
            appender.ActivateOptions();

            hierarchy.Root.AddAppender(appender);
            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;

            _logger = hierarchy.Root;
        }

        public static SystemTrace Instance
        {
            get { return _instance; }
        }

        public void Debug(object message, Exception ex = null)
        {
            _logger.Log(Level.Debug, message, ex);
        }
        public void Info(object message, Exception ex = null)
        {
            _logger.Log(Level.Info, message, ex);
        }

        public void Error(object message, Exception ex = null)
        {
            _logger.Log(Level.Error, message, ex);
        }
    }
}
