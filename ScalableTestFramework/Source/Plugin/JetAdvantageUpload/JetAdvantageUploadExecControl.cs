using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{
    /// <summary>
    /// Control that displays what this activity is doing in real-time.
    /// </summary>
    [ToolboxItem(false)]
    public partial class JetAdvantageUploadExecControl : UserControl, IPluginExecutionEngine
    {
        private JetAdvantageUploadEngine _engine = new JetAdvantageUploadEngine();

        //public int RetryCount
        //{
        //    get { return _activityExecution.RetryCount; }
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="JetAdvantageUploadExecControl"/> class.
        /// </summary>
        public JetAdvantageUploadExecControl()
        {
            InitializeComponent();
            _engine.StatusChanged += _engine_StatusChanged;
        }

        void _engine_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _engine.Initialize();
            var retryManager = new PluginRetryManager(executionData, UpdateStatus);
            return retryManager.Run(() => _engine.ProcessActivity(executionData));
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

            ExecutionServices.SystemTrace.LogDebug(message);

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
            ExecutionServices.SystemTrace.LogNotice(message);
        }

    }
}