using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Blueprint
{
    /// <summary>
    /// Creates object of type BlueprintAppFactory
    /// </summary>
    public sealed class BlueprintAppFactory : DeviceFactoryCore<IBlueprintApp>
    {

        private static BlueprintAppFactory _instance = new BlueprintAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="BlueprintAppFactory"/>
        /// </summary>
        private BlueprintAppFactory()
        {
            Add<JediWindjammerDevice, JediWindjammerBlueprintApp>();
            Add<JediOmniDevice, JediOmniBlueprintApp>();
        }

        /// <summary>
        /// Creates an <see cref="IBlueprintApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IBlueprintApp" /> for the specified device.</returns>
        public static IBlueprintApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
