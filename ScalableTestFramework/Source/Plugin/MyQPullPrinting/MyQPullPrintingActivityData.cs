using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;

namespace HP.ScalableTest.Plugin.MyQPullPrinting
{
    [DataContract]
    public class MyQPullPrintingActivityData
    {
        public MyQPullPrintingActivityData()
        {
            DocumentProcessAction = MyQPullPrintAction.Print;
            AuthProvider = AuthenticationProvider.MyQ;
            ShuffleDocuments = false;
            DelayAfterPrint = 10;
            UsePrintServerNotification = false;

            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
        }

        /// <summary>
        /// Gets or sets the document process action (see <see cref="GeniusBytesPullPrintAction" />.)
        /// </summary>
        /// <value>The document process action.</value>
        [DataMember]
        public MyQPullPrintAction DocumentProcessAction { get; set; }

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
        public bool SelectAll { get; set; }

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

    public enum MyQPullPrintAction
    {
        [Description("Print All")]
        PrintAll,
        [Description("Print")]
        Print,
        [Description("Delete")]
        Delete
    }

}
