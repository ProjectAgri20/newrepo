using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Oz;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Email
{
    /// <summary>
    /// Factory for creating <see cref="IEmailApp" /> objects.
    /// </summary>
    public sealed class EmailAppFactory : DeviceFactoryCore<IEmailApp>
    {
        private static EmailAppFactory _instance = new EmailAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAppFactory" /> class.
        /// </summary>
        private EmailAppFactory()
        {
            Add<JediOmniDevice, JediOmniEmailApp>();
            Add<JediWindjammerDevice, JediWindjammerEmailApp>();
            Add<OzWindjammerDevice, OzWindjammerEmailApp>();
            Add<PhoenixMagicFrameDevice, PhoenixMagicFrameEmailApp>();
            Add<PhoenixNovaDevice, PhoenixNovaEmailApp>();
            Add<SiriusUIv2Device, SiriusUIv2EmailApp>();
            Add<SiriusUIv3Device, SiriusUIv3EmailApp>();
        }

        /// <summary>
        /// Creates an <see cref="IEmailApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IEmailApp" /> for the specified device.</returns>
        public static IEmailApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
