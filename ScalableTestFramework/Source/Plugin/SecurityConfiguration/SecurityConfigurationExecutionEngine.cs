using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using HtmlAgilityPack;

namespace HP.ScalableTest.Plugin.SecurityConfiguration
{
    public partial class SecurityConfigurationExecutionEngine : IPluginExecutionEngine
    {
        private bool _setupDone = false;
        private SecurityConfigurationActivityData _activityData;
        private string _strAuthorization;
        private readonly Dictionary<string, string> _pairedValues = new Dictionary<string, string>();
        private bool _authorizationRequired;
        private CookieCollection _cookies;
        private HttpWebRequest _webRequest;
        private Uri _configUrl;
        private HttpWebResult _result;
        private const string UserName = "admin";
        private readonly List<SecurityConfigurationSetting> _optionsList;

        public SecurityConfigurationExecutionEngine()
        {
            InitializeComponent();

            //Parse XML Data to List of Objects

            XDocument xdoc = XDocument.Parse(Properties.Resources.SecurityConfig);
            var xElement = xdoc.Element("SecurityOption");
            if (xElement != null)
                _optionsList = xElement.Elements("Data").Select(d =>
                    new SecurityConfigurationSetting()
                    {
                        Name = d.Element("Name").Value,
                        Key = d.Element("Key").Value,
                        Value = d.Element("Value").Value,
                    }).ToList();
        }

        /// <summary>
        /// Execution
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            if (!_setupDone)
            {
                TelnetLibrary.EnableTelnetFeature();
                _setupDone = true;
            }

            _activityData = executionData.GetMetadata<SecurityConfigurationActivityData>();

            var printer = (PrintDeviceInfo)executionData.Assets.First();

            try
            {
                _strAuthorization = ConvertToBase64Encoding($"admin:{ (object)printer.AdminPassword}");

                switch (_activityData.SecurityType)
                {
                    case SecurityConfigurationType.Basic:
                        {
                            BasicEnhancedSecurityConfig(printer, SecurityConfigurationType.Basic);
                        }
                        break;

                    case SecurityConfigurationType.Enhanced:
                        {
                            BasicEnhancedSecurityConfig(printer, SecurityConfigurationType.Enhanced);
                        }
                        break;

                    case SecurityConfigurationType.Custom:
                        {
                            CustomSecurityConfiguration(printer, SecurityConfigurationType.Custom);
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                return new PluginExecutionResult(PluginResult.Failed, $"Failed for {printer.Address}: {exception.Message}");
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        /// <summary>
        /// Setting Basic/Enhanced Security Settings
        /// </summary>
        /// <param name="printer">printer Info</param>
        /// <param name="securityType"> Security Configuration Type</param>
        private void BasicEnhancedSecurityConfig(PrintDeviceInfo printer, SecurityConfigurationType securityType)
        {
            _cookies = new CookieCollection();
            Uri baseUri = new Uri($"https://{printer.Address}");
            if (!string.IsNullOrEmpty(printer.AdminPassword))
            {
                _authorizationRequired = true;
                Uri url = new Uri(baseUri, "hp/device/SignIn/Index");
                _webRequest = HttpWebRequestValidate(url);
                _webRequest.CookieContainer = new CookieContainer();
                _webRequest.CookieContainer.Add(_cookies);
                _webRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic " + _strAuthorization);
                _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("SignInIndexConfig"), UserName));
            }
            else
            {
                _configUrl = new Uri(baseUri, "welcome.html");
                _webRequest = PrepareHttpWebRequest(_configUrl);
                _result = HttpWebEngine.Get(_webRequest);
                if (_result.StatusCode == 0)
                {
                    _authorizationRequired = true;
                    _webRequest = PrepareHttpWebRequest(_configUrl);
                    _result = HttpWebEngine.Get(_webRequest);
                }
            }

            _cookies = _result.Cookies;
            _configUrl = new Uri(baseUri, "welcome.html/config");

            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, GetKey("WelcomeConfig"));

            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            int startindex = _result.Response.IndexOf("Hide\" VALUE=\"", StringComparison.OrdinalIgnoreCase) + 13;
            string truncatedResponse = _result.Response.Substring(startindex);
            int endIndex = truncatedResponse.IndexOf("\">", StringComparison.OrdinalIgnoreCase);
            string hideValue = truncatedResponse.Substring(0, endIndex);

            _configUrl = new Uri(baseUri, "level.html/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("LevelConfig"), securityType, hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            _configUrl = new Uri(baseUri, "password.html/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("PasswordConfig"), printer.AdminPassword, hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            if (securityType == SecurityConfigurationType.Enhanced)
            {
                _configUrl = new Uri(baseUri, "snmp.html/config");
                _webRequest = PrepareHttpWebRequest(_configUrl);
                _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("SNMPConfig"), _activityData.SnmpV3Enhanced, hideValue));
                ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{ (object)_configUrl.AbsoluteUri} Status:{ (object)_result.StatusCode}");

                _configUrl = new Uri(baseUri, "snmpv3_creds.html/config");
                _webRequest = PrepareHttpWebRequest(_configUrl);
                _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("SNMPV3Config"), _activityData.Snmpv3UserName, _activityData.AuthenticationPassphraseProtocol, _activityData.PrivacyPassphraseProtocol, hideValue));
                ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{ (object)_configUrl.AbsoluteUri} Status:{ (object)_result.StatusCode}");

