using System;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;

namespace HP.ScalableTest.Plugin.ScanToHpcr
{
    /// <summary>
    /// Contains data needed to execute a scan to email through the ScanToHpcr plugin.
    /// </summary>
    [DataContract]
    public class ScanToHpcrActivityData
    {

        [DataMember]
        public virtual DeviceMemoryProfilerConfig DeviceMemoryProfilerConfig { get; set; }

        /// <summary>
        /// Gets or sets a value indicating what HPCR button is to be pressed.
        /// </summary>
        /// <value>
        ///   <c>string</c> Scan To Me; or, <c>Scan To My Files</c>.
        /// </value>
        [DataMember]
        public string HpcrScanButton { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>The page count.</value>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets the automation pause for simulators.
        /// </summary>
        /// <value>The automation pause.</value>
        [DataMember]
        public TimeSpan AutomationPause { get; set; }

        /// <summary>
        /// Gets or sets the destination for scanned document.
        /// </summary>
        /// <value>
        ///   <c>string</c> Scan To Me; or, <c>Scan To My Files</c>.
        /// </value>
        [DataMember]
        public string ScanDestination { get; set; }

        /// <summary>
        /// Gets or sets the scan distribution.
        /// </summary>
        /// <value>
        /// <c>string</c>Public Distributions</c>.
        /// </value>
        [DataMember]
        public string ScanDistribution { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [image preview] is to be used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [image preview]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ImagePreview { get; set; }

        /// <summary>
        /// Gets or sets whether to use the HPCR button or the sign in button for authentication.
        /// </summary>
        /// <value>
        ///   <c>true</c> if using the HPCR button; otherwise, <c>false</c>
        /// </value>
        [DataMember]
        public bool ApplicationAuthentication { get; set; }

        /// <summary>
        /// Gets or sets what authentication provider to use.
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToHpcrActivityData"/> class.
        /// </summary>
        public ScanToHpcrActivityData()
            : base()
        {
            HpcrScanButton = string.Empty;
            ScanDestination = string.Empty;
            ScanDistribution = string.Empty;

            ApplicationAuthentication = false;
            ImagePreview = false;
            PageCount = 1;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5));
            AutomationPause = TimeSpan.FromSeconds(1);
            AuthProvider = AuthenticationProvider.Auto;

            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
        }

    }
}
