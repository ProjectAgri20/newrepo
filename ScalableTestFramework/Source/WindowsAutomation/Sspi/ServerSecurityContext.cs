using System;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// Represents a server security context.
    /// </summary>
    public sealed class ServerSecurityContext : SecurityContext
    {
        private readonly SecurityContextAttributes _requestedAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSecurityContext" /> class.
        /// </summary>
        /// <param name="credential">The security credential to use for authentication.</param>
        /// <param name="requestedAttributes">The desired properties of the context once it is established.</param>
        public ServerSecurityContext(SecurityCredential credential, SecurityContextAttributes requestedAttributes)
            : base(credential)
        {
            _requestedAttributes = requestedAttributes;
        }

        /// <summary>
        /// Performs and/or continues the authentication cycle.
        /// </summary>
        /// <param name="clientToken">The most recent token from the client.</param>
        /// <returns>The client's next authentication token in the cycle.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="clientToken" /> is null.</exception>
        /// <exception cref="SspiException">
        /// This method was called after this context was fully initialized.
        /// <para>or</para>
        /// The underlying SSPI operation failed.
        /// </exception>
        /// <remarks>
        /// This method is performed iteratively to start, continue, and end the authentication cycle with the server.
        /// Each stage works by acquiring a token from one side and presenting it to the other side, which in turn may generate a new token.
        /// The cycle continues until the client and server both indicate that they are done.
        /// </remarks>
        public SspiToken AcceptToken(SspiToken clientToken)
        {
            if (clientToken == null)
            {
                throw new ArgumentNullException(nameof(clientToken));
            }

            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(SecurityContext));
            }
            else if (Initialized)
            {
                throw new SspiException("Attempted to call Initialize on a server context that was already initialized.");
            }

            SecureBuffer clientBuffer = new SecureBuffer(clientToken.Token, SecureBufferType.Token);
            SecureBuffer outputBuffer = new SecureBuffer(new byte[Credential.PackageInfo.MaxTokenLength], SecureBufferType.Token);

            SecurityStatus status = Handle.AcceptSecurityContext(outputBuffer, clientBuffer, Credential.Handle, _requestedAttributes);

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
                throw new SspiException("Failed to initialize security context for a server.");
            }
        }
    }
}
