using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.Kiosk.Options
{
    [DataContract]
    public class KioskPrintOptions
    {
        /// <summary>
        /// Gets or sets the Path for print file.
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the PrintSource.
        /// </summary>
        [DataMember]
        public KioskPrintSource PrintSource { get; set; }

        /// <summary>
        /// Gets or sets the PageCount.
        /// </summary>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the ColorMode.
        /// </summary>
        [DataMember]
        public KioskColorMode ColorMode { get; set; }

        /// <summary>
        /// Gets or sets the Duplex.
        /// </summary>
        [DataMember]
        public KioskDuplexPrint Duplex { get; set; }

        /// <summary>
        /// Gets or sets the PaperSource.
        /// </summary>
        [DataMember]
        public KioskOriginalSize PaperSource { get; set; }

        /// <summary>
        /// Gets or sets the AutoFit.
        /// </summary>
        [DataMember]
        public bool AutoFit { get; set; }

        /// <summary>
        /// Creates new KioskPrintOptions
        /// </summary>
        public KioskPrintOptions()
        {
            Path = "";
            PrintSource = KioskPrintSource.PrinterOn;
            PageCount = 1;
            ColorMode = KioskColorMode.Mono;
            Duplex = KioskDuplexPrint.Off;
            PaperSource = KioskOriginalSize.A4;
            AutoFit = true;
        }
    }
}
