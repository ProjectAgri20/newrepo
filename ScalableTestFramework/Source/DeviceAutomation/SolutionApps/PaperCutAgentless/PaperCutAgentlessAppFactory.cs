using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.PaperCutAgentless
{
    /// <summary>
    /// Creates object of type PaperCutAppFactory
    /// </summary>
    public sealed class PaperCutAgentlessAppFactory : DeviceFactoryCore<IPaperCutAgentlessApp>
    {
        private static PaperCutAgentlessAppFactory _instance = new PaperCutAgentlessAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="PaperCutAgentlessAppFactory" /> class.
        /// </summary>
        private PaperCutAgentlessAppFactory()
        {            
            Add<JediOmniDevice, JediOmniPaperCutAgentlessApp>();
        }

        /// <summary>
        /// Creates an <see cref="IPaperCutAgentlessApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IPaperCutAgentlessApp" /> for the specified device.</returns>
        public static IPaperCutAgentlessApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
