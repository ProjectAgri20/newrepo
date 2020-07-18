using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.Kiosk.Options
{
    [DataContract]
    public class KioskScanOptions
    {
        /// <summary>
        /// Gets or sets the Job Build PageCount.
        /// </summary>
        [DataMember]
        public int JobBuildPageCount { get; set; }

        /// <summary>
        /// Gets or sets the ScanDestination.
        /// </summary>
        [DataMember]
        public KioskScanDestination ScanDestination { get; set; }

        /// <summary>
        /// Gets or sets the OriginalSize.
        /// </summary>
        [DataMember]
        public KioskOriginalSize OriginalSize { get; set; }

        /// <summary>
        /// Gets or sets the OriginalOriendtation.
        /// </summary>
        [DataMember]
        public KioskOriginalOrientation OriginalOrientation { get; set; }

        /// <summary>
        /// Gets or sets the Duplex.
        /// </summary>
        [DataMember]
        public KioskDuplexSided Duplex { get; set; }

        /// <summary>
        /// Gets or sets the FileName.
        /// </summary>
        [DataMember]
        public string StringField { get; set; }
        
        /// <summary>
        /// Gets or sets the ColorMode.
        /// </summary>
        [DataMember]
        public KioskColorMode ColorMode { get; set; }

        /// <summary>
        /// Gets or sets the FileFormat.
        /// </summary>
        [DataMember]
        public KioskFileFormat FileFormat { get; set; }

        /// <summary>
        /// Gets or sets the Resolution.
        /// </summary>
        [DataMember]
        public KioskResolution Resolution { get; set; }

        /// <summary>
        /// Creates new KioskScanOptions
        /// </summary>
        public KioskScanOptions()
        {
            JobBuildPageCount = 1;
            OriginalSize = KioskOriginalSize.A4;
            OriginalOrientation = KioskOriginalOrientation.UprightImages;
            Duplex = KioskDuplexSided.OneSided;
            ColorMode = KioskColorMode.Color;
            FileFormat = KioskFileFormat.PDF;
            Resolution = KioskResolution.e300dpi;
        }
    }
}
