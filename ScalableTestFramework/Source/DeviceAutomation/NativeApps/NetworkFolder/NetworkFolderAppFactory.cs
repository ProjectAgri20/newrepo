using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Oz;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder
{
    /// <summary>
    /// Factory for creating <see cref="INetworkFolderApp" /> objects.
    /// </summary>
    public sealed class NetworkFolderAppFactory : DeviceFactoryCore<INetworkFolderApp>
    {
        private static NetworkFolderAppFactory _instance = new NetworkFolderAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkFolderAppFactory" /> class.
        /// </summary>
        private NetworkFolderAppFactory()
        {
            Add<JediOmniDevice, JediOmniNetworkFolderApp>();
            Add<JediWindjammerDevice, JediWindjammerNetworkFolderApp>();
            Add<OzWindjammerDevice, OzWindjammerNetworkFolderApp>();
            Add<PhoenixMagicFrameDevice, PhoenixMagicFrameNetworkFolderApp>();
            Add<PhoenixNovaDevice, PhoenixNovaNetworkFolderApp>();
            Add<SiriusUIv2Device, SiriusUIv2NetworkFolderApp>();
            Add<SiriusUIv3Device, SiriusUIv3NetworkFolderApp>();
        }

        /// <summary>
        /// Creates an <see cref="INetworkFolderApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="INetworkFolderApp" /> for the specified device.</returns>
        public static INetworkFolderApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
