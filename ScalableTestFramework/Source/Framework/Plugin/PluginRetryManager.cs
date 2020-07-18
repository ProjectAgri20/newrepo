using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Manages retry settings during plugin execution.
    /// </summary>
    public sealed class PluginRetryManager
    {
        private readonly PluginExecutionData _executionData;
        private readonly PluginRetryCountDictionary _retryCounts = new PluginRetryCountDictionary();
        private readonly Action<string> _statusMessageUpdateAction;

        /// <summary>
        /// Gets the total number of retries performed by this <see cref="PluginRetryManager" />.
        /// </summary>
        public int TotalRetries => _retryCounts.Values.Sum();

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginRetryManager" /> class.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="executionData" /> is null.</exception>
        public PluginRetryManager(PluginExecutionData executionData)
        {
            _executionData = executionData ?? throw new ArgumentNullException(nameof(executionData));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginRetryManager" /> class.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <param name="statusMessageUpdateAction">A method to execute when this <see cref="PluginRetryManager" /> has a status update.</param>
        /// <exception cref="ArgumentNullException"><paramref name="executionData" /> is null.</exception>
        public PluginRetryManager(PluginExecutionData executionData, Action<string> statusMessageUpdateAction)
            : this(executionData)
        {
            _statusMessageUpdateAction = statusMessageUpdateAction;
        }

        /// <summary>
        /// Executes the specified plugin execution method, handling retries as configured in the <see cref="PluginExecutionData" />.
        /// </summary>
        /// <param name="action">The plugin method to execute.</param>
        /// <returns>A <see cref="PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        public PluginExecutionResult Run(Func<PluginExecutionResult> action)
        {
            PluginExecutionResult executionResult = InvokeAction(action);
            PluginRetryAction retryAction = GetRetryAction(executionResult.Result);

            // Only retry if the action specifies we should AND the action did not already 
            while (retryAction == PluginRetryAction.Retry && executionResult.RetryStatus == PluginRetryStatus.DidNotRetry)
            {
                PluginResult currentResult = executionResult.Result;

                OnStatusMessageUpdate($"Retry triggered for state {currentResult}).");
                _retryCounts.Increment(currentResult);
                LogRetryData(executionResult);
                DelayBeforeRetry(_executionData.RetrySettings[currentResult]);

                OnStatusMessageUpdate($"Starting retry number {TotalRetries}.");
                executionResult = InvokeAction(action);
                retryAction = GetRetryAction(executionResult.Result);
            }

            // Update the retry status if it is still set to "did not retry"
            if (executionResult.RetryStatus == PluginRetryStatus.DidNotRetry)
            {
                // Determine the final retry status based on the specified retry action
                switch (retryAction)
                {
                    case PluginRetryAction.Halt:
                        OnStatusMessageUpdate("Halting execution per retry settings.");
                        executionResult.RetryStatus = PluginRetryStatus.Halt;
                        break;

                    case PluginRetryAction.Continue:
                    default:
                        executionResult.RetryStatus = PluginRetryStatus.Continue;
                        break;
                }
            }

            return executionResult;
        }

        private PluginRetryAction GetRetryAction(PluginResult result)
        {
            PluginRetrySetting retrySetting = _executionData.RetrySettings[result];

            // Handle if we've exceeded retry limit
            if (retrySetting.RetryAction == PluginRetryAction.Retry && _retryCounts[result] >= retrySetting.RetryLimit)
            {
                string message = $"Retry limit of [{retrySetting.RetryLimit}] reached";
                switch (retrySetting.RetryLimitExceededAction)
                {
                    case PluginRetryAction.Halt:
                        OnStatusMessageUpdate(message + ", signaling worker halt");
                        return PluginRetryAction.Halt;

                    default:
                        OnStatusMessageUpdate(message + ", continuing execution");
                        return PluginRetryAction.Continue;
                }
            }
            else
            {
                return retrySetting.RetryAction;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Must be able to handle any exception that is thrown by the plugin action.")]
        private static PluginExecutionResult InvokeAction(Func<PluginExecutionResult> action)
        {
            try
            {
                return action.Invoke();
            }
            catch (Exception ex)
            {
                return new PluginExecutionResult(PluginResult.Error, ex.ToString(), "Unhandled exception.");
            }
        }

        private void DelayBeforeRetry(PluginRetrySetting triggeredRetrySetting)
        {
            OnStatusMessageUpdate($"Delaying for [{triggeredRetrySetting.DelayBeforeRetry}] before retry...");
            if (triggeredRetrySetting.DelayBeforeRetry > TimeSpan.Zero)
            {
                Thread.Sleep(triggeredRetrySetting.DelayBeforeRetry);
            }
        }

        private void LogRetryData(PluginExecutionResult result)
        {
            ExecutionServices.DataLogger.Submit(new ActivityRetryLog(_executionData, result));
        }

        private void OnStatusMessageUpdate(string message)
        {
            _statusMessageUpdateAction?.Invoke(message);
        }

        private sealed class PluginRetryCountDictionary : Dictionary<PluginResult, int>
        {
            public PluginRetryCountDictionary()
            {
                // Prepopulate with zeros for all result types, so that we don't have to check for keys every time
                foreach (PluginResult result in Enum.GetValues(typeof(PluginResult)))
                {
                    this[result] = 0;
                }
            }

            public void Increment(PluginResult result) => this[result]++;
        }
    }
}
