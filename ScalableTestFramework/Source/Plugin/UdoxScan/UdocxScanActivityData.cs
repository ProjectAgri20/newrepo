using HP.ScalableTest.Framework.Synchronization;
using System;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.SolutionApps.UdocxScan;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.Plugin.UdocxScan
{
    [DataContract]
    internal class UdocxScanActivityData
    {
        /// <summary>
        /// Gets or sets the locktimeouts
        /// </summary>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets the JobType
        /// </summary>
        [DataMember]
        public UdocxScanJobType JobType { get; set; }

        /// <summary>
        /// Gets or sets the EmailAddress
        /// </summary>
        [DataMember]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets whether to initiate authentication via the PaperCut Print Release button.
        /// <value><c>true</c> if [use Print Release button]; otherwise, <c>false</c>.</value>
        /// </summary>
        [DataMember]
        public bool UdocxScanAuthentication { get; set; }

        public UdocxScanActivityData()
        {
            AuthProvider = AuthenticationProvider.UdocxScan;
            UdocxScanAuthentication = true;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            JobType = UdocxScanJobType.ScantoMail;
        }
    }
}
