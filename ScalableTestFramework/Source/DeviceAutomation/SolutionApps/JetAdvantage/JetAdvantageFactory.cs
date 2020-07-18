using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.JetAdvantage
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class JetAdvantageFactory : DeviceFactoryCore<IJetAdvantageApp>
    {
        private static JetAdvantageFactory _instance = new JetAdvantageFactory();

        private JetAdvantageFactory()
        {
            Add<JediOmniDevice, JediOmniJetAdvantageApp>();
            Add<JediWindjammerDevice, JediWindjammerJetAdvantageApp>();
        }

        /// <summary>
        /// Creates the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns></returns>
        public static IJetAdvantageApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
