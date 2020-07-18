using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.SafeComSimulation
{
    [ToolboxItem(false)]
    public partial class SafeComSimulationConfigControl : UserControl, IPluginConfigurationControl
    {
        private PluginConfigurationData _configData = null;
        private SafeComSimulationActivityData _activityData = null;
        private AssetInfo _selectedAsset = null;

        public SafeComSimulationConfigControl()
        {
            InitializeComponent();

            // For this to work, the tag of each radio button must be set to the corresponding SafeComAuthenticationMode.
            userPassword_RadioButton.Tag = SafeComAuthenticationMode.NameAndPassword;
            userPin_RadioButton.Tag = SafeComAuthenticationMode.NameAndPin;
            cardPin_RadioButton.Tag = SafeComAuthenticationMode.CardAndPin;
            windows_RadioButton.Tag = SafeComAuthenticationMode.WindowsCredentials;

            // Set up Validation
            fieldValidator.RequireSelection(safecom_ServerComboBox, server_Label);
            fieldValidator.RequireValue(card_TextBox, card_Label, cardPin_RadioButton);
            fieldValidator.RequireCustom(address_TextBox, () => ValidateMacAddress(address_TextBox.Text), FieldValidator.BuildCustomMessage(address_Label, "{0} is not valid."));
            fieldValidator.RequireValue(asset_TextBox, asset_Label);

            // Set up Change notification
            safecom_ServerComboBox.SelectionChanged += OnConfigurationChanged;
            printAll_CheckBox.CheckedChanged += OnConfigurationChanged;
            address_TextBox.TextChanged += OnConfigurationChanged;
            asset_TextBox.TextChanged += OnConfigurationChanged;
        }

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new SafeComSimulationActivityData();
            //assetSelectionControl.Initialize(); //This will be added soon
            safecom_ServerComboBox.Initialize("SafeCom");
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Initialize the activity data by deserializing it from the configuration information.
            _configData = configuration;
            _activityData = _configData.GetMetadata<SafeComSimulationActivityData>();
            _selectedAsset = ConfigurationServices.AssetInventory.GetAssets(_configData.Assets.SelectedAssets).FirstOrDefault();

            safecom_ServerComboBox.Initialize(_configData.Servers.SelectedServers.FirstOrDefault(), "SafeCom");
            //assetSelectionControl.Initialize(configuration.Assets);

            // Brute force initialization
            card_TextBox.Text = _activityData.CardNumber;
            asset_TextBox.Text = _selectedAsset.AssetId;
            address_TextBox.Text = _activityData.AssetMacAddress;
            printAll_CheckBox.Checked = _activityData.PullAllDocuments;
            this.AuthenticationMode = _activityData.SafeComAuthenticationMode;
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.CardNumber = card_TextBox.Text;
            _activityData.SafeComAuthenticationMode = this.AuthenticationMode;
            _activityData.PullAllDocuments = printAll_CheckBox.Checked;
            _activityData.AssetMacAddress = address_TextBox.Text;

            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = GetSelectedAssetData(),
                Servers = new ServerSelectionData(safecom_ServerComboBox.SelectedServer)
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            PluginValidationResult result = new PluginValidationResult(fieldValidator.ValidateAll());
            return result;
        }

        private AssetSelectionData GetSelectedAssetData()
        {
            List<AssetInfo> assets = new List<AssetInfo>()
            {
                _selectedAsset
            };
            return new AssetSelectionData(assets);
        }

        /// <summary>
        /// Property to expose the AuthenticationMode
        /// </summary>
        private SafeComAuthenticationMode AuthenticationMode
        {
            get
            {
                foreach (Control control in authentication_GroupBox.Controls)
                {
                    RadioButton radioButton = control as RadioButton;
                    if (radioButton != null && radioButton.Checked)
                    {
                        return (SafeComAuthenticationMode)control.Tag;
                    }
                }
                return SafeComAuthenticationMode.NameAndPassword;
            }
            set
            {
                foreach (Control control in authentication_GroupBox.Controls)
                {
                    if (control.Tag != null && (SafeComAuthenticationMode)control.Tag == value)
                    {
                        ((RadioButton)control).Checked = true;
                        break;
                    }
                }
            }
        }

        private void Authentication_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            card_Label.Enabled = cardPin_RadioButton.Checked;
            card_TextBox.Enabled = cardPin_RadioButton.Checked;
            _activityData.SafeComAuthenticationMode = AuthenticationMode;
            OnConfigurationChanged(sender, e);
        }

        private void selectAsset_Button_Click(object sender, EventArgs e)
        {
            using (AssetSelectionForm assetSelectionForm = new AssetSelectionForm(AssetAttributes.Printer, asset_TextBox.Text, false))
            {
                assetSelectionForm.ShowDialog(this);
                if (assetSelectionForm.DialogResult == DialogResult.OK)
                {
                    _selectedAsset = assetSelectionForm.SelectedAssets.FirstOrDefault();
                    asset_TextBox.Text = _selectedAsset.AssetId;
                }
            }
        }

        /// <summary>
        /// Validates the device MAC address. Must be 12 hexadecimal digits, and may optionally include colons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool ValidateMacAddress(string textBoxValue)
        {
            string formattedValue = textBoxValue.ToUpperInvariant().Replace(":", string.Empty);

            Regex macAddress = new Regex("^[0-9A-F]{12}$");
            return macAddress.IsMatch(formattedValue);
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(this, e);
                //System.Diagnostics.Debug.WriteLine("Config Changed.");
            }
        }

    }
}
