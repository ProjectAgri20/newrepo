using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// A security package used for SSPI operations.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public sealed class SecurityPackageInfo
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SecurityPackageCapabilities _capabilities;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private short _version;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private short _rpcId;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _maxTokenLength;

        [MarshalAs(UnmanagedType.LPWStr), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _name;

        [MarshalAs(UnmanagedType.LPWStr), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _comment;

        /// <summary>
        /// Gets the name of the security package.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the capabilities provided by the security package.
        /// </summary>
        public SecurityPackageCapabilities Capabilities => _capabilities;

        /// <summary>
        /// Gets the maximum size, in bytes, of the token.
        /// </summary>
        public int MaxTokenLength => _maxTokenLength;

        /// <summary>
        /// Gets the comment provided by the package.
        /// </summary>
        public string Comment => _comment;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityPackageInfo" /> class.
        /// </summary>
        public SecurityPackageInfo()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Gets a collection of available security packages.
        /// </summary>
        /// <returns>A collection of <see cref="SecurityPackageInfo" /> objects representing the available security packages.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method creates a new object each time it is called.")]
        public static IEnumerable<SecurityPackageInfo> GetSecurityPackages()
        {
            List<SecurityPackageInfo> packages = new List<SecurityPackageInfo>();

            int numPackages = 0;
            IntPtr packageArrayPtr = IntPtr.Zero;

            SecurityStatus result = NativeMethods.EnumerateSecurityPackages(ref numPackages, ref packageArrayPtr);
            if (result != SecurityStatus.Ok)
            {
                throw new Win32Exception();
            }

            if (packageArrayPtr != IntPtr.Zero)
            {
                try
                {
                    for (int i = 0; i < numPackages; i++)
                    {
                        IntPtr packagePointer = IntPtr.Add(packageArrayPtr, i * Marshal.SizeOf<SecurityPackageInfo>());
                        packages.Add(Marshal.PtrToStructure<SecurityPackageInfo>(packagePointer));
                    }
                    return packages;
                }
                finally
                {
                    NativeMethods.FreeContextBuffer(packageArrayPtr);
                }
            }
            else
            {
                throw new SspiException("Failed to enumerate security packages.");
            }
        }

        /// <summary>
        /// Gets information about the specified security package.
        /// </summary>
        /// <param name="packageName">The security package name.</param>
        /// <returns>A <see cref="SecurityPackageInfo" /> object representing the specified security package.</returns>
        /// <exception cref="SspiException">The specified security package could not be found.</exception>
        public static SecurityPackageInfo GetSecurityPackage(SecurityPackage packageName)
        {
            return GetSecurityPackage(packageName.ToString());
        }

        /// <summary>
        /// Gets information about the specified security package.
        /// </summary>
        /// <param name="packageName">The security package name.</param>
        /// <returns>A <see cref="SecurityPackageInfo" /> object representing the specified security package.</returns>
        /// <exception cref="SspiException">The specified security package could not be found.</exception>
        public static SecurityPackageInfo GetSecurityPackage(string packageName)
        {
            IntPtr packagePtr = IntPtr.Zero;

            SecurityStatus result = NativeMethods.QuerySecurityPackageInfo(packageName, ref packagePtr);
            if (result != SecurityStatus.Ok)
            {
                throw new Win32Exception();
            }

            if (packagePtr != IntPtr.Zero)
            {
                try
                {
                    return Marshal.PtrToStructure<SecurityPackageInfo>(packagePtr);
                }
                finally
                {
                    NativeMethods.FreeContextBuffer(packagePtr);
                }
            }
            else
            {
                throw new SspiException($"Could not find security package named {packageName}.");
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Name;
        }

        private static class NativeMethods
        {
            [DllImport("secur32", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus EnumerateSecurityPackages(ref int pcPackages, ref IntPtr ppPackageInfo);

            [DllImport("secur32", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus QuerySecurityPackageInfo(string pszPackageName, ref IntPtr ppPackageInfo);

            [DllImport("secur32", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus FreeContextBuffer(IntPtr buffer);
        }
    }
}
