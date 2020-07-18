using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Equitrac
{
    /// <summary>
    /// Factory determining process in Omni or Windjammer device
    /// </summary>
    public sealed class EquitracAppFactory : DeviceFactoryCore<IEquitracApp>
    {
        private static EquitracAppFactory _instance = new EquitracAppFactory();

        private EquitracAppFactory()
        {
            Add<JediOmniDevice, JediOmniEquitracApp>();
            Add<JediWindjammerDevice, JediWindjammerEquitracApp>();
        }

        /// <summary>
        /// Creates  and returns the correct object for the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns></returns>
        public static IEquitracApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
