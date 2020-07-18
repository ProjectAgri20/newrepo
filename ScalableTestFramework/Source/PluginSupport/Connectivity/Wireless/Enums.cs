﻿using System;

namespace HP.ScalableTest.PluginSupport.Connectivity.Wireless
{

    /// <summary>
    /// Defines various opcodes used to set and query parameters for an interface.
    /// </summary>
    /// <remarks>
    /// Corresponds to the native <c>WLAN_INTF_OPCODE</c> type.
    /// </remarks>
    public enum WlanIntfOpcode
    {
        /// <summary>
        /// Opcode used to set or query whether auto config is enabled.
        /// </summary>
        AutoconfEnabled = 1,
        /// <summary>
        /// Opcode used to set or query whether background scan is enabled.
        /// </summary>
        BackgroundScanEnabled,
        /// <summary>
        /// Opcode used to set or query the media streaming mode of the driver.
        /// </summary>
        MediaStreamingMode,
        /// <summary>
        /// Opcode used to set or query the radio state.
        /// </summary>
        RadioState,
        /// <summary>
        /// Opcode used to set or query the BSS type of the interface.
        /// </summary>
        BssType,
        /// <summary>
        /// Opcode used to query the state of the interface.
        /// </summary>
        InterfaceState,
        /// <summary>
        /// Opcode used to query information about the current connection of the interface.
        /// </summary>
        CurrentConnection,
        /// <summary>
        /// Opcose used to query the current channel on which the wireless interface is operating.
        /// </summary>
        ChannelNumber,
        /// <summary>
        /// Opcode used to query the supported auth/cipher pairs for infrastructure mode.
        /// </summary>
        SupportedInfrastructureAuthCipherPairs,
        /// <summary>
        /// Opcode used to query the supported auth/cipher pairs for ad hoc mode.
        /// </summary>
        SupportedAdhocAuthCipherPairs,
        /// <summary>
        /// Opcode used to query the list of supported country or region strings.
        /// </summary>
        SupportedCountryOrRegionStringList,
        /// <summary>
        /// Opcode used to set or query the current operation mode of the wireless interface.
        /// </summary>
        CurrentOperationMode,
        /// <summary>
        /// Opcode used to query driver statistics.
        /// </summary>
        Statistics = 0x10000101,
        /// <summary>
        /// Opcode used to query the received signal strength.
        /// </summary>
        RSSI,
        SecurityStart = 0x20010000,
        SecurityEnd = 0x2fffffff,
        IhvStart = 0x30000000,
        IhvEnd = 0x3fffffff
    }

    /// <summary>
    /// Specifies the origin of automatic configuration (auto config) settings.
    /// </summary>
    /// <remarks>
    /// Corresponds to the native <c>WLAN_OPCODE_VALUE_TYPE</c> type.
    /// </remarks>
    public enum WlanOpcodeValueType
    {
        /// <summary>
        /// The auto config settings were queried, but the origin of the settings was not determined.
        /// </summary>
        QueryOnly = 0,
        /// <summary>
        /// The auto config settings were set by group policy.
        /// </summary>
        SetByGroupPolicy = 1,
        /// <summary>
        /// The auto config settings were set by the user.
        /// </summary>
        SetByUser = 2,
        /// <summary>
        /// The auto config settings are invalid.
        /// </summary>
        Invalid = 3
    }

    /// <summary>
    /// Defines flags passed to <see cref="WlanGetAvailableNetworkList"/>.
    /// </summary>
    [Flags]
    public enum WlanGetAvailableNetworkFlags
    {
        /// <summary>
        /// Include all ad-hoc network profiles in the available network list, including profiles that are not visible.
        /// </summary>
        IncludeAllAdhocProfiles = 0x00000001,
        /// <summary>
        /// Include all hidden network profiles in the available network list, including profiles that are not visible.
        /// </summary>
        IncludeAllManualHiddenProfiles = 0x00000002
    }

    /// <summary>
    /// Contains various flags for the network.
    /// </summary>
    [Flags]
    public enum WlanAvailableNetworkFlags
    {
        /// <summary>
        /// This network is currently connected.
        /// </summary>
        Connected = 0x00000001,
        /// <summary>
        /// There is a profile for this network.
        /// </summary>
        HasProfile = 0x00000002
    }

    [Flags]
    public enum WlanProfileFlags
    {
        /// <remarks>
        /// The only option available on Windows XP SP2.
        /// </remarks>
        AllUser = 0,
        GroupPolicy = 1,
        User = 2
    }

