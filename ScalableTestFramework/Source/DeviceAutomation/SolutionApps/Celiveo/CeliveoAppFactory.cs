using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Celiveo
{
    /// <summary>
    /// Creates object of type CeliveoAppFactory
    /// </summary>
    public sealed class CeliveoAppFactory : DeviceFactoryCore<ICeliveoApp>
    {
        private static CeliveoAppFactory _instance = new CeliveoAppFactory();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CeliveoAppFactory" /> class.
        /// </summary>
        private CeliveoAppFactory()
        {
            Add<JediOmniDevice, JediOmniCeliveoApp>();
        }

        /// <summary>
        /// creates an <see cref="ICeliveoApp"/> for the specified <see cref="IDevice"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="ICeliveoApp" />for the specified device.</returns>
        public static ICeliveoApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
