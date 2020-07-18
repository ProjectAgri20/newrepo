using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.PaperCut
{
    /// <summary>
    /// Creates object of type PaperCutAppFactory
    /// </summary>
    public sealed class PaperCutAppFactory : DeviceFactoryCore<IPaperCutApp>
    {
        private static PaperCutAppFactory _instance = new PaperCutAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="PaperCutAppFactory" /> class.
        /// </summary>
        private PaperCutAppFactory()
        {
            Add<JediWindjammerDevice, JediWindjammerPaperCutApp>();
            Add<JediOmniDevice, JediOmniPaperCutApp>();
        }

        /// <summary>
        /// Creates an <see cref="IPaperCutApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IPaperCutApp" /> for the specified device.</returns>
        public static IPaperCutApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
