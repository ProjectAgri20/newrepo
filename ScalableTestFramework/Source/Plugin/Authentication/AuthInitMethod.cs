using System.Collections.Generic;
using System.ComponentModel;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Authentication
{
    // Smart Scan is method of SafeCom but is currently not used.
    public class AuthInitMethod
    {
        public static string BadgeBox { get; } = InitiationMethod.Badge.GetDescription();
        public static string Copy { get; } = InitiationMethod.Copy.GetDescription();
        public static string Email { get; } = InitiationMethod.Email.GetDescription();
        public static string Equitrac { get; } = InitiationMethod.Equitrac.GetDescription();
        public static string Fax { get; } = InitiationMethod.Fax.GetDescription();
        public static string NetworkFolder { get; } = InitiationMethod.NetworkFolder.GetDescription();
        public static string HpacSecurePullPrint { get; } = InitiationMethod.HPAC.GetDescription();
        public static string HpecFollowYouPrinting { get; } = InitiationMethod.HPEC.GetDescription();
        public static string HpcrPersonalDistributions { get; } = InitiationMethod.HPCR_PersonalDistributions.GetDescription();
        public static string HpcrPublicDistributions { get; } = InitiationMethod.HPCR_PuplicDistributions.GetDescription();
        public static string HpcrRoutingSheet { get; } = InitiationMethod.HPCR_RoutingSheet.GetDescription();
        public static string HpcrScanToMe { get; } = InitiationMethod.HPCR_ScanToMe.GetDescription();
        public static string HpcrScanToMyFiles { get; } = InitiationMethod.HPCR_ScanToMyFiles.GetDescription();
        public static string HpRoam { get; } = InitiationMethod.HPRoam.GetDescription();    
        public static string SafeCom { get; } = InitiationMethod.SafeCom.GetDescription();
        public static string SignIn { get; } = InitiationMethod.SignIn.GetDescription();
        public static string Windows { get; } = InitiationMethod.Windows.GetDescription();
        public static string WorkFlow { get; } = InitiationMethod.WorkFlow.GetDescription();

        private List<string> _initiationMethods = new List<string>();

        public IEnumerable<string> InitiationMethodValues => _initiationMethods;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthInitMethod"/> class.
        /// </summary>
        public AuthInitMethod()
        {
            _initiationMethods.Add(BadgeBox);
            _initiationMethods.Add(Copy);
            _initiationMethods.Add(Email);
            _initiationMethods.Add(Equitrac);
            _initiationMethods.Add(Fax);
            _initiationMethods.Add(NetworkFolder);
            _initiationMethods.Add(HpacSecurePullPrint);
            _initiationMethods.Add(HpcrPersonalDistributions);
            _initiationMethods.Add(HpcrPublicDistributions);
            _initiationMethods.Add(HpcrRoutingSheet);
            _initiationMethods.Add(HpcrScanToMe);
            _initiationMethods.Add(HpcrScanToMyFiles);
            _initiationMethods.Add(HpecFollowYouPrinting);
            _initiationMethods.Add(HpRoam);
            _initiationMethods.Add(SafeCom);
            _initiationMethods.Add(SignIn);
            _initiationMethods.Add(Windows);
            _initiationMethods.Add(WorkFlow);
        }
        /// <summary>
        /// Gets the initiation method by the string value.
        /// </summary>
        /// <param name="initiationMethod">The initiation method.</param>
        /// <returns>InitiationMethods</returns>
        public static InitiationMethod GetInitiationMethod(string initiationMethod)
        {
            InitiationMethod im = EnumUtil.GetByDescription<InitiationMethod>(initiationMethod);
            return im;
        }
    }

    /// <summary>
    /// Initiation methods and title names
    /// </summary>
    public enum InitiationMethod
    {
        [Description("Badge")]
        Badge,
        [Description("Copy")]
        Copy,
        [Description("E-mail")]
        Email,
        [Description("Fax")]
        Fax,
        [Description("Save to Network Folder")]
        NetworkFolder,
        [Description("HP AC Secure Pull Print")]
        HPAC,
        [Description("My workflow (FutureSmart)")]
        HPEC,
        [Description("HP Roam")]
        HPRoam,
        [Description("HP Roam Pin")]
        HPRoamPin,
        [Description("Follow-You Printing")]
        Equitrac,
        [Description("Pull Print")]
        SafeCom,
        [Description("Personal Distributions")]
        HPCR_PersonalDistributions,
        [Description("Public Distributions")]
        HPCR_PuplicDistributions,
        [Description("Routing Sheet")]
        HPCR_RoutingSheet,
        [Description("Scan To Me")]
        HPCR_ScanToMe,
        [Description("Scan To My Files")]
        HPCR_ScanToMyFiles,
        [Description("Sign In")]
        SignIn,
        [Description("Windows")]
        Windows,
        [Description("Workflow")]
        WorkFlow,
        [Description("Do Not Sign In")]
        DoNotSignIn
    }
}
