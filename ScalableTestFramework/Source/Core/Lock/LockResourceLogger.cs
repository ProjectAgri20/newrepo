using System;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// Custom logging class for <see cref="LockResource" /> that logs to a different file for each resource.
    /// </summary>
    /// <remarks>
    /// This class builds up a custom log4net appender for each resource, where the file name is the resource ID.
    /// For this to work, the executable app.config must have a logger named "LockResourceTemplate" with a RollingFileAppender.
    /// </remarks>
    internal sealed class LockResourceLogger
    {
        private const string _rootLoggerName = "HP.ScalableTest.Lock";
        private const string _templateLoggerName = "LockResourceTemplate";

        private readonly Logger _logger;
        private readonly Type _callerStackBoundaryType = typeof(LockResourceLogger);

        /// <summary>
        /// Initializes a new instance of the <see cref="LockResourceLogger" /> class.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        public LockResourceLogger(string resourceId)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            // Create a new logger for this resource ID
            _logger = hierarchy.GetLogger($"{_rootLoggerName}.{resourceId}") as Logger;

            // Find the template logger and its appender and create a copy for this resource
            Logger templateLogger = hierarchy.GetLogger(_templateLoggerName) as Logger;
            if (templateLogger?.Appenders.Count > 0)
            {
                if (templateLogger.Appenders[0] is RollingFileAppender templateAppender)
                {
                    RollingFileAppender appender = CreateResourceAppender(templateAppender, resourceId);
                    _logger.AddAppender(appender);
                }
            }
        }

        private static RollingFileAppender CreateResourceAppender(RollingFileAppender template, string resourceId)
        {
            // Scrub any invalid characters from the filename
            string safeResourceId = string.Join("_", resourceId.Split(Path.GetInvalidFileNameChars()));

            RollingFileAppender appender = new RollingFileAppender
            {
                File = template.File.Replace("{ResourceId}", safeResourceId),
                AppendToFile = template.AppendToFile,
                CountDirection = template.CountDirection,
                DatePattern = template.DatePattern,
                DateTimeStrategy = template.DateTimeStrategy,
                Layout = template.Layout,
                MaximumFileSize = template.MaximumFileSize,
                MaxSizeRollBackups = template.MaxSizeRollBackups,
                PreserveLogFileNameExtension = template.PreserveLogFileNameExtension,
                RollingStyle = template.RollingStyle,
                StaticLogFileName = template.StaticLogFileName
            };
            appender.ActivateOptions();

            return appender;
        }

        /// <summary>
        /// Logs a Trace message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogTrace(string message)
        {
            Log(Level.Trace, message);
        }

        /// <summary>
        /// Logs a Debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogDebug(string message)
        {
            Log(Level.Debug, message);
        }

        /// <summary>
        /// Logs a Warn message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogWarn(string message)
        {
            Log(Level.Warn, message);
        }

        /// <summary>
        /// Logs an Error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogError(string message)
        {
            Log(Level.Error, message);
        }

        private void Log(Level level, object message)
        {
            LoggingEvent loggingEvent = new LoggingEvent(_callerStackBoundaryType, _logger.Repository, _logger.Name, level, message, null);
            loggingEvent.Properties["charlevel"] = level.ToString()[0];
            _logger.Log(loggingEvent);
        }
    }
}
