using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;

namespace HP.ScalableTest
{
    /// <summary>
    /// Impersonates a user when executing defined applications.  Can be used for remote access to other
    /// servers where specific credentials are required.
    /// </summary>
    public class UserImpersonator : IDisposable
    {
        WindowsImpersonationContext _wic = null;

        /// <summary>
        /// Launches the specified application under the defined user credentials.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domain">The user domain name.</param>
        /// <param name="password">The user password.</param>
        /// <param name="application">The application to run under user impersonation.</param>
        /// <param name="workingDirectory">The working directory for the application.</param>
        [SecurityCritical]
        public static uint CreateProcess(string userName, string domain, string password, string application, string workingDirectory, bool showMinimized = false)
        {
            TraceFactory.Logger.Debug("Creating user process for {0}/{1} - {2}".FormatWith(domain, userName, application));
            try
            {
                // Default flags for the impersonation startup.
                NativeMethods.LogonFlags logonFlags = NativeMethods.LogonFlags.LogonWithProfile;
                NativeMethods.CreationFlags creationFlags = 0;
                IntPtr environment = IntPtr.Zero;
                string currentDirectory = workingDirectory;
                string commandLine = application;

                NativeMethods.StartupInfo startupInfo = new NativeMethods.StartupInfo();
                startupInfo.Cb = Marshal.SizeOf(typeof(NativeMethods.StartupInfo));

                if (showMinimized)
                {
                    startupInfo.Flags = (uint)NativeMethods.StartupInfoFlags.StartfUseShowWindow;
                    startupInfo.ShowWindow = (ushort)NativeMethods.StartupInfoFlags.SWMinimize;
                }

                NativeMethods.ProcessInfo processInfo = new NativeMethods.ProcessInfo();

                TraceFactory.Logger.Debug("Calling CreateProcessWithLogonW...");
                bool created = NativeMethods.CreateProcessWithLogonW(userName, domain, password, logonFlags, null,
                        commandLine, creationFlags, environment, currentDirectory, ref startupInfo, out processInfo);

                if (created)
                {
                    TraceFactory.Logger.Debug("Process ({0}) successfully created, closing handles".FormatWith(processInfo.ProcessId));
                    NativeMethods.CloseHandle(processInfo.ProcessPtr);
                    NativeMethods.CloseHandle(processInfo.ThreadPtr);
                }
                else
                {
                    int error = Marshal.GetLastWin32Error();
                    var msg = "Unable to create Process, error code {3}: {0}, {1}, {2}".FormatWith(userName, domain, password, error);
                    TraceFactory.Logger.Debug(msg);
                    throw new Exception(msg);
                }

                return processInfo.ProcessId;
            }
            catch (Exception ex)
            {
                // TODO: Need to do something here...
                TraceFactory.Logger.Fatal("Failed to create process", ex);
                throw;
            }
        }

        /// <summary>
        /// Impersonates the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domain">The user domain name.</param>
        /// <param name="password">The user password.</param>
        /// <exception cref="Win32Exception">Thrown if impersonation fails.</exception>
        [SecurityCritical]
        public void Impersonate(string userName, string domain, string password)
        {

            TraceFactory.Logger.Debug("Impersonating {0}@{1}".FormatWith(userName, domain));
            IntPtr adminToken = IntPtr.Zero;
            WindowsIdentity adminWId = null;
            bool status = NativeMethods.LogonUser
                (
                    userName,
                    domain,
                    password,
                    NativeMethods.Logon32LogonNewCredentials,
                    NativeMethods.Logon32ProviderDefault,
                    ref adminToken
                );

            if (!status)
            {
                throw new Win32Exception();
            }

            using (adminWId = new WindowsIdentity(adminToken))
            {
                _wic = adminWId.Impersonate();
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_wic != null)
                {
                    _wic.Undo();
                    _wic.Dispose();
                    _wic = null;
                }
            }
        }

        #endregion

        #region DLL Imports

        private class NativeMethods
        {
            internal const int Logon32ProviderDefault = 0;
            internal const int Logon32LogonInteractive = 2;
            internal const int Logon32LogonNewCredentials = 9;

            [Flags]
            internal enum LogonFlags
            {
                LogonWithProfile = 0x00000001,
                LogonNetcredentialsOnly = 0x00000002
            }

            [Flags]
            internal enum CreationFlags
            {
                CreateSuspended = 0x00000004,
                CreateNewConsole = 0x00000010,
                CreateNewProcessGroup = 0x00000200,
                CreateUnicodeEnvironment = 0x00000400,
                CreateSeparateWowVdm = 0x00000800,
                CreateDefaultErrorMode = 0x04000000,
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct ProcessInfo
            {
                public IntPtr ProcessPtr;
                public IntPtr ThreadPtr;
                public uint ProcessId;
                public uint ThreadId;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct StartupInfo
            {
                public int Cb;
                public string Reserved1;
                public string Desktop;
                public string Title;
                public uint X;
                public uint Y;
                public uint XSize;
                public uint YSize;
                public uint XCountChars;
                public uint YCountChars;
                public uint FillAttribute;
                public uint Flags;
                public ushort ShowWindow;
                public short Reserved2;
                public int Reserved3;
                public IntPtr StdInputPtr;
                public IntPtr StdOutputPtr;
                public IntPtr StdErrorPtr;
            }

            [Flags]
            internal enum StartupInfoFlags
            {
                SWHide = 0x00000000,
                StartfUseShowWindow = 0x00000001,
                SWMaximize = 0x00000003,
                SWMinimize = 0x00000006,
            }

            [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool LogonUser(
                string lpszUsername,
                string lpszDomain,
                string lpszPassword,
                int dwLogonType,
                int dwLogonProvider,
                ref IntPtr phToken);

            [DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool CreateProcessWithLogonW(
                string principal,
                string authority,
                string password,
                LogonFlags logonFlags,
                string appName,
                string cmdLine,
                CreationFlags creationFlags,
                IntPtr environmentBlock,
                string currentDirectory,
                ref StartupInfo startupInfo,
                out ProcessInfo processInfo);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool CloseHandle(IntPtr handle);
        }

        #endregion
    }
}
