using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Oz;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Fax
{
    /// <summary>
    /// Factory for creating <see cref="IFaxApp" /> objects.
    /// </summary>
    public sealed class FaxAppFactory : DeviceFactoryCore<IFaxApp>
    {
        private static FaxAppFactory _instance = new FaxAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="FaxAppFactory" /> class.
        /// </summary>
        private FaxAppFactory()
        {
            Add<JediWindjammerDevice, JediWindjammerFaxApp>();
            Add<JediOmniDevice, JediOmniFaxApp>();
            Add<OzWindjammerDevice, OzWindjammerFaxApp>();
            Add<PhoenixDevice, PhoenixNovaFaxApp>();
            Add<SiriusUIv3Device, SiriusUIv3FaxApp>();
        }

        /// <summary>
        /// Creates an <see cref="IFaxApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IFaxApp" /> for the specified device.</returns>
        public static IFaxApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
