using System;
using System.Runtime.InteropServices;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// A managed handle to an SSPI credential.
    /// </summary>
    internal sealed class SafeCredentialHandle : SafeSspiHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeCredentialHandle" /> class.
        /// </summary>
        public SafeCredentialHandle()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Acquires the credential handle.
        /// </summary>
        /// <param name="packageName">The package name to use.</param>
        /// <param name="use">The intended use for the credential.</param>
        /// <exception cref="SspiException">The underlying operation failed.</exception>
        public void AcquireCredentialHandle(string packageName, CredentialUse use)
        {
            Timestamp expiry = new Timestamp();
            SecurityStatus status = NativeMethods.AcquireCredentialsHandle(
                null, packageName, use, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, ref RawHandle, ref expiry);

            if (status != SecurityStatus.Ok)
            {
                throw new SspiException("Failed to acquire credential handle.");
            }
        }

        /// <summary>
        /// Gets the User Principle Name of the credential.
        /// </summary>
        /// <returns>The User Principle Name of the credential.</returns>
        /// <exception cref="SspiException">The underlying operation failed.</exception>
        public string GetPrincipleName()
        {
            return QueryCredentialString(NativeMethods.CredentialsQueryAttribute.Names);
        }

        private string QueryCredentialString(NativeMethods.CredentialsQueryAttribute attribute)
        {
            bool addRefSuccess = false;
            try
            {
                DangerousAddRef(ref addRefSuccess);
            }
            catch
            {
                if (addRefSuccess)
                {
                    DangerousRelease();
                }
                throw;
            }

            string result = null;
            SecurityStatus status = SecurityStatus.InternalError;
            if (addRefSuccess)
            {
                var buffer = new NativeMethods.CredentialsQueryResult();
                status = NativeMethods.QueryCredentialsAttributes(ref RawHandle, attribute, ref buffer);
                DangerousRelease();

                if (status == SecurityStatus.Ok && buffer.Result != IntPtr.Zero)
                {
                    try
                    {
                        result = Marshal.PtrToStringUni(buffer.Result);
                    }
                    finally
                    {
                        NativeMethods.FreeContextBuffer(buffer.Result);
                    }
                }
            }

            if (status.IsError())
            {
                throw new SspiException($"Failed to query credential attribute {attribute}.");
            }

            return result;
        }

        /// <summary>
        /// Executes the code required to free the handle.
        /// </summary>
        /// <returns>true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false.</returns>
        protected override bool ReleaseHandle()
        {
            SecurityStatus status = NativeMethods.FreeCredentialsHandle(ref RawHandle);

            base.ReleaseHandle();
            return status == SecurityStatus.Ok;
        }

        private static class NativeMethods
        {
            internal enum CredentialsQueryAttribute
            {
                Names = 1
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct CredentialsQueryResult
            {
                public IntPtr Result;
            }

            [DllImport("secur32", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus AcquireCredentialsHandle
            (
                string pszPrincipal,
                string pszPackage,
                CredentialUse fCredentialUse,
                IntPtr pvLogonID,
                IntPtr pAuthData,
                IntPtr pGetKeyFn,
                IntPtr pvGetKeyArgument,
                ref RawSspiHandle phCredential,
                ref Timestamp ptsExpiry
            );

            [DllImport("secur32", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus QueryCredentialsAttributes
            (
                ref RawSspiHandle phCredential,
                CredentialsQueryAttribute ulAttribute,
                ref CredentialsQueryResult pBuffer
            );

            [DllImport("secur32", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus FreeCredentialsHandle(ref RawSspiHandle phCredential);

            [DllImport("secur32", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus FreeContextBuffer(IntPtr pvContextBuffer);
        }
    }
}
