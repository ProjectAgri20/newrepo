using System;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// Represents a client security context.
    /// </summary>
    public sealed class ClientSecurityContext : SecurityContext
    {
        private readonly string _serverPrinciple;
        private readonly SecurityContextAttributes _requestedAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSecurityContext" /> class.
        /// </summary>
        /// <param name="credential">The security credential to use for authentication.</param>
        /// <param name="serverPrinciple">The principle name of the server to connect to, or null for any.</param>
        /// <param name="requestedAttributes">The desired properties of the context once it is established.</param>
        public ClientSecurityContext(SecurityCredential credential, string serverPrinciple, SecurityContextAttributes requestedAttributes)
            : base(credential)
        {
            _serverPrinciple = serverPrinciple;
            _requestedAttributes = requestedAttributes;
        }

        /// <summary>
        /// Initiates the authentication cycle.
        /// </summary>
        /// <returns>The client's initial authentication token.</returns>
        /// <exception cref="SspiException">
        /// This method was called after the initial token was already acquired.
        /// <para>or</para>
        /// This method was called after this context was fully initialized.
        /// <para>or</para>
        /// The underlying SSPI operation failed.
        /// </exception>
        /// <remarks>
        /// This method is performed to start the authentication cycle with the server.
        /// Each stage works by acquiring a token from one side and presenting it to the other side, which in turn may generate a new token.
        /// The cycle continues until the client and server both indicate that they are done.
        /// </remarks>
        public SspiToken Initialize()
        {
            if (!Handle.IsInvalid)
            {
                throw new SspiException("Must provide the server's response when continuing the initialization process.");
            }

            return Initialize((byte[])null);
        }

        /// <summary>
        /// Performs and/or continues the authentication cycle.
        /// </summary>
        /// <param name="serverToken">The most recent token from the server, or null if beginning the authentication cycle.</param>
        /// <returns>The client's next authentication token in the cycle.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="serverToken" /> is null.</exception>
        /// <exception cref="SspiException">
        /// This method was called before the initial token was initialized.
        /// <para>or</para>
        /// This method was called after this context was fully initialized.
        /// <para>or</para>
        /// The underlying SSPI operation failed.
        /// </exception>
        /// <remarks>
        /// This method is performed iteratively to start, continue, and end the authentication cycle with the server.
        /// Each stage works by acquiring a token from one side and presenting it to the other side, which in turn may generate a new token.
        /// The cycle continues until the client and server both indicate that they are done.
        /// </remarks>
        public SspiToken Initialize(SspiToken serverToken)
        {
            if (serverToken == null)
            {
                throw new ArgumentNullException(nameof(serverToken));
            }

            if (Handle.IsInvalid)
            {
                throw new SspiException("Out-of-order usage detected: server token provided with no previous client token.");
            }

            return Initialize(serverToken.Token);
        }

        private SspiToken Initialize(byte[] serverToken)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(ClientSecurityContext));
            }
            else if (Initialized)
            {
                throw new SspiException("Attempted to call Initialize on a client context that was already initialized.");
            }

            // The security package tells us how long its biggest token will be.
            // Allocate a buffer that size, and it will tell us how much it used.
            SecureBuffer outputBuffer = new SecureBuffer(new byte[Credential.PackageInfo.MaxTokenLength], SecureBufferType.Token);

            SecurityStatus status;
            if (serverToken == null)
            {
                // First time calling initialize - no server token yet
                status = Handle.InitializeSecurityContext(outputBuffer, Credential.Handle, _serverPrinciple, _requestedAttributes);
            }
            else
            {
                // Subsequent calls - server token has been provided
                SecureBuffer serverBuffer = new SecureBuffer(serverToken, SecureBufferType.Token);
                status = Handle.InitializeSecurityContext(outputBuffer, serverBuffer, Credential.Handle, _serverPrinciple, _requestedAttributes);
            }

            if (!status.IsError())
            {
                if (status == SecurityStatus.Ok)
                {
                    Initialized = true;
                }

                return new SspiToken(outputBuffer);
            }
            else
            {
                throw new SspiException("Failed to initialize security context for a client.");
            }
        }
    }
}
