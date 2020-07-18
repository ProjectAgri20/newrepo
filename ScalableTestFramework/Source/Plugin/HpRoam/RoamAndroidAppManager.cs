using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using HP.SPS.SES;
using HP.SPS.SES.Helper;

namespace HP.ScalableTest.Plugin.HpRoam
{
    /// <summary>
    /// Manages printing activites from the Roam cloud to a device via an Android phone.
    /// </summary>
    public class RoamAndroidAppManager : AndroidAppManagerBase
    {
        private const string JobSelectToggle = "com.hp.roam:id/select_toggle";

        /// <summary>Initializes a new instance of the <see cref="RoamAndroidAppManager"/> class.</summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="activityData">The activity data.</param>
        public RoamAndroidAppManager(PluginExecutionData pluginExecutionData, HpRoamActivityData activityData) 
            : base(pluginExecutionData, activityData)
        {
            WorkflowLogger = new DeviceWorkflowLogger(pluginExecutionData);
        }

        /// <summary>
        /// Executes the pull print operation as specified by the Plugin Activity Data.
        /// </summary>
        /// <returns>PluginExecutionResult</returns>
        public PluginExecutionResult ExecutePullPrintOperation()
        {
            PluginExecutionResult result = null;
            try
            {
                IEnumerable<IDeviceInfo> devices = _executionData.Assets.OfType<IDeviceInfo>();
                IEnumerable<AssetLockToken> assetTokens = devices.Select(n => new AssetLockToken(n, _activityData.LockTimeouts));

                //If there is no device to run
                if (assetTokens.Count() == 0)
                {
                    //Skip when there are no device to execute on.
                    return new PluginExecutionResult(PluginResult.Skipped, "No Asset available to run.");
                }

                OnStatusUpdate("Waiting for device lock.");
                RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
                ExecutionServices.CriticalSection.Run(assetTokens, selectedToken =>
                {
                    IDeviceInfo deviceInfo = (selectedToken as AssetLockToken).AssetInfo as IDeviceInfo;
                    LogDevice(deviceInfo);

                    PluginRetryManager retryManager = new PluginRetryManager(_executionData, this.OnStatusUpdate);
                    result = retryManager.Run(() => LaunchAndPull(deviceInfo));
                });
            }
            catch (AcquireLockTimeoutException)
            {
                result = new PluginExecutionResult(PluginResult.Skipped, "Could not obtain lock on specified device(s).", "Device unavailable.");
            }
            catch (HoldLockTimeoutException)
            {
                result = new PluginExecutionResult(PluginResult.Error, $"Automation did not complete within {_activityData.LockTimeouts.HoldTimeout}.", "Automation timeout exceeded.");
            }
            finally
            {
                OnStatusUpdate("Finished HP Roam Android Pull Print activity.");
                RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);
            }

            return result;
        }

        /// <summary>
        /// Launches the Roam app and executes the action specified by the plugin activity data.
        /// </summary>
        /// <returns></returns>
        private PluginExecutionResult LaunchAndPull(IDeviceInfo deviceInfo)
        {
            try
            {
                //Prepare for launch
                _controller.PressKey(KeyCode.KEYCODE_WAKEUP); //Power Button
                SendToHomeScreen();

                if (_androidHelper.ExistResourceId("com.android.systemui:id/keyguard_indication_area"))
                {
                    _controller.Swipe(SESLib.To.Up);
                }

                RecordEvent(DeviceWorkflowMarker.ActivityBegin);

                Launch(true);
                ExecutePullPrintAction(deviceInfo);
            }
            catch (DeviceWorkflowException ex)
            {
                return new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error.");
            }
            catch (PrinterBluetoothException pbe)
            {
                return new PluginExecutionResult(PluginResult.Failed, pbe, "Device workflow error.");
            }
            catch (NoJobsFoundException ex)
            {
                return new PluginExecutionResult(PluginResult.Skipped, ex.Message, "Pull print queue empty.");
            }
            catch (PullPrintTimeoutException ex)
            {
                // Pull Printing operation took too long.  No real reason to fail the activity, but need to return some information.
                RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
                return new PluginExecutionResult(PluginResult.Passed, ex.Message, "Pull Print Timeout.");
            }
            catch (Exception ex)
            {
                return new PluginExecutionResult(PluginResult.Failed, ex, "Unknown Exception.");
            }
            finally
            {
                SendToHomeScreen();
                RecordEvent(DeviceWorkflowMarker.ActivityEnd);
            }
            return new PluginExecutionResult(PluginResult.Passed);
        }

