using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.SafeComInstaller
{
    /// <summary>
    /// A class that implements the execution portion of the plug-in.
    /// </summary>
    /// <remarks>
    /// This class implements the <see cref="IPluginExecutionEngine"/> interface.
    ///
    /// <seealso cref="IPluginExecutionEngine"/>
    /// </remarks>
    [ToolboxItem(false)]
    public partial class SafeComInstallerExecutionControl : UserControl, IPluginExecutionEngine
    {
        private IDevice _device;
        private string _deviceModel;
        private StringBuilder _logText = new StringBuilder();

        private string _signedSessionId;

        /// <summary>
        ///
        /// </summary>
        public SafeComInstallerExecutionControl()
        {
            InitializeComponent();
        }

        #region IPluginExecutionEngine implementation

        /// <summary>
        /// Executes this plug-in's workflow using the specified <see cref="PluginExecutionData"/>.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="PluginExecutionResult"/> indicating the outcome of the
        /// execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Skipped);
            var data = executionData.GetMetadata<SafeComInstallerActivityData>();


            var printDeviceInfo = executionData.Assets.OfType<PrintDeviceInfo>().FirstOrDefault();
            _device = DeviceConstructor.Create(printDeviceInfo);
            _deviceModel = _device.GetDeviceInfo().ModelName;
            var bundleInstaller = new SafeComBundleInstaller(_device as JediOmniDevice);
            //only register device doesn't have these details.
            if (data.SafeComAction != SafeComAdministratorAction.RegisterDevice)
            {
                data.SafeComConfigurationCollection["devName"] = _deviceModel;
                data.SafeComConfigurationCollection["devModel"] = _deviceModel;
                data.SafeComConfigurationCollection["devPw"] = _device.AdminPassword;
            }

            try
            {
                _signedSessionId = bundleInstaller.SignIn(string.Empty);

                switch (data.SafeComAction)
                {
                    case SafeComAdministratorAction.AddDevice:
                        result =bundleInstaller.InstallSolution(_signedSessionId, data.BundleFile);
                        break;

                    case SafeComAdministratorAction.InitialConfiguration:
                    case SafeComAdministratorAction.UpdateConfiguration:
                        result = bundleInstaller.ConfigureSafeCom(_signedSessionId, data.SafeComConfigurationCollection);
                        break;

                    case SafeComAdministratorAction.RemoveDevice:
                        result = bundleInstaller.RemoveSolution(_signedSessionId, "Safecom");
                        break;

                    case SafeComAdministratorAction.RegisterDevice:
                        result = bundleInstaller.RegisterDevice(_signedSessionId, data.SafeComConfigurationCollection);
                        break;
                }
            }
            catch (WebException wex)
            {
                _device.Dispose();
                ExecutionServices.SystemTrace.LogError(
                    $"Safecom Action {data.SafeComAction} failed on device:{_device.Address}", wex);
                UpdateStatus($"{printDeviceInfo.AssetId}: Failed with exception: {wex.Message}");
                return new PluginExecutionResult(PluginResult.Failed, wex.Message);
            }
            catch (Exception ex)
            {
                _device.Dispose();
                ExecutionServices.SystemTrace.LogError(
                    $"Safecom Action {data.SafeComAction} failed on device:{_device.Address}", ex);
                UpdateStatus($"{printDeviceInfo.AssetId}: Failed with exception: {ex.Message}");
                return new PluginExecutionResult(PluginResult.Failed, ex.Message);
            }
            _device.Dispose();
            UpdateStatus($"{printDeviceInfo.AssetId}: Passed");
            return result;


        }

      

        protected virtual void UpdateStatus(string text)
        {
            status_RichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogInfo(text);
                _logText.Clear();
                _logText.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
                _logText.Append(":  ");
                _logText.AppendLine(text);
                status_RichTextBox.AppendText(_logText.ToString());
                status_RichTextBox.Refresh();
            }
                );
        }

        #endregion IPluginExecutionEngine implementation
    }
}