    /// <summary>
    /// Defines the access mask of an all-user profile.
    /// </summary>
    [Flags]
    public enum WlanAccess
    {
        /// <summary>
        /// The user can view profile permissions.
        /// </summary>
        ReadAccess = 0x00020000 | 0x0001,
        /// <summary>
        /// The user has read access, and the user can also connect to and disconnect from a network using the profile.
        /// </summary>
        ExecuteAccess = ReadAccess | 0x0020,
        /// <summary>
        /// The user has execute access and the user can also modify and delete permissions associated with a profile.
        /// </summary>
        WriteAccess = ReadAccess | ExecuteAccess | 0x0002 | 0x00010000 | 0x00040000
    }

    /// <summary>
    /// Specifies where the notification comes from.
    /// </summary>
    [Flags]
    public enum WlanNotificationSource
    {
        None = 0,
        /// <summary>
        /// All notifications, including those generated by the 802.1X module.
        /// </summary>
        All = 0X0000FFFF,
        /// <summary>
        /// Notifications generated by the auto configuration module.
        /// </summary>
        ACM = 0X00000008,
        /// <summary>
        /// Notifications generated by MSM.
        /// </summary>
        MSM = 0X00000010,
        /// <summary>
        /// Notifications generated by the security module.
        /// </summary>
        Security = 0X00000020,
        /// <summary>
        /// Notifications generated by independent hardware vendors (IHV).
        /// </summary>
        IHV = 0X00000040
    }

    /// <summary>
    /// Indicates the type of an ACM (<see cref="WlanNotificationSource.ACM"/>) notification.
    /// </summary>
    /// <remarks>
    /// The enumeration identifiers correspond to the native <c>wlan_notification_acm_</c> identifiers.
    /// On Windows XP SP2, only the <c>ConnectionComplete</c> and <c>Disconnected</c> notifications are available.
    /// </remarks>
    public enum WlanNotificationCodeAcm
    {
        AutoconfEnabled = 1,
        AutoconfDisabled,
        BackgroundScanEnabled,
        BackgroundScanDisabled,
        BssTypeChange,
        PowerSettingChange,
        ScanComplete,
        ScanFail,
        ConnectionStart,
        ConnectionComplete,
        ConnectionAttemptFail,
        FilterListChange,
        InterfaceArrival,
        InterfaceRemoval,
        ProfileChange,
        ProfileNameChange,
        ProfilesExhausted,
        NetworkNotAvailable,
        NetworkAvailable,
        Disconnecting,
        Disconnected,
        AdhocNetworkStateChange
    }

    /// <summary>
    /// Indicates the type of an MSM (<see cref="WlanNotificationSource.MSM"/>) notification.
    /// </summary>
    /// <remarks>
    /// The enumeration identifiers correspond to the native <c>wlan_notification_msm_</c> identifiers.
    /// </remarks>
    public enum WlanNotificationCodeMsm
    {
        Associating = 1,
        Associated,
        Authenticating,
        Connected,
        RoamingStart,
        RoamingEnd,
        RadioStateChange,
        SignalQualityChange,
        Disassociating,
        Disconnected,
        PeerJoin,
        PeerLeave,
        AdapterRemoval,
        AdapterOperationModeChange
    }

    /// <summary>
    /// Defines connection parameter flags.
    /// </summary>
    [Flags]
    public enum WlanConnectionFlags
    {
        /// <summary>
        /// Connect to the destination network even if the destination is a hidden network. A hidden network does not broadcast its SSID. Do not use this flag if the destination network is an ad-hoc network.
        /// <para>If the profile specified by <see cref="WlanConnectionParameters.profile"/> is not <c>null</c>, then this flag is ignored and the nonBroadcast profile element determines whether to connect to a hidden network.</para>
        /// </summary>
        HiddenNetwork = 0x00000001,
        /// <summary>
        /// Do not form an ad-hoc network. Only join an ad-hoc network if the network already exists. Do not use this flag if the destination network is an infrastructure network.
        /// </summary>
        AdhocJoinOnly = 0x00000002,
        /// <summary>
        /// Ignore the privacy bit when connecting to the network. Ignoring the privacy bit has the effect of ignoring whether packets are encryption and ignoring the method of encryption used. Only use this flag when connecting to an infrastructure network using a temporary profile.
        /// </summary>
        IgnorePrivacyBit = 0x00000004,
        /// <summary>
        /// Exempt EAPOL traffic from encryption and decryption. This flag is used when an application must send EAPOL traffic over an infrastructure network that uses Open authentication and WEP encryption. This flag must not be used to connect to networks that require 802.1X authentication. This flag is only valid when <see cref="WlanConnectionParameters.wlanConnectionMode"/> is set to <see cref="WlanConnectionMode.TemporaryProfile"/>. Avoid using this flag whenever possible.
        /// </summary>
        EapolPassthrough = 0x00000008
    }


