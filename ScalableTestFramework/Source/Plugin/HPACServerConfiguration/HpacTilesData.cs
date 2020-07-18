using System.Collections.Generic;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    /// <summary>
    /// Contains data needed to execute the Settings Tab through the HpacServerConfiguration plugin.
    /// </summary>
    [DataContract]
    public class SettingsTabData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsTabData"/> class.
        /// </summary>
        public SettingsTabData()
        {
            QueueName = string.Empty;
            SettingsOperation = SettingsOperation.AddPrintQueue;
            PurgedJobs = false;
            Tracking = new List<SNMPTracking>();
            EnableQuota = false;
            ProtocolOptions = ProtocolOptions.SOCK;
            ServerURI = string.Empty;
            Encryption = false;
            ServerIPAddress = string.Empty;


        }
        /// <summary>
        /// Gets or sets the QueueName.
        /// </summary>
        /// <value>The Queue Name.</value>
        [DataMember]
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets the ServerURI.
        /// </summary>
        /// <value>The Server URI.</value>
        [DataMember]
        public string ServerURI { get; set; }

        /// Gets or sets the ServerIPAddress.
        /// </summary>
        /// <value>The Server ServerIPAddress.</value>
        [DataMember]
        public string ServerIPAddress { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to enable Quota, if available.
        /// </summary>
        /// <value><c>true</c> if Quota should be enabled; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool EnableQuota { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable PurgedJobs, if available.
        /// </summary>
        /// <value><c>true</c> if PurgedJobs should be enabled; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool PurgedJobs { get; set; }

        /// <summary>
        /// List of selected SNMP trackings to execute task on
        /// </summary>
        [DataMember]
        public List<SNMPTracking> Tracking { get; set; }

        /// <summary>
        ///  Gets or sets a value for Settings Operation
        /// <value>Enum value</value>
        /// </summary>
        [DataMember]
        public SettingsOperation SettingsOperation { get; set; }
        /// <summary>
        ///  Gets or sets a value for protocol option
        /// <value>Enum value</value>
        /// </summary>
        [DataMember]
        public ProtocolOptions ProtocolOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable Encryption, if available.
        /// </summary>
        /// <value><c>true</c> if Encryption should be enabled; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool Encryption { get; set; }



    }

    /// <summary>
    /// Contains data needed to execute the Device Tab through the HpacServerConfiguration plugin.
    /// </summary>
    [DataContract]
    public class DeviceTabData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceTabData"/> class.
        /// </summary>
        public DeviceTabData()
        {
            DeviceOperation = DeviceOperation.Add;
            Configuration = new List<HpacConfiguration>();
            Authenticators = new List<HpacAuthenticators> {HpacAuthenticators.DraServer};
        }

        /// <summary>
        /// List of selected configuration to execute task on
        /// </summary>
        [DataMember]
        public List<HpacConfiguration> Configuration { get; set; }

        [DataMember]
        public List<HpacAuthenticators> Authenticators { get; set; }

        /// <summary>
        ///  Gets or sets a value for DevcieOperation Operation
        /// <value>Enum value</value>
        /// </summary>
        [DataMember]
        public DeviceOperation DeviceOperation { get; set; } //there was a typo in device i have fixed

        /// <summary>
        /// Gets or sets the <see cref="AssetSelectionData" /> for this plugin configuration.
        /// </summary>
        /// <value>The asset selection data.</value>
        [DataMember]
        public IDeviceInfo Asset { get; set; }
    }

    /// <summary>
    /// Contains data needed to execute the PrintServer Tab through the HpacServerConfiguration plugin.
    /// </summary>
    [DataContract]
    public class PrintServerTabData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintServerTabData"/> class.
        /// </summary>
        public PrintServerTabData()
        {
            QueueName = string.Empty;
            Configuration = new List<HpacConfiguration>();
        }

        /// <summary>
        /// Gets or sets the QueueName.
        /// </summary>
        /// <value>The Queue Name.</value>
        [DataMember]
        public string QueueName { get; set; }

        /// <summary>
        /// List of selected Configuration to execute task on
        /// </summary>
        [DataMember]
        public List<HpacConfiguration> Configuration { get; set; }
    }

    /// <summary>
    /// Contains data needed to execute the IRM Tab through the HpacServerConfiguration plugin.
    /// </summary>
    [DataContract]
    public class IRMTabData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IRMTabData"/> class.
        /// </summary>
        public IRMTabData()
        {
            LDAPServer = string.Empty;
            LDAPServerUserName = string.Empty;
            LDAPServerPassword = string.Empty;
            Username = string.Empty;
            ADUserCardNumber = string.Empty;
            ADUserCodeNumber = string.Empty;
            IRMUserCardNumber = string.Empty;
            IRMUserCodeNumber = string.Empty;
            AuthenticationMode = HpacAuthenticationMode.Card;
            DataStorage = HpacDataStorage.LDAP;
            IrmOperation = IrmOperation.GeneralSettings;
        }

        /// <summary>
        ///  Gets or sets a value for IrmOperation Operation
        /// <value>Enum value</value>
        /// </summary>
        [DataMember]
        public IrmOperation IrmOperation { get; set; }

        /// <summary>
        ///  Gets or sets a value for AuthenticationMode Operation
        /// <value>Enum value</value>
        /// </summary>
        [DataMember]
        public HpacAuthenticationMode AuthenticationMode { get; set; }

        /// <summary>
        ///  Gets or sets a value for DataStorage Operation
        /// <value>Enum value</value>
        /// </summary>
        [DataMember]
        public HpacDataStorage DataStorage { get; set; }

        /// <summary>
        /// Gets or sets the LDAPServer.
        /// </summary>
        /// <value>The LDAPServer.</value>
        [DataMember]
        public string LDAPServer { get; set; }

        /// <summary>
        /// Gets or sets the LDAPServerUserName.
        /// </summary>
        /// <value>The LDAPServerUserName.</value>
        [DataMember]
        public string LDAPServerUserName { get; set; }

        /// <summary>
        /// Gets or sets the LDAPServerPassword.
        /// </summary>
        /// <value>The LDAPServerPassword.</value>
        [DataMember]
        public string LDAPServerPassword { get; set; }

        /// <summary>
        /// Gets or sets the Username.
        /// </summary>
        /// <value>The Username.</value>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the ADUserCardNumber.
        /// </summary>
        /// <value>The ADUserCardNumber.</value>
        [DataMember]
        public string ADUserCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the ADUserCodeNumber.
        /// </summary>
        /// <value>The ADUserCodeNumber.</value>
        [DataMember]
        public string ADUserCodeNumber { get; set; }

        /// <summary>
        /// Gets or sets the IRMUserCardNumber.
        /// </summary>
        /// <value>The IRMUserCardNumber.</value>
        [DataMember]
        public string IRMUserCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the IRMUserCodeNumber.
        /// </summary>
        /// <value>The IRMUserCodeNumber.</value>
        [DataMember]
        public string IRMUserCodeNumber { get; set; }
    }

    /// <summary>
    /// Contains data needed to execute the JobAccounting Tab through the HpacServerConfiguration plugin.
    /// </summary>
    [DataContract]
    public class JobAccountingTabData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobAccountingTabData"/> class.
        /// </summary>
        public JobAccountingTabData()
        {
            Username = string.Empty;
            Password = string.Empty;
            ReportName = string.Empty;
            ReportPassword = string.Empty;
            ReportUsername = string.Empty;
            ReportEmailTo = string.Empty;
            OutputFormat = OutputFormat.Pdf;
        }


        /// <summary>
        /// List of selected JobAccountingOperation to execute task on
        /// </summary>
        [DataMember]
        public JobAccountingOperation JobAccountingOperation { get; set; }

        /// <summary>
        /// Gets or sets the Username.
        /// </summary>
        /// <value>The Username.</value>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        /// <value>The Password.</value>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the ReportUsername.
        /// </summary>
        /// <value>The ReportUsername.</value>
        [DataMember]
        public string ReportUsername { get; set; }

        /// <summary>
        /// Gets or sets the ReportPassword.
        /// </summary>
        /// <value>The ReportPassword.</value>
        [DataMember]
        public string ReportPassword { get; set; }

        /// <summary>
        /// Gets or sets the ReportName.
        /// </summary>
        /// <value>The ReportName.</value>
        [DataMember]
        public string ReportName { get; set; }

        /// <summary>
        /// Gets or sets the ReportEmailTo.
        /// </summary>
        /// <value>The ReportEmailTo.</value>
        [DataMember]
        public string ReportEmailTo { get; set; }

        /// <summary>
        /// Gets or sets the OutputFormat.
        /// </summary>
        /// <value>The OutputFormat.</value>
        [DataMember]
        public OutputFormat OutputFormat { get; set; }
    }
}
