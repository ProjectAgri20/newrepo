using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A print driver package tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class PrintDriverPackage
    {
        /// <summary>
        /// Gets or sets unique identifier for the print driver package.
        /// </summary>
        public Guid PrintDriverPackageId { get; set; }

        /// <summary>
        /// Gets or sets the print driver package name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location for the x86 INF.
        /// </summary>
        public string InfX86 { get; set; }

        /// <summary>
        /// Gets or sets the location for the x64 INF.
        /// </summary>
        public string InfX64 { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PrintDriver" /> collection for this print driver package.
        /// </summary>
        public virtual ICollection<PrintDriver> PrintDrivers { get; set; } = new HashSet<PrintDriver>();
    }
}
