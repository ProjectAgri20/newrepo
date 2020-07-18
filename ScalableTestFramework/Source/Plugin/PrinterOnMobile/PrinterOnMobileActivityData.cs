using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.PrinterOnMobile
{
    [DataContract]
    public class PrinterOnMobileActivityData
    {
        /// <summary>
        /// Gets or sets the Printer id
        /// </summary>
        [DataMember]
        public string PrinterId { get; set; }

        /// <summary>
        /// Gets or sets the file path
        /// </summary>
        [DataMember]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Print Options
        /// </summary>
        [DataMember]
        public PrinterOnJobOptions Options { get; set; }

        public PrinterOnMobileActivityData()
        {
            Options = new PrinterOnJobOptions();           
        }
    }
}
