using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.PluginSupport.PullPrint;

namespace HP.ScalableTest.Plugin.PaperCut
{
    /// <summary>
    /// The data needed to execute operations on PaperCut solution.
    /// </summary>
    [DataContract]
    public class PaperCutActivityData
    {
        /// <summary>
        /// Creates a new instance of <see cref="PaperCutActivityData" /> class.
        /// </summary>
        public PaperCutActivityData()
        {
            DocumentProcessAction = PaperCutPullPrintAction.Print;

            AuthProvider = AuthenticationProvider.PaperCut;
            SelectAll = false;
            ReleaseOnSignIn = false;

            ShuffleDocuments = false;
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
        public PaperCutPullPrintAction DocumentProcessAction { get; set; }

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
        /// Gets or sets whether to initiate authentication via the PaperCut Print Release button.
        /// <value><c>true</c> if [use Print Release button]; otherwise, <c>false</c>.</value>
        /// </summary>
        [DataMember]
        public bool PaperCutAuthentication { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets whether to select all documents.
        /// </summary>
        [DataMember]
        public bool SelectAll { get; set; }

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
    public enum PaperCutPullPrintAction
    {
        [Description("Print")]
        Print,
        [Description("Delete")]
        Delete
    }
}
