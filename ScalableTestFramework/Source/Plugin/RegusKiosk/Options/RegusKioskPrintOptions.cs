using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.LinkApps.Kiosk.RegusKiosk;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.RegusKiosk.Options
{
    [DataContract]
    public class RegusKioskPrintOptions
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
        public RegusKioskPrintSource PrintSource { get; set; }

        /// <summary>
        /// Gets or sets the PageCount.
        /// </summary>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the ColorMode.
        /// </summary>
        [DataMember]
        public RegusKioskColorMode ColorMode { get; set; }

        /// <summary>
        /// Gets or sets the Duplex.
        /// </summary>
        [DataMember]
        public RegusKioskDuplexPrint Duplex { get; set; }

        /// <summary>
        /// Gets or sets the PaperSource.
        /// </summary>
        [DataMember]
        public RegusKioskOriginalSize PaperSource { get; set; }

        /// <summary>
        /// Gets or sets the AutoFit.
        /// </summary>
        [DataMember]
        public bool AutoFit { get; set; }

        /// <summary>
        /// Creates new RegusKioskPrintOptions
        /// </summary>
        public RegusKioskPrintOptions()
        {
            Path = "";
            PrintSource = RegusKioskPrintSource.PrinterOn;
            PageCount = 1;
            ColorMode = RegusKioskColorMode.Mono;
            Duplex = RegusKioskDuplexPrint.Off;
            PaperSource = RegusKioskOriginalSize.A4;
            AutoFit = true;
        }
    }
}
