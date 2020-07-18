using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Linq;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Automation.OfficeWorker;
using System.DirectoryServices.AccountManagement;
using HP.ScalableTest.WindowsAutomation;
using System.Net;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Automation
{
    [VirtualResourceHandler(VirtualResourceType.SolutionTester)]
    internal class SolutionTesterHandler : OfficeWorkerHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionTesterHandler" /> class.
        /// </summary>
        /// <param name="manifest"></param>
        public SolutionTesterHandler(SystemManifest manifest)
            : base(manifest)
        {
        }

        /// <summary>
        /// Creates the SolutionTester resources.
        /// </summary>
        public override void Start()
        {
            // Start the popup kill program that will monitor for bogus popups and kill them.
            InitializePopupManager();
            InstallLocalPrinters();

            ChangeMachineStatusMessage("Starting Testers");

            _userCount = SystemManifest.Resources.Credentials.Count();

            // Iterate through each domain user account and create the resource launcher process
            foreach (var credential in SystemManifest.Resources.Credentials)
            {
                TraceFactory.Logger.Debug("{0}".FormatWith(credential.UserName));

                credential.WorkingDirectory = Directory.GetCurrentDirectory();

                TraceFactory.Logger.Debug(credential);

                SolutionTesterDetail detail = GlobalDataStore.Manifest.Resources.GetWorker<SolutionTesterDetail>(credential.ResourceInstanceId);
                if (detail.UseCredential)
                {
                    // If we're starting the testers using their own credentials, add those users to the local admin group for WCF communication
                    try
                    {
                        ConfigureLocalUserGroups(credential);
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Error(ex.ToString());
                        ChangeMachineStatusMessage("Error: Permissions Config");
                        ChangeResourceState(credential.ResourceInstanceId, RuntimeState.Error);
                        return;
                    }
                }

                // Create the office worker console
                ChangeResourceState(credential.ResourceInstanceId, RuntimeState.Starting);

                try
                {
                    StartUserProcess(credential, Directory.GetCurrentDirectory());
                }
                catch (Exception)
                {
                    ChangeMachineStatusMessage("Error starting worker {0}".FormatWith(credential.UserName));
                    ChangeResourceState(credential.ResourceInstanceId, RuntimeState.Error);
                }
            }

            // Not worried about email accounts in STB
            //_emailTimer.Change(TimeSpan.FromSeconds(30), _interval);
        }

        /// <summary>
        /// Starts the SolutionTester process for the specified credential.
        /// </summary>
        /// <param name="credential">The user credential.</param>
        /// <param name="currentDirectory">The current directory.</param>
        protected override void StartUserProcess(OfficeWorkerCredential credential, string currentDirectory)
        {
            string root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string exeFile = Properties.Resources.SolutionTesterApplicationFileName;
            var solutionTester = Path.Combine(Path.GetDirectoryName(root), exeFile);
            if(!File.Exists(solutionTester))
                solutionTester = "C:\\VirtualResource\\Distribution\\SolutionTesterConsole\\SolutionTesterConsole.exe";
            try
            {
                SolutionTesterDetail detail = GlobalDataStore.Manifest.Resources.GetWorker<SolutionTesterDetail>(credential.ResourceInstanceId);
                if (detail.UseCredential)
                {
                    // The solution tester should be started using the same credentials
                    // that will be available to each plugin that executes.
                    TraceFactory.Logger.Debug("Starting SolutionTester using account for {0}".FormatWith(credential.UserName));

                    var commandLine = "{0} {1}".FormatWith
                        (
                            credential.ResourceInstanceId,
                            SystemManifest.SessionId
                        );

                    NetworkCredential netCredential = new NetworkCredential(credential.UserName, credential.Password, credential.Domain);
                    ProcessUtil.Launch(solutionTester, commandLine, currentDirectory, netCredential);
                }
                else
                {
                    // The solution tester should be started using the default destop account.
                    TraceFactory.Logger.Debug("Starting SolutionTester using Desktop account");

                    string args = "{0} {1}".FormatWith
                        (
                            credential.ResourceInstanceId,
                            SystemManifest.SessionId
                        );

                    ProcessStartInfo info = new ProcessStartInfo(solutionTester, args);
                    info.LoadUserProfile = true;
                    info.UseShellExecute = true;
                    info.WindowStyle = ProcessWindowStyle.Minimized;
                    info.WorkingDirectory = Path.GetDirectoryName(solutionTester);

                    TraceFactory.Logger.Debug("{0} {1}".FormatWith(solutionTester, args));
                    Process.Start(info);
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Fatal("Failed to create process.", ex);
                throw;
            }

            ChangeMachineStatusMessage("Tester started");
            TraceFactory.Logger.Debug("Start User Process Complete".FormatWith(credential.UserName, credential.Port));
        }

        /// <summary>
        /// Performs any cleanup activities like removing users from local Admin group, for any users other than the desktop user.
        /// </summary>
        public override void Cleanup()
        {
            string hostName = Environment.MachineName;

            foreach (var credential in SystemManifest.Resources.Credentials)
            {
                SolutionTesterDetail detail = GlobalDataStore.Manifest.Resources.GetWorker<SolutionTesterDetail>(credential.ResourceInstanceId);
                if (detail.UseCredential)
                {
                    VirtualResourceUserProfile.Cleanup(credential);
                    // This will attempt to remove the user from the Admin group on the current host.
                    //TraceFactory.Logger.Debug("Host Name: {0}".FormatWith(hostName));
                    //try
                    //{
                    //    PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, credential.Domain);
                    //    UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, credential.UserName);

                    //    PrincipalContext citrixContext = new PrincipalContext(ContextType.Machine, hostName);
                    //    GroupPrincipal administrators = GroupPrincipal.FindByIdentity(citrixContext, "Administrators");

                    //    ActiveDirectoryController.RemoveUserFromGroup(user, administrators);
                    //}
                    //catch (Exception ex)
                    //{
                    //    // Log the error for visibility, but move on.
                    //    TraceFactory.Logger.Error(ex);
                    //}
                }
            }
        }
    }
}