        /// <summary>
        /// Executes the pull print action.
        /// Print Single, Print All, Delete Single, etc.
        /// </summary>
        /// <exception cref="DeviceWorkflowException">Requested Android pull print action requested {_activityData.AndroidDocumentAction} not implemented.</exception>
        private void ExecutePullPrintAction(IDeviceInfo deviceInfo)
        {
            //SetInitialJobCount(); //Not tracking
            //VerifyJobsFound(); //Not tracking

            PullPrintJobRetrievalLog pullPrintLog = new PullPrintJobRetrievalLog(_executionData)
            {
                DeviceId = _activityData.MobileEquipmentId,
                JobStartDateTime = DateTime.Now,
                InitialJobCount = -1,
                FinalJobCount = -1,
                JobEndStatus = PluginResult.Failed.ToString(),
                SolutionType = "HPRoamAndroid",
                UserName = _executionData.Credential.UserName,
                NumberOfCopies = 1,
            };

            //Select the print device
            CheckPrinterSelection(deviceInfo.AssetId);

            //Execute the action
            switch (_activityData.AndroidDocumentAction)
            {
                case RoamAndroidAction.PrintAll:
                    SelectAllJobs();
                    ApplyPullPrintDelay();
                    RecordEvent(DeviceWorkflowMarker.PrintAllBegin);
                    PrintSelectedJobs(TimeSpan.FromMinutes(3));
                    RecordEvent(DeviceWorkflowMarker.PrintAllEnd);
                    break;
                case RoamAndroidAction.Print:
                    SelectSingleJob();
                    ApplyPullPrintDelay();
                    RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
                    PrintSelectedJobs(TimeSpan.FromMinutes(1));
                    RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
                    break;
                default:
                    throw new DeviceWorkflowException($"Android pull print action '{_activityData.AndroidDocumentAction}' not implemented.");
            }

            //pullPrintLog.FinalJobCount = GetFinalJobCount; //Set Final job count here if we decide to wire this up.
            pullPrintLog.JobEndDateTime = DateTime.Now;

            ExecutionServices.DataLogger.Submit(pullPrintLog);
        }

        private void CheckPrinterSelection(string assetId)
        {
            const string findPrinterButton = "com.hp.roam:id/find_a_printer_button";
            const string changePrinterButton = "com.hp.roam:id/private_change_printer";

            bool printerFound = false;

            if (_androidHelper.WaitForAvailableResourceIds(findPrinterButton, "com.hp.roam:id/private_printer_button", TimeSpan.FromSeconds(30)))
            {
                if (_androidHelper.ExistResourceId(findPrinterButton))
                {
                    //No printer is selected, so select the specified asset
                    SelectPrinter(findPrinterButton, assetId);
                    printerFound = true;
                }
            }

            if (!printerFound)
            {
                if (!_androidHelper.ExistTextOnResourceId("com.hp.roam:id/private_printer_name", assetId))
                {
                    //A different asset is selected, so change to the specified asset.
                    SelectPrinter(changePrinterButton, assetId);
                }
            }
        }

