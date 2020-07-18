using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ActivityOutcome
{
    [ToolboxItem(false)]
    public partial class ActivityOutcomeExecControl : UserControl, IPluginExecutionEngine
    {
        public ActivityOutcomeExecControl()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            ActivityOutcomeData activityData = executionData.GetMetadata<ActivityOutcomeData>();
            var retryManager = new PluginRetryManager(executionData, UpdateStatus);
            return retryManager.Run(() => DoActivity(executionData, activityData));
        }

        private PluginExecutionResult DoActivity(PluginExecutionData executionData, ActivityOutcomeData activityData)
        {
            UpdateStatus("Starting activity");
            Wait(2);
            PluginResult result = DetermineOutcome(activityData);
            Wait(2);
            PluginExecutionResult executionResult = GetExecutionResult(result);
            UpdateStatus("Result = " + executionResult.Message);
            return executionResult;
        }

        private PluginResult DetermineOutcome(ActivityOutcomeData activityData)
        {
            if (activityData.RandomResult)
            {
                var outcome = (new Random()).Next(8);
                switch (outcome)
                {
                    case 0:
                    case 1:
                        return PluginResult.Skipped;

                    case 2:
                        return PluginResult.Failed;

                    case 3:
                        return PluginResult.Error;

                    case 4:
                        throw new Exception("Intentional exception");

                    default:
                        return PluginResult.Passed;
                }
            }
            else
            {
                return activityData.Result;
            }
        }

        private PluginExecutionResult GetExecutionResult(PluginResult result)
        {
            string msg = string.Empty;
            switch (result)
            {
                case PluginResult.Error:
                    msg = "Error deliberately";
                    break;

                case PluginResult.Failed:
                    msg = "Failed deliberately";
                    break;

                case PluginResult.Skipped:
                    msg = "Skipped deliberately";
                    break;

                case PluginResult.Passed:
                    msg = "Passed";
                    break;
            }

            return new PluginExecutionResult(result, msg);
        }

        private void Wait(int seconds)
        {
            if (seconds > 0)
            {
                UpdateStatus(string.Format("Waiting for {0} seconds...", seconds));
                Thread.Sleep(TimeSpan.FromSeconds(seconds));
            }
        }

        /// <summary>
        /// Updates the status (text) of the text box plugin display.
        /// </summary>
        /// <param name="text">The text.</param>
        protected virtual void UpdateStatus(string text)
        {
            status_RichTextBox.InvokeIfRequired(c =>
                {
                    ExecutionServices.SystemTrace.LogNotice("Status=" + text);
                    StringBuilder logText = new StringBuilder();
                    logText.Append(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                    logText.Append("  ");
                    logText.AppendLine(text);
                    c.AppendText(logText.ToString());
                    c.Refresh();
                }
            );
        }
    }
}
