using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace HP.ScalableTest.Plugin.PaperCutInstaller
{
    /// <summary>
    /// A class that implements the execution portion of the plug-in.
    /// </summary>
    /// <remarks>
    /// This class implements the <see cref="IPluginExecutionEngine"/> interface.
    ///
    /// <seealso cref="IPluginExecutionEngine"/>
    /// </remarks>
    [ToolboxItem(false)]
    public partial class PaperCutInstallerExecutionControl : UserControl, IPluginExecutionEngine
    {
        private IDevice _device; private StringBuilder _logText = new StringBuilder();

        private string _signedSessionId;
        private ServerInfo _serverInfo;
        private PrintDeviceInfo _printDeviceInfo;
        private const string AcceptString = "text/html, application/xhtml+xml, */*";
        private string _signedAppSessionId;
        private PaperCutInstallerActivityData _data;

        public HttpClientHandler Handler { get; set; }

        /// <summary>
        ///
        /// </summary>
        public PaperCutInstallerExecutionControl()
        {
            InitializeComponent();

            Handler = new HttpClientHandler() { AllowAutoRedirect = true, UseCookies = false };
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.Expect100Continue = false;
        }

        #region IPluginExecutionEngine implementation

        /// <summary>
        /// Executes this plug-in's workflow using the specified <see cref="PluginExecutionData"/>.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="PluginExecutionResult"/> indicating the outcome of the
        /// execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Skipped);
            _data = executionData.GetMetadata<PaperCutInstallerActivityData>();
            _serverInfo = executionData.Servers.FirstOrDefault();

            _printDeviceInfo = executionData.Assets.OfType<PrintDeviceInfo>().FirstOrDefault();
            _device = DeviceConstructor.Create(_printDeviceInfo);

            var bundleInstaller = new BundleInstaller(_device as JediOmniDevice);

            try
            {
                _signedSessionId = bundleInstaller.SignIn(string.Empty);

                switch (_data.Action)
                {
                    case PaperCutInstallerAction.Install:
                        {
                            result = bundleInstaller.InstallSolution(_signedSessionId, _data.BundleFile);
                        }
                        break;

                    case PaperCutInstallerAction.Register:
                        {
                            result = RegisterDevice();
                        }
                        break;

                    case PaperCutInstallerAction.ConfigureCredentials:
                    case PaperCutInstallerAction.ConfigureSettings:
                        {
                            result = ConfigureDevice();
                        }
                        break;
                }
            }
            catch (WebException wex)
            {
                _device.Dispose();
                ExecutionServices.SystemTrace.LogError(
                    $"PaperCut Installer Action {_data.Action} failed on device:{_device.Address}", wex);
                UpdateStatus($"{_printDeviceInfo.AssetId}: Failed with exception: {wex.Message}");
                return new PluginExecutionResult(PluginResult.Failed, wex.Message);
            }
            catch (Exception ex)
            {
                _device.Dispose();
                ExecutionServices.SystemTrace.LogError(
                    $"PaperCut Installer Action {_data.Action} failed on device:{_device.Address}", ex);
                UpdateStatus($"{_printDeviceInfo.AssetId}: Failed with exception: {ex.Message}");
                return new PluginExecutionResult(PluginResult.Failed, ex.Message);
            }
            _device.Dispose();
            UpdateStatus($"{_printDeviceInfo.AssetId}: {result.Result}");
            return result;
        }

        private PluginExecutionResult RegisterDevice()
        {
            Handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = false };
            using (HttpClient client = new HttpClient(Handler))
            {
                client.DefaultRequestHeaders.Add("Accept", AcceptString);
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.Add("Cookie", $"sessionId={_signedSessionId}");
                client.DefaultRequestHeaders.Add("Referer", $"https://{_device.Address}/hp/device/DisplaySettings/Index");

                var response =
                    client.GetAsync(
                        $"https://{_device.Address}/hp/device/PaperCut.Ext.Device.HP.FutureSmart.Client.Ews.ServerConfig/Index");
                if (response.Result.IsSuccessStatusCode)
                {
                    HtmlDocument hDoc = new HtmlDocument();
                    hDoc.LoadHtml(response.Result.Content.ReadAsStringAsync().Result);

                    var csrfToken = hDoc.GetElementbyId("CSRFToken").Attributes["value"].Value;

                    client.DefaultRequestHeaders.Referrer = new Uri($"https://{_device.Address}/hp/device/PaperCut.Ext.Device.HP.FutureSmart.Client.Ews.ServerConfig/Index");
                    var multipartFormData = new MultipartFormDataContent
                    {
                        {new StringContent(csrfToken), "\"CSRFToken\""},
                        {new StringContent(_serverInfo.Address), "\"server-ip\""},
                        {new StringContent(_printDeviceInfo.AssetId), "\"device-name\""},
                        {new StringContent("9191"), "\"server-http-port\""},
                        {new StringContent("9192"), "\"server-https-port\""},
                        {new StringContent("Configure"), "\"config-server\""}
                    };

                    var configureResponse =
                        client.PostAsync(
                            $"https://{_device.Address}/hp/device/PaperCut.Ext.Device.HP.FutureSmart.Client.Ews.ServerConfig/Index",
                            multipartFormData);

                    if (configureResponse.Result.IsSuccessStatusCode)
                    {
                        hDoc.LoadHtml(configureResponse.Result.Content.ReadAsStringAsync().Result);
                        var messageElement =
                            hDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'message info')]");
                        UpdateStatus(messageElement.InnerText);
                        if (messageElement.InnerText.Contains("success"))
                            return new PluginExecutionResult(PluginResult.Passed);
                        else
                        {
                            return new PluginExecutionResult(PluginResult.Failed, messageElement.InnerText);
                        }
                    }
                }
            }

            return new PluginExecutionResult(PluginResult.Failed, "Failed to configure the device with PaperCut Server");
        }

        private PluginExecutionResult SignInAdminApp()
        {
            Handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = true };
            using (HttpClient client = new HttpClient(Handler))
            {
                client.DefaultRequestHeaders.Add("Accept", AcceptString);
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.Add("DNT", "1");
                client.DefaultRequestHeaders.ExpectContinue = false;

                var appResponse = client.GetAsync($"http://{_serverInfo.Address}:9191/admin");
                if (appResponse.Result.IsSuccessStatusCode)
                {
                    _signedAppSessionId = appResponse.Result.Headers.GetValues("Set-Cookie")?.FirstOrDefault()?.Split(';').FirstOrDefault();
                    _signedAppSessionId = _signedAppSessionId?.Replace("JSESSIONID=", string.Empty);
                }

                client.DefaultRequestHeaders.Add("Cookie", $"JSESSIONID={_signedAppSessionId}");
                client.DefaultRequestHeaders.Add("Referer", $"http://{_serverInfo.Address}:9191/admin");

                var formData = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("service", "direct/1/Home/$Form$0"),
                        new KeyValuePair<string, string>("sp", "S0"),
                        new KeyValuePair<string, string>("Form0", "$Hidden$0,$Hidden$1,inputUsername,inputPassword,$PropertySelection$0,$Submit$0"),
                        new KeyValuePair<string, string>("$Hidden$0", "true"),
                        new KeyValuePair<string, string>("$Hidden$1", "X"),
                        new KeyValuePair<string, string>("inputUsername", _data.AdminUserName),
                        new KeyValuePair<string, string>("inputPassword", _data.AdminPassword),
                        new KeyValuePair<string, string>("$PropertySelection$0", "en"),
                        new KeyValuePair<string, string>("$Submit$0", "Log in")
                    });
                var message = client.PostAsync($"http://{_serverInfo.Address}:9191/app", formData);

                if (message.Result.IsSuccessStatusCode)
                {
                    _signedAppSessionId = message.Result.Headers.GetValues("Set-Cookie").FirstOrDefault()
                        ?.Split(';').FirstOrDefault();
                    _signedAppSessionId = _signedAppSessionId?.Replace("JSESSIONID=", string.Empty);

                    return new PluginExecutionResult(PluginResult.Passed);
                }
                else
                {
                    return new PluginExecutionResult(PluginResult.Failed, "Unable to sign in successfully to the admin application. Please check the credentials provided.");
                }
            }
        }

        private PluginExecutionResult ConfigureDevice()
        {
            var result = SignInAdminApp();
            if (result.Result == PluginResult.Passed)
            {
                var deviceId = GetDeviceId();
                if (string.IsNullOrEmpty(deviceId))
                    return new PluginExecutionResult(PluginResult.Failed, "Unable to find the device in device list.");

                if (_data.Action == PaperCutInstallerAction.ConfigureCredentials)
                    result = AddDeviceCredentials(deviceId);

                if (_data.Action == PaperCutInstallerAction.ConfigureSettings)
                {
                    result = ConfigureSettings(deviceId);
                    //control panel will be locked out let's enable Guest Access
                    if (result.Result == PluginResult.Passed)
                        EnableGuestAccess();
                }
            }

            return result;
        }

        private PluginExecutionResult AddDeviceCredentials(string deviceId)
        {
            Handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = false };
            using (HttpClient client = new HttpClient(Handler))
            {
                client.DefaultRequestHeaders.Add("Accept", AcceptString);
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.Add("DNT", "1");
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.Add("Cookie", $"JSESSIONID={_signedAppSessionId}");
                client.DefaultRequestHeaders.Add("Referer", $"http://{_serverInfo.Address}:9191/app?service=page/Home");

                var deviceResponse = client.GetAsync(
                    $"http://{_serverInfo.Address}:9191/app?service=direct/1/DeviceList/selectDevice&sp={deviceId}");
                if (deviceResponse.Result.IsSuccessStatusCode)
                {
                    HtmlDocument hDoc = new HtmlDocument();
                    hDoc.LoadHtml(deviceResponse.Result.Content.ReadAsStringAsync().Result);

                    var form0Node = hDoc.DocumentNode.SelectSingleNode("//input[contains(@name,'Form0')]");
                    var form0NodeValue = form0Node.Attributes["value"].Value;

                    //let's add the device credentials to the device
                    client.DefaultRequestHeaders.Referrer = new Uri(
                        $"http://{_serverInfo.Address}:9191/app?service=direct/1/DeviceList/selectDevice&sp={deviceId}");
                    var formKeyValuePairs = (new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("service", "direct/1/PrinterDetails/$Form"),
                        new KeyValuePair<string, string>("sp", "S0"),
                        new KeyValuePair<string, string>("Form0", form0NodeValue),
                        new KeyValuePair<string, string>("printerId", deviceId),
                        new KeyValuePair<string, string>("location", ""),
                        new KeyValuePair<string, string>("$PropertySelection", "0,NOT_DISABLED"),
                        new KeyValuePair<string, string>("inputDeviceAdminUsername", "admin"),
                        new KeyValuePair<string, string>("inputDeviceAdminPassword", _device.AdminPassword)
                    });

                    // if (_data.AuthenticationMethod.HasFlag(PaperCutAuthentication.Password))
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("usernamePassword", "on"));

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$0", "NOT_CONFIGURED"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$1", "NOT_CONFIGURED"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$2", "NOT_CONFIGURED"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$3", "NOT_CONFIGURED"));

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("devAuthAnonymousUsername", ""));
                    //tracking options
                    // if (_data.Tracking.HasFlag(PaperCutTracking.Print))
                    {
                        formKeyValuePairs.Add(new KeyValuePair<string, string>("$Checkbox$7", "on"));
                    }

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$4", "0"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("scanCostPerPage", "$0.00"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("scanCostAfterThreshold", "$0.00"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("scanPageCountThreshold", "1"));

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$5", "0"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("faxCostPerPage", "$0.00"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("faxCostAfterThreshold", "$0.00"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("faxPageCountThreshold", "1"));

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$Checkbox$10", "on"));

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$6", "0,SINGLE_QUEUE"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$7", "SECURE"));

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$TextField$6", string.Empty));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("alternateId", string.Empty));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$TextArea", string.Empty));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$Submit$3", "Apply"));

                    FormUrlEncodedContent formContent = new FormUrlEncodedContent(formKeyValuePairs);
                    var signInResponse = client.PostAsync($"http://{_serverInfo.Address}:9191/app", formContent);
                    if (signInResponse.Result.IsSuccessStatusCode)
                    {
                        var signInResponseString = signInResponse.Result.Content.ReadAsStringAsync().Result;
                        hDoc.LoadHtml(signInResponseString);
                        var infoNode = hDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'infoMessage')]");
                        if (infoNode != null)
                        {
                            UpdateStatus(infoNode.Descendants("table").FirstOrDefault().InnerText.Trim());
                        }

                        var errorNode = hDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'errorMessage')]");
                        if (errorNode != null)
                        {
                            UpdateStatus(errorNode.Descendants("table").FirstOrDefault().InnerText.Trim());
                            return new PluginExecutionResult(PluginResult.Failed, errorNode.Descendants("table").FirstOrDefault().InnerText.Trim());
                        }

                        UpdateStatus("Updated device credentials. Now applying settings.");
                        return new PluginExecutionResult(PluginResult.Passed);
                    }

                    return new PluginExecutionResult(PluginResult.Failed,
                        "Unable to add device admin credentials.");
                }

                return new PluginExecutionResult(PluginResult.Failed, "Unable to open device list.");
            }
        }

        private PluginExecutionResult ConfigureSettings(string deviceId)
        {
            Handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = false };
            using (HttpClient client = new HttpClient(Handler))
            {
                client.DefaultRequestHeaders.Add("Accept", AcceptString);
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.Add("DNT", "1");
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.Add("Cookie", $"JSESSIONID={_signedAppSessionId}");
                client.DefaultRequestHeaders.Referrer = new Uri(
                    $"http://{_serverInfo.Address}:9191/app?service=direct/1/DeviceList/selectDevice&sp={deviceId}");

                var deviceResponse = client.GetAsync(
                    $"http://{_serverInfo.Address}:9191/app?service=direct/1/DeviceList/selectDevice&sp={deviceId}");
                if (deviceResponse.Result.IsSuccessStatusCode)
                {
                    HtmlDocument hDoc = new HtmlDocument();
                    hDoc.LoadHtml(deviceResponse.Result.Content.ReadAsStringAsync().Result);

                    var form0Node = hDoc.DocumentNode.SelectSingleNode("//input[contains(@name,'Form0')]");
                    var form0NodeValue = form0Node.Attributes["value"].Value;

                    var sourcePrinterDivs =
                        hDoc.DocumentNode.SelectNodes("//div[contains(@class,'item-checkbox-container')]");
                    var sourcePrinterNode =
                        sourcePrinterDivs.FirstOrDefault(x => x.InnerText.Contains(_data.SourcePrintQueue));
                    if (sourcePrinterNode == null)
                        return new PluginExecutionResult(PluginResult.Failed, "Could not find the source printer");

                    var sourcePrinterId = sourcePrinterNode.Descendants("input").FirstOrDefault()?.Attributes["name"]
                        .Value;

                    //let's add the device credentials to the device
                    client.DefaultRequestHeaders.Referrer = new Uri(
                        $"http://{_serverInfo.Address}:9191/app?service=direct/1/DeviceList/selectDevice&sp={deviceId}");
                    var formKeyValuePairs = (new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("service", "direct/1/PrinterDetails/$Form"),
                        new KeyValuePair<string, string>("sp", "S0"),
                        new KeyValuePair<string, string>("Form0", form0NodeValue),
                        new KeyValuePair<string, string>("printerId", deviceId),
                        new KeyValuePair<string, string>("location", ""),
                        new KeyValuePair<string, string>("$PropertySelection", "0,NOT_DISABLED"),
                        new KeyValuePair<string, string>("inputDeviceAdminUsername", "admin"),
                        new KeyValuePair<string, string>("inputDeviceAdminPassword", _device.AdminPassword)
                    });

                    //let's configure the device with the rest of the values.
                    if (_data.AuthenticationMethod.HasFlag(PaperCutAuthentication.Password))
                        formKeyValuePairs.Add(new KeyValuePair<string, string>("usernamePassword", "on"));

                    if (_data.AuthenticationMethod.HasFlag(PaperCutAuthentication.Identity))
                    {
                        formKeyValuePairs.Add(new KeyValuePair<string, string>("devAuthId", "on"));
                    }

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("devAuthIdPIN", "on"));

                    if (_data.AuthenticationMethod.HasFlag(PaperCutAuthentication.SwipeCard))
                    {
                        formKeyValuePairs.Add(new KeyValuePair<string, string>("devAuthCard", "on"));
                    }

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("devAuthCardPIN", "on"));

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$0", "NOT_CONFIGURED"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$1", "NOT_CONFIGURED"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$2", "NOT_CONFIGURED"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$3", "NOT_CONFIGURED"));

                    if (_data.AuthenticationMethod.HasFlag(PaperCutAuthentication.Guest))
                    {
                        formKeyValuePairs.Add(new KeyValuePair<string, string>("devAuthAutoLogin", "on"));
                    }

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("devAuthAnonymousUsername", ""));

                    //tracking options
                    if (_data.Tracking.HasFlag(PaperCutTracking.Print))
                    {
                        formKeyValuePairs.Add(new KeyValuePair<string, string>("$Checkbox$7", "on"));
                    }

                    if (_data.Tracking.HasFlag(PaperCutTracking.Scan))
                    {
                        formKeyValuePairs.Add(new KeyValuePair<string, string>("$Checkbox$8", "on"));
                    }

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$4", "0"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("scanCostPerPage", "$0.00"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("scanCostAfterThreshold", "$0.00"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("scanPageCountThreshold", "1"));

                    if (_data.Tracking.HasFlag(PaperCutTracking.Fax))
                    {
                        formKeyValuePairs.Add(new KeyValuePair<string, string>("$Checkbox$9", "on"));
                    }

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$5", "0"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("faxCostPerPage", "$0.00"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("faxCostAfterThreshold", "$0.00"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("faxPageCountThreshold", "1"));

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$Checkbox$10", "on"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$Checkbox$11", "on"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>(sourcePrinterId, "on"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$6", "0,SINGLE_QUEUE"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$PropertySelection$7", "SECURE"));

                    if (_data.AutoRelease)
                        formKeyValuePairs.Add(new KeyValuePair<string, string>("releaseAllOnLogin", "on"));

                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$TextField$6", string.Empty));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("alternateId", string.Empty));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$TextArea", string.Empty));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("$Submit$3", "Apply"));

                    var formContent = new FormUrlEncodedContent(formKeyValuePairs);

                    var addDeviceResponse = client.PostAsync($"http://{_serverInfo.Address}:9191/app", formContent);
                    if (addDeviceResponse.Result.IsSuccessStatusCode)
                    {
                        var errorNode = hDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'errorMessage')]");
                        if (errorNode != null)
                        {
                            UpdateStatus(errorNode.Descendants("table").FirstOrDefault()?.InnerText.Trim());
                            return new PluginExecutionResult(PluginResult.Failed, errorNode.Descendants("table").FirstOrDefault()?.InnerText.Trim());
                        }

                        var bodyResponseString = addDeviceResponse.Result.Content.ReadAsStringAsync().Result;
                        var startIndex = bodyResponseString?.IndexOf("csrfToken");
                        var csrfToken = bodyResponseString?.Substring(startIndex.Value + 13);
                        startIndex = csrfToken?.IndexOf("';");
                        csrfToken = csrfToken?.Substring(0, startIndex.Value);
                        UpdateStatus("Added device. Wait for settings to be applied.");
                        client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                        client.DefaultRequestHeaders.Add("X-CSRF-Token", csrfToken);
                        if (Retry.UntilTrue(() => CheckStatus(client, deviceId.Substring(1)), 20,
                            TimeSpan.FromSeconds(10)))
                        {
                            UpdateStatus("Device Configured Successfully.");
                            return new PluginExecutionResult(PluginResult.Passed,
                                "Device Configured Successfully.");
                        }
                    }
                }
            }
            return new PluginExecutionResult(PluginResult.Failed, "Device configuration failed.");
        }

        private string GetDeviceId()
        {
            string deviceId = string.Empty;
            Handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = false };
            using (HttpClient client = new HttpClient(Handler))
            {
                client.DefaultRequestHeaders.Add("Accept", AcceptString);
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.Add("DNT", "1");
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.Add("Cookie", $"JSESSIONID={_signedAppSessionId}");
                client.DefaultRequestHeaders.Add("Referer", $"http://{_serverInfo.Address}:9191/app?service=page/Home");
                var appResponse = client.GetAsync($"http://{_serverInfo.Address}:9191/app?service=page/DeviceList");
                if (appResponse.Result.IsSuccessStatusCode)
                {
                    var responseBody = appResponse.Result.Content.ReadAsStringAsync().Result;
                    HtmlDocument hDoc = new HtmlDocument();
                    hDoc.LoadHtml(responseBody);

                    var resultTable = hDoc.DocumentNode.SelectNodes("//td[contains(@class,'deviceNameColumnValue')]");
                    var deviceRow = resultTable.Descendants("a");
                    var deviceItem = deviceRow.FirstOrDefault(x => x.InnerText.Contains(_printDeviceInfo.AssetId));
                    deviceId = deviceItem?.Attributes["href"].Value.Split('=').LastOrDefault();
                }
            }

            return deviceId;
        }

        private void EnableGuestAccess()
        {
            Handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = false };
            using (HttpClient client = new HttpClient(Handler))
            {
                client.DefaultRequestHeaders.Add("Accept", AcceptString);
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.Add("Cookie", $"sessionId={_signedSessionId}");

                var csrfResponse = client.GetAsync($"https://{_device.Address}/hp/device/AccessControlSetup");
                if (csrfResponse.Result.IsSuccessStatusCode)
                {
                    HtmlDocument hDoc = new HtmlDocument();
                    hDoc.LoadHtml(csrfResponse.Result.Content.ReadAsStringAsync().Result);

                    var csrfToken = hDoc.GetElementbyId("CSRFToken").Attributes["value"].Value;
                    var inputGuestNodes = hDoc.DocumentNode.SelectNodes("//input[contains(@name,'deviceGuest')]");
                    var inputGuestNodesControlPanel =
                        inputGuestNodes.Where(x => !x.Attributes["appid"].Value.StartsWith("ews") && x.Attributes.Contains("appid")).ToList();
                    var formKeyValuePairs = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("CSRFToken", csrfToken) };
                    foreach (var inputGuestNode in inputGuestNodesControlPanel)
                    {
                        formKeyValuePairs.Add(new KeyValuePair<string, string>("deviceGuest", inputGuestNode.Attributes["value"].Value));
                    }

                    foreach (var inputGuestNode in inputGuestNodesControlPanel)
                    {
                        formKeyValuePairs.Add(new KeyValuePair<string, string>("deviceUser", inputGuestNode.Attributes["value"].Value));
                    }
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("SignInMethod__DefaultValue", "hp_EmbeddedPin_v1"));
                    var appIds = inputGuestNodesControlPanel.Select(x => x.Attributes["appId"].Value).Distinct();
                    foreach (var appId in appIds)
                    {
                        formKeyValuePairs.Add(appId.StartsWith("oxpd")
                            ? new KeyValuePair<string, string>($"SignInMethod__{appId}", "PaperCut_AuthenticationAgent_v1")
                            : new KeyValuePair<string, string>($"SignInMethod__{appId}", "default"));
                    }
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("SignInMethod__Ews", "hp_EmbeddedPin_v1"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("AllowUsersChooseSignIn", "on"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("CustomMessage", ""));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("SignOutDelay", "Immediate"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("RetainSettingsCopy", "Always"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("RetainSettingsSend", "Prompt"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("RetainSettingsFax", "Prompt"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("DefaultRole__hp_EmbeddedLDAP_v1", "deviceUser"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("DefaultRole__hp_EmbeddedWindows_v1", "deviceUser"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("DefaultPermissionSetNewAccounts", "deviceUser"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("PinListPagination__itemsPerPage", "25"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("PinListPagination__current", "1"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("PinListPagination__totalNumberOfItems", "0"));
                    formKeyValuePairs.Add(new KeyValuePair<string, string>("FormButtonSubmit", "Apply"));
                    var formContent = new FormUrlEncodedContent(formKeyValuePairs);

                    var accessControlResponse = client.PostAsync($"https://{_device.Address}/hp/device/AccessControlSetup/Save", formContent);
                    if (accessControlResponse.Result.IsSuccessStatusCode)
                    {
                        UpdateStatus("Enabled Guest Access for Control Panel");
                    }
                }
            }
        }

        private bool CheckStatus(HttpClient client, string deviceId)
        {
            var statusResponse = client.GetAsync($"http://{_serverInfo.Address}:9191/rpc/api/rest/internal/adminui/device/{deviceId}/status");
            if (statusResponse.Result.IsSuccessStatusCode)
            {
                var responseString = statusResponse.Result.Content.ReadAsStringAsync().Result;
                UpdateStatus(responseString);
                if (responseString.Contains("Started\""))
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual void UpdateStatus(string text)
        {
            status_RichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogInfo(text);
                _logText.Clear();
                _logText.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
                _logText.Append(":  ");
                _logText.AppendLine(text);
                status_RichTextBox.AppendText(_logText.ToString());
                status_RichTextBox.Refresh();
            }
                );
        }

        #endregion IPluginExecutionEngine implementation
    }
}