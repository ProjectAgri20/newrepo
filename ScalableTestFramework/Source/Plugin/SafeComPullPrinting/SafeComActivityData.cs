using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.PluginSupport.PullPrint;

namespace HP.ScalableTest.Plugin.SafeComPullPrinting
{
    public class SafeComActivityData
    {
        private int _numOfCopies = 1;

        [DataMember]
        public SafeComPullPrintAction DocumentProcessAction { get; set; }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        [DataMember]
        public bool SafeComAuthentication { get; set; }

        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        [DataMember]
        public DeviceMemoryProfilerConfig DeviceMemoryProfilerConfig { get; set; }

        [DataMember]
        public int NumberOfCopies
        {
            get { return _numOfCopies; }
            set { _numOfCopies = Math.Max(value, 1); }
        }

        [DataMember]
        public bool SelectAll { get; set; }

        [DataMember]
        public bool ReleaseOnSignIn { get; set; }

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
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeComActivityData"/> class.
        /// </summary>
        public SafeComActivityData()
        {
            DocumentProcessAction = SafeComPullPrintAction.Print;
            SafeComAuthentication = false;
            AuthProvider = AuthenticationProvider.SafeCom;
            SelectAll = false;
            ReleaseOnSignIn = false;
            NumberOfCopies = 1; //Default to 1.  Will throw if 0.
            ShuffleDocuments = false;
            DelayAfterPrint = 10;
            UsePrintServerNotification = false;

            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
        }
    }

    public enum SafeComPullPrintAction
    {
        [Description("Print All")]
        PrintAll,
        [Description("Print")]
        Print,
        [Description("Delete All")]
        DeleteAll,
        [Description("Delete")]
        Delete,
        [Description("Print Retain All")]
        PrintRetainAll,
        [Description("Print Retain")]
        PrintRetain,
        [Description("Refresh")]
        Refresh
    }
}