    /// <summary>
    /// The connection state of an ad hoc network.
    /// </summary>
    public enum WlanAdhocNetworkState
    {
        /// <summary>
        /// The ad hoc network has been formed, but no client or host is connected to the network.
        /// </summary>
        Formed = 0,
        /// <summary>
        /// A client or host is connected to the ad hoc network.
        /// </summary>
        Connected = 1
    }

    // TODO: .NET-ify the WlanReasonCode enum (naming convention + docs).

    /// <summary>
    /// Specifies reasons for a failure of a WLAN operation.
    /// </summary>
    /// <remarks>
    /// To get the WLAN API native reason code identifiers, prefix the identifiers with <c>WLAN_REASON_CODE_</c>.
    /// </remarks>
    public enum WlanReasonCode
    {
        Success = 0,
        // general codes
        UNKNOWN = 0x10000 + 1,

        RANGE_SIZE = 0x10000,
        BASE = 0x10000 + RANGE_SIZE,

        // range for Auto Config
        //
        AC_BASE = 0x10000 + RANGE_SIZE,
        AC_CONNECT_BASE = (AC_BASE + RANGE_SIZE / 2),
        AC_END = (AC_BASE + RANGE_SIZE - 1),

        // range for profile manager
        // it has profile adding failure reason codes, but may not have 
        // connection reason codes
        //
        PROFILE_BASE = 0x10000 + (7 * RANGE_SIZE),
        PROFILE_CONNECT_BASE = (PROFILE_BASE + RANGE_SIZE / 2),
        PROFILE_END = (PROFILE_BASE + RANGE_SIZE - 1),

        // range for MSM
        //
        MSM_BASE = 0x10000 + (2 * RANGE_SIZE),
        MSM_CONNECT_BASE = (MSM_BASE + RANGE_SIZE / 2),
        MSM_END = (MSM_BASE + RANGE_SIZE - 1),

        // range for MSMSEC
        //
        MSMSEC_BASE = 0x10000 + (3 * RANGE_SIZE),
        MSMSEC_CONNECT_BASE = (MSMSEC_BASE + RANGE_SIZE / 2),
        MSMSEC_END = (MSMSEC_BASE + RANGE_SIZE - 1),

        // AC network incompatible reason codes
        //
        NETWORK_NOT_COMPATIBLE = (AC_BASE + 1),
        PROFILE_NOT_COMPATIBLE = (AC_BASE + 2),

        // AC connect reason code
        //
        NO_AUTO_CONNECTION = (AC_CONNECT_BASE + 1),
        NOT_VISIBLE = (AC_CONNECT_BASE + 2),
        GP_DENIED = (AC_CONNECT_BASE + 3),
        USER_DENIED = (AC_CONNECT_BASE + 4),
        BSS_TYPE_NOT_ALLOWED = (AC_CONNECT_BASE + 5),
        IN_FAILED_LIST = (AC_CONNECT_BASE + 6),
        IN_BLOCKED_LIST = (AC_CONNECT_BASE + 7),
        SSID_LIST_TOO_LONG = (AC_CONNECT_BASE + 8),
        CONNECT_CALL_FAIL = (AC_CONNECT_BASE + 9),
        SCAN_CALL_FAIL = (AC_CONNECT_BASE + 10),
        NETWORK_NOT_AVAILABLE = (AC_CONNECT_BASE + 11),
        PROFILE_CHANGED_OR_DELETED = (AC_CONNECT_BASE + 12),
        KEY_MISMATCH = (AC_CONNECT_BASE + 13),
        USER_NOT_RESPOND = (AC_CONNECT_BASE + 14),

        // Profile validation errors
        //
        INVALID_PROFILE_SCHEMA = (PROFILE_BASE + 1),
        PROFILE_MISSING = (PROFILE_BASE + 2),
        INVALID_PROFILE_NAME = (PROFILE_BASE + 3),
        INVALID_PROFILE_TYPE = (PROFILE_BASE + 4),
        INVALID_PHY_TYPE = (PROFILE_BASE + 5),
        MSM_SECURITY_MISSING = (PROFILE_BASE + 6),
        IHV_SECURITY_NOT_SUPPORTED = (PROFILE_BASE + 7),
        IHV_OUI_MISMATCH = (PROFILE_BASE + 8),
        // IHV OUI not present but there is IHV settings in profile
        IHV_OUI_MISSING = (PROFILE_BASE + 9),
        // IHV OUI is present but there is no IHV settings in profile
        IHV_SETTINGS_MISSING = (PROFILE_BASE + 10),
        // both/conflict MSMSec and IHV security settings exist in profile 
        CONFLICT_SECURITY = (PROFILE_BASE + 11),
        // no IHV or MSMSec security settings in profile
        SECURITY_MISSING = (PROFILE_BASE + 12),
        INVALID_BSS_TYPE = (PROFILE_BASE + 13),
        INVALID_ADHOC_CONNECTION_MODE = (PROFILE_BASE + 14),
        NON_BROADCAST_SET_FOR_ADHOC = (PROFILE_BASE + 15),
        AUTO_SWITCH_SET_FOR_ADHOC = (PROFILE_BASE + 16),
        AUTO_SWITCH_SET_FOR_MANUAL_CONNECTION = (PROFILE_BASE + 17),
        IHV_SECURITY_ONEX_MISSING = (PROFILE_BASE + 18),
        PROFILE_SSID_INVALID = (PROFILE_BASE + 19),
        TOO_MANY_SSID = (PROFILE_BASE + 20),

