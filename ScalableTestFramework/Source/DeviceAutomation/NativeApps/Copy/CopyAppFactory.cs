using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Phoenix;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Copy
{
    /// <summary>
    /// Factory for creating <see cref="ICopyApp" /> objects.
    /// </summary>
    public sealed class CopyAppFactory : DeviceFactoryCore<ICopyApp>
    {
        private static CopyAppFactory _instance = new CopyAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAppFactory" /> class.
        /// </summary>
        private CopyAppFactory()
        {
            Add<JediWindjammerDevice, JediWindjammerCopyApp>();
            Add<JediOmniDevice, JediOmniCopyApp>();
            Add<PhoenixNovaDevice, PhoenixNovaCopyApp>();
        }

        /// <summary>
        /// Creates an <see cref="ICopyApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="ICopyApp" /> for the specified device.</returns>
        public static ICopyApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}