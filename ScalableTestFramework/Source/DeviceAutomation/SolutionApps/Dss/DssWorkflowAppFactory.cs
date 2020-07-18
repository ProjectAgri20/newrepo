using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Oz;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Dss
{
    /// <summary>
    /// Factory for creating <see cref="IDssWorkflowApp" /> objects.
    /// </summary>
    public sealed class DssWorkflowAppFactory : DeviceFactoryCore<IDssWorkflowApp>
    {
        private static DssWorkflowAppFactory _instance = new DssWorkflowAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="DssWorkflowAppFactory" /> class.
        /// </summary>
        private DssWorkflowAppFactory()
        {
            Add<JediOmniDevice, JediOmniDssWorkflowApp>();
            Add<JediWindjammerDevice, JediWindjammerDssWorkflowApp>();
            Add<OzWindjammerDevice, OzWindjammerDssWorkflowApp>();
        }

        /// <summary>
        /// Creates an <see cref="IDssWorkflowApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IDssWorkflowApp" /> for the specified device.</returns>
        public static IDssWorkflowApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
