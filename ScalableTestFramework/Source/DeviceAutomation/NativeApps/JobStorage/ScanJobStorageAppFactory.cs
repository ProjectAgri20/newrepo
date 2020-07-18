using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.JobStorage
{
    /// <summary>
    /// Factory for creating <see cref="IJobStorageScanApp" /> objects.
    /// </summary>
    public sealed class ScanJobStorageAppFactory : DeviceFactoryCore<IJobStorageScanApp>
    {
        private static readonly ScanJobStorageAppFactory Instance = new ScanJobStorageAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanJobStorageAppFactory" /> class.
        /// </summary>
        private ScanJobStorageAppFactory()
        {
            Add<JediOmniDevice, JediOmniJobStorageScanApp>();
            Add<JediWindjammerDevice, JediWindjammerJobStorageScanApp>();
        }

        /// <summary>
        /// Creates an <see cref="IJobStorageScanApp" /> for the specified <see cref="IDevice"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IJobStorageScanApp" /> for the specified device.</returns>
        public static IJobStorageScanApp Create(IDevice device) => Instance.FactoryCreate(device);
    }
}