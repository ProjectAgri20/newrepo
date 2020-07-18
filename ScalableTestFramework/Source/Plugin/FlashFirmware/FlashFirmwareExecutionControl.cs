using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.FlashFirmware.BashLog;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HP.ScalableTest.Plugin.FlashFirmware
{
    /// <summary>
    /// Used to execute the activity of the FlashFirmware plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class FlashFirmwareExecutionControl : UserControl, IPluginExecutionEngine
    {
        private FlashFirmwareActivityData _data;
        private PluginExecutionData _executionData;
        private PrintDeviceInfo _asset;
        private string _firmwareFileName;
        private string _firmwareVersion;
        private IDevice _device;
        private JediOmniPreparationManager _preparationManager;
        private string _sessionId;

        // private bool _flashDowngrade;
        private bool _rebootEndEventLogged;

        private int _flashTimeoutPeriod;
        private readonly BackgroundWorker _flashBackgroundWorker = new BackgroundWorker();
        private bool _foundShell;
        private const string ShellPrompt = "1:Continue";

        private BashLogCollectorClient _client;
        private string _bashLogCollectorServiceHost;
        public PluginExecutionResult Result { get; set; }
        private Thread _breakIntoEfiShellThread;
        private readonly TimeSpan _commandTimeSpan = TimeSpan.FromSeconds(1);
        private DeviceWorkflowLogger _performanceLogger;
        private ActivityExecutionDetailLog _activityExecutionDetailLog;
        private readonly HttpClientHandler _httpClientHandler;
        private HttpClient _httpClient;
        private string _csrfToken;

        /// <summary>
        /// Initializes a new instance of the FlashFirmwareExecutionControl class.
        /// </summary>
        public FlashFirmwareExecutionControl()
        {
            InitializeComponent();

            _flashBackgroundWorker.DoWork += FlashBackgroundWorker_DoWork;
            _flashBackgroundWorker.RunWorkerCompleted += FlashBackgroundWorker_RunWorkerCompleted;
            Result = new PluginExecutionResult(PluginResult.Passed);
            _httpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = false,
                MaxAutomaticRedirections = 2
            };
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        /// <summary>
        /// Execute the task of the FlashFirmware activity.
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _executionData = executionData;
            _data = _executionData.GetMetadata<FlashFirmwareActivityData>();
            _performanceLogger = new DeviceWorkflowLogger(_executionData);

            if (executionData.Environment.PluginSettings.ContainsKey("BashLogCollectorServiceHost"))
            {
                _bashLogCollectorServiceHost = executionData.Environment.PluginSettings["BashLogCollectorServiceHost"];
            }

            Result = new PluginExecutionResult(PluginResult.Passed);
            _asset = executionData.Assets.OfType<PrintDeviceInfo>().First();
            ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(_executionData, _asset));
            ValidateFirmwareBundle();
            if (Result.Result == PluginResult.Passed)
            {
                try
                {
                    UpgradeFirmware(_data.FlashMethod);
                }
                catch (Exception e)
                {
                    //we have an error while performing firmware upgrade
                    UpdateStatus(e.Message);
                    if (Result.Result == PluginResult.Passed)
                    {
                        Result = new PluginExecutionResult(PluginResult.Failed, e.JoinAllErrorMessages());
                    }
                }
            }
            _device.Dispose();
            return Result;
        }

        /// <summary>
        /// Define the first step in the activity's task.
        /// </summary>
        private void ValidateFirmwareBundle()
        {
            string[] separator = { Environment.NewLine };
            string bundleFile = string.Empty;
            string modelName = string.Empty;
            UpdateStatus("Validating Firmware bundle for the selected asset(s)");
            //first copy the firmware file
            var tempDumpDirectory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "Dump"));
            var dumpUtilityFileName = Path.Combine(tempDumpDirectory.FullName, "FimDumpUtility.exe");
            File.WriteAllBytes(dumpUtilityFileName, ResourceDump.FimDumpUtility);

            _firmwareFileName = Path.Combine(tempDumpDirectory.FullName, Path.GetFileName(_data.FirmwareFileName));
            File.Copy(_data.FirmwareFileName, _firmwareFileName, true);

            //calculate the probable time to upload this file to the device taking 100mb as 5 minutes
            FileInfo fInfo = new FileInfo(_firmwareFileName);
            var fileSize = fInfo.Length / (1024 * 1024);
            if (fileSize < 100)
                fileSize = 100;

            var fileSizeMb = ((int)(fileSize / 100.0)) * 8;
            _flashTimeoutPeriod = (int)TimeSpan.FromMinutes(fileSizeMb).TotalMilliseconds;

            try
            {
                ExecutionServices.SystemTrace.LogDebug($"Executing FIM Dump Utility on {_firmwareFileName} and dumping it to {tempDumpDirectory.FullName}");
                var result = ProcessUtil.Execute(dumpUtilityFileName,
                    $"-o {tempDumpDirectory.FullName} \"{_firmwareFileName}\"");
                var outputLines = result.StandardOutput.Split(separator, StringSplitOptions.None);
                ExecutionServices.SystemTrace.LogDebug(outputLines);
                bundleFile = Path.Combine(outputLines.ElementAt(0).Substring(18), "Bundle.manifest.xml");
            }
            catch (Exception e)
            {
                ExecutionServices.SystemTrace.LogDebug(e.JoinAllErrorMessages());
                //let try to force open the bundle file
                if (File.Exists(Path.Combine(tempDumpDirectory.FullName, "Bundle.manifest.xml")))
                {
                    bundleFile = Path.Combine(tempDumpDirectory.FullName, "Bundle.manifest.xml");
                }
            }

            if (File.Exists(bundleFile))
            {
                XDocument xDoc = XDocument.Load(bundleFile);
                modelName = xDoc.Element("Bundle")?.Attribute("Name")?.Value;
                if (string.IsNullOrEmpty(modelName))
                {
                    Result = new PluginExecutionResult(PluginResult.Failed,
                        "Unable to determine model name from the bundlefile");
                }

                _firmwareVersion = xDoc.Element("Bundle")?.Attribute("Version")?.Value.Split(' ').First();
            }
            else
            {
                ExecutionServices.SystemTrace.LogDebug("Unable to find the bundle file information file, skipping validation check.");
            }

            try
            {
                _device = DeviceConstructor.Create(_asset);

                var deviceInfo = _device.GetDeviceInfo();

                //check the model name at the end with the device.
                if (!string.IsNullOrEmpty(modelName) && deviceInfo.ModelName.EndsWith(Regex.Match(modelName.Split(' ').Last(), @"\d+").Value,
                    StringComparison.OrdinalIgnoreCase))
                {
                    ExecutionServices.SystemTrace.LogDebug(
                        $"Upgrading {deviceInfo.FirmwareRevision} to {_firmwareVersion} version for {modelName}");
                    UpdateStatus($"Validated firmware for device: {_device.Address}");
                }
                else
                {
                    ExecutionServices.SystemTrace.LogDebug(
                        $"Firmware model {modelName} does not match device model:{deviceInfo.ModelName}");
                    UpdateStatus($"Validation of firmware upgrade for device: {_device.Address} failed");
                    UpdateStatus("Proceeding with warning...");
                }
            }
            catch (DeviceSecurityException)
            {
                throw;
            }
            catch (Exception e)
            {
                Result = new PluginExecutionResult(PluginResult.Failed, "Device couldn't be added");
                throw new Exception("Device couldn't be added", e);
            }
        }

        private void UpgradeFirmware(FlashMethod flashMethod)
        {
            UpdateStatus("Performing firmware upgrade");
            if (_device == null)
                return;
            var firmwareTriage = TriageFactory.Create(_device, _executionData);
            DateTime firmwareUpdateStartTime = DateTime.UtcNow;
            _performanceLogger.RecordExecutionDetail(DeviceWorkflowMarker.FirmwareUpdateBegin, _device.GetDeviceInfo().FirmwareRevision);
            SignIn(_device.Address, _device.AdminPassword);
            _csrfToken = GetCsrfToken($"https://{_device.Address}/hp/device/FirmwareUpgrade/Save", ref _sessionId);
            SetBackupRestoreSetting(_csrfToken, _data.AutoBackup);
            switch (flashMethod)
            {
                case FlashMethod.Ews:
                    FlashFirmwareEws();
                    break;

                case FlashMethod.ControlPanel:
                    {
                        if (!_asset.Attributes.HasFlag(AssetAttributes.ControlPanel))
                        {
                            Result = new PluginExecutionResult(PluginResult.Skipped,
                                "Device does not have control panel and cannot be upgraded by Control Panel method.");
                            return;
                        }
                        FlashFirmwareControlPanel();
                    }
                    break;

                case FlashMethod.Bios:
                    FlashFirmwareBios();
                    break;
               
            }

            if (Result.Result != PluginResult.Passed)
            {
                GetTriageInfo(firmwareTriage);
                return;
            }

            if (!_data.ValidateFlash)
            {
                _performanceLogger.RecordExecutionDetail(DeviceWorkflowMarker.FirmwareUpdateEnd, _device.GetDeviceInfo().FirmwareRevision);
                return;
            }
            //the Device Get Status call takes 10 seconds additionally, as it does 2 retries with 5 seconds interval
            int maxRetries = (int)_data.ValidateTimeOut.TotalSeconds / (15);
            ExecutionServices.SystemTrace.LogDebug($"Checking whether the device has rebooted for {maxRetries / 10} tries with 5 second interval.");
           

            if (Retry.UntilTrue(() => HasDeviceRebooted(_device), maxRetries / 10, TimeSpan.FromSeconds(5)))
            {
                int copyTime = (int)(DateTime.UtcNow - firmwareUpdateStartTime).TotalSeconds;
                _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData,
                    "FirmwareFlashCopyTime", copyTime.ToString());
                ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);
                _performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootBegin);
                UpdateStatus("Device has Rebooted. Waiting for device to boot up...");
                UpdateStatus($"Firmware copy process took {copyTime} seconds.");
            }
            else
            {
                //due to a race issue in control panel upgrade which results in a call going for checking the popup and the device reboots, which results in 5 minute timeout.
                //check if the device has already rebooted and is in the target version.
                if (IsDeviceRunning(_device))
                {
                    if (string.IsNullOrEmpty(_firmwareVersion))
                    {
                        ExecutionServices.SystemTrace.LogDebug("There was an issue in finding out the target firmware version using FIM dump utility. " +
                                                               "We do not have a method to check if the target firmware is already flashed or not. Failing the activity for now.");
                        ExecutionServices.SystemTrace.LogDebug($"Current Firmware version is {_device.GetDeviceInfo().FirmwareRevision}");
                        ExecutionServices.SystemTrace.LogDebug("Device did not reboot.");
                        GetTriageInfo(firmwareTriage);
                        Result = new PluginExecutionResult(PluginResult.Failed,
                            "Device did not reboot after firmware was uploaded. Please check the device for pending jobs and try again.");
                        return;
                    }

                    if (_device.GetDeviceInfo().FirmwareRevision == _firmwareVersion)
                    {
                        //device has flashed to the target version
                        ExecutionServices.SystemTrace.LogDebug(
                            $"The device has rebooted with the target firmware version. Current firmware version is {_firmwareVersion}");
                    }
                }
                else
                {
                    ExecutionServices.SystemTrace.LogDebug("Device did not reboot.");
                    GetTriageInfo(firmwareTriage);
                    Result = new PluginExecutionResult(PluginResult.Failed,
                        "Device did not reboot after firmware was uploaded. Please check the device for pending jobs and try again.");
                    return;
                }
            }

            DateTime deviceRebootStartTime = DateTime.UtcNow;
            ExecutionServices.SystemTrace.LogDebug(
                $"Checking whether the device is in Running state for {maxRetries} tries with 5 second interval.");
            if (Retry.UntilTrue(() => IsDeviceRunning(_device), maxRetries, TimeSpan.FromSeconds(5)))
            {
                ExecutionServices.SystemTrace.LogDebug(
                    $"Checking whether JDI has initialised for {maxRetries / 5} tries with 5 second interval.");
                if (Retry.UntilTrue(() => IsJetDirectUp(_device), maxRetries / 5, TimeSpan.FromSeconds(5)))
                {
                    UpdateStatus("JetDirect Initialised.");
                }
                else
                {
                    ExecutionServices.SystemTrace.LogDebug("JetDirect has not initialised.");
                    GetTriageInfo(firmwareTriage);
                }

                ExecutionServices.SystemTrace.LogDebug(
                    $"Checking whether the device web services are functional {maxRetries / 5} tries with 5 second interval.");
                if (Retry.UntilTrue(() => IsWebServicesUp(_device), (maxRetries / 5), TimeSpan.FromSeconds(5)))
                {
                    UpdateStatus("Jedi WebServices Initialised.");
                    int rebootTime = (int)(DateTime.UtcNow - deviceRebootStartTime).TotalSeconds;
                    _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData,
                        "FirmwareFlashRebootTime", rebootTime.ToString());
                    ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);
                    UpdateStatus($"Device Reboot took {rebootTime} seconds.");
                    if (_data.ValidateFlash)
                        ValidateFlash(_device);

                    postRevision_textBox.InvokeIfRequired(x => x.Text = _device.GetDeviceInfo().FirmwareRevision);
                }
                else
                {
                    ExecutionServices.SystemTrace.LogDebug("Jedi Web Services has not initialised.");
                    GetTriageInfo(firmwareTriage);
                    Result = new PluginExecutionResult(PluginResult.Failed,
                        $"Device firmware could not be validated for device:{_device.Address}");
                }
            }
            else
            {
                TraceFactory.Logger.Debug("Device has not come to ready state. Collecting Triage Info.");
                GetTriageInfo(firmwareTriage);
                Result = new PluginExecutionResult(PluginResult.Failed,
                    $"Device firmware could not be validated for device:{_device.Address}");
                return;
            }

            _performanceLogger.RecordEvent(DeviceWorkflowMarker.FirmwareUpdateEnd);
            _performanceLogger.RecordExecutionDetail(DeviceWorkflowMarker.FirmwareUpdateEnd, _device.GetDeviceInfo().FirmwareRevision);

            if (File.Exists(_firmwareFileName))
                File.Delete(_firmwareFileName);
        }
      
        private void GetTriageInfo(ITriage firmwareTriage)
        {
            ExecutionServices.SystemTrace.LogDebug($"Message Center: {GetMessageCenter()}");
            //the update process failed, just return
            GetControlPanelImage();
            firmwareTriage.CollectTriageData();
            try
            {
                firmwareTriage.Submit();
            }
            catch (Exception e)
            {
                ExecutionServices.SystemTrace.LogError(e.JoinAllErrorMessages());
            }

            if (File.Exists(_firmwareFileName))
                File.Delete(_firmwareFileName);
        }

        #region Bios

        private void FlashFirmwareBios()
        {
            var omniDevice = _device as JediDevice;
            if (omniDevice == null)
            {
                throw new DeviceWorkflowException("This plugin only supports Jedi Omni devices");
            }
            if (string.IsNullOrEmpty(_bashLogCollectorServiceHost))
            {
                throw new DeviceWorkflowException("Bash Collector Service setting is not defined. Cannot proceed ahead with Firmware Flash.");
            }
            UpdateProgressBar(0);
            _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashMethod", "Boot Loader");
            ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);
            _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashUpgrade", _data.IsDowngrade ? "false" : "true");
            ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);

            _client = new BashLogCollectorClient(_bashLogCollectorServiceHost);
            var assetId = _asset.AssetId;
            UpdateStatus($"{_device.Address}: Restarting the device...");
            RestartDevice(omniDevice);
            UpdateProgressBar(10);
            UpdateStatus($"{_device.Address}: Breaking into Bios and Shell...");
            BreakIn(assetId);
            UpdateProgressBar(20);

            var firmwareFilePath = GetFirmwareFileOnUsb(assetId, Path.GetFileName(_data.FirmwareFileName));
            if (string.IsNullOrEmpty(firmwareFilePath))
            {
                Result = new PluginExecutionResult(PluginResult.Skipped, "Firmware file not found on Media");
                _client.Dispose();
                _device.Dispose();
                return;
            }
            UpdateStatus($"{_device.Address}: Found firmware file on Media - {firmwareFilePath}");
            UpdateStatus($"{_device.Address}: Updating firmware...");
            UpdateProgressBar(30);
            _performanceLogger.RecordEvent(DeviceWorkflowMarker.FirmwareUpdateBegin);
            FlashFirmwareShell(assetId, firmwareFilePath);
            UpdateStatus($"{_device.Address}: Firmware Update completed.");
            UpdateProgressBar(100);
            //  _performanceLogger.RecordEvent(DeviceWorkflowMarker.FirmwareUpdateEnd);
            _client.Dispose();
            _device.Dispose();
        }

        /// <summary>
        /// Break into the EFI shell from a reboot; this method does not reboot the UUT; this method expects the UUT to be rebooting
        /// </summary>
        private void BreakIn(string assetId)
        {
            _foundShell = false;
            const Int32 timeoutToBreakIntoEfiShellInMsecs = 240000;

            _breakIntoEfiShellThread = new Thread(() => FindShell(assetId)); //Start a new thread to signal breaking into efi shell
            _breakIntoEfiShellThread.Start();

            DateTime timeLimitToBreakIntoEfiShell = DateTime.Now.AddMilliseconds(timeoutToBreakIntoEfiShellInMsecs);
            //Send the break in commands until arriving in shell or the timeout passes
            do
            {
                // SPAD unlock command
                _client.WriteStream(assetId, 0xff);
                _client.WriteStream(assetId, 0xf4);

                _client.WriteStream(assetId, CommonConstants.bESC);          // Send Escape
                Thread.Sleep(_commandTimeSpan);
            }
            while (!_foundShell && (DateTime.Now < timeLimitToBreakIntoEfiShell));

            _client.WriteStream(assetId, CommonConstants.bACK);// Send Ctrl+F
            _client.WriteStream(assetId, CommonConstants.bCR); // Send Continue

            _breakIntoEfiShellThread.Join();

            if (!_foundShell)
            {
                throw new RemotingException("Failed to get to the shell prompt");
            }

            //slow down to make sure all the data is received on  the stream
            Thread.Sleep(_commandTimeSpan);
        }

        private void FindShell(string assetId)
        {
            const Int32 waitTimeoutToDetectEfiShellPromptInMsecs = 1 * CommonConstants.MILLISECONDS_PER_MINUTE;

            try
            {
                //wait for the shell prompt
                _client.WaitFor(assetId, waitTimeoutToDetectEfiShellPromptInMsecs, ShellPrompt, true);
                _foundShell = true;
            }
            catch (Exception e)
            {
                ExecutionServices.SystemTrace.LogDebug(e.Message);
                ExecutionServices.SystemTrace.LogDebug(e.StackTrace);
                _foundShell = false;
            }
        }

        private void RestartDevice(JediDevice device)
        {
            device.Snmp.Set("1.3.6.1.4.1.11.2.3.9.4.2.1.1.1.65.0", 9);
            while (NetworkUtil.PingUntilTimeout(IPAddress.Parse(device.Address), _commandTimeSpan))
            {
                //do nothing
            }
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        private void FlashFirmwareShell(string assetId, string firmwareFileName)
        {
            _client.WriteToEfiShell(assetId, $"fimdl fs {firmwareFileName}");
            Thread.Sleep(_commandTimeSpan);
            _client.WaitForLine(assetId, (int)TimeSpan.FromSeconds(120).TotalMilliseconds, "Closing repository");
            Thread.Sleep(5000);
            _client.WriteToEfiShell(assetId, "reset");
        }

        private string GetFirmwareFileOnUsb(string assetId, string firmwareFile)
        {
            _client.WriteToEfiShell(assetId, "map -t hd -sfo");
            Thread.Sleep(_commandTimeSpan);
            var outputString = _client.ReadCommandOutputStream(assetId);
            outputString = Normalize(outputString);
            var usbDrives = outputString.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains("USB"));
            foreach (var usbDrive in usbDrives)
            {
                var usbDriveLetter = usbDrive.Split(',').ElementAt(1).Replace("\"", "");
                _client.WriteToEfiShell(assetId, usbDriveLetter);
                Thread.Sleep(_commandTimeSpan);
                _client.WriteToEfiShell(assetId, $"ls -r -a {firmwareFile} -b");
                Thread.Sleep(5000); //we have to wait for the command to complete
                outputString = _client.ReadCommandOutputStream(assetId);
                outputString = Normalize(outputString);
                if (outputString.Contains(firmwareFile))
                {
                    var filePath = outputString.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ElementAt(0);
                    return filePath.Substring(filePath.IndexOf(usbDriveLetter, StringComparison.Ordinal));
                }
            }
            return string.Empty;
        }

        #endregion Bios

        #region ControlPanel

        private void FlashFirmwareControlPanel()
        {
            TimeSpan waitTimeSpan = TimeSpan.FromSeconds(5);
            Pacekeeper pacekeeper = new Pacekeeper(TimeSpan.FromSeconds(2));

            var omniDevice = _device as JediOmniDevice;
            if (omniDevice == null)
            {
                FlashFirmwareControlPanelWindJammer();
                return;
            }
            _preparationManager = new JediOmniPreparationManager(omniDevice);
            device_textBox.InvokeIfRequired(x => x.Text = _device.Address);
            currentRevision_textBox.InvokeIfRequired(x => x.Text = _device.GetDeviceInfo().FirmwareRevision);
            _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashMethod", "USB");
            ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);
            _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashUpgrade", _data.IsDowngrade ? "false" : "true");
            ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);
            UpdateProgressBar(0);
            try
            {
                omniDevice.ControlPanel.SignalUserActivity();
            }
            catch (Exception e)
            {
                ExecutionServices.SystemTrace.LogDebug(e.Message);
                omniDevice.ControlPanel.PressScreen(-1, -1);
            }

            _preparationManager.NavigateHome();
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

            #endregion handle maintanance popup

            if (omniDevice.ControlPanel.WaitForState("#hpid-tree-node-listitem-usbfirmwareupgrade", OmniElementState.Exists, waitTimeSpan))
                omniDevice.ControlPanel.ScrollPressWait("#hpid-tree-node-listitem-usbfirmwareupgrade", "#hpid-usb-firmware-upgrade-screen", waitTimeSpan);

            pacekeeper.Pause();
            if (omniDevice.ControlPanel.WaitForAvailable("#hpid-firmware-bundles-list", waitTimeSpan))
            {
                if (omniDevice.ControlPanel.CheckState(".hp-listitem-text:contains(" + _firmwareVersion + ")", OmniElementState.Exists))
                {
                    omniDevice.ControlPanel.ScrollPress(".hp-listitem-text:contains(" + _firmwareVersion + ")");
                }
                else
                {
                    Result = new PluginExecutionResult(PluginResult.Skipped, "Specified firmware bundle not found on USB media");
                    throw new DeviceWorkflowException("Specified firmware bundle not found on USB media");
                }

                if (omniDevice.ControlPanel.WaitForState("#hpid-setting-install-button", OmniElementState.Enabled,
                    waitTimeSpan))
                {
                    omniDevice.ControlPanel.Press("#hpid-setting-install-button");
                    pacekeeper.Pause();
                }
                else
                {
                    Result = new PluginExecutionResult(PluginResult.Failed, "Unable to install firmware");
                    throw new DeviceWorkflowException("Unable to install firmware");
                }

                if (omniDevice.ControlPanel.WaitForState("#hpid-reinstall-button", OmniElementState.Enabled,
                    waitTimeSpan))
                {
                    //let's take the assumption that the user wants to reinstall and press this button
                    omniDevice.ControlPanel.Press("#hpid-reinstall-button");
                    pacekeeper.Pause();
                }
                else if (omniDevice.ControlPanel.WaitForAvailable("#hpid-upgrade-message", waitTimeSpan))
                {
                    if (omniDevice.ControlPanel.WaitForAvailable("#hpid-upgrade-button", waitTimeSpan))
                    {
                        omniDevice.ControlPanel.Press("#hpid-upgrade-button");
                        pacekeeper.Pause();
                    }
                }
                else if (omniDevice.ControlPanel.WaitForAvailable("#hpid-rollback-message", waitTimeSpan))
                {
                    if (omniDevice.ControlPanel.WaitForAvailable("#hpid-rollback-button", waitTimeSpan))
                    {
                        omniDevice.ControlPanel.Press("#hpid-rollback-button");
                        pacekeeper.Pause();
                    }
                }

                _performanceLogger.RecordEvent(DeviceWorkflowMarker.FirmwareUpdateBegin);
                UpdateProgressBar(30);

                try
                {
                    DateTime startUpgradeDateTime = DateTime.Now;
                    while (omniDevice.ControlPanel.WaitForState(".hp-popup.hp-with-header", OmniElementState.Enabled, waitTimeSpan) && !omniDevice.ControlPanel.WaitForAvailable("#hpid-usbfirmwareupgrade-error-msg-popup", waitTimeSpan))
                    {
                        //do nothing
                        Application.DoEvents();
                        //let's not spam the device with check messages
                        Thread.Sleep(waitTimeSpan + waitTimeSpan);
                        if (DateTime.Now >= (startUpgradeDateTime + _data.ValidateTimeOut))
                        {
                            ExecutionServices.SystemTrace.LogDebug($"The upgrade is taking too long, popup message still seen after the timeout {_data.ValidateTimeOut}.");
                            Result = new PluginExecutionResult(PluginResult.Failed, "The upgrade is taking too long, popup message still seen after the timeout.");
                        }
                        
                    }
                    if (omniDevice.ControlPanel.WaitForAvailable("#hpid-usbfirmwareupgrade-error-msg-popup",
                        waitTimeSpan))
                    {
                        var errorMessage = omniDevice.ControlPanel.GetValue(".hp-popup-content", "textContent",
                            OmniPropertyType.Property);
                        omniDevice.ControlPanel.Press("#hpid-usbfirmwareupgrade-error-popup-button-ok");
                        Result = new PluginExecutionResult(PluginResult.Failed, errorMessage);
                        throw new DeviceWorkflowException(errorMessage);
                    }
                }
                catch (Exception)
                {
                    //ignore as the device would have restarted.
                }

                UpdateProgressBar(100);
            }
            else
            {
                var upgradeMessage = omniDevice.ControlPanel.GetValue("#hpid-usb-firmware-upgrade-message", "innerText",
                    OmniPropertyType.Property);
                Result = new PluginExecutionResult(PluginResult.Failed, upgradeMessage);
                throw new DeviceWorkflowException(upgradeMessage);
            }
        }

        private void FlashFirmwareControlPanelWindJammer()
        {
            var jediWindjammerDevice = _device as JediWindjammerDevice;
            if (jediWindjammerDevice == null)
            {
                Result = new PluginExecutionResult(PluginResult.Failed, "This plugin only supports Jedi devices.");
                return;
            }
            device_textBox.InvokeIfRequired(x => x.Text = _device.Address);
            currentRevision_textBox.InvokeIfRequired(x => x.Text = _device.GetDeviceInfo().FirmwareRevision);
            _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashMethod", "USB");
            ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);
            _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashUpgrade", _data.IsDowngrade ? "false" : "true");
            ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);

            UpdateProgressBar(0);
            jediWindjammerDevice.ControlPanel.PressKey(JediHardKey.Reset);
            jediWindjammerDevice.ControlPanel.WaitForControl("ServiceabilityApp", TimeSpan.FromSeconds(5));
            jediWindjammerDevice.ControlPanel.PressWait("ServiceabilityApp", "ServiceabilityAppMainForm", TimeSpan.FromSeconds(5));
            jediWindjammerDevice.ControlPanel.WaitForControl("CurrentFirmwareVersion", TimeSpan.FromSeconds(5));
            jediWindjammerDevice.ControlPanel.PressWait("CurrentFirmwareVersion", "UsbFirmwareUpgradeSettings", TimeSpan.FromSeconds(5));

            int retry = 0;
            while (jediWindjammerDevice.ControlPanel.GetControls().FirstOrDefault(x => x.Contains(_firmwareVersion)) == null && retry <= 15)
            {
                jediWindjammerDevice.ControlPanel.PressKey(JediHardKey.Down);
                Thread.Sleep(1000);
                retry++;
            }
            var targetBundleControl = jediWindjammerDevice.ControlPanel.GetControls().FirstOrDefault(x => x.Contains(_firmwareVersion));

            if (targetBundleControl != null)
            {
                jediWindjammerDevice.ControlPanel.Press(targetBundleControl);
                jediWindjammerDevice.ControlPanel.PressWait("m_OKButton", "TwoButtonMessageBox", TimeSpan.FromSeconds(5));
                jediWindjammerDevice.ControlPanel.Press("m_OKButton");
                UpdateProgressBar(30);
                while (jediWindjammerDevice.ControlPanel.CurrentForm() == "HPProgressPopup")
                {
                    //do nothing
                    Application.DoEvents();
                }
                UpdateProgressBar(100);
            }
            else
            {
                Result = new PluginExecutionResult(PluginResult.Failed, "Specified firmware bundle not found on USB media");
            }
        }

        #endregion ControlPanel

        #region EWS

        private void FlashFirmwareEws()
        {
            device_textBox.InvokeIfRequired(x => x.Text = _device.Address);
            currentRevision_textBox.InvokeIfRequired(x => x.Text = _device.GetDeviceInfo().FirmwareRevision);
            _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashMethod", "EWS");
            ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);
            _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashUpgrade", _data.IsDowngrade ? "false" : "true");
            ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);

            if (Result.Result == PluginResult.Failed)
                return;
            _flashBackgroundWorker.RunWorkerAsync(_device);

            UpdateStatus($"Firmware update for device {_device.Address} is in progress, please wait...");
            while (_flashBackgroundWorker.IsBusy)
            {
                Application.DoEvents();
            }

            UpdateStatus($"Firmware update process for device: {_device.Address} completed.");
        }

        private void FlashBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //_performanceLogger.RecordEvent(DeviceWorkflowMarker.FirmwareUpdateEnd);
            if (Result.Result != PluginResult.Passed)
            {
                UpdateStatus("Firmware process failed.");
                return;
            }
            UpdateStatus("Firmware updated for all asset(s)");
            ExecutionServices.SystemTrace.LogInfo("Firmware updated for all asset(s)");
        }

        private void FlashBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var device = e.Argument as IDevice;
            _performanceLogger.RecordEvent(DeviceWorkflowMarker.FirmwareUpdateBegin);
            ExecutionServices.SystemTrace.LogInfo("Starting Firmware update process...");
            if (device is JediWindjammerDevice)
            {
                FlashFirmware(device.Address);
            }
            else if (device is JediOmniDevice)
            {
                if (_data.IsDowngrade && _firmwareVersion == device.GetDeviceInfo().FirmwareRevision)
                {
                    //do a cold reset
                    UpdateStatus("Target firmware version is same as the current one, current operation is downgrade, performing a cold reset instead.");
                    ColdReset(device.Address, _sessionId);
                }
                else
                {
                    FlashOmniFirmware(device.Address);
                }
            }
        }

        private static bool IsDeviceRunning(IDevice device)
        {
            var status = device.GetDeviceStatus();
            Application.DoEvents();

            //the device can be in Down status (Error) if a jam door or front door is open and thus it is ignored and considered as "Running"
            return (status == DeviceStatus.Running || status == DeviceStatus.Warning || status == DeviceStatus.Down);
        }

        private static bool HasDeviceRebooted(IDevice device)
        {
            var status = device.GetDeviceStatus();
            Application.DoEvents();

            return (status == DeviceStatus.None || status == DeviceStatus.Unknown);
        }

        private bool IsJetDirectUp(IDevice device)
        {
            string bodyString;

            WebClient client = new WebClient();

            try
            {
                bodyString = client.DownloadString($"https://{device.Address}/hp/jetdirect");
                //while the device is still initialising it will hit 404 webexception and won't trigger this
            }
            catch (Exception)
            {
                bodyString = string.Empty;
            }

            return !string.IsNullOrEmpty(bodyString);
        }

        private bool IsWebServicesUp(IDevice device)
        {
            string bodyString;

            WebClient client = new WebClient();
            try
            {
                bodyString = client.DownloadString($"https://{device.Address}:7627/controlpanel");
                //while the device is still initialising it will hit 404 webexception and won't trigger this
                if (!_rebootEndEventLogged)
                {
                    _performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootEnd);
                    _rebootEndEventLogged = true;
                }
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

        private void ValidateFlash(IDevice device)
        {
            var deviceInfo = device.GetDeviceInfo();
            Result = deviceInfo.FirmwareRevision == _firmwareVersion ? new PluginExecutionResult(PluginResult.Passed, $"Firmware updated for device {device.Address}")
                                                                    : new PluginExecutionResult(PluginResult.Failed, "The device firmware update validation failed");
        }

        private void FlashFirmware(object device)
        {
            UpdateStatus($"Uploading firmware file to device, please wait for {TimeSpan.FromMilliseconds(_flashTimeoutPeriod).TotalSeconds} seconds");
            ExecutionServices.SystemTrace.LogDebug(
                $"Uploading firmware file to device, please wait for {TimeSpan.FromMilliseconds(_flashTimeoutPeriod).TotalSeconds} seconds");

            string address = device as string;
            var csrfToken = GetCsrfToken($"https://{address}/hp/device/FirmwareUpgrade", ref _sessionId);

            _httpClient = new HttpClient(_httpClientHandler);
            _httpClient.DefaultRequestHeaders.ExpectContinue = false;
            _httpClient.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"sessionId={_sessionId}");
            _httpClient.Timeout = TimeSpan.FromMilliseconds(_flashTimeoutPeriod);

            var multiPartFormData = new MultipartFormDataContent { { new StringContent("2"), "\"FutureSmartVersion\"" } };

            if (!string.IsNullOrEmpty(csrfToken))
                multiPartFormData.Add(new StringContent(csrfToken), "\"CSRFToken\"");

            if (_data.AutoBackup)
                multiPartFormData.Add(new StringContent("on"), "\"AutomaticBackupRestore\"");

            var fileContent = new StreamContent(File.OpenRead(_firmwareFileName));
            fileContent.Headers.Add("Content-Type", "application/octet-stream");
            fileContent.Headers.Add("Content-Disposition",
                "form-data; name=\"bundleFile\"; filename=\"" + Path.GetFileName(_firmwareFileName) + "\"");

            multiPartFormData.Add(fileContent, "bundleFile", Path.GetFileName(_firmwareFileName));
            multiPartFormData.Add(new StringContent("Install"), "\"InstallButton\"");

            var progressContent = new ProgressableStreamContent(multiPartFormData, 4096,
                (sent, total) =>
                {
                    var newPercentage = (sent * 100) / total;
                    flash_progressBar.InvokeIfRequired(p => p.Value = (int)newPercentage);
                    value_label.InvokeIfRequired(x => x.Text = flash_progressBar.Value + @"%");
                });

            var upgradeMessage =
                _httpClient.PostAsync($"https://{address}/hp/device/FirmwareUpgrade/Save?jsAnchor=FirmwareInstallViewSectionId", progressContent);

            if (!upgradeMessage.Result.IsSuccessStatusCode)
            {
                GetControlPanelImage();
                Result = new PluginExecutionResult(PluginResult.Failed, "Failed to upgrade the firmware");
            }
        }

        private void FlashOmniFirmware(object device)
        {
            string address = device as string;

            UpdateStatus($"Uploading firmware file to device, please wait for {TimeSpan.FromMilliseconds(_flashTimeoutPeriod).TotalSeconds} seconds");
            ExecutionServices.SystemTrace.LogDebug(
                $"Uploading firmware file to device, please wait for {TimeSpan.FromMilliseconds(_flashTimeoutPeriod).TotalSeconds} seconds");

            _httpClient = new HttpClient(_httpClientHandler);
            try
            {
                _httpClient.DefaultRequestHeaders.ExpectContinue = false;
                _httpClient.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
                _httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                _httpClient.DefaultRequestHeaders.Add("Cookie", $"sessionId={_sessionId}");
                _httpClient.Timeout = TimeSpan.FromMilliseconds(_flashTimeoutPeriod);

                using (var multiPartFormData = new MultipartFormDataContent())
                {
                    if (!string.IsNullOrEmpty(_csrfToken))
                        multiPartFormData.Add(new StringContent(_csrfToken), "\"CSRFToken\"");
                    using (var firmwareBundleStream = File.OpenRead(_firmwareFileName))
                    {
                        var fileContent = new StreamContent(firmwareBundleStream);
                        fileContent.Headers.Add("Content-Type", "application/octet-stream");
                        fileContent.Headers.Add("Content-Disposition",
                            "form-data; name=\"bundleFile\"; filename=\"" + Path.GetFileName(_firmwareFileName) + "\"");

                        multiPartFormData.Add(fileContent, "bundleFile", Path.GetFileName(_firmwareFileName));
                        if (_data.AutoBackup)
                            multiPartFormData.Add(new StringContent("on"), "\"AutomaticBackupRestore\"");

                        multiPartFormData.Add(new StringContent("Install"), "\"InstallButton\"");

                        var progressContent = new ProgressableStreamContent(multiPartFormData, 4096,
                            (sent, total) =>
                            {
                                var newPercentage = (sent * 100) / total;
                                flash_progressBar.InvokeIfRequired(p => p.Value = (int)newPercentage);
                                value_label.InvokeIfRequired(x => x.Text = flash_progressBar.Value + @"%");
                            });

                        var upgradeMessage =
                            _httpClient.PostAsync($"https://{address}/hp/device/FirmwareUpgrade/Save", progressContent);
                        ExecutionServices.SystemTrace.LogDebug($"Starting firmware upgrade using SessionId: {_sessionId} and CSRFToken: {_csrfToken}");
                        if (upgradeMessage.Result.IsSuccessStatusCode)
                        {
                            if (upgradeMessage.Result.RequestMessage.RequestUri.AbsoluteUri.EndsWith(
                                "DeviceStatus&StepBackAction=Index"))
                            {
                                //firmware needs to be downgraded.
                                UpdateStatus("The device firmware needs to be downgraded, downgrading the firmware...");
                                ExecutionServices.SystemTrace.LogDebug(
                                    "The device firmware needs to be downgraded, downgrading the firmware...");
                                FormUrlEncodedContent downgradeContent = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("CSRFToken", _csrfToken),
                                    new KeyValuePair<string, string>("OperationIdentifier", "FirmwareDowngrade"),
                                    new KeyValuePair<string, string>("DialogButtonYes", "Rollback")
                                });

                                var downgradeMessage = _httpClient.PostAsync(
                                    $"https://{address}/hp/device/FirmwareUpgrade/DialogResponse",
                                    downgradeContent);
                                if (downgradeMessage.Result.IsSuccessStatusCode)
                                {
                                    UpdateStatus(@"Firmware downgraded successfully");
                                    ExecutionServices.SystemTrace.LogDebug($"Successfully Downgraded using SessionId: {_sessionId} and CSRFToken: {_csrfToken}");
                                    return;
                                }
                                else
                                {
                                    GetControlPanelImage();
                                    Result = new PluginExecutionResult(PluginResult.Failed,
                                        $"Couldn't downgrade the device {address} with the firmware {Path.GetFileNameWithoutExtension(_firmwareFileName)}");
                                }
                            }

                            UpdateStatus(@"Firmware Updated successfully.");
                        }
                        else
                        {
                            string responseBodyString = upgradeMessage.Result.Content.ReadAsStringAsync().Result;
                            int index = responseBodyString.IndexOf("message message-error",
                                StringComparison.OrdinalIgnoreCase);
                            if (index > 1)
                            {
                                responseBodyString = responseBodyString.Substring(index);
                                index = responseBodyString.IndexOf("<h2>", StringComparison.OrdinalIgnoreCase);
                                var endIndex = responseBodyString.IndexOf("</h2>",
                                    StringComparison.OrdinalIgnoreCase);
                                var errorMessage = responseBodyString.Substring(index + 4,
                                    endIndex - (index + 4));
                                UpdateStatus($"Firmware upgrade process failed with message : {errorMessage}");
                                Result = new PluginExecutionResult(PluginResult.Failed,
                                    $"Firmware upgrade process failed with message : {errorMessage}");
                                ExecutionServices.SystemTrace.LogError(
                                    $"Firmware upgrade process failed with message : {errorMessage}");
                            }

                            GetControlPanelImage();
                        }
                    }
                }
            }
            catch (HttpRequestException httpException)
            {
                ExecutionServices.SystemTrace.LogError(httpException);
            }
            catch (AggregateException aggregateException)
            {
                GetControlPanelImage();
                Result = new PluginExecutionResult(PluginResult.Failed, aggregateException.JoinAllErrorMessages());
                ExecutionServices.SystemTrace.LogError(aggregateException);
            }
        }

        protected string GetSessionId(string address)
        {
            try
            {
                //Getting the session ID by making request to /hp/device/DeviceStatus/Index
                HttpWebRequest signinRequest =
                    (HttpWebRequest)WebRequest.Create($"https://{address}/hp/device/SignIn/Index");
                var sessionResponse = HttpMessenger.ExecuteRequest(signinRequest);

                //Getting the session ID from the response recieved
                string sessionId = sessionResponse.Headers.GetValues("Set-Cookie")?.FirstOrDefault()?.Split(';').FirstOrDefault();
                sessionId = sessionId?.Replace("sessionId=", string.Empty);

                return sessionId;
            }
            catch (WebException)
            {
                return string.Empty;
            }
        }

        protected void SignIn(string address, string password)
        {
            string postData;
            password = Uri.EscapeDataString(password);

            var csrfToken = GetCsrfToken($"https://{address}/hp/device/SignIn/Index", ref _sessionId);

            HttpWebRequest signinRequest =
                (HttpWebRequest)WebRequest.Create($"https://{address}/hp/device/SignIn/Index");
            signinRequest.Method = "POST";
            signinRequest.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            signinRequest.ContentType = "application/x-www-form-urlencoded";
            signinRequest.CookieContainer = new CookieContainer();
            signinRequest.CookieContainer.Add(new Cookie("sessionId", _sessionId) { Domain = address });
            signinRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            signinRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            signinRequest.ServicePoint.Expect100Continue = false;
            signinRequest.MaximumAutomaticRedirections = 2;
            if (!string.IsNullOrEmpty(csrfToken))
            {
                postData = $"CSRFToken={HttpUtility.UrlEncode(csrfToken)}&agentIdSelect=hp_EmbeddedPin_v1&DomainDropDown=&UserNameTextBox=&PinDropDown=AdminItem&PasswordTextBox={password}&signInOk=Sign+In";
            }
            else
            {
                postData = $"agentIdSelect=hp_EmbeddedPin_v1&DomainDropDown=&UserNameTextBox=&PinDropDown=AdminItem&PasswordTextBox={password}&signInOk=Sign+In";
            }

            byte[] buffer = Encoding.ASCII.GetBytes(postData);
            signinRequest.ContentLength = buffer.Length;
            using (Stream stream = signinRequest.GetRequestStream())
            {
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }

            try
            {
                var signinResponse = (HttpWebResponse)signinRequest.GetResponse();

                if (signinResponse.StatusCode == HttpStatusCode.OK || signinResponse.StatusCode == HttpStatusCode.Moved)
                {
                    signinResponse.Close();
                    _sessionId = signinRequest.Headers["Cookie"];
                    int index = _sessionId.IndexOf(";", StringComparison.Ordinal);
                    if (index != -1)
                        _sessionId = _sessionId.Substring(0, index);

                    _sessionId = _sessionId.Replace("sessionId=", string.Empty);
                    ExecutionServices.SystemTrace.LogDebug($"Logged in successfully, SessionId is {_sessionId} & CSRFToken = {csrfToken}");
                }
                else
                {
                    Result = new PluginExecutionResult(PluginResult.Failed, $"Couldn't login to the device {address} with the password: {password}");
                }
            }
            catch (WebException webException)
            {
                string errorText;
                WebResponse errorResponse = webException.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    errorText = reader.ReadToEnd();
                    //assetId errorText
                    ExecutionServices.SystemTrace.LogError(errorText);
                }
                Result = new PluginExecutionResult(PluginResult.Failed, string.IsNullOrEmpty(errorText)? webException.Message : errorText);
            }
        }

        private static string GetCsrfToken(string urlAddress, ref string sessionId)
        {
            string csrfToken = string.Empty;
            HttpWebRequest signinCsrfRequest =
                (HttpWebRequest)WebRequest.Create(urlAddress);
            signinCsrfRequest.Method = "GET";
            if (!string.IsNullOrEmpty(sessionId))
            {
                signinCsrfRequest.Headers.Add(HttpRequestHeader.Cookie, $"sessionId={sessionId}");
            }
            //get the CSRF token from the response
            var signinResponse = (HttpWebResponse)signinCsrfRequest.GetResponse();
            if (signinResponse.StatusCode == HttpStatusCode.OK)
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    sessionId = signinResponse.Headers.GetValues("Set-Cookie")?.FirstOrDefault()?.Split(';').FirstOrDefault();
                    sessionId = sessionId?.Replace("sessionId=", string.Empty);
                }

                using (var responseStream = signinResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            string responseBodyString = reader.ReadToEnd();
                            HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
                            hDoc.LoadHtml(responseBodyString);

                            csrfToken = hDoc.GetElementbyId("CSRFToken")?.Attributes["value"]?.Value;
                            ExecutionServices.SystemTrace.LogDebug($"SessionId is {sessionId} and CSRFToken is {csrfToken}");
                        }

                        responseStream.Close();
                    }
                }
            }
            signinResponse.Close();
            return csrfToken;
        }

        private void SetBackupRestoreSetting(string csrfToken, bool restore)
        {
            _httpClient = new HttpClient(_httpClientHandler);

            _httpClient.DefaultRequestHeaders.ExpectContinue = false;
            _httpClient.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"sessionId={_sessionId}");

            var multiPartFormData = new MultipartFormDataContent();
            if (!string.IsNullOrEmpty(csrfToken))
                multiPartFormData.Add(new StringContent(csrfToken), "\"CSRFToken\"");

            if (restore)
                multiPartFormData.Add(new StringContent("on"), "\"AutomaticBackupRestore\"");

            multiPartFormData.Add(new StringContent("Save"), "\"AutomaticBackupRestoreButton\"");
            multiPartFormData.Add(new StringContent("FirmwareInstallViewSectionId"), "\"StepBackAnchor\"");
            multiPartFormData.Add(new StringContent("FirmwareInstallViewSectionId"), "\"jsAnchor\"");

            var message = _httpClient.PostAsync(
                $"https://{_device.Address}/hp/device/FirmwareUpgrade/Save?jsAnchor=FirmwareInstallViewSectionId",
                multiPartFormData);

            if (!message.Result.IsSuccessStatusCode)
            {
                UpdateStatus("Unable to set backup restore option setting, continuing with warning");
            }
            ExecutionServices.SystemTrace.LogDebug($"Successfully set BackupRestore setting to {restore} using SessionId: {_sessionId} and CSRFToken: {csrfToken}");
        }

        private void ColdReset(string address, string sessionId)
        {
            var csrfToken = GetCsrfToken($"https://{address}/hp/device/ResetFactoryDefaults", ref sessionId);

            _httpClient = new HttpClient(_httpClientHandler);

            _httpClient.DefaultRequestHeaders.ExpectContinue = false;
            _httpClient.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"sessionId={sessionId}");

            Dictionary<string, string> formParameters =
                new Dictionary<string, string>
                {
                        {"ResetFirmwareButton", "Reset"},
                        {"StepBackAnchor", "ResetFirmwareSectionId"},
                        {"jsAnchor", "ResetFirmwareSectionId"}
                };

            if (!string.IsNullOrEmpty(csrfToken))
            {
                formParameters.Add("CSRFToken", csrfToken);
            }

            var formdata = new FormUrlEncodedContent(formParameters);

            var dialogResponseAsync = _httpClient.PostAsync($"https://{address}/hp/device/ResetFactoryDefaults/Save?jsAnchor=ResetFirmwareSectionId", formdata);
            ExecutionServices.SystemTrace.LogDebug($"Cold Reset request sent using SessionId: {sessionId} and CSRFToken: {csrfToken}");
            if (dialogResponseAsync.Result.IsSuccessStatusCode)
            {
                formParameters.Clear();
                if (!string.IsNullOrEmpty(csrfToken))
                {
                    formParameters.Add("CSRFToken", csrfToken);
                }
                formParameters.Add("OperationIdentifier", "ResetFirmware");
                formParameters.Add("DialogButtonYes", "Reset");

                formdata = new FormUrlEncodedContent(formParameters);

                var resetResponseAsync =
                    _httpClient.PostAsync(
                        $"https://{address}/hp/device/ResetFactoryDefaults/DialogResponse?jsAnchor=ResetFirmwareSectionId",
                        formdata);

                if (resetResponseAsync.Result.IsSuccessStatusCode)
                {
                    var rebootResponse = _httpClient.GetAsync($"https://{address}/hp/device/HardDriveList/RebootOperation");
                    if (rebootResponse.Result.IsSuccessStatusCode)
                        UpdateStatus("Cold Reset Complete");
                }
                else
                {
                    UpdateStatus("Cold Reset failed");
                }
            }
        }

        #endregion EWS

        /// <summary>
        /// Updates the status text in the execution control display.
        /// </summary>
        /// <param name="statusMsg"></param>
        protected virtual void UpdateStatus(string statusMsg)
        {
            if (string.IsNullOrEmpty(statusMsg))
                return;
            status_RichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + statusMsg);
                c.AppendText($"{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}  {statusMsg}\n");
                c.Select(c.Text.Length, 0);
                c.ScrollToCaret();
            });

            ExecutionServices.SystemTrace.LogInfo(statusMsg);
        }

        private void UpdateProgressBar(int progress)
        {
            flash_progressBar.InvokeIfRequired(x => x.Value = progress);
            value_label.InvokeIfRequired(x => x.Text = flash_progressBar.Value + @"%");
        }

        private static string Normalize(string message)
        {
            // EFI output is formatted with ANSI CSI codes.
            // See: http://en.wikipedia.org/wiki/ANSI_escape_code

            if (String.IsNullOrEmpty(message)) return String.Empty;

            // remove control sequences
            Regex ctrPattern = new Regex(@"(\x1b\[[\d;=?]*[ABCDEFGHJKSTfmnsulh]{1})|\u0000");
            string outputString = ctrPattern.Replace(message, "");

            // fix-up new lines
            //Regex newLine = new Regex(@"((?!\u000D)\u000A)|\u000D(?!\u000A)");
            //OutputString = newLine.Replace(OutputString, "\r\n");

            return outputString;
        }

        private string GetMessageCenter()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                WebClient ewsWebClient = new WebClient();
                var responseBodyString = ewsWebClient.DownloadString($"https://{_device.Address}/hp/device/MessageCenter/Summary");
                var messageCenter = JsonConvert.DeserializeObject<MessageCenterMessages>(responseBodyString);
                return string.Join(";", messageCenter.Notifications.Select(x => x.Message));
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void GetControlPanelImage()
        {
            try
            {
                if (_device is JediOmniDevice)
                {
                    var jediOmniDevice = _device as JediOmniDevice;
                    pictureBoxControlPanel.InvokeIfRequired(c =>
                        {
                            pictureBoxControlPanel.Image = jediOmniDevice.ControlPanel.ScreenCapture();
                            pictureBoxControlPanel.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                    );
                    return;
                }

                var jediWjDevice = _device as JediWindjammerDevice;
                if (jediWjDevice != null)
                {
                    jediWjDevice.ControlPanel.ScreenCapture();
                    pictureBoxControlPanel.Image = jediWjDevice.ControlPanel.ScreenCapture();
                    pictureBoxControlPanel.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch (Exception e)
            {
                ExecutionServices.SystemTrace.LogDebug("Error getting screencapture of Control Panel. " + e.Message);
            }
        }
    }

    internal class MessageCenterMessages
    {
        [JsonProperty("messages")]
        internal List<MessageCenterNotification> Notifications { get; set; }

        internal MessageCenterMessages()
        {
            Notifications = new List<MessageCenterNotification>();
        }
    }

    /// <summary>
    /// class representing message center notification from EWS
    /// </summary>
    internal class MessageCenterNotification
    {
        //{"messages":[{"type":"notification","msg":"Sleep mode on","priority":"299"}]}

        [JsonProperty("type")]
        internal string Type { get; set; }

        [JsonProperty("msg")]
        internal string Message { get; set; }

        [JsonProperty("priority")]
        internal int Priority { get; set; }
    }

    internal class ProgressableStreamContent : HttpContent
    {
        /// <summary>
        /// Lets keep buffer of 20kb
        /// </summary>
        private const int DefaultBufferSize = 5 * 4096;

        private readonly HttpContent _content;
        private readonly int _bufferSize;

        //private bool contentConsumed;
        private readonly Action<long, long> _progress;

        public ProgressableStreamContent(HttpContent content, Action<long, long> progress) : this(content, DefaultBufferSize, progress)
        {
        }

        public ProgressableStreamContent(HttpContent content, int bufferSize, Action<long, long> progress)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            _content = content;
            _bufferSize = bufferSize;
            _progress = progress;

            foreach (var h in content.Headers)
            {
                Headers.Add(h.Key, h.Value);
            }
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Task.Run(async () =>
            {
                var buffer = new byte[_bufferSize];
                long size;
                TryComputeLength(out size);
                var uploaded = 0;

                using (var sinput = await _content.ReadAsStreamAsync())
                {
                    while (true)
                    {
                        var length = sinput.Read(buffer, 0, buffer.Length);
                        if (length <= 0) break;

                        uploaded += length;
                        _progress?.Invoke(uploaded, size);

                        if (stream.CanWrite)
                        {
                            stream.Write(buffer, 0, length);
                            stream.Flush();
                        }
                        else
                        {
                            ExecutionServices.SystemTrace.LogDebug($"Unable to write to stream. Stream position: {sinput.Position}");
                        }
                    }
                }
                stream.Flush();
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _content.Headers.ContentLength.GetValueOrDefault();
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _content.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}