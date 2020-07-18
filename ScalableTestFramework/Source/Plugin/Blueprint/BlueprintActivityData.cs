using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.PluginSupport.PullPrint;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Blueprint
{
    /// <summary>
    /// Meta data for the Blue Print solution
    /// </summary>
    [DataContract]
    public class BlueprintActivityData
    {
        /// <summary>
        /// Gets or sets the document process action.
        /// </summary>
        /// 
        [DataMember]
        public BlueprintPullPrintAction DocumentProcessAction { get; set; }

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
        /// Gets or sets a value indicating whether [pharos authentication].
        /// </summary>
        /// <value><c>true</c> if [pharos authentication]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool BlueprintAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the authentication provider.
        /// </summary>
        /// <value>
        /// The authentication provider.
        /// </value>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [select all].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [select all]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool SelectAll { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [shuffle documents].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [shuffle documents]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ShuffleDocuments { get; set; }

        /// <summary>
        /// Gets or sets the delay after print.
        /// </summary>
        /// <value>
        /// The delay after print.
        /// </value>
        [DataMember]
        public int DelayAfterPrint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use print server notification.
        /// </summary>
        /// <value><c>true</c> if [use print server notification]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UsePrintServerNotification { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlueprintActivityData"/> class.
        /// </summary>
        public BlueprintActivityData()
        {
            DocumentProcessAction = BlueprintPullPrintAction.PrintAll;
            BlueprintAuthentication = false;
            AuthProvider = AuthenticationProvider.Blueprint;
            SelectAll = false;
            //NumberOfCopies = 1; //Default to 1.  Will throw if 0.
            ShuffleDocuments = false;
            DelayAfterPrint = 10;

            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
        }
    }


    /// <summary>
    /// Blue print button workflows
    /// </summary>
    public enum BlueprintPullPrintAction
    {
        [Description("Print all")]
        PrintAll,
        [Description("Print")]
        Print,
        [Description("Delete")]
        Delete,
    }
}
