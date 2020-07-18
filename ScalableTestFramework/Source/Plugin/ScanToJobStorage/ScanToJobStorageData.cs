using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.Scan;
using System;
using System.Runtime.Serialization;


namespace HP.ScalableTest.Plugin.ScanToJobStorage
{
    /// <summary>
    /// Contains data needed to execute a scan to folder through the ScanToFolder plugin.
    /// </summary>
    [DataContract]
    public class ScanToJobStorageData
    {
        /// <summary>
        /// Gets or sets the JobName.
        /// </summary>
        /// <value>The JobName Name.</value>
        [DataMember]
        public string JobName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [IsPinRequired] is to be used.
        /// </summary>
        ///   <c>true</c> if [Pin Required]; otherwise, <c>false</c>.
        [DataMember]
        public bool IsPinRequired { get; set; }

        /// <summary>
        /// Gets or sets the PIN of Job.
        /// </summary>
        /// <value>The PIN Number .</value>
        /// 
        [DataMember]
        public string Pin { get; set; }

        /// <summary>
        /// Gets or sets the automation pause for simulators.
        /// </summary>
        /// <value>The automation pause.</value>
        [DataMember]
        public TimeSpan AutomationPause { get; set; }

        /// <summary>
        /// Gets or sets a value indicating ScanOptions
        /// </summary>
        /// <value>The scan Options</value>
        [DataMember]
        public ScanOptions ScanOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [ScanToJobStorage app auth (lazy)]
        /// </summary>
        [DataMember]
        public bool ApplicationAuthentication { get; set; }

        /// <summary>
        /// Gets or sets what authentication provider to use.
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToJobStorageData"/> class.
        /// </summary>
        public ScanToJobStorageData()
        {
            JobName = string.Empty;
            IsPinRequired = false;
            Pin = string.Empty;
            AutomationPause = TimeSpan.FromSeconds(1);
            ScanOptions = new ScanOptions();
            ApplicationAuthentication = true;
            AuthProvider = AuthenticationProvider.Auto;
        }

        [OnDeserialized]
        private void SetValuesOnDeserialized(StreamingContext context)
        {
            if (ScanOptions == null)
            {
                ScanOptions = new ScanOptions();
            }
        }

    }
}
