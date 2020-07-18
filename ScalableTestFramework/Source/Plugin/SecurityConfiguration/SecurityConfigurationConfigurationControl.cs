using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.SecurityConfiguration
{
    [ToolboxItem(false)]
    public partial class SecurityConfigurationUserControl : UserControl, IPluginConfigurationControl
    {
        private SecurityConfigurationActivityData _data;

        public SecurityConfigurationUserControl()
        {
            InitializeComponent();

            encryptionStrength_comboBox.SelectedIndex = 0;
            security_assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            basicSecurity_radioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            enhancedSecurity_radioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            customSecurity_radioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);

            security_fieldValidator.RequireAssetSelection(security_assetSelectionControl);
            security_fieldValidator.RequireValue(snmpv3UserName_textBox, label8, ValidationCondition.IfEnabled);
            security_fieldValidator.RequireValue(authProtocolPassphrase_textBox, label9, ValidationCondition.IfEnabled);
            security_fieldValidator.RequireValue(privacyProtocolPassphrase_textBox, label14, ValidationCondition.IfEnabled);
            security_fieldValidator.RequireSelection(encryptionStrength_comboBox, label15, ValidationCondition.IfEnabled);
            security_fieldValidator.RequireValue(authenticationPassword_textBox, label25, ValidationCondition.IfEnabled);
            SetBindings();
        }

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            _data = new SecurityConfigurationActivityData();
            LoadUi();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<SecurityConfigurationActivityData>();
            security_assetSelectionControl.Initialize(configuration.Assets, Framework.Assets.AssetAttributes.None);

            LoadUi();
        }

        public PluginConfigurationData GetConfiguration()
        {
            if (basicSecurity_radioButton.Checked)
            {
                _data.SecurityType = SecurityConfigurationType.Basic;
            }
            else if (enhancedSecurity_radioButton.Checked)
            {
                _data.SecurityType = SecurityConfigurationType.Enhanced;
            }
            else
            {
                _data.SecurityType = SecurityConfigurationType.Custom;
            }
            _data.EncryptionStrength = encryptionStrength_comboBox.Text;
            _data.SnmpV3Enhanced = snmpv3_checkBox.Checked ? "on" : "off";
            _data.AccessControlIpv4 = accesscontrol_ipAddressControl.Text;
            _data.Mask = mask_ipAddressControl.Text;
            _data.AuthenticationPassword = authenticationPassword_textBox.Text;
            _data.Snmpv3UserName = snmpv3UserName_textBox.Text;
            _data.AuthenticationPassphraseProtocol = authProtocolPassphrase_textBox.Text;
            _data.PrivacyPassphraseProtocol = privacyProtocolPassphrase_textBox.Text;
            _data.Snmpv1v2ReadOnlyAccess = snmpreadonly_checkBox.Checked ? "on" : "off";
            _data.SnmpV1V2 = snmpv1v2custom_checkBox.Checked ? "on" : "off";

            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = security_assetSelectionControl.AssetSelectionData
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(security_fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Enables/Disables control
        /// </summary>
        private void SetBindings()
        {
            custom_panel.DataBindings.Add("Enabled", customSecurity_radioButton, "Checked");
            enhanced_panel.DataBindings.Add("Enabled", enhancedSecurity_radioButton, "Checked");
        }



        private void LoadUi()
        {
            if (_data.SecurityType == SecurityConfigurationType.Basic)
            {
                basicSecurity_radioButton.Checked = true;
            }
            else if (_data.SecurityType == SecurityConfigurationType.Enhanced)
            {
                enhancedSecurity_radioButton.Checked = true;

                snmpv3_checkBox.Checked = _data.SnmpV3Enhanced.Equals("on", StringComparison.OrdinalIgnoreCase);

                snmpv3UserName_textBox.Text = _data.Snmpv3UserName;

                authProtocolPassphrase_textBox.Text = _data.AuthenticationPassphraseProtocol;
                privacyProtocolPassphrase_textBox.Text = _data.PrivacyPassphraseProtocol;

                snmpreadonly_checkBox.Checked = _data.Snmpv1v2ReadOnlyAccess.Equals("on", StringComparison.OrdinalIgnoreCase);
            }
            else if (_data.SecurityType == SecurityConfigurationType.Custom)
            {
                customSecurity_radioButton.Checked = true;

                encryptionStrength_comboBox.SelectedItem = _data.EncryptionStrength;

                snmpv1v2custom_checkBox.Checked = _data.SnmpV1V2.Equals("on", StringComparison.OrdinalIgnoreCase);

                accesscontrol_ipAddressControl.Text = _data.AccessControlIpv4;
                mask_ipAddressControl.Text = _data.Mask;
                authenticationPassword_textBox.Text = _data.AuthenticationPassword;
            }
        }
    }
}