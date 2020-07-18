using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A DART board tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{DartBoardId,nq}")]
    public sealed class DartBoard
    {
        /// <summary>
        /// Gets or sets the unique identifier for the DART board.
        /// </summary>
        public string DartBoardId { get; set; }

        /// <summary>
        /// Gets or sets the DART board network address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the DART board firmware version.
        /// </summary>
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// Gets or sets the asset ID of the associated print device.
        /// </summary>
        public string PrinterId { get; set; }
    }
}
