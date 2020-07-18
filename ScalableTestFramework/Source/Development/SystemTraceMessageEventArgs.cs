using System;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// Event args for the <see cref="SystemTraceMock.LogMessage" /> event.
    /// </summary>
    public sealed class SystemTraceMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the log level, e.g. Debug, Info, etc.
        /// </summary>
        public string Level { get; }

        /// <summary>
        /// Gets the message to be logged.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the exception if one was specified; null otherwise.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTraceMessageEventArgs"/> class.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The message.</param>
        internal SystemTraceMessageEventArgs(string level, object message)
        {
            Level = level;
            Message = message?.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTraceMessageEventArgs"/> class.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception.</param>
        internal SystemTraceMessageEventArgs(string level, object message, Exception ex)
            : this(level, message)
        {
            Exception = ex;
        }
    }
}
