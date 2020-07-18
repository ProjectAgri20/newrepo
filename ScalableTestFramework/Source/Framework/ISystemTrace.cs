using System;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Provides trace logging capability for debugging purposes.
    /// </summary>
    public interface ISystemTrace
    {
        /// <summary>
        /// Logs a Trace message.
        /// Used for fine-grained messages typically only logged during development.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        void LogTrace(object message);

        /// <summary>
        /// Logs a Debug message.
        /// Used for general-purpose messages to show program flow.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        void LogDebug(object message);

        /// <summary>
        /// Logs an Info message.
        /// Used for messages that describe a high-level view of program flow.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        void LogInfo(object message);

        /// <summary>
        /// Logs a Notice message.
        /// Used for logging less-used code paths or unusual conditions.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        void LogNotice(object message);

        /// <summary>
        /// Logs a Notice message with an exception.
        /// Used for logging less-used code paths or unusual conditions.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        void LogNotice(object message, Exception ex);

        /// <summary>
        /// Logs a Warn message.
        /// Used for conditions that might be a problem but will not prevent the current operation from continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        void LogWarn(object message);

        /// <summary>
        /// Logs a Warn message with an exception.
        /// Used for conditions that might be a problem but will not prevent the current operation from continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        void LogWarn(object message, Exception ex);

        /// <summary>
        /// Logs an Error message.
        /// Used for conditions that prevent an operation from succeeding/continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        void LogError(object message);

        /// <summary>
        /// Logs an Error message with an exception.
        /// Used for conditions that prevent an operation from succeeding/continuing.
        /// </summary>
        /// <param name="message">The object representing the message to log.</param>
        /// <param name="ex">The exception to log with the message.</param>
        void LogError(object message, Exception ex);
    }
}
