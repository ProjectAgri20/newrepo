using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Net.Security;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HP.ScalableTest.Framework;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Text;
using System.Xml;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json.Linq;
using HP.ScalableTest.Framework.Assets;


namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    [DataContract]
    public class HPKInstallData : IComponentData
    {
        [DataMember]
        public List<HpkFileInfo> InstallFileList { get; set; }

        [DataMember]
        public decimal RetryCount { get; set; }

        [DataMember]
        public bool RetryCheck { get; set; }

        [DataMember]
        public bool SkipCheck { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();

        public HPKInstallData()
        {
            InstallFileList = new List<HpkFileInfo>();
            RetryCount = 1;
            RetryCheck = false;
            SkipCheck = false;
        }
        /// <summary>
        /// Individual function differences separated into delagate methods.
        /// </summary>
        public bool ExecuteJob(JediDevice device, AssetInfo assetInfo, PluginExecutionData data)
        {
            List<DevicePackageInfo> InstalledPackages = null;
            bool result = true;
            bool isIdle = GetInstallerStatus(device);

            while (!isIdle)
            {
                isIdle = GetInstallerStatus(device);
            }

            InstalledPackages = GetPackages(device);

            foreach (HpkFileInfo hpkfile in InstallFileList)
            {
                isIdle = GetInstallerStatus(device);
                while (!isIdle)
                {
                    Thread.Sleep(3000);
                    isIdle = GetInstallerStatus(device);
                }

                if (SkipCheck)
                {
                    if (!isExistPackage(device, hpkfile, InstalledPackages))
                    {
                        result &= UpdateHpk(hpkfile, device, assetInfo, data);
                    }
                }
                else
                {
                    result &= UpdateHpk(hpkfile, device, assetInfo, data);
                }
            }
            return result;
        }

        /// <summary>
        /// Check the package is already installed
        /// </summary>
        /// <param name="device">Jedi device</param>
        /// <param name="hpkfile">Hpk file information</param>
        /// <param name="installedPackages">List of installed packages</param>
        /// <returns></returns>
        public bool isExistPackage(JediDevice device, HpkFileInfo hpkfile, List<DevicePackageInfo> installedPackages)
        {
            string fileName = hpkfile.FilePath.Split('\\').Last();
            foreach (DevicePackageInfo p in installedPackages)
            {
                if (p.installedFileName.Split('.').First() == fileName.Split('.').First())
                {
                    Console.WriteLine($"{hpkfile.FilePath.Split('\\').Last()}({device.Address}) is already installed");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Control the installation process.
        /// </summary>
        /// <param name="hpkfile">Hpk file infomation</param>
        /// <param name="device">Jedi device</param>
        /// <param name="assetInfo">Asset information</param>
        /// <param name="pluginData">Plugin execution data</param>
        /// <returns></returns>
        public bool UpdateHpk(HpkFileInfo hpkfile, JediDevice device, AssetInfo assetInfo, PluginExecutionData pluginData)
        {
            bool success = false;
            int count = 0;
            DeviceConfigResultLog log = new DeviceConfigResultLog(pluginData, assetInfo.AssetId);
            string progressState = null;

            try
            {
                while (success == false && count < RetryCount)
                {
                    if (InstallPackage(device, hpkfile))
                    {
                        progressState = TrackPackage(device, hpkfile);

                        while (progressState == "psInProgress" || progressState == "404 Not Found" || progressState == "503 Service Unavailable" || string.IsNullOrEmpty(progressState))
                        {
                            Thread.Sleep(3000);
                            progressState = TrackPackage(device, hpkfile);
                        }
                        if (progressState == "psCompleted")
                        {
                            success = true;
                        }
                        else if (progressState == "psFailed")
                        {
                            success = false;
                        }
                        else
                        {
                            success = false;
                        }
                    }
                    else
                    {
                        success = false;
                        Thread.Sleep(10000);
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to install HPK : (Device:{device.Address}){hpkfile.PackageName}, {ex.Message}, progressState = {progressState}");
                _failedSettings.AppendLine($"Failed to install HPK: (Device:{device.Address}){hpkfile.PackageName}, {ex.Message}");
                success = false;
            }

            log.FieldChanged = hpkfile.PackageName;
            log.Result = success ? "Passed" : "Failed";
            log.Value = "HpkInstall Values";
            log.ControlChanged = $@"HpkInstall :{hpkfile.PackageName}";

            ExecutionServices.DataLogger.Submit(log);

            return success;
        }

        /// <summary>
        /// Check the service is available.
        /// </summary>
        /// <param name="device">Jedi device</param>
        /// <returns></returns>
        public bool GetInstallerStatus(JediDevice device)
        {
            Uri installer_state_uri = new Uri($"https://{device.Address}/hp/device/webservices/ext/pkgmgt/installer");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpClient myclient = new HttpClient();
            HttpClientHandler httphandler = new HttpClientHandler();
            httphandler.Credentials = new NetworkCredential("admin", device.AdminPassword);
            httphandler.PreAuthenticate = true;
            var client = new HttpClient(httphandler);
            try
            {
                using (HttpResponseMessage message = client.GetAsync(installer_state_uri).Result)
                {
                    string s = message.Content.ReadAsStringAsync().Result;
                    if (s.Contains("insIdle"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (WebException ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return false;
            }
            catch (HttpRequestException ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return false;
            }
            catch (SocketException ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return false;
            }
            catch (AggregateException ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                throw ex;
            }
        }

        /// <summary>
        /// Get installed package list of device
        /// </summary>
        /// <param name="device">Jedi device</param>
        /// <returns></returns>
        public List<DevicePackageInfo> GetPackages(JediDevice device)
        {
            List<DevicePackageInfo> InstalledPackageList = new List<DevicePackageInfo>();
            Uri delete_packages_uri = new Uri($"https://{device.Address}/hp/device/webservices/ext/pkgmgt/packages");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpClientHandler httphandler = new HttpClientHandler();
            httphandler.Credentials = new NetworkCredential("admin", device.AdminPassword);
            var client = new HttpClient(httphandler);
            
            try
            {
                using (HttpResponseMessage message = client.GetAsync(delete_packages_uri).Result)
                {
                    JArray jarray;
                    string s = message.Content.ReadAsStringAsync().Result;
                    jarray = JArray.Parse(s);

                    foreach (var jtocken in jarray.Children())
                    {
                        var elements = jtocken.Children<JProperty>();
                        string uuid = elements.FirstOrDefault(x => x.Name == "uuid").Value.ToString();
                        string packageName = elements.FirstOrDefault(x => x.Name == "name").Value.ToString();
                        string version = elements.FirstOrDefault(x => x.Name == "version").Value.ToString();
                        string metadata = elements.FirstOrDefault(x => x.Name == "metaData").Value.ToString();

                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(metadata);
                        string installedFile = xml.GetElementsByTagName("installFile")[0].InnerText;
                        InstalledPackageList.Add(new DevicePackageInfo(packageName, version, uuid, installedFile));
                    }
                    return InstalledPackageList;
                }
            }
            catch (WebException ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return new List<DevicePackageInfo>(); ;
            }

            catch (HttpRequestException ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return new List<DevicePackageInfo>();
            }

            catch (SocketException ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return new List<DevicePackageInfo>();
            }
            catch (AggregateException ex)
            {

                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return new List<DevicePackageInfo>();
            }

            catch (Exception ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                throw ex;
            }
        }

        /// <summary>
        /// Request install to package manager
        /// </summary>
        /// <param name="device">Jedi device</param>
        /// <param name="hpkfile">Hpk file information</param>
        /// <returns></returns>
        public bool InstallPackage(JediDevice device, HpkFileInfo hpkfile)
        {
            Uri install_packages_uri = new Uri($"https://{device.Address}/hp/device/webservices/ext/pkgmgt/installer/install?clientId=ciJamc&installSource=isStandardRepository&forceInstall=true&acceptPermissions=true");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpClientHandler httphandler = new HttpClientHandler();
            httphandler.Credentials = new NetworkCredential("admin", device.AdminPassword);
            httphandler.PreAuthenticate = true;
            var client = new HttpClient(httphandler);
            client.Timeout = TimeSpan.FromSeconds(110);
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.DefaultRequestHeaders.Connection.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;  // true = keepalive off

            ByteArrayContent fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(hpkfile.FilePath));
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            fileContent.Headers.ContentDisposition.Name = "\"file\"";
            fileContent.Headers.ContentDisposition.FileName = "\"" + hpkfile.PackageName + "\"";
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.hp.package-archive");

            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(fileContent);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, install_packages_uri);
            request.Content = content;

            try
            {
                using (HttpResponseMessage message = client.SendAsync(request).Result)
                {
                    //string s = message.Content.ReadAsStringAsync().Result;
                    string s = message.Headers.Location.ToString();
                    if (s.Contains(hpkfile.Uuid))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            catch (WebException ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return false;
            }

            catch (HttpRequestException ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return false;
            }

            catch (SocketException ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return false;
            }

            catch (AggregateException ex)
            {

                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return false;
            }

            catch (Exception ex)
            {
                Logger.LogError($"InstallPackage Error : {hpkfile.PackageName}, {ex.Message}");
                _failedSettings.AppendLine($"InstallPackage Error: {hpkfile.PackageName}, {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// Request uninstall to package manager
        /// </summary>
        /// <param name="device">Jedi device</param>
        /// <param name="hpkfile">Hpk file Information</param>
        public void UninstallPackage(JediDevice device, HpkFileInfo hpkfile)
        {
            Uri delete_packages_uri = new Uri($"https://{device.Address}/hp/device/webservices/ext/pkgmgt/installer/uninstall?uuid={hpkfile.Uuid}&clientId=ciGallery");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpClientHandler httphandler = new HttpClientHandler();
            httphandler.Credentials = new NetworkCredential("admin", device.AdminPassword);
            var client = new HttpClient(httphandler);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, delete_packages_uri);
            try
            {
                using (HttpResponseMessage message = client.SendAsync(request).Result)
                {
                    string s = message.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                throw ex;
            }
        }

        /// <summary>
        /// Tracking state of installation
        /// </summary>
        /// <param name="device">Jedi device</param>
        /// <param name="hpkfile">Hpk file information</param>
        /// <returns></returns>
        public string TrackPackage(JediDevice device, HpkFileInfo hpkfile)
        {
            Uri track_install_uri = new Uri($"https://{device.Address}/hp/device/webservices/ext/pkgmgt/installer/install/{hpkfile.Uuid}");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpClientHandler httphandler = new HttpClientHandler();
            httphandler.Credentials = new NetworkCredential("admin", device.AdminPassword);
            var client = new HttpClient(httphandler);

            try
            {
                using (HttpResponseMessage message = client.GetAsync(track_install_uri).Result)
                {
                    string s = message.Content.ReadAsStringAsync().Result;
                    if (s.Contains("psInProgress"))
                    {
                        return "psInProgress";
                    }
                    else if (s.Contains("psCompleted"))
                    {
                        return "psCompleted";
                    }
                    else if (s.Contains("psFailed"))
                    {
                        return "psFailed";
                    }
                    else if (s.Contains("404 Not Found"))
                    {
                        return "404 Not Found";
                    }
                    else if (s.Contains("503 Service Unavailable"))
                    {
                        return "503 Service Unavailable";
                    }
                    else
                    {
                        Logger.LogError($"TrackPackage unknown state error:{device.Address}:{hpkfile.PackageName}:{s}");
                        return null;
                    }
                }
            }
            catch (AggregateException ex)
            {

                Logger.LogError(ex.Message + $"Device : {device.Address}");
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message + $"Device : {device.Address}");
                throw ex;
            }
        }
        /// <summary>
        /// Throws NotImplementedException since installer does not utilize functionality
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ChangeValue"></param>
        /// <param name="device"></param>
        /// <param name="data"></param>
        /// <param name="urn"></param>
        /// <param name="endpoint"></param>
        /// <param name="assetInfo"></param>
        /// <param name="activity"></param>
        /// <param name="pluginExecutionData"></param>
        /// <returns>Success bool</returns>
        public bool UpdateField<T>(Func<WebServiceTicket, WebServiceTicket> ChangeValue, JediDevice device, DataPair<T> data, string urn, string endpoint, AssetInfo assetInfo, string fieldChanged, Framework.Plugin.PluginExecutionData pluginData)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Interface function to drive setting of data fields and return results upstream
        /// </summary>
        /// <param name="device"></param>
        /// <param name="assetInfo"></param>
        /// <param name="pluginExecutionData"></param>
        /// <returns>result</returns>
        public DataPair<string> SetFields(JediDevice device, AssetInfo assetInfo, PluginExecutionData data)
        {
            _failedSettings = new StringBuilder();
            var result = ExecuteJob(device, assetInfo, data);
            return new DataPair<string> { Key = _failedSettings.ToString(), Value = result };
        }
    }
}
