using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32.SafeHandles;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Provides extension methods and utilities for working with <see cref="Process" />.
    /// </summary>
    public static class ProcessUtil
    {
        /// <summary>
        /// Launches a new process defined by the specified command and arguments.
        /// </summary>
        /// <param name="fileName">The file name of the application to run.</param>
        /// <param name="arguments">The command-line arguments to pass to the application when the process starts.</param>
        /// <returns>A <see cref="Process" /> object representing the process that was started.</returns>
        public static Process Launch(string fileName, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName, arguments);
            return Launch(startInfo);
        }

        /// <summary>
        /// Launches a new process defined by the specified command and arguments.
        /// </summary>
        /// <param name="fileName">The file name of the application to run.</param>
        /// <param name="arguments">The command-line arguments to pass to the application when the process starts.</param>
        /// <param name="workingDirectory">The working directory for the started process.</param>
        /// <returns>A <see cref="Process" /> object representing the process that was started.</returns>
        public static Process Launch(string fileName, string arguments, string workingDirectory)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName, arguments)
            {
                WorkingDirectory = workingDirectory
            };

            return Launch(startInfo);
        }

        /// <summary>
        /// Launches a new process defined by the specified command and arguments.
        /// </summary>
        /// <param name="fileName">The file name of the application to run.</param>
        /// <param name="arguments">The command-line arguments to pass to the application when the process starts.</param>
        /// <param name="workingDirectory">The working directory for the started process.</param>
        /// <param name="credential">The <see cref="NetworkCredential" /> that should be used to execute the process.</param>
        /// <returns>A <see cref="Process" /> object representing the process that was started.</returns>
        public static Process Launch(string fileName, string arguments, string workingDirectory, NetworkCredential credential)
        {
            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(fileName, arguments)
            {
                WorkingDirectory = workingDirectory,
                UserName = credential.UserName,
                Domain = credential.Domain,
                Password = credential.SecurePassword,
                LoadUserProfile = true
            };

            return Launch(startInfo);
        }

        private static Process Launch(ProcessStartInfo startInfo)
        {
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            LogDebug($"Launching external process {startInfo.FileName}");
            return Process.Start(startInfo);
        }

        /// <summary>
        /// Executes a process defined by the specified command and arguments and returns the result.
        /// Waits one minute for the process to exit.
        /// </summary>
        /// <param name="fileName">The file name of the application to run.</param>
        /// <param name="arguments">The command-line arguments to pass to the application when the process starts.</param>
        /// <returns>A <see cref="ProcessExecutionResult" /> representing the output of the process.</returns>
        public static ProcessExecutionResult Execute(string fileName, string arguments)
        {
            return Execute(fileName, arguments, TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Executes a process defined by the specified command and arguments and returns the result.
        /// Waits the specified amount of time for the process to exit.
        /// </summary>
        /// <param name="fileName">The file name of the application to run.</param>
        /// <param name="arguments">The command-line arguments to pass to the application when the process starts.</param>
        /// <param name="timeout">The amount of time to wait for the application to exit.</param>
        /// <returns>A <see cref="ProcessExecutionResult" /> representing the output of the process.</returns>
        public static ProcessExecutionResult Execute(string fileName, string arguments, TimeSpan timeout)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName, arguments)
            {
                CreateNoWindow = true
            };
            return Execute(startInfo, timeout);
        }

        /// <summary>
        /// Executes a process defined by the specified command and arguments and returns the result.
        /// Waits the specified amount of time for the process to exit.
        /// </summary>
        /// <param name="fileName">The file name of the application to run.</param>
        /// <param name="arguments">The command-line arguments to pass to the application when the process starts.</param>
        /// <param name="credential">The <see cref="NetworkCredential" /> that should be used to execute the process.</param>
        /// <param name="timeout">The amount of time to wait for the application to exit.</param>
        /// <returns>A <see cref="ProcessExecutionResult" /> representing the output of the process.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="credential" /> is null.</exception>
        public static ProcessExecutionResult Execute(string fileName, string arguments, NetworkCredential credential, TimeSpan timeout)
        {
            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(fileName, arguments)
            {
                UserName = credential.UserName,
                Domain = credential.Domain,
                Password = credential.SecurePassword,
                LoadUserProfile = true
            };
            return Execute(startInfo, timeout);
        }

        /// <summary>
        /// Executes a process defined by the specified <see cref="ProcessStartInfo" /> and returns the result.
        /// Waits the specified amount of time for the process to exit.
        /// </summary>
        /// <param name="startInfo">The <see cref="ProcessStartInfo" /> that defines the process to execute.</param>
        /// <param name="timeout">The amount of time to wait for the application to exit.</param>
        /// <returns>A <see cref="ProcessExecutionResult" /> representing the output of the process.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="startInfo" /> is null.</exception>
        public static ProcessExecutionResult Execute(ProcessStartInfo startInfo, TimeSpan timeout)
        {
            if (startInfo == null)
            {
                throw new ArgumentNullException(nameof(startInfo));
            }

            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            LogDebug($"Executing external process {startInfo.FileName}");
            using (Process process = Process.Start(startInfo))
            {
                StringBuilder stdout = new StringBuilder();
                process.OutputDataReceived += (s, e) => ProcessDataReceived(stdout, e.Data);
                process.BeginOutputReadLine();

                StringBuilder stderr = new StringBuilder();
                process.ErrorDataReceived += (s, e) => ProcessDataReceived(stderr, e.Data);
                process.BeginErrorReadLine();

                bool successfulExit = process.WaitForExit((int)timeout.TotalMilliseconds);
                if (!process.HasExited)
                {
                    process.Kill();
                    process.WaitForExit((int)timeout.TotalMilliseconds);
                }

                int exitCode = process.HasExited ? process.ExitCode : -1;
                return new ProcessExecutionResult(successfulExit, exitCode, stdout.ToString(), stderr.ToString());
            }
        }

        private static void ProcessDataReceived(StringBuilder builder, string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                builder.AppendLine(data);
            }
        }

        /// <summary>
        /// Kills all processes with the specified name owned by the current user.
        /// </summary>
        /// <param name="processName">The name of the process to kill.</param>
        /// <returns><c>true</c> if at least one process was killed successfully, <c>false</c> otherwise.</returns>
        public static bool KillProcess(string processName)
        {
            return KillProcess(processName, true);
        }

        /// <summary>
        /// Kills all processes with the specified name, optionally ignoring those that are not owned by the current user.
        /// </summary>
        /// <param name="processName">The name of the process to kill.</param>
        /// <param name="currentUserOnly">if set to <c>true</c> only kill processes owned by the current user.</param>
        /// <returns><c>true</c> if at least one process was killed successfully, <c>false</c> otherwise.</returns>
        public static bool KillProcess(string processName, bool currentUserOnly)
        {
            return KillProcess(processName, currentUserOnly, TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Kills all processes with the specified name, optionally ignoring those that are not owned by the current user.
        /// </summary>
        /// <param name="processName">The name of the process to kill.</param>
        /// <param name="currentUserOnly">if set to <c>true</c> only kill processes owned by the current user.</param>
        /// <param name="timeout">The amount of time to wait for the process to exit.</param>
        /// <returns><c>true</c> if at least one process was killed successfully, <c>false</c> otherwise.</returns>
        public static bool KillProcess(string processName, bool currentUserOnly, TimeSpan timeout)
        {
            bool processKilled = false;
            foreach (Process process in Process.GetProcessesByName(processName))
            {
                if (!currentUserOnly || IsOwnedByCurrentUser(process))
                {
                    try
                    {
                        process.Kill();
                    }
                    catch (Win32Exception)
                    {
                        // Ignore this exception
                    }

                    processKilled |= process.WaitForExit((int)timeout.TotalMilliseconds);
                }
            }

            if (processKilled)
            {
                LogDebug($"Killed process(es) with name {processName}");
            }

            return processKilled;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static bool IsOwnedByCurrentUser(Process process)
        {
            // There seems to be a catch-22.  In order to see if the process is owned by the current user,
            // the process must be "opened".  Unfortunately, if you attempt to open a process that you don't own,
            // an exception is thrown.  The following try-catch is to handle this case.
            SafeAccessTokenHandle tokenHandle = SafeAccessTokenHandle.InvalidHandle;
            try
            {
                if (NativeMethods.OpenProcessToken(process.SafeHandle, TokenAccessLevels.Query, out tokenHandle))
                {
                    using (WindowsIdentity processUser = new WindowsIdentity(tokenHandle.DangerousGetHandle()))
                    {
                        return processUser.Name.Contains(Environment.UserName, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }
            catch
            {
                // This process must not be owned by the current user.
            }
            finally
            {
                tokenHandle.Dispose();
            }
            return false;
        }

        /// <summary>
        /// Gets the command line that was used to start the specified <see cref="Process" />.
        /// </summary>
        /// <param name="process">The <see cref="Process" />.</param>
        /// <returns>The command line used to start the process.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="process" /> is null.</exception>
        public static string GetCommandLine(Process process)
        {
            if (process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
            {
                using (ManagementObjectCollection objects = searcher.Get())
                {
                    return objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
                }
            }
        }

        #region DLL Imports

        private static class NativeMethods
        {
            [DllImport("advapi32")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool OpenProcessToken(SafeProcessHandle processHandle, TokenAccessLevels desiredAccess, out SafeAccessTokenHandle tokenHandle);
        }

        #endregion
    }
}
