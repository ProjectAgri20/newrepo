using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.SoidBerg
{
    [ToolboxItem(false)]
    public partial class SoidBergExecutionControl : UserControl, IPluginExecutionEngine
    {
        public SoidBergExecutionControl()
        {
            InitializeComponent();
        }

        public bool IsNumeric(object val, ref int result)
        {
            return int.TryParse((string)val, out result);
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            SoidBergActivityData data = executionData.GetMetadata<SoidBergActivityData>();
            StringBuilder messages = new StringBuilder();
            bool activityFailed = false;
            string snmpOutput = string.Empty;
            if (!string.IsNullOrEmpty(data.SnmpOid))
            {
                foreach (var asset in executionData.Assets)
                {
                    try
                    {
                        Snmp snmp = new Snmp(((PrintDeviceInfo)asset).Address);

                        switch (data.SnmpCommand)
                        {
                            case SnmpCommandTypes.Get:
                                {
                                    snmpOutput = snmp.Get(data.SnmpOid);
                                }
                                break;

                            case SnmpCommandTypes.GetNext:
                                {
                                    snmpOutput = snmp.GetNext(data.SnmpOid).ToString();
                                }
                                break;

                            case SnmpCommandTypes.Set:
                                {
                                    int value = 0;
                                    snmpOutput = IsNumeric(data.SnmpSetValue, ref value) ? snmp.Set(data.SnmpOid, value).ToString() : snmp.Set(data.SnmpOid, data.SnmpSetValue).ToString();
                                }
                                break;
                        }
                        UpdateStatus(string.Format("Device:{0}, SNMP Result:{1}", snmp.Address, snmpOutput));
                        messages.AppendLine(string.Format("{0}: {1}", snmp.Address, snmpOutput));
                    }
                    catch (SnmpException snmpException)
                    {
                        messages.AppendLine(string.Format("Failed for {0}: {1}", ((PrintDeviceInfo)asset).Address,
                            snmpException.Message));
                        activityFailed = true;
                    }
                }
                return activityFailed ? new PluginExecutionResult(PluginResult.Failed, string.Format("Activity failed for one or more devices. {0}", messages.ToString())) : new PluginExecutionResult(PluginResult.Passed, messages.ToString());
            }
            return new PluginExecutionResult(PluginResult.Skipped);
        }

        public void UpdateStatus(string message)
        {
            if (pluginStatus_TextBox.InvokeRequired)
            {
                pluginStatus_TextBox.Invoke(new MethodInvoker(() => UpdateStatus(message)));
                return;
            }

            ExecutionServices.SystemTrace.LogDebug(message);

            pluginStatus_TextBox.SuspendLayout();
            pluginStatus_TextBox.AppendText(message);
            pluginStatus_TextBox.AppendText(Environment.NewLine);

            if (pluginStatus_TextBox.Lines.Length > 1000)
            {
                int location = pluginStatus_TextBox.Text.IndexOf(Environment.NewLine, StringComparison.OrdinalIgnoreCase);
                pluginStatus_TextBox.Text = pluginStatus_TextBox.Text.Remove(0, location + 2);
            }

            pluginStatus_TextBox.SelectionStart = pluginStatus_TextBox.Text.Length;
            pluginStatus_TextBox.ScrollToCaret();
            pluginStatus_TextBox.ResumeLayout();
        }
    }
}