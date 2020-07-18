using System;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// An association between a <see cref="SoftwareInstallerPackage" /> and a <see cref="EnterpriseTest.SoftwareInstaller" />.
    /// </summary>
    public class SoftwareInstallerPackageItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for the associated software installer package.
        /// </summary>
        public Guid SoftwareInstallerPackageId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated software installer.
        /// </summary>
        public Guid SoftwareInstallerId { get; set; }

        /// <summary>
        /// Gets or sets the order of the installer within the package.
        /// </summary>
        public int InstallOrder { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="EnterpriseTest.SoftwareInstaller" />.
        /// </summary>
        public virtual SoftwareInstaller SoftwareInstaller { get; set; }
    }
}
