using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpac
{
    /// <summary>
    /// Creates object of type HpacAppFactory
    /// </summary>
    public sealed class HpacAppFactory : DeviceFactoryCore<IHpacApp>
    {
        private static HpacAppFactory _instance = new HpacAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="HpacAppFactory" /> class.
        /// </summary>
        private HpacAppFactory()
        {
            Add<JediWindjammerDevice, JediWindjammerHpacApp>();
            Add<JediOmniDevice, JediOmniHpacApp>();
            Add<SiriusUIv3Device, SiriusUIv3HpacApp>();
        }

        /// <summary>
        /// Creates an <see cref="IHpacApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IHpacApp" /> for the specified device.</returns>
        public static IHpacApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
