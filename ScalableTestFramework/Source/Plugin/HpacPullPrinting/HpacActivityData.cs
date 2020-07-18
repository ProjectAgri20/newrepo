using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.PluginSupport.PullPrint;

namespace HP.ScalableTest.Plugin.HpacPullPrinting
{
    [DataContract]
    public class HpacActivityData
    {
        /// <summary>
        /// Gets or sets the document process action.
        /// </summary>
        /// <value>The document process action.</value>
        [DataMember]
        public HpacPullPrintAction DocumentProcessAction { get; set; }

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
        /// Gets or sets a value indicating whether [hpac authentication].
        /// </summary>
        /// <value><c>true</c> if [hpac authentication]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool HpacAuthentication { get; set; }

        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

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

        public HpacActivityData()
        {
            DocumentProcessAction = HpacPullPrintAction.PrintAll;

            HpacAuthentication = false;
            AuthProvider = AuthenticationProvider.HpacDra;
            SelectAll = false;
            ReleaseOnSignIn = false;

            ShuffleDocuments = false;
            DelayAfterPrint = 10;
            UsePrintServerNotification = false;

            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
        }

    }
    public enum HpacPullPrintAction
    {
        [Description("Print all")]
        PrintAll,
        [Description("Print-Delete")]
        PrintDelete,
        [Description("Print-Keep")]
        PrintKeep,
        [Description("Delete")]
        Delete,
        [Description("Refresh")]
        Refresh
    }
}
