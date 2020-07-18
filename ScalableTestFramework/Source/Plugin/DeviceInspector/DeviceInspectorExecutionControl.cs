using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi.OmniUserInteraction;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Plugin.DeviceInspector.SettingsData;

namespace HP.ScalableTest.Plugin.DeviceInspector
{
    /// <summary>
    /// Used to execute the activity of the DeviceConfiguration plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class DeviceInspectorExecutionControl : UserControl, IPluginExecutionEngine
    {
        private StringBuilder _logText = new StringBuilder();
        private DeviceInspectorActivityData _activityData;
        private PluginExecutionData _executionData;

        /// <summary>
        /// Initializes a new instance of the DeviceConfigurationExecutionControl class.
        /// </summary>
        public DeviceInspectorExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute the task of the DeviceConfiguration activity.
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _executionData = executionData;
            _activityData = executionData.GetMetadata<DeviceInspectorActivityData>();
            TimeSpan lockTimeout = TimeSpan.FromMinutes(5);
            TimeSpan holdTimeout = TimeSpan.FromMinutes(5);
            ConcurrentDictionary<string, DataPair<string>> verifiedResults = new ConcurrentDictionary<string, DataPair<string>>();
            if (!_executionData.Assets.OfType<IDeviceInfo>().Any())
            {
                return new PluginExecutionResult(PluginResult.Failed, "There were no assets retrieved.  If this is a count-based run, your reservation in asset inventory may have expired.", "DeviceInfo Asset error");
            }

            ExecutionServices.SystemTrace.LogDebug("Beginning Update");

            try
            {
                //Parallel.ForEach(_executionData.Assets.OfType<IDeviceInfo>(),asset =>
                foreach(var asset in _executionData.Assets.OfType<IDeviceInfo>())
                    {
                        JediDevice device = null;
                        try
                        {
                            try
                            {
                                device = DeviceConstructor.Create(asset) as JediDevice;
                            }
                            catch
                            {
                                device = DeviceFactory.Create(asset.Address) as JediDevice;
                            }

                            // Make sure the device is in a good state
                            var devicePrepManager = DevicePreparationManagerFactory.Create(device);
                            try
                            {
                                devicePrepManager.InitializeDevice(true);
                            }
                            catch (WebInspectorException webInspectorException)
                            {
                                //let's ignore initialise device errors, this might be due to inspection page in use,
                                //since we are not doing any control panel action, this should be ok
                                ExecutionServices.SystemTrace.LogDebug(webInspectorException.Message);
                            }
                           

                            ExecutionServices.SystemTrace.LogDebug(
                                $"Processing {asset.AssetId} on thread {Thread.CurrentThread}");

                            AssetLockToken assetToken = new AssetLockToken(asset, lockTimeout, holdTimeout);
                            
                            ExecutionServices.CriticalSection.Run(assetToken, () =>
                            {
                            ExecutionServices.SystemTrace.LogDebug(
                                    $"Performing update on device {asset.AssetId} at address {asset.Address}");
                                var verifiedResult = UpdateDevice(device, asset);
                                verifiedResults.AddOrUpdate(asset.AssetId, verifiedResult,(key, oldvalue) => UpdateDevice(device, asset));
                            
                            });
                        }
                        catch (DeviceCommunicationException deviceCommunicationException)
                        {
                            ExecutionServices.SystemTrace.LogDebug($"Unable to communicate with the device: {deviceCommunicationException.Message}");
                            verifiedResults.AddOrUpdate(asset.AssetId,
                                new DataPair<string> { Key = "Device Communication", Value = false },
                                (s, pair) => new DataPair<string>() { Key = "Device Communication", Value = false });
                           device?.Dispose();
                        }
                        catch (Exception e)
                        {
                            ExecutionServices.SystemTrace.LogDebug(e);
                            verifiedResults.AddOrUpdate(asset.AssetId,
                                new DataPair<string> {Key = "Failed Settings", Value = false},
                                (s, pair) => new DataPair<string> {Key = "Failed Settings", Value = false});
                            device?.Dispose();
                        }
                        
                    }

            }
            catch
            {
                return new PluginExecutionResult(PluginResult.Error, "Error during Device Configuration Setup");
            }

            foreach (var verifiedResult in verifiedResults)
            {
                UpdateStatus($"Device: {verifiedResult.Key} Result: {verifiedResult.Value.Value}, {verifiedResult.Value.Key}");
            }

            if (verifiedResults.Any(x => x.Value.Value == false))
            {
                return new PluginExecutionResult(PluginResult.Failed);
            }
         

            return new PluginExecutionResult(PluginResult.Passed);
        }

        public DataPair<string> UpdateDevice(JediDevice device, IDeviceInfo asset)
        {
            bool success = true;
            StringBuilder failedSettings = new StringBuilder();
            foreach (var control in _activityData.ComponentData)
            {
                //if the device doesn't have scanner then we skip checking scan related settings
                if (!asset.Attributes.HasFlag(AssetAttributes.Scanner))
                {
                    if (control is EmailSettingsData || control is CopySettingsData || control is FolderSettingsData ||
                        control is QuickSetSettingsData || control is ScanUsbSettingData)
                    {
                        continue;
                    }
                }

                // if the device is scanner type and doesn't have print then we skip check settings which require print
                if (!asset.Attributes.HasFlag(AssetAttributes.Printer))
                {
                    if (control is CopySettingsData || control is PrintSettingsData || control is JobSettingsData)
                    {
                        continue;
                    }
                }

                var temp = control.VerifyFields(device);
                if (!temp.Value)
                {
                    if (success)
                    {
                        success = false;
                    }
                    failedSettings.AppendLine($"{control.GetType().Name} - {temp.Key}");
                }
            }
            if (failedSettings.Length == 0)
            {
                failedSettings.Append("All Setting Verified.");
            }
            else
            {
                failedSettings.Insert(0, "Failed Settings: ");
            }
            return new DataPair<string>{Key = failedSettings.ToString(), Value = success}; /*SetDomain(device) && SetPasswordSettings(device) &&*/ //_activityData.genData.SetFields(device);
        }
       

       
        /// <summary>
        /// Updates the status text in the execution control display.
        /// </summary>
        /// <param name="text"></param>
        protected virtual void UpdateStatus(string text)
        {
            status_RichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogInfo(text);
                _logText.Clear();
                _logText.Append(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                _logText.Append("  ");
                _logText.AppendLine(text);
                status_RichTextBox.AppendText(_logText.ToString());
                status_RichTextBox.Refresh();
            }
                );
        }
    }
}
