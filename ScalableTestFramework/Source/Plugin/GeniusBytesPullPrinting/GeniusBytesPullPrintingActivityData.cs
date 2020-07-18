using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.GeniusBytesPullPrinting
{
    [DataContract]
    public class GeniusBytesPullPrintingActivityData
    {
        public GeniusBytesPullPrintingActivityData()
        {
            DocumentProcessAction = GeniusBytesPullPrintAction.PrintAll;
            AuthProvider = AuthenticationProvider.Auto;
            ReleaseOnSignIn = false;
            PrintAll = true;
            ShuffleDocuments = false;
            DelayAfterPrint = 10;
            UsePrintServerNotification = false;
            UseColorModeNotification = false;
            UseDeletionNotification = false;

            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
        }

        /// <summary>
        /// Gets or sets the document process action (see <see cref="GeniusBytesPullPrintAction" />.)
        /// </summary>
        /// <value>The document process action.</value>
        [DataMember]
        public GeniusBytesPullPrintAction DocumentProcessAction { get; set; }

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
        /// Gets or sets whether to select all documents.
        /// </summary>
        [DataMember]
        public bool PrintAll { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating wheter to use pull print color mode notification.
        /// </summary>
        [DataMember]
        public bool UseColorModeNotification { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use pull print deletion notification.
        /// </summary>
        [DataMember]
        public bool UseDeletionNotification { get; set; }
    }

    public enum GeniusBytesPullPrintAction
    {
        [Description("Print All")]
        PrintAll,
        [Description("Print All and Delete")]
        PrintAllandDelete,
        [Description("Print")]
        Print,
        [Description("Print and Delete")]
        PrintandDelete,
        [Description("Delete")]
        Delete,
        [Description("Delete All")]
        DeleteAll
    }
}
