using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.$safenamespace$
{
    [ToolboxItem(false)]
    public partial class $safeclassprefix$ExecutionControl : UserControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="$safeclassprefix$ExecutionControl" /> class.
        /// </summary>
        public $safeclassprefix$ExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes this plugin's workflow using the specified <see cref="PluginExecutionData" />.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            $safeclassprefix$ActivityData data = executionData.GetMetadata<$safeclassprefix$ActivityData>();

            UpdateStatus("Starting activity.");

            PluginExecutionResult executionResult = new PluginExecutionResult(PluginResult.Passed);

            UpdateStatus("Finished activity.");
            UpdateStatus($"Result = {executionResult.Result}");

            return executionResult;
        }

        private void UpdateStatus(string message)
        {
            statusRichTextBox.InvokeIfRequired(n =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status = " + message);
                n.AppendText($"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")}  {message}\n");
                n.Select(n.Text.Length, 0);
                n.ScrollToCaret();
            });
        }
    }
}
