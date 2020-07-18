using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.SolutionApps.SafeComUC;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.SafeComUCPullPrinting
{
    [ToolboxItem(false)]
    public partial class SafeComUCPullPrintingExecutionControl : UserControl, IPluginExecutionEngine
    {
        DocumentCollectionIterator _documentCollectionIterator = null;

        private static List<SafeComUCPullPrintAction> _validateTargets = new List<SafeComUCPullPrintAction>()
            { SafeComUCPullPrintAction.PrintAll, SafeComUCPullPrintAction.Print, SafeComUCPullPrintAction.PrintRetain, SafeComUCPullPrintAction.PrintRetainAll };

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeComUCPullPrintingExecutionControl" /> class.
        /// </summary>
        public SafeComUCPullPrintingExecutionControl()
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
            SafeComUCPullPrintingActivityData data = executionData.GetMetadata<SafeComUCPullPrintingActivityData>();

            if(_documentCollectionIterator == null)
            {
                CollectionSelectorMode mode = data.ShuffleDocuments ? CollectionSelectorMode.ShuffledRoundRobin : CollectionSelectorMode.RoundRobin;
                _documentCollectionIterator = new DocumentCollectionIterator(mode);
            }

            SafeComUCPullPrintManager manager = new SafeComUCPullPrintManager(executionData, data);

            manager.StatusUpdate += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;
            manager.DocumentActionSelected += UpdateDocumentAction;
            manager.TimeStatusUpdate += PullPrintManager_TimeStatusUpdate;
            manager.SessionIdUpdate += UpdateSessionId;

            if (executionData.PrintQueues.Any() && executionData.Documents.Any())
            {
                try
                {
                    manager.ExecutePrintJob(_documentCollectionIterator, data.UsePrintServerNotification, data.DelayAfterPrint);
                }
                catch (PrintQueueNotAvailableException ex)
                {
                    //This exception has already been logged in the call to manager.ExecutePrintJob
                    return new PluginExecutionResult(PluginResult.Failed, ex, "Print Failure.");
                }
            }
            return manager.ExecutePullPrintOperation();
        }

        /// <summary>
        /// Updates the device displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        protected void UpdateDevice(object sender, StatusChangedEventArgs e)
        {
            activeDevice_Label.InvokeIfRequired(n => n.Text = e.StatusMessage);
        }
        /// <summary>
        /// Updates the document process.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs"/> instance containing the event data.</param>
        protected void UpdateDocumentAction(object sender, StatusChangedEventArgs e)
        {
            labelDocumentProcessAction.InvokeIfRequired(n => n.Text = e.StatusMessage);
        }
        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="status">The status.</param>
        protected void UpdateStatus(string status)
        {
            string statusLine = $"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")} {status}";
            status_RichTextBox.InvokeIfRequired(n => n.AppendText(statusLine + Environment.NewLine));
        }
        private void UpdateSessionId(object sender, StatusChangedEventArgs e)
        {
            label_sessionId.InvokeIfRequired(n => n.Text = e.StatusMessage);
        }
        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        protected void UpdateStatus(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }
        private void PullPrintManager_TimeStatusUpdate(object sender, TimeStatusEventArgs e)
        {
            // Update the Time Taken when the Step has been completed
            TimeSpan duration = e.EndDateTime.Subtract(e.StartDateTime);
            UpdateStatus($"...Time Taken for Completing the  Step {e.StatusMessage} : {duration}");
        }
    }
}
