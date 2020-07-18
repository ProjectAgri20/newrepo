using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.JobStorage
{
    /// <summary>
    /// Factory for creating <see cref="IJobStoragePrintApp" /> objects.
    /// </summary>
    public sealed class JobStoragePrintAppFactory : DeviceFactoryCore<IJobStoragePrintApp>
    {
        private static JobStoragePrintAppFactory _instance = new JobStoragePrintAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="JobStoragePrintAppFactory" /> class.
        /// </summary>
        private JobStoragePrintAppFactory()
        {
            Add<JediOmniDevice, JediOmniJobStoragePrintApp>();
            Add<JediWindjammerDevice, JediWindjammerJobStoragePrintApp>();
        }

        /// <summary>
        /// Creates an <see cref="IJobStoragePrintApp" /> for the specified <see cref="IDevice"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IJobStoragePrintApp" /> for the specified device.</returns>
        public static IJobStoragePrintApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}