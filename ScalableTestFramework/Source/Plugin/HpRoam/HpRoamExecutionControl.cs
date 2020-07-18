using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.SolutionApps.HpRoam;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.HpRoam
{
    [ToolboxItem(false)]
    public partial class HpRoamExecutionControl : UserControl, IPluginExecutionEngine
    {
        private DocumentCollectionIterator _documentIterator = null;
        private PluginExecutionData _executionData = null;
        private HpRoamActivityData _activityData = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HpRoamExecutionControl" /> class.
        /// </summary>
        public HpRoamExecutionControl()
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
            _activityData = executionData.GetMetadata<HpRoamActivityData>();
            _executionData = executionData;

            PluginExecutionResult finalResult = null;

            if (_documentIterator == null)
            {
                CollectionSelectorMode mode = _activityData.ShuffleDocuments ? CollectionSelectorMode.ShuffledRoundRobin : CollectionSelectorMode.RoundRobin;
                _documentIterator = new DocumentCollectionIterator(mode);
            }

            try
            {
                activeSession_Label.InvokeIfRequired(n => n.Text = _executionData.SessionId);

                // Execute Print Operation (Push job to Roam)
                switch (_activityData.RoamDocumentSendAction)
                {
                    case DocumentSendAction.Android:
                        PrintToRoamCloudViaPhone();
                        break;
                    case DocumentSendAction.Windows:
                        if (executionData.PrintQueues.Any() && executionData.Documents.Any())
                        {
                            PrintToRoamCloudViaDriver(0);
                        }
                        break;
                    default: //DocumentSend.WebClient not implemented at this time
                        break;
                }

                // Execute Pull Print Operation (Pull job from Roam to device)
                if (_activityData.PhoneDocumentPush)
                {
                    finalResult = PrintFromRoamToDeviceViaPhone();
                }
                else
                {
                    finalResult = PrintFromRoamToDevice();
                }
            }
            catch (Exception ex)
            {
                //If there were errors during the printing operation, we could end up here.
                finalResult = new PluginExecutionResult(PluginResult.Error, ex);
            }

            return finalResult;
        }

        private void PrintToRoamCloudViaPhone()
        {
            RoamAndroidPrintManager androidPrintMgr = new RoamAndroidPrintManager(_executionData, _activityData);
            androidPrintMgr.StatusUpdate += UpdateStatus;
            UpdateDevice(this, new StatusChangedEventArgs($"Android {_activityData.MobileEquipmentId}"));
            UpdateDocumentAction(this, new StatusChangedEventArgs(_activityData.DocumentProcessAction.GetDescription()));
            androidPrintMgr.TimeStatusUpdate += PullPrintManager_TimeStatusUpdate;

            androidPrintMgr.Launch(false);
            UpdateStatus($"Sending document {_activityData.PhoneDocument} from the phone.");
            ExecutionServices.SystemTrace.LogDebug("Pressing 'Add Document' icon.");
            androidPrintMgr.PressAddDocumentIcon();

            ExecutionServices.SystemTrace.LogDebug($"Selecting document {_activityData.PhoneDocument}");
            androidPrintMgr.SelectDocument();

            ExecutionServices.SystemTrace.LogDebug("Uploading selected document.");
            androidPrintMgr.PressUploadButton();
            androidPrintMgr.WaitForUploadComplete();
            androidPrintMgr.SendToHomeScreen();

            ExecutionServices.SystemTrace.LogDebug($"Upload complete. Delaying {_activityData.DelayAfterPrint} seconds.");
            UpdateStatus($"Upload complete.  Delaying { _activityData.DelayAfterPrint} seconds.");
            Thread.Sleep(_activityData.DelayAfterPrint * 1000);
        }

        private void PrintToRoamCloudViaDriver(int delayInSeconds)
        {
            RoamDriverPrintManager roamDriverPrintMgr = new RoamDriverPrintManager();

            UpdateStatus("Signing into the HP Roam Cloud.");
            roamDriverPrintMgr.SignIntoRoamDriver(_executionData.Credential);

            UpdateStatus("Pushing the specified documents to HP Roam...");
            roamDriverPrintMgr.ExecutePrintTask(_executionData, _documentIterator);

            // Execute delay after printing.
            TimeSpan delay = TimeSpan.FromSeconds(delayInSeconds);
            UpdateStatus("Print completed. Delaying pull printing by " + delayInSeconds.ToString() + " seconds.");

            Thread.Sleep(delay);
        }

        /// <summary>
        /// Pulls job from Roam Queue to device via phone.
        /// </summary>
        /// <returns></returns>
        private PluginExecutionResult PrintFromRoamToDeviceViaPhone()
        {
            UpdateStatus("Pulling Roam print job via phone.", true);
            RoamAndroidAppManager androidManager = new RoamAndroidAppManager(_executionData, _activityData);

            androidManager.StatusUpdate += UpdateStatus;
            UpdateDevice(this, new StatusChangedEventArgs($"Android {_activityData.MobileEquipmentId}"));
            UpdateDocumentAction(this, new StatusChangedEventArgs(_activityData.DocumentProcessAction.GetDescription()));
            androidManager.TimeStatusUpdate += PullPrintManager_TimeStatusUpdate;

            return androidManager.ExecutePullPrintOperation();
        }

        /// <summary>
        /// Pulls job from Roam Queue to device via device control panel.
        /// </summary>
        /// <returns></returns>
        private PluginExecutionResult PrintFromRoamToDevice()
        {
            UpdateStatus("Pulling Roam print job via device control panel (OXPd).", true);
            RoamOxpdPullPrintManager oxpdManager = new RoamOxpdPullPrintManager(_executionData, _activityData);

            oxpdManager.StatusUpdate += UpdateStatus;
            oxpdManager.DeviceSelected += UpdateDevice;
            oxpdManager.DocumentActionSelected += UpdateDocumentAction;
            oxpdManager.TimeStatusUpdate += PullPrintManager_TimeStatusUpdate;

            return oxpdManager.ExecutePullPrintOperation();
        }

        /// <summary>
        /// Updates the device displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        private void UpdateDevice(object sender, StatusChangedEventArgs e)
        {
            activeDevice_Label.InvokeIfRequired(n => n.Text = e.StatusMessage);
        }

        /// <summary>
        /// Updates the document process.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs"/> instance containing the event data.</param>
        private void UpdateDocumentAction(object sender, StatusChangedEventArgs e)
        {
            labelDocumentProcessAction.InvokeIfRequired(n => n.Text = e.StatusMessage);
        }

        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="status">The status.</param>
        private void UpdateStatus(string status, bool logStatus = false)
        {
            string statusLine = $"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")} {status}";
            statusRichTextBox.InvokeIfRequired(n => n.AppendText(statusLine + Environment.NewLine));
            if (logStatus)
            {
                ExecutionServices.SystemTrace.LogDebug(status);
            }
        }

        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        private void UpdateStatus(object sender, StatusChangedEventArgs e)
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
