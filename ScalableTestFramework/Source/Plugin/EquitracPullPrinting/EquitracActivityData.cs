using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.PluginSupport.PullPrint;

namespace HP.ScalableTest.Plugin.EquitracPullPrinting
{
    [DataContract]
    public class EquitracActivityData
    {

        [DataMember]
        public EquitracPullPrintAction DocumentProcessAction { get; set; }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        [DataMember]
        public virtual DeviceMemoryProfilerConfig DeviceMemoryProfilerConfig { get; set; }

        [DataMember]
        public bool EquitracAuthentication { get; set; }

        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        [DataMember]
        public string EquitracServer { get; set; }

        [DataMember]
        public int NumberOfCopies { get; set; }

        [DataMember]
        public bool SelectAll { get; set; }

        [DataMember]
        public bool ShuffleDocuments { get; set; }

        [DataMember]
        public int DelayAfterPrint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use print server notification.
        /// </summary>
        /// <value><c>true</c> if [use print server notification]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UsePrintServerNotification { get; set; }
        public EquitracActivityData()
        {
            EquitracAuthentication = false;
            AuthProvider = AuthenticationProvider.Equitrac;
            EquitracServer = string.Empty;
            NumberOfCopies = 1;

            ShuffleDocuments = false;
            DelayAfterPrint = 10;
            UsePrintServerNotification = false;

            SelectAll = false;

            DocumentProcessAction = EquitracPullPrintAction.Print;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
        }

    }
    public enum EquitracPullPrintAction
    {
        [Description("Print")]
        Print,
        [Description("Print & Save")]
        PrintSave,
        [Description("Delete")]
        Delete,
        [Description("Refresh")]
        Refresh,
        [Description("Select All")]
        SelectAll,
        [Description("Server")]
        Server
    }
}
