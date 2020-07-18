using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.HpacScan
{
    /// <summary>
    /// Creates object of type HpacScanAppFactory
    /// </summary>
    public sealed class HpacScanAppFactory : DeviceFactoryCore<IHpacScanApp>
    {
        private static HpacScanAppFactory _instance = new HpacScanAppFactory();

        private HpacScanAppFactory()
        {
            Add<JediOmniDevice, JediOmniHpacScanApp>();
        }

        /// <summary>
        /// creates an <see cref="IHpacScanApp"/> for the specified <see cref="IDevice"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IHpacScanApp" />for the specified device.</returns>
        public static IHpacScanApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
