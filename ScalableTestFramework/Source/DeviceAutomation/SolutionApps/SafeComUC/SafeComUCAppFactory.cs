using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeComUC
{
    /// <summary>
    /// SafeCom Factory
    /// </summary>
    public sealed class SafeComUCAppFactory : DeviceFactoryCore<ISafeComUCApp>
    {
        private static SafeComUCAppFactory _instance = new SafeComUCAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeComUCAppFactory" /> class.
        /// </summary>
        private SafeComUCAppFactory()
        {            
            Add<JediOmniDevice, JediOmniSafeComUCApp>();            
        }

        /// <summary>
        /// Creates an <see cref="ISafeComUCApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="ISafeComUCApp" /> for the specified device.</returns>
        public static ISafeComUCApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
