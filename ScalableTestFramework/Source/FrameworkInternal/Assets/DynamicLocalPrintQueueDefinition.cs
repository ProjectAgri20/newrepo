using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// A minimal identifier used to save <see cref="DynamicLocalPrintQueueInfo" />.
    /// </summary>
    [DataContract]
    public class DynamicLocalPrintQueueDefinition : PrintQueueDefinition
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _assetId;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Guid _printDriverId;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PrinterPortInfo _printerPort;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PrintDriverConfiguration _printDriverConfiguration;

        /// <summary>
        /// Gets the asset identifier for the device to use for this print queue.
        /// </summary>
        public string AssetId => _assetId;

        /// <summary>
        /// Gets the ID of the print driver to use for this print queue.
        /// </summary>
        public Guid PrintDriverId => _printDriverId;

        /// <summary>
        /// Gets information about the port to use for this print queue.
        /// </summary>
        public PrinterPortInfo PrinterPort => _printerPort;

        /// <summary>
        /// Gets the print driver configuration file.
        /// </summary>
        public PrintDriverConfiguration PrintDriverConfiguration => _printDriverConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicLocalPrintQueueDefinition" /> class.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <param name="printDriverId">The print driver identifier.</param>
        /// <param name="printerPort">The <see cref="PrinterPortInfo" /> information.</param>
        public DynamicLocalPrintQueueDefinition(string assetId, Guid printDriverId, PrinterPortInfo printerPort)
        {
            _assetId = assetId;
            _printDriverId = printDriverId;
            _printerPort = printerPort;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicLocalPrintQueueDefinition" /> class.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <param name="printDriverId">The print driver identifier.</param>
        /// <param name="printerPort">The <see cref="PrinterPortInfo" /> information.</param>
        /// <param name="printDriverConfiguration">The print driver configuration.</param>
        public DynamicLocalPrintQueueDefinition(string assetId, Guid printDriverId, PrinterPortInfo printerPort, PrintDriverConfiguration printDriverConfiguration)
            : this(assetId, printDriverId, printerPort)
        {
            _printDriverConfiguration = printDriverConfiguration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicLocalPrintQueueDefinition" /> class.
        /// </summary>
        /// <param name="printQueueInfo">The <see cref="DynamicLocalPrintQueueInfo" /> whose information should be stored.</param>
        /// <exception cref="ArgumentNullException"><paramref name="printQueueInfo" /> is null.</exception>
        public DynamicLocalPrintQueueDefinition(DynamicLocalPrintQueueInfo printQueueInfo)
        {
            if (printQueueInfo == null)
            {
                throw new ArgumentNullException(nameof(printQueueInfo));
            }

            _assetId = printQueueInfo.AssociatedAssetId;
            _printDriverId = printQueueInfo.PrintDriver.PrintDriverId;
            _printerPort = printQueueInfo.PrinterPort;
            _printDriverConfiguration = printQueueInfo.PrintDriverConfiguration;
        }
    }
}
