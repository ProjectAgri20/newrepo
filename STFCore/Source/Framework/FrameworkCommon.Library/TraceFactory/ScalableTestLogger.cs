using System;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest
{
    /// <summary>
    /// ScalableTest Logger
    /// </summary>
    public class ScalableTestLogger
    {
        private readonly SystemTraceLogger _systemTraceLogger = new SystemTraceLogger(typeof(ScalableTestLogger));

        public void Initialize()
        {
            Logger.Initialize(new SystemTraceLogger(typeof(Logger)));
        }

        /// <summary>
        /// Logs a Trace message
        /// </summary>
        /// <param name="message">The <see cref="String"/> message content.</param>
        public void Trace(object message) => _systemTraceLogger.LogTrace(message);

        /// <summary>
        /// Logs a Debug message
        /// </summary>
        /// <param name="message">The <see cref="String"/> message content.</param>
        public void Debug(object message) => _systemTraceLogger.LogDebug(message);

        /// <summary>
        /// Logs a Info message
        /// </summary>
        /// <param name="message">The <see cref="String"/> message content.</param>
        public void Info(object message) => _systemTraceLogger.LogInfo(message);

        /// <summary>
        /// Logs a Notice message
        /// </summary>
        /// <param name="message">The <see cref="String"/> message content.</param>
        public void Notice(object message) => _systemTraceLogger.LogNotice(message);

        /// <summary>
        /// Logs a Notice message
        /// </summary>
        /// <param name="message">The <see cref="String"/> message content.</param>
        /// <param name="ex">The <see cref="Exception"/> to be logged.</param>
        public void Notice(object message, Exception ex) => _systemTraceLogger.LogNotice(message, ex);

        /// <summary>
        /// Logs a Warn message
        /// </summary>
        /// <param name="message">The <see cref="String"/> message content.</param>
        public void Warn(object message) => _systemTraceLogger.LogWarn(message);

        /// <summary>
        /// Logs a Warn message
        /// </summary>
        /// <param name="message">The <see cref="String"/> message content.</param>
        /// <param name="ex">The <see cref="Exception"/> to be logged.</param>
        public void Warn(object message, Exception ex) => _systemTraceLogger.LogWarn(message, ex);

        /// <summary>
        /// Logs an Error message
        /// </summary>
        /// <param name="message">The <see cref="String"/> message content.</param>
        public void Error(object message) => _systemTraceLogger.LogError(message);

        /// <summary>
        /// Logs an Error message
        /// </summary>
        /// <param name="message">The <see cref="String"/> message content.</param>
        /// <param name="ex">The <see cref="Exception"/> to be logged.</param>
        public void Error(object message, Exception ex) => _systemTraceLogger.LogError(message, ex);

        /// <summary>
        /// Logs a Fatal message
        /// </summary>
        /// <param name="message">The <see cref="String"/> message content.</param>
        public void Fatal(object message) => _systemTraceLogger.LogError(message);

        /// <summary>
        /// Logs a Fatal message
        /// </summary>
        /// <param name="message">The <see cref="String"/> message content.</param>
        /// <param name="ex">The <see cref="Exception"/> to be logged.</param>
        public void Fatal(object message, Exception ex) => _systemTraceLogger.LogError(message, ex);
    }
}
