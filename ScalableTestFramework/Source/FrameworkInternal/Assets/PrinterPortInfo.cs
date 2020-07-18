using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Base class for information about a printer port.
    /// </summary>
    [DataContract]
    public abstract class PrinterPortInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterPortInfo" /> class.
        /// </summary>
        protected PrinterPortInfo()
        {
            // Constructor explicitly declared for XML doc.
        }
    }
}
