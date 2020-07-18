using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpec
{
    /// <summary>
    /// Factory object for Hpec
    /// </summary>
    public sealed class HpecFactory : DeviceFactoryCore<IHpecApp>
    {
        private static HpecFactory _instance = new HpecFactory();

        private HpecFactory()
        {
            Add<JediOmniDevice, JediOmniHpecApp>();
            Add<JediWindjammerDevice, JediWindJammerHpecApp>();
        }

        /// <summary>
        /// Creates a Hpec interface for the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>IHpecApp object</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="DeviceFactoryCoreException"></exception>
        public static IHpecApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
