using System;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// Represents the credentials of the user running the current process, for use as an SSPI client.
    /// </summary>
    public sealed class ClientSecurityCredential : SecurityCredential
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSecurityCredential" /> class.
        /// </summary>
        /// <param name="package">The security package to use.</param>
        /// <exception cref="SspiException">The specified security package could not be found.</exception>
        public ClientSecurityCredential(SecurityPackage package)
            : this(SecurityPackageInfo.GetSecurityPackage(package))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSecurityCredential" /> class.
        /// </summary>
        /// <param name="package">The security package to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="package" /> is null.</exception>
        public ClientSecurityCredential(SecurityPackageInfo package)
            : base(package, CredentialUse.Outbound)
        {
        }
    }
}
