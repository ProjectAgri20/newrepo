using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.WirelessAssociation
{
    [ToolboxItem(false)]
    public partial class WirelessAssociationConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private const AssetAttributes _printerAttribute = AssetAttributes.Printer;

        // Create the definition of the data that will be used by this activity.  It will be
        // instantiated later when the plugin is started up.
        private WirelessAssociationActivityData _activityData = null;

        public WirelessAssociationConfigurationControl()
        {
            InitializeComponent();
            WPA2Authentication_comboBox.DataSource = WirelessAssociationActivityData.AuthenticationList;
            wireless_fieldValidator.RequireAssetSelection(wireless_assetSelectionControl);
            wireless_fieldValidator.RequireValue(SSID_textBox, ssid_label);
            wireless_fieldValidator.RequireSelection(WPA2Authentication_comboBox, authentication_label);
            wireless_fieldValidator.RequireValue(passPhrase_textBox, passphrase_label, ValidationCondition.IfEnabled);

            wireless_assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged?.Invoke(s, e);
            WPA2Authentication_comboBox.SelectedIndexChanged += (s, e) => ConfigurationChanged?.Invoke(s, e);
        }

        #region IPluginConfigurationControl

        // This event can be used to tell the core framework that your edit control has changes.
        // This will cause the framework to prompt the user to save the changes if they
        // move off the control and haven't selected save.
        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            // Initialize the activity data with a default value for the Label
            _activityData = new WirelessAssociationActivityData();
            wireless_assetSelectionControl.Initialize(_printerAttribute);
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Initialize the activity data by deserializing it from an existing copy of configuration information.
            _activityData = configuration.GetMetadata<WirelessAssociationActivityData>();
            wireless_assetSelectionControl.Initialize(configuration.Assets, _printerAttribute);
            SSID_textBox.Text = _activityData.Ssid;
            WPA2Authentication_comboBox.SelectedIndex = WPA2Authentication_comboBox.Items.IndexOf(_activityData.AuthenticationType.GetDescription());
            passPhrase_textBox.Text = _activityData.Passphrase;
            Hex_textBox.Text = _activityData.HexPassphrase;

            PowerCycle_checkBox.Checked = _activityData.PowerCycleRequired;

        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.AuthenticationType = EnumUtil.GetByDescription<AuthenticationMode>(WPA2Authentication_comboBox.Text);
            _activityData.HexPassphrase = Hex_textBox.Text;
            _activityData.Ssid = SSID_textBox.Text;
            _activityData.Passphrase = passPhrase_textBox.Text;
            _activityData.PowerCycleRequired = PowerCycle_checkBox.Checked;

            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = wireless_assetSelectionControl.AssetSelectionData
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            // This is where you can add any validation for your UI and then
            // return the appropriate validation result when saving the data.
            return new PluginValidationResult(wireless_fieldValidator.ValidateAll());
        }

        #endregion IPluginConfigurationControl

        /// <summary>
        /// occurs the WPA selection has changed
        /// </summary>
        /// <param name="sender">the combobox that triggered</param>
        /// <param name="e">event args</param>
        private void WPA2Authentication_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WPA2Authentication_comboBox.SelectedIndex > -1)
            {
                // enable it to cover previous cases of disabled
                passPhrase_textBox.Enabled = true;
                switch (EnumUtil.GetByDescription<AuthenticationMode>(WPA2Authentication_comboBox.Text))
                {
                    case AuthenticationMode.WPAHex:
                        {
                            passphrase_label.Text = "Please enter 64bit Hex/ASCII characters";
                        }
                        break;
                    default:
                        {
                            passphrase_label.Text = string.Empty;
                        }
                        break;
                }


            }
        }


        /// <summary>
        /// Converting Passphrase string to 64 Hex passphrase string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConvertTo64HexPassphrase(object sender, EventArgs e)
        {
            Hex_textBox.Text = WirelessAssociationActivityData.StringToHexConversion(passPhrase_textBox.Text);
        }
    }
}