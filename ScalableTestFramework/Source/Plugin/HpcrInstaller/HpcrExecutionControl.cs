using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Plugin.HpcrInstaller.Properties;
using HP.ScalableTest.Utility;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.HpcrInstaller
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
    public partial class HpcrExecutionControl : UserControl, IPluginExecutionEngine
    {
        private IDevice _device;
        private readonly StringBuilder _logText = new StringBuilder();

        /// <summary>
        ///
        /// </summary>
        public HpcrExecutionControl()
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
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed);
            var data = executionData.GetMetadata<HpcrActivityData>();

            TimeSpan lockTimeout = TimeSpan.FromMinutes(5);
            TimeSpan holdTimeout = TimeSpan.FromMinutes(5);
            string hpcrServerAddress = executionData.Servers.FirstOrDefault()?.Address;
            var asset = executionData.Assets.OfType<IDeviceInfo>().FirstOrDefault();

            AssetLockToken assetToken = new AssetLockToken(asset, lockTimeout, holdTimeout);
            ExecutionServices.CriticalSection.Run(assetToken, () =>
            {
                result = ExecuteHpcrComAction((PrintDeviceInfo)asset, data, hpcrServerAddress);
            });

            return result;
        }

        private PluginExecutionResult ExecuteHpcrComAction(PrintDeviceInfo asset, HpcrActivityData data, string serverAddress)
        {
            _device = DeviceConstructor.Create(asset);

            try
            {
                switch (data.HpcrAction)
                {
                    case HpcrAction.Install:
                        Install(serverAddress, data.DeviceGroup, _device.Address, _device.AdminPassword);
                        break;

                    case HpcrAction.Uninstall:
                        Uninstall(serverAddress, data.DeviceGroup, _device.Address, _device.AdminPassword);
                        break;
                }
            }
            catch (WebException wex)
            {
                ExecutionServices.SystemTrace.LogError($"Hpcr Action {data.HpcrAction} failed on device:{_device.Address}", wex);
                UpdateStatus($"{asset.AssetId}: Failed with exception: {wex.Message}");
                return new PluginExecutionResult(PluginResult.Failed, wex.Message);
            }
            _device.Dispose();
            UpdateStatus($"{asset.AssetId}-{data.HpcrAction}: Passed");
            return new PluginExecutionResult(PluginResult.Passed);
        }

        protected virtual void UpdateStatus(string text)
        {
            status_RichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogInfo(text);
                _logText.Clear();
                _logText.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
                _logText.Append("  ");
                _logText.AppendLine(text);
                status_RichTextBox.AppendText(_logText.ToString());
                status_RichTextBox.Refresh();
            }
                );
        }

        /// <summary>
        /// Install HPCR on thespecified device
        /// </summary>
        /// <param name="hpcrServerAddress">The HPCR server address</param>
        /// <param name="deviceGroup">The device group to which the device to be added</param>
        /// <param name="deviceAddress">The device address</param>
        /// <param name="devicePassword">The device password</param>
        private void Install(string hpcrServerAddress, string deviceGroup, string deviceAddress, string devicePassword = "")
        {
            Uri installUri = new Uri(string.Format(Resources.HpcrUrl, hpcrServerAddress, HpcrAction.Install, deviceGroup, deviceAddress));
            string data = string.Format(Resources.InstallPostData, deviceGroup, deviceAddress, devicePassword);
            SendRequest(installUri, data);
        }

        /// <summary>
        /// Uninstall HPCR on thespecified device
        /// </summary>
        /// <param name="hpcrServerAddress">The HPCR server address</param>
        /// <param name="deviceGroup">The device group to which the device to be added</param>
        /// <param name="deviceAddress">The device address</param>
        /// <param name="devicePassword">The device password</param>
        private void Uninstall(string hpcrServerAddress, string deviceGroup, string deviceAddress, string devicePassword = "")
        {
            Uri installUri = new Uri(string.Format(Resources.HpcrUrl, hpcrServerAddress, HpcrAction.Uninstall, deviceGroup, deviceAddress));
            string data = string.Format(Resources.UninstallPostData, deviceGroup, deviceAddress, devicePassword);
            SendRequest(installUri, data);
        }

        /// <summary>
        /// Sends the Http request with the specified Uri and post data.
        /// </summary>
        /// <param name="uri">The Uri</param>
        /// <param name="stringData">The data</param>
        /// <returns><see cref="HttpResponse"/></returns>
        private static void SendRequest(Uri uri, string stringData)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/xml; charset=utf-8";
            request.KeepAlive = false;

            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(stringData);
                request.ContentLength = buffer.Length;
                Stream sw = request.GetRequestStream();
                sw.Write(buffer, 0, buffer.Length);
                sw.Close();

                var response = HttpMessenger.ExecuteRequest(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("Hpcr Solution Installation failed");
                }
            }
            catch (Exception e)
            {
                throw new WebException("Hpcr Solution Installation failed", e);
            }
        }

        #endregion IPluginExecutionEngine implementation
    }
}