        // MSM network incompatible reasons
        //
        UNSUPPORTED_SECURITY_SET_BY_OS = (MSM_BASE + 1),
        UNSUPPORTED_SECURITY_SET = (MSM_BASE + 2),
        BSS_TYPE_UNMATCH = (MSM_BASE + 3),
        PHY_TYPE_UNMATCH = (MSM_BASE + 4),
        DATARATE_UNMATCH = (MSM_BASE + 5),

        // MSM connection failure reasons, to be defined
        // failure reason codes
        //
        // user called to disconnect
        USER_CANCELLED = (MSM_CONNECT_BASE + 1),
        // got disconnect while associating
        ASSOCIATION_FAILURE = (MSM_CONNECT_BASE + 2),
        // timeout for association
        ASSOCIATION_TIMEOUT = (MSM_CONNECT_BASE + 3),
        // pre-association security completed with failure
        PRE_SECURITY_FAILURE = (MSM_CONNECT_BASE + 4),
        // fail to start post-association security
        START_SECURITY_FAILURE = (MSM_CONNECT_BASE + 5),
        // post-association security completed with failure
        SECURITY_FAILURE = (MSM_CONNECT_BASE + 6),
        // security watchdog timeout
        SECURITY_TIMEOUT = (MSM_CONNECT_BASE + 7),
        // got disconnect from driver when roaming
        ROAMING_FAILURE = (MSM_CONNECT_BASE + 8),
        // failed to start security for roaming
        ROAMING_SECURITY_FAILURE = (MSM_CONNECT_BASE + 9),
        // failed to start security for adhoc-join
        ADHOC_SECURITY_FAILURE = (MSM_CONNECT_BASE + 10),
        // got disconnection from driver
        DRIVER_DISCONNECTED = (MSM_CONNECT_BASE + 11),
        // driver operation failed
        DRIVER_OPERATION_FAILURE = (MSM_CONNECT_BASE + 12),
        // Ihv service is not available
        IHV_NOT_AVAILABLE = (MSM_CONNECT_BASE + 13),
        // Response from ihv timed out
        IHV_NOT_RESPONDING = (MSM_CONNECT_BASE + 14),
        // Timed out waiting for driver to disconnect
        DISCONNECT_TIMEOUT = (MSM_CONNECT_BASE + 15),
        // An internal error prevented the operation from being completed.
        INTERNAL_FAILURE = (MSM_CONNECT_BASE + 16),
        // UI Request timed out.
        UI_REQUEST_TIMEOUT = (MSM_CONNECT_BASE + 17),
        // Roaming too often, post security is not completed after 5 times.
        TOO_MANY_SECURITY_ATTEMPTS = (MSM_CONNECT_BASE + 18),

        // MSMSEC reason codes
        //

        MSMSEC_MIN = MSMSEC_BASE,

