using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;

namespace $safenamespace$
{
    /// <summary>
    /// Control used to perform and monitor the execution of the $safeclassprefix$ activity.
    /// </summary>
    public partial class $safeclassprefix$ExecutionControl : UserControl, IPluginExecutionEngine
    {
        private $safeclassprefix$ActivityData _data = null;

        /// <summary>
        /// Initializes a new instance of this control.
        /// </summary>
        public $safeclassprefix$ExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Defines and executes the $safeclassprefix$ workflow.
        /// </summary>
        /// <param name="executionData">Information used in the execution of this workflow.</param>
        /// <returns>The result of executing the workflow.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            PluginResult result = PluginResult.Passed;
            _data = executionData.GetMetadata<$safeclassprefix$ActivityData>();

            UpdateStatus("Starting execution");

            UpdateStatus("Completing execution");
            UpdateStatus("Result = " + result.ToString());
            return new PluginExecutionResult(result);
        }

        /// <summary>
        /// Logs and displays status messages.
        /// </summary>
        /// <param name="text">The status message to be logged and displayed.</param>
        protected virtual void UpdateStatus(string text)
        {
            Action displayText = new Action(() =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + text);
                status_RichTextBox.AppendText($"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")}  {text}\n");
                status_RichTextBox.Select(status_RichTextBox.Text.Length, 0);
                status_RichTextBox.ScrollToCaret();
            });

            if (status_RichTextBox.InvokeRequired)
            {
                status_RichTextBox.Invoke(displayText);
            }
            else
            {
                displayText();
            }
        }
    }
}
