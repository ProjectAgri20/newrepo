using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.CertificateInstaller
{
    public partial class CertificateInstallerExecutionEngine : IPluginExecutionEngine
    {
        private CertificateInstallerActivityData _activityData = null;
        private BrowserAgent _userAgent;
        private IDevice _device;

       

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _activityData = executionData.GetMetadata<CertificateInstallerActivityData>();

            PrintDeviceInfo printDeviceInfo = (PrintDeviceInfo)executionData.Assets.First();
            _device = DeviceConstructor.Create(printDeviceInfo);

            try
            {
                string authorization = string.Empty;

                _userAgent = GetUserAgent(_activityData.BrowserType);

                // Installs certificate on Client VM
                if (_activityData.ClientVMCA)
                {
                    ExecutionServices.SystemTrace.LogDebug($"Certificate Path { (object)_activityData.CACertificate}");
                    InstallVMCertificate(_activityData.CACertificate);
                }

                // Check for the printer availability
                if (!PingUntilTimeout(IPAddress.Parse(_device.Address), TimeSpan.FromMinutes(1)))
                {
                    string errorMessage = $"Ping failed with IP Address:{ (object)_device.Address}";
                    ExecutionServices.SystemTrace.LogDebug(errorMessage);
                    _device.Dispose();
                    return new PluginExecutionResult(PluginResult.Failed, errorMessage);
                }

                if (!string.IsNullOrEmpty(_device.AdminPassword))
                {
                    string credentials = $"admin:{_device.AdminPassword}";
                    byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(credentials);
                    authorization = "Basic " + Convert.ToBase64String(plainTextBytes);
                }

                // Enterprise Lock on device so that only one dispatcher can reserve at a time.
                // Action action = new Action(() =>
                {
                    //Installs CA certificate on the printer
                    if (_activityData.InstallPrinterCA)
                    {
                        ExecutionServices.CriticalSection.Run(new AssetLockToken(executionData.Assets.First(), new LockTimeoutData(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1))), () => InstallCertificate(authorization));
                    }

                    //Deletes CA certificate from the printer
                    if (_activityData.DeletePrinterCA)
                    {
                        ExecutionServices.CriticalSection.Run(new AssetLockToken(executionData.Assets.First(), new LockTimeoutData(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1))), () => DeleteCertificate(authorization, _device.Address));
                    }
                }
            }
            catch (Exception exception)
            {
                var failureMessage = $"Activity failed on device {_device.Address} with exception {exception.Message}";
                _device.Dispose();
                return new PluginExecutionResult(PluginResult.Failed, failureMessage);
                
            }

            _device.Dispose();
            return new PluginExecutionResult(PluginResult.Passed);
        }

        private void DeleteCertificate(string authorization, string deviceAddress)
        {
            Uri printerUrl = GetPrinterUrl(_device.Address);
            ExecutionServices.SystemTrace.LogDebug("Deleting certificate from printer");

            if (_device is PhoenixDevice)
            {
                DeleteCAFilePhoenix(printerUrl, authorization);
            }
            else if (_device is SiriusDevice)
            {
                DeleteCAFileInk(printerUrl, authorization, _activityData.CACertificate);
            }
            else if (_device is JediDevice)
            {
                DeleteCAFileJedi(printerUrl, authorization, GetHostName(deviceAddress));
            }
            else
            {
                throw new UnknownDeviceTypeException("Device is not supported");
            }
        }

        private void InstallCertificate(string authorization)
        {
            Uri printerUrl = GetPrinterUrl(_device.Address);
            ExecutionServices.SystemTrace.LogDebug($"Certificate Path {_activityData.CACertificate}");
            NameValueCollection nvc = new NameValueCollection();

            if (_device is PhoenixDevice)
            {
                nvc.Add("FinishText", "Finish");
                InstallCAFilePhoenix(printerUrl, _activityData.CACertificate, authorization, nvc);
            }
            else if (_device is SiriusDevice)
            {
                //Inkjet printers allow installation of multiple certificates
                string resourceUrl = GetCertificateUrl(printerUrl, authorization, _activityData.CACertificate);
                if (!string.IsNullOrEmpty(resourceUrl))
                {
                    ExecutionServices.SystemTrace.LogDebug(
                        $"Certificate already exists: {_activityData.CACertificate}");
                }
                InstallCAFileInk(printerUrl, _activityData.CACertificate, authorization, nvc);
            }
            else if (_device is JediDevice)
            {
                InstallCAFileJedi(printerUrl, _activityData.CACertificate, authorization,
                    _activityData.IntermediateCA);
            }
            else
            {
                throw new UnknownDeviceTypeException("Device is not supported");
            }
        }

        /// <summary>
        /// Pings the printer until it is success or time out happens
        /// </summary>
        /// <param name="ipAddress">IP Address of the printer</param>
        /// <param name="timeout">Number of minutes to wait</param>
        /// <returns>Returns true if the printer is pinging in the timeout else return false</returns>
        public static bool PingUntilTimeout(IPAddress ipAddress, TimeSpan timeout)
        {
            using (Ping ping = new Ping())
            {
                DateTime endTime = DateTime.Now + timeout;

                PingReply pingStatus = ping.Send(ipAddress);

                // ping the printer until it succeeds or till a timeout occurs
                while ((pingStatus.Status != IPStatus.Success) && (DateTime.Now < endTime))
                {
                    pingStatus = ping.Send(ipAddress);
                }

                return (pingStatus.Status == IPStatus.Success);
            }
        }

        /// <summary>
        /// Installs CA certificate on Inkjet printer
        /// </summary>
        /// <param name="printerIp"></param>
        /// <param name="file"></param>
        /// <param name="authorization"></param>
        /// <param name="nameCollection"></param>
        public void InstallCAFileInk(Uri printerIp, string file, string authorization, NameValueCollection nameCollection)
        {
            Uri uploadUrl = null;
            var fileExtension = Path.GetExtension(file);
            if (fileExtension != null && fileExtension.Equals(".cer", StringComparison.OrdinalIgnoreCase))
            {
                uploadUrl = new Uri($"{ (object)printerIp}{ (object)Properties.Resources.URLInstallInkCer}");
            }
            else
            {
                uploadUrl = new Uri($"{printerIp}{Properties.Resources.URLInstallInkPfx}");
            }

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
            webRequest.Accept = "text/html, application/xhtml+xml, */*";
            webRequest.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            if (!string.IsNullOrEmpty(authorization))
            {
                webRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
            }
            try
            {
                var response = HttpWebEngine.UploadFile(webRequest, file, "certificate", nameCollection, _userAgent);

                if ((response.StatusCode == HttpStatusCode.OK) || (response.StatusCode == HttpStatusCode.Created))
                {
                    if (fileExtension != null && fileExtension.Equals(".cer", StringComparison.OrdinalIgnoreCase))
                    {
                        ExecutionServices.SystemTrace.LogDebug("Certificate installed!");
                        VerifyCAUploadInk(printerIp);
                    }
                    else
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(response.Response);

                        string certError = xDoc.DocumentElement.GetElementsByTagName("cert:InterfaceError").Item(0).InnerText;
                        if (certError.Contains("invalidCertData"))
                        {
                            ExecutionServices.SystemTrace.LogDebug("File is unsupported file format");
                            throw new SiriusInvalidOperationException("Certificate is invalid");
                        }
                    }
                }
                else
                {
                    ExecutionServices.SystemTrace.LogDebug("Upload Failed");
                    throw new WebException("Upload Failed");
                }
            }
            catch (WebException ex)
            {
                ExecutionServices.SystemTrace.LogDebug("Upload Failed");
                throw new WebException("Upload Failed", ex);
            }
        }

        /// <summary>
        /// Installs CA certificate on the printer
        /// </summary>
        /// <param name="printerIp"></param>
        /// <param name="file"></param>
        /// <param name="authorization"></param>
        /// <param name="nameCollection"></param>
        public void InstallCAFilePhoenix(Uri printerIp, string file, string authorization, NameValueCollection nameCollection)
        {
            Uri uploadUrl = new Uri($"{printerIp}{Properties.Resources.URLInstallPhoenix}");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
            webRequest.Accept = "text/html, application/xhtml+xml, */*";
            webRequest.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            if (!string.IsNullOrEmpty(authorization))
            {
                webRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
                webRequest.AllowAutoRedirect = false;
            }

            try
            {
                var response = HttpWebEngine.UploadFile(webRequest, file, "upFile", nameCollection, _userAgent);

                if (!string.IsNullOrEmpty(authorization))
                {
                    if (response.StatusCode == HttpStatusCode.Found)
                    {
                        response = GetUploadStatus(printerIp, authorization, file);
                        if (response.Response.Contains("The format of the file is invalid."))
                        {
                            ExecutionServices.SystemTrace.LogDebug("The format of the file is invalid.");
                            throw new FormatException("The format of the file is invalid.");
                        }
                    }
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response = GetUploadStatus(printerIp, authorization, file);
                    if (response.Response.Contains("The format of the file is invalid."))
                    {
                        ExecutionServices.SystemTrace.LogDebug("The format of the file is invalid.");
                        throw new FormatException("The format of the file is invalid.");
                    }

                    ExecutionServices.SystemTrace.LogDebug("Certificate installed!");

                    if (!VerifyCAUploadPhoenix(printerIp, authorization, true))
                    {
                        ExecutionServices.SystemTrace.LogDebug("Certificate upload verification failed");
                        throw new WebException("Certificate upload verification failed");
                    }
                }
                else
                {
                    throw new WebException("Upload Failed");
                }
            }
            catch (WebException ex)
            {
                throw new WebException("Upload Failed", ex);
            }
        }

        public HttpWebResult GetSessionIdRequest(Uri printerIp, string authorization)
        {
            Uri statusUrl = new Uri($"{printerIp}{Properties.Resources.URLJediHome}");

            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(statusUrl);
            getRequest.ContentType = "application/x-www-form-urlencoded";
            getRequest.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            getRequest.Accept = Properties.Resources.GetRequestAccept;
            getRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

            if (!string.IsNullOrEmpty(authorization))
            {
                getRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
                getRequest.AllowAutoRedirect = false;
            }

            return HttpWebEngine.Get(getRequest, _userAgent);
        }

        public HttpWebResult GetWizardIdResponse(Uri printerIp, string authorization, CookieCollection cookieCollection)
        {
            Uri uploadUrl = new Uri($"{printerIp}{Properties.Resources.URLJediWizard}");

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
            webRequest.Accept = Properties.Resources.WebRequestAccept;
            webRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            webRequest.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(authorization))
            {
                webRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
                webRequest.AllowAutoRedirect = false;
            }

            string postData = string.Format(Properties.Resources.WizardRequest);

            webRequest.CookieContainer = new CookieContainer();
            webRequest.CookieContainer.Add(cookieCollection);
            return HttpWebEngine.Post(webRequest, postData, _userAgent);
        }

        /// <summary>
        /// Installs CA certificate on VEP printer
        /// </summary>
        /// <param name="printerIp"></param>
        /// <param name="file"></param>
        /// <param name="authorization"></param>
        /// <param name="allowIntermediate"></param>
        public void InstallCAFileJedi(Uri printerIp, string file, string authorization, bool allowIntermediate)
        {
            //Retrieve Session ID
            HttpWebResult webResultGetReqeust = GetSessionIdRequest(printerIp, authorization);
            var cookies = webResultGetReqeust.Cookies;

            for (int i = 0; i < webResultGetReqeust.Headers.Count; i++)
            {
                if (webResultGetReqeust.Headers.Get(i).Contains("sessionId"))
                {
                    string strTemp = webResultGetReqeust.Headers.Get(i);
                    strTemp = strTemp.Replace("sessionId=", "");
                    strTemp = strTemp.Replace("; path=/;", "");
                    cookies.Add(new Cookie("sessionId", strTemp) { Domain = "localhost" });
                }
            }

            //Retrieve WizardID and HideValue
            var getResponse = GetWizardIdResponse(printerIp, authorization, cookies);

            string wizardid = getResponse.Headers.Get("Set-Cookie");
            int startindex = getResponse.Response.IndexOf("Hide\" VALUE=\"", StringComparison.OrdinalIgnoreCase) + 13;
            string truncatedResponse = getResponse.Response.Substring(startindex);
            int endIndex = truncatedResponse.IndexOf("\">", StringComparison.OrdinalIgnoreCase);
            string hideValue = truncatedResponse.Substring(0, endIndex);

            //Step 1 : Post Wizard page
            Uri uploadUrl = new Uri($"{printerIp}{Properties.Resources.URLJediCookie}");
            HttpWebRequest optionRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
            optionRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            wizardid = wizardid.Replace("wizardid=", "");
            wizardid = wizardid.Replace(";PATH=/;SECURE", "");
            cookies.Add(new Cookie("wizardid", wizardid) { Domain = "localhost" });

            optionRequest.CookieContainer = new CookieContainer();
            optionRequest.CookieContainer.Add(cookies);
            optionRequest.Accept = Properties.Resources.WebRequestAccept;
            optionRequest.ContentType = "application/x-www-form-urlencoded";
            optionRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            optionRequest.Headers.Add("Accept-Language", "en-US");
            optionRequest.Host = uploadUrl.Host;
            optionRequest.KeepAlive = true;

            string postOptionData = Properties.Resources.OptionRequestJediInstall + hideValue;

            if (!string.IsNullOrEmpty(authorization))
            {
                optionRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
                optionRequest.AllowAutoRedirect = false;
            }

            try
            {
                var optionResponse = HttpWebEngine.Post(optionRequest, postOptionData, _userAgent);
                if (optionResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("Certificate option page POST failed");
                }
            }
            catch (WebException ex)
            {
                throw new WebException("Certificate option page POST failed", ex);
            }

            //Step 2 : Upload File
            NameValueCollection nvc = new NameValueCollection();
            if (allowIntermediate)
            {
                nvc.Add("Intermediate_CA", "on");
            }
            nvc.Add("Hide", hideValue);
            nvc.Add("Finish", "Finish");
            uploadUrl = new Uri($"{printerIp}{Properties.Resources.URLJediUpload}");
            HttpWebRequest installRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);

            installRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            wizardid = wizardid.Replace("wizardid=", "");
            wizardid = wizardid.Replace(";PATH=/", "");
            cookies.Add(new Cookie("wizardid", wizardid) { Domain = "localhost" });

            installRequest.CookieContainer = new CookieContainer();
            installRequest.CookieContainer.Add(cookies);

            installRequest.Accept = Properties.Resources.WebRequestAccept;
            installRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            installRequest.ContentType = "application/x-www-form-urlencoded";
            installRequest.Headers.Add("Accept-Language", "en-US");
            installRequest.Host = uploadUrl.Host;
            installRequest.KeepAlive = true;
            installRequest.ServicePoint.Expect100Continue = false;
            if (!string.IsNullOrEmpty(authorization))
            {
                installRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
                installRequest.AllowAutoRedirect = false;
            }

            try
            {
                var uploadResponse = HttpWebEngine.UploadFile(installRequest, file, ".Install_FileName_handle", nvc, _userAgent);
                if (uploadResponse.StatusCode == HttpStatusCode.OK)
                {
                    var responseError = !allowIntermediate ? "The format of the file is invalid." : "The certificate entered was invalid. Please try again and be sure to include the entire certificate correctly.";
                    if (uploadResponse.Response.Contains(responseError))
                    {
                        CloseWizard(printerIp, authorization, wizardid, ".Install_FileName_handle", hideValue);
                        ExecutionServices.SystemTrace.LogDebug(responseError);
                        return;
                    }

                    CloseWizard(printerIp, authorization, wizardid);

                    ExecutionServices.SystemTrace.LogDebug("Certificate installed!");
                    if (!VerifyCAUploadJedi(printerIp, authorization, true))
                    {
                        throw new WebException("Certificate upload verification failed");
                    }
                }
                else
                {
                    throw new WebException("Upload Failed");
                }
            }
            catch (WebException ex)
            {
                throw new WebException("Upload Failed", ex);
            }
        }

        /// <summary>
        /// Close the Wizard page of CA certificate installation for VEP printers
        /// </summary>
        /// <param name="printerIp"></param>
        /// <param name="authorization"></param>
        /// <param name="wizardid"></param>
        private void CloseWizard(Uri printerIp, string authorization, string wizardid)
        {
            Uri uploadUrl = new Uri($"{printerIp}{Properties.Resources.URLJediClose}");
            HttpWebRequest oKRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
            oKRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            CookieCollection cookies = new CookieCollection {new Cookie("wizardid", wizardid) {Domain = "localhost"}};

            oKRequest.CookieContainer = new CookieContainer();
            oKRequest.CookieContainer.Add(cookies);
            oKRequest.Accept = Properties.Resources.WebRequestAccept;
            oKRequest.ContentType = "application/x-www-form-urlencoded";
            oKRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            oKRequest.Headers.Add("Accept-Language", "en-US");
            oKRequest.Host = uploadUrl.Host;
            oKRequest.KeepAlive = true;

            string optionData = "ok=OK";

            if (!string.IsNullOrEmpty(authorization))
            {
                oKRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
                oKRequest.AllowAutoRedirect = false;
            }
            try
            {
                var request = HttpWebEngine.Post(oKRequest, optionData, _userAgent);
                if (request.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("Certificate Wizard closure failed");
                }
            }
            catch (WebException ex)
            {
                throw new WebException("Certificate Wizard closure failed", ex);
            }
        }

        /// <summary>
        /// Close the Wizard page of CA certificate installation for VEP printers
        /// </summary>
        /// <param name="printerIp"></param>
        /// <param name="authorization"></param>
        /// <param name="wizardid"></param>
        /// <param name="paramName"></param>
        /// <param name="hideValue"></param>
        private void CloseWizard(Uri printerIp, string authorization, string wizardid, string paramName, string hideValue)
        {
            Uri uploadUrl = new Uri($"{printerIp}{Properties.Resources.URLJediError}");
            HttpWebRequest oKRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
            oKRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            CookieCollection cookies = new CookieCollection {new Cookie("wizardid", wizardid) {Domain = "localhost"}};

            oKRequest.CookieContainer = new CookieContainer();
            oKRequest.CookieContainer.Add(cookies);
            oKRequest.Accept = Properties.Resources.WebRequestAccept;
            oKRequest.ContentType = "application/x-www-form-urlencoded";
            oKRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            oKRequest.Headers.Add("Accept-Language", "en-US");
            oKRequest.Host = uploadUrl.Host;
            oKRequest.KeepAlive = true;

            string optionData = "ok=OK";

            if (!string.IsNullOrEmpty(authorization))
            {
                oKRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
                oKRequest.AllowAutoRedirect = false;
            }
            try
            {
                var request = HttpWebEngine.Post(oKRequest, optionData, _userAgent);
                if (request.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("Certificate Wizard closure failed");
                }
            }
            catch (WebException ex)
            {
                throw new WebException("Certificate Wizard closure failed", ex);
            }

            //Step 2 : Upload Empty File
            NameValueCollection nvc = new NameValueCollection {{"Hide", hideValue}, {"Cancel", "Cancel"}};
            uploadUrl = new Uri($"{printerIp}{Properties.Resources.URLJediUpload}");
            HttpWebRequest cancelRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);

            cancelRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            cancelRequest.CookieContainer = new CookieContainer();
            cancelRequest.CookieContainer.Add(cookies);

            cancelRequest.Accept = Properties.Resources.WebRequestAccept;
            cancelRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            cancelRequest.ContentType = "application/x-www-form-urlencoded";
            cancelRequest.Headers.Add("Accept-Language", "en-US");
            cancelRequest.Host = uploadUrl.Host;
            cancelRequest.KeepAlive = true;
            cancelRequest.ServicePoint.Expect100Continue = false;
            if (!string.IsNullOrEmpty(authorization))
            {
                cancelRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
                cancelRequest.AllowAutoRedirect = false;
            }

            try
            {
                var uploadResponse = HttpWebEngine.UploadFile(cancelRequest, "", paramName, nvc, _userAgent);
                if (uploadResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("Certificate Wizard closure failed");
                }
            }
            catch (WebException ex)
            {
                throw new WebException("Certificate Wizard closure failed", ex);
            }
        }

        private static Uri GetPrinterUrl(string ipAddress)
        {
            Uri printerIp = null;
            IPAddress address = IPAddress.Parse(ipAddress);
            if (address.AddressFamily == AddressFamily.InterNetwork) // IPv4
            {
                printerIp = new Uri($"https://{ (object)ipAddress}");
            }
            else if (address.AddressFamily == AddressFamily.InterNetworkV6) // IPv6
            {
                printerIp = new Uri($"https://[{ipAddress}]");
            }
            return printerIp;
        }

        private static BrowserAgent GetUserAgent(string userAgent)
        {
            BrowserAgent agent = BrowserAgent.IE;
            if (userAgent == "IE7" || userAgent == "IE8" || userAgent == "IE9")
                agent = BrowserAgent.IE;
            else if (userAgent == "Firefox")
                agent = BrowserAgent.Firefox;
            else if (userAgent == "Chrome")
                agent = BrowserAgent.Chrome;
            else if (userAgent == "Opera")
                agent = BrowserAgent.Opera;
            else if (userAgent == "Safari")
                agent = BrowserAgent.Safari;
            return agent;
        }

        private bool VerifyCAUploadJedi(Uri printerIp, string authorization, bool install)
        {
            bool result;

            Uri certificateUrl = new Uri($"{printerIp}{Properties.Resources.URLJediVerify}");

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(certificateUrl);
            if (!string.IsNullOrEmpty(authorization))
            {
                req.Headers.Add(HttpRequestHeader.Authorization, authorization);
            }

            // grab the response
            try
            {
                var verifyCertificate = HttpWebEngine.Get(req, _userAgent);
                if (verifyCertificate.StatusCode == HttpStatusCode.OK)
                {
                    string source = verifyCertificate.Response;
                    result = source.Contains(install ? "name=\"CAview\"" : "disabled=\"disabled\" name=\"CAviewdis\"");
                }
                else
                {
                    string errorStatus = "Certificate upload verification failed. Status Code: " + verifyCertificate.StatusCode;
                    throw new WebException(errorStatus);
                }
            }
            catch (WebException ex)
            {
                string errorStatus = "Certificate upload verification failed.";
                throw new WebException(errorStatus, ex);
            }

            return result;
        }

        /// <summary>
        /// Deletes CA certificate from the printer
        /// </summary>
        /// <param name="printerIp"></param>
        /// <param name="authorizationPassword"></param>
        /// <param name="hostName"></param>
        public void DeleteCAFileJedi(Uri printerIp, string authorizationPassword, string hostName)
        {
            if (!VerifyCAUploadJedi(printerIp, authorizationPassword, true))
            {
                throw new InvalidOperationException("Printer does not have a CA Certificate");
            }

            //Retrieve Session ID
            Uri statusUrl = new Uri($"{printerIp}{Properties.Resources.URLJediHome}");

            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(statusUrl);
            getRequest.ContentType = "application/x-www-form-urlencoded";
            getRequest.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            getRequest.Accept = Properties.Resources.GetRequestAccept;
            getRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

            if (!string.IsNullOrEmpty(authorizationPassword))
            {
                getRequest.Headers.Add(HttpRequestHeader.Authorization, authorizationPassword);
                getRequest.AllowAutoRedirect = false;
            }

            HttpWebResult webResultGetReqeust = HttpWebEngine.Get(getRequest, _userAgent);

            var cookies = webResultGetReqeust.Cookies;

            for (int i = 0; i < webResultGetReqeust.Headers.Count; i++)
            {
                if (webResultGetReqeust.Headers.Get(i).Contains("sessionId"))
                {
                    string strTemp = webResultGetReqeust.Headers.Get(i);
                    strTemp = strTemp.Replace("sessionId=", "");
                    strTemp = strTemp.Replace("; path=/;", "");
                    cookies.Add(new Cookie("sessionId", strTemp) { Domain = "localhost" });
                }
            }

            //Retrieve WizardID and HideValue
            Uri uploadUrl = new Uri($"{printerIp}{Properties.Resources.URLJediDeleteWizard}");

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
            webRequest.Accept = Properties.Resources.WebRequestAccept;
            webRequest.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            webRequest.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(authorizationPassword))
            {
                webRequest.Headers.Add(HttpRequestHeader.Authorization, authorizationPassword);
                webRequest.AllowAutoRedirect = false;
            }

            string postData = string.Format(Properties.Resources.ConfigRequestJediDelete, hostName);

            var getResponse = HttpWebEngine.Post(webRequest, postData, _userAgent);

            string wizardid = getResponse.Headers.Get("Set-Cookie");
            int startindex = getResponse.Response.IndexOf("Hide\" VALUE=\"", StringComparison.OrdinalIgnoreCase) + 13;
            string truncatedResponse = getResponse.Response.Substring(startindex);
            int endIndex = truncatedResponse.IndexOf("\">", StringComparison.OrdinalIgnoreCase);
            string hideValue = truncatedResponse.Substring(0, endIndex);

            //Step 1
            uploadUrl = new Uri($"{printerIp}{Properties.Resources.URLJediCookie}");
            HttpWebRequest optionRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
            optionRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            wizardid = wizardid.Replace("wizardid=", "");
            wizardid = wizardid.Replace(";PATH=/;SECURE", "");
            cookies.Add(new Cookie("wizardid", wizardid) { Domain = "localhost" });

            optionRequest.CookieContainer = new CookieContainer();
            optionRequest.CookieContainer.Add(cookies);

            optionRequest.Accept = Properties.Resources.WebRequestAccept;
            optionRequest.ContentType = "application/x-www-form-urlencoded";
            optionRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            optionRequest.Headers.Add("Accept-Language", "en-US");
            optionRequest.Host = uploadUrl.Host;
            optionRequest.KeepAlive = true;

            string postData1 = Properties.Resources.OptionRequestJediDelete + hideValue;

            if (!string.IsNullOrEmpty(authorizationPassword))
            {
                optionRequest.Headers.Add(HttpRequestHeader.Authorization, authorizationPassword);
                optionRequest.AllowAutoRedirect = false;
            }

            try
            {
                var optionResponse = HttpWebEngine.Post(optionRequest, postData1, _userAgent);

                if (optionResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("Certificate option POST failed");
                }
            }
            catch (WebException ex)
            {
                throw new WebException("Certificate option POST failed", ex);
            }

            //Step 2
            uploadUrl = new Uri($"{printerIp}{Properties.Resources.URLJediFinish}");
            HttpWebRequest delRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
            delRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            wizardid = wizardid.Replace("wizardid=", "");
            wizardid = wizardid.Replace(";PATH=/", "");
            cookies.Add(new Cookie("wizardid", wizardid) { Domain = "localhost" });

            delRequest.CookieContainer = new CookieContainer();
            delRequest.CookieContainer.Add(cookies);

            delRequest.Accept = Properties.Resources.WebRequestAccept;
            delRequest.ContentType = "application/x-www-form-urlencoded";
            delRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            delRequest.Headers.Add("Accept-Language", "en-US");
            delRequest.Host = uploadUrl.Host;
            delRequest.KeepAlive = true;
            delRequest.ServicePoint.Expect100Continue = false;

            string postData2 = Properties.Resources.FinishData + hideValue;

            if (!string.IsNullOrEmpty(authorizationPassword))
            {
                delRequest.Headers.Add(HttpRequestHeader.Authorization, authorizationPassword);
                delRequest.AllowAutoRedirect = false;
            }

            try
            {
                var postResponse = HttpWebEngine.Post(delRequest, postData2, _userAgent);

                if (postResponse.StatusCode == HttpStatusCode.OK)
                {
                    CloseWizard(printerIp, authorizationPassword, wizardid);
                    ExecutionServices.SystemTrace.LogDebug("Deleted certificate from printer");
                    if (!VerifyCAUploadJedi(printerIp, authorizationPassword, false))
                    {
                        throw new WebException("Certificate delete verification failed");
                    }
                }
                else
                {
                    throw new WebException("Certificate delete failed");
                }
            }
            catch (WebException ex)
            {
                throw new WebException("Certificate delete failed", ex);
            }
        }

        /// <summary>
        /// Retrieves certificate upload status
        /// </summary>
        /// <param name="printerIp"></param>
        /// <param name="authorization"></param>
        /// <returns></returns>
        private HttpWebResult GetUploadStatus(Uri printerIp, string authorization, string file)
        {
            Uri getCertInfo;

            if (Path.GetExtension(file).Contains("pfx", StringComparison.OrdinalIgnoreCase))
            {
                getCertInfo = new Uri(string.Format(Properties.Resources.UploadStatusPfx, printerIp));
            }
            else
            {
                getCertInfo = new Uri(string.Format(Properties.Resources.UploadStatus, printerIp));
            }

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(getCertInfo);
            webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");

            if (!string.IsNullOrEmpty(authorization))
            {
                webRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
            }

            var response = HttpWebEngine.Get(webRequest, _userAgent);

            return response;
        }

        /// <summary>
        /// Deletes CA certificate from the printer
        /// </summary>
        /// <param name="printerIp"></param>
        /// <param name="authorizationPassword"></param>
        public void DeleteCAFilePhoenix(Uri printerIp, string authorizationPassword)
        {
            if (!VerifyCAUploadPhoenix(printerIp, authorizationPassword, true))
            {
                throw new InvalidOperationException("Printer does not have a CA Certificate");
            }

            string postData = "Finish=Finish";
            Uri uploadUrl = new Uri($"{printerIp}{Properties.Resources.URLPhoenixDelete}");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            if (!string.IsNullOrEmpty(authorizationPassword))
            {
                webRequest.Headers.Add(HttpRequestHeader.Authorization, authorizationPassword);
                webRequest.AllowAutoRedirect = false;
            }
            try
            {
                var postResponse = HttpWebEngine.Post(webRequest, postData, _userAgent);
                if (postResponse.StatusCode == HttpStatusCode.SeeOther)
                {
                    int startindex = postResponse.Response.IndexOf("A HREF=\"", StringComparison.OrdinalIgnoreCase) + 8;
                    int stopindex = postResponse.Response.IndexOf("\">Moved</A>", StringComparison.OrdinalIgnoreCase);
                    string redirectUrl = postResponse.Response.Substring(startindex, stopindex - startindex);

                    HttpWebRequest redirectRequest = (HttpWebRequest)WebRequest.Create(new Uri($"{printerIp}{redirectUrl.TrimStart('/')}"));
                    redirectRequest.Headers.Add(HttpRequestHeader.Authorization, authorizationPassword);
                    redirectRequest.Accept = "text/html, application/xhtml+xml, */*";
                    redirectRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                    postResponse = HttpWebEngine.Get(redirectRequest, _userAgent);
                }

                if (postResponse.StatusCode == HttpStatusCode.OK)
                {
                    if (!VerifyCAUploadPhoenix(printerIp, authorizationPassword, false))
                    {
                        throw new WebException("Certificate delete verification failed");
                    }
                }
                else
                {
                    throw new WebException("Certificate delete failed");
                }
            }
            catch (WebException ex)
            {
                throw new WebException("Certificate delete failed", ex);
            }
        }

        /// <summary>
        /// Deletes CA certificate from inkjet printer
        /// </summary>
        /// <param name="printerIp"></param>
        /// <param name="authorization"></param>
        /// <param name="certificatePath"></param>
        public void DeleteCAFileInk(Uri printerIp, string authorization, string certificatePath)
        {
            //retrieve Url for deleting certificate
            string resourceUrl = GetCertificateUrl(printerIp, authorization, certificatePath);

            if (!string.IsNullOrEmpty(resourceUrl))
            {
                Uri delCertUrl = new Uri($"{ (object)printerIp}{ (object)resourceUrl.TrimStart('/')}");
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(delCertUrl);
                webRequest.Accept = "application/xml, text/xml, */*";
                webRequest.Referer = $"{printerIp}#hId-pgCertificates";
                webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                webRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                webRequest.ContentLength = 0;
                if (!string.IsNullOrEmpty(authorization))
                {
                    webRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
                }

                var response = HttpWebEngine.Delete(webRequest, _userAgent);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ExecutionServices.SystemTrace.LogDebug("Failed to delete certificate");
                    throw new WebException("Certificate deletion failed");
                }
                else
                {
                    ExecutionServices.SystemTrace.LogDebug("Deleted certificate");
                }
            }
            else
            {
                throw new WebException("Certificate does not exist on Printer");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="printerIp"></param>
        /// <param name="authorization"></param>
        /// <param name="cookieCollection"></param>
        /// <returns></returns>
        private string GetCertificateUrl(string printerIp, string authorization, out CookieCollection cookieCollection)
        {
            Uri getCertInfo = new Uri($"{printerIp}/Security/CACertificate/Info");

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(getCertInfo);
            webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            if (!string.IsNullOrEmpty(authorization))
            {
                webRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
            }

            cookieCollection = new CookieCollection();
            var response = HttpWebEngine.Get(webRequest, _userAgent);

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(response.Response);

            return xDoc.DocumentElement.GetElementsByTagName("cert:ResourceURL").Item(0).InnerText;
        }

        /// <summary>
        /// Checks if the certificate is already installed and retrieves the Url
        /// </summary>
        /// <param name="printerIp"></param>
        /// <param name="authorization"></param>
        /// <param name="certificatePath"></param>
        /// <returns></returns>
        private string GetCertificateUrl(Uri printerIp, string authorization, string certificatePath)
        {
            if (Path.GetExtension(certificatePath).Contains("cer", StringComparison.OrdinalIgnoreCase))
            {
                X509Certificate2 certificateOptions = new X509Certificate2(X509Certificate2.CreateFromCertFile(certificatePath));
                string certificateSerialNumber = certificateOptions.SerialNumber;

                Uri certificateUrl = new Uri($"{printerIp}Security/CACertificate/Info");

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(certificateUrl);
                webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                if (!string.IsNullOrEmpty(authorization))
                {
                    webRequest.Headers.Add(HttpRequestHeader.Authorization, authorization);
                }

                var response = HttpWebEngine.Get(webRequest, _userAgent);

                if (string.IsNullOrEmpty(response.Response))
                    return null;

                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(response.Response);

                int count = xDoc.DocumentElement.GetElementsByTagName("cert:SerialNumber").Count;
                for (int index = 0; index < count; index++)
                {
                    string text = xDoc.DocumentElement.GetElementsByTagName("cert:SerialNumber").Item(index).InnerText;
                    string printerSerialNumber = Regex.Replace(text.Trim().ToLower(CultureInfo.InvariantCulture), ":", "");
                    if (certificateSerialNumber.Equals(printerSerialNumber)) //EqualsIgnoreCase
                    {
                        return xDoc.DocumentElement.GetElementsByTagName("cert:ResourceURL").Item(index).InnerText;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Verifies if the certificate is installed/deleted for TPS printers
        /// </summary>
        /// <param name="printerIp"></param>
        /// <param name="authorization"></param>
        /// <param name="install">true if installed, false if deleted</param>
        /// <returns></returns>
        private bool VerifyCAUploadPhoenix(Uri printerIp, string authorization, bool install)
        {
            bool result = false;

            Uri certificateUrl = new Uri($"{printerIp}{Properties.Resources.URLPhoenixVerify}");

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(certificateUrl);
            if (!string.IsNullOrEmpty(authorization))
            {
                req.Headers.Add(HttpRequestHeader.Authorization, authorization);
            }

            // grab the response
            var verifyCertificate = HttpWebEngine.Get(req, _userAgent);
            if (verifyCertificate.StatusCode == HttpStatusCode.OK)
            {
                string source = verifyCertificate.Response;
                if (install)
                {
                    result = (source.Contains("class=\"buttonTxtSize\"") && source.Contains("name=\"ViewCACert\""));
                }
                else
                {
                    result = (source.Contains("class=\"buttonTxtSizeDisabled\"") && source.Contains("name=\"ViewCACert2\""));
                }
            }

            return result;
        }

        /// <summary>
        /// Verifies if the certificate is installed for inkjet printers
        /// </summary>
        /// <param name="printerIp"></param>
        private void VerifyCAUploadInk(Uri printerIp)
        {
            Uri certificateUrl = new Uri($"{printerIp}{Properties.Resources.URLInkVerify}");
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(certificateUrl);

            var verifyCertificate = HttpWebEngine.Get(req, _userAgent);
            if (verifyCertificate.StatusCode == HttpStatusCode.NoContent)
            {
                throw new WebException("Certificate upload verification failed");
            }

            ExecutionServices.SystemTrace.LogDebug("Certificate upload verified");
        }

        /// <summary>
        /// Installs CA certificate on the Client VM
        /// </summary>
        /// <param name="certificate"></param>
        public static void InstallVMCertificate(string certificate)
        {
            try
            {
                X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                store.Add(new X509Certificate2(X509Certificate2.CreateFromCertFile(certificate)));

                ExecutionServices.SystemTrace.LogDebug($"Added to Root: {certificate}");
                store.Close();
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("Failed to Insall VM Certificate", ex.Message);
            }
            catch (ArgumentException argumentException)
            {
                throw new ArgumentException("Failed to Install VM Certificate", argumentException.Message);
            }
        }

        private static string GetHostName(string address)
        {
            Snmp snmp = new Snmp(address);
            string oIDhostname = "1.3.6.1.2.1.1.5.0";
            return snmp.Get(oIDhostname);
        }
    }
}