        // Key index specified is not valid
        MSMSEC_PROFILE_INVALID_KEY_INDEX = (MSMSEC_BASE + 1),
        // Key required, PSK present
        MSMSEC_PROFILE_PSK_PRESENT = (MSMSEC_BASE + 2),
        // Invalid key length
        MSMSEC_PROFILE_KEY_LENGTH = (MSMSEC_BASE + 3),
        // Invalid PSK length
        MSMSEC_PROFILE_PSK_LENGTH = (MSMSEC_BASE + 4),
        // No auth/cipher specified
        MSMSEC_PROFILE_NO_AUTH_CIPHER_SPECIFIED = (MSMSEC_BASE + 5),
        // Too many auth/cipher specified
        MSMSEC_PROFILE_TOO_MANY_AUTH_CIPHER_SPECIFIED = (MSMSEC_BASE + 6),
        // Profile contains duplicate auth/cipher
        MSMSEC_PROFILE_DUPLICATE_AUTH_CIPHER = (MSMSEC_BASE + 7),
        // Profile raw data is invalid (1x or key data)
        MSMSEC_PROFILE_RAWDATA_INVALID = (MSMSEC_BASE + 8),
        // Invalid auth/cipher combination
        MSMSEC_PROFILE_INVALID_AUTH_CIPHER = (MSMSEC_BASE + 9),
        // 802.1x disabled when it's required to be enabled
        MSMSEC_PROFILE_ONEX_DISABLED = (MSMSEC_BASE + 10),
        // 802.1x enabled when it's required to be disabled
        MSMSEC_PROFILE_ONEX_ENABLED = (MSMSEC_BASE + 11),
        MSMSEC_PROFILE_INVALID_PMKCACHE_MODE = (MSMSEC_BASE + 12),
        MSMSEC_PROFILE_INVALID_PMKCACHE_SIZE = (MSMSEC_BASE + 13),
        MSMSEC_PROFILE_INVALID_PMKCACHE_TTL = (MSMSEC_BASE + 14),
        MSMSEC_PROFILE_INVALID_PREAUTH_MODE = (MSMSEC_BASE + 15),
        MSMSEC_PROFILE_INVALID_PREAUTH_THROTTLE = (MSMSEC_BASE + 16),
        // PreAuth enabled when PMK cache is disabled
        MSMSEC_PROFILE_PREAUTH_ONLY_ENABLED = (MSMSEC_BASE + 17),
        // Capability matching failed at network
        MSMSEC_CAPABILITY_NETWORK = (MSMSEC_BASE + 18),
        // Capability matching failed at NIC
        MSMSEC_CAPABILITY_NIC = (MSMSEC_BASE + 19),
        // Capability matching failed at profile
        MSMSEC_CAPABILITY_PROFILE = (MSMSEC_BASE + 20),
        // Network does not support specified discovery type
        MSMSEC_CAPABILITY_DISCOVERY = (MSMSEC_BASE + 21),
        // Passphrase contains invalid character
        MSMSEC_PROFILE_PASSPHRASE_CHAR = (MSMSEC_BASE + 22),
        // Key material contains invalid character
        MSMSEC_PROFILE_KEYMATERIAL_CHAR = (MSMSEC_BASE + 23),
        // Wrong key type specified for the auth/cipher pair
        MSMSEC_PROFILE_WRONG_KEYTYPE = (MSMSEC_BASE + 24),
        // "Mixed cell" suspected (AP not beaconing privacy, we have privacy enabled profile)
        MSMSEC_MIXED_CELL = (MSMSEC_BASE + 25),
        // Auth timers or number of timeouts in profile is incorrect
        MSMSEC_PROFILE_AUTH_TIMERS_INVALID = (MSMSEC_BASE + 26),
        // Group key update interval in profile is incorrect
        MSMSEC_PROFILE_INVALID_GKEY_INTV = (MSMSEC_BASE + 27),
        // "Transition network" suspected, trying legacy 802.11 security
        MSMSEC_TRANSITION_NETWORK = (MSMSEC_BASE + 28),
        // Key contains characters which do not map to ASCII
        MSMSEC_PROFILE_KEY_UNMAPPED_CHAR = (MSMSEC_BASE + 29),
        // Capability matching failed at profile (auth not found)
        MSMSEC_CAPABILITY_PROFILE_AUTH = (MSMSEC_BASE + 30),
        // Capability matching failed at profile (cipher not found)
        MSMSEC_CAPABILITY_PROFILE_CIPHER = (MSMSEC_BASE + 31),

