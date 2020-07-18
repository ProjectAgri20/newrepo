using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using System;
using HP.ScalableTest.DeviceAutomation;
using System.ComponentModel;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.SolutionApps.HpRoam;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.HpRoam
{
    [DataContract]
    public class HpRoamActivityData
    {
        /// <summary>Gets or sets the International mobile equipment identifier (IMEI).</summary>
        /// <value>The mobile equipment identifier.</value>
        [DataMember]
        public string MobileEquipmentId { get; set; }
        /// <summary>
        /// Gets or sets the document process action.
        /// </summary>
        [DataMember]
        public HpRoamPullPrintAction DocumentProcessAction { get; set; }

        [DataMember]
        public RoamAndroidAction AndroidDocumentAction { get; set; }

        /// <summary>Gets or sets the HP Roam document send action.</summary>
        /// <value>The roam document send action.</value>
        [DataMember]
        public DocumentSendAction RoamDocumentSendAction { get; set; }

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
        /// Gets or sets a value indicating whether [HP Roam authentication].
        /// </summary>
        /// <value><c>true</c> if [HP Roam authentication]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool HPRoamAuthentication { get; set; }

        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        [DataMember]
        public bool SelectAll { get; set; }

        [DataMember]
        public bool ShuffleDocuments { get; set; }

        /// <summary>
        /// Gets/Sets the delay after a print job is sent to the Roam cloud.
        /// </summary>
        [DataMember]
        public int DelayAfterPrint { get; set; }

        /// <summary>
        /// Gets/Sets the delay after a document is selected and before the print button is clicked.
        /// </summary>
        [DataMember]
        public int DelayBeforePullPrint { get; set; }

        /// <summary>
        /// If selected, pushes a job to the device via phone. Otherwise, expects oxpd app and attempts to use it.
        /// </summary>
        [DataMember]
        public bool PhoneDocumentPush { get; set; }

        /// <summary>Gets or sets the document that pushed from the phone.</summary>
        /// <value>The phone document.</value>
        [DataMember]
        public string PhoneDocument { get; set; }

        /// <summary>Gets or sets a value indicating whether [use print server notification] for when to start release action</summary>
        /// <value>
        ///   <c>true</c> if [use print server notification]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UsePrintServerNotification { get; set; }

        public HpRoamActivityData()
        {
            DocumentProcessAction = HpRoamPullPrintAction.PrintAll;
            RoamDocumentSendAction = DocumentSendAction.Windows;
            AndroidDocumentAction = RoamAndroidAction.PrintAll;
            PhoneDocument = string.Empty;
            HPRoamAuthentication = false;
            AuthProvider = AuthenticationProvider.Card;
            SelectAll = false;
            //NumberOfCopies = 1; //Default to 1.  Will throw if 0.
            ShuffleDocuments = false;
            DelayAfterPrint = 10;
            DelayBeforePullPrint = 5;

            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            DeviceMemoryProfilerConfig = new DeviceMemoryProfilerConfig();
            PhoneDocumentPush = false;
        }
    }

}
