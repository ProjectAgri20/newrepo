using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.UdocxScan
{
    /// <summary>
    /// Creates object of type UdocxAppFactory
    /// </summary>
    public sealed class UdocxScanAppFactory : DeviceFactoryCore<IUdocxScanApp>
    {
        private static UdocxScanAppFactory _instance = new UdocxScanAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniUdocxScanApp" /> class.
        /// </summary>
        private UdocxScanAppFactory()
        {
            Add<JediOmniDevice, JediOmniUdocxScanApp>();
        }

        /// <summary>
        /// creates an <see cref="IUdocxScanApp"/> for the specified <see cref="IDevice"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IUdocxScanApp" />for the specified device.</returns>
        public static IUdocxScanApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
