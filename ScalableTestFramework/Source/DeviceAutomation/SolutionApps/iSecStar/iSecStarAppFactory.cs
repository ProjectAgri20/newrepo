using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.iSecStar
{
    /// <summary>
    /// Creates object of type iSecStarAppFactory
    /// </summary>
    public sealed class iSecStarAppFactory : DeviceFactoryCore<IiSecStarApp>
    {
        private static iSecStarAppFactory _instance = new iSecStarAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="iSecStarAppFactory" /> class.
        /// </summary>
        private iSecStarAppFactory()
        {
            Add<JediOmniDevice, JediOmniiSecStarApp>();
        }

        /// <summary>
        /// Creates an <see cref="IiSecStarApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IiSecStarApp" /> for the specified device.</returns>
        public static IiSecStarApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
