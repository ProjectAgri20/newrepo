using System;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using System.ComponentModel;

namespace HP.ScalableTest.Plugin.iSecStarPullPrinting
{
    [DataContract]
    public class iSecStarActivityData
    {
        /// <summary>
        /// Gets or sets the document process action.
        /// </summary>
        /// <value>The document process action.</value>
        [DataMember]
        public iSecStarPullPrintAction DocumentProcessAction { get; set; }

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
        /// Gets or sets a value indicating whether [ISecStar authentication].
        /// </summary>
        /// <value><c>true</c> if [ISecStar authentication]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool ISecStarAuthentication { get; set; }

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
        /// Gets or sets whether to pull all documents immediately after sign-in.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [release on sign in]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ReleaseOnSignIn { get; set; }

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
        /// Initializes a new instance of the <see cref="iSecStarActivityData"/> class.
        /// </summary>
        public iSecStarActivityData()
        {
            DocumentProcessAction = iSecStarPullPrintAction.Reprint;

            ISecStarAuthentication = false;
            AuthProvider = AuthenticationProvider.ISecStar;
            SelectAll = false;
            ReleaseOnSignIn = false;

            ShuffleDocuments = false;
            DelayAfterPrint = 10;
            UsePrintServerNotification = false;

            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
        }

    }

    /// <summary>
    /// ISecStar print button workflows
    /// </summary>
    public enum iSecStarPullPrintAction
    {
        [Description("Reprint")]
        Reprint,
        [Description("Print")]
        Print,
        [Description("Delete")]
        Delete
    }
}
