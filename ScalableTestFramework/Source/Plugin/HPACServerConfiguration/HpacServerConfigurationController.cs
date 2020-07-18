using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Utility;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Xml;
using System.Xml.Linq;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    /// <summary>
    /// HpacServer ConfigurationController for the HpacServerConfiguration plugin.
    /// </summary>
    public class HpacServerConfigurationController
    {
        private readonly string _serviceUrl;
        private string _domain;
        private string jaLoginSessionId;
        private string _sessionId;
        private const string DPRAA = "DPRAA/DPR.asmx";
        private const string IRM = "Ad-Authenticator/AD-Configurator.aspx";
        private const string AdminConsole = "AdminConsoleWS/AdminConsole.asmx";
        private readonly string VpsxUrl;
        private readonly string _serverName;
        private readonly string _serverIp;
        private string _cookies;
        private string _serverResponseBody;

        /// <summary>
        /// Constructor, takes the domain and the name of the HPAC server then combines them for the webservice domain
        /// </summary>
        /// <param name="domain">Domain</param>
        /// <param name="serverName">HPAC server name</param>
        /// <param name="serverIp">Server Address</param>
        /// <param name="user">User Credentials</param>
        public HpacServerConfigurationController(string domain, string serverName, string serverIp, NetworkCredential user)
        {
            _domain = domain;
            _serviceUrl = $"http://{serverIp}";
            UserCredential = user;
            _serverName = serverName;
            _serverIp = serverIp;
            VpsxUrl = $"https://{serverName}/hpacspp/nlrswc2.exe/VPSX";
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        private string SignInOmni(string address, string password)
        {
            string sessionId = string.Empty;// = GetSessionId(address);
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

        public NetworkCredential UserCredential { get; set; }

        /// <summary>
        /// Executes the Capella webservice for HPAC
        /// </summary>
        /// <param name="url">The domain of the webservice</param>
        /// <param name="service">The service name or location</param>
        /// <param name="function">The function of the webservice</param>
        /// <param name="args">Arguments that the webservice uses to process requests</param>
        /// <returns>A string of XML data about the response</returns>
        private static HttpWebResponse ExecuteWebService(string url, string service, string function, string args)
        {
            return PostRequest($"{url}/{service}/{function}", args);
        }

        /// <summary>
        /// Queue is added
        /// </summary>
        /// <param name="queuename">Queue name.</param>
        public void AddQueue(string queuename)
        {
            string hpacPath = @"C:\Program Files\HP\HP Access Control";
            string spoolPath = @"C:\Program Files\HP\HP Access Control\spoolroot";

            var responseCode = ExecuteWebService(_serviceUrl, AdminConsole, "ManagePersonalQ", $"queuename={queuename}&duplex=true&color=true&retain=0&delete=false&filter=false&path={hpacPath}&spoolpath={spoolPath}&TCPPORTDPR=5501");
            if (responseCode.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Unable to add Queue:");
            }
            Framework.Logger.LogInfo("Queue added Successfully.");
        }

        /// <summary>
        ///  Add Multiple Server URI
        /// </summary>
        /// <param name="serverUri">Server URI.</param>
        public void AddMultipleServer(string serverUri)
        {
            if (!string.IsNullOrEmpty(serverUri))
            {
                string postData = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                              "<ExecuteNonQueryCE xmlns = \"http://adminconsolews.capellatech.com/\">" +
                              $"<sqlQuery>INSERT INTO VPSX (Name, Description, Status) VALUES (N'{serverUri}:631','', '')</sqlQuery>" +
                              "<sqlConn>Data Source=C:\\Program Files\\HP\\HP Access Control\\Database\\config4.sdf; Password='89SWenEC'</sqlConn>" +
                              "</ExecuteNonQueryCE></s:Body></s:Envelope>";

                Dictionary<string, string> headers = new Dictionary<string, string>
                {
                    {"SOAPAction", "http://adminconsolews.capellatech.com/ExecuteNonQueryCE"},
                    {"Accept-Encoding", "gzip, deflate"}
                };

                int responseCode = PostSoapRequest($"{_serviceUrl}/{AdminConsole}", postData, headers);
                if (responseCode != 200)
                {
                    throw new WebException("Unable to add New Server:");
                }
                Framework.Logger.LogInfo("New Server added Successfully.");
            }
        }

        /// <summary>
        ///  Add Server to IIS Tab
        /// </summary>
        /// <param name="serverUri">Server URI.</param>
        /// <param name="ServerIPAddress">Server Address</param>
        public void AddServerToIIS(string serverUri, string ServerIPAddress)
        {
            if (!string.IsNullOrEmpty(serverUri))
            {
                string postData = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                                  "<ExecuteNonQueryCE xmlns = \"http://adminconsolews.capellatech.com/\">" +
                                  $"<sqlQuery>INSERT INTO IIS (Name, IP, Description, Status, Master) VALUES (N'{serverUri}','{ServerIPAddress}',N'','', 0)</sqlQuery>" +
                                  "<sqlConn>Data Source=C:\\Program Files\\HP\\HP Access Control\\Database\\config4.sdf; Password='89SWenEC'</sqlConn>" +
                                  "</ExecuteNonQueryCE></s:Body></s:Envelope>";
                Dictionary<string, string> headers = new Dictionary<string, string>
                {
                    {"SOAPAction", "http://adminconsolews.capellatech.com/ExecuteNonQueryCE"},
                    {"Accept-Encoding", "gzip, deflate"}
                };
                int responseCode = PostSoapRequest($"{_serviceUrl}/{AdminConsole}", postData, headers);
                if (responseCode != 200)
                {
                    throw new WebException("Unable to add Server to ISS Tab:");
                }
                Framework.Logger.LogInfo("Server added IIS Tab Successfully.");
            }
        }

        /// <summary>
        /// Queue is deleted
        /// </summary>
        /// <param name="queuename">Queue name.</param>
        public void DeleteQueue(string queuename)
        {
            string hpacPath = @"C:\Program Files\HP\HP Access Control";
            string spoolPath = @"C:\Program Files\HP\HP Access Control\spoolroot";
            var responseCode = ExecuteWebService(_serviceUrl, AdminConsole, "ManagePersonalQ", $"queuename={queuename}&duplex=true&color=true&retain=0&delete=true&filter=false&path={hpacPath}&spoolpath={spoolPath}&TCPPORTDPR=");
            if (responseCode.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Unable to Delete Queue");
            }
            Framework.Logger.LogInfo("Queue Deleted Successfully.");
        }

        /// <summary>
        /// Installs the Agent
        /// </summary>
        /// <param name="deviceIp">device Ip.</param>
        public void InstallAgent(string deviceIp)
        {
            var bundleFile = GetValueFromRegistry("MIPA", "MIPAInstallFile", "SOFTWARE\\Hewlett-Packard\\HP ACJA\\");
            //int responseCode = ExecuteWebService(_serviceUrl, AdminConsole, "Print9100", $"deviceHost={deviceIp}&model=FutureSmart&install=true&jobAbsPath={bundleFile}&message=installing agent");
            //if (responseCode != 200)
            if (!InstallAgentRequest(deviceIp, bundleFile))
            {
                throw new WebException("Fail to install HP Agent");
            }
            Framework.Logger.LogInfo("HP Agent installed Successfully.");
        }

        /// <summary>
        /// Devices is added
        /// </summary>
        /// <param name="printDevice"></param>
        public void AddDevice(PrintDeviceInfo printDevice)
        {
            //string addXml = string.Format(HpacResource.AddPrinterHPACPostdata, deviceIp, adminPassword);
            //var responseCode = ExecuteNestedService($"{_serviceUrl}/{AdminConsole}", addXml, @"http://adminconsolews.capellatech.com/GetDeviceInfo");
            //if (responseCode.StatusCode != HttpStatusCode.OK)
            //{
            //    throw new WebException("Fail to Add Device");
            //}
            var stokValue = GetLogonInformation(out _cookies);
            string vpsServerId = GetVpsServerId(ref stokValue, _cookies);
            var addWebRequest = CreateVpsxWebRequest(_cookies);
            var printerInfo = GetPrinterInfo(printDevice);
            string postData =
                $"trid=vxajax&method=AjaxSavePrtConfig&ltrid=vxvprl&stok={stokValue}&srvid=VSV1&strid=add-printer&vpsid=%3C%3FVPSID%3E&prtid=%3C%3FPRTID%3E&tabid=%3C%3FTABID%3E&cmdid=8&revision=" +
                $"&prtname={HttpUtility.UrlEncode(printDevice.AssetId)}&oprtname=&vpsname={vpsServerId}&ovpsname=&tcphost={printDevice.Address}&otcphost=&devdescDISPLAY={HttpUtility.UrlEncode(printerInfo[2])}&devdescSNMP={HttpUtility.UrlEncode(printerInfo[2])}&" +
                "odevdescSNMP=&devdescKWD=&odevdescKWD=&commtype=TCPIP%2FSOCK&ocommtype=&tcprport=9100&otcprport=&tcprprt=&otcprprt=&tcpalthost=&otcpalthost=&jobovlap=1&ojobovlap=1&driver=&odriver=&ppdfile=&oppdfile=&location=&olocation=" +
                "&contact=&ocontact=&dept=&odept=&rtime=0&ortime=&duplex=on&pullprint=on&ocolor=N&oduplex=N&ostaple=N&opullprint=N&snmp=on&osnmp=N&snmpversion=&osnmpversion=&snmpname=public&osnmpname=public&snmpreaduser=&osnmpreaduser=" +
                "&snmpauthprot=&osnmpauthprot=&snmpauthtype=&osnmpauthtype=&snmpauthpswd=&osnmpauthpswd=&snmpprivprot=&osnmpprivprot=&snmpprivtype=&osnmpprivtype=&snmpprivpswd=&osnmpprivpswd=&snmpcontext=Jetdirect&osnmpcontext=&devtype=" +
                "&odevtype=TXT&devmodel=&odevmodel=&pcmdstrt=&opcmdstrt=&pcmdend=&opcmdend=&separ=N&osepar=N&separname=default&oseparname=default&oseprestart=N&winpaper=&owinpaper=&winduplex=&owinduplex=&wincolor=&owincolor=&winorient=&owinorient=" +
                "&oencrypt=N&edevice=LRSQUEUE&oedevice=LRSQUEUE&ekey=dynamic&oekey=dynamic&erract=HOLD&oerract=HOLD&ftype1=&oftype1=&filter1=&ofilter1=&farg1=&ofarg1=&ftype2=&oftype2=&filter2=&ofilter2=&farg2=&ofarg2=&ftype3=&oftype3=&filter3=&ofilter3=" +
                "&farg3=&ofarg3=&ftype4=&oftype4=&filter4=&ofilter4=&farg4=&ofarg4=&ftype5=&oftype5=&filter5=&ofilter5=&farg5=&ofarg5=&ftype6=&oftype6=&filter6=&ofilter6=&farg6=&ofarg6=&ftype7=&oftype7=&filter7=&ofilter7=&farg7=&ofarg7=&ftype8=&oftype8=" +
                "&filter8=&ofilter8=&farg8=&ofarg8=&ftype9=&oftype9=&filter9=&ofilter9=&farg9=&ofarg9=&altprtr=&oaltprtr=&odivert=N&divertrsn=&odivertrsn=&oacct=N&ae=on&oae=Y&compress=on&ocompress=Y&odrain=N&ffseq=0D0C0D&offseq=0D0C0D" +
                "&nlseq=0D0A&onlseq=0D0A&queue=&oqueue=&qtime=0&oqtime=&retrycnt=20&oretrycnt=&ohold=N&zplchar=10&ozplchar=10&maxkbps=0&omaxkbps=&ovpsx2vpsx=N&tcpdisc=0&otcpdisc=&opjl=N&opjlopt1=N&opjlopt2=N&opjlopt3=N&opjlopt4=N" +
                "&opjlopt5=N&opjlopt6=N&opjlopt7=N&opjlopt8=N&opjlopt9=N&opjlopt10=N&opjlopt11=N&opjlopt12=N&oprtropt1=N&oprtropt11=N&oprtropt7=N&oprtropt4=N&oprtropt5=N&oprtropt6=N&oprtropt3=N&oprtropt8=N&oprtropt10=N&oprtropt9=N" +
                "&oprtropt13=N&oprtropt12=N&oprtropt2=N&winopt1-decision=off&winopt1=&owinopt1=N&winopt2-decision=off&winopt6=&owinopt6=N&winopt2=&owinopt2=N&owinopt3=N&owinopt4=N&owinopt5=N&ovwcopt1=N&class=&oclass=&form=&oform=" +
                "&udata1=&oudata1=&udata2=&oudata2=&udata3=&oudata3=&udata4=&oudata4=&mconopts=&omconopts=&mfpxtaddr=&omfpxtaddr=&devuser=&odevuser=&fakepassword=_%2B%3F%3F334&devpass=&odevpass=&pjlpass=&opjlpass=&osslopt1=N" +
                "&ousrprops=&usrprops=&mailto=&omailto=&mailfrom=&omailfrom=&replyto=&oreplyto=&mailcset=ISO-8859-15&omailcset=ISO-8859-15&mailsize=0&omailsize=&mailfnam=&omailfnam=&omailopt1=&omailopt2=&omailopt3=&omailopt4=" +
                "&omailopt5=&omailopt6=&notmail=&onotmail=&notlevel=1&onotlevel=1&otrcopt1=N&otrcopt2=N&otrcopt3=N&otrcopt4=N&otrcopt5=N&otrcopt6=N&otrcopt7=N&otrcopt8=N";
            var buffer = Encoding.ASCII.GetBytes(postData);
            addWebRequest.ContentLength = buffer.Length;
            using (var requestStream = addWebRequest.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
            }
            var response = (HttpWebResponse)addWebRequest.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Fail to Add Device");
            }
            Framework.Logger.LogInfo("Added the device successfully.");
        }

        /// <summary>
        ///Configures the Device
        /// </summary>
        /// <param name="device">device</param>
        /// <param name="configuration">configuration list</param>
        public void ConfigureDevice(PrintDeviceInfo device, List<HpacConfiguration> configuration)
        {
            bool trackingValue = false;
            bool pullPrintingValue = false;
            bool authenticationValue = false;
            bool quota = false;

            foreach (var configitem in configuration)
            {
                switch (configitem)
                {
                    case HpacConfiguration.PullPrinting:
                        pullPrintingValue = true;
                        break;

                    case HpacConfiguration.Tracking:
                        trackingValue = true;
                        break;

                    case HpacConfiguration.Authentication:
                        authenticationValue = true;
                        break;

                    case HpacConfiguration.Quota:
                        quota = true;
                        break;
                }
            }

            //have to enable PJL for the installation to go through, we will disable it once we are done.
            EnablePJL(device);
            var printerInfo = GetPrinterInfo(device);

            //we need to establish a session and then send the configure object
            //var _serverName = "SRVTHPAC1808.etl.psr.rd.hpicorp.net";
            var stokValue = GetLogonInformation(out _cookies);
            string vpsServerId = GetVpsServerId(ref stokValue, _cookies);
            string portString = HttpUtility.UrlEncode($"prx:{device.AssetId};{vpsServerId}");

            var snmpGetStringValue = GetPortList(portString, ref stokValue, _cookies);
            if (string.IsNullOrEmpty(snmpGetStringValue))
            {
                throw new WebException("Device not found in HPAC Server. Try adding it first.");
            }
            snmpGetStringValue = snmpGetStringValue.Replace("\\/", "/");

            //0 - servername, 1- printername, 2 - printer ip, 3 - printer model, 4- pullprinting, 5- tracking, 6- authentication, 7 - quota, 8 - device password, 9 - stokvalue
            string mfpData = string.Format(
                "{{\"MFP\":[{{\"Hostname\":\"{0}\",\"printerIP\":\"{2}\",\"xtBox\":false,\"Elatec\":false,\"printerName\":\"{1};{9}\",\"secureDelivery\":false,\"scan2PCX\":false,\"scan2Email\":false," +
                "\"VPSXURL\":\"http://{0}:631/{1}\",\"VPSXStatusURL\":[\"http://{0}:631/{1}\"],\"manualDeployment\":false,\"printerModel\":\"{3}\",\"locale\":\"en-US\"," +
                "\"AgentData\":{{\"SNMPVersion\":1,\"snmpGet\":\"{8}\"}},\"pullprinting\":{4},\"tracking\":{5},\"trackingserver\":\"{0}\",\"trackingmethod\":0,\"authentication\":{6}," +
                "\"authenticationserver\":\"{0}\",\"quota\":{7},\"authenticationmethod\":0}}]}}", _serverName.ToUpper(),
                device.AssetId.ToUpper(), device.Address, printerInfo[2], pullPrintingValue.ToString().ToLowerInvariant(), trackingValue.ToString().ToLowerInvariant(), authenticationValue.ToString().ToLowerInvariant(), quota.ToString().ToLowerInvariant(), snmpGetStringValue, vpsServerId);
            mfpData = HttpUtility.UrlEncode(mfpData);
            var postData = string.Format("MFPHost={0}&MFPData={1}&method=MFP_Route&MFPMethod=MFP_InstallWorkflow&stok={2}&srvid=VSV1&trid=vxajax", _serverName, mfpData, stokValue);

            //string configXml = string.Format(HpacResource.ConfigureDeviceHPACPostdata, device.Address, printerInfo[0], printerInfo[1], printerInfo[2], device.AdminPassword, _serverIp, _serverName, pullPrinting, trackingValue, authenticationalue, authorization, quota, dtm);
            //var responseCode = ExecuteNestedService($"{_serviceUrl}/{AdminConsole}", configXml, @"http://adminconsolews.capellatech.com/ConfigureDevice");
            var webRequest = CreateVpsxWebRequest(_cookies);
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            var buffer = Encoding.ASCII.GetBytes(postData);
            webRequest.ContentLength = buffer.Length;
            using (var requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
            }

            var response = (HttpWebResponse)webRequest.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Fail to Configure Device.");
            }

            response.Close();
        }

        public void CheckForDeployment(PrintDeviceInfo printDeviceInfo)
        {
            var stokValue = GetLogonInformation(out _cookies);
            string vpsServerId = GetVpsServerId(ref stokValue, _cookies);
            var portString = $"{printDeviceInfo.AssetId};{vpsServerId}";
            if (Retry.UntilTrue(() => IsInstallCompleted(portString, stokValue), 20, TimeSpan.FromSeconds(10)))
            {
                Framework.Logger.LogInfo("Device Configured Successfully.");
            }
            else
            {
                DisablePJL(printDeviceInfo);
                Framework.Logger.LogError("Device Installation is not completed, please check server for further information");
                throw new Exception($"Deployment failed, Please check the server for further information. Server Response: {_serverResponseBody}");
            }
            DisablePJL(printDeviceInfo);
        }

        private bool IsInstallCompleted(string portString, string stokValue)
        {
            var statusCheckRequest = CreateVpsxWebRequest(_cookies);
            string mfpData = HttpUtility.UrlEncode($"{{\"MFP\":[{{\"printerName\":\"{portString}\"}}]}}");
            string postData =
                $"MFPHost={_serverName}&MFPData={mfpData}&method=MFP_Route&MFPMethod=MFP_InstallStatus&stok={stokValue}&srvid=VSV1&trid=vxajax";

            var buffer = Encoding.ASCII.GetBytes(postData);
            statusCheckRequest.ContentLength = buffer.Length;
            using (var requestStream = statusCheckRequest.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
            }

            var response = (HttpWebResponse)statusCheckRequest.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
                var encoding = Encoding.GetEncoding(response.CharacterSet);
                StreamReader sReader = new StreamReader(responseStream, encoding);
                _serverResponseBody = sReader.ReadToEnd();

                //read the server response which is in JSON format
                var jsonList = _serverResponseBody.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //the line with JSONSTRING contains the response
                var jsonstring = jsonList.Find(x => x.Contains("JSONSTRING"));

                //lets split the information
                var messageList = jsonstring.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //we are only interested in the latest information, so we will take the last 15 entries
                var latestMessageList = messageList.Skip(Math.Max(0, messageList.Count - 15));
                //clean up the messages
                latestMessageList = latestMessageList.Select(x => x.RemoveWhiteSpace());
                latestMessageList = latestMessageList.Select(x => x.Replace("\\", ""));

                //now display the same
                var latestMessage = string.Join(",", latestMessageList).Trim();

                Framework.Logger.LogDebug(latestMessage);
                if (latestMessage.Contains("COMPLETE", StringComparison.Ordinal))
                {
                    response.Close();
                    return true;
                }

                //done to propogate the final message to the user
                _serverResponseBody = latestMessage;
            }

            response.Close();
            return false;
        }

        private string GetPortList(string portString, ref string stokValue, string cookies)
        {
            string snmpGetString = string.Empty;
            var webRequest = CreateVpsxWebRequest(cookies);
            string postData =
                $"arraymax=1&doMasking=n&find={portString}&plst=11&method=PrtList&stok={stokValue}&srvid=VSV1&trid=vxajax";

            var buffer = Encoding.ASCII.GetBytes(postData);
            webRequest.ContentLength = buffer.Length;
            using (var requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
            }

            var response = (HttpWebResponse)webRequest.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
                var encoding = Encoding.GetEncoding(response.CharacterSet);
                StreamReader sReader = new StreamReader(responseStream, encoding);
                var responseBody = sReader.ReadToEnd();
                var jsonList = responseBody.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                stokValue = jsonList.Find(x => x.Contains("STOK"));
                stokValue = stokValue.Split(':').Last().Trim().Replace("\"", "");
                stokValue = stokValue.Remove(stokValue.Length - 1);
                snmpGetString = jsonList.Find(x => x.Contains("snmpGet"));
                if (!string.IsNullOrEmpty(snmpGetString))
                {
                    snmpGetString = snmpGetString.Split(':').Last().Trim().Replace("\"", "");
                    snmpGetString = snmpGetString.Remove(snmpGetString.Length - 1).Trim();
                }
            }
            response.Close();
            return snmpGetString;
        }

        private string GetVpsServerId(ref string stokValue, string cookies)
        {
            string postData =
                "printermask=&groupmask=&vpsmask=&statusmask=0&devmask=0&devprop=10&location=&oquemin=0&rquemin=0&mfpmask=&mfpagent=&mfpdevice=&mfpinstallstatusmask=0&mfpworkflowsmask=0&plst=101&arraymax=3&vsvxra=LOCATION&" +
                $"vpsxra=OCOUNT+RCOUNT&doMasking=y&find=&method=PrtList&stok={stokValue}&srvid=VSV1&trid=vxajax";

            var webRequest = CreateVpsxWebRequest(cookies);
            var buffer = Encoding.ASCII.GetBytes(postData);
            webRequest.ContentLength = buffer.Length;
            using (var requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
            }

            var response = (HttpWebResponse)webRequest.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
                var encoding = Encoding.GetEncoding(response.CharacterSet);
                StreamReader sReader = new StreamReader(responseStream, encoding);
                var responseBody = sReader.ReadToEnd();
                var jsonList = responseBody.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                stokValue = jsonList.Find(x => x.Contains("STOK"));
                stokValue = stokValue.Split(':').Last().Trim().Replace("\"", "");
                stokValue = stokValue.Remove(stokValue.Length - 1);
                string vpsId = jsonList.Find(x => x.Contains("VPSID"));
                vpsId = vpsId.Split(':').Last().Trim().Replace("\"", "");
                response.Close();
                return vpsId.Remove(vpsId.Length - 1).Trim();
            }
        }

        private string GetLogonInformation(out string cookies)
        {
            var logonWebRequest = (HttpWebRequest)WebRequest.Create($"{VpsxUrl}?trid=logonv&hpac=y");

            logonWebRequest.Method = "POST";
            // logonWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            logonWebRequest.Headers.Add("UA-CPU", "AMD64");
            logonWebRequest.Accept = "*/*";
            logonWebRequest.ContentType = "application/x-www-form-urlencoded";
            string postData =
                $"trid=logonv&jtrid=vxvprl&usid=admin&pwid=Admin&srvid=VSV1&hpac=y&hpacfqdn={_serverName}&hpacspp=y&hpacppqadmin=y";
            var buffer = Encoding.ASCII.GetBytes(postData);
            logonWebRequest.ContentLength = buffer.Length;
            using (var requestStream = logonWebRequest.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
            }

            var logonResponse = (HttpWebResponse)logonWebRequest.GetResponse();
            string responseBody;
            using (var responseStream = logonResponse.GetResponseStream())
            {
                var encoding = Encoding.GetEncoding(logonResponse.CharacterSet);
                StreamReader sReader = new StreamReader(responseStream, encoding);
                responseBody = sReader.ReadToEnd();
            }

            logonResponse.Close();
            cookies = logonResponse.Headers["Set-Cookie"];

            int startIndex = responseBody.IndexOf("name=\"stok\" value=\"");
            int endIndex = responseBody.IndexOf("\" />", startIndex);
            var stokValue = responseBody.Substring(startIndex + "name=\"stok\" value=\"".Length,
                endIndex - startIndex - "name=\"stok\" value=\"".Length);
            return stokValue;
        }

        private HttpWebRequest CreateVpsxWebRequest(string cookies)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(VpsxUrl);
            webRequest.Method = "POST";
            webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            webRequest.Headers.Add("UA-CPU", "AMD64");
            webRequest.Headers.Add("Cache-Control", "no-cache");
            webRequest.Accept = "application/json, text/javascript, */*";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.KeepAlive = true;
            webRequest.Headers.Add(HttpRequestHeader.Cookie, cookies);
            webRequest.ServicePoint.Expect100Continue = false;

            return webRequest;
        }

        /// <summary>
        /// TO update the Authenticator for HPAC
        /// </summary>
        /// /// <param name="device">device</param>
        /// <param name="authenticators">configuration list</param>
        public void UpdateAuthenticators(PrintDeviceInfo device, List<HpacAuthenticators> authenticators)
        {
            string licenseId;
            string csrfToken;
            _sessionId = SignInOmni(device.Address, device.AdminPassword);
            var httpClientHandler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = false, MaxAutomaticRedirections = 2 };
            var httpClient = new HttpClient(httpClientHandler);

            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
            httpClient.DefaultRequestHeaders.Add("Cookie", $"sessionId={_sessionId}");

            try
            {
                var licenseMessage = httpClient.GetStringAsync($"https://{device.Address}/hp/device/Solution.AccessControl.Jedi.WebSetup/Index").Result;

                if (string.IsNullOrEmpty(licenseMessage))
                    throw new WebException("HPAC License Identifier information not found.");

                HtmlDocument hDoc = new HtmlDocument();
                hDoc.LoadHtml(licenseMessage);
                licenseId = hDoc.DocumentNode.DescendantsAndSelf("input").FirstOrDefault(x => x.Id == "licenseIdentifier")?.Attributes["value"].Value;
                csrfToken = hDoc.GetElementbyId("CSRFToken")?.Attributes["value"]?.Value;
                if (string.IsNullOrEmpty(licenseId))
                {
                    throw new WebException("HPAC License Identifier information not found.");
                }
            }
            catch (Exception e)
            {
                throw new HttpRequestException("Solution Installer not available on this device." + e.JoinAllErrorMessages());
            }

            var multiPartFormData =
                new MultipartFormDataContent { { new StringContent(licenseId), "\"licenseIdentifier\"" } };
            if (!string.IsNullOrEmpty(csrfToken))
            {
                multiPartFormData.Add(new StringContent(csrfToken), "\"CSRFToken\"");
            }
            foreach (var authenticator in authenticators)
            {
                multiPartFormData.Add(new StringContent(authenticator.GetDescription()), "\"authenticatorsEnabled\"");
            }
            multiPartFormData.Add(new StringContent("Update"), "\"update\"");
            var fileContent = new StreamContent(Stream.Null);
            fileContent.Headers.Add("Content-Disposition", "form-data; name=\"bundleFile\"; filename=\"\"");
            multiPartFormData.Add(fileContent, "bundleFile", "\"\"");

            var message = httpClient.PostAsync(
                $"https://{device.Address}/hp/device/Solution.AccessControl.Jedi.WebSetup/Save",
                multiPartFormData);

            if (!message.Result.IsSuccessStatusCode)
            {
                // UpdateStatus("Unable to set backup restore option setting, continuing with warning");
                throw new WebException("Updating Authenticators failed.");
            }
        }

        /// <summary>
        ///Configures the Print Server
        /// </summary>
        /// <param name="printername">printername.</param>
        /// <param name="configuration">configuration</param>
        public void ConfigurePrintServer(string printername, List<HpacConfiguration> configuration)
        {
            int responseCode = 200;
            bool result = true;
            foreach (var configitem in configuration)
            {
                switch (configitem)
                {
                    case HpacConfiguration.Tracking:
                        //responseCode = ExecuteWebService(_serviceUrl, AdminConsole, "iSetValueToPrinterRegistry", $"printername={printername}&keyname=MT_NoLocalTracking&value=0");
                        result &= SetValueToPrinterRegistry(printername, "MT_NoLocalTracking", "0");
                        break;

                    case HpacConfiguration.Quota:
                        result &= SetValueToPrinterRegistry(printername, "MT_NoLocalTracking", "1");
                        result &= SetValueToPrinterRegistry(printername, "MT_QuotaSupport", "1");
                        ConfigureQuota(printername, "1", _serverName);
                        result &= SetValueToPrinterRegistry(printername, "MT_QuotaSupport", "0");
                        break;

                    case HpacConfiguration.IPM:
                        result &= SetValueToPrinterRegistry(printername, "MT_NoLocalTracking", "1");
                        result &= SetValueToPrinterRegistry(printername, "MT_IPMSupport", "1");
                        result &= SetValueToPrinterRegistry(printername, "MT_IPMCost", "0");
                        break;
                }
            }
            // int responseCode = ExecuteWebService (_serviceUrl, AdminConsole, "iSetValueToPrinterRegistry", "printername={0}&keyname=MT_NoLocalTracking&value=0",printername));
            if (responseCode != 200 || !result)
            {
                throw new WebException("Fail to Configure PrintServer");
            }

            Framework.Logger.LogInfo("PrintServer configured Successfully.");
        }

        /// <summary>
        /// This selects the Protocol option under the Setting Tab in HPAC server setting based on the value of protocol option selected
        /// </summary>
        /// <param name="protocolOption"></param>
        public void SelectProtocolOption(ProtocolOptions protocolOption)
        {
            if (!SetValueToRegistry("Application", "PrintMethod", $"{(int)protocolOption}", "SOFTWARE\\Hewlett-Packard\\DPR"))
            {
                throw new WebException("Unable to Configure Protocol Options");
            }
            Framework.Logger.LogInfo("Protocol Options Configured Successfully.");
        }

        /// <summary>
        ///Configures the IRM
        /// </summary>
        /// <param name="authenticationMethod">authentication Method.</param>
        /// <param name="storageMethod">storageM ethod</param>
        public void ConfigureIrm(string authenticationMethod, string storageMethod)
        {
            int responseCode = ExecuteAuthenticationWebService(_serviceUrl, "ad-authenticator", "Ad-configurator.aspx ", $"encrypted:false&authenticationMethod:{authenticationMethod}&storageMethod:{storageMethod}&multicard:false&features:null&states:(name:HPAC-IPA&enabled:true)&(name:HPAC-PIC&enabled:true)&(name:HPAC-DRA&enabled:true)&(name:HPAC-IRM&enabled:true)");
            if (responseCode != 200)
            {
                throw new WebException("Fail to Configure IRM");
            }
            Framework.Logger.LogInfo("IRM configured Successfully.");
        }

        /// <summary>
        ///Configures the LDAP IRM
        /// </summary>
        /// <param name="server">server name.</param>
        /// <param name="username">user name</param>
        /// <param name="password">password</param>
        public void ConfigureLdapIrm(string server, string username, string password)
        {
            var responseCode = ExecuteWebService(_serviceUrl, IRM, "SetConfigurationLdap", $"servers:(server:{server}&username:{username}&password:{password}&binding:simple)");
            if (responseCode.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Fail to Configure LdapIrm");
            }
            Framework.Logger.LogInfo("LdapIrm configured Successfully.");
        }

        /// <summary>
        ///Configures the Card attribute
        /// </summary>
        /// <param name="cardAttribute">card Attribute.</param>
        public void ConfigureCardAttribute(string cardAttribute)
        {
            var responseCode = ExecuteWebService(_serviceUrl, IRM, "SetConfigurationCards", $"cardStorageMethod:AD&cardAttribute:{cardAttribute}&cardAttributeIsReadOnly:false&userEditorCardIsVisible:true&cardDecodingEnabled:false&cardsPermitNetworkCredentials:true&cardsPermitSelfEnrollment:true");
            if (responseCode.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Fail to Configure Card Attribute");
            }
            Framework.Logger.LogInfo("Card Attribute configured Successfully.");
        }

        /// <summary>
        ///Configures the Code attribute
        /// </summary>
        /// <param name="codeAttribute">code Attribute.</param>
        public void ConfigureCodeAttribute(string codeAttribute)
        {
            var responseCode = ExecuteWebService(_serviceUrl, IRM, "SetConfigurationCodes", $"codeAttribute:{codeAttribute}&codeAttributeIsReadOnly:false&codeGeneratorMinLength:6&codeGeneratorMaxLength:6&codeGeneratorCharacters:0123456789&codePassphrase: &userEditorCodeIsVisible:true&userEnrollCodeIsVisible:true&userEnrollCanSendEmails:true&userEnrollCanChangeCode:true&userEnrollCanChooseCode:true");
            if (responseCode.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Fail to Configure Code Attribute");
            }
            Framework.Logger.LogInfo("Code Attribute configured Successfully.");
        }

        /// <summary>
        ///Configures the Job Accounting Login
        /// </summary>
        /// <param name="userId">user ID.</param>
        /// <param name="password">Password.</param>
        private void JobAccountingLogin(string userId, string password)
        {
            Uri jobAccountingLoginUrl = new Uri($@"http://{_serverName}/hp_acja/global/login.asp?mt=1 ");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(jobAccountingLoginUrl);
            webRequest.Accept = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, */*";
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            webRequest.ContentType = "application/x-www-form-urlencoded";
            SessionIDManager manager = new SessionIDManager();
            jaLoginSessionId = manager.CreateSessionID(HttpContext.Current);
            webRequest.CookieContainer = new CookieContainer();
            webRequest.CookieContainer.Add(new Cookie("ASPSESSIONIDCCTBQRAB", jaLoginSessionId) { Domain = _serverName });
            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
            string postData = $"login={userId}&password={password}&resx=&resy=";
            HttpWebResult loginResult = HttpWebEngine.Post(webRequest, postData);
            jaLoginSessionId = loginResult.Headers.GetValues("Set-Cookie")?.FirstOrDefault();
            RetrieveSessionId();
        }

        /// <summary>
        ///Sets the Quota
        /// </summary>
        /// <param name="quotaMenu">quota Menu</param>
        public void SetQuota(string quotaMenu)
        {
            Uri jobAccountingLoginUrl = new Uri($@"http://{_serverName}/hp_acja/quota/updatedefault.asp");
            JobAccountingLogin(UserCredential.UserName, UserCredential.Password);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(jobAccountingLoginUrl);
            webRequest.Accept = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, */*";
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.CookieContainer = new CookieContainer();
            webRequest.CookieContainer.Add(new Cookie("ASPSESSIONIDCCTBQRAB", jaLoginSessionId));
            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
            string postData = $"QuotaMode={quotaMenu}&RadioQuota=3&QuotaRenew=1&limit=2000&warning=50&colorlimit=500&colorwarning=50&copieslimit=2000&colorcopieslimit=50&digitalsendinglimit=2000&limitcc=50000&warningcc=50&colorlimitcc=10000&colorwarningcc=50&copieslimitcc=500&colorcopieslimitcc=50&digitalsendinglimitcc=500&limitprintercc=5&warningprintercc=5&colorlimitprintercc=5&colorwarningprintercc=5&copieslimitprinter=5&colorcopieslimitprinter=5&digitalsendinglimitprinter=5&warningmessage=Your+job+has+been+processed%3B+the+number+of+pages+remaining+is&errormessage=Your+job+has+been+deleted+without+printing%3B+this+job+exceeds+your+available+pages+quota&colorwarningmessage=Your+job+has+been+processed%3B+the+number+of+color+pages+remaining+is&colorerrormessage=Your+job+has+been+deleted+without+printing%3B+this+job+exceeds+your+available+color+pages+quota";
            HttpWebEngine.Post(webRequest, postData);
        }

        /// <summary>
        ///Get the Report
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="email">email</param>
        /// <param name="format">format</param>
        public void GetReport(string name, string email, string format)
        {
            Uri jobAccountingLoginUrl = new Uri($@"http://{_serverName}/hp_acja/report/ChParam.asp");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(jobAccountingLoginUrl);
            webRequest.Accept = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, */*";
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.CookieContainer = new CookieContainer();
            webRequest.CookieContainer.Add(new Cookie("ASPSESSIONIDCCTBQRAB", jaLoginSessionId));
            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
            string postData = $"name={name}&month=11&year=2016&email={email}&schedule=1&format={format}";
            HttpWebEngine.Post(webRequest, postData);
        }

        /// <summary>
        ///Enables the Purge
        /// </summary>
        /// <value><c>true</c> if Purge should be enabled; otherwise, <c>false</c>.</value>
        public void EnablePurge(bool value)
        {
            //int responseCode = ExecuteWebService(_serviceUrl, "AdminConsoleWS/AdminConsole.asmx", "SetValueToRegistry", $"name=Application&key=PurgeTracking&value={value}&path=SOFTWARE\\Hewlett-Packard\\DPR\\");
            if (!SetValueToRegistry("Application", "PurgeTracking", $"{value}", "SOFTWARE\\Hewlett-Packard\\DPR\\"))
            {
                throw new WebException("Unable to Configure Purge Checkbox");
            }
            Framework.Logger.LogInfo("Purge Checkbox Configured Successfully.");
        }

        ///<summary>
        ///Enables the Encryption at rest
        ///</summary>
        /// <value><c>true</c> if Encryption  should be enabled; otherwise, <c>false</c>.</value>
        public void EnableEncryption(bool value)
        {
            if (!SetValueToRegistry("Application", "Encryption", $"{value}", "SOFTWARE\\Hewlett-Packard\\DPR\\"))
            {
                throw new WebException("Unable to Configure Encryption");
            }
            Framework.Logger.LogInfo("Encryption Configured Successfully.");
        }

        /// <summary>
        ///Enables the Quota Copies
        /// </summary>
        /// <value><c>true</c> if Quota Copies should be enabled; otherwise, <c>false</c>.</value>
        public void EnableQuotaCopies(bool value)
        {
            if (!SetValueToRegistry("MIPA", "Quota", $"{value}", "SOFTWARE\\Hewlett-Packard\\HP ACJA\\"))
            {
                throw new WebException("Unable to Configure Quota Copies");
            }
            Framework.Logger.LogInfo("QutaCopies Checkbox Configured Successfully.");
        }

        /// <summary>
        ///Enables the Copies Tracking
        /// </summary>
        /// <value><c>true</c> if Copies Tracking should be enabled; otherwise, <c>false</c>.</value>
        public void EnableCopiesTracking(bool value)
        {
            if (!SetValueToRegistry("Application", "CopiesTracking", $"{value}", "SOFTWARE\\Hewlett-Packard\\HP ACJA\\"))
            {
                throw new WebException("Unable to Configure Copies Tracking");
            }
            Framework.Logger.LogInfo("Copies Tracking Configured Successfully.");
        }

        /// <summary>
        ///Enables the DSTracking
        /// </summary>
        /// <value><c>true</c> if DSTracking should be enabled; otherwise, <c>false</c>.</value>
        public void EnableDSTracking(bool value)
        {
            if (!SetValueToRegistry("Application", "DSTracking", $"{value}", "SOFTWARE\\Hewlett-Packard\\HP ACJA\\"))
            {
                throw new WebException("Unable to Configure Digital Send Tracking");
            }
            Framework.Logger.LogInfo("Digital Send Tracking Configured Successfully.");
        }

        /// <summary>
        ///Updates User Editor
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="code">code</param>
        public void UpdateUserEditor(string user, int code)
        {
            var loadResponse = ExecuteWebService(_serviceUrl, "AD-Authenticator/AD-Configurator.aspx", "LoadUser", $"Field=username&Value={user}");
            var saveResponse = ExecuteWebService(_serviceUrl, "AD-Authenticator/AD-Configurator.aspx", "SaveUser", $"Locator=LDAP//10.11.14.95/CN=Administrator,CN=Users,DC=RDLPS1,DC=COM&Code={code},Username=Administrator&FullName=Administrator,Domain=RDLPS1&Email=Administrator@rdlps1.com&Group=&Home=");
            if (loadResponse.StatusCode != HttpStatusCode.OK && saveResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Fail to Update User");
            }
            Framework.Logger.LogInfo("User updated Successfully.");
        }

        private static HttpWebResponse PostRequest(string url, string postData)
        {
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            webRequest.Proxy = null;
            webRequest.UseDefaultCredentials = true; // Uses Windows Credentials to verify user

            byte[] bytes = Encoding.ASCII.GetBytes(postData);

            // Send the Post
            webRequest.ContentLength = bytes.Length;
            using (Stream objectStream = webRequest.GetRequestStream())
            {
                objectStream.Write(bytes, 0, bytes.Length);
            }

            try
            {
                // Get the response
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                return webResponse;
            }
            catch (WebException e)
            {
                throw new WebException($"Unable to get Response for {webRequest.RequestUri}, {e.Message}");
            }
        }

        private static int PostSoapRequest(string url, string postData, Dictionary<string, string> headers)
        {
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.ContentType = "text/xml;charset=utf-8";
            webRequest.Method = "POST";
            webRequest.Proxy = null;
            webRequest.UseDefaultCredentials = true; // Uses Windows Credentials to verify user

            foreach (var header in headers)
            {
                webRequest.Headers.Add(header.Key, header.Value);
            }

            byte[] bytes = Encoding.ASCII.GetBytes(postData);

            // Send the Post
            webRequest.ContentLength = bytes.Length;
            using (Stream objectStream = webRequest.GetRequestStream())
            {
                objectStream.Write(bytes, 0, bytes.Length);
            }

            // Get the response
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            return (int)webResponse.StatusCode;
        }

        private int ExecuteAuthenticationWebService(string url, string service, string function, string args)
        {
            // NetworkCredential credential = new NetworkCredential("u03000", "1qaz2wsx", "etl.boi.rd.hpicorp.net");
            string authurl = $"{url}/{service}/{function}";
            WebRequest webRequest = WebRequest.Create(authurl);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "GET";
            webRequest.Proxy = null;
            webRequest.Credentials = UserCredential;
            webRequest.PreAuthenticate = true;
            webRequest.UseDefaultCredentials = true;
            //webRequest.Credentials = CredentialCache.DefaultNetworkCredentials;
            WebHeaderCollection headers = new WebHeaderCollection
            {
                {"Accept", "text / html, application / xhtml + xml, */*"},
                {"Accept-Encoding", "gzip, deflate"},
                {"Accept-Language", "en - US"}
            };
            var plainTextBytes = Encoding.UTF8.GetBytes(UserCredential.Domain + UserCredential.UserName + UserCredential.Password);
            string authorization = Convert.ToBase64String(plainTextBytes);
            headers.Add(HttpRequestHeader.Authorization, "Basic " + authorization + "");
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            string sessionId = string.Empty;
            if (webResponse.Headers.GetValues("Set-Cookie") != null)
            {
                sessionId = (webResponse.Headers.GetValues("Set-Cookie").FirstOrDefault().Split(';').FirstOrDefault());
            }
            // Send the Post
            return 200;
        }

        private HttpWebResponse ExecuteNestedService(string url, string inputXml, string methodname)
        {
            HttpWebRequest request = CreateWebRequest(url, methodname);
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(inputXml);
            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();

            return webResponse;
        }

        /// <summary>
        /// Create a soap webrequest to [Url]
        /// </summary>
        /// <returns></returns>
        private HttpWebRequest CreateWebRequest(string url, string methodName)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add($@"SOAPAction:{methodName}");
            webRequest.ContentType = "text/xml; charset=utf-8";
            webRequest.Headers.Add("AcceptEncoding", "gzip, deflate");
            webRequest.Method = "POST";
            return webRequest;
        }

        private bool ConfigureQuota(string printerName, string value, string serverName)
        {
            string soapMessage = string.Format(CultureInfo.CurrentCulture, HpacResource.SoapRequestBody, $"<adm:ConfigureQuota><adm:printername>{printerName}</adm:printername><adm:value>{value}</adm:value><adm:tbQuotaServerName>{_serverName}</adm:tbQuotaServerName></adm:ConfigureQuota>");

            var webRequest = CreateWebRequest($"{_serviceUrl}/AdminConsoleWS/AdminConsole.asmx", "http://adminconsolews.capellatech.com/ConfigureQuota");
            string returnValue;
            if (PostSoapMessage(soapMessage, webRequest, out returnValue))
            {
                if (returnValue == "1")
                    return true;
            }

            return false;
        }

        private bool InstallAgentRequest(string deviceIp, string bundleFile)
        {
            string soapMessage = string.Format(CultureInfo.CurrentCulture, HpacResource.SoapRequestBody,
                $"<adm:Print9100><adm:deviceHost>{deviceIp}</adm:deviceHost><adm:model>FutureSmart</adm:model><adm:install>true</adm:install><adm:jobAbsPath>{bundleFile}</adm:jobAbsPath><adm:message>Installing Agent</adm:message></adm:Print9100>");
            var webRequest = CreateWebRequest($"{_serviceUrl}/AdminConsoleWS/AdminConsole.asmx", "http://adminconsolews.capellatech.com/Print9100");
            webRequest.Timeout = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
            string returnValue;
            if (PostSoapMessage(soapMessage, webRequest, out returnValue))
            {
                if (returnValue == "true")
                    return true;
            }
            return false;
        }

        private string GetValueFromRegistry(string name, string key, string path)
        {
            string soapMessage = string.Format(CultureInfo.CurrentCulture, HpacResource.SoapRequestBody, $"<adm:GetValueFromRegistry><adm:name>{name}</adm:name><adm:key>{key}</adm:key><adm:path>{path}</adm:path></adm:GetValueFromRegistry>");

            var webRequest = CreateWebRequest($"{_serviceUrl}/AdminConsoleWS/AdminConsole.asmx", "http://adminconsolews.capellatech.com/GetValueFromRegistry");

            string returnValue;

            return PostSoapMessage(soapMessage, webRequest, out returnValue) ? returnValue : string.Empty;
        }

        private string GetValueFromPrinterRegistry(string printerName, string key)
        {
            string soapMessage = string.Format(CultureInfo.CurrentCulture, HpacResource.SoapRequestBody,
                $"<adm:GetValueFromPrinterRegistry><adm:printername>{printerName}</adm:printername><adm:keyname>{key}</adm:keyname></adm:GetValueFromPrinterRegistry>");

            var webRequest = CreateWebRequest($"{_serviceUrl}/AdminConsoleWS/AdminConsole.asmx", "http://adminconsolews.capellatech.com/GetValueFromPrinterRegistry");
            string returnValue;

            return PostSoapMessage(soapMessage, webRequest, out returnValue) ? returnValue : string.Empty;
        }

        private bool SetValueToRegistry(string name, string key, string value, string path)
        {
            string soapMessage = string.Format(CultureInfo.CurrentCulture, HpacResource.SoapRequestBody,
                $"<adm:SetValueToRegistry><adm:name>{name}</adm:name><adm:key>{key}</adm:key><adm:value>{value}</adm:value><adm:path>{path}</adm:path></adm:SetValueToRegistry>");

            var webRequest = CreateWebRequest($"{_serviceUrl}/AdminConsoleWS/AdminConsole.asmx", "http://adminconsolews.capellatech.com/SetValueToRegistry");

            string temp;
            if (PostSoapMessage(soapMessage, webRequest, out temp))
            {
                return value == GetValueFromRegistry(name, key, path);
            }
            return false;
        }

        private bool SetValueToPrinterRegistry(string printerName, string key, string value)
        {
            string soapMessage = string.Format(CultureInfo.CurrentCulture, HpacResource.SoapRequestBody,
                $"<adm:iSetValueToPrinterRegistry><adm:printername>{printerName}</adm:printername><adm:keyname>{key}</adm:keyname><adm:value>{value}</adm:value></adm:iSetValueToPrinterRegistry>");

            var webRequest = CreateWebRequest($"{_serviceUrl}/AdminConsoleWS/AdminConsole.asmx", "http://adminconsolews.capellatech.com/iSetValueToPrinterRegistry");

            string temp;
            if (PostSoapMessage(soapMessage, webRequest, out temp))
            {
                return value == GetValueFromPrinterRegistry(printerName, key);
            }
            return false;
        }

        private bool PostSoapMessage(string soapMessage, HttpWebRequest webRequest, out string value)
        {
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(soapMessage);
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                if (webResponse.StatusCode == HttpStatusCode.OK)
                {
                    value = XElement.Load(webResponse.GetResponseStream()).Value;
                    return true;
                }
            }
            value = string.Empty;
            return false;
        }

        private void RetrieveSessionId()
        {
            if (!string.IsNullOrEmpty(jaLoginSessionId))
            {
                jaLoginSessionId = jaLoginSessionId.Split(';').FirstOrDefault();
                jaLoginSessionId = jaLoginSessionId?.Replace("sessionId=", string.Empty);
            }
        }

        private List<string> GetPrinterInfo(PrintDeviceInfo deviceInfo)
        {
            List<string> printerInfo = new List<string>();
            try
            {
                var device = DeviceConstructor.Create(deviceInfo);
                if (device is JediOmniDevice)
                {
                    var jediDevice = device as JediOmniDevice;
                    var macId = jediDevice.Snmp.Get("1.3.6.1.4.1.11.2.4.3.1.23.0");
                    var macids = macId.Select(x => $"{Convert.ToInt32(x):X}");

                    printerInfo.Add(string.Join(":", macids));
                    printerInfo.Add(((JediOmniDevice)device).Snmp.Get(".1.3.6.1.4.1.2699.1.2.1.2.1.1.2.1"));
                    printerInfo.Add(((JediOmniDevice)device).Snmp.Get(".1.3.6.1.2.1.43.5.1.1.16.1"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving Printing Information", ex.InnerException);
            }
            return printerInfo;
        }

        private void EnablePJL(PrintDeviceInfo deviceInfo)
        {
            JediDevice device = new JediDevice(deviceInfo.Address, deviceInfo.AdminPassword);
            string urn = "urn:hp:imaging:con:service:security:SecurityService";
            string endpoint = "security";

            WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
            string oldValue = tic.FindElement("PjlDeviceAccess").Value;
            if (oldValue == "disabled")
            {
                tic.FindElement("PjlDeviceAccess").SetValue("enabled");
                device.WebServices.PutDeviceTicket(endpoint, urn, tic);
            }

            oldValue = tic.FindElement("PJLFileSystemExternalAccess").Value;
            if (oldValue == "disabled")
            {
                tic.FindElement("PJLFileSystemExternalAccess").SetValue("enabled");
                device.WebServices.PutDeviceTicket(endpoint, urn, tic);
            }
        }

        private void DisablePJL(PrintDeviceInfo deviceInfo)
        {
            JediDevice device = new JediDevice(deviceInfo.Address, deviceInfo.AdminPassword);
            string urn = "urn:hp:imaging:con:service:security:SecurityService";
            string endpoint = "security";

            WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
            string oldValue = tic.FindElement("PjlDeviceAccess").Value;
            if (oldValue == "enabled")
            {
                tic.FindElement("PjlDeviceAccess").SetValue("disabled");
                device.WebServices.PutDeviceTicket(endpoint, urn, tic);
            }

            oldValue = tic.FindElement("PJLFileSystemExternalAccess").Value;
            if (oldValue == "enabled")
            {
                tic.FindElement("PJLFileSystemExternalAccess").SetValue("disabled");
                device.WebServices.PutDeviceTicket(endpoint, urn, tic);
            }
        }
    }
}