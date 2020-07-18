using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Xml;
using System.Net;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Properties;
using HP.ScalableTest.Framework.ClientController.EventLogCollector;
using HP.ScalableTest.Utility;
using System.DirectoryServices.AccountManagement;
using HP.ScalableTest.WindowsAutomation;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// This is the main controller that runs on STF client VMs. It communicates with the 
    /// Dispatcher through the <see cref="SessionProxyBackendConnection"/> to register
    /// and receive its <see cref="SystemManifest"/>.  It uses the manifest to 
    /// load <see cref="GlobalSettings"/>, the <see cref="GlobalDataStore"/> and ultimately
    /// create a VirtualResource from the defined manifest.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class VirtualClientController
    {
        private string _sessionId = string.Empty;
        private Collection<VirtualResourceHandler> _handlers = null;
        ///private EventLogCollectorHandler _eventLogHandler = null;
        private EventLogCollector _eventLogCollector = null;
        private readonly int _installerStartIndex = 0;
        private readonly string _dispatcherAddress = string.Empty;
        private SystemManifest _manifest = null;
        private string _installerMessage = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualClientController" /> class.
        /// </summary>
        /// <param name="dispatcherAddress">The dispatcher IP Address.</param>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="installerStartIndex">The installer start index.</param>
        public VirtualClientController(string dispatcherAddress, int installerStartIndex)
        {
            _dispatcherAddress = dispatcherAddress;
            _installerStartIndex = installerStartIndex;
        }

        /// <summary>
        /// Starts this instance of the <see cref=" VirtualClientController"/> to work with the Dispatcher,
        /// then creates the virtual resources based on the provided <see cref="SystemManifest"/>.
        /// </summary>
        /// <remarks>
        /// Creates a <see cref="SessionProxyBackendConnection"/> that is used to communicate with the
        /// Dispatcher to provide status updates.  Loads the <see cref="SystemManifest"/>  into
        /// the <see cref="GlobalDataStore"/> and into the <see cref="GlobalSettings"/>.  
        /// </remarks>
        public void Start(string sessionId)
        {
            _sessionId = sessionId;

            // Contact the dispatcher and register.
            string manifestData = string.Empty;

            TraceFactory.Logger.Debug("Registering with Dispatcher {0}, SessionId: {1}".FormatWith(_dispatcherAddress, _sessionId));
            using (var proxyClient = SessionProxyBackendConnection.Create(_dispatcherAddress, _sessionId))
            {
                manifestData = proxyClient.Channel.RegisterMachine(Environment.MachineName);
            }

            _manifest = SystemManifest.Deserialize(manifestData);
            string resourceInstanceId = string.Empty;

            switch (_manifest.ResourceType)
            {
                case VirtualResourceType.AdminWorker:
                case VirtualResourceType.OfficeWorker:
                case VirtualResourceType.SolutionTester:
                    // These types don't use the resource instance Id as there can be multiple instances
                    // of the worker within the client controller process.
                    _manifest.PushToGlobalDataStore();

                    //The following code monitors the local host VM client event logs.
                    if (_manifest.CollectEventLogs)
                    {
                        EventLogCollectorDetail detail = new EventLogCollectorDetail();
                        detail.ComponentsData = Resources.ClientEventLogComponents;
                        detail.EntryTypesData = Resources.ClientEventLogEntryTypes;
                        detail.Description = "Client VM Event Collector";
                        detail.Enabled = true;
                        detail.HostName = Environment.MachineName;
                        detail.Name = Environment.MachineName;
                        detail.ResourceType = VirtualResourceType.EventLogCollector;
                        detail.PollingInterval = 15;
                        _manifest.Resources.Add(detail);
                        resourceInstanceId = _manifest.Resources.OfType<EventLogCollectorDetail>().First().HostName;
                        _manifest.PushToGlobalDataStore(resourceInstanceId);
                    }

                    break;
                case VirtualResourceType.CitrixWorker:
                case VirtualResourceType.LoadTester:
                    // These resources use credentials like the workers above, but run in the
                    // client controller process, so the data store needs the resource instance Id set.
                    resourceInstanceId = _manifest.Resources.Credentials.First().ResourceInstanceId;
                    _manifest.PushToGlobalDataStore(resourceInstanceId);
                    break;
                case VirtualResourceType.EventLogCollector:
                    // This resource is unique as it uses the hostname as its unique name.  It also
                    // runs within the client controller process so it needs its instance Id set.
                    // This event log collector monitors target machines rather than the local VM running the client controller
                    resourceInstanceId = _manifest.Resources.OfType<EventLogCollectorDetail>().First().HostName;
                    _manifest.PushToGlobalDataStore(resourceInstanceId);
                    break;
                default:
                    // All other resources also run in the client controller process, but they use Name
                    // as their unique id.
                    var type = GetDetailType(_manifest.ResourceType);
                    resourceInstanceId = _manifest.Resources.Where(x => x.GetType().Equals(type)).First().Name;
                    TraceFactory.Logger.Debug("Resource Instance Id: {0}".FormatWith(resourceInstanceId));
                    _manifest.PushToGlobalDataStore(resourceInstanceId);
                    break;
            }

            TraceFactory.Logger.Debug("Resource Instance Id: {0}".FormatWith(resourceInstanceId));

            _manifest.PushToGlobalSettings();

            GlobalSettings.SetDispatcher(_dispatcherAddress);

            TraceFactory.Logger.Debug(_manifest.ToString());

            InstallPrintingCertficates();
            InstallClientSoftware();

            // Creates all the virtual resources specified in the provided manifest.
            _handlers = VirtualResourceHandlerFactory.Create(_manifest);
            foreach (var handler in _handlers)
            {
                handler.Start();
            }
        }

        private static Type GetDetailType(VirtualResourceType thisType)
        {
            string typeName = "HP.ScalableTest.Framework.Manifest.{0}Detail".FormatWith(thisType);
            return Type.GetType(typeName);
        }

        /// <summary>
        /// Stops this controller which ensures dependent resources are properly disposed.
        /// </summary>
        public void Stop()
        {
            if (_handlers != null)
            {
                foreach (var hand in _handlers)
                {
                    hand.Dispose();
                }
            }
            if (_eventLogCollector != null)
            {
                _eventLogCollector.Stop();
            }
        }

        /// <summary>
        /// Selects all of the Printing activities from the manifest and determines which (if any) use local print queues.
        /// Certificates are installed for each local print queue.
        /// </summary>
        protected virtual void InstallPrintingCertficates()
        {
            if (_manifest.ActivityPrintQueues.Values.OfType<DynamicLocalPrintQueueInfo>().Any())
            {
                NetworkCredential adminCredential = new NetworkCredential();
                adminCredential.UserName = GlobalSettings.Items[Setting.DomainAdminUserName];
                adminCredential.Password = GlobalSettings.Items[Setting.DomainAdminPassword];
                adminCredential.Domain = Environment.UserDomainName;

                SystemSetup.Run(GlobalDataStore.Manifest.Settings[Framework.Settings.Setting.PrintingCertsInstaller], adminCredential, true);
            }
        }

        /// <summary>
        /// Iterates through the list of installers to be executed.
        /// If a reboot is required, the location of the last executed installer is persisted to the registry.
        /// Upon restart, cycles through the list until the installer startup index is found,
        /// then resumes installation.
        /// </summary>
        protected virtual void InstallClientSoftware()
        {
            bool reboot = false;
            NetworkCredential adminCredential = null;

            if (_manifest.SoftwareInstallers.Count > 0)
            {
                TraceFactory.Logger.Debug("StartIndex: {0}".FormatWith(_installerStartIndex));
                adminCredential = new NetworkCredential();
                adminCredential.UserName = GlobalSettings.Items[Setting.DomainAdminUserName];
                adminCredential.Password = GlobalSettings.Items[Setting.DomainAdminPassword];
                adminCredential.Domain = Environment.UserDomainName;
            }
            else //No Installers.  Return immediately.
            {
                return; 
            }

            // Execute the installers
            int index = -1;
            foreach (SoftwareInstallerDetail installer in _manifest.SoftwareInstallers.OrderBy(i => i.InstallOrderNumber))
            {
                index++;
                if (index >= _installerStartIndex)
                {
                    // Install the specified software, but send a status message back periodically to ensure
                    // the dispatcher doesn't think the client is hung and then tries to reboot it.

                    _installerMessage = "Installing {0}".FormatWith(installer.Description);
                    SendStatusMessage(_installerMessage);

                    using (System.Timers.Timer installerTimer = new System.Timers.Timer(TimeSpan.FromMinutes(1).TotalMilliseconds))
                    {
                        try
                        {
                            installerTimer.Elapsed += installerTimer_Elapsed;
                            installerTimer.Start();
                            SystemSetup.Run(installer.Path, installer.Arguments, adminCredential, installer.CopyDirectory);
                        }
                        finally
                        {
                            installerTimer.Stop();
                        }
                    }

                    reboot = RebootRequired(installer.RebootMode);
                    if (installer.RebootMode == RebootMode.Immediate)
                    {
                        break;
                    }
                }
            }

            //Check to see if we need to reboot
            //We may be rebooting because of an immediate reboot, or because we're finished and a deferred reboot was found.
            if (reboot)
            {
                SendStatusMessage("Install complete - rebooting");
                SetStartupRegistryValue(index);
                Reboot();
            }
        }

        private void SendStatusMessage(string message)
        {
            using (var proxyClient = SessionProxyBackendConnection.Create(_dispatcherAddress, _sessionId))
            {
                proxyClient.Channel.ChangeMachineStatusMessage(Environment.MachineName, message);
            }
        }

        private void installerTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SendStatusMessage(_installerMessage);
        }
 
        /// <summary>
        /// Determines whether a reboot is required.
        /// </summary>
        /// <param name="rebootMode"></param>
        /// <returns></returns>
        private static bool RebootRequired(RebootMode rebootMode)
        {
            return (rebootMode == RebootMode.Deferred || rebootMode == RebootMode.Immediate);
        }

        /// <summary>
        /// Sets the RunOnce registry value for client VM.
        /// </summary>
        /// <param name="lastInstalledIndex">Index value of the last-run installer</param>
        private void SetStartupRegistryValue(int lastInstalledIndex)
        {
            lastInstalledIndex++; // Increment the last index to set the correct start index at startup
            using (RegistryKey runKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", true))
            {
                runKey.SetValue("STFClientFactory", @"C:\VirtualResource\Deployment\RebootClient.cmd {0} {1} {2}".FormatWith(_dispatcherAddress, _sessionId, lastInstalledIndex.ToString(CultureInfo.InvariantCulture)));
            }
        }

        /// <summary>
        /// Reboots the VM.
        /// </summary>
        private static void Reboot()
        {
            TraceFactory.Logger.Info("Restarting...{0}".FormatWith(Environment.MachineName));
            Process.Start("shutdown.exe", "-r -t 0");

            Environment.Exit(0);
        }
    }
}
