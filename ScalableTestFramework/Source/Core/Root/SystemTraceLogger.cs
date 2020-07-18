using System;
using System.IO;
using HP.ScalableTest.Framework;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace HP.ScalableTest.Core
{
    /// <summary>
    /// Implementation of <see cref="ISystemTrace" /> using log4net.
    /// </summary>
    public sealed class SystemTraceLogger : ISystemTrace
    {
        private readonly ILog _logger;
        private readonly Type _callerStackBoundaryType;

        /// <summary>
        /// Gets the default logger name used if no other logger name is specified.
        /// </summary>
        public static string DefaultLoggerName { get; } = "HP.ScalableTest";

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTraceLogger" /> class.
        /// </summary>
        public SystemTraceLogger()
            : this(typeof(SystemTraceLogger))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTraceLogger" /> class.
        /// </summary>
        /// <param name="callerStackBoundary">The type that will be used as the boundary for the call stack.</param>
        public SystemTraceLogger(Type callerStackBoundary)
            : this(callerStackBoundary, DefaultLoggerName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTraceLogger" /> class.
        /// </summary>
        /// <param name="callerStackBoundary">The type that will be used as the boundary for the call stack.</param>
        /// <param name="loggerName">The logger name to look for in the configuration data.</param>
        public SystemTraceLogger(Type callerStackBoundary, string loggerName)
        {
            _callerStackBoundaryType = callerStackBoundary;

            ILoggerRepository loggerRepository = LogManager.GetRepository();
            if (!loggerRepository.Configured)
            {
                ConfigureFromAppConfig();
                if (!loggerRepository.Configured)
                {
                    ConfigureDefault(loggerRepository);
                }
            }
            _logger = LogManager.GetLogger(loggerName);
        }

        private static void ConfigureFromAppConfig()
        {
            string appconfig = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(appconfig));
        }

        private static void ConfigureDefault(ILoggerRepository loggerRepository)
        {
            PatternLayout layout = new PatternLayout("%date{yyyy-MM-dd HH:mm:ss.fff} [%property{charlevel}] %class{1}::%method::%message%newline");
            layout.ActivateOptions();

            RollingFileAppender fileAppender = new RollingFileAppender
            {
                File = $"Logs/{AppDomain.CurrentDomain.FriendlyName}.log",
                StaticLogFileName = true,
                AppendToFile = false,
                PreserveLogFileNameExtension = true,
                MaxSizeRollBackups = 10,
                MaximumFileSize = "1MB",
                RollingStyle = RollingFileAppender.RollingMode.Size,
                Layout = layout
            };
            fileAppender.ActivateOptions();

            ConsoleAppender consoleAppender = new ConsoleAppender
            {
                Layout = layout
            };
            consoleAppender.ActivateOptions();

            Hierarchy hierarchy = (Hierarchy)loggerRepository;
            hierarchy.Root.AddAppender(fileAppender);
            hierarchy.Root.AddAppender(consoleAppender);
            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }

        /// <summary>
        /// Sets a global property that can be referenced in log4net configuration.
        /// This property is shared by all threads in the current AppDomain.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        public static void SetGlobalProperty(string name, string value)
        {
            GlobalContext.Properties[name] = value;
        }

        /// <summary>
        /// Sets a thread-specific property that can be referenced in log4net configuration.
        /// This property is visible only to the current managed thread.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        public static void SetThreadProperty(string name, string value)
        {
            ThreadContext.Properties[name] = value;
        }

        #region ISystemTrace Members

        /// <summary>
        /// Logs a Trace message.
        /// Used for fine-grained messages typically only logged during development.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogTrace(object message)
        {
            Log(Level.Trace, message);
        }

        /// <summary>
        /// Logs a Debug message.
        /// Used for general-purpose messages to show program flow.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogDebug(object message)
        {
            Log(Level.Debug, message);
        }

        /// <summary>
        /// Logs an Info message.
        /// Used for messages that describe a high-level view of program flow.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogInfo(object message)
        {
            Log(Level.Info, message);
        }

        /// <summary>
        /// Logs a Notice message.
        /// Used for logging less-used code paths or unusual conditions.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogNotice(object message)
        {
            Log(Level.Notice, message);
        }

        /// <summary>
        /// Logs a Notice message with an exception.
        /// Used for logging less-used code paths or unusual conditions.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        public void LogNotice(object message, Exception ex)
        {
            Log(Level.Notice, message, ex);
        }

        /// <summary>
        /// Logs a Warn message.
        /// Used for conditions that might be a problem but will not prevent the current operation from continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogWarn(object message)
        {
            Log(Level.Warn, message);
        }

        /// <summary>
        /// Logs a Warn message with an exception.
        /// Used for conditions that might be a problem but will not prevent the current operation from continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        public void LogWarn(object message, Exception ex)
        {
            Log(Level.Warn, message, ex);
        }

        /// <summary>
        /// Logs an Error message.
        /// Used for conditions that prevent an operation from succeeding/continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogError(object message)
        {
            Log(Level.Error, message);
        }

        /// <summary>
        /// Logs an Error message with an exception.
        /// Used for conditions that prevent an operation from succeeding/continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        public void LogError(object message, Exception ex)
        {
            Log(Level.Error, message, ex);
        }

        private void Log(Level level, object message, Exception ex = null)
        {
            LoggingEvent loggingEvent = new LoggingEvent(_callerStackBoundaryType, _logger.Logger.Repository, _logger.Logger.Name, level, message, ex);
            loggingEvent.Properties["charlevel"] = level.ToString()[0];
            _logger.Logger.Log(loggingEvent);
        }

        #endregion
    }
}
