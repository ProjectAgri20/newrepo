using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using BadgeBoxInfo = HP.ScalableTest.Framework.Assets.BadgeBoxInfo;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.Authentication
{
    [ToolboxItem(false)]
    public partial class AuthenticationExecutionControl : UserControl, IPluginExecutionEngine
    {
        protected StringBuilder _logText = new StringBuilder();

        private AuthenticationData _activityData;
        private DeviceWorkflowLogger _workflowLogger;
        private List<IDeviceInfo> _deviceAssets = null;
        private PluginExecutionData _executionData = null;

        public AuthenticationExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Logs the device.
        /// </summary>
        private void LogDevice(IDeviceInfo deviceAsset)
        {
            ActivityExecutionAssetUsageLog assetLog = new ActivityExecutionAssetUsageLog(_executionData, deviceAsset);
            ExecutionServices.DataLogger.Submit(assetLog);
        }

        /// <summary>
        /// Launches the application.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="deviceAsset">The device asset.</param>
        /// <returns>PluginExecutionResult</returns>
        private PluginExecutionResult LaunchApp(IDevice device, IDeviceInfo deviceAsset)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);
            string buttonText = _activityData.InitiationButton;
            try
            {
                _workflowLogger.RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                
                AuthenticationInitMethod aim = GetAuthenticationInitMethod(_activityData.InitiationButton);
                DeviceSignOutMethod deviceUnauthMethod = SignOutMethod.GetSignOutMethod(_activityData.UnAuthenticateMethod);
                IAuthenticator auth = GetAuthenticator(device, deviceAsset);

                // retrieve the Authentication solution, plugin.
                IAuthenticationDriver app = GetApp(device);

                UpdateStatus("Launching authentication for " + buttonText + ".");
                if (!aim.Equals(AuthenticationInitMethod.DoNotSignIn))
                {
                    app.Launch(auth, aim);
                }
                else
                {
                    UpdateStatus("Skip Sign In :: Do not Sign In");
                }

                UpdateStatus("Logging out of device " + deviceAsset.AssetId + " - " + deviceAsset.Address + " by " + _activityData.UnAuthenticateMethod);
                if (deviceUnauthMethod.Equals(DeviceSignOutMethod.DoNotSignOut))
                {
                    UpdateStatus("Skip Sign Out :: Do Not Sign Out");
                }
                else
                {
                    app.SignOut(deviceUnauthMethod, device);
                }

                _workflowLogger.RecordEvent(DeviceWorkflowMarker.ActivityEnd);
            }
            catch (DeviceCommunicationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex.Message, "Device communication error.");
            }
            catch (DeviceInvalidOperationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex.Message, "Device automation error.");
            }
            catch (DeviceWorkflowException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error.");
            }

            return result;
        }

        private AuthenticationInitMethod GetAuthenticationInitMethod(string initMethod)
        {
            AuthenticationInitMethod aim = AuthenticationInitMethod.ApplicationButton;

            if (initMethod.Equals(AuthenticationInitMethod.SignInButton.GetDescription()))
            {
                aim = AuthenticationInitMethod.SignInButton;
            }
            else if (initMethod.Equals(AuthenticationInitMethod.Badge.GetDescription()))
            {
                aim = AuthenticationInitMethod.Badge;
            }
            else if (initMethod.Equals(AuthenticationInitMethod.DoNotSignIn.GetDescription()))
            {
                aim = AuthenticationInitMethod.DoNotSignIn;
            }
            return aim;
        }

        private void app_StatusMessageUpdate(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }
        /// <summary>
        /// Sets the device.
        /// </summary>
        /// <param name="deviceAsset">The device asset.</param>
        /// <returns></returns>
        private IDevice SetDevice(IDeviceInfo deviceAsset)
        {
            // Select a device
            IDevice device = DeviceConstructor.Create(deviceAsset);
            UpdateStatus(string.Format("Using device {0} ({1})", deviceAsset.AssetId, deviceAsset.Address));

            return device;
        }

        /// <summary>
        /// Gets the authenticator for the given device and requested solution.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="deviceInfo">Device information</param>
        /// <returns>IAuthenticator</returns>
        private IAuthenticator GetAuthenticator(IDevice device, IDeviceInfo deviceInfo)
        {
            AuthenticationCredential authCredential = null;
            if (_activityData.AuthProvider == AuthenticationProvider.Card || _activityData.AuthMethod.GetDescription().Contains("Badge"))
            {
                authCredential = new AuthenticationCredential(_executionData, deviceInfo.AssetId, deviceInfo.Address);
            }
            else
            {
                authCredential = new AuthenticationCredential(_executionData.Credential);
            }

            IAuthenticator auth = AuthenticatorFactory.Create(device, authCredential, _activityData.AuthProvider);
            auth.WorkflowLogger = _workflowLogger;
            return auth;
        }

        private IAuthenticationDriver GetApp(IDevice device)
        {
            var result = AuthenticatorDriverFactory.Create(device, _activityData.InitiationButton, _workflowLogger);
            return result;
        }

        /// <summary>
        /// Updates the status TextBox.
        /// </summary>
        /// <param name="text">The text.</param>
        protected virtual void UpdateStatus(string text)
        {
            status_RichTextBox.InvokeIfRequired(c =>
                    {
                        ExecutionServices.SystemTrace.LogNotice("Status=" + text);
                        _logText.Clear();
                        _logText.Append(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                        _logText.Append("  ");
                        _logText.AppendLine(text);
                        c.AppendText(_logText.ToString());
                        c.Refresh();
                    }
                );
        }

        /// <summary>
        /// Updates the label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="text">The text.</param>
        protected virtual void UpdateLabel(Label label, string text)
        {
            label.InvokeIfRequired(c =>
                   {
                       ExecutionServices.SystemTrace.LogInfo("Label=" + text);
                       c.Text = text;
                       c.Refresh();
                   }
                );
        }

        /// <summary>
        /// Executes this plugin's workflow using the specified <see cref="T:HP.ScalableTest.Framework.Plugin.PluginExecutionData" />.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="T:HP.ScalableTest.Framework.Plugin.PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);
            _executionData = executionData;

            _deviceAssets = executionData.Assets.OfType<IDeviceInfo>().ToList();
            ExecutionServices.SystemTrace.LogDebug($"# of assets in ExecutionData: {executionData.Assets.Count}");

            try
            {
                UpdateStatus("Starting activity...");
                _activityData = executionData.GetMetadata<AuthenticationData>(ConverterProvider.GetMetadataConverters());

                _workflowLogger = new DeviceWorkflowLogger(executionData);
                TimeSpan acquireTimeout = _activityData.LockTimeouts.AcquireTimeout;
                TimeSpan holdTimeout = _activityData.LockTimeouts.HoldTimeout;

                string msg = string.Empty;

                List<AssetLockToken> tokens = _deviceAssets.Select(n => new AssetLockToken(n, acquireTimeout, holdTimeout)).ToList();
                _workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
                ExecutionServices.CriticalSection.Run(tokens, selectedToken =>
                {
                    IDeviceInfo deviceInfo = (selectedToken as AssetLockToken).AssetInfo as IDeviceInfo;
                    LogDevice(deviceInfo);

                    using (IDevice device = SetDevice(deviceInfo))
                    {
                        var retryManager = new PluginRetryManager(executionData, UpdateStatus);
                        result = retryManager.Run(() => LaunchApp(device, deviceInfo));
                    }
                });

                _workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);

            }
            catch (DeviceCommunicationException ex)
            {
                return new PluginExecutionResult(PluginResult.Failed, ex.Message, "Device communication error.");
            }
            catch (DeviceInvalidOperationException ex)
            {
                return new PluginExecutionResult(PluginResult.Failed, ex.Message, "Device automation error.");
            }
            catch (DeviceWorkflowException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error.");
            }
            catch (AcquireLockTimeoutException)
            {
                return new PluginExecutionResult(PluginResult.Skipped, "Could not obtain lock on specified devices(s).", "Device unavailable.");
            }
            catch (HoldLockTimeoutException)
            {
                return new PluginExecutionResult(PluginResult.Error, $"Automation did not complete within {_activityData.LockTimeouts.HoldTimeout}.", "Automation timeout exceeded.");
            }
            finally
            {
                UpdateStatus("Finished activity");
            }
            return result;
        }
    }
}
