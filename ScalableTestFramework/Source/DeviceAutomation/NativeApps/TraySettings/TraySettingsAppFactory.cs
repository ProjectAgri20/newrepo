using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.TraySettings
{
    /// <summary>
    /// Factory for creating <see cref="ITraySettingsApp" /> objects.
    /// </summary>
    public sealed  class TraySettingsAppFactory : DeviceFactoryCore<ITraySettingsApp>
    {
        private static TraySettingsAppFactory _instance = new TraySettingsAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="TraySettingsAppFactory" /> class.
        /// </summary>
        private TraySettingsAppFactory()
        {
            Add<JediOmniDevice, JediOmniTraySettingsApp>();
        }
        /// <summary>
        /// Creates an <see cref="ITraySettingsApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="ITraySettingsApp" /> for the specified device.</returns>
        public static ITraySettingsApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
