using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Automation.OfficeWorker;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Properties;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Print;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.ServiceModel;
using System.Threading;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Handler used to create Citrix workers on a citrix server
    /// </summary>
    internal class CitrixWorkerHandler : OfficeWorkerHandler
    {
        private readonly List<CitrixQueueClientData> _printClientData = new List<CitrixQueueClientData>();
        private readonly CitrixWorkerDetail _worker;
        private readonly OfficeWorkerCredential _credential;
        private readonly TimeSpan _minStartupDelay;
        private readonly TimeSpan _maxStartupDelay;
        private readonly string _userProfile;
        private readonly string _citrixServer;
        private readonly bool _randomizeStartupDelay;

        static OfficeWorkerCredential AdminCredential
        {
            get
            {
                string adminUserName = GlobalSettings.Items[Setting.DomainAdminUserName];
                OfficeWorkerCredential credential = new OfficeWorkerCredential
                {
                    UserName = adminUserName,
                    Password = GlobalSettings.Items[Setting.DomainAdminPassword],
                    Domain = Environment.UserDomainName
                };

                return credential;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CitrixWorkerHandler" /> class.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        public CitrixWorkerHandler(SystemManifest manifest)
            : base(manifest)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException("manifest");
            }

            _userProfile = Environment.GetEnvironmentVariable("USERPROFILE");

            _credential = SystemManifest.Resources.Credentials.First();
            _credential.WorkingDirectory = Directory.GetCurrentDirectory();

            _worker = SystemManifest.Resources.OfType<CitrixWorkerDetail>().First();
            _citrixServer = _worker.ServerHostname;

            _randomizeStartupDelay = _worker.RandomizeStartupDelay;
            _minStartupDelay = TimeSpan.FromSeconds(_worker.MinStartupDelay);
            _maxStartupDelay = TimeSpan.FromSeconds(_worker.MaxStartupDelay);
        }

        /// <summary>
        /// Creates all the specified virtual resources.
        /// </summary>
        public override void Start()
        {
            // Install all print queues for the specified user.
            InstallRemotePrinters();
            InstallLocalPrinters();

            // Perform user specific setup, but don't actually start the Office Worker on the
            // Citrix server.  Wait until the ExecuteResources is called for that.  This will
            // better simulate a simultaneous load on the Citrix server.

            ChangeResourceState(_credential.ResourceInstanceId, RuntimeState.Starting);

            ChangeMachineStatusMessage("Configuring User");
            ConfigureUserGroups(_credential);
            CitrixSessionManager.ConfigureLocalUserGroups(_credential, _citrixServer);

            ChangeMachineStatusMessage("Resetting Citrix");
            CitrixSessionManager.ResetCitrixSession(_credential.UserName, _citrixServer);

            // If a value is provided for the published app, then start it first.
            if (!string.IsNullOrEmpty(_worker.PublishedApp))
            {
                CitrixSessionManager.StartPublishedApp(_credential, _citrixServer, _worker.PublishedApp);
            }

            ChangeMachineStatusMessage("Starting Citrix User");
            StartCitrixUserSession();
        }

        private new void InstallLocalPrinters()
        {
            var installer = new LocalPrintQueueInstaller(SystemManifest);
            installer.PrintQueueInstalled += installer_PrintQueueInstalled;
            installer.Install();
        }

        private void installer_PrintQueueInstalled(object sender, LocalPrintQueueInstalledEventArgs e)
        {
            // Figure out how long it will be before this worker is started.  This is important because
            // the Citrix print queue creation monitor has a cleanup timer, and it tries to talk to the
            // worker to determine if it doesn't respond.  If the worker is gone, then this will fail
            // and any registered queues being monitored will be purged.  This can also fail if the q
            // worker hasn't started yet because of this delay.  So send this information through with
            // the ClientData below so the monitor doesn't prematurely whack any registered queues.

            TimeSpan startupDelay; 
            TimeSpan minStartupDelay = _minStartupDelay;
            TimeSpan maxStartupDelay = _maxStartupDelay;
            TimeSpan timeBuffer = new TimeSpan(0, 30, 0);

            if (_randomizeStartupDelay)
            {
                startupDelay = maxStartupDelay.Add(timeBuffer);
            }
            else
            {
                startupDelay = minStartupDelay.Add(timeBuffer);
            }

            CitrixQueueClientData clientData = new CitrixQueueClientData
            {
                QueueName = e.QueueName,
                HostName = Environment.MachineName,
                UserName = _credential.UserName,
                PrintDriver = e.DriverName,
                SessionId = SystemManifest.SessionId,
                StartupDelay = startupDelay
            };

            var endpoint = new Uri("http://{0}:{1}/{2}".FormatWith(_citrixServer, _credential.Port, WcfService.VirtualResource));

            clientData.EndPoint = endpoint;
            TraceFactory.Logger.Debug("Client Endpoint {0}".FormatWith(endpoint));

            _printClientData.Add(clientData);
        }

        /// <summary>
        /// Executes the specified virtual resources.
        /// </summary>
        private void StartCitrixUserSession()
        {
            // First copy a file over by username that will be used to define this client
            // and the associated dispatcher.  The remote machine will then pick it up.
            string remotePath = @"\\{0}\C$\VirtualResource\UserConfiguration".FormatWith(_citrixServer);
            if (!Directory.Exists(remotePath))
            {
                Directory.CreateDirectory(remotePath);
            }
            TraceFactory.Logger.Debug(remotePath);

            string datFile = @"{0}\{1}.dat".FormatWith(remotePath, _credential.UserName);
            File.WriteAllText(datFile, "{0},{1}".FormatWith(Environment.MachineName, _credential.ResourceInstanceId));
            TraceFactory.Logger.Debug("Created startup information file on Citrix Server");

            TraceFactory.Logger.Debug("Creating user {0} on port {1}...".FormatWith(_credential.UserName, _credential.Port));

            TimeSpan startupDelay = _randomizeStartupDelay ? TimeSpanUtil.GetRandom(_minStartupDelay, _maxStartupDelay) : _minStartupDelay;
            Delay.Wait(startupDelay);

            TriggerQueueMonitoring();

            ChangeMachineStatusMessage("Starting User Process");
            StartUserProcess(_credential, Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Starts the defined Virtual Resource process for the defined credential.
        /// </summary>
        /// <param name="credential">The user credential.</param>
        /// <param name="currentDirectory">The current directory.</param>
        protected override void StartUserProcess(OfficeWorkerCredential credential, string currentDirectory)
        {
            if (credential == null)
            {
                throw new ArgumentNullException("credential");
            }

            Environment.SpecialFolder folder = (Environment.Is64BitOperatingSystem)
                ? Environment.SpecialFolder.ProgramFilesX86 
                : Environment.SpecialFolder.ProgramFiles;

            var exeFile = Path.Combine
                (
                    Environment.GetFolderPath(folder),
                    Resources.WFICA32Path
                );

            TraceFactory.Logger.Debug("ICA Client: {0}".FormatWith(exeFile));

            int tries = 0;
            int numTries = 5;

            while (tries < numTries)
            {
                if (CheckSessionStarted(credential, exeFile))
                {
                    break;
                }

                tries++;
                TraceFactory.Logger.Debug("Failed to start session, sleeping and trying again");

                if (!ProcessUtil.KillProcess(Resources.wfica32, currentUserOnly: false))
                {
                    TraceFactory.Logger.Debug("Failed to kill ICA process");
                }
                else
                {
                    TraceFactory.Logger.Debug("ICA process killed");
                }

                // Kill any session on the Citrix server
                CitrixSessionManager.ResetCitrixSession(credential.UserName, _citrixServer);

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

            if (tries >= numTries)
            {
                throw new ArgumentException("Unable to start Citrix process after {0} tries, for {1}".FormatWith(numTries, credential.UserName));
            }
            else
            {
                TraceFactory.Logger.Debug("Citrix session started");
            }
        }

        private bool CheckSessionStarted(OfficeWorkerCredential credential, string resourcePath)
        {
            bool sessionStarted = false;
            string appId = string.Empty;

            // The Citrix Office Worker can run on a Citrix server in two different ways, one as a published
            // app and the other under a desktop session.  This block of code defines how the ICA file
            // should be constructed in order to run the session in one of the two options.  It is based
            // on the value of the RunMode which is chosen by the user at runtime.
            switch (_worker.WorkerRunMode)
            {
                case CitrixWorkerRunMode.Desktop:
                    string key = "{0}-CitrixWorkerDesktop".FormatWith(_citrixServer);
                    appId = GlobalSettings.Items[key];

                    if (string.IsNullOrEmpty(appId))
                    {
                        throw new InvalidOperationException("The CitrixWorkerDesktop System Setting value is missing.");
                    }
                    break;

                case CitrixWorkerRunMode.PublishedApp:
                    appId = "{0}-OWC".FormatWith(_citrixServer);
                    break;
            }            

            // Start the worker process on the remote Citrix server by running the ICA client
            // as administrator, and the ICA config file contains the credentials for the the
            // actual Office Worker
            string icaFile = CitrixSessionManager.CreateIcaFile(credential, _citrixServer, appId);
            CitrixSessionManager.StartProcess(credential, resourcePath, icaFile, TimeSpan.FromSeconds(10));

            // The Citrix server will display a Citrix license warning anytime a user is not admin
            // and is running a client process.  So delay here about 10 seconds to let this dialog
            // display and eventually go away.
            TraceFactory.Logger.Info("Sleep 20 seconds to wait on Citrix to startup");
            Thread.Sleep(TimeSpan.FromSeconds(20));

            try
            {
                Retry.WhileThrowing
                    (
                        () => sessionStarted = PingWorker(credential),
                        10,
                        TimeSpan.FromSeconds(10),
                        new List<Type>() { typeof(FaultException), typeof(EndpointNotFoundException), typeof(SocketException) }
                    );
            }
            catch (FaultException ex)
            {
                TraceFactory.Logger.Debug("Ping worker failed (1): {0}".FormatWith(ex.Message));
            }
            catch (EndpointNotFoundException ex)
            {
                TraceFactory.Logger.Debug("Ping worker failed (2): {0}".FormatWith(ex.Message));
            }
            catch (SocketException ex)
            {
                TraceFactory.Logger.Debug("Ping worker failed (3): {0}".FormatWith(ex.Message));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("Ping worker failed (4): {0}".FormatWith(ex.Message));
            }
            finally
            {
                TraceFactory.Logger.Debug("Session Started: {0}".FormatWith(sessionStarted));
            }

            return sessionStarted;
        }

        /// <summary>
        /// Pings the worker.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns></returns>
        private bool PingWorker(OfficeWorkerCredential credential)
        {
            bool result = false;

            var endpoint = new Uri("http://{0}:{1}/{2}".FormatWith(_citrixServer, credential.Port, WcfService.VirtualResource));

            TraceFactory.Logger.Debug("Pinging {0}".FormatWith(endpoint));

            using (var service = VirtualResourceManagementConnection.Create(_citrixServer, credential.Port))
            {
                if (service.Channel.Ping())
                {
                    TraceFactory.Logger.Debug("Ping succeeded");
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Tells the service to perform any cleanup activities.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "The process should not be allowed to crash if the cleanup does not succeed for any reason.")]
        public override void Cleanup()
        {
            TraceFactory.Logger.Info("Cleanup Session for {0}".FormatWith(_credential.UserName));
            try
            {
                // Give the Citrix server a few seconds to gracefully log the user off the session before
                // sending the death signal through the KillCitrixSession().
                Thread.Sleep(TimeSpan.FromSeconds(2));

                ChangeMachineStatusMessage("Logoff Citrix");

                CitrixSessionManager.ResetCitrixSession(_credential.UserName, _citrixServer);
               // CitrixSessionManager.RemoveFromAdminGroup(_credential, _citrixServer);

                // Send a delete request to the Citrix Monitor service and request a cleanup.  This will
                // proceed to remove the user profile directory from the server to keep the disk space under
                // control.
                TraceFactory.Logger.Debug("Sending delete request for: {0}".FormatWith(_userProfile));
                string citrixHost = _citrixServer;
                using (CitrixQueueMonitorConnection client = CitrixQueueMonitorConnection.Create(citrixHost))
                {
                    //client.Channel.Cleanup(_credential.UserName, _userProfile);
                    client.Channel.Cleanup(_credential);
                }

                //the user configuration file deletion is moved here from startup program to allow for retries to work - Veda
                string remotePath = @"\\{0}\C$\VirtualResource\UserConfiguration".FormatWith(_citrixServer);
                string datFile = @"{0}\{1}.dat".FormatWith(remotePath, _credential.UserName);
                if (File.Exists(datFile))
                {
                    File.Delete(datFile);
                }

                // Now that the user is logged out, purge all print queues on the local machine.
                    // This also occurs on the Citrix server, both have to be purged in order to++
                    // ensure that there are no print jobs left in a queue after a completed test.
                    TraceFactory.Logger.Debug("Purging local print queues...");
                PurgeLocalPrintQueues();
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error cleaning up Citrix session", ex);
            }
        }

        /// <summary>
        /// Installs the remote printers.  For Citrix these printers need to be installed on the 
        /// client ahead of time so that they are autocreated on the server.
        /// </summary>
        private void InstallRemotePrinters()
        {
            TraceFactory.Logger.Debug("Installing remote printers");

            // The manifest includes all queues required for the entire session.
            // Only install queues for activities that are part of this manifest.
            var activityIds = SystemManifest.Resources.SelectMany(n => n.MetadataDetails).Select(n => n.Id);
            List<RemotePrintQueueInfo> remotePrintQueues = activityIds.Select(n => SystemManifest.ActivityPrintQueues[n]).SelectMany(n => n.OfType<RemotePrintQueueInfo>()).ToList();

            List<Guid> createdQueues = new List<Guid>();
            foreach (RemotePrintQueueInfo queueInfo in remotePrintQueues)
            {
                // Only install the printer once by checking the printQueueId list.
                if (!createdQueues.Contains(queueInfo.PrintQueueId))
                {
                    createdQueues.Add(queueInfo.PrintQueueId);
                }

                string printerName = queueInfo.GetPrinterName();

                if (!PrintQueueInstaller.IsInstalled(printerName))
                {
                    ChangeMachineStatusMessage("Installing queue {0}".FormatWith(printerName));

                    // Install the print queue using administrator first to ensure the driver gets down
                    // without any authorization issues.  Sleep for a few seconds, then try to install
                    // the queue for the given user.  It should use the already installed driver and eliminate
                    // any issues in getting the driver down on the box.
                    CallPrintUi(printerName, credential: AdminCredential);
                    Thread.Sleep(1000);
                    CallPrintUi(printerName, credential: _credential);
                }

                // Display the printers that have been installed
                List<string> queues = PrintQueueController.GetPrintQueues().Select(n => n.ShareName).ToList();

                TraceFactory.Logger.Debug("Printers after Install: {0}{1}".FormatWith
                    (
                        Environment.NewLine,
                        string.Join(Environment.NewLine, queues))
                    );
            }
        }

        /// <summary>
        /// Calls the print UI utility.
        /// </summary>
        /// <param name="printerPath">The printer path.</param>
        /// <param name="credential">The credential.</param>
        private static void CallPrintUi(string printerPath, OfficeWorkerCredential credential = null)
        {
            string arg = Resources.PrintUICommand.FormatWith(printerPath);
            TraceFactory.Logger.Debug(arg);

            CitrixSessionManager.StartProcess(credential, "CMD.EXE", arg, TimeSpan.FromMinutes(10));
        }

        private void TriggerQueueMonitoring()
        {
            using (CitrixQueueMonitorConnection client = CitrixQueueMonitorConnection.Create(_citrixServer))
            {
                foreach (CitrixQueueClientData data in _printClientData)
                {
                    data.SessionStart = DateTime.Now;
                    client.Channel.Subscribe(data);
                    TraceFactory.Logger.Debug("{0} : {1}".FormatWith(data.SessionStart.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture), data.QueueName));
                }
            }
        }

        /// <summary>
        /// Tells the service to copy any logs that are unique to this resource.
        /// </summary>
        /// <returns>LogFileDataCollection.</returns>
        public override LogFileDataCollection GetLogFiles(string sessionId)
        {
            var logFiles = base.GetLogFiles(sessionId);

            // The individual office workers will handle themselves in terms of getting log data back.
            // But this method is used to get Citrix service log files and CSV files from the Citrix server.
            using (CitrixQueueMonitorConnection client = CitrixQueueMonitorConnection.Create(_citrixServer))
            {
                logFiles.Append(client.Channel.GetCitrixLogFiles(sessionId));
            }
             
            using (var client = new WcfClient<IPrintMonitorService>(MessageTransferType.Http, WcfService.PrintMonitor.GetHttpUri(_citrixServer)))
            {
                logFiles.Append(client.Channel.GetLogFiles());
            }

            return logFiles;
        }
    }
}
