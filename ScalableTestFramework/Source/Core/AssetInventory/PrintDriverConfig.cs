using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A print driver configuration tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{ConfigFile,nq}")]
    public class PrintDriverConfig
    {
        /// <summary>
        /// Gets or sets the unique identifier for the print driver configuration.
        /// </summary>
        public Guid PrintDriverConfigId { get; set; }

        /// <summary>
        /// Gets or sets the path of the configuration file.
        /// </summary>
        public string ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="PrintDriverProduct" /> to which this driver configuration applies.
        /// </summary>
        public virtual ICollection<PrintDriverProduct> PrintDriverProducts { get; set; } = new HashSet<PrintDriverProduct>();

        /// <summary>
        /// Gets or sets the collection of <see cref="PrintDriverVersion" /> to which this driver configuration applies.
        /// </summary>
        public virtual ICollection<PrintDriverVersion> PrintDriverVersions { get; set; } = new HashSet<PrintDriverVersion>();
    }
}
