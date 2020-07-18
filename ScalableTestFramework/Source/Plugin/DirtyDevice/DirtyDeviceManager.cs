using System;
using System.Net;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.DeviceAutomation;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public sealed class DirtyDeviceManager : IDisposable
    {
        private Framework.Assets.IDeviceInfo _deviceInfo = null;
        private NetworkCredential _userCredential = null;
        private PluginEnvironment _environment = null;

        private JediOmniDevice _device;
        private JediOmniPreparationManager _preparationManager;

        private DigitalSendExerciser _digitalSendExerciser;
        private EwsExerciserViaSeleniumWebDriver _ewsExerciser;
        private PrintExerciser _printExerciser;
        private SnmpExerciser _snmpExerciser;
        private UIExerciser _uiExerciser;
        private WebServicesExerciser _webServicesExerciser;

        private DirtyDeviceActivityData _activityData = null;
        public event EventHandler<StatusChangedEventArgs> UpdateStatus;

        /// <summary>
        /// Gets the <see cref="IDevice" />.
        /// </summary>
        public IDevice Device => _device;

        public DirtyDeviceManager(Framework.Assets.IDeviceInfo deviceInfo, NetworkCredential userCredential, DirtyDeviceActivityData activityData, PluginEnvironment environment)
        {
            _deviceInfo = deviceInfo;
            _userCredential = userCredential;
            _environment = environment;
            _activityData = activityData;

            InitializeDevice();
        }

        public void ExecuteDirty()
        {
            var actions = DirtyDeviceActions.SelectedActions(_activityData.DirtyDeviceActionFlags);
            OnUpdateStatus(this, $"");

            foreach (var action in actions)
            {
                DateTime startTime = DateTime.Now;

                string actionDescription = $"Dirty {EnumUtil.GetDescription(action)} on {_deviceInfo.Address}";
                OnUpdateStatus(this, actionDescription + " Start");
               
                switch (action)
                {
                    case DirtyDeviceActionFlags.UserInterface:
                        _uiExerciser.Exercise(_activityData);
                        break;
                    case DirtyDeviceActionFlags.WebServices:
                        _webServicesExerciser.Exercise(_activityData,_deviceInfo.Attributes);
                        break;
                    case DirtyDeviceActionFlags.EWS:
                        _ewsExerciser.Exercise(_activityData, _userCredential, _environment, _deviceInfo.Attributes);
                        break;
                    case DirtyDeviceActionFlags.SNMP:
                        _snmpExerciser.Exercise(_activityData);
                        break;
                    case DirtyDeviceActionFlags.Print:
                        _printExerciser.Exercise(_activityData);
                        break;
                    case DirtyDeviceActionFlags.DigitalSend:
                        if (_deviceInfo.Attributes.HasFlag(AssetAttributes.Scanner))
                        {
                            _digitalSendExerciser.Exercise(_activityData, _userCredential, _environment);
                        }
                        break;
                }

                TimeSpan duration = DateTime.Now - startTime;
                OnUpdateStatus(this, actionDescription + $" Complete (Time elapsed: {duration})");
                OnUpdateStatus(this, $"");
            }
        }

        internal void OnUpdateStatus(object sender, string message)
        {
            UpdateStatus?.Invoke(sender, new StatusChangedEventArgs(message));
        }

        public void Dispose()
        {
            _device?.Dispose();
        }

        private void InitializeDevice()
        {
            try
            {
                OnUpdateStatus(this, "Initializing device.");
                _device = (JediOmniDevice)DeviceConstructor.Create(_deviceInfo);
                _preparationManager = new JediOmniPreparationManager(_device);

                _digitalSendExerciser = new DigitalSendExerciser(this, _device, _preparationManager);
                _ewsExerciser = new EwsExerciserViaSeleniumWebDriver(this, _device);
                _printExerciser = new PrintExerciser(this, _device);
                _snmpExerciser = new SnmpExerciser(this, _device);
                _uiExerciser = new UIExerciser(this, _device, _preparationManager);
                _webServicesExerciser = new WebServicesExerciser(this, _device);

                _preparationManager.InitializeDevice(true);
            }
            catch (Exception ex)
            {
                OnUpdateStatus(this, ex.ToString());
                OnUpdateStatus(this, "Cleaning up.");

                _device?.Dispose();

                // Log the error and re-throw.
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }
    }
}
