using System;
using HP.ScalableTest.Framework;
using log4net;
using log4net.Config;
using log4net.Core;

namespace PluginSimulator
{
    internal sealed class SystemTraceLogger : ISystemTrace
    {
        private readonly ILog _logger;
        private readonly Type _callerStackBoundaryType;

        public SystemTraceLogger()
            : this(typeof(SystemTraceLogger))
        {
        }

        public SystemTraceLogger(Type callerStackBoundary)
        {
            _callerStackBoundaryType = callerStackBoundary;

            XmlConfigurator.Configure();
            _logger = LogManager.GetLogger("HP.ScalableTest");
        }

        /// <summary>
        /// Logs a Trace message.  Used for fine-grained messages typically only logged during development.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogTrace(object message)
        {
            Log(Level.Trace, message);
        }

        /// <summary>
        /// Logs a Debug message.  Used for general-purpose messages to show program flow.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogDebug(object message)
        {
            Log(Level.Debug, message);
        }

        /// <summary>
        /// Logs an Info message.  Used for messages that describe a high-level view of program flow.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogInfo(object message)
        {
            Log(Level.Info, message);
        }

        /// <summary>
        /// Logs a Notice message.  Used for logging less-used code paths or unusual conditions.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogNotice(object message)
        {
            Log(Level.Notice, message);
        }

        /// <summary>
        /// Logs a Notice message with an exception.  Used for logging less-used code paths or unusual conditions.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        public void LogNotice(object message, Exception ex)
        {
            Log(Level.Notice, message, ex);
        }

        /// <summary>
        /// Logs a Warn message.  Used for conditions that might be a problem but will not prevent the current operation from continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogWarn(object message)
        {
            Log(Level.Warn, message);
        }

        /// <summary>
        /// Logs a Warn message with an exception.  Used for conditions that might be a problem but will not prevent the current operation from continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        public void LogWarn(object message, Exception ex)
        {
            Log(Level.Warn, message, ex);
        }

        /// <summary>
        /// Logs an Error message.  Used for conditions that prevent an operation from succeeding/continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogError(object message)
        {
            Log(Level.Error, message);
        }

        /// <summary>
        /// Logs an Error message with an exception.  Used for conditions that prevent an operation from succeeding/continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        public void LogError(object message, Exception ex)
        {
            Log(Level.Error, message, ex);
        }

        private void Log(Level level, object message, Exception ex = null)
        {
            _logger.Logger.Log(_callerStackBoundaryType, level, message, ex);
        }
    }
}
