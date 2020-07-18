using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.EdtIntervention
{
    /// <summary>
    /// EDT Intervention Execution Engine
    /// </summary>
    [ToolboxItem(false)]
    public partial class EdtInterventionExecutionEngine : UserControl, IPluginExecutionEngine
    {
        private PluginExecutionData _executionData;
        private readonly StringBuilder _logText = new StringBuilder();

        public EdtInterventionExecutionEngine()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            DialogResult result;
            _executionData = executionData;
            EdtInterventionActivityData pluginData = executionData.GetMetadata<EdtInterventionActivityData>();

            switch (pluginData.TestType)
            {
                case EdtTestType.None:
                default:
                    {
                        result = MessageBox.Show(pluginData.AlertMessage, @"EDT", MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    break;

                case EdtTestType.PowerBoot:
                    {
                        result = PowerBoot(pluginData);
                    }
                    break;

                case EdtTestType.OperationReliability:
                    {
                        result = OperationReliability(pluginData);
                    }
                    break;

                case EdtTestType.FIM:
                {
                    result = FirmwareUpgrade(pluginData);
                }
                    break;
            }

            switch (result)
            {
                case DialogResult.No:
                    DialogResult showFaultEventHandler = MessageBox.Show(@"Do you wish to enter a Fault Event?", @"Fault Event Handler", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    if (showFaultEventHandler == DialogResult.Yes)
                    {
                        FaultHandler eventHandler = new FaultHandler(executionData);
                        eventHandler.ShowDialog();
                    }
                    return new PluginExecutionResult(PluginResult.Failed, pluginData.AlertMessage);

                case DialogResult.Yes:
                    return new PluginExecutionResult(PluginResult.Passed, pluginData.AlertMessage);

                default:
                    return new PluginExecutionResult(PluginResult.Skipped, pluginData.AlertMessage);
            }
        }

        private DialogResult FirmwareUpgrade(EdtInterventionActivityData pluginData)
        {
            var log = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashMethod", "Web Jet Admin");
            ExecutionServices.DataLogger.Submit(log);
            log = new ActivityExecutionDetailLog(_executionData, "FirmwareFlashUpgrade", "true");
            ExecutionServices.DataLogger.Submit(log);

            MessageBox.Show(
                @"Use WJA, and begin the firmware upgrade for the device with the target firmware file. Press OK once the firmware upgrade has started.",@"EDT FIM", MessageBoxButtons.OK, 
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            log = new ActivityExecutionDetailLog(_executionData, "FirmwareUpdateBegin","WJA",DateTimeOffset.Now);
            ExecutionServices.DataLogger.Submit(log);

            var result =
                MessageBox.Show(
                    @"Wait for the firmware upgrade process to complete, Press Yes to continue once the device is in Ready state.",
                    @"EDT FIM", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            log = new ActivityExecutionDetailLog(_executionData, "FirmwareUpdateEnd", "WJA", DateTimeOffset.Now);
            ExecutionServices.DataLogger.Submit(log);

            return result;
        }

        private DialogResult PowerBoot(EdtInterventionActivityData pluginData)
        {
            BootMethod bootMethod = EnumUtil.Parse<BootMethod>(pluginData.TestMethod);
            //first let's prompt the user what we need to do for shutting down
            switch (bootMethod)
            {
                case BootMethod.Dirty:
                    MessageBox.Show(
                        @"Please remove the power cable to perform a dirty power shutdown, Press OK to continue.", @"EDT PowerBoot - Shut Down", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;

                case BootMethod.Hard:
                    MessageBox.Show(@"Press and Hold the power button until the device shuts down. Press OK to continue.", @"EDT PowerBoot - Shut Down", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;

                case BootMethod.Soft:
                    MessageBox.Show(
                        @"Press the power button once to initiate a shutdown of the device. Press OK to continue.", @"EDT PowerBoot - Shut Down", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
            }
            var log = new ActivityExecutionDetailLog(_executionData, "PowerBootMethod", bootMethod.GetDescription());
            ExecutionServices.DataLogger.Submit(log);
            UpdateStatus($"Device has been shutdown by {bootMethod.GetDescription()} method.");
            UpdateStatus("Waiting for 30 seconds before continuing...");
            //let's wait 30 seconds to continue
            Thread.Sleep(TimeSpan.FromSeconds(30));

            MessageBox.Show(@"Please reboot the device now", @"EDT PowerBoot - Reboot", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            log = new ActivityExecutionDetailLog(_executionData, "PowerBootStart", "Device Reboot Start");
            ExecutionServices.DataLogger.Submit(log);
            UpdateStatus("Device has been Powered up.");
            DateTime startTime = DateTime.UtcNow;
            var result = MessageBox.Show(@"Please wait until the device has booted and reached Ready state. Press Yes for successful boot or No for a failure.",
                @"EDT PowerBoot - Ready State", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            if (result == DialogResult.Yes)
            {
                log = new ActivityExecutionDetailLog(_executionData, "PowerBootEnd", "Device Reboot End");
                ExecutionServices.DataLogger.Submit(log);
                UpdateStatus($"Device has reached Ready State. It took {(DateTime.UtcNow - startTime).TotalSeconds} seconds for Device to boot up.");
            }

            return result;
        }

        private DialogResult OperationReliability(EdtInterventionActivityData pluginData)
        {
            SleepWakeMethod sleepWakeMethod = EnumUtil.Parse<SleepWakeMethod>(pluginData.TestMethod);
            WakeMethod wakeMethod;
            var result = DialogResult.None;
            //first let's prompt the user how we need to put the device for Sleep
            switch (sleepWakeMethod)
            {
                case SleepWakeMethod.Sleep:
                    if (pluginData.AlertMessage.Length > 5)
                        MessageBox.Show(pluginData.AlertMessage, @"Prerequisites", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    result = MessageBox.Show(
                        @"Put the device into sleep. Press Yes once device goes to sleep.", @"Sleep", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
                case SleepWakeMethod.Wake:
                    if (pluginData.AlertMessage.Length > 5)
                        MessageBox.Show(pluginData.AlertMessage, @"Prerequisites", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    result = MessageBox.Show(
                        @"Did the device woke up by " + pluginData.WakeMethod + " and behaved as expected?.", @"Wake up", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
            }

            if (sleepWakeMethod != SleepWakeMethod.Sleep)
            {
                wakeMethod = EnumUtil.Parse<WakeMethod>(pluginData.WakeMethod);
                var log = new ActivityExecutionDetailLog(_executionData, "WakeMethod", wakeMethod.GetDescription());
                ExecutionServices.DataLogger.Submit(log);

                if (result == DialogResult.Yes)
                {
                    UpdateStatus($"Device woken up by {wakeMethod.GetDescription()}");
                }
                else
                {

                    UpdateStatus($"Device didn't wake up. Entering a fault event");
                    result = DialogResult.No;
                }
            }
            else
            {
                var log = new ActivityExecutionDetailLog(_executionData, "SleepMethod", sleepWakeMethod.GetDescription());
                ExecutionServices.DataLogger.Submit(log);
                if(result == DialogResult.Yes)
                    UpdateStatus("Device has entered Almost 1 Watt sleep.");
            }
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
    }
}