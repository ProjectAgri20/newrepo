using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.Threading;
using HP.DeviceAutomation;

using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using HP.ScalableTest.Email;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Xml;
using HP.ScalableTest.Framework.PluginService;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Core.Security;

namespace HP.ScalableTest.Development
{
    public class KellyTest
    {
        private static IEmailController _emailController = null;
        private static StringBuilder _operationLog = new StringBuilder();

        public KellyTest()
        {
            //Load Dev settings
            //Prod: 15.86.232.53
            //STBServer: 15.86.232.90
            GlobalSettings.Load("15.86.232.90");
            GlobalSettings.IsDistributedSystem = false;
            FrameworkServicesInitializer.InitializeExecution();
        }

        public void Go()
        {
            string errorMessage = "System.AggregateException: One or more errors occurred. ---> System.ArgumentOutOfRangeException: Index and length must refer to a location within the string.";
            Console.WriteLine(errorMessage.Substring(0, Math.Min(errorMessage.Length, 60)));

        }

        /// <summary>
        /// Generates a new Sequential Guid.
        /// </summary>
        public void PrintNewId()
        {
            System.Diagnostics.Debug.WriteLine(SequentialGuid.NewGuid());
        }

        private void LogPerformance(int iterations)
        {
            try
            {
                PluginConfigurationData configData = PluginConfigurationData.LoadFromFile("C:\\Work\\PluginConfig\\GFriendDataBlank.plugindata");
                PluginExecutionData executionData = CreateExecutionData(configData);
                DeviceWorkflowLogger workflowLogger = new DeviceWorkflowLogger(executionData);

                for (int i = 0; i < iterations; i++)
                {
                    workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
            }
        }

        private PluginExecutionData CreateExecutionData(PluginConfigurationData configurationData)
        {
            if (configurationData == null)
            {
                throw new ArgumentNullException(nameof(configurationData));
            }

            PluginExecutionContext executionContext = new PluginExecutionContext
            {
                ActivityExecutionId = new Guid("DB0F4D03-3C83-43FB-A169-327BAFE47B7D"),
                SessionId = "EQWX9L4M",
                UserName = "u05401",
                UserPassword = "1qaz2wsx"
            };

            return new PluginExecutionData
            (
                configurationData.GetMetadata(),
                configurationData.MetadataVersion,
                new AssetInfoCollection(new List<AssetInfo>()),
                new DocumentCollection(new List<Document>()),
                new ServerInfoCollection(new List<ServerInfo>()),
                new PrintQueueInfoCollection(new List<PrintQueueInfo>()),
                new PluginEnvironment(new SettingsDictionary(new Dictionary<string, string>()), GlobalSettings.Items[Setting.Domain], GlobalSettings.Items[Setting.DnsDomain]),
                executionContext,
                new PluginRetrySettingDictionary(new List<PluginRetrySetting>())
            );
        }


        private void ContractFactory_OnStatusChanged(object sender, ScalableTest.Utility.StatusChangedEventArgs e)
        {
            TraceFactory.Logger.Debug($"ContractFactory.Create::{e.StatusMessage}");
        }

        public void TestAutoDiscover()
        {
            // Prompt for inputs
            Console.WriteLine("Enter User Name:");
            string userName = Console.ReadLine();
            Console.WriteLine("Enter User Password:");
            string userPassword = Console.ReadLine();
            Console.WriteLine("Enter Domain:");
            string domain = Console.ReadLine();

            try
            {
                HP.ScalableTest.TraceFactory.Logger.Debug($"Autodiscovering Exchange Server: {userName}@{domain}");
                ExchangeEmailController controller = new ExchangeEmailController(new NetworkCredential(userName, userPassword), new MailAddress($"{userName}@{domain}"));

                //NetworkCredential credential = new NetworkCredential(userName, userPassword);
                //MailAddress address = new MailAddress($"{userName}@{domain}");
                //var uri = ExchangeEmailController.AutodiscoverExchangeUrl(address);
                //Console.WriteLine(uri.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void AuthenticateOmni(string deviceIP, string userName)
        {
            IDevice device = DeviceFactory.Create(deviceIP, "!QAZ2wsx");
            //AuthenticationCredential credential = new AuthenticationCredential(userName, "1qaz2wsx", "etl.boi.rd.hpicorp.net");
            AuthenticationCredential credential = new AuthenticationCredential("03000");

            JediOmniPreparationManager prepMgr = new JediOmniPreparationManager(((JediOmniDevice)device));
            JediOmniLaunchHelper helper = new JediOmniLaunchHelper((JediOmniDevice)device);

            // Set up the device for Authentication
            prepMgr.Reset();
            helper.PressSignInButton();

            IAuthenticator authenticator = AuthenticatorFactory.Create(device, credential, AuthenticationProvider.Auto);
            authenticator.Authenticate();
        }

        private void AuthenticateWindjammer(string deviceIP, string userName)
        {
            IDevice device = DeviceFactory.Create(deviceIP, "!QAZ2wsx");
            AuthenticationCredential credential = new AuthenticationCredential(userName, "1qaz2wsx", "etl.boi.rd.hpicorp.net");

            JediWindjammerPreparationManager prepMgr = new JediWindjammerPreparationManager(((JediWindjammerDevice)device));
            JediWindjammerControlPanel controlPanel = ((JediWindjammerDevice)device).ControlPanel;

            // Set up the device for Authentication
            //prepMgr.Reset();
            controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);

            //IEnumerable<string> controls = controlPanel.GetControls();
            //foreach (string s in controls)
            //{
            //    System.Diagnostics.Debug.WriteLine(s);
            //}    


            IAuthenticator authenticator = AuthenticatorFactory.Create(device, credential, AuthenticationProvider.Auto);
            authenticator.Authenticate();
        }

        private static void Autodiscover(string userName, string domain)
        {
            NetworkCredential credential = new NetworkCredential(userName, domain);
            HP.ScalableTest.TraceFactory.Logger.Debug("Autodiscovering...");
            HP.ScalableTest.TraceFactory.Logger.Debug("User:" + userName);
            HP.ScalableTest.TraceFactory.Logger.Debug("Domain: " + domain);

            MailAddress address = ExchangeEmailController.GetLdapEmailAddress(credential);
            var uri = ExchangeEmailController.AutodiscoverExchangeUrl(address);
            HP.ScalableTest.TraceFactory.Logger.Debug("Autodiscovered URL:" + uri.ToString());

            _emailController = new ExchangeEmailController(credential, uri);

        }

        private int ExecuteAuthenticationWebService(string url, string service, string function, string args)
        {
            NetworkCredential credential = new NetworkCredential("u03000", "1qaz2wsx", "etl.boi.rd.hpicorp.net");
            string authurl = "{0}/{1}/{2}".FormatWith(url, service, function);
            WebRequest webRequest = WebRequest.Create(authurl);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "GET";
            webRequest.Proxy = null;
            webRequest.Credentials = credential;
            webRequest.PreAuthenticate = true;
            webRequest.UseDefaultCredentials = true;
            //webRequest.Credentials = CredentialCache.DefaultNetworkCredentials;
            WebHeaderCollection headers = new WebHeaderCollection();
            headers.Add("Accept", "text / html, application / xhtml + xml, */*");
            headers.Add("Accept-Encoding", "gzip, deflate");
            headers.Add("Accept-Language", "en - US");
            var plainTextBytes = Encoding.UTF8.GetBytes(credential.Domain + credential.UserName + credential.Password);
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

        private void SendEmail(string sender, string recipient)
        {
            using (MailMessage message = new MailMessage(sender, recipient))
            {
                message.Subject = "Email from: " + sender;
                message.BodyEncoding = System.Text.Encoding.ASCII;
                message.Body = "Trying to get the Exchange Server to break.";
                //Add Tracker info to the email header
                EmailTracker.Tag(message);

                // Send the Email.
                _emailController.Send(message);
                _operationLog.Append("Sent email from: ").Append(sender);
                _operationLog.Append(" to: ").AppendLine(recipient);
            }
        }

        private void ReceiveEmail(string recipient)
        {
            Collection<EmailMessage> messages = _emailController.RetrieveMessages(EmailFolder.Inbox);

            foreach (EmailMessage email in messages)
            {
                _operationLog.Append("Received email from: ").Append(email.FromAddress.Address);
                _operationLog.Append(" to: ").AppendLine(recipient);
            }

            _operationLog.Append("Clearing out Inbox for ").AppendLine(recipient);
            _emailController.Clear(EmailFolder.Inbox);
        }

        //private void TestBackChannel()
        //{
        //    using (var proxy = SessionProxyBackendConnection.Create("15.198.216.75", "1232212F"))
        //    {
        //        proxy.Channel.SaveLogFiles(LogFileDataCollection.Create("C:\\Temp"));
        //    }
        //}

        private static void WriteToFile(XElement serialized, string fileName)
        {
            string file = $"{fileName}.xml";
            File.WriteAllText(file, serialized.ToString());
        }

        private static XElement ReadFromFile(string fileName)
        {
            //string file = $"{fileName}.xml";
            string text = File.ReadAllText(fileName);
            return XElement.Parse(text);
        }

        public void ParseJobXml(string xmlJobInfo)
        {
            ParseJobXml(XmlUtil.CreateXDocument(xmlJobInfo));
        }

        public void ParseJobXml(XDocument xmlJobInfo)
        {
            string jobType = string.Empty;
            string userName = string.Empty;
            string fileType = string.Empty;
            string scannedPages = string.Empty;
            string version = string.Empty;
            string processedBy = string.Empty;
            string model = string.Empty;

            XElement headerElement = xmlJobInfo.Descendants("HeaderElements").FirstOrDefault();
            model = headerElement.Element("cDeviceDescription").Value;
            //jobType = element.Element("cJobType").Value;
            //userName = element.Element("cUserName").Value;

            XElement detailsElement = xmlJobInfo.Descendants("JobLogDetails").FirstOrDefault();
            fileType = detailsElement.Element("cFileType").Value;
            scannedPages = detailsElement.Element("cPagesScanned").Value;
            version = detailsElement.Element("cDssSoftwareVersionColon").Value;
            processedBy = detailsElement.Element("cJobProcessedBy").Value;

            Console.WriteLine("FileType:{0}  ScannedPages:{1}  Version:{2}  ProcessedBy:{3}".FormatWith(fileType, scannedPages, version, processedBy));
        }

        private static XElement ConvertMetadata(XElement startMetadata, string startVersion, IEnumerable<IPluginMetadataConverter> converters)
        {
            if (converters == null)
            {
                throw new ArgumentNullException(nameof(converters));
            }

            XElement currentMetadata = startMetadata;
            string currentVersion = startVersion;
            List<string> encounteredVersions = new List<string> { startVersion };

            // Find a converter that matches the specified version
            IPluginMetadataConverter converter = converters.FirstOrDefault(n => string.Equals(currentVersion, n.OldVersion, StringComparison.OrdinalIgnoreCase));
            while (converter != null)
            {
                // Check the new version to make sure we don't convert to something we've already seen
                if (encounteredVersions.Contains(converter.NewVersion))
                {
                    return currentMetadata;
                }

                currentMetadata = converter.Convert(currentMetadata);
                currentVersion = converter.NewVersion;
                encounteredVersions.Add(converter.NewVersion);

                // Check to see if there is another converter to run
                converter = converters.FirstOrDefault(n => string.Equals(currentVersion, n.OldVersion));
            }

            // No more converters to run.  Return the end result.
            return currentMetadata;
        }

        private static void ManifestTest()
        {
            //SessionTicket ticket = SessionTicket.Create("STF_3-10_Beta - Pause Printer");

            //Collection<string> assetIds = null;
            //using (EnterpriseTestContext context = new EnterpriseTestContext())
            //{
            //    assetIds = ResourceUsageUtil.SelectAssetIds(context, ticket.ScenarioId);
            //}

            //Collection<TestAsset> testAssets = new Collection<TestAsset>();
            //using (var service = AssetInventoryController.CreateAssetInventoryClient())
            //{
            //    service.Channel.CleanUpReservations(ticket.SessionId);
            //    testAssets = service.Channel.ReserveDevices(assetIds, ticket.SessionId, string.Empty, ticket.DurationHours);
            //}

            //AssetDetailCollection assets = new AssetDetailCollection();
            //foreach (TestAsset testAsset in testAssets)
            //{
            //    assets.Add(AssetDetailCreator.CreateAssetDetail(testAsset));
            //}

            //var manifestAgent = new SystemManifestAgent(ticket, assets);
            //manifestAgent.BuildManifests(false);

            //foreach (var manifest in manifestAgent.ManifestSet)
            //{
            //    var xml = Serializer.SerializeContract<SystemManifest>(manifest);
            //}

            //var manifest = manifestAgent.ManifestSet.First();

            //var installer = new LocalPrintQueueInstaller(manifest);
            //installer.Install();

            //var manifest = manifestAgent.ManifestSet.First(x => x.ResourceType == VirtualResourceType.OfficeWorker);


            //var foo = manifest.Printers.Devices;
            //var sharedPrintDevices = manifest.Printers;
            //var assetSet = manifest.Assets.Devices;

            //manifest.Assets.SetAvailable("LTL-12734", true);
            //manifest.RefreshPrinterAvailability();

            //var printer = sharedPrintDevices["LTL-12734"] as Print.RemotePrintDevice;

            //foo = manifest.Printers.Devices;

            //PrintDeviceDetail device = manifest.Assets["LTL-12734"] as PrintDeviceDetail;
            //System.Diagnostics.Debug.WriteLine(device.ToString());

            //foreach (var metadataDetail in resource.MetadataDetails.Cast<LoadTesterMetadataDetail>())
            //{
            //    ResourceMetadataDetail d = metadataDetail;

            //    var d1 = d as LoadTesterMetadataDetail;

            //    var t = new LoadTestThread(metadataDetail, TimeSpan.Zero);
            //}

            //var details = manifest.Resources.OfType<OfficeWorkerDetail>().SelectMany(x => x.MetadataDetails).Distinct(x => x.MetadataType);
            //var pluginList = details.ToList();
            //foreach (var detail in details)
            //{
            //    var activity = new Activity(detail);

            //    if (activity.Plugin is IPluginSetup)
            //    {
            //        ((IPluginSetup)activity.Plugin).Setup(manifest);
            //    }
            //}

            ////var manifest = manifestAgent.ManifestSet[0];
            ////VirtualResourceHandlerFactory.Create(manifest);
            ////var xml = Serializer.SerializeContract<SystemManifest>(manifest);
            ////manifest.PushToGlobalDataStore();
            ////manifest.PushToGlobalSettings();

            ////XmlDocument doc = new XmlDocument();
            ////doc.LoadXml(manifest.Resources[0].MetadataDetails[0].Data);

            ////EpicPrintJobExecutionController controller = new EpicPrintJobExecutionController();
            ////controller.ProcessActivity(doc);

            //DomainAccountService.Release(ticket.SessionId);

        }




        private void LoadSystemSettings(string manifestFilePath)
        {
            SystemManifest systemManifest = LegacySerializer.DeserializeDataContract<SystemManifest>(GetFileContents(manifestFilePath));
            GlobalDataStore.Load(systemManifest);
        }

        private string GetFileContents(string filePath)
        {
            string content = string.Empty;
            using (FileStream fileStream = System.IO.File.OpenRead(filePath))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    content = reader.ReadToEnd();
                }
            }
            return content;
        }

    } //KellyTest

    public class RepositoryUri
    {
        private List<string> _parsedPath = new List<string>();

        public RepositoryUri(string repositoryPath)
        {
            foreach (string pathPart in repositoryPath.Split('\\'))
            {
                if (pathPart.Trim() != string.Empty)
                {
                    _parsedPath.Add(pathPart);
                }
            }
        }

        public string Host
        {
            get { return GetDirectory(0); }
        }

        public string LastDirectory
        {
            get { return GetDirectory(_parsedPath.Count - 1); }
        }

        public int DirectoryCount
        {
            get { return _parsedPath.Count; }
        }

        public string FullPath
        {
            get
            {
                StringBuilder result = new StringBuilder("\\\\");
                _parsedPath.ForEach(x => result.Append(x).Append("\\"));
                return result.ToString(0, result.Length - 1);
            }
        }

        public string GetDirectory(int index)
        {
            if (index < _parsedPath.Count)
            {
                return _parsedPath[index];
            }

            return string.Empty;
        }

        public string GetPartialPath(int index)
        {
            StringBuilder result = new StringBuilder("\\\\");
            for (int i = 0; i < index; i++)
            {
                result.Append(_parsedPath[i]).Append("\\");
            }
            return result.ToString(0, result.Length - 1);
        }
    }
}
