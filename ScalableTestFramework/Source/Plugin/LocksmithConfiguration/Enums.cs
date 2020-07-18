using System.ComponentModel;

namespace HP.ScalableTest.Plugin.LocksmithConfiguration.ActivityData
{
    /// <summary>
    /// Printer Discovery Options
    /// </summary>
    public enum PrinterDiscovery
    {
        None,

        /// <summary>
        /// Manual discovery type operation
        /// </summary>
        [Description("Manual")]
        ManualIPAddress,

        /// <summary>
        /// Manual discovery type operation using a File
        /// </summary>
        [Description("Add From File")]
        DeviceListFile,

        /// <summary>
        /// Automatic number of hops discovery type operation
        /// </summary>
        [Description("Automatic")]
        AutomaticHops,

        /// <summary>
        /// IP Range discovery type operation
        /// </summary>
        [Description("Automatic")]
        AutomaticRange,

        /// <summary>
        /// Fetch and devices from Asset Inventory
        /// </summary>
        [Description("Add Devices From AssetInventory")]
        AssetInventory
    }

    /// <summary>
    /// Browser type
    /// </summary>
    public enum BrowserType
    {
        /// <summary>
        /// Internet Explorer
        /// </summary>
        [Description("Internet Explorer")]
        InternetExplorer,

        /// <summary>
        /// Google Chrome
        /// </summary>
        [Description("Google Chrome")]
        GoogleChrome
    }
}
