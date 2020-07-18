using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A print driver version tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{Value,nq}")]
    public sealed class PrintDriverVersion
    {
        /// <summary>
        /// Gets or sets the unique identifier for the print driver version.
        /// </summary>
        public Guid PrintDriverVersionId { get; set; }

        /// <summary>
        /// Gets or sets the value of the print driver version.
        /// </summary>
        public string Value { get; set; }
    }
}
