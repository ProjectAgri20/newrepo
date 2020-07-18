using System.ComponentModel;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Oz;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr
{
    /// <summary>
    /// HPCR Application Factory class
    /// </summary>
    public sealed class HpcrAppFactory : DeviceFactoryCore<IHpcrApp>
    {
        private static HpcrAppFactory _instance = new HpcrAppFactory();
        private HpcrAppFactory()
        {
            Add<JediOmniDevice, JediOmniHpcrApp>();
            Add<JediWindjammerDevice, JediWindJammerHpcrApp>();
            Add<OzWindjammerDevice, OzWindJammerHpcrApp>();
            Add<SiriusUIv2Device, SiriusUIv2HpcrApp>();
            Add<SiriusUIv3Device, SiriusUIv3HpcrApp>();
        }

        /// <summary>
        /// Create a HPCR instance for the given device
        /// </summary>
        /// <param name="device"></param>
        /// <param name="appButtonTitle"></param>
        /// <param name="scanDestination"></param>
        /// <param name="scanDistribution"></param>
        /// <param name="documentName"></param>
        /// <param name="imagePreview"></param>
        /// <returns></returns>
        public static IHpcrApp Create(IDevice device, string appButtonTitle, string scanDestination, string scanDistribution, string documentName, bool imagePreview)
        {
            return _instance.FactoryCreate(device, appButtonTitle, scanDestination, scanDistribution, documentName, imagePreview);
        }
    }

    /// <summary>
    /// HPCR Application types
    /// </summary>
    public enum HpcrAppTypes
    {
        /// <summary>
        /// Scan to Me
        /// </summary>
        [Description("Scan To Me")]
        ScanToMe,

        /// <summary>
        /// Scan To My Files 
        /// </summary>
        [Description("Scan To My Files")]
        ScanToMyFiles,

        /// <summary>
        /// Scan To Folder
        /// </summary>
        [Description("Scan To Folder")]
        ScanToFolder,

        /// <summary>
        /// Personal Distributions
        /// </summary>
        [Description("Personal Distributions")]
        PersonalDistributions,

        /// <summary>
        /// Public Distributions
        /// </summary>
        [Description("Public Distributions")]
        PublicDistributions,

        /// <summary>
        /// Routing Sheet
        /// </summary>
        [Description("Routing Sheet")]
        RoutingSheet
    }
}
