using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.PaperCutAgentless
{
    [DataContract]
    public class PaperCutAgentlessActivityData
    {
        public PaperCutAgentlessActivityData()
        {
            DocumentProcessAction = PaperCutAgentlessPullPrintAction.Print;

            AuthProvider = AuthenticationProvider.PaperCutAgentless;
            SelectAll = false;
            ForceGrayscale = false;
            Force2sided = false;
            ReleaseOnSignIn = false;

            UseSingleJobOptions = false;
            SingleJobCopies = 1;
            SingleJobDuplex = false;
            SingleJobGrayScale = false;

            DelayAfterPrint = 10;
            UsePrintServerNotification = false;

            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
        }

        /// <summary>
        /// Gets or sets the document process action (see <see cref="PaperCutPullPrintAction" />.)
        /// </summary>
        /// <value>The document process action.</value>
        [DataMember]
        public PaperCutAgentlessPullPrintAction DocumentProcessAction { get; set; }

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
        /// Gets or sets AuthoProvider for selecting authentication mode.
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets whether to select all documents.
        /// </summary>
        [DataMember]
        public bool SelectAll { get; set; }

        /// <summary>
        /// Gets or sets Force Gray Scale mode.
        /// </summary>
        [DataMember]
        public bool ForceGrayscale { get; set; }

        /// <summary>
        /// Gets or sets Force 2-sided mode
        /// </summary>
        [DataMember]
        public bool Force2sided { get; set; }

        /// <summary>
        /// Gets or sets using the single job options.- If test scenario use this option, the test will run with first job on the list.
        /// </summary>
        [DataMember]
        public bool UseSingleJobOptions { get; set; }

        /// <summary>
        /// Gets or sets copies on the single job option.
        /// </summary>
        [DataMember]
        public int SingleJobCopies { get; set; }

        /// <summary>
        /// Gets or sets duplex mode on the single job option.
        /// true: 1-sided, false: 2-sided
        /// </summary>
        [DataMember]
        public bool SingleJobDuplex { get; set; }

        /// <summary>
        /// Gets or sets color mode on the single job option.
        /// true: Color, false: Grayscale
        /// </summary>
        [DataMember]
        public bool SingleJobGrayScale { get; set; }

        /// <summary>
        /// Gets or sets whether to pull all documents immediately after sign-in.
        /// </summary>
        [DataMember]
        public bool ReleaseOnSignIn { get; set; }

        /// <summary>
        /// Gets or set whether to shuffle the documents when printing.
        /// </summary>
        [DataMember]
        public bool ShuffleDocuments { get; set; }

        /// <summary>
        /// Gets or sets the delay time in seconds between the print operation and the pull operation.
        /// </summary>
        [DataMember]
        public int DelayAfterPrint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use print server notification after printing.
        /// </summary>
        /// <value><c>true</c> if [use print server notification]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UsePrintServerNotification { get; set; }
    }
    
    /// <summary>
    /// Defines the actions available for PaperCut automated operations.
    /// </summary>
    public enum PaperCutAgentlessPullPrintAction
    {
        [Description("Print")]
        Print,
        [Description("Delete")]
        Delete
    }
}
