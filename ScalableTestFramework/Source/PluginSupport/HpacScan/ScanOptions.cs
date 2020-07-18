using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Enums;

namespace HP.ScalableTest.Plugin.HpacScan
{
    [DataContract]
    public class ScanOptions
    {
        /// <summary>
        /// Gets or sets the PaperSupply 
        /// </summary>
        [DataMember]
        public PaperSupplyType PaperSupplyType { get; set; }

        /// <summary>
        /// Gets or sets the ColorMode 
        /// </summary>
        [DataMember]
        public ColorModeType ColorModeType { get; set; }

        /// <summary>
        /// Gets or sets the Quality 
        /// </summary>
        [DataMember]
        public QualityType QualityType { get; set; }

        /// <summary>
        /// Gets or sets the JobBuild 
        /// </summary>
        [DataMember]
        public bool JobBuild { get; set; }
        public ScanOptions()
        {
            PaperSupplyType = PaperSupplyType.Simplex;
            ColorModeType = ColorModeType.Monochrome;
            QualityType = QualityType.Normal;
            JobBuild = false;
        }
    }
}
