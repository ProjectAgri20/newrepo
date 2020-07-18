using HP.ScalableTest.Plugin.LocksmithConfiguration.ActivityData;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.LocksmithConfiguration
{
    /// <summary>
    /// Consists Locksmith Configuration data for the execution
    /// </summary>
    [DataContract]
    public class LockSmithConfigurationActivityData
    {
        #region General
        /// <summary>
        /// Type of the browser
        /// </summary>
        [DataMember]
        public BrowserType Browser { get; set; }

        /// <summary>
        /// LockSmith admin user name
        /// </summary>
        [DataMember]
        public string LockSmithUser { get; set; }

        /// <summary>
        /// LockSmith admin user password
        /// </summary>
        [DataMember]
        public string LockSmithPassword { get; set; }

        /// <summary>
        /// Global Credentials Password
        /// </summary>
        [DataMember]
        public string EWSAdminPassword { get; set; }


        /// <summary>
        /// Group Name to add the devices
        /// </summary>
        [DataMember]
        public string GroupName { get; set; }

        /// <summary>
        /// Generate reports functionality
        /// </summary>
        [DataMember]
        public bool ReportExtraction { get; set; }

        #endregion General

        #region Policy
        
        /// <summary>
        /// File address path of the Policy
        /// </summary>
        [DataMember]
        public string PolicyFilePath { get; set; }

        /// <summary>
        /// Password of the policy file 
        /// </summary>
        [DataMember]
        public string PolicyPassword { get; set; }

        /// <summary>
        /// Import and Apply policy functionality
        /// </summary>
        [DataMember]
        public bool PolicyConfiguration { get; set; }

        /// <summary>
        /// Existing policy name Checkbox
        /// </summary>
        [DataMember]
        public bool ExisintingPolicyCheckbox { get; set; }

        /// <summary>
        /// Existing policy name
        /// </summary>
        [DataMember]
        public string ExistingPolicyName { get; set; }

        /// <summary>
        /// Selects Assessonly option while applying the policy
        /// </summary>
        [DataMember]
        public bool AssessOnly { get; set; }

        /// <summary>
        /// Selects AssessandRemediate option while applying the policy
        /// </summary>
        [DataMember]
        public bool AssessandRemediate { get; set; }

        #endregion Policy

        #region Device Discovery

        /// <summary>
        /// IPAddress of the Manual Discovery 
        /// </summary>
        [DataMember]
        public string ManualIPAddress { get; set; }

        /// <summary>
        /// IPAddress file path of the Manual Discovery 
        /// </summary>
        [DataMember]
        public string DeviceListPath { get; set; }

        /// <summary>
        /// Number of Hops value of the Automatic Discovery
        /// </summary>
        [DataMember]
        public decimal NumberOfHops { get; set; }

        /// <summary>
        /// Starting IPAddress Range of the Automatic Discovery
        /// </summary>
        [DataMember]
        public string StartIPAddress { get; set; }

        /// <summary>
        /// End IPAddress Range of the Automatic Discovery
        /// </summary>
        [DataMember]
        public string EndIPAddress { get; set; }

        /// <summary>
        /// Printer discovery functionality
        /// </summary>
        [DataMember]
        public bool ValidatePolicyPath { get; set; }

        /// <summary>
        /// Type of Printer Discovery operation
        /// </summary>
        [DataMember]
        public PrinterDiscovery Adddevice { get; set; }

        /// <summary>
        /// Printer discovery functionality
        /// </summary>
        [DataMember]
        public bool DeviceDiscovery { get; set; }

        #endregion Device Discovery

        public LockSmithConfigurationActivityData()
        {         
            Adddevice = new PrinterDiscovery();
            GroupName = string.Empty;
        }
    }
}