        // Failed to queue UI request
        MSMSEC_UI_REQUEST_FAILURE = (MSMSEC_CONNECT_BASE + 1),
        // 802.1x authentication did not start within configured time 
        MSMSEC_AUTH_START_TIMEOUT = (MSMSEC_CONNECT_BASE + 2),
        // 802.1x authentication did not complete within configured time
        MSMSEC_AUTH_SUCCESS_TIMEOUT = (MSMSEC_CONNECT_BASE + 3),
        // Dynamic key exchange did not start within configured time
        MSMSEC_KEY_START_TIMEOUT = (MSMSEC_CONNECT_BASE + 4),
        // Dynamic key exchange did not succeed within configured time
        MSMSEC_KEY_SUCCESS_TIMEOUT = (MSMSEC_CONNECT_BASE + 5),
        // Message 3 of 4 way handshake has no key data (RSN/WPA)
        MSMSEC_M3_MISSING_KEY_DATA = (MSMSEC_CONNECT_BASE + 6),
        // Message 3 of 4 way handshake has no IE (RSN/WPA)
        MSMSEC_M3_MISSING_IE = (MSMSEC_CONNECT_BASE + 7),
        // Message 3 of 4 way handshake has no Group Key (RSN)
        MSMSEC_M3_MISSING_GRP_KEY = (MSMSEC_CONNECT_BASE + 8),
        // Matching security capabilities of IE in M3 failed (RSN/WPA)
        MSMSEC_PR_IE_MATCHING = (MSMSEC_CONNECT_BASE + 9),
        // Matching security capabilities of Secondary IE in M3 failed (RSN)
        MSMSEC_SEC_IE_MATCHING = (MSMSEC_CONNECT_BASE + 10),
        // Required a pairwise key but AP configured only group keys
        MSMSEC_NO_PAIRWISE_KEY = (MSMSEC_CONNECT_BASE + 11),
        // Message 1 of group key handshake has no key data (RSN/WPA)
        MSMSEC_G1_MISSING_KEY_DATA = (MSMSEC_CONNECT_BASE + 12),
        // Message 1 of group key handshake has no group key
        MSMSEC_G1_MISSING_GRP_KEY = (MSMSEC_CONNECT_BASE + 13),
        // AP reset secure bit after connection was secured
        MSMSEC_PEER_INDICATED_INSECURE = (MSMSEC_CONNECT_BASE + 14),
        // 802.1x indicated there is no authenticator but profile requires 802.1x
        MSMSEC_NO_AUTHENTICATOR = (MSMSEC_CONNECT_BASE + 15),
        // Plumbing settings to NIC failed
        MSMSEC_NIC_FAILURE = (MSMSEC_CONNECT_BASE + 16),
        // Operation was cancelled by caller
        MSMSEC_CANCELLED = (MSMSEC_CONNECT_BASE + 17),
        // Key was in incorrect format 
        MSMSEC_KEY_FORMAT = (MSMSEC_CONNECT_BASE + 18),
        // Security downgrade detected
        MSMSEC_DOWNGRADE_DETECTED = (MSMSEC_CONNECT_BASE + 19),
        // PSK mismatch suspected
        MSMSEC_PSK_MISMATCH_SUSPECTED = (MSMSEC_CONNECT_BASE + 20),
        // Forced failure because connection method was not secure
        MSMSEC_FORCED_FAILURE = (MSMSEC_CONNECT_BASE + 21),
        // ui request couldn't be queued or user pressed cancel
        MSMSEC_SECURITY_UI_FAILURE = (MSMSEC_CONNECT_BASE + 22),

        MSMSEC_MAX = MSMSEC_END
    }


    /// <summary>
    /// Indicates the state of an interface.
    /// </summary>
    /// <remarks>
    /// Corresponds to the native <c>WLAN_INTERFACE_STATE</c> type.
    /// </remarks>
    public enum WlanInterfaceState
    {
        /// <summary>
        /// The interface is not ready to operate.
        /// </summary>
        NotReady = 0,
        /// <summary>
        /// The interface is connected to a network.
        /// </summary>
        Connected = 1,
        /// <summary>
        /// The interface is the first node in an ad hoc network. No peer has connected.
        /// </summary>
        AdHocNetworkFormed = 2,
        /// <summary>
        /// The interface is disconnecting from the current network.
        /// </summary>
        Disconnecting = 3,
        /// <summary>
        /// The interface is not connected to any network.
        /// </summary>
        Disconnected = 4,
        /// <summary>
        /// The interface is attempting to associate with a network.
        /// </summary>
        Associating = 5,
        /// <summary>
        /// Auto configuration is discovering the settings for the network.
        /// </summary>
        Discovering = 6,
        /// <summary>
        /// The interface is in the process of authenticating.
        /// </summary>
        Authenticating = 7
    }


    /// <summary>
    /// Defines an 802.11 PHY and media type.
    /// </summary>
    /// <remarks>
    /// Corresponds to the native <c>DOT11_PHY_TYPE</c> type.
    /// </remarks>
    public enum Dot11PhyType : uint
    {
        /// <summary>
        /// Specifies an unknown or uninitialized PHY type.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Specifies any PHY type.
        /// </summary>
        Any = Unknown,
        /// <summary>
        /// Specifies a frequency-hopping spread-spectrum (FHSS) PHY. Bluetooth devices can use FHSS or an adaptation of FHSS.
        /// </summary>
        FHSS = 1,
        /// <summary>
        /// Specifies a direct sequence spread spectrum (DSSS) PHY.
        /// </summary>
        DSSS = 2,
        /// <summary>
        /// Specifies an infrared (IR) baseband PHY.
        /// </summary>
        IrBaseband = 3,
        /// <summary>
        /// Specifies an orthogonal frequency division multiplexing (OFDM) PHY. 802.11a devices can use OFDM.
        /// </summary>
        OFDM = 4,
        /// <summary>
        /// Specifies a high-rate DSSS (HRDSSS) PHY.
        /// </summary>
        HRDSSS = 5,
        /// <summary>
        /// Specifies an extended rate PHY (ERP). 802.11g devices can use ERP.
        /// </summary>
        ERP = 6,
        /// <summary>
        /// Specifies the start of the range that is used to define PHY types that are developed by an independent hardware vendor (IHV).
        /// </summary>
        IHV_Start = 0x80000000,
        /// <summary>
        /// Specifies the end of the range that is used to define PHY types that are developed by an independent hardware vendor (IHV).
        /// </summary>
        IHV_End = 0xffffffff
    }

