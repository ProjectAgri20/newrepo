using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Print;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Plugin.Printing
{
    /// <summary>
    /// Control that displays what this activity is doing in real-time.
    /// </summary>
    [ToolboxItem(false)]
    public partial class PrintingExecutionControl : UserControl, IPluginExecutionEngine
    {
        private PluginSupport.Print.PrintingEngine _engine = new PluginSupport.Print.PrintingEngine();

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintingExecutionControl"/> class.
        /// </summary>
        public PrintingExecutionControl()
        {
            InitializeComponent();
            _engine.StatusChanged += (s, e) => UpdateStatus(e.StatusMessage);
        }

        /// <summary>
        /// Executes this plugin's workflow using the specified <see cref="PluginExecutionData" />.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            try
            {
                // Thread the print task so that any issues don't crash the user main thread.
                Task<PluginExecutionResult> printTask = Task<PluginExecutionResult>.Factory.StartNew(() => _engine.ProcessActivity(executionData, ConverterProvider.GetMetadataConverters()));
                printTask.Wait();

                return printTask.Result;
            }
            catch (AggregateException ex)
            {
                ExecutionServices.SystemTrace.LogError(ex);
                return new PluginExecutionResult(PluginResult.Failed, ex, "Print failure.");
            }
        }

        /// <summary>
        /// Updates the Status label on the UI
        /// </summary>
        /// <param name="message">Message to post on the UI.</param>
        public void UpdateStatus(string message)
        {
            if (pluginStatus_TextBox.InvokeRequired)
            {
                pluginStatus_TextBox.Invoke(new MethodInvoker(() => UpdateStatus(message)));
                return;
            }

            pluginStatus_TextBox.SuspendLayout();
            pluginStatus_TextBox.AppendText(message);
            pluginStatus_TextBox.AppendText(Environment.NewLine);

            if (pluginStatus_TextBox.Lines.Length > 1000)
            {
                int location = pluginStatus_TextBox.Text.IndexOf(Environment.NewLine, StringComparison.OrdinalIgnoreCase);
                pluginStatus_TextBox.Text = pluginStatus_TextBox.Text.Remove(0, location + 2);
            }

            pluginStatus_TextBox.SelectionStart = pluginStatus_TextBox.Text.Length;
            pluginStatus_TextBox.ScrollToCaret();
            pluginStatus_TextBox.ResumeLayout();
        }
    }
}