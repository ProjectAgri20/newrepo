using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Oz;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.DevicePreparation
{
    /// <summary>
    /// Factory for creating <see cref="IDevicePreparationManager" /> objects.
    /// </summary>
    public sealed class DevicePreparationManagerFactory : DeviceFactoryCore<IDevicePreparationManager>
    {
        private static DevicePreparationManagerFactory _instance = new DevicePreparationManagerFactory();

        private DevicePreparationManagerFactory()
        {
            Add<JediOmniDevice, JediOmniPreparationManager>();
            Add<JediWindjammerDevice, JediWindjammerPreparationManager>();
            Add<OzWindjammerDevice, OzWindjammerPreparationManager>();
            Add<PhoenixMagicFrameDevice, PhoenixMagicFramePreparationManager>();
            Add<PhoenixNovaDevice, PhoenixNovaPreparationManager>();
            Add<SiriusUIv2Device, SiriusUIv2PreparationManager>();
            Add<SiriusUIv3Device, SiriusUIv3PreparationManager>();
        }

        /// <summary>
        /// Creates an <see cref="IDevicePreparationManager" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IDevicePreparationManager" /> for the specified device.</returns>
        public static IDevicePreparationManager Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
