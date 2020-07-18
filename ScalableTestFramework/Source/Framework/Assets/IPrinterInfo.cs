namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a network-enabled printer.
    /// </summary>
    public interface IPrinterInfo : IAssetInfo
    {
        /// <summary>
        /// Gets the network address (IP or hostname) of the printer.
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Gets the port used for printing to the printer.
        /// </summary>
        int PrinterPort { get; }

        /// <summary>
        /// Gets a value indicating whether SNMP is enabled for the printer.
        /// </summary>
        bool SnmpEnabled { get; }
    }
}
