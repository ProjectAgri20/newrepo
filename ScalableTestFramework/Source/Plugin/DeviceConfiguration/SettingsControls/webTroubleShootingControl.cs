using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    [ToolboxItem(false)]
    public partial class WebTroubleShootingControl : UserControl, IGetSetComponentData
    {
        private WebTroubleShootingData _webTroubleShootingData;
        public bool Modified = false;
        /// <summary>
        /// Occurs when configuration data in this control has changed
        /// Used to Track whether the control has unsaved changes.
        /// </summary>
        public EventHandler ControlComponentChanged;
        public WebTroubleShootingControl()
        {
            InitializeComponent();
            _webTroubleShootingData = new WebTroubleShootingData();
            proxyServer_TextBox.text_Box.Text = string.Empty;
            proxyPort_TextBox.text_Box.Text = string.Empty;
            proxyException_TextBox.text_Box.Text = string.Empty;
            syslogServer_TextControl.text_Box.Text = string.Empty;
            syslogPort_textControl.text_Box.Text = string.Empty;
            autoRecovery_choiceCombo.choice_Combo.DataSource =  ListValues.TrueFalseList;

            AddEventHandlers();
        }

        private void OnControlComponentChanged(object sender, EventArgs e)
        {
            Modified = true;
            ControlComponentChanged?.Invoke(this, e);
        }

        public IComponentData GetData()
        {
            return _webTroubleShootingData;
        }

        public void SetData()
        {
            _webTroubleShootingData.AutoRecoveryMode.Key = autoRecovery_choiceCombo.choice_Combo.SelectedValue.ToString();
            _webTroubleShootingData.AutoRecoveryMode.Value = autoRecovery_choiceCombo.onOff_CheckBox.Checked;

            _webTroubleShootingData.ServerName.Key = proxyServer_TextBox.text_Box.Text;
            _webTroubleShootingData.ServerName.Value = proxyServer_TextBox.onOff_CheckBox.Checked;
            _webTroubleShootingData.ServerPort.Key = proxyPort_TextBox.text_Box.Text;
            _webTroubleShootingData.ServerPort.Value = proxyPort_TextBox.onOff_CheckBox.Checked;
            _webTroubleShootingData.ProxyExceptionList.Key = proxyException_TextBox.text_Box.Text;
            _webTroubleShootingData.ProxyExceptionList.Value = proxyException_TextBox.onOff_CheckBox.Checked;
            _webTroubleShootingData.SyslogServer.Key = syslogServer_TextControl.text_Box.Text;
            _webTroubleShootingData.SyslogServer.Value = syslogServer_TextControl.onOff_CheckBox.Checked;
            _webTroubleShootingData.SyslogPort.Key = syslogPort_textControl.text_Box.Text;
            _webTroubleShootingData.SyslogPort.Value = syslogPort_textControl.onOff_CheckBox.Checked;
        }

        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();
            _webTroubleShootingData = list.OfType<WebTroubleShootingData>().FirstOrDefault();

            autoRecovery_choiceCombo.choice_Combo.SelectedItem = ListValues.TrueFalseList.FirstOrDefault(x => x == _webTroubleShootingData.AutoRecoveryMode.Key);
            autoRecovery_choiceCombo.onOff_CheckBox.Checked = _webTroubleShootingData.AutoRecoveryMode.Value;

            proxyServer_TextBox.text_Box.Text = _webTroubleShootingData.ServerName.Key;
            proxyServer_TextBox.onOff_CheckBox.Checked = _webTroubleShootingData.ServerName.Value;
            proxyPort_TextBox.text_Box.Text = _webTroubleShootingData.ServerPort.Key;
            proxyPort_TextBox.onOff_CheckBox.Checked = _webTroubleShootingData.ServerPort.Value;
            proxyException_TextBox.text_Box.Text = _webTroubleShootingData.ProxyExceptionList.Key;
            proxyException_TextBox.onOff_CheckBox.Checked = _webTroubleShootingData.ProxyExceptionList.Value;

            syslogServer_TextControl.text_Box.Text = _webTroubleShootingData.SyslogServer.Key;
            syslogServer_TextControl.onOff_CheckBox.Checked = _webTroubleShootingData.SyslogServer.Value;
            syslogPort_textControl.text_Box.Text = _webTroubleShootingData.SyslogPort.Key;
            syslogPort_textControl.onOff_CheckBox.Checked = _webTroubleShootingData.SyslogPort.Value;

            AddEventHandlers();
        }

        private void AddEventHandlers()
        {
            autoRecovery_choiceCombo.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            autoRecovery_choiceCombo.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;

            proxyServer_TextBox.text_Box.TextChanged += OnControlComponentChanged;
            proxyServer_TextBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            proxyPort_TextBox.text_Box.TextChanged += OnControlComponentChanged;
            proxyPort_TextBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            proxyException_TextBox.text_Box.TextChanged += OnControlComponentChanged;
            proxyException_TextBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;

            syslogServer_TextControl.text_Box.TextChanged += OnControlComponentChanged;
            syslogServer_TextControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            syslogPort_textControl.text_Box.TextChanged += OnControlComponentChanged;
            syslogPort_textControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
        }

        private void RemoveEventHandlers()
        {
            autoRecovery_choiceCombo.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            autoRecovery_choiceCombo.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;

            proxyServer_TextBox.text_Box.TextChanged -= OnControlComponentChanged;
            proxyServer_TextBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            proxyPort_TextBox.text_Box.TextChanged -= OnControlComponentChanged;
            proxyPort_TextBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            proxyException_TextBox.text_Box.TextChanged -= OnControlComponentChanged;
            proxyException_TextBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;

            syslogServer_TextControl.text_Box.TextChanged -= OnControlComponentChanged;
            syslogServer_TextControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            syslogPort_textControl.text_Box.TextChanged -= OnControlComponentChanged;
            syslogPort_textControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
        }

    }
}
