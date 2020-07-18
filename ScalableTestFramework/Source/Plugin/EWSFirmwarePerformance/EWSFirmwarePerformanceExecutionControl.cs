using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;
using HP.ScalableTest.Framework.Assets;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using System.Collections.Specialized;
using System.Web;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Plugin.EWSFirmwarePerformance
{
    [ToolboxItem(false)]
    public partial class EWSFirmwarePerformanceExecutionControl : UserControl, IPluginExecutionEngine
    {
        private PluginExecutionData _executionData;
        private EWSFirmwarePerformanceActivityData _activityData;
        private DeviceWorkflowLogger _performanceLogger;
        private ActivityExecutionDetailLog _activityExecutionDetailLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="EWSFirmwarePerformanceExecutionControl" /> class.
        /// </summary>
        public EWSFirmwarePerformanceExecutionControl()
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
            _activityData = executionData.GetMetadata<EWSFirmwarePerformanceActivityData>();

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
                    ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(_executionData, asset));
                    
                    ExecutionServices.SystemTrace.LogDebug("Validating Firmware bundle for the selected asset");
                    result = ValidateFirmwareBundles(asset.AssetId, asset.AdminPassword, asset.Address);
                    if (result.Result == PluginResult.Failed)
                    {
                        throw new DeviceWorkflowException($"Failed to validate FW bundle for device {asset.AssetId}. Error: {result.Message}");
                    }
                    if (result.Result != PluginResult.Skipped)
                    {
                        ExecutionServices.SystemTrace.LogDebug($"Performing update on device {asset.AssetId} at address {asset.Address}");

                        result = UpgradeFirmware(asset);
                    }
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

        private void GatherTriageData(IDevice device, string reason)
        {
            if (device != null)
            {
                DeviceAutomation.InfoCollection.TriageData.ITriage triage = DeviceAutomation.InfoCollection.TriageData.TriageFactory.Create(device, _executionData);
                triage.CollectTriageData(reason);
                triage.Submit();
            }
        }
        /// <summary>
        /// Performs a firmware update on Omni and WindJammer Devices
        /// </summary>
        /// <param name="asset"></param>
        /// <returns>A <see cref="PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        public PluginExecutionResult UpgradeFirmware(IDeviceInfo asset)
        {
            IDevice device = DeviceConstructor.Create(asset);
            string sessionId = GetSessionId(device.Address);
            _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashMethod", "EWS");
            ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);


            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Failed to Update Firmware");
            string startingFW = device.GetDeviceInfo().FirmwareRevision;
            device_textBox.InvokeIfRequired(c => { c.Text = device.Address; });
            currentRevision_textBox.InvokeIfRequired(c => { c.Text = startingFW; });

            UpdateStatus($"Firmware upgrade for device {device.Address} is in progress, please wait...");
            bool downgrade = false;
            if (device is JediWindjammerDevice)
            {
                result = SignInWJ(device.Address, device.AdminPassword, sessionId);

                _performanceLogger.RecordEvent(DeviceWorkflowMarker.FirmwareUpdateBegin);
                result = FlashFirmwareWJ(asset, sessionId);
            }
            else if (device is JediOmniDevice)
            {
                sessionId = SignInOmni(device.Address, device.AdminPassword);

                _performanceLogger.RecordEvent(DeviceWorkflowMarker.FirmwareUpdateBegin);
                result = FlashOmniFirmware(asset, ref sessionId);
                downgrade = result.Message == "True" ? true : false;
                
            }
            ExecutionServices.SystemTrace.LogInfo($"FW Update Complete");
            _activityExecutionDetailLog = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashUpgrade", downgrade ? "false" : "true" );
            ExecutionServices.DataLogger.Submit(_activityExecutionDetailLog);

            //Wait for device to finish rebooting or end

            //if (!_activityData.ValidateFlash || downgrade) 
            //{
            //    _performanceLogger.RecordEvent(DeviceWorkflowMarker.FirmwareUpdateEnd);
            //    return result; 
            //}

            if (result.Result == PluginResult.Failed)
            {
                return result;
            }

            int maxRetries = (int)_activityData.ValidateTimeOut.TotalSeconds / 10;
            if (Retry.UntilTrue(() => HasDeviceRebooted(device), maxRetries / 5, TimeSpan.FromSeconds(5)))
            {
                _performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootBegin);
                UpdateStatus("Device has Rebooted. Waiting for device to boot up...");
            }
            else
            {
                result = new PluginExecutionResult(PluginResult.Failed, $"Device did not reboot after firmware was uploaded. Please check the device for pending jobs and try again.");
                return result;
            }


            ExecutionServices.SystemTrace.LogInfo($"Starting Reboot");
            UpdateStatus("Waiting for device to boot up...");
            //_performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootBegin);
            //We're probably not up and running right away.
            Thread.Sleep(TimeSpan.FromSeconds(30));

            //int maxRetries = (int)_activityData.ValidateTimeOut.TotalSeconds / 10;
            ExecutionServices.SystemTrace.LogDebug($"Max Retries: {maxRetries}");
            int retry = 0;
            string fwRevision = string.Empty;
            bool controlPanelUp = false; //Actually webservices, but close enough.
            bool embeddedServerUp = false;
            if (Retry.UntilTrue(() => IsDeviceRunning(device, retry++, ref controlPanelUp, ref embeddedServerUp), maxRetries, TimeSpan.FromSeconds(10)))
            {
                try
                {
                    if (downgrade)
                    {
                        SetDefaultPassword(device.Address, device.AdminPassword);
                    }

                    fwRevision = device.GetDeviceInfo().FirmwareRevision;
                    postRevision_textBox.InvokeIfRequired(c => { c.Text = fwRevision; });
                }
                catch
                {
                    fwRevision = string.Empty;
                }
                //Validate update passed by comparing starting and ending FW
                result = startingFW != fwRevision ? new PluginExecutionResult(PluginResult.Passed, $"Firmware upgraded for device {device.Address}") : new PluginExecutionResult(PluginResult.Failed, "The device firmware upgrade validation failed");
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

            return result;
        }

        public void SetDefaultPassword(string address, string password)
        {
            var defPWUrn = "urn:hp:imaging:con:service:security:SecurityService:AdministratorAuthenticationSettings";
            string endpoint = "security";
            ExecutionServices.SystemTrace.LogDebug($"{address}: Setting default password");

            JediDevice dev;
            try
            {
                dev = new JediDevice(address, "");
                WebServiceTicket tic = dev.WebServices.GetDeviceTicket(endpoint, defPWUrn);
                if (password.Length < 8)
                {
                    tic.FindElement("MinLength").SetValue(password.Length - 1);
                    tic.FindElement("IsPasswordComplexityEnabled").SetValue("false");

                }
                tic.FindElement("Password").SetValue(password);
                tic.FindElement("PasswordStatus").SetValue("set");
                dev.WebServices.PutDeviceTicket("security", defPWUrn, tic, false);

                dev = new JediDevice(address, password);
                ExecutionServices.SystemTrace.LogDebug($"{address}: Default password set");
            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.Message);
                dev = new JediDevice(address, password);
            }
            //disposing the device
            dev.Dispose();
            
        }
        private static bool HasDeviceRebooted(IDevice device)
        {
            var status = device.GetDeviceStatus();
            Application.DoEvents();

            return (status == DeviceStatus.None || status == DeviceStatus.Unknown);
        }

        /// <summary>
        /// Sends the upgraged/downgrade firmware request for WindJammer
        /// </summary>
        /// <param name="device"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        private PluginExecutionResult FlashFirmwareWJ(IDeviceInfo device, string sessionId)
        {
            NameValueCollection nvc = new NameValueCollection
            {
                {"FutureSmartVersion", "2"},
                {"AutomaticBackupRestore", "on"}
            };
            string address = device.Address as string;
            string firmwareFileName = _activityData.AssetMapping[device.AssetId].FirmwareFile;
            var firmwareUpgradeRequest = (HttpWebRequest)WebRequest.Create($"https://{address}/hp/device/FirmwareUpgrade/Save?jsAnchor=FirmwareInstallViewSectionId");
            firmwareUpgradeRequest.Method = "POST";
            firmwareUpgradeRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            firmwareUpgradeRequest.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            int flashTimeoutPeriod = _activityData.FWBundleInfo.Where(x => x.ProductFamily == _activityData.AssetMapping[device.AssetId].ProductFamily).First().FlashTimeOutPeriod;
            firmwareUpgradeRequest.Timeout = flashTimeoutPeriod;

            firmwareUpgradeRequest.Headers.Add(HttpRequestHeader.Cookie, $"sessionId={sessionId}");
            FillUploadData(firmwareUpgradeRequest, firmwareFileName, "bundleFile", nvc);

            try
            {
                var firmwareUpgradeResponse = (HttpWebResponse)firmwareUpgradeRequest.GetResponse();
                if (firmwareUpgradeResponse.StatusCode == HttpStatusCode.OK)
                {
                    firmwareUpgradeResponse.Close();
                }
            }
            catch (WebException webException)
            {
                return new PluginExecutionResult(PluginResult.Failed, webException);
            }
            return new PluginExecutionResult(PluginResult.Passed);
        }

        /// <summary>
        /// Sends the upgraged/downgrade firmware request for Omni
        /// </summary>
        /// <param name="device"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        private PluginExecutionResult FlashOmniFirmware(IDeviceInfo device, ref string sessionId)
        {
            string address = device.Address as string;
            bool flashDowngrade = false;
            int flashTimeoutPeriod = _activityData.FWBundleInfo.Where(x => x.ProductFamily == _activityData.AssetMapping[device.AssetId].ProductFamily).First().FlashTimeOutPeriod;
            string firmwareFileName = _activityData.AssetMapping[device.AssetId].FirmwareFile;
            var csrfToken = GetCsrfToken($"https://{address}/hp/device/FirmwareUpgrade/Save", ref sessionId);
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);// new PluginExecutionResult(PluginResult.Failed, "Failed to Flash Firmware");
            NameValueCollection nvc = new NameValueCollection();
            if (!string.IsNullOrEmpty(csrfToken))
            {
                nvc.Add("CSRFToken", csrfToken);
            }
            nvc.Add("AutomaticBackupRestore", "on");
            var upgradeRequest = (HttpWebRequest)WebRequest.Create($"https://{address}/hp/device/FirmwareUpgrade/Save");
            upgradeRequest.Method = "POST";
            upgradeRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            upgradeRequest.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            upgradeRequest.Timeout = flashTimeoutPeriod == 0 ? 120 : flashTimeoutPeriod; //if the bundle is less than a MB, which it shouldn't be.
            upgradeRequest.Headers.Add(HttpRequestHeader.Cookie, $"sessionId={sessionId}");
            UpdateStatus($"Uploading firmware file to device, please wait for {flashTimeoutPeriod / 100} seconds");
            ExecutionServices.SystemTrace.LogInfo($"Uploading firmware file to device, please wait for {flashTimeoutPeriod / 100} seconds");
            try
            {
                FillUploadData(upgradeRequest, firmwareFileName, "bundleFile", nvc);

                var saveResponse = (HttpWebResponse)upgradeRequest.GetResponse();
                if (saveResponse.StatusCode == HttpStatusCode.OK)
                {
                    //check if we have a prompt for downgrade, if so handle it
                    if (saveResponse.ResponseUri.AbsoluteUri.EndsWith("DeviceStatus&StepBackAction=Index"))
                    {
                        //UpdateStatus("The device firmware needs to be downgraded, downgrading the firmware...");
                        //get the CSRF parameter for this:
                        string csrfTokenDownGrade = string.Empty;
                        using (var responseStream = saveResponse.GetResponseStream())
                        {
                            if (responseStream != null)
                            {
                                using (var reader = new StreamReader(responseStream))
                                {
                                    string responseBodyString = reader.ReadToEnd();
                                    int startIndex = responseBodyString.IndexOf("name=\"CSRFToken\" value=\"",
                                        StringComparison.OrdinalIgnoreCase);
                                    if (startIndex != -1)
                                    {
                                        responseBodyString = responseBodyString.Substring(startIndex);
                                        int endIndex = responseBodyString.IndexOf("\" />",
                                            StringComparison.OrdinalIgnoreCase);
                                        csrfTokenDownGrade =
                                            responseBodyString.Substring("name=\"CSRFToken\" value=\"".Length,
                                                endIndex - "name=\"CSRFToken\" value=\"".Length);
                                    }
                                }

                                responseStream.Close();
                            }
                        }
                        saveResponse.Close();

                        //DowngradeFirmware
                        var downgradeRequest =
                            (HttpWebRequest)
                            WebRequest.Create($"https://{address}/hp/device/FirmwareUpgrade/DialogResponse");

                        downgradeRequest.Method = "POST";
                        downgradeRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                        downgradeRequest.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
                        downgradeRequest.Timeout = flashTimeoutPeriod;
                        downgradeRequest.Headers.Add(HttpRequestHeader.Cookie, $"sessionId={sessionId}");
                        downgradeRequest.ContentType = "application/x-www-form-urlencoded";
                        downgradeRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
                        string postData;
                        if (!string.IsNullOrEmpty(csrfTokenDownGrade))
                        {
                            postData = $"CSRFToken={csrfTokenDownGrade}&OperationIdentifier=FirmwareDowngrade&DialogButtonYes=Rollback";
                        }
                        else
                        {
                            postData = "OperationIdentifier=FirmwareDowngrade&DialogButtonYes=Rollback";
                        }
                        byte[] buffer = Encoding.ASCII.GetBytes(postData);
                        downgradeRequest.ContentLength = buffer.Length;
                        using (Stream stream = downgradeRequest.GetRequestStream())
                        {
                            stream.Write(buffer, 0, buffer.Length);
                            stream.Flush();
                        }

                        var downgradeResponse = (HttpWebResponse)downgradeRequest.GetResponse();

                        if (downgradeResponse.StatusCode == HttpStatusCode.OK ||
                            downgradeResponse.StatusCode == HttpStatusCode.Moved)
                        {
                            flashDowngrade = true;
                            downgradeResponse.Close();
                        }
                        else
                        {
                            result = new PluginExecutionResult(PluginResult.Failed,
                                $"Couldn't downgrade the device {address} with the firmware {firmwareFileName}");
                        }
                    }
                    else
                    {
                        using (var responseStream = saveResponse.GetResponseStream())
                        {
                            if (responseStream != null)
                            {
                                using (var reader = new StreamReader(responseStream))
                                {
                                    string responseBodyString = reader.ReadToEnd();
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
                                        result = new PluginExecutionResult(PluginResult.Failed,
                                            $"Firmware upgrade process failed with message : {errorMessage}");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    result = new PluginExecutionResult(PluginResult.Failed,
                        $"Couldn't flash the device {address} with the firmware {firmwareFileName}");
                }

                saveResponse.Close();
            }
            catch (WebException webException)
            {
                result = new PluginExecutionResult(PluginResult.Failed, webException);
            }
            catch (Exception exception)
            {
                result = new PluginExecutionResult(PluginResult.Failed, exception);
            }

            if (result.Result == PluginResult.Passed)
            {
                result = new PluginExecutionResult(PluginResult.Passed, flashDowngrade.ToString());
            }

            return result; 
        }


        /// <summary>
        /// Uploads firmware to the device. Additionally updates the UI progress bar
        /// </summary>
        /// <param name="webRequest"></param>
        /// <param name="file"></param>
        /// <param name="paramName"></param>
        /// <param name="nvc"></param>
        private void FillUploadData(HttpWebRequest webRequest, string file, string paramName, NameValueCollection nvc)
        {
            string contentType = MimeMapping.GetMimeMapping(file); 
            webRequest.ServicePoint.Expect100Continue = false;

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.AllowReadStreamBuffering = false;
            webRequest.AllowWriteStreamBuffering = false;

            var length = new FileInfo(file).Length;

            string headerTemplate = $"Content-Disposition: form-data; name=\"{paramName}\"; filename=\"{Path.GetFileName(file)}\"\r\nContent-Type: {contentType}\r\n\r\n";
            byte[] headerBytes = Encoding.UTF8.GetBytes(headerTemplate);

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
            StringBuilder formdataStringBuilder = new StringBuilder();
            foreach (string key in nvc.Keys)
            {
                formdataStringBuilder.Append($"--{boundary}\r\n");
                formdataStringBuilder.Append(string.Format(formdataTemplate, key, nvc[key]));
            }
            byte[] formitemBytes = Encoding.UTF8.GetBytes(formdataStringBuilder.ToString());
            byte[] trailerBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            var installBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n" + "Content-Disposition: form-data; name=\"InstallButton\"\r\n\r\nInstall");

            webRequest.ContentLength = formitemBytes.Length + boundaryBytes.Length + headerBytes.Length + length + installBytes.Length + trailerBytes.Length;
            webRequest.UserAgent = "IE";

            using (Stream requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(formitemBytes, 0, formitemBytes.Length);

                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);

                requestStream.Write(headerBytes, 0, headerBytes.Length);

                if (string.IsNullOrEmpty(file))
                {
                    byte[] buffer = new byte[8192];
                    requestStream.Write(buffer, 0, 0);
                }
                else
                {
                    using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        int percentage = 0;
                        byte[] buffer = new byte[8192];
                        int bytesRead;
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            requestStream.Write(buffer, 0, bytesRead);
                            requestStream.Flush();
                            var newPercentage = (int)(100.0 * fileStream.Position / length);
                            if (newPercentage != percentage)
                            {
                                flash_progressBar.InvokeIfRequired(p => p.Value = newPercentage);
                                value_label.InvokeIfRequired(x => x.Text = flash_progressBar.Value + "%");
                                percentage = newPercentage;
                            }
                        }
                        ///Add marker?
                        UpdateStatus("Waiting for device to boot up...");
                    }

                }
                requestStream.Write(installBytes, 0, installBytes.Length);
                requestStream.Write(trailerBytes, 0, trailerBytes.Length);
            }
        }


        /// <summary>
        /// Signs into web service in WindJammer
        /// </summary>
        /// <param name="address"></param>
        /// <param name="password"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        protected PluginExecutionResult SignInWJ(string address, string password, string sessionId)
        {
            HttpWebRequest signinRequest =(HttpWebRequest)WebRequest.Create($"https://{address}/hp/device/SignIn/Index");

            signinRequest.Method = "POST";
            signinRequest.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            signinRequest.ContentType = "application/x-www-form-urlencoded";
            signinRequest.Headers.Add(HttpRequestHeader.Cookie, $"sessionId={sessionId}");
            signinRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            signinRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");

            var postData = $"agentIdSelect=hp_EmbeddedPin_v1&DomainDropDown=&UserNameTextBox=&PinDropDown=AdminItem&PasswordTextBox={password}&signInOk=Sign+In";

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
                }
                else
                {
                    return new PluginExecutionResult(PluginResult.Failed, $"Couldn't login to the device {address} with the password: {password}");
                }
            }
            catch (WebException webException)
            {
                return new PluginExecutionResult(PluginResult.Failed, webException);
            }
            return new PluginExecutionResult(PluginResult.Passed);
        }

        /// <summary>
        /// Uses Jedi Firmware Utility to validate we have a well contructed firmware file that is valid for a specified device
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public PluginExecutionResult ValidateFirmwareBundles(string assetId, string password, string address)
        {

            //if (!_activityData.ValidateFWBundles)
            //{
            //    return new PluginExecutionResult(PluginResult.Passed);
            //}

                string[] separator = { Environment.NewLine };
            UpdateStatus("Validating Firmware bundle for the selected asset");
            //extract the file;
            var tempDumpDirectory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "Dump"));
            var dumpUtilityFileName = Path.Combine(tempDumpDirectory.FullName, "FimDumpUtility.exe");
            File.WriteAllBytes(dumpUtilityFileName, ResourceDump.FimDumpUtility);

            if (!Directory.Exists(_activityData.FimBundlesLocation))
            {
                return new PluginExecutionResult(PluginResult.Failed, "Failed to find FW bundle directory");
            }

            string[] files = Directory.GetFiles(_activityData.FimBundlesLocation, "*.bdl");

            if (files.Length == 0)
            {
                return new PluginExecutionResult(PluginResult.Failed, "Unable to find .bdl files in the directory"); ;
            }


            string firmwareFile = _activityData.AssetMapping[assetId].FirmwareFile;

            FirmwareData fwData = new FirmwareData();

            string fwFileName = Path.Combine(tempDumpDirectory.FullName, Path.GetFileName(firmwareFile));
            File.Copy(firmwareFile, fwFileName, true);



            var result = ProcessUtil.Execute(dumpUtilityFileName, $"-o {tempDumpDirectory.FullName} \"{fwFileName}\"");
            var outputLines = result.StandardOutput.Split(separator, StringSplitOptions.None);

            var revision = outputLines.FirstOrDefault(x => x.Contains("Version"));
            if (string.IsNullOrEmpty(revision))
            {
                return  new PluginExecutionResult(PluginResult.Failed, "Failed to find FW bundle revision"); ;
            }
            revision = revision.Substring(revision.IndexOf(':') + 1).Trim();
            fwData.FirmwareRevision = revision.Split(' ').First();

            var version = outputLines.FirstOrDefault(x => x.Contains("Description"))?.Trim();
            if (string.IsNullOrEmpty(version))
            {
                return new PluginExecutionResult(PluginResult.Failed, "Failed to find FW bundle version");
            }
            version = version.Substring(version.IndexOf(':') + 1);
            fwData.FWBundleVersion = version;

            var dateCode = revision.Substring(revision.IndexOf('(') + 1, revision.LastIndexOf(')') - (revision.IndexOf('(') + 1));
            fwData.FirmwareDateCode = dateCode;

            var name = outputLines.FirstOrDefault(x => x.Contains("Name"));
            if (string.IsNullOrEmpty(name))
            {
                return new PluginExecutionResult(PluginResult.Failed, "Failed to find FW bundle name");
            }
            name = name.Substring(name.IndexOf(':') + 1).Trim();
            fwData.FWModelName = name;
            var pFamily = outputLines.FirstOrDefault(x => x.Contains("Identifier"));
            pFamily = pFamily.Substring(pFamily.IndexOf(':') + 1).Trim();
            fwData.ProductFamily = pFamily;

            if (_activityData.ValidateFWBundles)
            {
                if (!_activityData.FWBundleInfo.Any(
                x => x.ProductFamily == fwData.ProductFamily &&
                x.FWModelName == fwData.FWModelName &&
                x.FWBundleVersion == fwData.FWBundleVersion &&
                x.FirmwareRevision == fwData.FirmwareRevision &&
                x.FirmwareDateCode == fwData.FirmwareDateCode))
                {
                    return new PluginExecutionResult(PluginResult.Failed, "Failed to find FW bundle match");
                }
            }
            else
            {
                JediDevice dev = new JediDevice(address, password);
                var temp = dev.GetDeviceInfo();

                if (fwData.FirmwareRevision == temp.FirmwareRevision)
                {
                    return new PluginExecutionResult(PluginResult.Skipped, "Firmware on device is the same as the bundle");
                }
            }

            ///Check to see that the FW is still tied to a device.
            if (_activityData.AssetMapping.Where(x => x.Value.ProductFamily == pFamily).Count() == 0)
            {
                return new PluginExecutionResult(PluginResult.Failed, "Unable to match a device to a FW bundle");
            }

            _activityData.AssetMapping.Where(x => x.Value.ProductFamily == pFamily).First().Value.FirmwareFile = fwFileName;
            
            UpdateStatus($"Validated firmware for device");
            return new PluginExecutionResult(PluginResult.Passed);
        }


        private string GetSessionId(string address)
        {
            try
            {
                //Getting the session ID by making request to /hp/device/DeviceStatus/Index
                HttpWebRequest signinRequest = (HttpWebRequest)WebRequest.Create($"https://{address}/hp/device/SignIn/Index");
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



            #region Debugging and Future Improvements on performance
            //try
            //{
            //    if (device is JediOmniDevice)
            //    {
            //        var jedidevice = (JediOmniDevice)device;
            //        jedidevice.ControlPanel.GetIds("div", OmniIdCollectionType.Children);
            //    }
            //    else
            //    {
            //        var jedidevice = (JediWindjammerDevice)device;
            //        jedidevice.ControlPanel.GetControls();
            //    }
            //    ExecutionServices.SystemTrace.LogDebug("We made it!");
            //    performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootEnd);
            //    return true;
            //}
            //catch (DeviceCommunicationException e)
            //{
            //    ExecutionServices.SystemTrace.LogDebug("Failed to see some Control panel goodness");
            //}
            //try
            //{
            //    if (device is JediOmniDevice)
            //    {
            //        var jedidevice = (JediOmniDevice)device;
            //        var temp = jedidevice.PowerManagement.GetPowerState();
            //        ExecutionServices.SystemTrace.LogInfo($"Power State: {temp}");

            //    }
            //    else
            //    {
            //        var jedidevice = (JediWindjammerDevice)device;
            //        var temp = jedidevice.PowerManagement.GetPowerState();
            //        ExecutionServices.SystemTrace.LogInfo($"Power State: {temp}");
            //    }
            //    ExecutionServices.SystemTrace.LogInfo("We made it via powerstate!");
            //    performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootEnd);
            //    //return true;
            //}
            //catch (DeviceCommunicationException e)
            //{
            //    ExecutionServices.SystemTrace.LogInfo("Failed to see some power state goodness");
            //}
            //try
            //{
            //    string urn = "urn:hp:imaging:con:service:security:SecurityService";
            //    string endpoint = "security";
            //    if (device is JediOmniDevice)
            //    {
            //        var jedidevice = (JediOmniDevice)device;
            //        jedidevice.WebServices.GetDeviceTicket(endpoint, urn);
            //    }
            //    else
            //    {
            //        var jedidevice = (JediWindjammerDevice)device;
            //        jedidevice.WebServices.GetDeviceTicket(endpoint, urn);
            //    }
            //    ExecutionServices.SystemTrace.LogDebug("We made it!");
            //    performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootEnd);
            //    return true;
            //}
            //catch (DeviceCommunicationException e)
            //{
            //    ExecutionServices.SystemTrace.LogDebug("Failed to see some Control panel goodness");
            //}
            #endregion


            return  controlPanelUp && embeddedServerUp;
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





        private string SignInOmni(string address, string password)
        {
            string sessionId = string.Empty;
            string postData;

            var csrfToken = GetCsrfToken($"https://{address}/hp/device/SignIn/Index", ref sessionId);
            password = Uri.EscapeDataString(password);
            HttpWebRequest signinRequest = (HttpWebRequest)WebRequest.Create($"https://{address}/hp/device/SignIn/Index");
            signinRequest.CookieContainer = new CookieContainer();
            signinRequest.CookieContainer.Add(new Cookie("sessionId", sessionId) { Domain = address });
            signinRequest.Method = "POST";
            signinRequest.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
            signinRequest.ContentType = "application/x-www-form-urlencoded";
            signinRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            signinRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");

            if (string.IsNullOrEmpty(csrfToken))
            {
                postData = $"agentIdSelect=hp_EmbeddedPin_v1&DomainDropDown=&UserNameTextBox=&PinDropDown=AdminItem&PasswordTextBox={password}&signInOk=Sign+In";
            }
            else
            {
                postData = $"CSRFToken={csrfToken}&agentIdSelect=hp_EmbeddedPin_v1&DomainDropDown=&UserNameTextBox=&PinDropDown=AdminItem&PasswordTextBox={password}&signInOk=Sign+In";
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

                if (signinResponse.StatusCode == HttpStatusCode.OK || signinResponse.StatusCode == HttpStatusCode.Moved || signinResponse.StatusCode == HttpStatusCode.Found)
                {
                    signinResponse.Close();

                    sessionId = signinRequest.Headers["Cookie"];
                    sessionId = sessionId.Substring(0, sessionId.IndexOf(";", StringComparison.Ordinal));
                    sessionId = sessionId.Replace("sessionId=", string.Empty);
                }

                return sessionId;
            }
            catch (WebException webException)
            {
                throw new Exception("Failed to login", webException);
            }
        }

        ///// <summary>
        ///// Validates that we successfully installed the firmware on the device by looking at the device info after reboot
        ///// </summary>
        ///// <param name="device"></param>
        ///// <returns></returns>
        //private PluginExecutionResult ValidateFlash(IDevice device)
        //{
        //    var deviceInfo = device.GetDeviceInfo();

        //    string endpoint = "fim";
        //    string urn = "urn:hp:imaging:con:service:fim:FIMService";
        //    string productFamily = "";
        //    try
        //    {
        //        ExecutionServices.SystemTrace.LogInfo($"Attempting to get product family from flashed device");

        //        JediDevice jediDevice = new JediDevice(device.Address, device.AdminPassword);
        //        WebServiceTicket tic = jediDevice.WebServices.GetDeviceTicket(endpoint, urn);
        //        var ident = tic.FindElements("AssetIdentifier").First().Value;
        //        productFamily = ident;
        //        ExecutionServices.SystemTrace.LogInfo($"Device Info product Family: {ident}");


        //        var firmwareRevision = _activityData.FWBundleInfo.Where(x => x.ProductFamily == productFamily).FirstOrDefault().FirmwareRevision;
        //        ExecutionServices.SystemTrace.LogInfo($"Device Firmware Revision: {deviceInfo.FirmwareRevision}");
        //        ExecutionServices.SystemTrace.LogInfo($"Device bundle info: {firmwareRevision}");

        //    }
        //    catch
        //    {
        //        ExecutionServices.SystemTrace.LogInfo($"Failed to validate product family");
        //    }

        //    return deviceInfo.FirmwareRevision == _activityData.FWBundleInfo.Where(x => x.ProductFamily == productFamily).FirstOrDefault().FirmwareRevision  ? new PluginExecutionResult(PluginResult.Passed, $"Firmware upgraded for device {device.Address}") : new PluginExecutionResult(PluginResult.Failed, "The device firmware upgrade validation failed");
        //}

        /// <summary>
        /// Get csrf Token used to sign into a device.
        /// </summary>
        /// <param name="urlAddress"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        private string GetCsrfToken(string urlAddress, ref string sessionId)
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
                            int startIndex = responseBodyString.IndexOf("name=\"CSRFToken\" value=\"",
                                StringComparison.OrdinalIgnoreCase);
                            if (startIndex != -1)
                            {
                                responseBodyString = responseBodyString.Substring(startIndex);
                                int endIndex = responseBodyString.IndexOf("\" />", StringComparison.OrdinalIgnoreCase);
                                csrfToken = responseBodyString.Substring("name=\"CSRFToken\" value=\"".Length,
                                    endIndex - "name=\"CSRFToken\" value=\"".Length);
                                //csrfToken = HttpUtility.UrlEncode(csrfToken);
                            }
                        }

                        responseStream.Close();
                    }
                }
            }
            signinResponse.Close();
            return csrfToken;
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
