using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Plugin.Contention
{
    /// <summary>
    /// Execution control for the Contention plugin.
    /// </summary>
    public partial class ContentionExecControl : UserControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentionExecControl"/> class.
        /// </summary>
        public ContentionExecControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The execute method.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>The <see cref="PluginExecutionResult"/>.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            var manager = new ContentionManager(executionData);
            manager.ActivityStatusChanged += UpdateStatus;
            return manager.RunContentionJob();
        }

        protected void UpdateStatus(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }

        protected virtual void UpdateStatus(string statusMsg)
        {
            status_RichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + statusMsg);
                c.AppendText($"{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}  {statusMsg}\n");
                c.Select(c.Text.Length, 0);
                c.ScrollToCaret();
            });
        }
    }
}
