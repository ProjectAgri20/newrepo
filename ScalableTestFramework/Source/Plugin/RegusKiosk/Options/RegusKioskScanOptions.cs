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
    public class RegusKioskScanOptions
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
        public RegusKioskScanDestination ScanDestination { get; set; }

        /// <summary>
        /// Gets or sets the OriginalSize.
        /// </summary>
        [DataMember]
        public RegusKioskOriginalSize OriginalSize { get; set; }

        /// <summary>
        /// Gets or sets the OriginalOriendtation.
        /// </summary>
        [DataMember]
        public RegusKioskOriginalOrientation OriginalOrientation { get; set; }

        /// <summary>
        /// Gets or sets the Duplex.
        /// </summary>
        [DataMember]
        public RegusKioskDuplexSided Duplex { get; set; }

        /// <summary>
        /// Gets or sets the Image Rotation.
        /// </summary>
        [DataMember]
        public RegusKioskImageRotation ImageRotation { get; set; }

        /// <summary>
        /// Gets or sets the FileName.
        /// </summary>
        [DataMember]
        public string StringField { get; set; }
        
        /// <summary>
        /// Gets or sets the ColorMode.
        /// </summary>
        [DataMember]
        public RegusKioskColorMode ColorMode { get; set; }

        /// <summary>
        /// Gets or sets the FileFormat.
        /// </summary>
        [DataMember]
        public RegusKioskFileFormat FileFormat { get; set; }

        /// <summary>
        /// Gets or sets the Resolution.
        /// </summary>
        [DataMember]
        public RegusKioskResolution Resolution { get; set; }

        /// <summary>
        /// Creates new ReguKioskScanOptions
        /// </summary>
        public RegusKioskScanOptions()
        {
            JobBuildPageCount = 1;
            OriginalSize = RegusKioskOriginalSize.A4;
            OriginalOrientation = RegusKioskOriginalOrientation.UprightImages;
            Duplex = RegusKioskDuplexSided.OneSided;
            ColorMode = RegusKioskColorMode.Color;
            FileFormat = RegusKioskFileFormat.PDF;
            Resolution = RegusKioskResolution.e300dpi;
        }
    }
}
