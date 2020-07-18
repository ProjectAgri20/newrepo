using System;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// Defines options for creating a <see cref="SecurityContext" />.
    /// Required attribute flags are specified when creating the context.
    /// </summary>
    [Flags]
    public enum SecurityContextAttributes
    {
        /// <summary>
        /// No attributes are provided.
        /// </summary>
        None = 0,

        /// <summary>
        /// The server can use the context to authenticate to other servers as the client.
        /// The <see cref="MutualAuthentication" /> flag must be set for this flag to work.
        /// </summary>
        Delegate = 0x0001,

        /// <summary>
        /// The mutual authentication policy of the service will be satisfied.
        /// </summary>
        MutualAuthentication = 0x0002,

        /// <summary>
        /// Detect replayed messages that have been encoded by using EncryptMessage or MakeSignature.
        /// </summary>
        ReplayDetect = 0x0004,

        /// <summary>
        /// Detect messages received out of sequence when using the message support functionality.
        /// </summary>
        SequenceDetect = 0x0008,

        /// <summary>
        /// The context must protect data while in transit.
        /// </summary>
        Confidentiality = 0x0010,

        /// <summary>
        /// A new session key must be negotiated.
        /// </summary>
        UseSessionKey = 0x0020,

        /// <summary>
        /// The security package allocates output buffers for you.  Buffers allocated by the security
        /// package have to be released by the context memory management functions.
        /// </summary>
        AllocateMemory = 0x0100,

        /// <summary>
        /// The security context will not handle formatting messages.
        /// </summary>
        Connection = 0x0800
    }
}
