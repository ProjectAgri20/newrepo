using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a printer port using the LPR protocol.
    /// </summary>
    [DataContract]
    public class LprPrinterPortInfo : PrinterPortInfo
    {
        /// <summary>
        /// The default port number for a printer port using the LPR protocol.
        /// </summary>
        public static readonly int DefaultPortNumber = 515;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _queueName;

        /// <summary>
        /// Gets the LPR queue name for this port.
        /// </summary>
        public string QueueName => _queueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="LprPrinterPortInfo" /> class.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        public LprPrinterPortInfo(string queueName)
        {
            _queueName = queueName;
        }
    }
}
