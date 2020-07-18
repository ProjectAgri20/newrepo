using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
namespace HP.ScalableTest.Plugin.ScanToSafeQ
{
    [DataContract]
    public class ScanToSafeQActivityData
    {
        public ScanToSafeQActivityData()
        {
            SafeQFileType = ScanToSafeQFileType.PDF;
            ScanCount = 1;
            WorkFlowDescription = null;
            PrintAll = false;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
        }

        /// <summary>
        /// Gets or sets the document process action (see <see cref="CelioveoPrintAction" />.)
        /// </summary>
        /// <value>The document process action.</value>
        [DataMember]
        public ScanToSafeQFileType SafeQFileType { get; set; }

        /// <summary>
        /// Gets or sets the Scan Count
        /// </summary>
        /// <value>The document process action.</value>
        [DataMember]
        public int ScanCount { get; set; }

        /// <summary>
        /// Gets or sets the WorkFlowDescription for scanning.
        /// </summary>
        [DataMember]
        public string WorkFlowDescription { get; set; }

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
        /// Gets or sets whether to initiate authentication via the SafeQ button.
        /// <value><c>true</c> if [use SafeQ Scan button]; otherwise, <c>false</c>.</value>
        /// </summary>
        [DataMember]
        public bool SafeQAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the AuthProvider 
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets whether to pull all documents immediately after sign-in.
        /// </summary>
        [DataMember]
        public bool ReleaseOnSignIn { get; set; }

        /// <summary>
        /// Gets or sets whether to print all.
        /// </summary>
        [DataMember]
        public bool PrintAll { get; set; }
    }
    
    /// <summary>
    /// Defines FileType for scanning.
    /// </summary>
    public enum ScanToSafeQFileType
    {
        [Description("JPEG")]
        JPEG,
        [Description("PDF")]
        PDF,
        [Description("TIFF")]
        TIFF,
        [Description("Multipage TIFF")]
        MultipageTIFF
    }
}