        private void SelectPrinter(string printerButton, string assetId)
        {                                   
            if (_androidHelper.WaitForAvailableResourceId(printerButton, TimeSpan.FromSeconds(30)))
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
                _controller.Click(new UiSelector().ResourceId(printerButton));

                OnStatusUpdate($"Looking for printer {assetId}.");
                TimeSpan timeout = TimeSpan.FromMinutes(3);
                if (_androidHelper.WaitForAvailableText(assetId, timeout))
                {
                    if (!_controller.Click(new UiSelector().TextContains(assetId)))
                    {
                        throw new DeviceWorkflowException($"Error selecting printer {assetId}.");
                    }
                    RecordEvent(DeviceWorkflowMarker.PrinterListReady, assetId); //More like PrinterFound
                }
                else
                {
                    throw new PrinterBluetoothException($"The desired printer, {assetId}, was not found via bluetooth within {timeout.TotalMinutes} minutes.");
                }
            }
            else
            {
                throw new DeviceWorkflowException("HP Roam Android application did not show within 30 seconds.");
            }
        }

        /// <summary>
        /// Verifies whether the device has finished printing.
        /// </summary>
        private void VerifyDeviceFinishedPrinting(TimeSpan waitTime)
        {
            //Wait for the "Sending to printer..." dialog to disappear within the specified timeframe.
            if (! _androidHelper.WaitForNotDisplayedResourceId("com.hp.roam:id/release_dialog_title", waitTime))
            {
                throw new PullPrintTimeoutException($"'Sending to printer...' dialog did not close within {waitTime.TotalSeconds} seconds.");
            }

            //This section waits for each individual print job's printing icon to disappear
            if (_androidHelper.WaitForAvailableResourceId("com.hp.roam:id/printing_job_details", TimeSpan.FromSeconds(30)))
            {
                if (!_androidHelper.WaitForNotDisplayedResourceId("com.hp.roam:id/printing_job_details", waitTime))
                {
                    throw new PullPrintTimeoutException($"Job(s) did not finish printing within {waitTime.TotalSeconds} seconds.");
                }
            }
        }

        /// <summary>
        /// Prints selected jobs.
        /// </summary>
        /// <param name="assetId">The asset Id the printer name should contain.</param>
        private void PrintSelectedJobs(TimeSpan waitTime)
        {
            OnStatusUpdate("Sending document(s) to the printer.");
            bool onScreen = _androidHelper.WaitForAvailableText("Send to Printer", TimeSpan.FromSeconds(30));
            if (onScreen)
            {
                if (_androidHelper.ResourceIdEnabled("com.hp.roam:id/private_printer_button"))
                {
                    if (!_controller.Click(new UiSelector().TextContains("Send to Printer")))
                    {
                        throw new DeviceWorkflowException("Error clicking 'Send to Printer' button.");
                    }
                }
                else
                {
                    throw new NoJobsFoundException($"No jobs found in My HP Roam Queue for {_executionData.Credential.UserName}.");
                }
            }

            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
            VerifyDeviceFinishedPrinting(waitTime);
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            SendToHomeScreen();
        }

        /// <summary>
        /// Not measuring this for now.  Will wait for a specific request.
        /// </summary>
        private void SelectAllJobs()
        {
            //Select All if not all selected by default.
            if (_androidHelper.ExistTextOnResourceId(JobSelectToggle, "Select All"))
            {
                if (!_controller.Click(new UiSelector().ResourceId(JobSelectToggle)))
                {
                    throw new DeviceWorkflowException("Error selecting all jobs in the queue.");
                }
            }
        }

        /// <summary>
        /// Assumes we are on the "My HP Roam Queue" page.  No perfomrance markers set here.
        /// </summary>
        private void SelectSingleJob()
        {
            UiSelector selector = new UiSelector();
            //First, deselect all jobs if Select All is on by default.
            if (_androidHelper.ExistTextOnResourceId(JobSelectToggle, "Deselect All"))
            {
                if (!_controller.Click(selector.ResourceId(JobSelectToggle)))
                {
                    throw new DeviceWorkflowException("Error deselecting all jobs in the queue.");
                }

                selector.Clear();
                //Select the first job in the queue
                if (!_controller.Click(selector.ResourceId("com.hp.roam:id/ready_selected_iv")))
                {
                    throw new NoJobsFoundException($"No jobs found in My HP Roam Queue for {_executionData.Credential.UserName}.");
                }
            }
        }

        private void ApplyPullPrintDelay()
        {
            //Allow the device time to get info about the doc(s) from the Roam server.
            OnStatusUpdate($"Delaying {_activityData.DelayBeforePullPrint} seconds.");
            Delay.Wait(TimeSpan.FromSeconds(_activityData.DelayBeforePullPrint));
        }

        /// <summary>
        /// Logs workflow information on the device.
        /// </summary>
        /// <param name="deviceInfo">The device information.</param>
        private void LogDevice(IDeviceInfo deviceInfo)
        {
            var log = new ActivityExecutionAssetUsageLog(_executionData, deviceInfo.AssetId);
            ExecutionServices.DataLogger.Submit(log);
        }
    }

    /// <summary>Android enumerations</summary>
    public enum RoamAndroidAction
    {
        /// <summary>Enumeration for printing all listed documents.</summary>
        [Description("Print all")]
        PrintAll,
        /// <summary>Print a single (first) document from the list of print Jobs if possible.</summary>
        [Description("Print")]
        Print,
        /// <summary>Delete a single (first) document from the list of print jobs if possible</summary>
        [Description("Delete")]
        Delete,
    }
}
