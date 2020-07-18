using System;
using System.IO;
using System.Linq;
using System.Net;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.WindowsAutomation
{
    /// <summary>
    /// Provides functionality for executing a setup file, such as an .MSI or .CMD file.
    /// </summary>
    public static class SystemSetup
    {
        private const string _msiCommand = @"/C start /wait msiexec.exe /i ""{0}"" {1}";
        private const string _msiArguments = "SILENT=1 REBOOT=ReallySuppress MSIDISABLERMRESTART=0 MSIRESTARTMANAGERCONTROL=0 ARPSYSTEMCOMPONENT=1 ALLUSERS=1";

        /// <summary>
        /// Executes the specified setup/installer file using the provided user credential.
        /// </summary>
        /// <param name="remoteInstallerPath">The remote installer path.</param>
        /// <param name="credential">The user credential.</param>
        /// <param name="copyInstallerDirectory">if set to <c>true</c> copy the entire contents of the installer directory to the local machine.</param>
        /// <returns><c>true</c> if the installation is successful; otherwise, <c>false</c>.</returns>
        public static bool Run(string remoteInstallerPath, NetworkCredential credential, bool copyInstallerDirectory)
        {
            return Run(remoteInstallerPath, string.Empty, credential, copyInstallerDirectory);
        }

        /// <summary>
        /// Executes the specified setup/installer file using the provided user credential and arguments.
        /// </summary>
        /// <param name="remoteInstallerPath">The remote installer path.</param>
        /// <param name="arguments">The arguments for the installer.</param>
        /// <param name="credential">The user credential.</param>
        /// <param name="copyInstallerDirectory">if set to <c>true</c> copy the entire contents of the installer directory to the local machine.</param>
        /// <returns><c>true</c> if the installation is successful; otherwise, <c>false</c>.</returns>
        public static bool Run(string remoteInstallerPath, string arguments, NetworkCredential credential, bool copyInstallerDirectory)
        {
            if (string.IsNullOrEmpty(remoteInstallerPath))
            {
                throw new ArgumentException("Remote installer path cannot be null or empty.", nameof(remoteInstallerPath));
            }

            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            // Try to copy the file down from the remote location.
            // Retry up to 10 times to avoid any file access conflicts.
            string localDirectory = CreateLocalDirectory();
            string localFilePath = string.Empty;
            void action()
            {
                localFilePath = CopyInstallerFiles(remoteInstallerPath, localDirectory, copyInstallerDirectory);
            }
            Retry.WhileThrowing<Exception>(action, 10, TimeSpan.FromSeconds(5));

            // Execute the installer
            bool result;
            TimeSpan timeout = TimeSpan.FromMinutes(1200);
            if (Path.GetExtension(localFilePath).EqualsIgnoreCase(".MSI"))
            {
                if (string.IsNullOrEmpty(arguments))
                {
                    arguments = _msiArguments;
                }
                result = StartProcess("cmd.exe", string.Format(_msiCommand, localFilePath, arguments), credential, timeout);
            }
            else
            {
                result = StartProcess(localFilePath, arguments, credential, timeout);
            }

            return result;
        }

        private static string CreateLocalDirectory()
        {
            string localDirectoryPath = Path.Combine(Path.GetTempPath(), "ScalableTestFramework");
            if (!Directory.Exists(localDirectoryPath))
            {
                Directory.CreateDirectory(localDirectoryPath);
            }
            return localDirectoryPath;
        }

        private static string CopyInstallerFiles(string sourcePath, string destinationPath, bool copyInstallerDirectory)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(Path.GetDirectoryName(sourcePath));
            if (!sourceDirectory.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourcePath}");
            }

            string localFilePath;
            if (copyInstallerDirectory)
            {
                // Copy the whole directory to the local location
                LogDebug($"Copying installer directory from {sourcePath} to {destinationPath}.");
                string localDirectoryPath = Path.Combine(destinationPath, sourceDirectory.Name);
                localFilePath = Path.Combine(localDirectoryPath, Path.GetFileName(sourcePath));

                if (Directory.Exists(localDirectoryPath))
                {
                    // Delete the directory before copying
                    FileSystem.DeleteDirectory(localDirectoryPath);
                }

                FileSystem.CopyDirectory(sourceDirectory.FullName, localDirectoryPath);
            }
            else
            {
                // Copy the file to the destination directory, overwriting if it already exists
                LogDebug($"Copying installer file from {sourcePath} to {destinationPath}.");
                localFilePath = Path.Combine(destinationPath, Path.GetFileName(sourcePath));
                File.Copy(sourcePath, localFilePath, true);
                File.SetAttributes(localFilePath, FileAttributes.Normal); // Make sure it's not set as a read-only file.
            }

            return localFilePath;
        }

        private static bool StartProcess(string localFilePath, string arguments, NetworkCredential credential, TimeSpan timeout)
        {
            //We want to ignore the following error codes:
            //Exit Code 3010 = The requested operation is successful. Changes will not be effective until the system is rebooted.
            //Exit Code 3011 = The requested operation is successful. Changes will not be effective until the service is restarted.
            // More may need to be added based on this information http://msdn.microsoft.com/en-us/library/ee225238.aspx
            int[] okExitCodes = new[] { 0, 3010, 3011, -2147185721 };

            ProcessExecutionResult result = ProcessUtil.Execute(localFilePath, arguments, credential, timeout);
            return okExitCodes.Contains(result.ExitCode);
        }
    }
}
