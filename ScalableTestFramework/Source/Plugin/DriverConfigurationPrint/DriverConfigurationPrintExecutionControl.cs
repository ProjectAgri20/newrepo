using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.DriverConfigurationPrint
{
    /// <summary>
    /// Control that displays what this activity is doing in real-time.
    /// </summary>
    [ToolboxItem(false)]
    public partial class DriverConfigurationPrintExecutionControl : UserControl, IPluginExecutionEngine
    {
        private DriverConfigPrintingEngine _engine = new DriverConfigPrintingEngine();

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverConfigurationPrintExecutionControl"/> class.
        /// </summary>
        public DriverConfigurationPrintExecutionControl()
        {
            InitializeComponent();
            _engine.StatusChanged += (s, e) => UpdateStatus(e.StatusMessage);
            _engine.StatusUpdate += (s, e) => UpdateStatus(e.StatusMessage);
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
                DriverConfigurationPrintActivityData data = executionData.GetMetadata<DriverConfigurationPrintActivityData>();
                PrintQueueInfo item = executionData.PrintQueues.GetRandom();
                _engine.ConfigureDriverSettings(data.PrinterPreference, item.QueueName);

                return _engine.ProcessActivity(executionData);
            }
            catch (Exception ex)
            {
                return new PluginExecutionResult(PluginResult.Failed, ex);
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