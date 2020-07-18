using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.LocksmithConfiguration
{
    /// <summary>
    /// Execution control for the LocksmithConfiguration plugin.
    /// </summary>
    public partial class LockSmithConfigurationExecControl : UserControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocksmithConfigurationExecControl"/> class.
        /// </summary>
        public LockSmithConfigurationExecControl()
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
            LockSmithConfigurationActivityManager manager = new LockSmithConfigurationActivityManager();
            UpdateStatus("Locksmith Configuration activity begins");
            manager.ActivityStatusChanged += UpdateStatus;
            return manager.ApplyConfiguration(executionData);
        }

        protected void UpdateStatus(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }

        /// <summary>
        /// Updates the status text in the execution control display.
        /// </summary>
        /// <param name="statusMsg"></param>
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

