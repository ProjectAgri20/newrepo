using System.ComponentModel;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.AutoStore
{
    /// <summary>
    /// Factory for AutoStore
    /// </summary>
    public sealed class AutoStoreAppFactory: DeviceFactoryCore<IAutoStoreApp>
    {
        private static AutoStoreAppFactory _instance = new AutoStoreAppFactory();
        private AutoStoreAppFactory()
        {
            Add<JediOmniDevice, JediOmniAutoStoreApp>();
            Add<JediWindjammerDevice, JediWindjammerAutoStoreApp>();
        }
        /// <summary>
        /// Creates the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="appButtonTitle">The application button title.</param>
        /// <param name="documentName">Name of the document.</param>
        /// <returns></returns>
        public static IAutoStoreApp Create(IDevice device, string appButtonTitle, string documentName)
        {
            return _instance.FactoryCreate(device, appButtonTitle, documentName);
        }
    }
    /// <summary>
    /// AutoStore Application button names
    /// </summary>
    public enum AutoStoreAppTypes
    {
        /// <summary>
        /// The send to AutoStore email
        /// </summary>
        [Description("AutoStore Email")]
        SendToAutoStoreEmail,

        /// <summary>
        /// The send to AutoStore folder
        /// </summary>
        [Description("AutoStore Folder")]
        SendToAutoStoreFolder
    }
}
