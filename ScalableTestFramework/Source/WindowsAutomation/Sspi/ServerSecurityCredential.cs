using System;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// Represents the credentials of the user running the current process, for use as an SSPI server.
    /// </summary>
    public sealed class ServerSecurityCredential : SecurityCredential
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSecurityCredential" /> class.
        /// </summary>
        /// <param name="package">The security package to use.</param>
        /// <exception cref="SspiException">The specified security package could not be found.</exception>
        public ServerSecurityCredential(SecurityPackage package)
            : this(SecurityPackageInfo.GetSecurityPackage(package))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSecurityCredential" /> class.
        /// </summary>
        /// <param name="package">The security package to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="package" /> is null.</exception>
        public ServerSecurityCredential(SecurityPackageInfo package)
            : base(package, CredentialUse.Inbound)
        {
        }
    }
}
