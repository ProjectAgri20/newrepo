using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeQ
{
    /// <summary>
    /// Creates object of type SafeQAppFactory
    /// </summary>
    public sealed class SafeQAppFactory : DeviceFactoryCore<ISafeQApp>
    {
        private static SafeQAppFactory _instance = new SafeQAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeQAppFactory" /> class.
        /// </summary>
        private SafeQAppFactory()
        {
            Add<JediOmniDevice, JediOmniSafeQApp>();
        }

        /// <summary>
        /// creates an <see cref="ISafeQApp"/> for the specified <see cref="IDevice"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="ISafeQApp" />for the specified device.</returns>
        public static ISafeQApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
