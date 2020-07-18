using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Plugin.GeniusBytesPullPrinting
{
    [ToolboxItem(false)]
    public partial class GeniusBytesPullPrintingExecutionControl : UserControl, IPluginExecutionEngine
    {
        DocumentCollectionIterator _documentCollectionIterator = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="GeniusBytesPullPrintingExecutionControl" /> class.
        /// </summary>
        public GeniusBytesPullPrintingExecutionControl()
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
            GeniusBytesPullPrintingActivityData data = executionData.GetMetadata<GeniusBytesPullPrintingActivityData>();

            GeniusBytesPullPrintManager manager = new GeniusBytesPullPrintManager(executionData, data, executionData.Environment);

            if(_documentCollectionIterator == null)
            {
                CollectionSelectorMode mode = data.ShuffleDocuments ? CollectionSelectorMode.ShuffledRoundRobin : CollectionSelectorMode.RoundRobin;
                _documentCollectionIterator = new DocumentCollectionIterator(mode);
            }
            manager.StatusUpdate += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;
            manager.DocumentActionSelected += UpdateDocumentAction;
            manager.TimeStatusUpdate += PullPrintManager_TimeStatusUpdate;
            manager.SessionIdUpdate += UpdateSessionId;

            if(executionData.PrintQueues.Any() && executionData.Documents.Any())
            {
                try
                {
                    // Thread the print task so that any issues don't crash the user main thread.
                    Task printTask = Task.Factory.StartNew(() => manager.ExecutePrintJob(_documentCollectionIterator, data.UsePrintServerNotification, data.DelayAfterPrint));
                    printTask.Wait();
                }
                catch(AggregateException ex)
                {
                    ExecutionServices.SystemTrace.LogError(ex);
                    return new PluginExecutionResult(PluginResult.Failed, ex, "Print Process Aborted.");
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
            label_activeDevice.InvokeIfRequired(n => n.Text = e.StatusMessage);
        }
        /// <summary>
        /// Updates the document process.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs"/> instance containing the event data.</param>
        protected void UpdateDocumentAction(object sender, StatusChangedEventArgs e)
        {
            label_pullPrintAction.InvokeIfRequired(n => n.Text = e.StatusMessage);
        }
        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="status">The status.</param>
        protected void UpdateStatus(string status)
        {
            string statusLine = $"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")} {status}";
            statusRichTextBox.InvokeIfRequired(n => n.AppendText(statusLine + Environment.NewLine));
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
