using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.EPrint
{
    [DataContract]
    public class EPrintActivityData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ePrintActivityData"/> class.
        /// </summary>
        public EPrintActivityData()
        {
            PrinterEmail = string.Empty;
            NumberOfDocuments = 1;
        }

        /// <summary>
        /// Gets or sets the ePrint Printer Email.
        /// </summary>
        [DataMember]
        public string PrinterEmail { get; set; }

        /// <summary>
        /// Gets or sets the number of documents to include in the attachment.
        /// </summary>
        [DataMember]
        public int NumberOfDocuments { get; set; }


    }
}
