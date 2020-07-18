using System;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A monitor configuration tracked by Asset Inventory.
    /// </summary>
    public sealed class MonitorConfig
    {
        /// <summary>
        /// Gets or sets the unique identifier for the monitor configuration.
        /// </summary>
        public Guid MonitorConfigId { get; set; }

        /// <summary>
        /// Gets or sets the host name of the server where the monitor is hosted.
        /// </summary>
        public string ServerHostName { get; set; }

        /// <summary>
        /// Gets or sets the monitor type.
        /// </summary>
        public string MonitorType { get; set; }

        /// <summary>
        /// Gets or sets the monitor configuration data.
        /// </summary>
        public string Configuration { get; set; }
    }
}
