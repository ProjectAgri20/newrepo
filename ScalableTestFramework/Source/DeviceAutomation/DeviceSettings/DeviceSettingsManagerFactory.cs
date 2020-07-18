using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Oz;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.DeviceSettings
{
    /// <summary>
    /// Factory for creating <see cref="IDeviceSettingsManager" /> objects.
    /// </summary>
    public sealed class DeviceSettingsManagerFactory : DeviceFactoryCore<IDeviceSettingsManager>
    {
        private static DeviceSettingsManagerFactory _instance = new DeviceSettingsManagerFactory();

        private DeviceSettingsManagerFactory()
        {
            Add<JediDevice, JediDeviceSettingsManager>();
            Add<OzDevice, OzDeviceSettingsManager>();
            Add<PhoenixDevice, PhoenixDeviceSettingsManager>();
            Add<SiriusDevice, SiriusDeviceSettingsManager>();
        }

        /// <summary>
        /// Creates an <see cref="IDeviceSettingsManager" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IDeviceSettingsManager" /> for the specified device.</returns>
        public static IDeviceSettingsManager Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
