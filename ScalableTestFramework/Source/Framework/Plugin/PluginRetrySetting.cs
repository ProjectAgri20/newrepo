using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Defines behavior for retrying plugin execution when a certain <see cref="PluginResult" /> is returned.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{State,nq}")]
    public sealed class PluginRetrySetting
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PluginResult _state;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int _retryLimit;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TimeSpan _delayBeforeRetry;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PluginRetryAction _retryAction;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PluginRetryAction _retryLimitExceededAction;

        /// <summary>
        /// The <see cref="PluginResult" /> for which this behavior should be applied.
        /// </summary>
        public PluginResult State => _state;

        /// <summary>
        /// The <see cref="PluginRetryAction" /> to take when the specified <see cref="State" /> occurs.
        /// </summary>
        public PluginRetryAction RetryAction => _retryAction;

        /// <summary>
        /// The maximum number of times the plugin should retry when the specified <see cref="State" /> occurs.
        /// Ignored if <see cref="RetryAction" /> is not <see cref="PluginRetryAction.Retry" />.
        /// </summary>
        public int RetryLimit => _retryLimit;

        /// <summary>
        /// The amount of time the plugin should wait between retries.
        /// </summary>
        /// Ignored if <see cref="RetryAction" /> is not <see cref="PluginRetryAction.Retry" />.
        public TimeSpan DelayBeforeRetry => _delayBeforeRetry;

        /// <summary>
        /// The <see cref="PluginRetryAction" /> to take when <see cref="RetryLimit" /> has been reached.
        /// (<see cref="PluginRetryAction.Retry" /> is not a valid action.)
        /// </summary>
        public PluginRetryAction RetryLimitExceededAction => _retryLimitExceededAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginRetrySetting" /> class.
        /// </summary>
        /// <param name="state">The <see cref="PluginResult" /> for which this behavior should be applied.</param>
        /// <param name="retryAction">The <see cref="PluginRetryAction" /> to take when the specified state occurs.</param>
        /// <param name="retryLimit">The maximum number of times the plugin should retry.</param>
        /// <param name="delayBeforeRetry">The amount of time the plugin should wait between retries.</param>
        /// <param name="limitExceededAction">The <see cref="PluginRetryAction" /> to take when the retry limit is reached. (<see cref="PluginRetryAction.Retry" /> is not a valid action.)</param>
        public PluginRetrySetting(PluginResult state, PluginRetryAction retryAction, int retryLimit, TimeSpan delayBeforeRetry, PluginRetryAction limitExceededAction)
        {
            if (limitExceededAction == PluginRetryAction.Retry)
            {
                throw new ArgumentException("Retry limit exceeded action cannot be retry.", nameof(limitExceededAction));
            }

            _state = state;
            _retryAction = retryAction;
            _delayBeforeRetry = delayBeforeRetry;
            _retryLimit = retryLimit;
            _retryLimitExceededAction = limitExceededAction;
        }
    }
}
