using System;
using System.Collections.Generic;
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
using HP.ScalableTest.DeviceAutomation.DeviceSettings;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using Newtonsoft.Json;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.Reboot
{
    [ToolboxItem(false)]
    public partial class RebootExecutionControl : UserControl, IPluginExecutionEngine
    {
        private PluginExecutionData _executionData;
        private RebootActivityData _activityData;
        private DeviceWorkflowLogger _performanceLogger;
        private ActivityExecutionDetailLog _activityExecutionDetailLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="RebootExecutionControl" /> class.
        /// </summary>
        public RebootExecutionControl()
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
            _activityData = executionData.GetMetadata<RebootActivityData>();
            _performanceLogger = new DeviceWorkflowLogger(_executionData);

            TimeSpan lockTimeout = TimeSpan.FromMinutes(10);
            TimeSpan holdTimeout = TimeSpan.FromMinutes(60);


            UpdateStatus("Starting activity.");
            if (_executionData.Assets.OfType<IDeviceInfo>().Count() == 0)
            {
                return new PluginExecutionResult(PluginResult.Failed, $"There were no assets retrieved.  If this is a count-based run, your reservation in asset inventory may have expired.", "DeviceInfo Asset error");
            }

            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Failed to Start Reboot");


            try
            {
                var assetTokens = _executionData.Assets.OfType<IDeviceInfo>().Select(n => new AssetLockToken(n, lockTimeout, holdTimeout));
                _performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);

                ExecutionServices.CriticalSection.Run(assetTokens, selectedToken =>
                {
                    _performanceLogger.RecordEvent(DeviceWorkflowMarker.ActivityBegin);

                    IDeviceInfo asset = (selectedToken as AssetLockToken).AssetInfo as IDeviceInfo;
                    ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(_executionData, asset));
                    IDevice device = DeviceConstructor.Create(asset);


                    ExecutionServices.SystemTrace.LogInfo($@"Rebooting {asset.AssetId}");
                    UpdateStatus($@"Rebooting {asset.AssetId}");

                    result = RebootDevice(device);


                    //If we rebooted AND we want to set PJL, do so
                    if (_activityData.SetPaperless)
                    {

                        //Wait for WS* to come back up. It's one of the last services
                        WaitForService(device);

                        Thread.Sleep(60000);
                        EnablePJL(device);
                        Thread.Sleep(1000);
                        SetPaperlessPrintMode(true, device);
                    }

                });
            }
            catch (Exception e)
            {


                ExecutionServices.SystemTrace.LogInfo(e);
                UpdateStatus(e.Message);
                result = new PluginExecutionResult(PluginResult.Failed, e.Message);

            }


            UpdateStatus("Finished activity.");
            UpdateStatus($"Result = {result.Result}");

            return result;
        }

        private void UpdateStatus(string message)
        {
            statusRichTextBox.InvokeIfRequired(n =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status = " + message);
                n.AppendText($"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")}  {message}\n");
                n.Select(n.Text.Length, 0);
                n.ScrollToCaret();
            });
        }

        public static bool EnablePJL(IDevice device)
        {
            try
            {
                JediDevice jediDevice = new JediDevice(device.Address, device.AdminPassword);
                string urn = "urn:hp:imaging:con:service:security:SecurityService";
                string endpoint = "security";

                WebServiceTicket tic = jediDevice.WebServices.GetDeviceTicket(endpoint, urn);
                tic.FindElement("PjlDeviceAccess").SetValue("enabled");
                jediDevice.WebServices.PutDeviceTicket(endpoint, urn, tic);
            }
            catch (Exception e)
            {
                ExecutionServices.SystemTrace.LogInfo(e);
                throw;
            }
            return true;
        }

        public static bool SetPaperlessPrintMode(bool PaperlessModeOn, IDevice device)
        {
            JobMediaMode mode = PaperlessModeOn ? JobMediaMode.Paperless : JobMediaMode.Paper;
            bool success = false;
            try
            {
                IDeviceSettingsManager manager = DeviceSettingsManagerFactory.Create(device);
                success = manager.SetJobMediaMode(mode);
            }
            catch (DeviceFactoryCoreException)
            {
                return false;
            }

            return success;
        }

        private PluginExecutionResult RebootDevice(IDevice device)
        {
            PluginExecutionResult rebootResult = new PluginExecutionResult(PluginResult.Failed, "Failed to Reboot Device");

            //2 part operation, reboot start and wait for completion.

            rebootResult = TriggerReboot(device);

            if (rebootResult.Result != PluginResult.Passed)
            {
                return rebootResult;
            }

            rebootResult = WaitReboot(device);


            return rebootResult;
        }

        private PluginExecutionResult TriggerReboot(IDevice device)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Failed to Trigger Reboot");


            //FalconReboot
            //Powermanagement

           // bool jediRebootSuccess = false;
            try
            {
                JediOmniDevice dev = new JediOmniDevice(device.Address, device.AdminPassword);
                PUTCDM(device.Address, @"{""snmpv1v2Enabled"": ""true"",""accessOption"": ""readWrite"",""readOnlyPublicAllowed"": ""true"",""readOnlyCommunityNameSet"": ""false"",""writeOnlyCommunitryNameSet"": ""false""}");


                dev.PowerManagement.Reboot();
                //jediRebootSuccess = true;
                //_performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootBegin);
            }
            catch(Exception e)
            {
                ExecutionServices.SystemTrace.LogInfo($"Could not reboot via Jedi, Attempting Falcon Service. Error:{e.Message}");
                throw;
                
            }




            int maxRetries = (int)TimeSpan.FromMinutes(30).TotalSeconds / 10;


            if (Retry.UntilTrue(() => HasDeviceRebooted(device), maxRetries/5, TimeSpan.FromSeconds(5)))
            {
                _performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootBegin);
                result = new PluginExecutionResult(PluginResult.Passed);
                UpdateStatus("Device has Rebooted. Waiting for device to boot up...");
            }
            else
            {
                result = new PluginExecutionResult(PluginResult.Failed, $@"Failed to trigger reboot within {maxRetries*5} seconds");   
            }

            return result;
        }



        private PluginExecutionResult WaitReboot(IDevice device)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Failed to Bring up device from Reboot");
            //Convoluted, basically checks if we rebooted for up to half an hour.

            int retry = 0;
            bool controlPanelUp = false; //Actually webservices, but close enough.
            bool embeddedServerUp = false;
            int maxRetries = (int)TimeSpan.FromMinutes(30).TotalSeconds / 10;

            ExecutionServices.SystemTrace.LogInfo($"Max Retries: {maxRetries}");

            if (Retry.UntilTrue(() => IsDeviceRunning(device, retry++, ref controlPanelUp, ref embeddedServerUp), maxRetries, TimeSpan.FromSeconds(10)))
            {
                result = new PluginExecutionResult(PluginResult.Passed);
            }
            else
            {
                result = new PluginExecutionResult(PluginResult.Failed, $"Failed to detect that the device rebooted");
            }
            _performanceLogger.RecordEvent(DeviceWorkflowMarker.DeviceRebootEnd);
           return result;
        }

        private bool HasDeviceRebooted(IDevice device)
        {
            var status = device.GetDeviceStatus();
            Application.DoEvents();

            return (status == DeviceStatus.None || status == DeviceStatus.Unknown);
        }


        private bool IsDeviceRunning(IDevice device, int retry, ref bool controlPanelUp, ref bool embeddedServerUp)
        {
            ExecutionServices.SystemTrace.LogInfo($"Retry Count: {retry}");
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

        private void WaitForService(IDevice device)
        {
            int maxRetries = 10;
            if (Retry.UntilTrue(() =>
            {
                try
                {
                    device.GetDeviceInfo();
                }
                catch
                {
                    return false;
                }
                return true;
            }
            
            , maxRetries, TimeSpan.FromSeconds(10)))
            {
                return;
            }
            else
            {
                throw new DeviceWorkflowException($"Failed to detect that device services came up");
            }

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


        //private void MakeNVRAMCall(IDevice device)
        //{
        //    string jsonString = @"{""printer"": """ + device.Address + @""", ""nvps"":[[""1429e79e-d9ba-412e-a2bc-1f3d245041ce"",""REBOOT_SYSTEM"",""str"",""DUMMY""]";

        //    jsonString += @"], ""username"":""admin"", ""password"": " + $@"""{device.AdminPassword}""" + "}";



        //    var endurl = @"/jedi/nvpconfig";

        //    //var endurl = @"/jedi/fim";
        //    string id = POST(_webServiceURL, jsonString, endurl);
        //    ExecutionServices.SystemTrace.LogInfo(id);
        //    string temp = GET(_webServiceURL, id, endurl);

        //    try
        //    {
        //        while (temp.Contains("PENDING"))
        //        {
        //            temp = GET(_webServiceURL, id, endurl);
        //            Thread.Sleep(2000);
        //            ExecutionServices.SystemTrace.LogInfo(temp);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExecutionServices.SystemTrace.LogInfo(ex);
        //        throw ex;
        //    }
        //}


        //private static string POST(string url, string wherepost, string json)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + wherepost);
        //    request.Method = "POST";
        //    request.ContentType = "access-control-allow-headers";
        //    request.Proxy = null;
        //    UTF8Encoding encoding = new UTF8Encoding();
        //    byte[] byteArray = encoding.GetBytes(json);

        //    request.ContentLength = byteArray.Length;
        //    request.ContentType = @"application/json";

        //    using (Stream dataStream = request.GetRequestStream())
        //    {
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //    }
        //    long length = 0;
        //    string temp = "";
        //    try
        //    {
        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            Stream receiveStream = response.GetResponseStream();
        //            length = response.ContentLength;

        //            StreamReader read = new StreamReader(receiveStream, Encoding.UTF8);
        //            temp = read.ReadToEnd();

        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        ExecutionServices.SystemTrace.LogDebug(ex);
        //        // Log exception and throw as for GET example above
        //    }

        //    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(temp);

        //    return dict["id"];

        //}

        //private static string GET(string url, string wherepost, string id)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{url}{wherepost}?id={id}");
        //    request.Proxy = null;
        //    try
        //    {
        //        WebResponse response = request.GetResponse();
        //        using (Stream responseStream = response.GetResponseStream())
        //        {
        //            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
        //            return reader.ReadToEnd();
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        WebResponse errorResponse = ex.Response;
        //        using (Stream responseStream = errorResponse.GetResponseStream())
        //        {
        //            StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
        //            string errorText = reader.ReadToEnd();
        //            // log errorText
        //        }
        //        throw;
        //    }
        //}


        public static void PUTCDM(string url, string jsonContent)
        {
            string final = $@"https://{url}/hp/network/ioConfig/v1/networkInterfaces/wired1/snmpv1v2Config";
            //string jsonContent = @"{""snmpv1v2Enabled"": ""true"",""accessOption"": ""readWrite"",""readOnlyPublicAllowed"": ""true"",""readOnlyCommunityNameSet"": ""false"",""writeOnlyCommunitryNameSet"": ""false""}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(final);
            request.Method = "PUT";
            request.ContentType = "access-control-allow-headers";
            request.Proxy = null;
            request.Credentials = new NetworkCredential("admin", "!QAZ2wsx");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream receiveStream = response.GetResponseStream();
                    length = response.ContentLength;
                    Console.WriteLine(response.StatusCode);

                }
            }
            catch (WebException ex)
            {
                throw new WebException($"Check Admin Password. Otherwise: {ex.InnerException}");
                // Log exception and throw as for GET example above
            }

        }

    }
}
