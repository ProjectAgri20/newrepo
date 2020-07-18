using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.SolutionApps.HpRoam;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.HpRoam
{
    /// <summary>
    /// Creates object of type HpRoamAppFactory
    /// </summary>
    public sealed class HpRoamAppFactory : DeviceFactoryCore<IHpRoamApp>
    {

        private static HpRoamAppFactory _instance = new HpRoamAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="HpRoamAppFactory"/>
        /// </summary>
        private HpRoamAppFactory()
        {
            Add<JediOmniDevice, JediOmniHpRoamApp>();
        }

        /// <summary>
        /// Creates an <see cref="IHpRoamApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IHpRoamApp" /> for the specified device.</returns>
        public static IHpRoamApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
