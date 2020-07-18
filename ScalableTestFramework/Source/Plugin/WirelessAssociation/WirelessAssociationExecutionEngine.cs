using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.WirelessAssociation
{
    public class WirelessAssociationExecutionEngine : IPluginExecutionEngine
    {
        private WirelessAssociationActivityData _activityData;

        private string _ssid = string.Empty;
        private string _passPhrase = string.Empty;
        private string _jediProfileString;
        private string _siriusTriptaneProfileString = null;

        private HP.DeviceAutomation.IDevice _device;
        private string _sessionId;
        private HttpClientHandler _httpClientHandler;

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _activityData = executionData.GetMetadata<WirelessAssociationActivityData>();

            PrintDeviceInfo printDeviceInfo = (PrintDeviceInfo)executionData.Assets.First();
            _device = DeviceConstructor.Create(printDeviceInfo);
            _httpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = false,
                MaxAutomaticRedirections = 2
            };
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                _ssid = WirelessAssociationActivityData.StringToHexConversion(_activityData.Ssid);
                _passPhrase = WirelessAssociationActivityData.StringToHexConversion(_activityData.Passphrase);
                switch (_activityData.AuthenticationType)
                {
                    case AuthenticationMode.WPAAES:
                        {
                            _jediProfileString = string.Format(Properties.Resources.VEPWPA2AESProfilesValue, _activityData.Ssid, _activityData.Passphrase);
                        }
                        break;

                    case AuthenticationMode.WPAHex:
                        {
                            _jediProfileString = string.Format(Properties.Resources.VEPWPA2HexProfilesValue, _activityData.Ssid, _passPhrase);
                        }
                        break;

                    case AuthenticationMode.WPAAuto:
                        {
                            _jediProfileString = string.Format(Properties.Resources.VEPWPA2AutoProfilesValue, _activityData.Ssid, _activityData.Passphrase);
                        }
                        break;

                    case AuthenticationMode.AutoAES:
                        {
                            _siriusTriptaneProfileString = string.Format(Properties.Resources.TriptaneAutoAESProfilesValue, _ssid, _passPhrase);
                            _jediProfileString = string.Format(Properties.Resources.VEPAutoAESProfilesValue, _activityData.Ssid, _activityData.Passphrase);
                        }
                        break;
                }
                PersonalWirelessSetting();
            }
            catch (Exception exception)
            {
                return new PluginExecutionResult(PluginResult.Failed, $"Failed for {_device.Address} with exception:{exception.Message}");
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        /// <summary>
        /// function to set Jedi,Phoenix,Sirius wireless settings
        /// </summary>
        private void PersonalWirelessSetting()
        {
            if (_device is JediWindjammerDevice)
            {
                VEPWPA2Config(_jediProfileString);
            }
            else if (_device is SiriusUIv3Device)
            {
                InkJetWPA2Config(_siriusTriptaneProfileString, Properties.Resources.InkjetPostDataValue);
            }
            else if (_device is JediOmniDevice)
            {
                OmniWpaConfig();
            }
            else
            {
                throw new Exception($"Device {_device.Address} is unsupported");
            }
        }

        private void OmniWpaConfig()
        {
            SignIn(_device.Address, _device.AdminPassword);
            var csrfToken = GetCsrfToken($"https://{_device.Address}/dot11_config.htm", ref _sessionId);
            
            var httpClient = new HttpClient(_httpClientHandler);
            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
            httpClient.DefaultRequestHeaders.Add("Cookie", $"sessionId={_sessionId}");

            Dictionary<string, string> formParameters =
                new Dictionary<string, string>
                {
                    {"dot11_Radio", "MODE_ENABLE"},
                    {"dot11_radiomode", "fre2pt4GHZand5GHZ" },
                    {"dot11_netnameSSID", "MODE_SELECT"},
                    {"dot11ssidlist", _activityData.Ssid},
                    {"dot11_netname","" },
                    {"dot11_auth_value", "AUTH_WPA" }
                };
            switch (_activityData.AuthenticationType)
            {
                case AuthenticationMode.WPAAES:
                    {
                        formParameters.Add("dot11_wpa_auth", "Wpa2");
                        formParameters.Add("dot11_wpa_encry", "AES");
                        formParameters.Add("dot11_wpa", "WPA_PERSONAL");
                    }
                    break;

                case AuthenticationMode.WPAAuto:
                    {
                        formParameters.Add("dot11_wpa_auth", "Wpa2");
                        formParameters.Add("dot11_wpa_encry", "Auto");
                        formParameters.Add("dot11_wpa", "Auto");
                    }
                    break;

                case AuthenticationMode.AutoAES:
                    {
                        formParameters.Add("dot11_wpa_auth", "Auto");
                        formParameters.Add("dot11_wpa_encry", "AES");
                        formParameters.Add("dot11_wpa", "Auto");
                    }
                    break;

                default:
                    {
                        formParameters.Add("dot11_wpa_auth", "Auto");
                        formParameters.Add("dot11_wpa_encry", "Auto");
                        formParameters.Add("dot11_wpa", "Auto");
                    }
                    break;
            }

            formParameters.Add("dot11_wpa_pass", _activityData.Passphrase);
            formParameters.Add("dot1x_authReAuth", "on");
            formParameters.Add("Apply", "Apply");
            formParameters.Add("CSRFToken", csrfToken);

            var formdata = new FormUrlEncodedContent(formParameters);
            var wifiResult = httpClient.PostAsync($"https://{_device.Address}/dot11_config.htm/config", formdata);
            try
            {
                wifiResult.Result.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                throw new Exception($" Unable to associate the wireless device to the SSID. {e.Message}");
            }
           
        }

        private void SignIn(string address, string password)
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

            var signinResponse = (HttpWebResponse)signinRequest.GetResponse();

            if (signinResponse.StatusCode == HttpStatusCode.OK || signinResponse.StatusCode == HttpStatusCode.Moved)
            {
                signinResponse.Close();
                _sessionId = signinRequest.Headers["Cookie"];
                int index = _sessionId.IndexOf(";", StringComparison.Ordinal);
                if (index != -1)
                    _sessionId = _sessionId.Substring(0, index);

                _sessionId = _sessionId.Replace("sessionId=", string.Empty);
            }
            else
            {
                throw new Exception($"Couldn't login to the device {address} with the password: {password}");
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
                            int startIndex = responseBodyString.IndexOf("name=\"CSRFToken\" value=\"",
                                StringComparison.OrdinalIgnoreCase);
                            if (startIndex != -1)
                            {
                                responseBodyString = responseBodyString.Substring(startIndex);
                                int endIndex = responseBodyString.IndexOf("\" />", StringComparison.OrdinalIgnoreCase);
                                //for JDI the boundaries are different, so let's check again
                                if (endIndex == -1)
                                {
                                    endIndex = responseBodyString.IndexOf("\">", StringComparison.OrdinalIgnoreCase);
                                    if (endIndex == -1) //JDI sometimes returns empty CSRF Token
                                        csrfToken = string.Empty;
                                    else
                                        csrfToken = responseBodyString.Substring("name=\"CSRFToken\" value=\"".Length,
                                            endIndex - "name=\"CSRFToken\" value=\"".Length);
                                }
                                else
                                {
                                    csrfToken = responseBodyString.Substring("name=\"CSRFToken\" value=\"".Length,
                                        endIndex - "name=\"CSRFToken\" value=\"".Length);
                                }
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
        /// VEP WPA2 config
        /// </summary>
        /// <param name="profilesValue"></param>
        private void VEPWPA2Config(string profilesValue)
        {
            string cookiesValue = string.Empty;

            ExecutionServices.SystemTrace.LogDebug("Profiles Value  : " + profilesValue);

            Uri url = new Uri($"https://{_device.Address}/dot11_config.htm");
            HttpWebResult result = PrepareVEPWebRequest(url, HttpVerb.GET, cookiesValue);
            ExecutionServices.SystemTrace.LogDebug($"Navigating to URL https://{_device.Address}/dot11_config.htm Result:{result.StatusCode}");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                // cookiesValue = result.Headers.GetValues("Cookie").ToString();
                //TraceFactory.Logger.Info("Cookies Value: " + cookiesValue);

                url = new Uri($"https://{_device.Address}/dot11_config.htm/config");
                result = PrepareVEPWebRequest(url, HttpVerb.POST, cookiesValue, profilesValue);
                ExecutionServices.SystemTrace.LogDebug($"Navigating to URL https://{_device.Address}/dot11_config.htm/config Result:{result.StatusCode}");

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    url = new Uri($"https://{_device.Address}/success_result.htm/config");
                    result = PrepareVEPWebRequest(url, HttpVerb.POST, cookiesValue, Properties.Resources.VEPWPA2PostDataValue);
                    ExecutionServices.SystemTrace.LogDebug($"Navigating to URL https://{_device.Address}/success_result.htm/config Result:{result.StatusCode}");

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        if (_activityData.PowerCycleRequired)
                        {
                            PowerCyclePrinter();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Performs Power Cycle of the Device
        /// </summary>
        private void PowerCyclePrinter()
        {
            int telnetPort = 223;

            TelnetLibrary.EnableTelnetFeature();

            using (TelnetLibrary telnetConnection = new TelnetLibrary(_device.Address, telnetPort))
            {
                ExecutionServices.SystemTrace.LogDebug("Telnet Status:" + telnetConnection.Read());
                if (telnetConnection.IsConnected)
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromMinutes(1));
                    ExecutionServices.SystemTrace.LogDebug("Power Cycle Started");
                    telnetConnection.WriteLine("reboot");
                    ExecutionServices.SystemTrace.LogDebug("Telnet Status:" + telnetConnection.Read());
                    Retry.UntilTrue(() => HasDeviceRebooted(_device), 10, TimeSpan.FromSeconds(5));
                    Retry.UntilTrue(IsPrinterAwake, 20, TimeSpan.FromSeconds(10));
                }
                else
                {
                    ExecutionServices.SystemTrace.LogDebug("Error while connecting through Telnet.");
                }
            }
        }

        private static bool HasDeviceRebooted(IDevice device)
        {
            var status = device.GetDeviceStatus();
            Application.DoEvents();

            return (status == DeviceStatus.None || status == DeviceStatus.Unknown);
        }

        /// <summary>
        /// Checks whether Printer is in ready state
        /// </summary>
        private bool IsPrinterAwake()
        {
            if (!string.IsNullOrEmpty(_device.Address))
            {
                var navigateUrl = new Uri($"https://{_device.Address}");
                var webResult = PrepareVEPWebRequest(navigateUrl, HttpVerb.GET, null);
                ExecutionServices.SystemTrace.LogDebug($"Navigating to URL https://{navigateUrl}/ Status Code:{webResult.StatusCode}");
                if (webResult.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Inkjet WPA2 config
        /// </summary>
        /// <param name="profilesValue"></param>
        /// <param name="postDataValue"></param>
        private void InkJetWPA2Config(string profilesValue, string postDataValue)
        {
            string cookiesValue = string.Empty;

            try
            {
                if (profilesValue != null)
                {
                    ExecutionServices.SystemTrace.LogDebug("Profiles Value  : " + profilesValue);

                    Uri url = new Uri($"https://{ (object)_device.Address}");

                    HttpWebResult result = PrepareInkWebRequest(_device, url, HttpVerb.GET, cookiesValue, postDataValue);

                    ExecutionServices.SystemTrace.LogDebug($"Navigating to URL https://{ (object)_device.Address} Result:{ (object)result.StatusCode}");
                    if (result.StatusCode != 0)
                    {
                        cookiesValue = result.Headers.GetValues("Set-Cookie").FirstOrDefault().Split(';').FirstOrDefault();
                        ExecutionServices.SystemTrace.LogDebug("Cookies Value: " + cookiesValue);

                        url = new Uri($"https://{ (object)_device.Address}/{ (object)"IoMgmt/Adapters/Wifi0/Profiles/1"}");

                        result = PrepareInkWebRequest(_device, url, HttpVerb.PUT, cookiesValue, profilesValue);
                        ExecutionServices.SystemTrace.LogDebug($"Navigating to URL https://{ (object)_device.Address}/{ (object)"IoMgmt/Adapters/Wifi0/Profiles/1"}  Result:{ (object)result.StatusCode}");
                        if (result.StatusCode != 0)
                        {
                            url = new Uri($"https://{ (object)_device.Address}/{ (object)"IoMgmt/Adapters/Wifi0"}");
                            result = PrepareInkWebRequest(_device, url, HttpVerb.PUT, cookiesValue, postDataValue);
                            ExecutionServices.SystemTrace.LogDebug($"Navigating to URL https://{_device.Address}/{"IoMgmt/Adapters/Wifi0"}  Result:{result.StatusCode}");
                        }
                    }
                    else
                    {
                        ExecutionServices.SystemTrace.LogDebug("Time out exception occurred. Please re-run it.");
                    }
                }
                else
                {
                    ExecutionServices.SystemTrace.LogDebug("This feature is not supported in this device.");
                }
            }
            catch (HttpListenerException exception)
            {
                ExecutionServices.SystemTrace.LogDebug(exception.Message);
            }
        }

        /// <summary>
        /// HTTP Web Request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webMethod">webMethod</param>
        /// <param name="cookies"></param>
        /// <param name="postDataValue"></param>
        /// <returns></returns>
        private static HttpWebResult PrepareInkWebRequest(HP.DeviceAutomation.IDevice device, Uri url, HttpVerb webMethod, string cookies, string postDataValue)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Accept = "*/*";
            if (device is SiriusUIv3Device)
            {
                webRequest.Headers.Add("Accept-Language", "en-US");
                webRequest.ContentType = "text/xml";
            }
            else
            {
                webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.5");
                webRequest.ContentType = "text/xml; charset=UTF-8";
            }
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            if (!string.IsNullOrEmpty(cookies))
            {
                webRequest.Headers.Add("Cookie", cookies);
            }

            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.Method = webMethod.ToString();
            if (webMethod == HttpVerb.GET)
            {
                return HttpWebEngine.Get(webRequest);
            }
            if (webMethod == HttpVerb.PUT)
            {
                return HttpWebEngine.Put(webRequest, postDataValue);
            }
            return null;
        }

        /// <summary>
        /// VEP HTTP Web Request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webMethod">webMethod</param>
        /// <param name="cookies"></param>
        /// <param name="profilesValue"></param>
        /// <returns></returns>
        private static HttpWebResult PrepareVEPWebRequest(Uri url, HttpVerb webMethod, string cookies, string profilesValue = null)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            if (!string.IsNullOrEmpty(cookies))
            {
                webRequest.Headers.Add("Cookie", cookies);
            }
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = webMethod.ToString();
            if (webMethod == HttpVerb.GET)
            {
                return HttpWebEngine.Get(webRequest);
            }
            if (webMethod == HttpVerb.POST)
            {
                return HttpWebEngine.Post(webRequest, profilesValue);
            }
            return null;
        }

        private enum HttpVerb
        {
            GET,
            PUT,
            POST
        }
    }
}