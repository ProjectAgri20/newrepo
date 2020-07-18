using System;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Logger class for internal STF components that are not tied to plugin configuration/execution.
    /// </summary>
    public static class Logger
    {
        private static ISystemTrace _systemTrace;
        private static ISystemTrace Instance
        {
            get
            {
                if (_systemTrace == null)
                {
                    // If the system trace instance has not been specifically set,
                    // check to see whether we can use one of the framework services.
                    try
                    {
                        _systemTrace = ExecutionServices.SystemTrace;
                    }
                    catch (FrameworkServiceUnavailableException)
                    {
                        // Execution service is not available.
                    }

                    try
                    {
                        _systemTrace = ConfigurationServices.SystemTrace;
                    }
                    catch (FrameworkServiceUnavailableException)
                    {
                        // Configuration service is not available.
                    }
                }
                return _systemTrace;
            }
        }

        /// <summary>
        /// Initializes the <see cref="Logger" /> class to use the specified <see cref="ISystemTrace" /> instance for logging.
        /// </summary>
        /// <param name="systemTrace">The <see cref="ISystemTrace" /> instance.</param>
        public static void Initialize(ISystemTrace systemTrace)
        {
            _systemTrace = systemTrace;
        }

        /// <summary>
        /// Logs a Trace message.
        /// Used for fine-grained messages typically only logged during development.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public static void LogTrace(object message) => Instance?.LogTrace(message);

        /// <summary>
        /// Logs a Debug message.
        /// Used for general-purpose messages to show program flow.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public static void LogDebug(object message) => Instance?.LogDebug(message);

        /// <summary>
        /// Logs an Info message.
        /// Used for messages that describe a high-level view of program flow.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public static void LogInfo(object message) => Instance?.LogInfo(message);

        /// <summary>
        /// Logs a Notice message.
        /// Used for logging less-used code paths or unusual conditions.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public static void LogNotice(object message) => Instance?.LogNotice(message);

        /// <summary>
        /// Logs a Notice message with an exception.
        /// Used for logging less-used code paths or unusual conditions.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        public static void LogNotice(object message, Exception ex) => Instance?.LogNotice(message, ex);

        /// <summary>
        /// Logs a Warn message.
        /// Used for conditions that might be a problem but will not prevent the current operation from continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public static void LogWarn(object message) => Instance?.LogWarn(message);

        /// <summary>
        /// Logs a Warn message with an exception.
        /// Used for conditions that might be a problem but will not prevent the current operation from continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        public static void LogWarn(object message, Exception ex) => Instance?.LogWarn(message, ex);

        /// <summary>
        /// Logs an Error message.
        /// Used for conditions that prevent an operation from succeeding/continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public static void LogError(object message) => Instance?.LogError(message);

        /// <summary>
        /// Logs an Error message with an exception.
        /// Used for conditions that prevent an operation from succeeding/continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        public static void LogError(object message, Exception ex) => Instance?.LogError(message, ex);
    }
}