                _configUrl = new Uri(baseUri, "snmp_legacy.html/config");
                _webRequest = PrepareHttpWebRequest(_configUrl);
                _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("SNMPLegacy"), _activityData.Snmpv1v2ReadOnlyAccess, hideValue));
                ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");
            }

            _configUrl = new Uri(baseUri, "review.html/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("ReviewConfig"), hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            PowerCyclePrinter(printer.Address);

            ValidateEncryption(baseUri, securityType);
        }

        /// <summary>
        /// Setting Custom Security Settings
        /// </summary>
        /// <param name="printer">printer Info</param>
        /// <param name="securityType"> Security Configuration Type</param>
        private void CustomSecurityConfiguration(PrintDeviceInfo printer, SecurityConfigurationType securityType)
        {
            _cookies = new CookieCollection();
            Uri baseUri = new Uri($"https://{printer.Address}");
            if (!string.IsNullOrEmpty(printer.AdminPassword))
            {
                _authorizationRequired = true;
                Uri url = new Uri(baseUri, "hp/device/SignIn/Index");
                _webRequest = HttpWebRequestValidate(url);
                _webRequest.CookieContainer = new CookieContainer();
                _webRequest.CookieContainer.Add(_cookies);
                _webRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic " + _strAuthorization);
                _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("SignInIndexConfig"), UserName));
            }
            else
            {
                _configUrl = new Uri(baseUri, "welcome.html");

                _webRequest = PrepareHttpWebRequest(_configUrl);
                _result = HttpWebEngine.Get(_webRequest);
                if (_result.StatusCode == 0)
                {
                    _authorizationRequired = true;
                    _webRequest = PrepareHttpWebRequest(_configUrl);
                    _result = HttpWebEngine.Get(_webRequest);
                }
            }
            _cookies = _result.Cookies;

            _configUrl = new Uri(baseUri, "welcome.html/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, GetKey("WelcomeConfig"));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            int startindex = _result.Response.IndexOf("Hide\" VALUE=\"", StringComparison.OrdinalIgnoreCase) + 13;
            string truncatedResponse = _result.Response.Substring(startindex);
            int endIndex = truncatedResponse.IndexOf("\">", StringComparison.OrdinalIgnoreCase);
            string hideValue = truncatedResponse.Substring(0, endIndex);

            _configUrl = new Uri(baseUri, "level.html/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("LevelConfig"), securityType, hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            _configUrl = new Uri(baseUri, "password.html/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("PasswordConfig"), printer.AdminPassword, hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            _configUrl = new Uri(baseUri, "websecurity/http_mgmt.html/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("WebSecurityConfig"), _activityData.EncryptionStrength.ToLower(CultureInfo.InvariantCulture), hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            _configUrl = new Uri(baseUri, "mgmt_protocols.html/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("MgmtConfig"), hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            _configUrl = new Uri(baseUri, "snmp.html/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("SNMPCustomConfig"), hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            _configUrl = new Uri(baseUri, "snmpv1v2_creds.html/config");
            _webRequest = (HttpWebRequest)WebRequest.Create(_configUrl);
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("SNMPV1V2Config"), hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            _configUrl = new Uri(baseUri, "acl.html/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("ACLConfig"), _activityData.AccessControlIpv4, _activityData.Mask, hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            _configUrl = new Uri(baseUri, "protocols.html/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("ProtocolConfig"), hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            int startIndex = _result.Response.IndexOf("SuserID\">", StringComparison.OrdinalIgnoreCase) + 9;
            string tempResponse = _result.Response.Substring(startIndex);
            endIndex = tempResponse.IndexOf(" / ", StringComparison.OrdinalIgnoreCase);
            tempResponse = tempResponse.Substring(0, endIndex);

            _configUrl = new Uri(baseUri, "dot1x_config.htm/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("Dot1xConfig"), tempResponse, _activityData.AuthenticationPassword, _activityData.AuthenticationPassword, hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            _configUrl = new Uri(baseUri, "review.html/config");
            _webRequest = PrepareHttpWebRequest(_configUrl);
            _result = HttpWebEngine.Post(_webRequest, string.Format(GetKey("ReviewConfig"), hideValue));
            ExecutionServices.SystemTrace.LogDebug($"Navigating to Url:{_configUrl.AbsoluteUri} Status:{_result.StatusCode}");

            PowerCyclePrinter(printer.Address);

            ValidateEncryption(baseUri, securityType);
        }

        /// <summary>
        /// Performs Power Cycle of the Device
        /// </summary>
        /// <param name="address">Address</param>
        private void PowerCyclePrinter(string address)
        {
            const int telnetPort = 223;



            using (TelnetLibrary telnetConnection = new TelnetLibrary(address, telnetPort))
            {
                ExecutionServices.SystemTrace.LogDebug("Telnet Status:" + telnetConnection.Read());
                if (telnetConnection.IsConnected)
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromMinutes(1));
                    ExecutionServices.SystemTrace.LogDebug("Power Cycle Started");
                    telnetConnection.WriteLine("reboot");
                    ExecutionServices.SystemTrace.LogDebug("Telnet Status:" + telnetConnection.Read());
                    System.Threading.Thread.Sleep(TimeSpan.FromMinutes(5));
                    Retry.UntilTrue(() => IsPrinterAwake(address), 10, TimeSpan.FromMinutes(1));
                }
                else
                {
                    ExecutionServices.SystemTrace.LogDebug("Error while connecting through Telnet.");
                }
            }
        }

        /// <summary>
        /// Checks whether Printer is in ready state
        /// </summary>
        /// <param name="address">address</param>
        private bool IsPrinterAwake(string address)
        {
            if (!string.IsNullOrEmpty(address))
            {
                var navigateUrl = new Uri($"https://{ (object)address}{ (object)"/hp/device/SignIn/Index"}");
                var webRequest = HttpWebRequestValidate(navigateUrl);
                webRequest.CookieContainer = new CookieContainer();
                webRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic " + _strAuthorization);
                HttpWebEngine.Post(webRequest, string.Format(GetKey("SignInIndexConfig"), UserName));

                navigateUrl = new Uri($"https://{address}");
                webRequest = HttpWebRequestValidate(navigateUrl);
                var webResult = HttpWebEngine.Get(webRequest);
                ExecutionServices.SystemTrace.LogDebug("Status Code: " + webResult.StatusCode);
                if (webResult.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check the values from dictionary
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="configurationType">Type</param>
        private void CheckDictionary(string name, SecurityConfigurationType configurationType)
        {
            string result;

            var searchList = _optionsList.First(x => !x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            var key = searchList.Key;
            var value = searchList.Value;

            if (_pairedValues.TryGetValue(key, out result))
            {
                if (!result.Equals(value))
                {
                    throw new Exception($"Validation failed for { (object)key}, Value: { (object)value}");
                }
                ExecutionServices.SystemTrace.LogDebug($"Validated { (object)key} with value:{ (object)result} in { (object)configurationType} setting");
            }
            else
            {
                throw new Exception($"Validation failed for {name}");
            }
        }

        /// <summary>
        /// get key value from xml dictionary
        /// </summary>
        /// <param name="name"></param>
        private string GetKey(string name)
        {
            return _optionsList.First(searchList => searchList.Name.EqualsIgnoreCase(name)).Value;
        }

        /// <summary>
        /// Validates Basic Encryption
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="securityType">Type</param>
        private void ValidateEncryption(Uri url, SecurityConfigurationType securityType)
        {
            char[] delimiters = new char[] { ';' };
            string[] configurations = new string[] { };

            NavigateSettingsConfiguration(url);

            if (securityType == SecurityConfigurationType.Basic)
            {
                configurations = Properties.Resources.BasicValidation.Split(delimiters,
                            StringSplitOptions.RemoveEmptyEntries);
            }
            if (securityType == SecurityConfigurationType.Enhanced)
            {
                configurations = Properties.Resources.EnhancedValidation.Split(delimiters,
                            StringSplitOptions.RemoveEmptyEntries);
            }
            if (securityType == SecurityConfigurationType.Custom)
            {
                configurations = Properties.Resources.CustomValidation.Split(delimiters,
                            StringSplitOptions.RemoveEmptyEntries);
            }

            foreach (var configuration in configurations)
            {
                CheckDictionary(configuration, securityType);
            }
        }

        /// <summary>
        /// Navigate to Settings Configuration Page to fetch values
        /// </summary>
        /// <param name="baseUrl"></param>
        private void NavigateSettingsConfiguration(Uri baseUrl)
        {
            _cookies = new CookieCollection();

            Uri targetUrl = new Uri($"{baseUrl}{"hp/device/SignIn/Index"}");
            var webRequest = HttpWebRequestValidate(targetUrl);
            webRequest.CookieContainer = new CookieContainer();
            webRequest.CookieContainer.Add(_cookies);
            HttpWebEngine.Post(webRequest, string.Format(GetKey("SignInIndexConfig"), UserName));

            string authorization = ConvertToBase64Encoding("admin:public");

            targetUrl = new Uri($"{baseUrl}{"hp/jetdirect"}");
            webRequest = HttpWebRequestValidate(targetUrl);
            webRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authorization);
            webRequest.CookieContainer = new CookieContainer();
            webRequest.CookieContainer.Add(_cookies);
            HttpWebEngine.Get(webRequest);

            targetUrl = new Uri($"{baseUrl}{"security_status.html"}");
            webRequest = HttpWebRequestValidate(targetUrl);
            webRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authorization);
            webRequest.CookieContainer = new CookieContainer();
            webRequest.CookieContainer.Add(_cookies);
            var webResult = HttpWebEngine.Get(webRequest);

            HtmlDocument htmlDoc = new HtmlDocument
            {
                OptionFixNestedTags = true,
                OptionOutputAsXml = true
            };

            // There are various options, set as needed

            string cleanedHtmlResult = webResult.Response.Replace("&nbsp", "").Trim();
            cleanedHtmlResult = cleanedHtmlResult.Replace(Environment.NewLine, string.Empty).Replace("\n", "").Trim();
            cleanedHtmlResult = cleanedHtmlResult.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?> ", "");
            cleanedHtmlResult = cleanedHtmlResult.Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\"> ", "");

            // Get all tables in the document
            htmlDoc.LoadHtml(cleanedHtmlResult.Trim());
            // Get all tables in the document

            HtmlNodeCollection tdNodes = htmlDoc.DocumentNode.SelectNodes("//td[@class='noWrapPadRt']");
            int count = 1;
            foreach (var node in tdNodes)
            {
                if (!string.IsNullOrEmpty(node.InnerText))
                {
                    if (!_pairedValues.ContainsKey(node.InnerText.Trim()))
                    {
                        _pairedValues.Add(node.InnerText.Trim(), node.ParentNode.ChildNodes[3].InnerText.Trim());
                        ExecutionServices.SystemTrace.LogDebug(node.InnerText.Trim() + "->" + node.ParentNode.ChildNodes[3].InnerText.Trim());
                    }
                    else
                    {
                        _pairedValues.Add(node.InnerText.Trim() + "_" + count, node.ParentNode.ChildNodes[3].InnerText.Trim());
                        ExecutionServices.SystemTrace.LogDebug(node.InnerText.Trim() + "_" + count + "->" + node.ParentNode.ChildNodes[3].InnerText.Trim());
                        count++;
                    }
                }
            }
        }

        /// <summary>
        /// Converts string to Base 64 Encoding
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string ConvertToBase64Encoding(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// HTTP Web Request
        /// </summary>
        /// <param name="uri">webRequest</param>
        /// <returns></returns>
        private HttpWebRequest PrepareHttpWebRequest(Uri uri)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Accept = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            webRequest.Headers.Add(HttpRequestHeader.CacheControl, "no-cache; Expires: Thu, 01 Jan 1970 00:00:00 GMT");
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            webRequest.ContentType = "text/html;charset=utf-8";
            webRequest.CookieContainer = new CookieContainer();
            webRequest.CookieContainer.Add(_cookies);
            if (_authorizationRequired)
            {
                webRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic " + _strAuthorization);
            }
            return webRequest;
        }

        /// <summary>
        /// HTTP Web Request Validate
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static HttpWebRequest HttpWebRequestValidate(Uri url)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            webRequest.ContentType = "application/x-www-form-urlencoded";
            return webRequest;
        }


    }
}