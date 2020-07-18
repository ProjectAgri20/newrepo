using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeCom
{
    /// <summary>
    /// SafeCom Factory
    /// </summary>
    public sealed class SafeComAppFactory : DeviceFactoryCore<ISafeComApp>
    {
        private static SafeComAppFactory _instance = new SafeComAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeComAppFactory" /> class.
        /// </summary>
        private SafeComAppFactory()
        {
            Add<JediWindjammerDevice, JediWindjammerSafeComApp>();
            Add<JediOmniDevice, JediOmniSafeComApp>();
            Add<SiriusUIv3Device, SiriusUIv3SafeComApp>();
        }

        /// <summary>
        /// Creates an <see cref="ISafeComApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="ISafeComApp" /> for the specified device.</returns>
        public static ISafeComApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
