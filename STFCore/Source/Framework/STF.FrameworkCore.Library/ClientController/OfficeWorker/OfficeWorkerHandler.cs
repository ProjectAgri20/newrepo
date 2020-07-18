using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.Net;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Automation.EmailMonitor;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Print;
using HP.ScalableTest.Xml;
using HP.ScalableTest.Utility;
using HP.ScalableTest.WindowsAutomation;
using System.Printing;
using System.DirectoryServices.AccountManagement;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Automation.OfficeWorker
{
    /// <summary>
    /// Controller that handles setting up OfficeWorkers on a VM Client.
    /// </summary>
    [VirtualResourceHandler(VirtualResourceType.OfficeWorker)]
    internal class OfficeWorkerHandler : VirtualResourceHandler
    {
        private List<Uri> _userEndpoints = new List<Uri>();
        private readonly object _lock = new object();
        protected int _userCount = 0;
        protected Timer _emailTimer = null;
        protected TimeSpan _interval = TimeSpan.FromMinutes(5);
        /// <summary>
        /// Gets or sets the popup assassin.
        /// </summary>
        /// <value>The popup assassin.</value>
        protected PopupAssassin PopupAssassin { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeWorkerHandler"/> class.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        public OfficeWorkerHandler(SystemManifest manifest)
            : base(manifest)
        {
           // _emailTimer = new Timer(TimerCallBack);
        }

        /// <summary>
        /// Creates the OfficeWorker resources.
        /// </summary>
        public override void Start()
        {
            // Start the popup kill program that will monitor for bogus popups and kill them.
            InitializePopupManager();
            StartPrintQueueMonitor();
            InstallLocalPrinters();

            ChangeMachineStatusMessage("Starting Workers");

            _userCount = SystemManifest.Resources.Credentials.Count();

            // Iterate through each domain user account and create the resource launcher process
            foreach (var credential in SystemManifest.Resources.Credentials)
            {                        
                credential.WorkingDirectory = Directory.GetCurrentDirectory();

                TraceFactory.Logger.Debug(credential);                

                //Subscribe to Email notifications
               // ExchangeEmailMonitor.Subscribe(new NetworkCredential(credential.UserName, credential.Password));

                // Configure user groups.
                ConfigureUserGroups(credential);
                ConfigureLocalUserGroups(credential);

                // Create the office worker console
                ChangeResourceState(credential.ResourceInstanceId, RuntimeState.Starting);

                try
                {
                    StartUserProcess(credential, Directory.GetCurrentDirectory());
                }
                catch
                {
                    ChangeMachineStatusMessage("Error starting worker {0}".FormatWith(credential.UserName));
                    ChangeResourceState(credential.ResourceInstanceId, RuntimeState.Error);
                }
            }

           // _emailTimer.Change(TimeSpan.FromSeconds(30), _interval);
        }

        private static void StartPrintQueueMonitor()
        {
            string appPath = Properties.Resources.PrintQueueMonitorApplicationFileName;
            string args = "-standalone /database=" + GlobalSettings.Items[Setting.EnterpriseTestDatabase];

            TraceFactory.Logger.Debug("Starting " + appPath);
            using (Process process = Process.Start(appPath, args))
            {
                // Make sure the process starts correctly.  Wait on it for a little while.
                if (process.WaitForExit((int)new TimeSpan(0, 0, 10).TotalMilliseconds))
                {
                    // This means the process already terminated.
                    throw new InvalidOperationException("Unable to launch the Print Queue Monitor.");
                }

                TraceFactory.Logger.Debug("PrintQueueMonitor started.");
            }
        }

        protected void InstallLocalPrinters()
        {
            var installer = new LocalPrintQueueInstaller(SystemManifest);
            installer.Install();
            installer.ValidateShortcuts();
        }

        /// <summary>
        /// Performs any cleanup activities like removing any users from AD groups.
        /// </summary>
        public override void Cleanup()
        {
            foreach (var credential in SystemManifest.Resources.Credentials)
            {
                TraceFactory.Logger.Debug("Cleaning up profile: {0}".FormatWith(credential.UserName));
                VirtualResourceUserProfile.Cleanup(credential);
                //ConfigureUserGroups(credential, addToGroups: false);
            }

            try
            {
                TraceFactory.Logger.Debug("Purging local print queues...");
                PurgeLocalPrintQueues();
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex.Message);
            }
        }

        protected static void PurgeLocalPrintQueues()
        {
            LocalPrintServer localMachine = new LocalPrintServer(PrintSystemDesiredAccess.AdministrateServer);
            foreach (var queue in localMachine.GetPrintQueues())
            {
                // Need to create another print queue object with sufficient privileges to purge
                PrintQueue privilegedQueue = new PrintQueue(localMachine, queue.Name, PrintSystemDesiredAccess.AdministratePrinter);
                privilegedQueue.Purge();
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (PopupAssassin != null)
                {
                    PopupAssassin.Stop();
                    PopupAssassin = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Initializes the popup manager.
        /// </summary>
        protected void InitializePopupManager()
        {
            Collection<string> titles = new Collection<string>();

            TraceFactory.Logger.Debug("Initializing Popup Manager.");
            titles.Add("Save the file as"); //.xps popup

            PopupAssassin = new PopupAssassin(titles);
            PopupAssassin.Start(new TimeSpan(0, 0, 5));
        }

        /// <summary>
        /// Configures User Groups in Activity Directory for each user count..
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="addToGroups">Whether to add the user to the AD groups specified in the manifest.</param>
        protected void ConfigureUserGroups(OfficeWorkerCredential credential, bool addToGroups = true)
        {
            if (credential == null)
            {
                throw new ArgumentNullException("credential");
            }

            string userName = credential.UserName;

            // Check to be sure there are user groups to configure
            string securityGroupXml = SystemManifest.Resources.GetByUsername(userName).SecurityGroups;
            if (string.IsNullOrEmpty(securityGroupXml))
            {
                // There are no active directory groups to process, so return
                return;
            }

            // Get the groups to be processed and the appropriate logging label
            Collection<ActiveDirectoryGroup> groups = null;
            string label = string.Empty;
            if (addToGroups)
            {
                groups = LegacySerializer.DeserializeDataContract<Collection<ActiveDirectoryGroup>>(securityGroupXml);
                label = "Adding";
            }
            else
            {
                groups = new Collection<ActiveDirectoryGroup>();
                label = "Removing";
            }

            PrincipalContext context = new PrincipalContext(ContextType.Domain);

            // Compare what the list of groups are to the master list from Active Directory
            // for every entry found in active directory add it to the list to be processed.
            // If there is a group listed to be assigned but it doesn't exist anymore in active
            // directory, log that error.
            var groupsToAssign = new List<GroupPrincipal>();
            if (addToGroups)
            {
                foreach (var group in groups)
                {
                    GroupPrincipal groupPrincipal = GroupPrincipal.FindByIdentity(context, group.Name);
                    if (groupPrincipal != null)
                    {
                        TraceFactory.Logger.Debug("Group {0} will be assigned to {1}".FormatWith(groupPrincipal.Name, credential.UserName));
                        groupsToAssign.Add(groupPrincipal);
                    }
                    else
                    {
                        TraceFactory.Logger.Error("The group {0} does not exist in the Active Directory server".FormatWith(group.Name));
                    }
                }
            }

            // Find any groups the user is a member of that must be removed.  Ignore Domain Users, since that group cannot be unjoined.
            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, userName);
            var existingUserGroups = userPrincipal.GetAuthorizationGroups().OfType<GroupPrincipal>();
            var groupsToRemove = existingUserGroups.Except(groupsToAssign).Where(n => n.Name != "Domain Users");

            Action action = () =>
            {
                ActiveDirectoryController.RemoveUserFromGroups(userPrincipal, groupsToRemove);
                ActiveDirectoryController.AddUserToGroups(userPrincipal, groupsToAssign);
            };

            try
            {
                Retry.WhileThrowing(action, 10, TimeSpan.FromSeconds(5), new List<Type> { typeof(DirectoryServicesCOMException) });
            }
            catch (UnauthorizedAccessException)
            {
                TraceFactory.Logger.Debug("User {0} is not authorized to assign group membership. {1} will not be assigned."
                    .FormatWith(Environment.UserName, credential.UserName));
            }
        }

        /// <summary>
        /// Configures the local user groups.
        /// </summary>
        /// <param name="credential">The credential.</param>
        protected static void ConfigureLocalUserGroups(OfficeWorkerCredential credential)
        {
            ConfigureLocalUserGroups(credential, Environment.MachineName);
        }

        /// <summary>
        /// Configures the local user groups.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="targetHostName">Name of the target host.</param>
        protected static void ConfigureLocalUserGroups(OfficeWorkerCredential credential, string targetHostName)
        {
            if (credential == null)
            {
                throw new ArgumentNullException("credential");
            }

            // This will attempt to put the user in the Admin group on the target host
            // By default the target host is the Environment.MachineName, but for Citrix
            // as an example, it is the Citrix server.
            TraceFactory.Logger.Debug($"User: {credential.UserName}.  Domain: {credential.Domain}. MachineName: {targetHostName}.");

            PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, credential.Domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, credential.UserName);
            TraceFactory.Logger.Debug($"Found Domain UserPrincipal: {user.Name}");

            PrincipalContext machineContext = new PrincipalContext(ContextType.Machine, targetHostName);
            GroupPrincipal administrators = GroupPrincipal.FindByIdentity(machineContext, "Administrators");
            TraceFactory.Logger.Debug($"Found machine GroupPrincipal: {administrators.Name}");

            try
            {
                ActiveDirectoryController.AddUserToGroup(user, administrators);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                // The group membership check failed, most likely because the current executing user does not
                // have permissions to view all the members in the group.  This condition produces the follwing error:
                // System.Runtime.InteropServices.COMException (0x80070035): The network path was not found.
                TraceFactory.Logger.Debug($"Unable to iterate {administrators.Name} membership. ({ex.GetType().FullName}. {ex.Message.Trim()})");
                TraceFactory.Logger.Debug("Attempting add via LDAP.");
                //Try adding using LDAP
                ActiveDirectoryController.AddUserToGroupLdap(credential.Domain, credential.UserName, "Administrators");
            }
        }

        /// <summary>
        /// Starts the defined Virtual Resource process for the defined credential.
        /// </summary>
        /// <param name="credential">The user credential.</param>
        /// <param name="currentDirectory">The current directory.</param>
        protected virtual void StartUserProcess(OfficeWorkerCredential credential, string currentDirectory)
        {
            if (credential == null)
            {
                throw new ArgumentNullException("credential");
            }

            
            TraceFactory.Logger.Debug("Starting User Process {0} on port {1}".FormatWith(credential.UserName, credential.Port));

            string exeFile = Properties.Resources.OfficeWorkerApplicationFileName;
            TraceFactory.Logger.Debug(exeFile);

            

            var officeWorkerExe = Path.Combine(Path.GetDirectoryName(currentDirectory), exeFile);
            var commandLine = "{0} {1}".FormatWith
                (
                    Environment.MachineName,
                    credential.ResourceInstanceId
                );
            var bootStrapperExePath = Path.Combine(currentDirectory, "OfficeWorkerBootStrapper.exe");
            if (File.Exists(bootStrapperExePath))
            {
                commandLine = $"{officeWorkerExe} {Environment.MachineName} {credential.ResourceInstanceId}";
                officeWorkerExe = bootStrapperExePath;
            }
            TraceFactory.Logger.Debug("Executing {0} {1}".FormatWith(officeWorkerExe, commandLine));

            NetworkCredential netCredential = new NetworkCredential(credential.UserName, credential.Password, credential.Domain);
            ProcessUtil.Launch(officeWorkerExe, commandLine, currentDirectory, netCredential);
            Thread.Sleep(TimeSpan.FromSeconds(5));

            TraceFactory.Logger.Debug("Start User Process Complete".FormatWith(credential.UserName, credential.Port));
        }

        /// <summary>
        /// Notifies the state of the worker to the main controller.
        /// </summary>
        /// <param name="endpoint">The endpoint used to communicate back to the worker.</param>
        /// <param name="state">The state of the worker being sent to the controller.</param>
        public override void NotifyResourceState(Uri endpoint, RuntimeState state)
        {
            TraceFactory.Logger.Debug($"Notifying {endpoint.AbsoluteUri} with state {state}");
            if (state == RuntimeState.Ready)
            {
                lock (_lock)
                {
                    if (_userEndpoints.All(x => x != endpoint))
                    {
                        _userEndpoints.Add(endpoint);
                        TraceFactory.Logger.Debug("Added endpoint {0}".FormatWith(endpoint));
                    }
                    else
                    {
                        throw new KeyNotFoundException("The same virtual user tried to register more than once.  Endpoint {0}".FormatWith(endpoint));
                    }

                    if (_userEndpoints.Count >= _userCount)
                    {
                        TraceFactory.Logger.Debug("All workers reported in, now peforming system wide tasks in the background.");
                        Task.Factory.StartNew(PerformSystemSetupActivities);
                    }
                }
            }
        }

        private void PerformSystemSetupActivities()
        {
            ChangeMachineStatusMessage("Plugin Setup");

            // Once the all child workers have reported in, notify all the workers that they can continue
            // the registration process.
            foreach (var workerEndpoint in _userEndpoints)
            {
                TraceFactory.Logger.Debug("{0} - ReadyToRegister START".FormatWith(workerEndpoint));

                using (var client = VirtualResourceManagementConnection.Create(workerEndpoint))
                {
                    try
                    {
                        client.Channel.ReadyToRegister();
                        TraceFactory.Logger.Debug("{0} - ReadyToRegister END".FormatWith(workerEndpoint));
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Debug("{0} - ReadyToRegister did not return: {1}".FormatWith(workerEndpoint, ex));
                    }
                }
            }

            TraceFactory.Logger.Debug("All workers sent notification to resume registration");
        }

        private void TimerCallBack(object state)
        {
            try
            {
                ExchangeEmailMonitor.CheckEmail();
            }
            catch
            {
                TraceFactory.Logger.Debug("Failed to contact exchange server.");
            }
        }
    }
}