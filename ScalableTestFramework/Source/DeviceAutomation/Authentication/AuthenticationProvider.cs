using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation.Authentication
{
    /// <summary>
    /// Enumeration for AuthenticationProvider - type of authentication utilized by STF plugins
    /// </summary>
    public enum AuthenticationProvider
    {
        /// <summary>
        /// Automatically detect type of authentication
        /// </summary>
        [Description("Auto-detect")]
        Auto,

        /// <summary>
        /// AutoStore Authentication
        /// </summary>
        [Description("AutoStore")]
        AutoStore,

        /// <summary>
        /// Blueprint Authentication
        /// </summary>
        [Description("Blueprint")]
        Blueprint,

        /// <summary>
        /// This is only used internally to create the card authenticator via factory
        /// Proximity card Authentication
        /// </summary>
        [Description("Proximity Card")]
        Card,

        /// <summary>
        /// Celiveo Authentication
        /// </summary>
        [Description("Celiveo Authentication")]
        Celiveo,

        /// <summary>
        /// DSS Authentication
        /// </summary>
        [Description("DSS")]
        DSS,

        /// <summary>
        /// Equitrac Authentication
        /// </summary>
        [Description("Equitrac")]
        Equitrac,

        /// <summary>
        /// The equitrac windows - Server Side
        /// </summary>
        [Description("Equitrac-Windows")]
        EquitracWindows,

        /// <summary>
        /// Genius Bytes Guest.
        /// </summary>
        [Description("Genius Bytes Guest Login")]
        GeniusBytesGuest,

        /// <summary>
        /// Genius Bytes Manual (User-name and Password).
        /// </summary>
        [Description("Genius Bytes Manual Login")]
        GeniusBytesManual,

        /// <summary>
        /// Genius Bytes PIN.
        /// </summary>
        [Description("Genius Bytes PIN Login")]
        GeniusBytesPin,

        /// <summary>
        /// MyQ
        /// </summary>
        [Description("MyQ")]
        MyQ,

        /// <summary>
        /// The Hpac DRA Authentication
        /// </summary>
        [Description("HPAC-DRA")]
        HpacDra,

        /// <summary>
        /// Hpac IRM Authentication
        /// </summary>
        [Description("HPAC-IRM")]
        HpacIrm,

        /// <summary>
        /// HPAC Agentless Authentication
        /// </summary>
        [Description("HPAC-Agentless")]
        HpacAgentLess,

        /// <summary>
        /// The HPAC PIC Authentication
        /// </summary>
        [Description("HPAC-PIC")]
        HpacPic,

        /// <summary>
        /// HPAC windows - Server Side
        /// </summary>
        [Description("HPAC-Windows")]
        HpacWindows,

        /// <summary>
        /// HpacScan
        /// </summary>
        [Description("HpacScan")]
        HpacScan,

        /// <summary>
        /// HP Id
        /// </summary>
        [Description("HPId")]
        HpId,

        /// <summary>
        /// HP Roam provides the authentication
        /// </summary>
        [Description("Embedded Badge Authentication")]
        HpRoamPin,

        /// <summary>
        /// ISec Star User Authentication
        /// </summary>
        [Description("User Authentication")]
        ISecStar,

        /// <summary>
        /// Windows Authentication
        /// </summary>
        [Description("LDAP")]
        LDAP,

        /// <summary>
        /// Local device user Authentication
        /// </summary>
        [Description("Local Device")]
        LocalDevice,

        /// <summary>
        /// PaperCut Authentication
        /// </summary>
        [Description("PaperCut")]
        PaperCut,

        /// <summary>
        /// PaperCutAgentless Authentication
        /// </summary>
        [Description("PaperCut MF")]
        PaperCutAgentless,

        /// <summary>
        /// Safe COM Authentication
        /// </summary>
        [Description("SafeCom")]
        SafeCom,

        /// <summary>
        /// Safe COM Authentication
        /// </summary>
        [Description("KOFAX")]
        SafeComUC,

        /// <summary>
        /// YSoft SafeQ Authentication
        /// </summary>
        [Description("YSoft SafeQ")]
        SafeQ,

        /// <summary>
        /// Do not perform Authentication
        /// </summary>
        [Description("None")]
        Skip,

        /// <summary>
        /// Udocx Scan Authentication
        /// </summary>
        [Description("Udocx Scan")]
        UdocxScan,

        /// <summary>
        /// Windows Authentication
        /// </summary>
        [Description("Windows")]
        Windows
    }

    /// <summary>
    /// Container class to help map Providers with their corresponding Get/Set text.
    /// </summary>
    public class ProviderMapItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderMapItem"/> class.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        /// <param name="setText">The set text.</param>
        public ProviderMapItem(string displayText, string setText)
        {
            DisplayText = displayText;
            SetText = setText;
        }

        /// <summary>
        /// Gets the text that is displayed for a provider.
        /// </summary>
        /// <value>The display text.</value>
        public string DisplayText { get; }

        /// <summary>
        /// Gets the text that is used to select a provider.
        /// </summary>
        /// <value>The select text.</value>
        public string SetText { get; }
    }

    /// <summary>
    /// AuthenticationMode Enum -- This is going to be deprecated. Use AuthenticationInitMethod instead
    /// </summary>
    public enum AuthenticationMode
    {
        /// <summary>
        /// Authenticate by using the "Sign-In" button.
        /// </summary>
        [Description("Eager")]
        Eager,

        /// <summary>
        /// Authenticate by using the app button and waiting for the device to prompt for credentials.
        /// </summary>
        [Description("Lazy")]
        Lazy
    }

    /// <summary>
    /// Describes how authentication is initialized on the device
    /// </summary>
    public enum AuthenticationInitMethod
    {
        /// <summary>
        /// Press the sign in button
        /// </summary>
        [Description("Sign In")]
        SignInButton,

        /// <summary>
        /// Press one of the device's application / solution buttons
        /// </summary>
        [Description("Application")]
        ApplicationButton,

        /// <summary>
        /// Use a badge
        /// </summary>
        [Description("Badge")]
        Badge,

        /// <summary>
        /// Do Not Sign In
        /// </summary>
        [Description("Do Not Sign In")]
        DoNotSignIn
    }

    /// <summary>
    /// These are the various ways to initialize the authentication process. Primary are starting with the 'Sign In' button,
    /// using a badge, or pressing one of the solution / application buttons installed on the device.
    /// </summary>
    public enum InitializationMethod
    {
        /// <summary>
        /// The sign in Button is used to initialize the authentication process
        /// </summary>
        [Description("Sign In")]
        SignIn,

        /// <summary>
        /// A badge/card is used to initialize the authentication process
        /// </summary>
        [Description("Badge")]
        Badge,

        /// <summary>
        /// The copy application button is pressed
        /// </summary>
        [Description("Copy")]
        Copy,
        /// <summary>
        /// The email application button is pressed
        /// </summary>
        [Description("E-mail")]
        Email,
        /// <summary>
        /// The fax application button is pressed
        /// </summary>
        [Description("Fax")]
        Fax,
        /// <summary>
        /// The network folder application button is pressed
        /// </summary>
        [Description("Save to Network Folder")]
        NetworkFolder,
        /// <summary>
        /// The HP AC solution button is pressed
        /// </summary>
        [Description("HP AC Secure Pull Print")]
        HPAC,
        /// <summary>
        /// The HPEC  solution button is pressed
        /// </summary>
        [Description("My workflow (FutureSmart)")]
        HPEC,
        /// <summary>
        /// The HP Roam solution button is pressed
        /// </summary>
        [Description("HP Roam")]
        HPRoam,
        /// <summary>
        /// The Equitrac solution button is pressed
        /// </summary>
        [Description("Follow-You Printing")]
        Equitrac,
        /// <summary>
        /// The SafeCom solution button is pressed
        /// </summary>
        [Description("Pull Print")]
        SafeCom,
        /// <summary>
        /// The HPCR personal distributions  solution button is pressed
        /// </summary>
        [Description("Personal Distributions")]
        HPCR_PersonalDistributions,
        /// <summary>
        /// The HPCR public distributions solution button is pressed
        /// </summary>
        [Description("Public Distributions")]
        HPCR_PublicDistributions,
        /// <summary>
        /// The HPCR routing sheet solution button is pressed
        /// </summary>
        [Description("Routing Sheet")]
        HPCR_RoutingSheet,
        /// <summary>
        /// The HPCR scan to me  solution button is pressed
        /// </summary>
        [Description("Scan To Me")]
        HPCR_ScanToMe,
        /// <summary>
        /// The HPCR scan to my files solution button is pressed
        /// </summary>
        [Description("Scan To My Files")]
        HPCR_ScanToMyFiles,
        /// <summary>
        /// The work flow application button is pressed
        /// </summary>
        [Description("Workflow")]
        WorkFlow,
        /// <summary>
        /// Do not Sign In
        /// </summary>
        [Description("Do Not Sign In")]
        DoNotSignIn
    }

    /// <summary>
    /// The authentication methods used by STF/STB plug-ins
    /// </summary>
    public enum AuthenticationMethod
    {
        /// <summary>
        /// Automatically detect type of authentication Method
        /// </summary>
        [Description("Auto-detect")]
        Auto,

        /// <summary>
        /// The user name password
        /// </summary>
        [Description("UserName+PWD")]
        UserNamePWD,

        /// <summary>
        /// The pin
        /// </summary>
        [Description("Pin")]
        Pin,

        /// <summary>
        /// The swipe
        /// </summary>
        [Description("Badge")]
        Badge,

        /// <summary>
        /// The swipe pin
        /// </summary>
        [Description("Badge+Pin")]
        BadgePin
    }
}
