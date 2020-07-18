using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    [ToolboxItem(false)]
    public partial class DirtyDeviceExecutionControl : UserControl, IPluginExecutionEngine
    {
        private NetworkCredential _currentUser = null;
        private StringBuilder _logText = new StringBuilder();
        private PluginExecutionData _executionData = null;
        private DirtyDeviceActivityData _activityData = null;
        private Framework.Assets.IDeviceInfo _currentDevice = null;
        private DeviceWorkflowLogger _workflowLogger = null;

        public DirtyDeviceExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes this plugin's workflow using the specified <see cref="T:HP.ScalableTest.Framework.Plugin.PluginExecutionData" />.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="T:HP.ScalableTest.Framework.Plugin.PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            // reset current user as it may have changed since control load.
            _executionData = executionData;
            _currentUser = _executionData.Credential;
            _activityData = _executionData.GetMetadata<DirtyDeviceActivityData>(new[] { new DirtyDeviceDataConverter1_1() });

            UpdateExecutionStatus(this, "Starting activity...");

            _currentDevice = _executionData.Assets.GetRandom<Framework.Assets.IDeviceInfo>();
            _workflowLogger = new DeviceWorkflowLogger(_executionData);

            InitializeControlWithActivityData();

            try
            {
                // Wrap in using to release any connections to the device.  This is critical for Omni operations.
                using (DirtyDeviceManager pluginActivityManager = new DirtyDeviceManager(_currentDevice, _currentUser, _activityData, executionData.Environment))
                {
                    pluginActivityManager.UpdateStatus += (s, e) => UpdateExecutionStatus(s, e);

                    if (_currentDevice == null)
                    {
                        return new PluginExecutionResult(PluginResult.Error, $"{typeof(IDeviceInfo).Name} retrieved is null.  If this is a count-based run, your reservation in asset inventory may have expired.", "DeviceInfo Asset error");
                    }
                    else if (_currentDevice.AssetId == null)
                    {
                        return new PluginExecutionResult(PluginResult.Error, $"{typeof(IDeviceInfo).Name}.AssetId property is null.", "DeviceInfo Asset error");
                    }
                    else if (_currentDevice.AssetType == null)
                    {
                        return new PluginExecutionResult(PluginResult.Error, $"{typeof(IDeviceInfo).Name}.AssetType property is null.", "DeviceInfo Asset error");
                    }
                    else if (pluginActivityManager.Device == null)
                    {
                        return new PluginExecutionResult(PluginResult.Error, $"{nameof(pluginActivityManager)}.Device property is null.", $"Could not create IDevice (AssetId: {_currentDevice.AssetId})");
                    }

                    PluginExecutionResult pluginExecutionResult = new PluginExecutionResult(PluginResult.Error, new Exception($"The {typeof(PluginExecutionResult)} was never set by the activity."));
                    try
                    {
                        UpdateExecutionStatus(this, $"Waiting for access to {_currentDevice.AssetId} ({_currentDevice.Address})");
                        var token = new AssetLockToken(_currentDevice, _activityData.LockTimeouts.AcquireTimeout, _activityData.LockTimeouts.HoldTimeout);
                        _workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
                        ExecutionServices.CriticalSection.Run(token, () =>
                        {
                            var retryManager = new PluginRetryManager(executionData, (e) => UpdateExecutionStatus(this, e));
                            pluginExecutionResult = retryManager.Run(() =>
                            {
                                ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(_executionData, _currentDevice.AssetId));

                                try
                                {
                                    pluginActivityManager.ExecuteDirty();
                                    return new PluginExecutionResult(PluginResult.Passed);
                                }
                                catch (DeviceCommunicationException ex)
                                {
                                    GatherTriageData(ex.ToString(), pluginActivityManager);
                                    return new PluginExecutionResult(PluginResult.Failed, ex, "Device communication error.");
                                }
                                catch (DeviceInvalidOperationException ex)
                                {
                                    GatherTriageData(ex.ToString(), pluginActivityManager);
                                    return new PluginExecutionResult(PluginResult.Failed, ex, "Device automation error.");
                                }
                                catch (DeviceWorkflowException ex)
                                {
                                    GatherTriageData(ex.ToString(), pluginActivityManager);
                                    return new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error.");
                                }
                                catch (Exception ex)
                                {
                                    GatherTriageData(ex.ToString(), pluginActivityManager);
                                    return new PluginExecutionResult(PluginResult.Error, new Exception($"The plugin activity threw an unexpected exception: {ex.ToString()}", ex));
                                }
                            });
                        });
                        _workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);
                    }
                    catch (AcquireLockTimeoutException ex)
                    {
                        GatherTriageData(ex.ToString(), pluginActivityManager);
                        return new PluginExecutionResult(PluginResult.Skipped, string.Format("Could not obtain lock on device {0}.", (_currentDevice != null ? _currentDevice.AssetId : "null")), "Device unavailable.");
                    }
                    catch (HoldLockTimeoutException ex)
                    {
                        GatherTriageData(ex.ToString(), pluginActivityManager);
                        return new PluginExecutionResult(PluginResult.Error, $"Automation did not complete within {_activityData.LockTimeouts.HoldTimeout}.", "Automation timeout exceeded.");
                    }
                    catch (Exception ex)
                    {
                        GatherTriageData(ex.ToString(), pluginActivityManager);
                        return new PluginExecutionResult(PluginResult.Error, new Exception($"The {typeof(PluginRetryManager).Name} threw an unexpected exception.", ex));
                    }

                    return pluginExecutionResult;
                }
            }
            catch (Exception x)
            {
                return new PluginExecutionResult(PluginResult.Error, x, "Plugin error during activity setup.");
            }
            finally
            {
                UpdateExecutionStatus(this, $"Finished activity.");
            }
        }

        /// <summary>
        /// Gathers the triage data.
        /// </summary>
        /// <param name="reason">The reason.</param>
        private void GatherTriageData(string reason, DirtyDeviceManager pluginActivityManager)
        {
            if (pluginActivityManager?.Device == null)
            {
                var mgrState = (pluginActivityManager == null) ? "IS" : "IS NOT";
                var deviceState = (pluginActivityManager?.Device == null) ? "IS" : "IS NOT";
                throw new ArgumentException($"Cannot triage null device.  ({nameof(pluginActivityManager)} {mgrState} null.  {nameof(pluginActivityManager)}.Device {deviceState} null.)");
            }

            ITriage triage = TriageFactory.Create(pluginActivityManager.Device, _executionData);
            triage.CollectTriageData(reason);
            triage.Submit();
        }

        private InvokeMethodology GetInvokeMethodology(object sender)
        {
            if (sender is DigitalSendExerciser ||
                sender is ClientConnection ||
                sender is FtpServer ||
                sender is HttpListenerBase)
            {
                return InvokeMethodology.BeginInvoke;
            }
            return InvokeMethodology.Invoke;
        }

        /// <summary>
        /// Updates control display with activity data.
        /// </summary>
        private void InitializeControlWithActivityData()
        {
            this.InvokeIfRequired(c =>
            {
                if (_currentDevice == null)
                {
                    throw new InvalidOperationException($"Current device is null.  Your reservation in asset inventory may have expired.");
                }

                UpdateLabel(activeDeviceLabel, $"{_currentDevice.AssetId} ({_currentDevice.Address})");
                var actionList = new List<string>();
                foreach (DirtyDeviceActionFlags flag in DirtyDeviceActions.SelectedActions(_activityData.DirtyDeviceActionFlags))
                {
                    try
                    {
                        actionList.Add(EnumUtil.GetDescription(flag));
                    }
                    catch (Exception x)
                    {
                        UpdateExecutionStatus(this, new StatusChangedEventArgs($"Error deriving string value for '{flag}': {x.ToString()}"));
                        actionList.Add(flag.ToString());
                    }
                }
                var actionDescription = string.Join(" | ", actionList);
                UpdateLabel(labelDocumentProcessAction, actionDescription);
            });
        }

        private void UpdateLabel(Label label, string text)
        {
            label.InvokeIfRequired(c =>
            {
                label.Text = text;
                label.Refresh();
            });
        }

        private void UpdateExecutionStatus(object sender, StatusChangedEventArgs e)
        {
            var invokeMethodology = GetInvokeMethodology(sender);

            status_RichTextBox.SmartInvoke(
                invokeMethodology,
                c =>
                {
                    ExecutionServices.SystemTrace.LogInfo(e.StatusMessage);
                    _logText.Clear();
                    _logText.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    _logText.Append("  ");
                    _logText.AppendLine(e.StatusMessage);
                    status_RichTextBox.AppendText(_logText.ToString());
                    status_RichTextBox.Refresh();
                });
        }

        private void UpdateExecutionStatus(object sender, string text)
        {
            var invokeMethodology = GetInvokeMethodology(sender);

            status_RichTextBox.SmartInvoke(
                invokeMethodology,
                c =>
                {
                    ExecutionServices.SystemTrace.LogInfo(text);
                    _logText.Clear();
                    _logText.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    _logText.Append("  ");
                    _logText.AppendLine(text);
                    status_RichTextBox.AppendText(_logText.ToString());
                    status_RichTextBox.Refresh();
                });
        }
    }
}
