using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.GeniusBytes
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeniusBytesAppFactory" /> class.
    /// </summary>
    public sealed class GeniusBytesAppFactory : DeviceFactoryCore<IGeniusBytesApp>
    {
        private static GeniusBytesAppFactory _instance = new GeniusBytesAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="GeniusBytesAppFactory" /> class.
        /// </summary>
        private GeniusBytesAppFactory()
        {
            Add<JediOmniDevice, JediOmniGeniusBytesApp>();
        }

        /// <summary>
        /// creates an <see cref="IGeniusBytesApp"/> for the specified <see cref="IDevice"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IGeniusBytesApp" />for the specified device.</returns>
        public static IGeniusBytesApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
