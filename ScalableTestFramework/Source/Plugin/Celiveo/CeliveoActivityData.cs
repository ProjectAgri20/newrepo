using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Celiveo
{
    [DataContract]
    public class CeliveoActivityData
    {
        public CeliveoActivityData()
        {
            DocumentProcessAction = CeliveoPullPrintAction.Print;
            NumberOfCopies = 1;

            AuthProvider = AuthenticationProvider.Celiveo;
            SelectAll = false;
            ReleaseOnSignIn = false;

            ShuffleDocuments = false;
            DelayAfterPrint = 10;
            UsePrintServerNotification = false;

            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
        }

        /// <summary>
        /// Gets or sets the document process action (see <see cref="CelioveoPrintAction" />.)
        /// </summary>
        /// <value>The document process action.</value>
        [DataMember]
        public CeliveoPullPrintAction DocumentProcessAction { get; set; }

        /// <summary>
        /// Gets or sets the Number of copies
        /// </summary>
        /// <value>The document process action.</value>
        [DataMember]
        public int NumberOfCopies { get; set; }

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
        public bool CeliveoAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the AuthProvider 
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
    /// Defines the actions available for Celiveo automated operations.
    /// </summary>
    public enum CeliveoPullPrintAction
    {        
        [Description("Print")]
        Print,
        [Description("Print B/W")]
        PrintBW,
        [Description("Delete")]
        Delete
    }
}
