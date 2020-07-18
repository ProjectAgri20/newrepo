using HP.ScalableTest.Framework.Properties;
using HP.ScalableTest.Framework.Manifest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Net;
using HP.ScalableTest.Utility;
using System.DirectoryServices.AccountManagement;
using HP.ScalableTest.WindowsAutomation;

namespace HP.ScalableTest.Framework.Automation
{
    internal class CitrixSessionManager
    {

        internal static void ConfigureLocalUserGroups(OfficeWorkerCredential credential, string targetHostName)
        {
            if (credential == null)
            {
                throw new ArgumentNullException("credential");
            }

            // This will attempt to put the user in the Admin group on the target host
            // By default the target host is the Environment.MachineName, but for Citrix
            // as an example, it is the Citrix server.
            TraceFactory.Logger.Debug("Adding {0} to {1}".FormatWith(credential.UserName, targetHostName));

            PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, credential.Domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, credential.UserName);

            PrincipalContext citrixContext = new PrincipalContext(ContextType.Machine, targetHostName);
            GroupPrincipal administrators = GroupPrincipal.FindByIdentity(citrixContext, "Administrators");

            ActiveDirectoryController.AddUserToGroup(user, administrators);
        }

        internal static void RestartCitrixClient(OfficeWorkerCredential credential)
        {
            TraceFactory.Logger.Debug(@"{0}\{1}".FormatWith(credential.Domain, credential.UserName));

            // This will kill the Citrix client process, typically running under admin and
            // restart it under the target username.
            if (credential == null)
            {
                throw new ArgumentNullException("credential");
            }

            int tries = 0;
            bool done = false;


            // ALAN: Consider re-designing such that StartProcess throws an exception on failure...then this can be
            //       done via the RetryUtil.
            do
            {
                // First kill the wfcrun32 process as it will be restarted under the target username
                if (!ProcessUtil.KillProcess(Resources.wfcrun32, currentUserOnly: false))
                {
                    TraceFactory.Logger.Debug("Failed to kill current process");
                }
                else
                {
                    TraceFactory.Logger.Debug("Current process killed");
                }

                Environment.SpecialFolder folder = (Environment.Is64BitOperatingSystem)
                    ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles;

                string citrixClient = Path.Combine(Environment.GetFolderPath(folder), Resources.WFCRun32Path);
                TraceFactory.Logger.Debug("Starting {0}".FormatWith(citrixClient));

                if (!StartProcess(credential, citrixClient, string.Empty, TimeSpan.FromSeconds(10), true))
                {
                    tries++;
                }
                else
                {
                    done = true;
                }

            } while (tries < 3 && !done);

            if (!done)
            {
                throw new InvalidOperationException("Unable to start process");
            }

            TraceFactory.Logger.Debug("Process started");
        }

