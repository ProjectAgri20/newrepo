using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;

namespace HP.ScalableTest.WindowsAutomation
{
    /// <summary>
    /// Provides a method to perform an action under different user credentials.
    /// </summary>
    public static class UserImpersonator
    {
        /// <summary>
        /// Executes the specified <see cref="Action" /> under the specified <see cref="NetworkCredential" />.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="credential">The credential to impersonate while performing the action.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is null.
        /// <para>or</para>
        /// <paramref name="credential" /> is null.
        /// </exception>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        public static void Execute(Action action, NetworkCredential credential)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            SafeAccessTokenHandle tokenHandle = SafeAccessTokenHandle.InvalidHandle;
            try
            {
                bool success = NativeMethods.LogonUser(credential.UserName, credential.Domain, credential.Password, NativeMethods.LogonType.NewCredentials, NativeMethods.LogonProvider.Default, out tokenHandle);
                if (!success)
                {
                    throw new Win32Exception();
                }

                WindowsIdentity.RunImpersonated(tokenHandle, action);
            }
            finally
            {
                tokenHandle.Dispose();
            }
        }

        private static class NativeMethods
        {
            internal enum LogonType
            {
                NewCredentials = 9
            }

            internal enum LogonProvider
            {
                Default = 0
            }

            [DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, LogonType dwLogonType, LogonProvider dwLogonProvider, out SafeAccessTokenHandle phToken);
        }
    }
}
