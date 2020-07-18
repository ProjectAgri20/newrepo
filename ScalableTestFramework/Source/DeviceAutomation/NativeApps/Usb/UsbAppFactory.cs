using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.USB
{
    /// <summary>
    /// Factory for creating <see cref="IUsbApp" /> objects.
    /// </summary>
    public sealed class UsbAppFactory : DeviceFactoryCore<IUsbApp>
    {
        private static UsbAppFactory _instance = new UsbAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="UsbAppFactory" /> class.
        /// </summary>
        private UsbAppFactory()
        {
            Add<JediOmniDevice, JediOmniUsbApp>();
            Add<JediWindjammerDevice, JediWindjammerUsbApp>();
        }

        /// <summary>
        /// Creates an <see cref="IUsbApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IUsbApp" /> for the specified device.</returns>
        public static IUsbApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
