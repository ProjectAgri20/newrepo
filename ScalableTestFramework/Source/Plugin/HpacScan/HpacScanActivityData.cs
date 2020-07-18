using System;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.DeviceAutomation.HpacScan;

namespace HP.ScalableTest.Plugin.HpacScan
{
    [DataContract]
    public class HpacScanActivityData
    {
        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets the device memory profiler configuration.
        /// </summary>
        /// <value>The device memory profiler configuration.</value>
        [DataMember]
        public virtual DeviceMemoryProfilerConfig DeviceMemoryProfilerConfig { get; set; }

        /// <summary>
        /// Gets or sets the AuthProvider 
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

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

        /// <summary>
        /// Gets or sets the ScanCount 
        /// </summary>
        [DataMember]
        public int ScanCount { get; set; }

        public HpacScanActivityData()
        {
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
            AuthProvider = AuthenticationProvider.HpacScan;
            PaperSupplyType = PaperSupplyType.Simplex;
            ColorModeType = ColorModeType.Monochrome;
            QualityType = QualityType.Normal;
            JobBuild = false;
            ScanCount = 1;
        }
    }
}

