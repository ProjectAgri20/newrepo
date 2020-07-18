using System;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// Capabilities of a security package.
    /// </summary>
    [Flags]
    public enum SecurityPackageCapabilities
    {
        /// <summary>
        /// The security package supports generating messsages with integrity information.
        /// Required for the MakeSignature and VerifySignature functions.
        /// </summary>
        Integrity = 0x1,

        /// <summary>
        /// The security package supports generating encrypted messages.
        /// Required for the EncryptMessage and DecryptMessage functions.
        /// </summary>
        Privacy = 0x2,

        /// <summary>
        /// The package is interested only in the security-token portion of messages, and will ignore any other buffers.
        /// This is a performance-related issue.
        /// </summary>
        TokenOnly = 0x4,

        /// <summary>
        /// The package supports datagram-style authentication.
        /// </summary>
        Datagram = 0x8,

        /// <summary>
        /// The package supports connection-oriented style authentication.
        /// </summary>
        Connection = 0x10,

        /// <summary>
        /// Multiple legs are required for authentication.
        /// </summary>
        MultipleLegsRequired = 0x20,

        /// <summary>
        /// Server authentication support is not provided.
        /// </summary>
        ClientOnly = 0x40,

        /// <summary>
        /// The package supports extended error handling.
        /// </summary>
        ExtendedError = 0x80,

        /// <summary>
        /// The package supports Windows impersonation in server contexts.
        /// </summary>
        Impersonation = 0x100,

        /// <summary>
        /// The package understands Windows principal and target names.
        /// </summary>
        AcceptWin32Name = 0x200,

        /// <summary>
        /// The package supports stream semantics.
        /// </summary>
        Stream = 0x400,

        /// <summary>
        /// The package can be used by the Microsoft Negotiate security package.
        /// </summary>
        Negotiable = 0x800,

        /// <summary>
        /// The package supports GSS compatibility.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        GssCompatible = 0x1000,

        /// <summary>
        /// The package supports LsaLogonUser.
        /// </summary>
        LogOn = 0x2000,

        /// <summary>
        /// Token buffers are in ASCII characters format.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        AsciiBuffers = 0x4000,

        /// <summary>
        /// The package supports separating large tokens into smaller buffers.
        /// </summary>
        Fragment = 0x8000,

        /// <summary>
        /// The package supports mutual authentication.
        /// </summary>
        MutualAuthentication = 0x10000,

        /// <summary>
        /// The package supports delegation from the server to a third context.
        /// </summary>
        Delegation = 0x20000,

        /// <summary>
        /// The security package supports using a checksum instead of in-place encryption when calling the EncryptMessage function.
        /// </summary>
        ReadOnlyWithChecksum = 0x40000,

        /// <summary>
        /// The package supports callers with restricted tokens.
        /// </summary>
        RestrictedTokens = 0x80000,

        /// <summary>
        /// The security package extends the Microsoft Negotiate security package. There can be at most one package of this type.
        /// </summary>
        NegotiateExtender = 0x00100000,

        /// <summary>
        /// This package is negotiated by the package of type <see cref="NegotiateExtender" />.
        /// </summary>
        Negotiable2 = 0x00200000,

        /// <summary>
        /// This package receives all calls from app container apps.
        /// </summary>
        AppContainerPassThrough = 0x00400000,

        /// <summary>
        /// This package receives calls from app container apps if the caller has default credentials,
        /// the target is a proxy server, or the caller has supplied credentials.
        /// </summary>
        AppContainerChecks = 0x00800000
    }
}