        internal static void StartPublishedApp(OfficeWorkerCredential credential, string citrixServer, string publishedApp)
        {
            Environment.SpecialFolder folder = (Environment.Is64BitOperatingSystem)
                ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles;

            string resourcePath = Path.Combine
                (
                    Environment.GetFolderPath(folder),
                    Resources.WFICA32Path
                );

            credential.WorkingDirectory = Directory.GetCurrentDirectory();

            TraceFactory.Logger.Debug("Starting published app {0}".FormatWith(publishedApp));

            string icaFile = CitrixSessionManager.CreateIcaFile(credential, citrixServer, publishedApp);

            CitrixSessionManager.StartProcess(credential, resourcePath, icaFile, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Kills the citrix session for the given username.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="citrixServer"></param>
        internal static void ResetCitrixSession(string userName, string citrixServer)
        {
            TraceFactory.Logger.Debug("Resetting session for {0} on {1}".FormatWith(userName, citrixServer));

            string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TSLogoff");

            string logOffApp = Path.Combine(appDirectory, "TSLogoff.exe");

            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
                File.WriteAllBytes(logOffApp, Resources.TSLogoff);
            }

            string args = "{0} /server:{1}".FormatWith(userName, citrixServer);
            TraceFactory.Logger.Debug("Executing TSLogoff.exe {0}".FormatWith(args));

            StartProcess(null, logOffApp, args, TimeSpan.FromSeconds(10));

            // Pause for a few seconds to let Citrix catch up and cleanly kill the session
            TraceFactory.Logger.Debug("TSLogoff executed, sleeping for a few seconds...");
            Thread.Sleep(5000);
        }

        internal static string CreateIcaFile(OfficeWorkerCredential credential, string citrixServer, string publishedApp)
        {
            // Create the ICA Client file using the template, then launch the ICA Client against the template, this will
            // start the published app on the Citrix server.
            string template = Resources.CitrixWorkerXenApp.FormatWith
                (
                    credential.Domain.Split('.').First(),
                    credential.UserName,
                    credential.Password,
                    citrixServer,
                    publishedApp
                );

            TraceFactory.Logger.Debug(Environment.NewLine + template);

            if (!Directory.Exists(Resources.ICAFilePath))
            {
                Directory.CreateDirectory(Resources.ICAFilePath);
            }

            // Write the ICA File out to a temp location
            string citrixIcaFile = Path.Combine(Resources.ICAFilePath, Resources.ICAFile.FormatWith(credential.UserName));

            TraceFactory.Logger.Debug("ICA File: {0}".FormatWith(citrixIcaFile));
            File.WriteAllText(citrixIcaFile, template);

            if (!File.Exists(citrixIcaFile))
            {
                throw new IOException("ICA File not created");
            }

            return citrixIcaFile;
        }

        /// <summary>
        /// Starts the defined process.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="reverseWait">if set to <c>true</c> [reverse wait].</param>
        /// <returns></returns>
        internal static bool StartProcess
            (
                OfficeWorkerCredential credential,
                string fileName,
                string arguments,
                TimeSpan timeout,
                bool reverseWait = false
            )
        {
            bool completed = false;

            try
            {
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = fileName;
                    p.StartInfo.Arguments = arguments;
                    p.StartInfo.LoadUserProfile = true;
                    p.StartInfo.CreateNoWindow = false;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    p.StartInfo.RedirectStandardOutput = true;

                    if (credential != null)
                    {
                        TraceFactory.Logger.Debug("Using credential {0}".FormatWith(credential));

                        // Convert to NetworkCredential to make use of SecureString for password
                        NetworkCredential networkCredential = new NetworkCredential(credential.UserName, credential.Password, credential.Domain);

                        p.StartInfo.UserName = networkCredential.UserName;
                        p.StartInfo.Domain = networkCredential.Domain;
                        p.StartInfo.Password = networkCredential.SecurePassword;
                    }

                    TraceFactory.Logger.Debug("{0} {1}".FormatWith(fileName, arguments));

                    p.Start();

                    if (!reverseWait)
                    {
                        // Read standard out on processes that should run to completion
                        StreamReader reader = p.StandardOutput;
                        TraceFactory.Logger.Debug("STDOUT: " + reader.ReadLine());

                        if (!p.WaitForExit((int)timeout.TotalMilliseconds))
                        {
                            TraceFactory.Logger.Debug("Process failed to start, Exit Code: ".FormatWith(p.ExitCode));
                        }
                        else
                        {
                            completed = true;
                        }
                    }
                    else
                    {
                        TraceFactory.Logger.Debug("Reverse wait");

                        // If we come back immediately because we didn't reach the timeout,
                        // then something went wrong.  The process should not terminate, so
                        // we should reach the timeout, which would cause WaitForExit to return
                        // false.  If it returns true, then the process exited for some reason.
                        if (p.WaitForExit((int)timeout.TotalMilliseconds))
                        {
                            TraceFactory.Logger.Debug("Process failed to start, Exit Code: ".FormatWith(p.ExitCode));
                        }
                        else
                        {
                            completed = true;
                        }
                    }
                }
            }
            finally
            {
            }

            return completed;
        }

        internal static void RemoveFromAdminGroup(OfficeWorkerCredential credential, string citrixServer)
        {
            // Remove this user from the Administrators group.            
            TraceFactory.Logger.Debug("Removing {0} from Administrators group on {1}".FormatWith(credential.UserName, citrixServer));

            PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, credential.Domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, credential.UserName);

            PrincipalContext citrixContext = new PrincipalContext(ContextType.Machine, citrixServer);
            GroupPrincipal administrators = GroupPrincipal.FindByIdentity(citrixContext, "Administrators");

            ActiveDirectoryController.RemoveUserFromGroup(user, administrators);
        }


    }
}
