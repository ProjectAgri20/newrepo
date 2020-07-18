using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a printer port using the RAW protocol.
    /// </summary>
    [DataContract]
    public class RawPrinterPortInfo : PrinterPortInfo
    {
        /// <summary>
        /// The default port number for a printer port using the RAW protocol.
        /// </summary>
        public static readonly int DefaultPortNumber = 9100;

        /// <summary>
        /// Initializes a new instance of the <see cref="RawPrinterPortInfo" /> class.
        /// </summary>
        public RawPrinterPortInfo()
        {
            // Constructor explicitly declared for XML doc.
        }
    }
}
