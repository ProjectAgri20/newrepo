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
    public class RegusKioskCopyOptions
    {
        /// <summary>
        /// Gets or sets the Job Build PageCount.
        /// </summary>
        [DataMember]
        public int JobBuildPageCount { get; set; }

        /// <summary>
        /// Gets or sets the Number of Copies.
        /// </summary>
        [DataMember]
        public int NumberOfCopies { get; set; }

        /// <summary>
        /// Gets or sets the ColorMode.
        /// </summary>
        [DataMember]
        public RegusKioskColorMode ColorMode { get; set; }

        /// <summary>
        /// Gets or sets the OriginalSize.
        /// </summary>
        [DataMember]
        public RegusKioskOriginalSize OriginalSize { get; set; }

        /// <summary>
        /// Gets or sets the Nup.
        /// </summary>
        [DataMember]
        public RegusKioskNUp Nup { get; set; }

        /// <summary>
        /// Gets or sets the OriginalOrientation.
        /// </summary>
        [DataMember]
        public RegusKioskOriginalOrientation OriginalOrientation { get; set; }

        /// <summary>
        /// Gets or sets the DuplexOriginal.
        /// </summary>
        [DataMember]
        public RegusKioskDuplexSided DuplexOriginal { get; set; }

        /// <summary>
        /// Gets or sets the DuplexOutput.
        /// </summary>
        [DataMember]
        public RegusKioskDuplexSided DuplexOutput { get; set; }

        /// <summary>
        /// Gets or sets the ReduceEnlarge.
        /// </summary>
        [DataMember]
        public string ReduceEnlarge { get; set; }

        /// <summary>
        /// Creates new RegusKioskCopyOptions
        /// </summary>
        public RegusKioskCopyOptions()
        {
            JobBuildPageCount = 1;
            NumberOfCopies = 1;
            ColorMode = RegusKioskColorMode.Mono;
            OriginalSize = RegusKioskOriginalSize.A4;
            Nup = RegusKioskNUp.OneUp;
            OriginalOrientation = RegusKioskOriginalOrientation.UprightImages;
            DuplexOriginal = RegusKioskDuplexSided.OneSided;
            DuplexOutput = RegusKioskDuplexSided.OneSided;
            ReduceEnlarge = null;
        }
    }
}
