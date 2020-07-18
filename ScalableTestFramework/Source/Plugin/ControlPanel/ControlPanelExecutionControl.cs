using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.ControlPanel
{
    public partial class ControlPanelExecutionControl : UserControl, IPluginExecutionEngine
    {
        public ControlPanelExecutionControl()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed);
            var activityData = executionData.GetMetadata<ControlPanelActivityData>();

            TimeSpan lockTimeout = TimeSpan.FromMinutes(5);
            TimeSpan holdTimeout = TimeSpan.FromMinutes(5);

            //if the assets are not found then skip it
            if (!executionData.Assets.OfType<IDeviceInfo>().Any())
                return new PluginExecutionResult(PluginResult.Skipped, "No Assets found for execution");

            var asset = executionData.Assets.OfType<IDeviceInfo>().FirstOrDefault();
            if (!asset.Attributes.HasFlag(AssetAttributes.ControlPanel))
            {
                return new PluginExecutionResult(PluginResult.Skipped, "Device does not have control panel");
            }
            AssetLockToken assetToken = new AssetLockToken(asset, lockTimeout, holdTimeout);
            ExecutionServices.CriticalSection.Run(assetToken, () =>
                    {
                        ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(executionData, asset));
                        var printDevice = DeviceConstructor.Create(asset);
                        ControlPanelWrapper wrapper = new ControlPanelWrapper(printDevice, executionData.Credential,
                        activityData.ControlPanelAction, activityData.ParameterValues);
                        wrapper.ScreenCapture += (s, e) => UpdateScreenShot(e.ScreenShotImage);
                        wrapper.StatusUpdate += (s, e) => UpdateStatus(e.StatusMessage);
                        result = wrapper.Execute();
                        printDevice.Dispose();
                    });

            return result;
        }

        protected virtual void UpdateStatus(string statusMsg)
        {
            pluginStatus_TextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + statusMsg);
                c.AppendText($"{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}  {statusMsg}\n");
                c.Select(c.Text.Length, 0);
                c.ScrollToCaret();
            });

            ExecutionServices.SystemTrace.LogInfo(statusMsg);
        }

        protected virtual void UpdateScreenShot(Image screenshot)
        {
            controlPanel_pictureBox.InvokeIfRequired(c => { c.Image = screenshot; });
        }
    }

    public enum Language
    {
        Dansk, Deutsch, English, Espanol, French, Italiano, Norsk, Netherlands, Svensk, Suomi, Portugues, Turkey, Poliski, Russian, Cestina, Magyar, Japan, ChineseTraditional, ChineseSimplified, Korean, Greek, Croatian, Romanian, Slovak, Slovenian, Catalan, Thai, Indonesia
    };
}