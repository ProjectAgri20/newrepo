using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.WindowsAutomation
{
    /// <summary>
    /// Provides methods for adding and removing network connections for the local machine.
    /// </summary>
    public static class NetworkConnection
    {
        /// <summary>
        /// Adds a network connection to the specified location.
        /// </summary>
        /// <param name="shareLocation">The share location.</param>
        /// <param name="credential">The <see cref="NetworkCredential" /> to use to create the connection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="credential" /> is null.</exception>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        public static void AddConnection(string shareLocation, NetworkCredential credential)
        {
            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            LogDebug($"Adding network share {shareLocation}");

            NativeMethods.NetResource netResource = new NativeMethods.NetResource
            {
                Scope = NativeMethods.ResourceScope.GlobalNet,
                Type = NativeMethods.ResourceType.Disk,
                DisplayType = NativeMethods.ResourceDisplayType.Share,
                Usage = NativeMethods.ResourceUsage.Connectable,
                RemoteName = shareLocation
            };

            int result = NativeMethods.WNetAddConnection2(ref netResource, credential.Password, $@"{credential.Domain}\{credential.UserName}", NativeMethods.ConnectionOptions.Temporary);
            if (result != 0)
            {
                throw new Win32Exception();
            }
        }

        /// <summary>
        /// Removes the specified network connection.
        /// </summary>
        /// <param name="connection">The connection to remove.</param>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        public static void RemoveConnection(string connection)
        {
            RemoveConnection(connection, false);
        }

        /// <summary>
        /// Removes the specified network connection.
        /// </summary>
        /// <param name="connection">The connection to remove.</param>
        /// <param name="forceRemoval">if set to <c>true</c> force the removal even if the connection is in use.</param>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        public static void RemoveConnection(string connection, bool forceRemoval)
        {
            int result = NativeMethods.WNetCancelConnection2(connection, 0, forceRemoval ? 1 : 0);
            if (result != 0)
            {
                throw new Win32Exception();
            }
        }

        private static class NativeMethods
        {
            [Flags]
            internal enum ConnectionOptions
            {
                None = 0x0000,
                UpdateProfile = 0x0001,
                UpdateRecent = 0x0002,
                Temporary = 0x0004,
                Interactive = 0x0008,
                Prompt = 0x0010,
                Redirect = 0x0080,
                CurrentMedia = 0x0200,
                CommandLine = 0x0800,
                SaveCredential = 0x1000,
                CredentialReset = 0x2000
            }

            internal enum ResourceScope
            {
                Connected = 1,
                GlobalNet = 2,
                Remembered = 3
            }

            internal enum ResourceType
            {
                Any = 0,
                Disk = 1,
                Print = 2
            }

            internal enum ResourceDisplayType
            {
                Generic = 0,
                Domain = 1,
                Server = 2,
                Share = 3,
                File = 4,
                Group = 5,
                Network = 6,
                Root = 7,
                ShareAdmin = 8,
                Directory = 9,
                Tree = 10,
                NdsContainer = 11
            }

            [Flags]
            internal enum ResourceUsage
            {
                Connectable = 0x0001,
                Container = 0x0002,
                NoLocalDevice = 0x0004,
                Sibling = 0x0008,
                Attached = 0x0010
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct NetResource
            {
                public ResourceScope Scope;
                public ResourceType Type;
                public ResourceDisplayType DisplayType;
                public ResourceUsage Usage;
                public string LocalName;
                public string RemoteName;
                public string Comment;
                public string Provider;
            }

            [DllImport("mpr", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern int WNetAddConnection2(ref NetResource lpNetResource, string lpPassword, string lpUsername, ConnectionOptions dwFlags);

            [DllImport("mpr", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern int WNetCancelConnection2(string name, int flags, int force);
        }
    }
}
