using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi.OmniUserInteraction;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.DeviceConfiguration
{
    /// <summary>
    /// Used to execute the activity of the DeviceConfiguration plugin.
    /// </summary>
//
    public partial class DeviceConfigurationExecutionControl : UserControl, IPluginExecutionEngine
    {

        private StringBuilder _logText = new StringBuilder();
        private DeviceConfigurationActivityData _activityData;
        private PluginExecutionData _executionData;

        /// <summary>
        /// Initializes a new instance of the DeviceConfigurationExecutionControl class.
        /// </summary>
        public DeviceConfigurationExecutionControl()
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
            //bool result = false;
            _executionData = executionData;
            _activityData = executionData.GetMetadata<DeviceConfigurationActivityData>();
            TimeSpan lockTimeout = TimeSpan.FromMinutes(10);
            TimeSpan holdTimeout = TimeSpan.FromMinutes(20);
            ConcurrentDictionary<string, DataPair<string>> results = new ConcurrentDictionary<string, DataPair<string>>();

            if (!_executionData.Assets.OfType<IDeviceInfo>().Any())
            {
                return new PluginExecutionResult(PluginResult.Failed, "There were no assets retrieved.  If this is a count-based run, your reservation in asset inventory may have expired.", "DeviceInfo Asset error");
            }

            ExecutionServices.SystemTrace.LogDebug("Beginning Update");

            try
            {
                //Parallel.ForEach(_executionData.Assets.OfType<IDeviceInfo>(), asset =>
                foreach(var asset in _executionData.Assets.OfType<IDeviceInfo>())
                {
                    JediDevice device = null;
                    try
                    {
                        //DeviceConfigResultLog log = new DeviceConfigResultLog(_executionData, asset.AssetId);
                        SetDefaultPassword(asset.Address, asset.AdminPassword);

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

                        //devicePrepManager.PerformanceLogger = PerformanceLogger;

                        ExecutionServices.SystemTrace.LogDebug(
                            $"Processing {asset.AssetId} on thread {Thread.CurrentThread}");

                        AssetLockToken assetToken = new AssetLockToken(asset, lockTimeout, holdTimeout);

                        ExecutionServices.CriticalSection.Run(assetToken, () =>
                        {
                            ExecutionServices.SystemTrace.LogDebug(
                                $"Performing update on device {asset.AssetId} at address {asset.Address}");
                            var result = UpdateDevice(device, (AssetInfo) asset, _executionData);
                            results.AddOrUpdate(asset.AssetId, result,
                                (key, oldValue) => UpdateDevice(device, (AssetInfo) asset, _executionData));
                            device?.Dispose();
                        });
                    }
                    catch (DeviceCommunicationException deviceCommunicationException)
                    {
                        ExecutionServices.SystemTrace.LogDebug($"Unable to communicate with the device: {deviceCommunicationException.Message}");
                        results.AddOrUpdate(asset.AssetId,
                            new DataPair<string> {Key = "Device Communication", Value = false},
                            (s, pair) => new DataPair<string>() {Key = "Device Communication", Value = false});
                        device?.Dispose();
                    }
                    catch (Exception e)
                    {
                        ExecutionServices.SystemTrace.LogDebug(e);
                        results.AddOrUpdate(asset.AssetId,
                            new DataPair<string> {Key = "Failed Settings", Value = false},
                            (s, pair) => new DataPair<string> {Key = "Failed Settings", Value = false});
                        //[Veda]:we had few in use inspection pages error in subsequent plugin/activities, which was as a result of this plugin failing
                        //eg: invalid xml content. disposing the device to avoid those problems
                        device?.Dispose();
                    }
                }//);
            }
            catch
            {
                return new PluginExecutionResult(PluginResult.Error, "Error during Device Configuration Setup");
            }

            foreach (var item in results)
            {
                UpdateStatus($"Device {item.Key}: Result: {item.Value.Value}, {item.Value.Key}");
            }

            if (results.Any(x => x.Value.Value == false))
            {
                return new PluginExecutionResult(PluginResult.Failed);
            }


            return new PluginExecutionResult(PluginResult.Passed);
        }


        public DataPair<string> UpdateDevice(JediDevice device, AssetInfo assetInfo,  PluginExecutionData executionData)
        {
            bool success = true;
            StringBuilder failedSettings = new StringBuilder();
            foreach (var control in _activityData.ComponentData)
            {
                ExecutionServices.SystemTrace.LogDebug($"{assetInfo.AssetId}: Setting configuration for {control}");
                var temp = control.SetFields(device, assetInfo, executionData);
                if (!temp.Value)
                {
                    if (success)
                    {
                        success = false;
                    }
                    failedSettings.AppendLine($"{control.GetType().Name} = {temp.Key}");
                    ExecutionServices.SystemTrace.LogDebug($"{assetInfo.AssetId}: Configuration Failed for {control}");
                }
                else
                {
                    ExecutionServices.SystemTrace.LogDebug($"{assetInfo.AssetId}: Configuration set for {control}");
                }

            }
            if (failedSettings.Length == 0)
            {
                failedSettings.Append("All Settings Applied Successfully");
            }
            else
            {
                failedSettings.Insert(0, "Failed Settings: ");
            }

            return new DataPair<string> { Key = failedSettings.ToString(), Value = success };
        }


        public void SetDefaultPassword(string address, string password)
        {
            var defPWUrn = "urn:hp:imaging:con:service:security:SecurityService:AdministratorAuthenticationSettings";
            string endpoint = "security";
            ExecutionServices.SystemTrace.LogDebug($"{address}: Setting default password");
            if (_activityData.EnableDefaultPW)
            {
                JediDevice dev;
                try
                {
                    dev = new JediDevice(address, "");
                    WebServiceTicket tic= null;
                    Retry.WhileThrowing<DeviceCommunicationException>(()=> tic = dev.WebServices.GetDeviceTicket(endpoint, defPWUrn), 10, TimeSpan.FromSeconds(6));
                    if(tic == null)
                        throw new DeviceCommunicationException("Unable to connect to device to set password.");
                    if (password.Length < 8)
                    {
                        tic.FindElement("MinLength").SetValue(password.Length - 1);
                        tic.FindElement("IsPasswordComplexityEnabled").SetValue("false");
                      
                    }
                    tic.FindElement("Password").SetValue(password);
                    tic.FindElement("PasswordStatus").SetValue("set");
                    dev.WebServices.PutDeviceTicket("security", defPWUrn, tic, false);
                    
                    dev = new JediDevice(address, password);
                    ExecutionServices.SystemTrace.LogDebug($"{address}: Default password set");
                }
                catch (Exception exception)
                {
                    ExecutionServices.SystemTrace.LogError(exception.Message);
                    dev = new JediDevice(address, password);
                }
                //disposing the device
                dev.Dispose();
            }
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
                _logText.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
                _logText.Append("  ");
                _logText.AppendLine(text);
                status_RichTextBox.AppendText(_logText.ToString());
                status_RichTextBox.Refresh();
            }
                );
        }
    }
}
