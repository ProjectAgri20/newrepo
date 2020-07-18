using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
namespace HP.ScalableTest.Plugin.JetAdvantage
{
    /// <summary>
    /// Jet Advantage Execution Engine
    /// </summary>
    [ToolboxItem(false)]
    public partial class JetAdvantageExecControl : UserControl, IPluginExecutionEngine
    {
        JetAdvantageManager _manager = null;

        private IDevice _device;

        /// <summary>
        /// Constructor for Jet Advantage Execution Engine
        /// </summary>
        public JetAdvantageExecControl()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            var result = new PluginExecutionResult(PluginResult.Passed);
            IAssetInfo printAsset = (IAssetInfo)executionData.Assets.First(); ;
            DeviceWorkflowLogger workflowLogger = new DeviceWorkflowLogger(executionData);
            PrintDeviceInfo printDeviceInfo = (PrintDeviceInfo)executionData.Assets.First();

            var activityData = executionData.GetMetadata<JetAdvantageActivityData>();

            try
            {
                using (_device = GetDevice(printDeviceInfo))
                {
                    FillFormWithActivityData(activityData);
                    workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
                    UpdateStatus("Starting JetAdvantage Pull Printing Application");

                    _manager = new JetAdvantageManager(executionData, workflowLogger, _device, printDeviceInfo);
                    _manager.StatusUpdate += _manager_StatusUpdate;

                    var token = new AssetLockToken(printAsset, TimeSpan.FromHours(1), TimeSpan.FromHours(1));

                    ExecutionServices.CriticalSection.Run(token, () =>
                    {
                        var retryManager = new PluginRetryManager(executionData, UpdateStatus);
                        result = retryManager.Run(() => _manager.RunJetAdvantage());
                    });
                }
                workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);
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
            catch (ScalableTest.Framework.Synchronization.AcquireLockTimeoutException)
            {
                return new PluginExecutionResult(PluginResult.Skipped, "Could not obtain lock on specified device(s).", "Device unavailable.");
            }
            catch (ScalableTest.Framework.Synchronization.HoldLockTimeoutException)
            {
                return new PluginExecutionResult(PluginResult.Error, $"Automation did not complete within {TimeSpan.FromHours(1)}.", "Automation timeout exceeded.");
            }
            finally
            {
                UpdateStatus($"Finished JetAdvantage Printing activity");
            }


            return result;
        }

        private void _manager_StatusUpdate(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }

        private IDevice GetDevice(PrintDeviceInfo printDeviceInfo) => DeviceConstructor.Create(printDeviceInfo);

        protected void UpdateStatus(string text)
        {
            StringBuilder logText = new StringBuilder();
            status_RichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogInfo(text);
                logText.Clear();
                logText.Append(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                logText.Append("  ");
                logText.AppendLine(text);
                status_RichTextBox.AppendText(logText.ToString());
                status_RichTextBox.Refresh();
            });
        }
        /// <summary>
        /// Sets the text and check properties based on the Device and _activityData values in Execution Control
        /// </summary>
        private void FillFormWithActivityData(JetAdvantageActivityData jad)
        {
            // set the label and textbox controls 
            activeDevice_Label.InvokeIfRequired(adl =>
            {
                adl.Text = _device.Address;
                adl.Refresh();
            });

            activeSolution_Label.InvokeIfRequired(c =>
            {
                c.Text = "HP JetAdvantage Pull Printing";
                c.Refresh();
            });

            printAll_CheckBox.InvokeIfRequired(pac =>
            {
                pac.Checked = jad.PrintAllDocuments;
                pac.Refresh();
            });

            deleteAfterPrint_checkbox.InvokeIfRequired(dap =>
            {
                dap.Checked = jad.DeleteAfterPrint;
                dap.Refresh();
            });
        }

    }
}