using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Lock;
using HP.ScalableTest.Email;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Framework.Automation.EmailMonitor
{
    /// <summary>
    /// Class that subscribes to and handles push notifications from the Email server.
    /// </summary>
    public static class ExchangeEmailMonitor
    {
        private static Dictionary<string, IEmailController> _controllers = new Dictionary<string, IEmailController>();
        private static List<IEmailAnalyzer> _analyzers = new List<IEmailAnalyzer>();
        private static Uri _exchangeUrl = null;
        private static SettingsDictionary _exchangeServerSettings = null;

        /// <summary>
        /// Static constructor.  Add any new analyzer instances here.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static ExchangeEmailMonitor()
        {
            _analyzers.Add(new EmailAnalyzer());

            // Retrieve Exchange Server Settings
            AssetInventoryConnectionString connectionString = new AssetInventoryConnectionString(GlobalSettings.Items[Setting.AssetInventoryDatabase]);
            using (AssetInventoryContext context = new AssetInventoryContext(connectionString))
            {
                string serverType = ServerType.Exchange.ToString();
                FrameworkServer server = context.FrameworkServers.FirstOrDefault(n => n.ServerTypes.Any(m => m.Name == serverType) && n.Active);
                _exchangeServerSettings = new SettingsDictionary(server.ServerSettings.ToDictionary(n => n.Name, n => n.Value));
            }

        }
        
        /// <summary>
        /// Subscribes to Exchange-generated events that monitor new emails for the specified user.
        /// </summary>
        /// <param name="credential"></param>
        public static void Subscribe(NetworkCredential credential)
        {
            if (credential == null)
            {
                throw new ArgumentNullException("credential");
            }

            if (_exchangeUrl == null)
            {
                Initialize(credential);
            }

            if (!_controllers.ContainsKey(credential.UserName))
            {
                IEmailController controller = new ExchangeEmailController(credential, _exchangeUrl);
                _controllers.Add(credential.UserName, controller);

                //Start a background thread to clean out all folders for this user and subscribe to Exchange events.
                //ThreadPool.QueueUserWorkItem(Subscribe_CallBack, controller);
            }

        }

        /// <summary>
        /// Gets an EmailController for the specified userName.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEmailController GetEmailController(string userName)
        {
            return _controllers[userName];
        }

        /// <summary>
        /// Initializes the Exchange version and the URL so we don't have to use autodiscover for all the subscribers.
        /// </summary>
        /// <param name="credential"></param>
        private static void Initialize(NetworkCredential credential)
        {
            ExchangeConnectionSettings settings = new ExchangeConnectionSettings(_exchangeServerSettings);
            if (settings.AutodiscoverEnabled)
            {
                Action action = new Action(() =>
                {
                    TraceFactory.Logger.Debug("Autodiscover lock acquired.  Autodiscovering the Exchange Server.");
                    MailAddress autodiscoverAddress = ExchangeEmailController.GetLdapEmailAddress(credential);
                    _exchangeUrl = ExchangeEmailController.AutodiscoverExchangeUrl(autodiscoverAddress);
                });

                TraceFactory.Logger.Debug("Attempting to autodiscover against Exchange");
                CriticalSection criticalSection = new CriticalSection(new DistributedLockManager(GlobalSettings.WcfHosts["Lock"]));
                criticalSection.Run(new LocalLockToken("ExchangeAutodiscover", new TimeSpan(0, 5, 0), new TimeSpan(0, 5, 0)), action);
            }
            else
            {
                TraceFactory.Logger.Debug("Configuring exchange service using Exchange Web Services URL.");
                _exchangeUrl = settings.EwsUrl;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CheckEmail()
        {
            foreach (string key in _controllers.Keys)
            {
                var controller = _controllers[key];

                foreach (var exchangeReceivedEmail in controller.RetrieveMessages(EmailFolder.Inbox))
                {
                    foreach (IEmailAnalyzer analyzer in _analyzers)
                    {
                        if (analyzer.AnalyzeMessage(exchangeReceivedEmail))
                        {
                            analyzer.ProcessMessage(exchangeReceivedEmail, controller, key);
                            break;
                        }
                    }

                }
            }
        }

    }
}