    /// <summary>
    /// Defines a basic service set (BSS) network type.
    /// </summary>
    /// <remarks>
    /// Corresponds to the native <c>DOT11_BSS_TYPE</c> type.
    /// </remarks>
    public enum Dot11BssType
    {
        /// <summary>
        /// Specifies an infrastructure BSS network.
        /// </summary>
        Infrastructure = 1,
        /// <summary>
        /// Specifies an independent BSS (IBSS) network.
        /// </summary>
        Independent = 2,
        /// <summary>
        /// Specifies either infrastructure or IBSS network.
        /// </summary>
        Any = 3
    }


    /// <summary>
    /// Defines the mode of connection.
    /// </summary>
    /// <remarks>
    /// Corresponds to the native <c>WLAN_CONNECTION_MODE</c> type.
    /// </remarks>
    public enum WlanConnectionMode
    {
        /// <summary>
        /// A profile will be used to make the connection.
        /// </summary>
        Profile = 0,
        /// <summary>
        /// A temporary profile will be used to make the connection.
        /// </summary>
        TemporaryProfile,
        /// <summary>
        /// Secure discovery will be used to make the connection.
        /// </summary>
        DiscoverySecure,
        /// <summary>
        /// Unsecure discovery will be used to make the connection.
        /// </summary>
        DiscoveryUnsecure,
        /// <summary>
        /// A connection will be made automatically, generally using a persistent profile.
        /// </summary>
        Auto,
        /// <summary>
        /// Not used.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// Defines a wireless LAN authentication algorithm.
    /// </summary>
    /// <remarks>
    /// Corresponds to the native <c>DOT11_AUTH_ALGORITHM</c> type.
    /// </remarks>
    public enum Dot11AuthAlgorithm : uint
    {
        /// <summary>
        /// Specifies an IEEE 802.11 Open System authentication algorithm.
        /// </summary>
        IEEE80211_Open = 1,
        /// <summary>
        /// Specifies an 802.11 Shared Key authentication algorithm that requires the use of a pre-shared Wired Equivalent Privacy (WEP) key for the 802.11 authentication.
        /// </summary>
        IEEE80211_SharedKey = 2,
        /// <summary>
        /// Specifies a Wi-Fi Protected Access (WPA) algorithm. IEEE 802.1X port authentication is performed by the supplicant, authenticator, and authentication server. Cipher keys are dynamically derived through the authentication process.
        /// <para>This algorithm is valid only for BSS types of <see cref="Dot11BssType.Infrastructure"/>.</para>
        /// <para>When the WPA algorithm is enabled, the 802.11 station will associate only with an access point whose beacon or probe responses contain the authentication suite of type 1 (802.1X) within the WPA information element (IE).</para>
        /// </summary>
        WPA = 3,
        /// <summary>
        /// Specifies a WPA algorithm that uses preshared keys (PSK). IEEE 802.1X port authentication is performed by the supplicant and authenticator. Cipher keys are dynamically derived through a preshared key that is used on both the supplicant and authenticator.
        /// <para>This algorithm is valid only for BSS types of <see cref="Dot11BssType.Infrastructure"/>.</para>
        /// <para>When the WPA PSK algorithm is enabled, the 802.11 station will associate only with an access point whose beacon or probe responses contain the authentication suite of type 2 (preshared key) within the WPA IE.</para>
        /// </summary>
        WPA_PSK = 4,
        /// <summary>
        /// This value is not supported.
        /// </summary>
        WPA_None = 5,
        /// <summary>
        /// Specifies an 802.11i Robust Security Network Association (RSNA) algorithm. WPA2 is one such algorithm. IEEE 802.1X port authentication is performed by the supplicant, authenticator, and authentication server. Cipher keys are dynamically derived through the authentication process.
        /// <para>This algorithm is valid only for BSS types of <see cref="Dot11BssType.Infrastructure"/>.</para>
        /// <para>When the RSNA algorithm is enabled, the 802.11 station will associate only with an access point whose beacon or probe responses contain the authentication suite of type 1 (802.1X) within the RSN IE.</para>
        /// </summary>
        RSNA = 6,
        /// <summary>
        /// Specifies an 802.11i RSNA algorithm that uses PSK. IEEE 802.1X port authentication is performed by the supplicant and authenticator. Cipher keys are dynamically derived through a preshared key that is used on both the supplicant and authenticator.
        /// <para>This algorithm is valid only for BSS types of <see cref="Dot11BssType.Infrastructure"/>.</para>
        /// <para>When the RSNA PSK algorithm is enabled, the 802.11 station will associate only with an access point whose beacon or probe responses contain the authentication suite of type 2(preshared key) within the RSN IE.</para>
        /// </summary>
        RSNA_PSK = 7,
        /// <summary>
        /// Indicates the start of the range that specifies proprietary authentication algorithms that are developed by an IHV.
        /// </summary>
        /// <remarks>
        /// This enumerator is valid only when the miniport driver is operating in Extensible Station (ExtSTA) mode.
        /// </remarks>
        IHV_Start = 0x80000000,
        /// <summary>
        /// Indicates the end of the range that specifies proprietary authentication algorithms that are developed by an IHV.
        /// </summary>
        /// <remarks>
        /// This enumerator is valid only when the miniport driver is operating in Extensible Station (ExtSTA) mode.
        /// </remarks>
        IHV_End = 0xffffffff
    }

