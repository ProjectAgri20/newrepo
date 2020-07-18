using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// A package of software installers that is associated with a <see cref="ResourceType" /> or <see cref="MetadataType" />.
    /// </summary>
    [DebuggerDisplay("{Description,nq}")]
    public class SoftwareInstallerPackage
    {
        /// <summary>
        /// Gets or sets the unique identifier for the software installer package.
        /// </summary>
        public Guid SoftwareInstallerPackageId { get; set; }

        /// <summary>
        /// Gets or sets the installer package description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="SoftwareInstallerPackageItem" /> objects that are part of this package.
        /// </summary>
        public virtual ICollection<SoftwareInstallerPackageItem> SoftwareInstallerPackageItems { get; set; } = new HashSet<SoftwareInstallerPackageItem>();

        /// <summary>
        /// Gets or sets the collection of <see cref="ResourceType" /> objects that are associated with this installer package.
        /// </summary>
        public virtual ICollection<ResourceType> ResourceTypes { get; set; } = new HashSet<ResourceType>();

        /// <summary>
        /// Gets or sets the collection of <see cref="MetadataType" /> objects that are associated with this installer package.
        /// </summary>
        public virtual ICollection<MetadataType> MetadataTypes { get; set; } = new HashSet<MetadataType>();
    }
}
