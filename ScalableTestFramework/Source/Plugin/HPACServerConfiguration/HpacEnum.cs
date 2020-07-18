using System.ComponentModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    /// <summary>
    /// Different Hpac Authentication Mode
    /// </summary>
    public enum HpacAuthenticationMode
    {
        /// <summary>
        /// Authenticate using Pic code
        /// </summary>
        Code,
        /// <summary>
        /// Authenticate using smart card
        /// </summary>
        Card,
        /// <summary>
        /// Authenticate using the user's smart card (card ID 00001)
        /// </summary>
        CodeOrCard,
        /// <summary>
        /// Authenticate using the user's code or card (card ID 00001)
        /// </summary>
        CodeAndCard,
    }

    /// <summary>
    /// Different Hpac Tile
    /// </summary>
    public enum HpacTile
    {
        /// <summary>
        /// Perform device installation or configuration
        /// </summary>
        Devices,

        /// <summary>
        /// Perform IRM operation
        /// </summary>
        IRM,

        /// <summary>
        /// Perform PrintServer operation
        /// </summary>
        PrintServer,

        /// <summary>
        /// Perform JobAccounting operation
        /// </summary>
        JobAccounting,

        /// <summary>
        /// Perform Settings operation
        /// </summary>
        Settings

    }

    /// <summary>
    /// Different Hpac DataStorage
    /// </summary>
    public enum HpacDataStorage
    {
        /// <summary>
        /// Store Data in LDAP
        /// </summary>
        LDAP,
        /// <summary>
        /// Store Data in Database
        /// </summary>
        Database

    }

    /// <summary>
    /// Different Hpac Configuration
    /// </summary>
    public enum HpacConfiguration
    {
        /// <summary>
        /// Configure Pull Printing
        /// </summary>
        [Description("Pull Printing")]
        PullPrinting,
        /// <summary>
        /// Configure Tracking
        /// </summary>
        Tracking,
        /// <summary>
        /// Configure Authentication
        /// </summary>
        Authentication,
        /// <summary>
        /// Configure Authorization
        /// </summary>
        Authorization,
        /// <summary>
        /// Configure Quota
        /// </summary>
        Quota,
        /// <summary>
        /// Configure Confirmation Trap
        /// </summary>
        [Description("Confirmation Trap")]
        ConfirmationTrap,
        [Description("IPM")]
        IPM
       
    }

    /// <summary>
    /// Different Database Authentication Type
    /// </summary>
    public enum DatabaseAuthenticationType
    {
        /// <summary>
        /// Using SQL
        /// </summary>
        SQL,
        /// <summary>
        /// Using Windows
        /// </summary>
        Windows,
    }

    /// <summary>
    /// Different Device Operation
    /// </summary>
    public enum DeviceOperation
    {
        /// <summary>
        /// Adds the device
        /// </summary>
        Add,
        /// <summary>
        /// Configure the Device
        /// </summary>
        Configure,
        /// <summary>
        /// Install the HP Agent to the Device
        /// </summary>
        InstallHPAgent,
    }

    /// <summary>
    /// Different IRM Operation
    /// </summary>
    public enum IrmOperation
    {
        /// <summary>
        /// Adds the device
        /// </summary>
        GeneralSettings,
        /// <summary>
        /// Configure the Device
        /// </summary>
        LDAPServerConfigure,
        /// <summary>
        /// Install the HP Agent to the Device
        /// </summary>
        CodeandorCardAttribute,
        /// <summary>
        /// Rights
        /// </summary>
        Rights,
        /// <summary>
        /// Install the HP Agent to the Device
        /// </summary>
        ADUserEditor,
    }

    /// <summary>
    /// Different Settings Operation
    /// </summary>
    public enum SettingsOperation
    {
        /// <summary>
        /// Adds the PrintQueue
        /// </summary>
        AddPrintQueue,
        /// <summary>
        /// Delete the PrintQueue
        /// </summary>
        DeletePrintQueue,
        /// <summary>
        /// IRM Database settings
        /// </summary>
        IrmDatabaseSettings,
        /// <summary>
        /// Quota Settings
        /// </summary>
        QuotaSettings,
        /// <summary>
        /// Encryption
        /// </summary>
        Protocol,
        /// <summary>
        /// Adds the Server URI
        /// </summary>
        AddServerURI

    }

    /// <summary>
    /// Different SNMPTracking Operation
    /// </summary>
    public enum SNMPTracking
    {
        /// <summary>
        /// Track copies
        /// </summary>
        [Description("Track copies")]
        Copies,
        /// <summary>
        /// Track digital sending
        /// </summary>
        [Description("Track digital sending")]
        DigitalSending,
    }

    /// <summary>
    /// Different Protocol Operation
    /// </summary>
    [DataContract]
    public enum ProtocolOptions
    {
        /// <summary>
        /// TCPIP/SOCK
        /// </summary>
        [EnumMember]
        SOCK = 0,
        /// <summary>
        /// TCPIP/PJL
        /// </summary>
        [EnumMember]
        PJL = 1,
        /// <summary>
        /// TCPIP/IPPS
        /// </summary>
        [EnumMember]
        IPPS = 2,
    }

    /// <summary>
    /// Different JobAccounting Operation 
    /// </summary>
    public enum JobAccountingOperation
    {
        /// <summary>
        /// Quota Operation 
        /// </summary>
        Quota,
        /// <summary>
        /// Report Operation
        /// </summary>
        Report,
    }

    /// <summary>
    /// Different OutputFormat Operation 
    /// </summary>
    public enum OutputFormat
    {
        /// <summary>
        /// Pdf type
        /// </summary>
        Pdf,
        /// <summary>
        /// Excel type
        /// </summary>
        Excel,
        /// <summary>
        /// Word type
        /// </summary>
        Word,
    }

    public enum HpacAuthenticators
    {
        /// <summary>
        /// Configure HPAC Local List
        /// </summary>
        [Description("HPAC - Local List")]
        LocalList,
        /// <summary>
        /// Configure HPAC PIC Service
        /// </summary>
        [Description("HPAC - PIC Server")]
        PicServer,
        /// <summary>
        /// Configure HPAC IRM Service
        /// </summary>
        [Description("HPAC - IRM Server")]
        IrmServer,
        /// <summary>
        /// Configure DRA Server (Default)
        /// </summary>
        [Description("HPAC - DRA Server")]
        DraServer
    }
}
