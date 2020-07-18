using System;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.AutoStore
{
    [DataContract]
    public class AutoStoreActivityData
    {

        /// <summary>
        /// Gets or sets a value indicating whether [automatic store authentication].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic store authentication]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool AutoStoreAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the automatic store scan button.
        /// </summary>
        /// <value>
        /// The automatic store scan button.
        /// </value>
        [DataMember]
        public string AutoStoreScanButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [image preview].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [image preview]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ImagePreview { get; set; }

        /// <summary>
        /// Gets or sets the scan distribution.
        /// </summary>
        /// <value>
        /// The scan distribution.
        /// </value>
        [DataMember]
        public string ScanDistribution { get; set; }

        /// <summary>
        /// Gets or sets a value indicating ScanOptions
        /// </summary>
        /// <value>The scan Options</value>
        [DataMember]
        public ScanOptions ScanOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable OCR, if available.
        /// </summary>
        /// <value><c>true</c> if OCR should be enabled; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UseOcr { get; set; }

        /// <summary>
        /// Gets or sets what authentication provider to use.
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }
        /// <summary>
        /// Gets or sets the device memory profiler configuration.
        /// </summary>
        /// <value>The device memory profiler configuration.</value>
        [DataMember]
        public virtual DeviceMemoryProfilerConfig DeviceMemoryProfilerConfig { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoStoreActivityData"/> class.
        /// </summary>
        public AutoStoreActivityData()
        {
            AuthProvider = AuthenticationProvider.AutoStore;
            AutoStoreAuthentication = true;
            AutoStoreScanButton = string.Empty;
            ImagePreview = false;
            ScanDistribution = string.Empty;
            UseOcr = false;

            ScanOptions = new ScanOptions
            {
                PageCount = 1,
                FileType = DeviceAutomation.FileType.DeviceDefault,
                LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5))
                
            };
        }

        [OnDeserialized]
        private void SetValuesOnDeserialized(StreamingContext context)
        {
            if (ScanOptions == null)
            {
                ScanOptions = new ScanOptions();
            }
        }
    }
}
