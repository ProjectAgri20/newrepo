using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.MyQ
{
    /// <summary>
    /// Creates object of type MyQFactory
    /// </summary>
    public sealed class MyQAppFactory : DeviceFactoryCore<IMyQApp>
    {
        private static MyQAppFactory _instance = new MyQAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="MyQAppFactory" /> class.
        /// </summary>
        private MyQAppFactory()
        {
            Add<JediOmniDevice, JediOmniMyQApp>();
        }

        /// <summary>
        /// Creates an <see cref="IMyQApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IMyQApp" /> for the specified device.</returns>
        public static IMyQApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
