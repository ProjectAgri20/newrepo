using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DeviceSettings;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.DeviceUtility
{
    public sealed class DeviceUtilityManager : IDisposable
    {
        private NetworkCredential _userCredential = null;
        private Framework.Assets.IDeviceInfo _deviceInfo = null;
        private JediDevice _device = null;
        private RebootDeviceActivityData _activityData = null;

        public event EventHandler<StatusChangedEventArgs> UpdateStatus;

        public IDevice Device => _device;

        public DeviceWorkflowLogger WorkflowLogger { get; set; }

        public DeviceUtilityManager(Framework.Assets.IDeviceInfo deviceInfo, NetworkCredential userCredential, RebootDeviceActivityData activityData)
        {
            _deviceInfo = deviceInfo;
            _userCredential = userCredential;
            _activityData = activityData;

            InitializeDevice();
        }

        public void ExecuteReboot()
        {
            JobMediaMode jobMediaModeNeeded = JobMediaMode.Paper;
            RebootDeviceActivityData activityData = _activityData;

            IDevice device = DeviceConstructor.Create(_deviceInfo);
            IDeviceSettingsManager settingsManager = DeviceSettingsManagerFactory.Create(device);

            if (activityData.JobMediaMode == JobMediaModeDesired.Preserve)
            {
                OnUpdateStatus("Getting job media mode because it will return to default after a reboot...");
                jobMediaModeNeeded = settingsManager.GetJobMediaMode();
                OnUpdateStatus($"Current job media mode is {jobMediaModeNeeded}.");
            }
            else
            {
                jobMediaModeNeeded = EnumUtil.Parse<JobMediaMode>(activityData.JobMediaMode.ToString());
            }

            DateTime startTime = DateTime.Now;
            if (activityData.ShouldWaitForReady)
            {
                OnUpdateStatus("Reboot Begin");
            }
            Reboot(activityData.ShouldWaitForReady);
            if (activityData.ShouldWaitForReady)
            {
                OnUpdateStatus("Reboot Complete");
                ExecutionServices.SystemTrace.LogDebug($"Device rebooted");

                if (jobMediaModeNeeded == JobMediaMode.Paperless)
                {
                    OnUpdateStatus($"Setting job media mode to {jobMediaModeNeeded}.");
                    settingsManager.SetJobMediaMode(JobMediaMode.Paperless);
                }
            }
        }

        public void Reboot(bool waitForReady)
        {
            _device.PowerManagement.Reboot();

            if (waitForReady)
            {
                var waitForBootSleep = TimeSpan.FromSeconds(120);
                WaitForPowerOff();
                OnUpdateStatus($"Sleeping while device boots ({waitForBootSleep.TotalSeconds} seconds)...");
                Thread.Sleep(waitForBootSleep);
                WaitForReady();
                OnUpdateStatus($"Device rebooted");
            }
            else
            {
                OnUpdateStatus($"Reboot command was issued.  No checks to make sure device returns to ready were requested.  Proceeding...");
            }
        }

        public void WaitForPowerOff()
        {
            OnUpdateStatus("Waiting for device to power off...");

            PowerState[] offPowerStates = new PowerState[] { PowerState.Off, PowerState.None };

            bool success =
                Wait.ForTrue(
                    () =>
                    {
                        var powerState = _device.PowerManagement.GetPowerState();
                        OnUpdateStatus($"  Device Power State: {powerState}");
                        return offPowerStates.Contains(powerState);
                    }
                  , TimeSpan.FromSeconds(240)
                  , TimeSpan.FromSeconds(5));

            if (!success)
            {
                throw new DeviceInvalidOperationException($"Device did not power off. ({_device.Address})");
            }
        }

        public void WaitForReady()
        {
            TimeSpan waitBetweenChecks = TimeSpan.FromSeconds(5);

            OnUpdateStatus("Waiting for device to come to ready...");

            PowerState[] rejectedPowerStates = new PowerState[] { PowerState.Off, PowerState.None };
            OnUpdateStatus($"  Waiting for device to return a responsive power state...");
            int powerCheckCount = 0;
            bool success =
                Wait.ForTrue(
                    () =>
                    {
                        var state = _device.PowerManagement.GetPowerState();
                        OnUpdateStatus($"    Device Power State: {state}");
                        bool ok = !rejectedPowerStates.Contains(state);
                        if (ok)
                        {
                            ++powerCheckCount;
                        }
                        return ok;
                    }
                  , TimeSpan.FromSeconds(240)
                  , waitBetweenChecks);

            if (!success)
            {
                throw new DeviceInvalidOperationException($"Device did not come to an acceptable power state. ({_device.Address})");
            }

            DeviceStatus[] acceptedDeviceStatus = new DeviceStatus[] { DeviceStatus.Running, DeviceStatus.Warning };
            OnUpdateStatus($"  Waiting for device status reflecting readiness...");
            success =
                Wait.ForTrue(
                    () =>
                    {
                        var state = _device.GetDeviceStatus();
                        OnUpdateStatus($"    Device Status: {state}");
                        return acceptedDeviceStatus.Contains(state);
                    }
                  , TimeSpan.FromSeconds(240)
                  , waitBetweenChecks);

            if (!success)
            {
                throw new DeviceInvalidOperationException($"Device did not return an acceptable status. ({_device.Address})");
            }

            OnUpdateStatus($"  Waiting for device web services and OXP services to come online...");
            const int MaxCreateCreateDeviceAttempts = 10;
            int createDeviceAttempts = 0;
            const int ConsecutiveSuccessfulDeviceInfoCallsRequired = 2;
            int consecutiveSuccessfulDeviceInfoCalls = 0;
            StringBuilder statusMessages = new StringBuilder();
            do
            {
                JediDevice tempDevice = null;
                try
                {
                    using (tempDevice = (JediDevice)DeviceConstructor.Create(_deviceInfo))
                    {
                        var deviceInfo = tempDevice.GetDeviceInfo();
                        if (++consecutiveSuccessfulDeviceInfoCalls >= ConsecutiveSuccessfulDeviceInfoCallsRequired)
                        {
                            string message = $"Device is ready. (FirmwareRevision {deviceInfo.FirmwareRevision}; FirmwareDateCode: {deviceInfo.FirmwareDateCode})";
                            OnUpdateStatus($"    {message}");
                            break;
                        }
                        else
                        {
                            string message = $"Device is responding. (ModelName {deviceInfo.ModelName}; ModelNumber: {deviceInfo.ModelNumber}; SerialNumber: {deviceInfo.SerialNumber})";
                            OnUpdateStatus($"    {message}");
                            statusMessages.AppendLine(message);
                            waitBetweenChecks = TimeSpan.FromSeconds(3);
                        }
                    }
                }
                catch (InvalidCastException castX)
                {
                    // Might encounter exception: Unable to cast object of type 'HP.DeviceAutomation.Phoenix.PhoenixDevice' to type 'HP.DeviceAutomation.Jedi.JediDevice'.
                    // Device is still initializing operate a different web service which looks like a Phoenix device.
                    OnUpdateStatus($"    Device is still initializing and hosting a rudimentary EWS landing page on a Phoenix-y web server. ({castX.Message})");
                    statusMessages.AppendLine(castX.ToString());
                }
                catch (DeviceCommunicationException comX)
                {
                    // HP.DeviceAutomation.DeviceCommunicationException: OXP UI Configuration service could not be found at 15.86.229.141
                    // HP.DeviceAutomation.DeviceCommunicationException: OXP UI Configuration service at 15.86.229.141 did not respond.
                    OnUpdateStatus($"    Device OXP services not yet ready. ({comX.Message})");
                    statusMessages.AppendLine(comX.ToString());
                }
                catch (Exception x)
                {
                    OnUpdateStatus($"    {x.Message}");
                    statusMessages.AppendLine(x.ToString());
                }

                Thread.Sleep(waitBetweenChecks);

            } while (++createDeviceAttempts <= MaxCreateCreateDeviceAttempts);

            if (createDeviceAttempts >= MaxCreateCreateDeviceAttempts)
            {
                OnUpdateStatus(statusMessages.ToString());
                throw new DeviceInvalidOperationException($"Device web services and/or OXP services did not respond as expected after {MaxCreateCreateDeviceAttempts} attempts.");
            }
        }

        private void OnUpdateStatus(string message)
        {
            UpdateStatus?.Invoke(this, new StatusChangedEventArgs(message));
        }

        public void Dispose()
        {
            if (_device != null)
            {
                _device.Dispose();
                _device = null;
            }
        }

        private void InitializeDevice()
        {
            try
            {
                // Might encounter exception: Unable to cast object of type 'HP.DeviceAutomation.Phoenix.PhoenixDevice' to type 'HP.DeviceAutomation.Jedi.JediDevice'.
                // Device is still initializing operate a different web service which looks like a Phoenix device.
                _device = (JediDevice)DeviceConstructor.Create(_deviceInfo);
                _device.PowerManagement.Wake();
            }
            catch (Exception ex)
            {
                // Make sure the device is disposed, if necessary
                if (_device != null)
                {
                    _device.Dispose();
                    _device = null;
                }

                // Log the error and re-throw.
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }
    }
}