using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace HP.ScalableTest.Plugin.Kiosk.Options
{
    [DataContract]
    public class KioskCopyOptions
    {
        /// <summary>
        /// Gets or sets the Job Build PageCount.
        /// </summary>
        [DataMember]
        public int JobBuildPageCount { get; set; }

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
        /// Gets or sets the OriginalSize.
        /// </summary>
        [DataMember]
        public KioskOriginalSize OriginalSize { get; set; }

        /// <summary>
        /// Gets or sets the Nup.
        /// </summary>
        [DataMember]
        public KioskNUp Nup { get; set; }

        /// <summary>
        /// Gets or sets the OriginalOrientation.
        /// </summary>
        [DataMember]
        public KioskOriginalOrientation OriginalOrientation { get; set; }

        /// <summary>
        /// Gets or sets the DuplexOriginal.
        /// </summary>
        [DataMember]
        public KioskDuplexSided DuplexOriginal { get; set; }

        /// <summary>
        /// Gets or sets the DuplexOutput.
        /// </summary>
        [DataMember]
        public KioskDuplexSided DuplexOutput { get; set; }

        /// <summary>
        /// Gets or sets the ReduceEnlarge.
        /// </summary>
        [DataMember]
        public int ReduceEnlargeIndex { get; set; }

        /// <summary>
        /// Creates new KioskCopyOptions
        /// </summary>
        public KioskCopyOptions()
        {
            JobBuildPageCount = 1;
            PageCount = 1;
            ColorMode = KioskColorMode.Mono;
            OriginalSize = KioskOriginalSize.A4;
            Nup = KioskNUp.OneUp;
            OriginalOrientation = KioskOriginalOrientation.UprightImages;
            DuplexOriginal = KioskDuplexSided.OneSided;
            DuplexOutput = KioskDuplexSided.OneSided;
            ReduceEnlargeIndex = 0;
        }
    }
}
