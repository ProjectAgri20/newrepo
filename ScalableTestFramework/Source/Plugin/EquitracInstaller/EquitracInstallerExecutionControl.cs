using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.EquitracInstaller
{
    /// <summary>
    /// A class that implements the execution portion of the plug-in.
    /// </summary>
    /// <remarks>
    /// This class implements the <see cref="IPluginExecutionEngine"/> interface.
    ///
    /// <seealso cref="IPluginExecutionEngine"/>
    /// </remarks>
    public partial class EquitracInstallerExecutionControl : UserControl, IPluginExecutionEngine
    {
        private EquitracInstallerActivityData _activityData;
        private ServerInfo _serverInfo;
        private DeviceInfo _deviceInfo;

        private string _signedSessionId;

        /// <summary>
        /// Constructor
        /// </summary>
        public EquitracInstallerExecutionControl()
        {
            InitializeComponent();
        }

        #region IPluginExecutionEngine implementation

        /// <summary>
        /// Executes this plug-in's workflow using the specified <see cref="PluginExecutionData"/>.
        /// </summary>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            var result = new PluginExecutionResult(PluginResult.Skipped);
            _activityData = executionData.GetMetadata<EquitracInstallerActivityData>();
            _serverInfo = executionData.Servers.FirstOrDefault();
            _deviceInfo = executionData.Assets.OfType<DeviceInfo>().FirstOrDefault();

            var jediDevice = new JediOmniDevice(_deviceInfo.Address, _deviceInfo.AdminPassword);
            var bundleInstaller = new EquitracBundleInstaller(jediDevice);

            _signedSessionId = bundleInstaller.SignIn(string.Empty);
            switch (_activityData.InstallerAction)
            {
                case EquitracInstallerAction.DeployBundleFile:
                    result = bundleInstaller.InstallSolution(_signedSessionId, _activityData.BundleFileName);
                    break;

                case EquitracInstallerAction.RegisterDevice:
                    {
                        NameValueCollection nvc = new NameValueCollection
                    {
                        {"IPAddress", _serverInfo.Address},
                        {"debug", "on"},
                        {"trace", "on"},
                        {"submit", "Ok"}
                    };
                        result = bundleInstaller.RegisterDevice(_signedSessionId, nvc);
                    }
                    break;
                case EquitracInstallerAction.ConnectDomain:
                    result = bundleInstaller.EquitracConntecDomain(_serverInfo.ServerId.ToString());
                    break;
            }

            return result;
        }

        #endregion IPluginExecutionEngine implementation

    }
}