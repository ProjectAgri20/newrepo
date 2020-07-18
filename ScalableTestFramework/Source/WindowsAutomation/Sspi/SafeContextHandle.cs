using System;
using System.Runtime.InteropServices;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// A managed handle to an SSPI context.
    /// </summary>
    internal sealed class SafeContextHandle : SafeSspiHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeContextHandle" /> class.
        /// </summary>
        public SafeContextHandle()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Gets the logon user name that the context represents.
        /// </summary>
        /// <returns>The logon user name that the context represents.</returns>
        /// <exception cref="SspiException">The underlying operation failed.</exception>
        public string GetContextUserName()
        {
            return QueryContextString(NativeMethods.ContextQueryAttribute.Names);
        }

        /// <summary>
        /// Gets the name of the authenticating authority for the context.
        /// </summary>
        /// <returns>The name of the authenticating authority for the context.</returns>
        /// <exception cref="SspiException">The underlying operation failed.</exception>
        public string GetAuthorityName()
        {
            return QueryContextString(NativeMethods.ContextQueryAttribute.Authority);
        }

        /// <summary>
        /// Initializes the client-side security context from a credental handle.
        /// </summary>
        /// <param name="outputBuffer">The buffer that will hold the output token.</param>
        /// <param name="credential">The credential handle.</param>
        /// <param name="serverPrincipal">The server principal.</param>
        /// <param name="requestedAttributes">The requested attributes.</param>
        /// <returns>A <see cref="SecurityStatus" /> representing the result of the operation.</returns>
        public SecurityStatus InitializeSecurityContext(SecureBuffer outputBuffer, SafeCredentialHandle credential, string serverPrincipal, SecurityContextAttributes requestedAttributes)
        {
            SecurityContextAttributes finalAttributes = SecurityContextAttributes.None;
            Timestamp expiry = new Timestamp();

            using (SecureBufferAdapter outputAdapter = new SecureBufferAdapter(outputBuffer))
            {
                return NativeMethods.InitializeSecurityContext_1
                (
                    ref credential.RawHandle,
                    IntPtr.Zero,
                    serverPrincipal,
                    requestedAttributes,
                    0,
                    NativeMethods.SecureBufferDataRep.Network,
                    IntPtr.Zero,
                    0,
                    ref RawHandle,
                    outputAdapter.Handle,
                    ref finalAttributes,
                    ref expiry
                );
            }
        }

        /// <summary>
        /// Initializes the client-side security context from a credental handle.
        /// </summary>
        /// <param name="outputBuffer">The buffer that will hold the output token.</param>
        /// <param name="serverBuffer">The buffer containing the input data from the server.</param>
        /// <param name="credential">The credential handle.</param>
        /// <param name="serverPrincipal">The server principal.</param>
        /// <param name="requestedAttributes">The requested attributes.</param>
        /// <returns>A <see cref="SecurityStatus" /> representing the result of the operation.</returns>
        public SecurityStatus InitializeSecurityContext(SecureBuffer outputBuffer, SecureBuffer serverBuffer, SafeCredentialHandle credential, string serverPrincipal, SecurityContextAttributes requestedAttributes)
        {
            SecurityContextAttributes finalAttributes = SecurityContextAttributes.None;
            Timestamp expiry = new Timestamp();

            using (SecureBufferAdapter outputAdapter = new SecureBufferAdapter(outputBuffer))
            {
                using (SecureBufferAdapter serverAdapter = new SecureBufferAdapter(serverBuffer))
                {
                    return NativeMethods.InitializeSecurityContext_2
                    (
                        ref credential.RawHandle,
                        ref RawHandle,
                        serverPrincipal,
                        requestedAttributes,
                        0,
                        NativeMethods.SecureBufferDataRep.Network,
                        serverAdapter.Handle,
                        0,
                        ref RawHandle,
                        outputAdapter.Handle,
                        ref finalAttributes,
                        ref expiry
                    );
                }
            }
        }

        /// <summary>
        /// Establishes a security context between the server and a remote client.
        /// </summary>
        /// <param name="outputBuffer">The buffer that will hold the output token.</param>
        /// <param name="clientBuffer">The buffer containing the input data from the client.</param>
        /// <param name="credential">The credential handle.</param>
        /// <param name="requestedAttributes">The requested attributes.</param>
        /// <returns>A <see cref="SecurityStatus" /> representing the result of the operation.</returns>
        public SecurityStatus AcceptSecurityContext(SecureBuffer outputBuffer, SecureBuffer clientBuffer, SafeCredentialHandle credential, SecurityContextAttributes requestedAttributes)
        {
            SecurityContextAttributes finalAttributes = SecurityContextAttributes.None;
            Timestamp expiry = new Timestamp();

            using (SecureBufferAdapter outputAdapter = new SecureBufferAdapter(outputBuffer))
            {
                using (SecureBufferAdapter clientAdapter = new SecureBufferAdapter(clientBuffer))
                {
                    if (this.IsInvalid)
                    {
                        return NativeMethods.AcceptSecurityContext_1
                        (
                            ref credential.RawHandle,
                            IntPtr.Zero,
                            clientAdapter.Handle,
                            requestedAttributes,
                            NativeMethods.SecureBufferDataRep.Network,
                            ref RawHandle,
                            outputAdapter.Handle,
                            ref finalAttributes,
                            ref expiry
                        );
                    }
                    else
                    {
                        return NativeMethods.AcceptSecurityContext_2
                        (
                            ref credential.RawHandle,
                            ref RawHandle,
                            clientAdapter.Handle,
                            requestedAttributes,
                            NativeMethods.SecureBufferDataRep.Network,
                            ref RawHandle,
                            outputAdapter.Handle,
                            ref finalAttributes,
                            ref expiry
                        );
                    }
                }
            }
        }

        private string QueryContextString(NativeMethods.ContextQueryAttribute attribute)
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
                var buffer = new NativeMethods.ContextQueryResult();
                status = NativeMethods.QueryContextAttributes(ref RawHandle, attribute, ref buffer);
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

            if (status == SecurityStatus.Unsupported)
            {
                return null;
            }
            else if (status.IsError())
            {
                throw new SspiException($"Failed to query context attribute {attribute}.");
            }

            return result;
        }

        /// <summary>
        /// Executes the code required to free the handle.
        /// </summary>
        /// <returns>true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false.</returns>
        protected override bool ReleaseHandle()
        {
            SecurityStatus status = NativeMethods.DeleteSecurityContext(ref RawHandle);

            base.ReleaseHandle();
            return status == SecurityStatus.Ok;
        }

        private static class NativeMethods
        {
            internal enum ContextQueryAttribute
            {
                Sizes = 0,
                Names = 1,
                Authority = 6,
            }

            internal enum SecureBufferDataRep
            {
                Network = 0
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct ContextQueryResult
            {
                public IntPtr Result;
            }

            [DllImport("secur32", EntryPoint = "InitializeSecurityContext", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus InitializeSecurityContext_1
            (
                ref RawSspiHandle phCredential,
                IntPtr zero,
                string pszTargetName,
                SecurityContextAttributes fContextReq,
                int reserved1,
                SecureBufferDataRep targetDataRep,
                IntPtr pInput,
                int reserved2,
                ref RawSspiHandle phNewContext,
                IntPtr pOutput,
                ref SecurityContextAttributes pfContextAttr,
                ref Timestamp ptsExpiry
            );

            [DllImport("secur32", EntryPoint = "InitializeSecurityContext", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus InitializeSecurityContext_2
            (
                ref RawSspiHandle phCredential,
                ref RawSspiHandle phContext,
                string pszTargetName,
                SecurityContextAttributes fContextReq,
                int reserved1,
                SecureBufferDataRep targetDataRep,
                IntPtr pInput,
                int reserved2,
                ref RawSspiHandle phNewContext,
                IntPtr pOutput,
                ref SecurityContextAttributes pfContextAttr,
                ref Timestamp ptsExpiry
            );

            [DllImport("secur32", EntryPoint = "AcceptSecurityContext", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus AcceptSecurityContext_1
            (
                ref RawSspiHandle phCredential,
                IntPtr phContext,
                IntPtr pInput,
                SecurityContextAttributes fContextReq,
                SecureBufferDataRep targetDataRep,
                ref RawSspiHandle phNewContext,
                IntPtr pOutput,
                ref SecurityContextAttributes pfContextAttr,
                ref Timestamp ptsTimeStamp
            );

            [DllImport("secur32", EntryPoint = "AcceptSecurityContext", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus AcceptSecurityContext_2
            (
                ref RawSspiHandle phCredential,
                ref RawSspiHandle phContext,
                IntPtr pInput,
                SecurityContextAttributes fContextReq,
                SecureBufferDataRep targetDataRep,
                ref RawSspiHandle phNewContext,
                IntPtr pOutput,
                ref SecurityContextAttributes pfContextAttr,
                ref Timestamp ptsTimeStamp
            );

            [DllImport("secur32", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus QueryContextAttributes
            (
                ref RawSspiHandle phContext,
                ContextQueryAttribute ulAttribute,
                ref ContextQueryResult pBuffer
            );

            [DllImport("secur32", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus DeleteSecurityContext(ref RawSspiHandle phContext);

            [DllImport("secur32", CharSet = CharSet.Unicode)]
            internal static extern SecurityStatus FreeContextBuffer(IntPtr pvContextBuffer);

        }
    }
}
