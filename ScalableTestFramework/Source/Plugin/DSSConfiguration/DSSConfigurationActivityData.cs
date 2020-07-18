using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DSSConfiguration
{
    /// <summary>
    /// Contains data needed to execute a DSSConfiguration activity.
    /// </summary>
    [DataContract]
    public class DssConfigurationActivityData
    {
        /// <summary>
        /// Initializes a new instance of the DSSConfigurationActivityData class.
        /// </summary>
        public DssConfigurationActivityData()
        {
            ParameterValueDictionary = new Dictionary<string, object>();
        }

        [DataMember]
        public string TaskName { get; set; }

        [DataMember]
        public Dictionary<string, object> ParameterValueDictionary { get; set; }
    }

    #region Common

    /// <summary>
    /// Different LanFax Products
    /// </summary>
    public enum LanFaxProduct
    {
        [Description("ACCPAC")]
        Accpac,

        [Description("Anny Way Office EDITION")]
        AnnyWayOfficeEdition,

        [Description("Biscom FAXCOM")]
        BiscomFaxCom,

        [Description("Captaris RightFAX")]
        CaptarisRightFax,

        [Description("Captaris RightFAX version8.7 or later")]
        CaptarisRightFaXversion87Orlater,

        [Description("Captaris RightFAX-HP MFP Module")]
        CaptarisRightFaxHpmfpModule,

        [Description("Castelle FaxPress")]
        CastelleFaxPress,

        [Description("Castelle FaxPress Premier")]
        CastelleFaxPressPremier,

        [Description("Cycos-mrs Unified Communication")]
        CycosMrsUnifiedCommunication,

        [Description("Esker LanFax")]
        EskerLanFax,

        [Description("Esker Pulse/Fax")]
        EskerPulse,

        [Description("FACSys Fax Messaging Gateway")]
        FacSysFaxMessagingGateway,

        [Description("Fenestrae Faxination")]
        FenestraeFaxination,

        [Description("GFI FAXmaker")]
        GfifaXmaker,

        [Description("Gold-Fax")]
        GoldFax,

        [Description("Imecom Integral Fax")]
        ImecomIntegralFax,

        [Description("INTERCOPE FaxPlus/Open")]
        IntercopeFaxPlus,

        [Description("Interstar LightningFAX")]
        InterstarLightningFax,

        [Description("Generic LAN Fax product with notification support")]
        GenericLanFaxproductwithnotificationsupport,

        [Description("Generic LAN Fax product without notification support")]
        GenericLanFaxproductwithoutnotificationsupport,

        [Description("NET SatisFAXtion")]
        NetSatisFaXtion,

        [Description("Object Fax")]
        ObjectFax,

        [Description("Omtool")]
        Omtool,

        [Description("RedRock FaxNow")]
        RedRockFaxNow,

        [Description("RTEFAX")]
        Rtefax,

        [Description("Tobit DvISE")]
        TobitDvIse,

        [Description("TOPCALL")]
        Topcall,

        [Description("Zetafax")]
        Zetafax
    }

    /// <summary>
    /// Different Smtp Authentication Types
    /// </summary>
    public enum SmtpAuthenticationType
    {
        [Description("Use the user's credentials to connect after Sign In at the control panel")]
        UserCredentials,

        [Description("Always use these credentials")]
        AlwaysUse
    }

    /// <summary>
    /// Different Fax Service Types
    /// </summary>
    public enum FaxServiceType
    {
        [Description("Internal Modem")]
        AnalogFax,

        [Description("LAN Fax Service")]
        LanFax,

        [Description("Internet Fax Service")]
        InternetFax,

        [Description("via the Digital Sending Service")]
        ViaDss
    }

    /// <summary>
    /// Differnet Modes of Setting Status
    /// </summary>
    public enum SettingStatus
    {
        [Description("Enabled")]
        Enable,

        [Description("Disabled")]
        Disable
    }

    /// <summary>
    /// Differnet File Format types
    /// </summary>
    public enum FileFormat
    {
        [Description("MTIF/G3")]
        Mtiffg3,

        [Description("MTIF/G4")]
        Mtiffg4,

        [Description("PCL5")]
        Pcl5,

        [Description("PCL5 Uncompressed")]
        Pcl5Uncompressed
    }

    /// <summary>
    /// Differnet Transmission Speed types
    /// </summary>
    public enum TransmissionSpeed
    {
        [Description("Default")]
        Default,

        [Description("12 Kbps")]
        F12Kbps,

        [Description("14.4 Kbps")]
        F144Kbps,

        [Description("2400 bps")]
        F2400Bps,

        [Description("28.8 Kbps")]
        F288Kbps,

        [Description("38.4 Kbps")]
        F384Kbps,

        [Description("4800 Bps")]
        F4800Bps,

        [Description("57.6 Kbps")]
        F576Kbps,

        [Description("7200 bps")]
        F7200Bps,

        [Description("9600 bps")]
        F9600Bps
    }

    /// <summary>
    /// Contains data related to DSS Crendentials.
    /// </summary>
    public class DssCredential
    {
        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Domain.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string Domain { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssCredential"/> class.
        /// </summary>
        public DssCredential()
        {
            UserName = string.Empty;
            Password = string.Empty;
            Domain = string.Empty;
        }
    }

    /// <summary>
    /// Contains data related to Folder Settings.
    /// </summary>
    public class FolderSettings
    {
        /// <summary>
        /// Gets or sets the FolderPath.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the DssCredential.
        /// </summary>
        public DssCredential Credential { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Use Common Credential.
        /// </summary>
        /// <value><c>true</c> if Use Common Credential should be enabled; otherwise, <c>false</c>.</value>
        public bool UseCommonCredential { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Use Path Specific Credential.
        /// </summary>
        /// <value><c>true</c> if Use Path Specific Credential should be enabled; otherwise, <c>false</c>.</value>
        public bool UsePathSpecificCredential { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Use User Credential.
        /// </summary>
        /// <value><c>true</c> if Use User Credential should be enabled; otherwise, <c>false</c>.</value>
        public bool UseUserCredential { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderSettings"/> class.
        /// </summary>
        public FolderSettings()
        {
            FolderPath = "";
            Credential = new DssCredential();
            UseCommonCredential = false;
            UsePathSpecificCredential = false;
            UseUserCredential = false;
        }
    }

    #endregion Common

    #region Authentication

    /// <summary>
    /// Differnet Authentication types
    /// </summary>
    public enum AuthenticationType
    {
        [Description("None")]
        None,

        [Description("LDAP Server")]
        Ldap,

        [Description("Microsoft Windows")]
        Windows
    }

    /// <summary>
    /// Contains data related to Dss Authentication.
    /// </summary>
    public class DssAuthentication
    {
        /// <summary>
        /// Gets or sets the Ldap Setting.
        /// </summary>
        public DssAuthenticationLdap LdapSetting { get; set; }

        /// <summary>
        /// Gets or sets the Windows Setting.
        /// </summary>
        public DssAuthenticationWindows WindowsSetting { get; set; }
    }

    /// <summary>
    /// Contains data related to Dss Authentication Ldap.
    /// </summary>
    public class DssAuthenticationLdap
    {
        /// <summary>
        /// Gets or sets the DeviceAddress.
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the DeviceUserName.
        /// </summary>
        public string DeviceUserName { get; set; }

        /// <summary>
        /// Gets or sets the DevicePassword.
        /// </summary>
        public string DevicePassword { get; set; }

        /// <summary>
        /// Gets or sets the Domain Server Address.
        /// </summary>
        public string DomainServerAddress { get; set; }

        /// <summary>
        /// Gets or sets the Port Number
        /// </summary>
        public int PortNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Use SSL.
        /// </summary>
        /// <value><c>true</c> if Use Ssl should be enabled; otherwise, <c>false</c>.</value>
        public bool UseSsl { get; set; }

        /// <summary>
        /// Gets or sets the BindPrefix.
        /// </summary>
        [Description("Usually cn=users,dc=domainname")]
        public string BindPrefix { get; set; }

        /// <summary>
        /// Gets or sets the BindandSearch.
        /// </summary>
        public string BindandSearch { get; set; }

        /// <summary>
        /// Gets or sets the MatchAttribute.
        /// </summary>
        public string MatchAttribute { get; set; }

        /// <summary>
        /// Gets or sets the EmailAttribute.
        /// </summary>
        public string EmailAttribute { get; set; }

        /// <summary>
        /// Gets or sets the User NameAttribute.
        /// </summary>
        public string UserNameAttribute { get; set; }

        /// <summary>
        /// Gets or sets the User GroupAttribute.
        /// </summary>
        public string UserGroupAttribute { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssAuthenticationLdap"/> class.
        /// </summary>
        public DssAuthenticationLdap()
        {
            DomainServerAddress = "0.0.0.0";
            PortNumber = 389;
            UseSsl = false;
            BindPrefix = string.Empty;
            BindandSearch = string.Empty;
            MatchAttribute = string.Empty;
            EmailAttribute = string.Empty;
            UserNameAttribute = string.Empty;
            UserGroupAttribute = "objectClass";
            UserName = string.Empty;
            Password = string.Empty;
            DeviceAddress = string.Empty;
            DeviceUserName = string.Empty;
            DevicePassword = string.Empty;
        }
    }

    /// <summary>
    /// Contains data related to Dss Windows Authentication.
    /// </summary>
    public class DssAuthenticationWindows
    {
        /// <summary>
        /// Gets or sets a value indicating whether to Use SSL.
        /// </summary>
        /// <value><c>true</c> if Use Ssl should be enabled; otherwise, <c>false</c>.</value>
        public bool UseSsl { get; set; }

        /// <summary>
        /// Gets or sets the Domain.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the Match Attribute.
        /// </summary>
        public string MatchAttribute { get; set; }

        /// <summary>
        /// Gets or sets the Email Attribute.
        /// </summary>
        public string EmailAttribute { get; set; }

        /// <summary>
        /// Gets or sets the Home Folder Attribute.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string HomeFolderAttribute { get; set; }

        /// <summary>
        /// Gets or sets the User Name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Device Address.
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the Device UserName.
        /// </summary>
        public string DeviceUserName { get; set; }

        /// <summary>
        /// Gets or sets the Device Password.
        /// </summary>
        public string DevicePassword { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssAuthenticationWindows"/> class.
        /// </summary>
        public DssAuthenticationWindows()
        {
            UseSsl = false;
            Domain = string.Empty;
            MatchAttribute = "sAMAccountName";
            EmailAttribute = "mail";
            HomeFolderAttribute = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            DeviceAddress = string.Empty;
            DeviceUserName = string.Empty;
            DevicePassword = string.Empty;
        }
    }

    #endregion Authentication

    #region SendToFolder

    /// <summary>
    /// Contains data related to SendTo Folder Setup.
    /// </summary>
    public class SendToFolderSetup
    {
        /// <summary>
        /// Gets or sets the Device address.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the Device Password.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string DevicePassword { get; set; }

        /// <summary>
        /// Gets or sets the Folder Path.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the Folder Name.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the Folder Description.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string FolderDescription { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Domain.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the Prefix.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the Suffix.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string Suffix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Use Common Credential.
        /// </summary>
        /// <value><c>true</c> if Use Common Credential should be enabled; otherwise, <c>false</c>.</value>
        public bool UseCommonCredentials { get; set; }
    }

    #endregion SendToFolder

    #region E-Mail

    /// <summary>
    /// Contains data related to EmailSetup for Device.
    /// </summary>
    public class EmailSetupforDevice
    {
        /// <summary>
        /// Gets or sets the Device address.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the Device Password.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string DevicePassword { get; set; }

        /// <summary>
        /// Gets or sets the Default Email.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string DefaultEmail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to set ScanToMe.
        /// </summary>
        /// <value><c>true</c> if ScanToMe should be enabled; otherwise, <c>false</c>.</value>
        public bool ScanToMe { get; set; }

        /// <summary>
        /// Gets or sets the DssSigning.
        /// </summary>
        public DssSigning Signing { get; set; }

        /// <summary>
        /// Gets or sets the DssEncryption.
        /// </summary>
        public DssEncryption Encryption { get; set; }

        /// <summary>
        /// Gets or sets the DssNotificationSettings.
        /// </summary>
        public DssNotificationSettings NotificationSettings { get; set; }

        /// <summary>
        /// Gets or sets the DssScanSetting.
        /// </summary>
        public DssScanSetting ScanSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use JobBuild.
        /// </summary>
        /// <value><c>true</c> if JobBuild should be enabled; otherwise, <c>false</c>.</value>
        public bool JobBuild { get; set; }

        /// <summary>
        /// Gets or sets the DssFileSetting.
        /// </summary>
        public DssFileSetting FileSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable BlankPageSuppression.
        /// </summary>
        /// <value><c>true</c> if BlankPageSuppression should be enabled; otherwise, <c>false</c>.</value>
        public bool BlankPageSuppression { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSetupforDevice"/> class.
        /// </summary>
        public EmailSetupforDevice()
        {
            DeviceAddress = string.Empty;
            DevicePassword = string.Empty;
            DefaultEmail = string.Empty;
            ScanToMe = false;
            Signing = DssSigning.DoNotSign;
            Encryption = DssEncryption.DoNotEncrypt;
            NotificationSettings = new DssNotificationSettings();
            ScanSettings = new DssScanSetting();
            FileSettings = new DssFileSetting();
            BlankPageSuppression = false;
        }

    }

    /// <summary>
    /// Contains data related to Notification Settings.
    /// </summary>
    public class DssNotificationSettings
    {
        /// <summary>
        /// Gets or sets the NotifyCondition.
        /// </summary>
        public NotifyCondition NotificationCondition { get; set; }

        /// <summary>
        /// Gets or sets the NotifyMethod.
        /// </summary>
        public NotifyMethod NotificationDeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the NotificationEmail.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string NotificationEmail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include IncludeThumbnail.
        /// </summary>
        /// <value><c>true</c> if ScanToMe should be enabled; otherwise, <c>false</c>.</value>
        public bool IncludeThumbnail { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssNotificationSettings"/> class.
        /// </summary>
        public DssNotificationSettings()
        {
            NotificationCondition = NotifyCondition.DoNotNotify;
            NotificationDeliveryMethod = NotifyMethod.Print;
            NotificationEmail = string.Empty;
            IncludeThumbnail = false;
        }
    }

    /// <summary>
    /// Different DssSigning types
    /// </summary>
    public enum DssSigning
    {
        [Description("Do not sign")]
        DoNotSign,
        [Description("Sign")]
        Sign

    }

    /// <summary>
    /// Different DssEncryption types
    /// </summary>
    public enum DssEncryption
    {
        [Description("Do not encrypt")]
        DoNotEncrypt,
        [Description("Encrypt")]
        Encrypt
    }

    /// <summary>
    /// Contains data related to EmailServer for DSS.
    /// </summary>
    public class DssEmailServer
    {
        /// <summary>
        /// Gets or sets the Server Address.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string ServerAddress { get; set; }

        /// <summary>
        /// Gets or sets the Port Number.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Use SSL.
        /// </summary>
        /// <value><c>true</c> if Use Ssl should be enabled; otherwise, <c>false</c>.</value>
        public bool UseSsl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Use Require Authentication.
        /// </summary>
        /// <value><c>true</c> if Require Authentication should be enabled; otherwise, <c>false</c>.</value>
        public bool RequireAuthentication { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Use Always Use Credentials.
        /// </summary>
        /// <value><c>true</c> if Always Use Credentials should be enabled; otherwise, <c>false</c>.</value>
        public bool AlwaysUseCredentials { get; set; }

        /// <summary>
        /// Gets or sets the DssCredential.
        /// </summary>
        public DssCredential Credential { get; set; }

        /// <summary>
        /// Gets or sets the SplitSize.
        /// </summary>
        public double SplitSize { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssEmailServer"/> class.
        /// </summary>
        public DssEmailServer()
        {
            ServerAddress = "smtp3.hp.com";
            Port = 25;
            UseSsl = false;
            RequireAuthentication = false;
            AlwaysUseCredentials = true;
            SplitSize = 25;
            Credential = new DssCredential();
        }
    }

    #endregion E-Mail

    #region Fax

    /// <summary>
    /// Contains data related to Lan Fax of DSS.
    /// </summary>
    public class DssLanFax
    {
        /// <summary>
        /// Gets or sets the Device Address.
        /// </summary>
        [Description("Please enter Device IP for Device Configuration")]
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the Device Password.
        /// </summary>
        [Description("Please enter Device Password for above Device IP")]
        public string DevicePassword { get; set; }

        /// <summary>
        /// Gets or sets the LanFax Product.
        /// </summary>
        [Description("Please Select the Lan Fax Product Type")]
        public LanFaxProduct LanFaxDevice { get; set; }

        /// <summary>
        /// Gets or sets the File Format.
        /// </summary>
        [Description("Please Select the File Format for the Lan Fax ")]
        public FileFormat FileFormat { get; set; }

        /// <summary>
        /// Gets or sets the Folder Settings.
        /// </summary>
        [Description("Please fill Folder Settings for LanFax")]
        public FolderSettings FolderSetting { get; set; }

        /// <summary>
        /// Gets or sets the FaxDialing Setting.
        /// </summary>
        [Description("Please fill Lan Fax Dialling Settings")]
        public FaxDialingSetting LanFaxDialingSettings { get; set; }

        /// <summary>
        /// Gets or sets the FaxInput Setting.
        /// </summary>
        [Description("Please fill Lan Fax Input Settings")]
        public FaxInputSetting LanFaxInputSettings { get; set; }

        /// <summary>
        /// Gets or sets the FaxOutput Setting.
        /// </summary>
        [Description("Please fill Lan Fax Output Settings")]
        public FaxOutputSetting LanFaxOutputSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssLanFax"/> class.
        /// </summary>
        public DssLanFax()
        {
            DeviceAddress = string.Empty;
            DevicePassword = string.Empty;
            LanFaxDevice = LanFaxProduct.Accpac;
            FileFormat = FileFormat.Mtiffg4;
            FolderSetting = new FolderSettings();
            LanFaxDialingSettings = new FaxDialingSetting();
            LanFaxInputSettings = new FaxInputSetting();
            LanFaxOutputSettings = new FaxOutputSetting();
        }
    }

    /// <summary>
    /// Contains data related to Fax Dialing Setting.
    /// </summary>
    public class FaxDialingSetting
    {
        /// <summary>
        /// Gets or sets the MaxRetryAttempts.
        /// </summary>
        public int MaxRetryAttempts { get; set; }

        /// <summary>
        /// Gets or sets the RetryInterval.
        /// </summary>
        public int RetryInterval { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaxDialingSetting"/> class.
        /// </summary>
        public FaxDialingSetting()
        {
            MaxRetryAttempts = 3;
            RetryInterval = 5;
        }
    }

    /// <summary>
    /// Contains data related to Fax Input Setting.
    /// </summary>
    public class FaxInputSetting
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use Notification.
        /// </summary>
        /// <value><c>true</c> if Notification should be enabled; otherwise, <c>false</c>.</value>
        public bool Notification { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use Error Correction.
        /// </summary>
        /// <value><c>true</c> if Error Correction should be enabled; otherwise, <c>false</c>.</value>
        public bool ErrorCorrection { get; set; }

        /// <summary>
        /// Gets or sets the NotificationTimeout.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public int NotificationTimeout { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaxInputSetting"/> class.
        /// </summary>
        public FaxInputSetting()
        {
            Notification = false;
            ErrorCorrection = true;
            NotificationTimeout = 0;
        }
    }

    /// <summary>
    /// Contains data related to Fax Output Setting.
    /// </summary>
    public class FaxOutputSetting
    {
        /// <summary>
        /// Gets or sets the Transmission Speed.
        /// </summary>
        public TransmissionSpeed Speed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use CoverPage.
        /// </summary>
        /// <value><c>true</c> if CoverPage should be enabled; otherwise, <c>false</c>.</value>
        public bool CoverPage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaxOutputSetting"/> class.
        /// </summary>
        public FaxOutputSetting()
        {
            Speed = TransmissionSpeed.Default;
            CoverPage = false;
        }
    }

    /// <summary>
    /// Contains data related to Internet Fax of DSS.
    /// </summary>
    public class DssInternetFax
    {
        /// <summary>
        /// Gets or sets the Device Address.
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the Device Password.
        /// </summary>
        public string DevicePassword { get; set; }

        /// <summary>
        /// Gets or sets the EmailServer of DSS.
        /// </summary>
        public DssEmailServer EmailServer { get; set; }

        /// <summary>
        /// Gets or sets the FaxProvider Domain.
        /// </summary>
        public string FaxProviderDomain { get; set; }

        /// <summary>
        /// Gets or sets the FaxAccount EmailAddress.
        /// </summary>
        public string FaxAccountEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the T37Prefix.
        /// </summary>
        public string T37Prefix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use EmailAddress.
        /// </summary>
        /// <value><c>true</c> if EmailAddress should be enabled; otherwise, <c>false</c>.</value>
        public bool UseEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use AutoComplete.
        /// </summary>
        /// <value><c>true</c> if AutoComplete should be enabled; otherwise, <c>false</c>.</value>
        public bool AutoComplete { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssInternetFax"/> class.
        /// </summary>
        public DssInternetFax()
        {
            DeviceAddress = string.Empty;
            DevicePassword = string.Empty;
            EmailServer = new DssEmailServer();
            FaxProviderDomain = string.Empty;
            FaxAccountEmailAddress = string.Empty;
            T37Prefix = string.Empty;
            UseEmailAddress = false;
            AutoComplete = false;
        }
    }

    /// <summary>
    /// Contains data related to Analog Fax of DSS.
    /// </summary>
    public class DssAnalogFax
    {
        /// <summary>
        /// Gets or sets the Device Address.
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the Device Password.
        /// </summary>
        public string DevicePassword { get; set; }

        /// <summary>
        /// Gets or sets the FaxModem Settings.
        /// </summary>
        public FaxModemSettings FaxModemSetting { get; set; }

        /// <summary>
        /// Gets or sets the FaxSend Settings.
        /// </summary>
        public FaxSendSettings FaxSendSettings { get; set; }

        /// <summary>
        /// Gets or sets the FaxNotification.
        /// </summary>
        public FaxNotification FaxSendNotificationSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssAnalogFax"/> class.
        /// </summary>
        public DssAnalogFax()
        {
            DeviceAddress = string.Empty;
            DevicePassword = string.Empty;
            FaxModemSetting = new FaxModemSettings();
            FaxSendSettings = new FaxSendSettings();
            FaxSendNotificationSettings = new FaxNotification();
        }
    }

    /// <summary>
    /// Contains data related to FaxModem Settings.
    /// </summary>
    public class FaxModemSettings
    {
        /// <summary>
        /// Gets or sets the Country.
        /// </summary>
        public Country ModemCountry { get; set; }

        /// <summary>
        /// Gets or sets the CompanyName.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNumber.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaxModemSettings"/> class.
        /// </summary>
        public FaxModemSettings()
        {
            ModemCountry = Country.None;
            CompanyName = string.Empty;
            PhoneNumber = string.Empty;
        }
    }

    /// <summary>
    /// Contains data related to FaxSend Settings.
    /// </summary>
    public class FaxSendSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use PCFaxSend.
        /// </summary>
        /// <value><c>true</c> if PCFaxSend should be enabled; otherwise, <c>false</c>.</value>
        public bool EnablePcFaxSend { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use JBIGCompression.
        /// </summary>
        /// <value><c>true</c> if JBIGCompression should be enabled; otherwise, <c>false</c>.</value>
        public bool EnableJbigCompression { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use ErrorCorrectionMode.
        /// </summary>
        /// <value><c>true</c> if ErrorCorrectionMode should be enabled; otherwise, <c>false</c>.</value>
        public bool ErrorCorrectionMode { get; set; }

        /// <summary>
        /// Gets or sets the FaxHeader.
        /// </summary>
        public FaxHeader FaxHeaderType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaxSendSettings"/> class.
        /// </summary>
        public FaxSendSettings()
        {
            EnablePcFaxSend = true;
            EnableJbigCompression = false;
            ErrorCorrectionMode = true;
            FaxHeaderType = FaxHeader.Prepend;
        }
    }

    /// <summary>
    /// Contains data related to Fax Notification.
    /// </summary>
    public class FaxNotification
    {
        /// <summary>
        /// Gets or sets the NotifyCondition.
        /// </summary>
        public NotifyCondition NotificationCondition { get; set; }

        /// <summary>
        /// Gets or sets the NotifyMethod.
        /// </summary>
        public NotifyMethod NotificationDeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the NotificationEmail.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string NotificationEmail { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaxNotification"/> class.
        /// </summary>
        public FaxNotification()
        {
            NotificationCondition = NotifyCondition.DoNotNotify;
            NotificationDeliveryMethod = NotifyMethod.Email;
            NotificationEmail = string.Empty;
        }
    }

    public enum FaxHeader
    {
        Prepend,
        Overlay
    }

    #endregion Fax

    #region SendToFolder

    /// <summary>
    /// Contains data related to DSS Send To Folder.
    /// </summary>
    public class DssSendToFolder
    {
        /// <summary>
        /// Gets or sets the DssFolder.
        /// </summary>
        public DssFolder PredefinedFolder { get; set; }

        /// <summary>
        /// Gets or sets the DssCredential.
        /// </summary>
        public DssCredential PublicFolderCredential { get; set; }
    }

    /// <summary>
    /// Contains data related to DSS Folder.
    /// </summary>
    public class DssFolder
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use PersonalSharedFolder.
        /// </summary>
        /// <value><c>true</c> if PersonalSharedFolder should be enabled; otherwise, <c>false</c>.</value>
        public bool PersonalSharedFolder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use NameSubfolder.
        /// </summary>
        /// <value><c>true</c> if NameSubfolder should be enabled; otherwise, <c>false</c>.</value>
        public bool UserNameSubfolder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use AccessUserDirectory.
        /// </summary>
        /// <value><c>true</c> if AccessUserDirectory should be enabled; otherwise, <c>false</c>.</value>
        public bool AccessUserDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use VerifyFolderAccess.
        /// </summary>
        /// <value><c>true</c> if VerifyFolderAccess should be enabled; otherwise, <c>false</c>.</value>
        public bool VerifyFolderAccess { get; set; }

        /// <summary>
        /// Gets or sets the FolderSettings.
        /// </summary>
        public FolderSettings StandardSharedNetworkFolder { get; set; }
    }

    #endregion SendToFolder

    #region Workflows
    /// <summary>
    /// Contains data related to DSS Folder WorkflowForm.
    /// </summary>
    public class DssFolderWorkflowForm
    {
        /// <summary>
        /// Gets or sets the Device Address.
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the Device Password.
        /// </summary>
        public string DevicePassword { get; set; }

        /// <summary>
        /// Gets or sets the GroupName.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the MenuName.
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// Gets or sets the FolderPath.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the DssWorkflowForm.
        /// </summary>
        public DssWorkflowForm WorkflowForm { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssFolderWorkflowForm"/> class.
        /// </summary>
        public DssFolderWorkflowForm()
        {
            WorkflowForm = new DssWorkflowForm();
        }
    }

    /// <summary>
    /// Contains data related to DSS SharePoint WorkflowForm.
    /// </summary>
    public class DssSharePointWorkflowForm
    {
        /// <summary>
        /// Gets or sets the DeviceAddress.
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the DevicePassword.
        /// </summary>
        public string DevicePassword { get; set; }

        /// <summary>
        /// Gets or sets the GroupName.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the MenuName.
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// Gets or sets the SharePointUrl.
        /// </summary>
        public Uri SharePointUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use OverwriteExisting.
        /// </summary>
        /// <value><c>true</c> if OverwriteExisting should be enabled; otherwise, <c>false</c>.</value>
        public bool OverwriteExisting { get; set; }

        /// <summary>
        /// Gets or sets the DssWorkflowForm.
        /// </summary>
        public DssWorkflowForm WorkflowForm { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssSharePointWorkflowForm"/> class.
        /// </summary>
        public DssSharePointWorkflowForm()
        {
            WorkflowForm = new DssWorkflowForm();
        }
    }

    /// <summary>
    /// Contains data related to DSS FTP workflow.
    /// </summary>
    public class DssFtpWorkflowForm
    {
        /// <summary>
        /// Gets or sets the GroupName.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the MenuName.
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// Gets or sets the FormName.
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// Gets or sets the FtpServer.
        /// </summary>
        public string FtpServer { get; set; }

        /// <summary>
        /// Gets or sets the FtpPath.
        /// </summary>
        public string FtpPath { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the ImagePresets.
        /// </summary>
        public ImagePresets ImagePreset { get; set; }

        /// <summary>
        /// Gets or sets the DssScanSetting.
        /// </summary>
        public DssScanSetting ScanSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use JobBuild.
        /// </summary>
        /// <value><c>true</c> if JobBuild should be enabled; otherwise, <c>false</c>.</value>
        public bool JobBuild { get; set; }

        /// <summary>
        /// Gets or sets the DssEnhancedWorkflow.
        /// </summary>
        public DssEnhancedWorkflow EnhancedWorkflow { get; set; }

        /// <summary>
        /// Gets or sets the DssFileSetting.
        /// </summary>
        public DssFileSetting FileSettings { get; set; }

        /// <summary>
        /// Gets or sets the MetaDataFileFormat.
        /// </summary>
        public MetaDataFileFormat MetadataFileFormat { get; set; }

        /// <summary>
        /// Gets or sets the Prompts.
        /// </summary>
        [Description("Please enter Comma separated values for prompts")]
        public string Prompts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssFtpWorkflowForm"/> class.
        /// </summary>
        public DssFtpWorkflowForm()
        {
            GroupName = string.Empty;
            MenuName = string.Empty;
            FormName = string.Empty;
            FtpServer = string.Empty;
            FtpPath = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            JobBuild = false;
            ScanSettings = new DssScanSetting();
            ImagePreset = ImagePresets.Color;
            EnhancedWorkflow = new DssEnhancedWorkflow();
            MetadataFileFormat = MetaDataFileFormat.Hps;
            FileSettings = new DssFileSetting();
            Prompts = string.Empty;
        }
    }

    /// <summary>
    /// Contains data related to DSS PrinterWorkflowForm.
    /// </summary>
    public class DssPrinterWorkflowForm
    {
        /// <summary>
        /// Gets or sets the GroupName.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the MenuName.
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// Gets or sets the FormName.
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// Gets or sets the PrinterName.
        /// </summary>
        public string PrinterName { get; set; }

        /// <summary>
        /// Gets or sets the ImagePresets.
        /// </summary>
        public ImagePresets ImagePreset { get; set; }

        /// <summary>
        /// Gets or sets the DssScanSetting.
        /// </summary>
        public DssScanSetting ScanSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use JobBuild.
        /// </summary>
        /// <value><c>true</c> if JobBuild should be enabled; otherwise, <c>false</c>.</value>
        public bool JobBuild { get; set; }

        /// <summary>
        /// Gets or sets the DssEnhancedWorkflow.
        /// </summary>
        public DssEnhancedWorkflow EnhancedWorkflow { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssPrinterWorkflowForm"/> class.
        /// </summary>
        public DssPrinterWorkflowForm()
        {
            GroupName = string.Empty;
            MenuName = string.Empty;
            FormName = string.Empty;
            JobBuild = false;
            ScanSettings = new DssScanSetting();
            ImagePreset = ImagePresets.Color;
            EnhancedWorkflow = new DssEnhancedWorkflow();
        }
    }

    /// <summary>
    /// Contains data related to WorkFlow for Device.
    /// </summary>
    public class WorkFlowforDevice
    {
        /// <summary>
        /// Gets or sets the DeviceAddress.
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the DevicePassword.
        /// </summary>
        public string DevicePassword { get; set; }

        /// <summary>
        /// Gets or sets the GroupName.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkFlowforDevice"/> class.
        /// </summary>
        public WorkFlowforDevice()
        {
            DeviceAddress = string.Empty;
            DevicePassword = string.Empty;
            GroupName = string.Empty;
        }
    }

    /// <summary>
    /// Contains data related to DSS Folder.
    /// </summary>
    public class DssWorkflowForm
    {
        /// <summary>
        /// Gets or sets the FormName.
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// Gets or sets the DssAuthenticationSetting.
        /// </summary>
        public DssAuthenticationSetting AuthenticationSetting { get; set; }

        /// <summary>
        /// Gets or sets the ImagePresets.
        /// </summary>
        public ImagePresets ImagePreset { get; set; }

        /// <summary>
        /// Gets or sets the DssScanSetting.
        /// </summary>
        public DssScanSetting ScanSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use JobBuild.
        /// </summary>
        /// <value><c>true</c> if JobBuild should be enabled; otherwise, <c>false</c>.</value>
        public bool JobBuild { get; set; }

        /// <summary>
        /// Gets or sets the DssEnhancedWorkflow.
        /// </summary>
        public DssEnhancedWorkflow EnhancedWorkflow { get; set; }

        /// <summary>
        /// Gets or sets the DssFileSetting.
        /// </summary>
        public DssFileSetting FileSettings { get; set; }

        /// <summary>
        /// Gets or sets the MetaDataFileFormat.
        /// </summary>
        public MetaDataFileFormat MetadataFileFormat { get; set; }

        /// <summary>
        /// Gets or sets the Prompts.
        /// </summary>
        [Description("Please enter Comma separated values for prompts")]
        public string Prompts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssWorkflowForm"/> class.
        /// </summary>
        public DssWorkflowForm()
        {
            ScanSettings = new DssScanSetting();
            ImagePreset = ImagePresets.Color;
            MetadataFileFormat = MetaDataFileFormat.Hps;
            FileSettings = new DssFileSetting();
            AuthenticationSetting = new DssAuthenticationSetting();
        }
    }

    /// <summary>
    /// Contains data related to Dss Enhanced Workflow.
    /// </summary>
    public class DssEnhancedWorkflow
    {
        /// <summary>
        /// Gets or sets the OriginalSize.
        /// </summary>
        public SizeSetting OriginalSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use BlankPageSuppression.
        /// </summary>
        /// <value><c>true</c> if BlankPageSuppression should be enabled; otherwise, <c>false</c>.</value>
        public bool BlankPageSuppression { get; set; }

        /// <summary>
        /// Gets or sets the NotifyCondition.
        /// </summary>
        public NotifyCondition NotificationCondition { get; set; }

        /// <summary>
        /// Gets or sets the NotifyMethod.
        /// </summary>
        public NotifyMethod NotificationDeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the NotificationEmailAddress.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string NotificationEmailAddress { get; set; }
    }

    /// <summary>
    /// Contains data related to DSS Folder.
    /// </summary>
    public class DssAuthenticationSetting
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use AlwaysUseCredential.
        /// </summary>
        /// <value><c>true</c> if AlwaysUseCredential should be enabled; otherwise, <c>false</c>.</value>
        public bool AlwaysUseCredential { get; set; }

        /// <summary>
        /// Gets or sets the DssCredential.
        /// </summary>
        public DssCredential Credential { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssAuthenticationSetting"/> class.
        /// </summary>
        public DssAuthenticationSetting()
        {
            AlwaysUseCredential = true;
        }
    }

    /// <summary>
    /// Contains data related to DSS Scan Setting.
    /// </summary>
    public class DssScanSetting
    {
        /// <summary>
        /// Gets or sets the OriginalSize.
        /// </summary>
        public SizeSetting OriginalSize { get; set; }

        /// <summary>
        /// Gets or sets the OriginalSides.
        /// </summary>
        public Sides OriginalSides { get; set; }

        /// <summary>
        /// Gets or sets the OptimizeSetting.
        /// </summary>
        public OptimizeSetting Optimize { get; set; }

        /// <summary>
        /// Gets or sets the Orientation.
        /// </summary>
        public Orientation Orientation { get; set; }

        /// <summary>
        /// Gets or sets the BackgroundCleanup.
        /// </summary>
        public BackgroundCleanup BackgroundCleanup { get; set; }

        /// <summary>
        /// Gets or sets the Sharpness.
        /// </summary>
        public Sharpness Sharpness { get; set; }

        /// <summary>
        /// Gets or sets the Darkness.
        /// </summary>
        public Darkness Darkness { get; set; }

        /// <summary>
        /// Gets or sets the Contrast.
        /// </summary>
        public Contrast Contrast { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssScanSetting"/> class.
        /// </summary>
        public DssScanSetting()
        {
            OriginalSize = SizeSetting.Letter;
            OriginalSides = Sides.Simplex;
            Optimize = OptimizeSetting.Setting3;
            Orientation = Orientation.Portrait;
            BackgroundCleanup = BackgroundCleanup.Setting3;
            Sharpness = Sharpness.Setting3;
            Darkness = Darkness.Setting5;
            Contrast = Contrast.Setting5;
        }
    }

    /// <summary>
    /// Contains data related to DSS File Setting.
    /// </summary>
    public class DssFileSetting
    {
        /// <summary>
        /// Gets or sets the ColorPreference.
        /// </summary>
        public ColorPreference ColorPreference { get; set; }

        /// <summary>
        /// Gets or sets the OutputQuality.
        /// </summary>
        public OutputQuality OutputQuality { get; set; }

        /// <summary>
        /// Gets or sets the FileType.
        /// </summary>
        public FileType FileType { get; set; }

        /// <summary>
        /// Gets or sets the Resolution.
        /// </summary>
        public Resolution Resolution { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use HighCompression.
        /// </summary>
        /// <value><c>true</c> if HighCompression should be enabled; otherwise, <c>false</c>.</value>
        public bool HighCompression { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use PdfEncryption.
        /// </summary>
        /// <value><c>true</c> if PdfEncryption should be enabled; otherwise, <c>false</c>.</value>
        public bool PdfEncryption { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssFileSetting"/> class.
        /// </summary>
        public DssFileSetting()
        {
            ColorPreference = ColorPreference.Color;
            OutputQuality = OutputQuality.Medium;
            FileType = FileType.Pdf;
            Resolution = Resolution.Dpi150;
        }
    }

    /// <summary>
    /// Different Sides
    /// </summary>
    public enum Sides
    {
        Simplex,
        Duplex
    }

    /// <summary>
    /// Different Workflow Destination types
    /// </summary>
    public enum WorkflowDestinationType
    {
        [Description("Folder")]
        Folder,

        [Description("FTP Site")]
        Ftp,

        [Description("Printer")]
        Printer,

        [Description("SharePoint®")]
        SharePoint
    }

    /// <summary>
    /// Different ImagePresets types
    /// </summary>
    public enum ImagePresets
    {
        [Description("Color Document")]
        Color,

        [Description("B & W Document")]
        BlackWhite,

        [Description("Photograph")]
        Photograph,

        [Description("Fine Text (OCR)")]
        FineText,

        [Description("High Res Photo")]
        HiResPhoto,

        [Description("Custom")]
        Custom
    }

    /// <summary>
    /// Different SizeSetting types
    /// </summary>
    public enum SizeSetting
    {
        [Description("Letter")]
        Letter,

        [Description("Legal (8.5x14)")]
        Legal,

        [Description("Executive (7.25x10.5)")]
        Executive,

        [Description("A4 (210x297 mm)")]
        A4,

        [Description("A5 (148x210 mm)")]
        A5,

        [Description("B5 (182x257 mm)")]
        B5,

        [Description("Mixed Letter/Legal")]
        Mixed,

        [Description("Ledger (11x17)")]
        Ledger,

        [Description("A3 (297x420 mm)")]
        A3,

        [Description("B4 (257x364 mm)")]
        B4,

        [Description("Statement (5.5x8.5)")]
        Statement,

        [Description("Oficio")]
        Oficio
    }

    /// <summary>
    /// Different Orientation types
    /// </summary>
    public enum Orientation
    {
        Portrait,
        Landscape
    }

    /// <summary>
    /// Different OptimizeSetting types
    /// </summary>
    public enum OptimizeSetting
    {
        [Description("1 - (Text)")]
        Setting1,

        [Description("2")]
        Setting2,

        [Description("3 - (Mixed)")]
        Setting3,

        [Description("4")]
        Setting4,

        [Description("5 - (Printed Picture)")]
        Setting5
    }

    /// <summary>
    /// Different BackgroundCleanup types
    /// </summary>
    public enum BackgroundCleanup
    {
        [Description("1 - (More specks)")]
        Setting1,

        [Description("2")]
        Setting2,

        [Description("3 - (Normal)")]
        Setting3,

        [Description("4")]
        Setting4,

        [Description("5")]
        Setting5,

        [Description("6")]
        Setting6,

        [Description("7")]
        Setting7,

        [Description("8")]
        Setting8,

        [Description("9 - (Less specks)")]
        Setting9
    }

    /// <summary>
    /// Different Sharpness types
    /// </summary>
    public enum Sharpness
    {
        [Description("1 - (Soft Edges)")]
        Setting1,

        [Description("2")]
        Setting2,

        [Description("3 - (Normal)")]
        Setting3,

        [Description("4")]
        Setting4,

        [Description("5 - (Sharper Edges)")]
        Setting5
    }

    /// <summary>
    /// Different Darkness types
    /// </summary>
    public enum Darkness
    {
        [Description("1 - (Lighter)")]
        Setting1,

        [Description("2")]
        Setting2,

        [Description("3")]
        Setting3,

        [Description("4")]
        Setting4,

        [Description("5 - (Normal)")]
        Setting5,

        [Description("6")]
        Setting6,

        [Description("7")]
        Setting7,

        [Description("8")]
        Setting8,

        [Description("9 - (Darker)")]
        Setting9
    }

    /// <summary>
    /// Different Contrast
    /// </summary>
    public enum Contrast
    {
        [Description("1 - (Less)")]
        Setting1,

        [Description("2")]
        Setting2,

        [Description("3")]
        Setting3,

        [Description("4")]
        Setting4,

        [Description("5 - (Normal)")]
        Setting5,

        [Description("6")]
        Setting6,

        [Description("7")]
        Setting7,

        [Description("8")]
        Setting8,

        [Description("9 - (More)")]
        Setting9
    }

    /// <summary>
    /// Different Notify Conditions
    /// </summary>
    public enum NotifyCondition
    {
        [Description("Do not notify")]
        DoNotNotify,

        [Description("Notify only if job fails")]
        NotifyIfFail,

        [Description("Notify when job completes")]
        NotifyWhenComplete
    }

    /// <summary>
    /// Different Countries
    /// </summary>
    public enum Country
    {
        [Description("None")]
        None,

        [Description("Argentina")]
        Argentina,

        [Description("Australia")]
        Australia,

        [Description("Austria")]
        Austria,

        [Description("Belarus")]
        Belarus,

        [Description("Belgium")]
        Belgium,

        [Description("Brazil")]
        Brazil,

        [Description("Bulgaria")]
        Bulgaria,

        [Description("Canada")]
        Canada,

        [Description("China")]
        China,

        [Description("Denmark")]
        Denmark,

        [Description("Finland")]
        Finland,

        [Description("France")]
        France,

        [Description("Germany")]
        Germany,

        [Description("Greece")]
        Greece,

        [Description("India")]
        India,

        [Description("Indonesia")]
        Indonesia,

        [Description("Ireland")]
        Ireland,

        [Description("New Zealand")]
        NewZealand,

        [Description("Philippines")]
        Philippines,

        [Description("Poland")]
        Poland,

        [Description("Russia")]
        Russia,

        [Description("Singapore")]
        Singapore,

        [Description("South Africa")]
        SouthAfrica,

        [Description("Sri Lanka")]
        SriLanka,

        [Description("United Kingdom")]
        UnitedKingdom,

        [Description("United States")]
        UnitedStates,

        [Description("Vietnam")]
        Vietnam,
    }

    /// <summary>
    /// Different Notify Method
    /// </summary>
    public enum NotifyMethod
    {
        [Description("Print")]
        Print,

        [Description("E-mail")]
        Email
    }

    /// <summary>
    /// Different Color Preference
    /// </summary>
    public enum ColorPreference
    {
        [Description("Black/Gray")]
        Black,

        [Description("Color")]
        Color
    }

    /// <summary>
    /// Different Output Quality
    /// </summary>
    public enum OutputQuality
    {
        [Description("Low (small file)")]
        Low,

        [Description("Medium")]
        Medium,

        [Description("High (large file)")]
        High
    }

    /// <summary>
    /// Different FileType
    /// </summary>
    public enum FileType
    {
        [Description("PDF")]
        Pdf,

        [Description("MTIFF")]
        Mtiff,

        [Description("TIFF")]
        Tiff,

        [Description("JPEG")]
        Jpeg,

        [Description("Text (OCR)")]
        Text,

        [Description("Searchable PDF (OCR)")]
        SearchablePdf,

        [Description("PDF/A")]
        Pdfa,

        [Description("Searchable PDF/A (OCR)")]
        SearchablePdfa
    }

    /// <summary>
    /// Different Resolution
    /// </summary>
    public enum Resolution
    {
        [Description("600 dpi")]
        Dpi600,

        [Description("400 dpi")]
        Dpi400,

        [Description("300 dpi")]
        Dpi300,

        [Description("200 dpi")]
        Dpi200,

        [Description("150 dpi")]
        Dpi150,

        [Description("75 dpi")]
        Dpi75
    }

    /// <summary>
    /// Different Meta Data File Format
    /// </summary>
    public enum MetaDataFileFormat
    {
        [Description("None")]
        None,
        [Description("HPS")]
        Hps,
        [Description("XML")]
        Xml,
        [Description("FNA")]
        Fna
    }

    #endregion Workflows

    /// <summary>
    /// Contains data related to DSS Device Configuration.
    /// </summary>
    public class DssDeviceConfiguration
    {
        /// <summary>
        /// Gets or sets the GroupName.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the DeviceAddress.
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the DeviceUserName.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string DeviceUserName { get; set; }

        /// <summary>
        /// Gets or sets the DevicePassword.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string DevicePassword { get; set; }

        /// <summary>
        /// Gets or sets the DssTemplate.
        /// </summary>
        public DssTemplate Template { get; set; }
    }

    /// <summary>
    /// Contains data related to DSS Create Template.
    /// </summary>
    public class DssCreateTemplate
    {
        /// <summary>
        /// Gets or sets the DeviceAddress.
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the DevicePassword.
        /// </summary>      
        public string DevicePassword { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssCreateTemplate"/> class.
        /// </summary>
        public DssCreateTemplate()
        {
            DeviceAddress = string.Empty;
            DevicePassword = string.Empty;
        }
    }

    /// <summary>
    /// Contains data related to DSS Template.
    /// </summary>
    public class DssTemplate
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Contains data related to Remove Device.
    /// </summary>
    public class RemoveDevice
    {
        /// <summary>
        /// Gets or sets the DeviceAddress.
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the DevicePassword.
        /// </summary>
        public string DevicePassword { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveDevice"/> class.
        /// </summary>
        public RemoveDevice()
        {
            DeviceAddress = string.Empty;
            DevicePassword = string.Empty;
        }
    }

    /// <summary>
    /// Contains data related to DSS Import.
    /// </summary>
    #region Import
    public class DssImport
    {
        /// <summary>
        /// Gets or sets the FileName.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssImport"/> class.
        /// </summary>
        public DssImport()
        {
            FileName = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
        }
    }
    #endregion Import

    #region Addressing
    /// <summary>
    /// Contains data related to DSS Addressing.
    /// </summary>
    public class DssAddressing
    {
        /// <summary>
        /// Gets or sets the LdapServerAddress.
        /// </summary>
        public string LdapServerAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use Ssl.
        /// </summary>
        /// <value><c>true</c> if Ssl should be enabled; otherwise, <c>false</c>.</value>
        public bool UseSsl { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the BindandSearch.
        /// </summary>
        public string BindandSearch { get; set; }

        /// <summary>
        /// Gets or sets the MatchNameAttribute.
        /// </summary>
        public string MatchNameAttribute { get; set; }

        /// <summary>
        /// Gets or sets the EmailAttribute.
        /// </summary>
        public string EmailAttribute { get; set; }

        /// <summary>
        /// Gets or sets the EnterCharatersToVerify.
        /// </summary>
        public string EnterCharatersToVerify { get; set; }

        /// <summary>
        /// Gets or sets the DssMaximunLdapAddress.
        /// </summary>
        public DssMaximunLdapAddress MaximunLdapAddress { get; set; }

        /// <summary>
        /// Gets or sets the DssMaximunSearchTime.
        /// </summary>
        public DssMaximunSearchTime MaximunSearchTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssAddressing"/> class.
        /// </summary>
        public DssAddressing()
        {
            LdapServerAddress = string.Empty;
            UseSsl = false;
            UserName = string.Empty;
            Password = string.Empty;
            BindandSearch = string.Empty;
            MatchNameAttribute = string.Empty;
            EmailAttribute = string.Empty;
            EnterCharatersToVerify = string.Empty;
            MaximunLdapAddress = DssMaximunLdapAddress.FiveAddresses;
            MaximunSearchTime = DssMaximunSearchTime.FiveSeconds;
        }
    }


    /// <summary>
    /// Contains data related to DSS Personal Contact Addressing.
    /// </summary>
    public class DssPersonalContactAddressing
    {
        /// <summary>
        /// Gets or sets the Domain.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the EnterCharatersToVerify.
        /// </summary>
        public string EnterCharatersToVerify { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssPersonalContactAddressing"/> class.
        /// </summary>
        public DssPersonalContactAddressing()
        {
            Domain = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            EnterCharatersToVerify = string.Empty;
        }
    }

    /// <summary>
    /// Contains data related to DSS Address Book Import.
    /// </summary>
    public class DssAddressBookImport
    {
        /// <summary>
        /// Gets or sets the FileName.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssAddressBookImport"/> class.
        /// </summary>
        public DssAddressBookImport()
        {
            FileName = string.Empty;
        }
    }

    /// <summary>
    /// Contains data related to Creating Contact in DSS Address Book .
    /// </summary>
    public class DssAddressBookContacts
    {
        /// <summary>
        /// Gets or sets the ContactName.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the EmailAddress.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the FaxNumber.
        /// </summary>
        public string FaxNumber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DssAddressBookContacts"/> class.
        /// </summary>
        public DssAddressBookContacts()
        {
            ContactName = string.Empty;
            EmailAddress = string.Empty;
            FaxNumber = string.Empty;
        }
    }


    /// <summary>
    /// Different Maximun Ldap Address
    /// </summary>
    public enum DssMaximunLdapAddress
    {
        [Description("5 Addresses")]
        FiveAddresses,
        [Description("10 Addresses")]
        TenAddresses,
        [Description("20 Addresses")]
        TwentyAddresses,
        [Description("50 Addresses")]
        FiftyAddresses,
        [Description("100 Addresses")]
        OneHundredAddresses,
        [Description("200 Addresses")]
        TwoHundredAddresses,
        [Description("500 Addresses")]
        FiveHundredAddresses,
        [Description("1000 Addresses")]
        OneThousandAddresses,
        [Description("5000 Addresses")]
        FiveThousandAddresses,
        [Description("10000 Addresses")]
        TenThousandAddresses,
        [Description("Unlimited Addresses")]
        UnlimitedAddresses,
    }

    /// <summary>
    /// Different Maximun SearchTime
    /// </summary>
    public enum DssMaximunSearchTime
    {
        [Description("5 Seconds")]
        FiveSeconds,
        [Description("10 Seconds")]
        TenSeconds,
        [Description("15 Seconds")]
        FifteenSeconds,
        [Description("30 Seconds")]
        ThirtySeconds,
        [Description("1 Minute")]
        OneMinute,
        [Description("5 Minute")]
        FiveMinute,
        [Description("10 Minute")]
        TenMinute,
        [Description("Infinte")]
        Infinte,
    }
    #endregion Addressing

    #region General
    /// <summary>
    /// Contains data related to DSS General.
    /// </summary>
    public class DssGeneral
    {
        /// <summary>
        /// Gets or sets the LicenseCode.
        /// </summary>
        public string LicenseCode { get; set; }

        /// <summary>
        /// Gets or sets the AdministrationInfo.
        /// </summary>
        public AdministrationInfo AdminInfo { get; set; }

        /// <summary>
        /// Gets or sets the DssBackupFile.
        /// </summary>
        public DssBackupFile Backup { get; set; }

        /// <summary>
        /// Gets or sets the DssRestoreFile.
        /// </summary>
        public DssRestoreFile Restore { get; set; }
    }

    /// <summary>
    /// Contains data related to DSS AdministrationInfo.
    /// </summary>
    public class AdministrationInfo
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNumber.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the EmailAddress.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the Location.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use Notify.
        /// </summary>
        /// <value><c>true</c> if Notify should be enabled; otherwise, <c>false</c>.</value>
        [Description("Please enter EMPTY to ignore")]
        public bool Notify { get; set; }
    }

    /// <summary>
    /// Contains data related to DSS Backup File.
    /// </summary>
    public class DssBackupFile
    {
        /// <summary>
        /// Gets or sets the FileName.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the EncryptionKey.
        /// </summary>
        [Description("Please enter EMPTY to ignore")]
        public string EncryptionKey { get; set; }
    }

    /// <summary>
    /// Contains data related to DSS Restore File.
    /// </summary>
    public class DssRestoreFile
    {
        /// <summary>
        /// Gets or sets the DssBackupFile.
        /// </summary>
        public DssBackupFile BackupFile { get; set; }

        /// <summary>
        /// List of Restore Tabs.
        /// </summary>
        public List<DssTabs> RestoreTabsList { get; set; }

        /// <summary>
        /// List of Merge Tabs.
        /// </summary>
        public List<DssTabs> MergeTabsList { get; set; }
    }

    /// <summary>
    /// Different Tabs of DSS
    /// </summary>
    public enum DssTabs
    {
        General,
        Authentication,
        Email,
        Fax,
        SendToFolder,
        Workflows,
        Addressing,
        AddressBook,
        Devices,
        Templates
    }

    #endregion General
}