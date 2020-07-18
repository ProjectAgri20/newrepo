using System;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.SafeComUC;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;

namespace HP.ScalableTest.Plugin.SafeComUCPullPrinting
{
    [DataContract]
    public class SafeComUCPullPrintingActivityData
    {
        private int _numOfCopies = 1;

        [DataMember]
        public SafeComUCPullPrintAction DocumentProcessAction { get; set; }

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

        public SafeComUCPullPrintingActivityData()
        {
            DocumentProcessAction = SafeComUCPullPrintAction.Print;
            SafeComAuthentication = false;
            AuthProvider = AuthenticationProvider.SafeComUC;
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

}