    /// <summary>
    /// Defines a cipher algorithm for data encryption and decryption.
    /// </summary>
    /// <remarks>
    /// Corresponds to the native <c>DOT11_CIPHER_ALGORITHM</c> type.
    /// </remarks>
    public enum Dot11CipherAlgorithm : uint
    {
        /// <summary>
        /// Specifies that no cipher algorithm is enabled or supported.
        /// </summary>
        None = 0x00,
        /// <summary>
        /// Specifies a Wired Equivalent Privacy (WEP) algorithm, which is the RC4-based algorithm that is specified in the 802.11-1999 standard. This enumerator specifies the WEP cipher algorithm with a 40-bit cipher key.
        /// </summary>
        WEP40 = 0x01,
        /// <summary>
        /// Specifies a Temporal Key Integrity Protocol (TKIP) algorithm, which is the RC4-based cipher suite that is based on the algorithms that are defined in the WPA specification and IEEE 802.11i-2004 standard. This cipher also uses the Michael Message Integrity Code (MIC) algorithm for forgery protection.
        /// </summary>
        TKIP = 0x02,
        /// <summary>
        /// Specifies an AES-CCMP algorithm, as specified in the IEEE 802.11i-2004 standard and RFC 3610. Advanced Encryption Standard (AES) is the encryption algorithm defined in FIPS PUB 197.
        /// </summary>
        CCMP = 0x04,
        /// <summary>
        /// Specifies a WEP cipher algorithm with a 104-bit cipher key.
        /// </summary>
        WEP104 = 0x05,
        /// <summary>
        /// Specifies a Robust Security Network (RSN) Use Group Key cipher suite. For more information about the Use Group Key cipher suite, refer to Clause 7.3.2.9.1 of the IEEE 802.11i-2004 standard.
        /// </summary>
        WPA_UseGroup = 0x100,
        /// <summary>
        /// Specifies a Wifi Protected Access (WPA) Use Group Key cipher suite. For more information about the Use Group Key cipher suite, refer to Clause 7.3.2.9.1 of the IEEE 802.11i-2004 standard.
        /// </summary>
        RSN_UseGroup = 0x100,
        /// <summary>
        /// Specifies a WEP cipher algorithm with a cipher key of any length.
        /// </summary>
        WEP = 0x101,
        /// <summary>
        /// Specifies the start of the range that is used to define proprietary cipher algorithms that are developed by an independent hardware vendor (IHV).
        /// </summary>
        IHV_Start = 0x80000000,
        /// <summary>
        /// Specifies the end of the range that is used to define proprietary cipher algorithms that are developed by an IHV.
        /// </summary>
        IHV_End = 0xffffffff
    }

    /// <summary>
    /// Flags that specifiy the miniport driver's current operation mode.
    /// </summary>
    [Flags]
    public enum Dot11OperationMode : uint
    {
        Unknown = 0x00000000,
        Station = 0x00000001,
        AP = 0x00000002,
        /// <summary>
        /// Specifies that the miniport driver supports the Extensible Station (ExtSTA) operation mode.
        /// </summary>
        ExtensibleStation = 0x00000004,
        /// <summary>
        /// Specifies that the miniport driver supports the Network Monitor (NetMon) operation mode.
        /// </summary>
        NetworkMonitor = 0x80000000
    }


    /// <summary>
    /// A set of flags that modify the behavior of the function: WlanSetProfileEapUserData
    /// 
    /// On Windows Vista and Windows Server 2008, this parameter is reserved and should be set to zero.		
    /// On Windows 7, Windows Server 2008 R2, and later, this parameter can be one of the following values.
    /// </summary>
    [Flags]
    public enum SetEapUserDataMode : uint
    {
        None = 0x00000000,
        WlanSetEaphostDataAllUsers = 0x00000001
    }

    /// <summary>
    /// Wifi status
    /// </summary>
    public enum WifiStatus
    {
        Disconnected,
        Connected
    }
}
