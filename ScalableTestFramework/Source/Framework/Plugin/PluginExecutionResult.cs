using System;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Contains information about the outcome of an <see cref="IPluginExecutionEngine" /> execution.
    /// </summary>
    public sealed class PluginExecutionResult
    {
        /// <summary>
        /// A <see cref="PluginResult" /> representing the execution outcome.
        /// </summary>
        public PluginResult Result { get; }

        /// <summary>
        /// A message describing the result of the execution.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// A category used for displaying this result, used for grouping similar errors together.
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the <see cref="PluginRetryStatus" /> representing the execution's retry handling.
        /// </summary>
        public PluginRetryStatus RetryStatus { get; internal set; } = PluginRetryStatus.DidNotRetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginExecutionResult" /> class.
        /// </summary>
        /// <param name="result">The <see cref="PluginResult" /> representing the execution outcome.</param>
        public PluginExecutionResult(PluginResult result)
        {
            Result = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginExecutionResult" /> class.
        /// </summary>
        /// <param name="result">The <see cref="PluginResult" /> representing the execution outcome.</param>
        /// <param name="message">The message describing the result of the execution.</param>
        public PluginExecutionResult(PluginResult result, string message)
            : this(result)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginExecutionResult" /> class.
        /// </summary>
        /// <param name="result">The <see cref="PluginResult" /> representing the execution outcome.</param>
        /// <param name="exception">The exception whose message describes the result of the execution.</param>
        /// <exception cref="ArgumentNullException"><paramref name="exception" /> is null.</exception>
        public PluginExecutionResult(PluginResult result, Exception exception)
            : this(result)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Message = exception.Message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginExecutionResult" /> class.
        /// </summary>
        /// <param name="result">The <see cref="PluginResult" /> representing the execution outcome.</param>
        /// <param name="message">The message describing the result of the execution.</param>
        /// <param name="category">The category used for grouping similar messages together.</param>
        public PluginExecutionResult(PluginResult result, string message, string category)
            : this(result, message)
        {
            Category = category;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginExecutionResult" /> class.
        /// </summary>
        /// <param name="result">The <see cref="PluginResult" /> representing the execution outcome.</param>
        /// <param name="exception">The exception whose message describes the result of the execution.</param>
        /// <param name="category">The category used for grouping similar messages together.</param>
        /// <exception cref="ArgumentNullException"><paramref name="exception" /> is null.</exception>
        public PluginExecutionResult(PluginResult result, Exception exception, string category)
            : this(result, exception)
        {
            Category = category;
        }
    }
}
