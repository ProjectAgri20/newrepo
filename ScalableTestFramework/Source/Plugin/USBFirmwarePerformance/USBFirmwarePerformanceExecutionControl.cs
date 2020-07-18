using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.Plugin.USBFirmwarePerformance
{
    [ToolboxItem(false)]
    public partial class USBFirmwarePerformanceExecutionControl : UserControl, IPluginExecutionEngine
    {
        private PluginExecutionData _executionData;
        private USBFirmwarePerformanceActivityData _activityData;
        private DeviceWorkflowLogger _performanceLogger;
        private ActivityExecutionDetailLog _activityExecutionDetailLog;
        private JediOmniPreparationManager _preparationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="USBFirmwarePerformanceExecutionControl" /> class.
        /// </summary>
        public USBFirmwarePerformanceExecutionControl()
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
            _executionData = executionData;
            _performanceLogger = new DeviceWorkflowLogger(_executionData);
            _activityData = executionData.GetMetadata<USBFirmwarePerformanceActivityData>();

            TimeSpan lockTimeout = TimeSpan.FromMinutes(10);
            TimeSpan holdTimeout = TimeSpan.FromMinutes(60);

            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Failed to Start Upgrade");


            ///Dictionary<string, PluginExecutionResult> results = new Dictionary<string, PluginExecutionResult>();
            if (_executionData.Assets.OfType<IDeviceInfo>().Count() == 0)
            {
                return new PluginExecutionResult(PluginResult.Failed, $"There were no assets retrieved.  If this is a count-based run, your reservation in asset inventory may have expired.", "DeviceInfo Asset error");
            }

            try
            {
                var assetTokens = _executionData.Assets.OfType<IDeviceInfo>().Select(n => new AssetLockToken(n, lockTimeout, holdTimeout));
                _performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
                ExecutionServices.CriticalSection.Run(assetTokens, selectedToken =>
                {
                    _performanceLogger.RecordEvent(DeviceWorkflowMarker.ActivityBegin);

                    IDeviceInfo asset = (selectedToken as AssetLockToken).AssetInfo as IDeviceInfo;
                    IDevice device = DeviceConstructor.Create(asset);
                    ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(_executionData, asset));

                    ExecutionServices.SystemTrace.LogDebug($"Performing update on device {asset.AssetId} at address {asset.Address}");

                    result = UpgradeFirmware(asset);


                    if (result.Result != PluginResult.Passed)
                    {
                        //the update process failed, just return
                        return;
                    }

                    if (!_activityData.ValidateFlash)
                    {
                        _performanceLogger.RecordExecutionDetail(DeviceWorkflowMarker.FirmwareUpdateEnd, device.GetDeviceInfo().FirmwareRevision);
                        return;
                    }

                    int maxRetries = (int)_activityData.ValidateTimeOut.TotalSeconds / 5;
                    if (Retry.UntilTrue(() => HasDeviceRebooted(device), maxRetries, TimeSpan.FromSeconds(5)))
                    {
                        _performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootBegin);
                        UpdateStatus("Device has Rebooted. Waiting for device to boot up...");
                    }
                    else
                    {
                        result = new PluginExecutionResult(PluginResult.Failed, $"Device did not reboot after firmware was uploaded. Please check the device for pending jobs and try again.");
                        return;
                    }


                    ExecutionServices.SystemTrace.LogInfo($"FW Update Complete");

                    //Wait for device to finish rebooting or end
                    ExecutionServices.SystemTrace.LogInfo($"Starting Reboot");
                    UpdateStatus("Waiting for device to boot up...");
                    //_performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootBegin);
                    //We're probably not up and running right away.
                    Thread.Sleep(TimeSpan.FromSeconds(30));

                    
                    ExecutionServices.SystemTrace.LogDebug($"Max Retries: {maxRetries}");
                    int retry = 0;
                    string fwRevision = string.Empty;
                    bool controlPanelUp = false; //Actually webservices, but close enough.
                    bool embeddedServerUp = false;
                    if (Retry.UntilTrue(() => IsDeviceRunning(device, retry++, ref controlPanelUp, ref embeddedServerUp), maxRetries, TimeSpan.FromSeconds(10)))
                    {
                        try
                        {
                            fwRevision = device.GetDeviceInfo().FirmwareRevision;
                            postRevision_textBox.InvokeIfRequired(c => { c.Text = fwRevision; });
                        }
                        catch
                        {
                            fwRevision = string.Empty;
                        }
                        //Validate update passed by comparing starting and ending FW
                        //result = startingFW != fwRevision ? new PluginExecutionResult(PluginResult.Passed, $"Firmware upgraded for device {device.Address}") : new PluginExecutionResult(PluginResult.Failed, "The device firmware upgrade validation failed");
                        result = new PluginExecutionResult(PluginResult.Passed);
                    }
                    else
                    {
                        result = new PluginExecutionResult(PluginResult.Failed,
                            $"Device firmware could not be validated for device:{device.Address} within {_activityData.ValidateTimeOut}");
                    }
                    _performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootEnd);
                    _performanceLogger.RecordEvent(DeviceWorkflowMarker.FirmwareUpdateEnd);

                    ExecutionServices.SystemTrace.LogInfo($"Reboot End");

                    _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "PostUpgradeFirmware", fwRevision);
                    ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);

                    //return result;


                });
            }
            catch (Exception e)
            {
                ExecutionServices.SystemTrace.LogDebug(e);
                UpdateStatus(e.Message);
                result = new PluginExecutionResult(PluginResult.Failed, e.Message);

            }
            _performanceLogger.RecordEvent(DeviceWorkflowMarker.ActivityEnd);
            _performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);
            return result;

        }


        private static bool HasDeviceRebooted(IDevice device)
        {
            var status = device.GetDeviceStatus();
            Application.DoEvents();

            return (status == DeviceStatus.None || status == DeviceStatus.Unknown);
        }

        /// <summary>
        /// Look control panel and web server to come up.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="retry"></param>
        /// <returns></returns>
        private bool IsDeviceRunning(IDevice device, int retry, ref bool controlPanelUp, ref bool embeddedServerUp)
        {
            ExecutionServices.SystemTrace.LogDebug($"Retry Count: {retry}");
            var status = device.GetDeviceStatus();
            Application.DoEvents();

            if (!controlPanelUp)
            {
                controlPanelUp = IsWebServicesUp(device);
                if (controlPanelUp)
                {
                    //_activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "ControlPanelUp", fwRevision);
                    //ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);
                    _performanceLogger.RecordEvent(DeviceWorkflowMarker.WebServicesUp);
                }
            }

            if (!embeddedServerUp)
            {
                embeddedServerUp = (status == DeviceStatus.Running || status == DeviceStatus.Warning || status == DeviceStatus.Down);
                if (embeddedServerUp)
                {
                    //_activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "PostUpgradeFirmware", fwRevision);
                    //ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);
                    _performanceLogger.RecordEvent(DeviceWorkflowMarker.EmbeddedWebServerUp);
                }
            }

            
            return controlPanelUp && embeddedServerUp;
        }
        private bool IsWebServicesUp(IDevice device)
        {
            string bodyString;

            WebClient client = new WebClient();
            try
            {
                bodyString = client.DownloadString($"https://{device.Address}:7627/controlpanel");
                //while the device is still initialising it will hit 404 webexception and won't trigger this

                //_performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootEnd);

                if (!bodyString.Contains("FimService"))
                {
                    bodyString = string.Empty;
                }
            }
            catch (Exception)
            {
                bodyString = string.Empty;
            }

            return !string.IsNullOrEmpty(bodyString);
        }
        #region CDM Code To be deprecated.
        //public static void PUTCDM(string url)
        //{
        //    string final = $@"https://{url}/hp/network/ioConfig/v1/networkInterfaces/wired1/snmpv1v2Config";
        //    string jsonContent = @"{""snmpv1v2Enabled"": ""true"",""accessOption"": ""readWrite"",""readOnlyPublicAllowed"": ""true"",""readOnlyCommunityNameSet"": ""false"",""writeOnlyCommunitryNameSet"": ""false""}";
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(final);
        //    request.Method = "PUT";
        //    request.ContentType = "access-control-allow-headers";
        //    request.Proxy = null;
        //    request.Credentials = new NetworkCredential("admin", "!QAZ2wsx");

        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        //    UTF8Encoding encoding = new System.Text.UTF8Encoding();
        //    Byte[] byteArray = encoding.GetBytes(jsonContent);

        //    request.ContentLength = byteArray.Length;
        //    request.ContentType = @"application/json";

        //    using (Stream dataStream = request.GetRequestStream())
        //    {
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //    }
        //    long length = 0;
        //    try
        //    {
        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            Stream receiveStream = response.GetResponseStream();
        //            length = response.ContentLength;
        //            Console.WriteLine(response.StatusCode);

        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        throw ex;
        //        // Log exception and throw as for GET example above
        //    }
        //}
        #endregion


        private PluginExecutionResult UpgradeFirmware(IDeviceInfo deviceInfo)
        {
            TimeSpan waitTimeSpan = TimeSpan.FromSeconds(4);
            Pacekeeper pacekeeper = new Pacekeeper(TimeSpan.FromSeconds(7));
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);

            IDevice device = DeviceConstructor.Create(deviceInfo);
            var omniDevice = device as JediOmniDevice;
            _preparationManager = new JediOmniPreparationManager(omniDevice);

            if (omniDevice == null)
            {
                result = new PluginExecutionResult(PluginResult.Failed, "This plugin supports only Jedi Omni devices");
                throw new DeviceWorkflowException("This plugin supports only Jedi Omni devices");
            }

            device_textBox.InvokeIfRequired(x => x.Text = device.Address);
            currentRevision_textBox.InvokeIfRequired(x => x.Text = device.GetDeviceInfo().FirmwareRevision);
            _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashMethod", "USB");
            ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);
            //_activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashUpgrade", _activityData.IsDowngrade ? "false" : "true");
            //ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);

            UpdateProgressBar(0);

            _preparationManager.InitializeDevice(true);
            omniDevice.ControlPanel.ScrollPressWait("#hpid-supportTools-homescreen-button",
                "#hpid-supporttools-app-screen", waitTimeSpan);
            pacekeeper.Pause();



            #region handle maintanance popup
            JediOmniPopupManager popupManager = new JediOmniPopupManager(omniDevice);
            bool maintananceWindowNotSolved = false;
            int maintananceretries = 0;
            while (!maintananceWindowNotSolved && maintananceretries < 2)
            {
                if (omniDevice.ControlPanel.WaitForAvailable("#hpid-tree-node-listitem-maintenance", TimeSpan.FromSeconds(5)))
                {
                    Thread.Sleep(5);

                    omniDevice.ControlPanel.PressWait("#hpid-tree-node-listitem-maintenance", "#hpid-settings-app-menu-panel", waitTimeSpan);

                }

                if (omniDevice.ControlPanel.CheckState("#hpid-maintenancemode-failed-feedback-popup", OmniElementState.VisibleCompletely)) // TimeSpan.FromSeconds(4)))
                {
                    popupManager.HandleMaintananceUnavailablePopUp();
                    maintananceWindowNotSolved = true;
                }

                maintananceretries++;
            }

            if (maintananceWindowNotSolved)
            {
                if (omniDevice.ControlPanel.WaitForAvailable("#hpid-tree-node-listitem-maintenance", TimeSpan.FromSeconds(5)))
                {

                    omniDevice.ControlPanel.PressWait("#hpid-tree-node-listitem-maintenance", "#hpid-settings-app-menu-panel", waitTimeSpan);

                }
            }
            #endregion

            if (omniDevice.ControlPanel.WaitForState("#hpid-tree-node-listitem-usbfirmwareupgrade", OmniElementState.Exists, waitTimeSpan))
            {
                omniDevice.ControlPanel.ScrollPressWait("#hpid-tree-node-listitem-usbfirmwareupgrade", "#hpid-usb-firmware-upgrade-screen", waitTimeSpan);
            }
            //var items = omniDevice.ControlPanel.GetIds("div", OmniIdCollectionType.Children);
            //foreach (var item in items)
            //{
            //    ExecutionServices.SystemTrace.LogInfo(item.ToString());
            //}

            pacekeeper.Pause();
            if (omniDevice.ControlPanel.WaitForAvailable("#hpid-firmware-bundles-list", waitTimeSpan))
            {
                //Verify USB firmware?--Lets find out
                if (omniDevice.ControlPanel.CheckState(".hp-listitem-text:contains(_)", OmniElementState.Exists))
                {
                    omniDevice.ControlPanel.ScrollPress(".hp-listitem-text:contains(_)");
                }
                else
                {
                    result = new PluginExecutionResult(PluginResult.Skipped, "Specified firmware bundle not found on USB media");
                    // throw new DeviceWorkflowException("Specified firmware bundle not found on USB media");
                }

                if (omniDevice.ControlPanel.WaitForState("#hpid-setting-install-button", OmniElementState.Enabled,
                    waitTimeSpan))
                {
                    omniDevice.ControlPanel.Press("#hpid-setting-install-button");
                    pacekeeper.Pause();
                }
                else
                {
                    result = new PluginExecutionResult(PluginResult.Failed, "Unable to install firmware");
                    //throw new DeviceWorkflowException("Unable to install firmware");
                }

                bool popUpFound = false;
                var popUpWaitTime = DateTime.Now + TimeSpan.FromSeconds(6);
                string buttonPress = @"";
                while (DateTime.Now < popUpWaitTime)
                {
                    popUpFound = omniDevice.ControlPanel.WaitForState("#hpid-reinstall-button", OmniElementState.Enabled, TimeSpan.FromSeconds(.5));
                    if (popUpFound)
                    {
                        buttonPress = "#hpid-reinstall-button";
                        break;
                    }
                    popUpFound = omniDevice.ControlPanel.WaitForAvailable("#hpid-upgrade-message", TimeSpan.FromSeconds(.5));
                    if (popUpFound)
                    {
                        buttonPress = "#hpid-upgrade-button";
                        break;
                    }
                    popUpFound = omniDevice.ControlPanel.WaitForState("#hpid-rollback-button", OmniElementState.Enabled, TimeSpan.FromSeconds(.5));
                    if (popUpFound)
                    {
                        buttonPress = "#hpid-rollback-button";
                        break;
                    }
                }
                switch (buttonPress)
                {
                    case "#hpid-reinstall-button":
                        return new PluginExecutionResult(PluginResult.Skipped, "This version of firmware is already installed.");
                        //omniDevice.ControlPanel.Press("#hpid-reinstall-button");
                        //pacekeeper.Pause();
                        break;
                    case "#hpid-upgrade-button":
                        if (omniDevice.ControlPanel.WaitForAvailable("#hpid-upgrade-button", waitTimeSpan))
                        {
                            omniDevice.ControlPanel.Press("#hpid-upgrade-button");
                            pacekeeper.Pause();
                        }
                        break;
                    case "#hpid-rollback-button":
                        result = new PluginExecutionResult(PluginResult.Skipped, "Firmware rollback not officially supported, please check that you have the right firmware");
                        return result;
                        break;
                    default:
                        result = new PluginExecutionResult(PluginResult.Failed, "Failed to find upgrade button to press");
                        return result;
                }


                _performanceLogger.RecordEvent(DeviceWorkflowMarker.FirmwareUpdateBegin);
                UpdateProgressBar(20);
                while(omniDevice.ControlPanel.WaitForState("#hpid-firmware-install-progress-popup", OmniElementState.VisibleCompletely, waitTimeSpan))
                {
                    //do nothing
                    Application.DoEvents();

                    //let's not spam the device with check messages
                    Thread.Sleep(waitTimeSpan);
                }
                if (omniDevice.ControlPanel.WaitForAvailable("#hpid-usbfirmwareupgrade-error-msg-popup",
                    waitTimeSpan))
                {
                    var errorMessage = omniDevice.ControlPanel.GetValue(".hp-popup-content", "textContent",
                        OmniPropertyType.Property);
                    omniDevice.ControlPanel.Press("#hpid-usbfirmwareupgrade-error-popup-button-ok");
                    result = new PluginExecutionResult(PluginResult.Failed, errorMessage);
                    throw new DeviceWorkflowException(errorMessage);
                }
                //_performanceLogger.RecordEvent(DeviceWorkflowMarker.FirmwareUpdateEnd);
                UpdateProgressBar(100);
            }
            else
            {
                var upgradeMessage = omniDevice.ControlPanel.GetValue("#hpid-usb-firmware-upgrade-message", "innerText",
                    OmniPropertyType.Property);
                result = new PluginExecutionResult(PluginResult.Failed, upgradeMessage);
//                throw new DeviceWorkflowException(upgradeMessage);
            }

            return result;
        }


        private void ReturnHome(JediOmniDevice omniDevice)
        {
            try
            {
                while (!omniDevice.ControlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(1)))
                {
                    omniDevice.ControlPanel.PressScreen(10, 10);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
            catch (Exception exception)
            {
                throw new DeviceWorkflowException("Unable to return home", exception);
            }
        }

        private void UpdateStatus(string message)
        {
            status_RichTextBox.InvokeIfRequired(n =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status = " + message);
                n.AppendText($"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")}  {message}\n");
                n.Select(n.Text.Length, 0);
                n.ScrollToCaret();
            });
        }

        private void UpdateProgressBar(int progress)
        {
            flash_progressBar.InvokeIfRequired(x => x.Value = progress);
            value_label.InvokeIfRequired(x => x.Text = flash_progressBar.Value + @"%");
        }
    }
}
