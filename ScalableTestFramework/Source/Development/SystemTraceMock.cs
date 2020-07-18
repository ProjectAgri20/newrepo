using System;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// A simple implementation of <see cref="ISystemTrace" /> for development.
    /// </summary>
    public sealed class SystemTraceMock : ISystemTrace
    {
        /// <summary>
        /// Occurs when a message passes through this <see cref="ISystemTrace" /> handler.
        /// </summary>
        public event EventHandler<SystemTraceMessageEventArgs> LogMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTraceMock" /> class.
        /// </summary>
        public SystemTraceMock()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Logs a Trace message.  Used for fine-grained messages typically only logged during development.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogTrace(object message)
        {
            Log("Trace", message);
        }

        /// <summary>
        /// Logs a Debug message.  Used for general-purpose messages to show program flow.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogDebug(object message)
        {
            Log("Debug", message);
        }

        /// <summary>
        /// Logs an Info message.  Used for messages that describe a high-level view of program flow.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogInfo(object message)
        {
            Log("Info", message);
        }

        /// <summary>
        /// Logs a Notice message.  Used for logging less-used code paths or unusual conditions.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogNotice(object message)
        {
            Log("Notice", message);
        }

        /// <summary>
        /// Logs a Notice message with an exception.  Used for logging less-used code paths or unusual conditions.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        public void LogNotice(object message, Exception ex)
        {
            Log("Notice", message, ex);
        }

        /// <summary>
        /// Logs a Warn message.  Used for conditions that might be a problem but will not prevent the current operation from continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogWarn(object message)
        {
            Log("Warn", message);
        }

        /// <summary>
        /// Logs a Warn message with an exception.  Used for conditions that might be a problem but will not prevent the current operation from continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        public void LogWarn(object message, Exception ex)
        {
            Log("Warn", message, ex);
        }

        /// <summary>
        /// Logs an Error message.  Used for conditions that prevent an operation from succeeding/continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        public void LogError(object message)
        {
            Log("Error", message);
        }

        /// <summary>
        /// Logs an Error message with an exception.  Used for conditions that prevent an operation from succeeding/continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        public void LogError(object message, Exception ex)
        {
            Log("Error", message, ex);
        }

        private void Log(string level, object message)
        {
            LogMessage?.Invoke(this, new SystemTraceMessageEventArgs(level, message));
        }

        private void Log(string level, object message, Exception ex)
        {
            LogMessage?.Invoke(this, new SystemTraceMessageEventArgs(level, message, ex));
        }
    }
}
