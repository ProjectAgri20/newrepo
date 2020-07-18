using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Assets;
using System.Linq;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Plugin.LocksmithConfiguration.ActivityData;

namespace HP.ScalableTest.Plugin.LocksmithConfiguration
{
    /// <summary>
    /// Control to configure data for a LocksmithConfiguration operations.
    /// </summary>
    public partial class LockSmithConfigurationConfigControl : UserControl, IPluginConfigurationControl
    {
        private LockSmithConfigurationActivityData _activityData;        

        /// <summary>
        /// Initializes a new instance of the <see cref="LockSmithConfigurationConfigControl"/> class.
        /// </summary>
        public LockSmithConfigurationConfigControl()
        {
            InitializeComponent();
            lockSmith_FieldValidator.RequireSelection(lockSmith_ServerComboBox, locksmithServer_Label);
            lockSmith_FieldValidator.RequireValue(user_TextBox, adminUser_Label);
            lockSmith_FieldValidator.RequireValue(password_TextBox, adminPassword_Label);
            browserType_ComboBox.DataSource = EnumUtil.GetDescriptions<BrowserType>().ToList();
            discoveryType_ComboBox.DataSource = EnumUtil.GetDescriptions<PrinterDiscovery>().Distinct().ToList();
            lockSmith_ServerComboBox.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
        }

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.LockSmithUser = user_TextBox.Text;
            _activityData.LockSmithPassword = password_TextBox.Text;
            _activityData.EWSAdminPassword = ewsAdminPassword_TextBox.Text;
            _activityData.GroupName = groupName_TextBox.Text;
            if (printerDiscovery_CheckBox.Checked)
            {
                _activityData.DeviceDiscovery = printerDiscovery_CheckBox.Checked;
                switch (EnumUtil.GetByDescription<PrinterDiscovery>(discoveryType_ComboBox.SelectedItem.ToString()))
                {
                    case PrinterDiscovery.ManualIPAddress:
                        _activityData.Adddevice = PrinterDiscovery.ManualIPAddress;
                        _activityData.ManualIPAddress = manualIPAddress_IpAddressControl.Text;
                        break;

                    case PrinterDiscovery.DeviceListFile:
                        _activityData.Adddevice = PrinterDiscovery.DeviceListFile;
                        _activityData.DeviceListPath = ipAddressFile_textBox.Text.ToString();
                        break;

                    case PrinterDiscovery.AssetInventory:
                        _activityData.Adddevice = PrinterDiscovery.AssetInventory;
                        break;

                    case PrinterDiscovery.AutomaticHops:
                    case PrinterDiscovery.AutomaticRange:
                        if (numberOfNetworkHops_RadioButton.Checked)
                        {
                            _activityData.Adddevice = PrinterDiscovery.AutomaticHops;
                            _activityData.NumberOfHops = networkHop_NumericUpDown.Value;
                        }
                        if (ipRange_RadioButton.Checked)
                        {
                            _activityData.Adddevice = PrinterDiscovery.AutomaticRange;
                            _activityData.StartIPAddress = automaticStartIPAddress_IpAddressControl.Text;
                            _activityData.EndIPAddress = automaticEndIPAddress_IpAddressControl.Text;
                        }
                        break;
                }
            }
            else
            {
                _activityData.DeviceDiscovery = printerDiscovery_CheckBox.Checked;
                _activityData.Adddevice = PrinterDiscovery.None;
            }
            if (importAndApplyPolicy_CheckBox.Checked && policyConfiguration_GroupBox.Enabled == true && tasekType_GroupBox.Enabled == true)
            {
                _activityData.PolicyConfiguration = importAndApplyPolicy_CheckBox.Checked;
                _activityData.AssessOnly = assessOnly_RadioButton.Checked;
                _activityData.AssessandRemediate = assessAndRemediate_RadioButton.Checked;
                if (existingPolicyName_CheckBox.Checked)
                {
                    _activityData.ExisintingPolicyCheckbox = existingPolicyName_CheckBox.Checked;
                    _activityData.ExistingPolicyName = existingPolicyName_TextBox.Text;
                    _activityData.ValidatePolicyPath = validatePolicyPath_CheckBox.Checked;
                }
                else
                {
                    _activityData.ValidatePolicyPath = validatePolicyPath_CheckBox.Checked;
                    _activityData.PolicyFilePath = policyPath_TextBox.Text;
                    _activityData.PolicyPassword = policyPassword_TextBox.Text;
                    _activityData.ExistingPolicyName = String.Empty;
                    _activityData.ExisintingPolicyCheckbox = existingPolicyName_CheckBox.Checked;
                }
            }
            else
            {
                _activityData.PolicyConfiguration = importAndApplyPolicy_CheckBox.Checked;
            }
            if (generateReports_CheckBox.Checked)
            {
                _activityData.ReportExtraction = generateReports_CheckBox.Checked;
            }
            else
            {
                _activityData.ReportExtraction = generateReports_CheckBox.Checked;
            }
            if (browserType_ComboBox.Text == "Internet Explorer")
            {
                _activityData.Browser = BrowserType.InternetExplorer;
            }
            else if (browserType_ComboBox.Text == "Google Chrome")
            {
                _activityData.Browser = BrowserType.GoogleChrome;
            }
            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
                Servers = new ServerSelectionData(lockSmith_ServerComboBox.SelectedServer),
            };
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new LockSmithConfigurationActivityData();
            lockSmith_ServerComboBox.Initialize("LockSmith");
            discoveryType_ComboBox.SelectedIndex = 0;
            browserType_ComboBox.SelectedIndex = 0;
            toolTip.SetToolTip(user_TextBox, "Format: Domain\\Username");
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
             Guid _selectedServer = Guid.Empty;
            _activityData = configuration.GetMetadata<LockSmithConfigurationActivityData>();
            _selectedServer = configuration.Servers.SelectedServers.FirstOrDefault();
            lockSmith_ServerComboBox.Initialize(_selectedServer, "Locksmith");
            user_TextBox.Text = _activityData.LockSmithUser;
            password_TextBox.Text = _activityData.LockSmithPassword;
            ewsAdminPassword_TextBox.Text = _activityData.EWSAdminPassword;
            validatePolicyPath_CheckBox.Checked = _activityData.ValidatePolicyPath;
            existingPolicyName_CheckBox.Checked = _activityData.ExisintingPolicyCheckbox;
            policyPath_TextBox.Text = _activityData.PolicyFilePath;
            policyPassword_TextBox.Text = _activityData.PolicyPassword;
            groupName_TextBox.Text = _activityData.GroupName;
            ipAddressFile_textBox.Text = _activityData.DeviceListPath;
            automaticStartIPAddress_IpAddressControl.Text = _activityData.StartIPAddress;
            automaticEndIPAddress_IpAddressControl.Text = _activityData.EndIPAddress;
            manualIPAddress_IpAddressControl.Text = _activityData.ManualIPAddress;
            browserType_ComboBox.SelectedItem = _activityData.Browser;
            existingPolicyName_TextBox.Text = _activityData.ExistingPolicyName;
            printerDiscovery_CheckBox.Checked = _activityData.DeviceDiscovery;
            importAndApplyPolicy_CheckBox.Checked = _activityData.PolicyConfiguration;
            generateReports_CheckBox.Checked = _activityData.ReportExtraction;
            discoveryType_ComboBox.Text = _activityData.DeviceDiscovery.ToString();
            assessOnly_RadioButton.Checked = _activityData.AssessOnly;
            assessAndRemediate_RadioButton.Checked = _activityData.AssessandRemediate;
            assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.ControlPanel);
            switch (_activityData.Adddevice)
            {
                case PrinterDiscovery.ManualIPAddress:
                    printerDiscovery_CheckBox.Checked = true;
                    discoveryType_ComboBox.SelectedItem = EnumUtil.GetDescription(PrinterDiscovery.ManualIPAddress);
                    break;

                case PrinterDiscovery.DeviceListFile:
                    printerDiscovery_CheckBox.Checked = true;
                    discoveryType_ComboBox.SelectedItem = EnumUtil.GetDescription(PrinterDiscovery.DeviceListFile);
                    break;

                case PrinterDiscovery.AssetInventory:
                    assetSelectionControl.Enabled = true;
                    discoveryType_ComboBox.SelectedItem = EnumUtil.GetDescription(PrinterDiscovery.AssetInventory);
                    break;

                case PrinterDiscovery.AutomaticHops:
                    discoveryType_ComboBox.SelectedItem = EnumUtil.GetDescription(PrinterDiscovery.AutomaticHops);
                    printerDiscovery_CheckBox.Checked = true;
                    numberOfNetworkHops_RadioButton.Checked = true;
                    ipRange_RadioButton.Checked = false;
                    break;

                case PrinterDiscovery.AutomaticRange:
                    discoveryType_ComboBox.SelectedItem = EnumUtil.GetDescription(PrinterDiscovery.AutomaticRange);
                    printerDiscovery_CheckBox.Checked = true;
                    ipRange_RadioButton.Checked = true;
                    numberOfNetworkHops_RadioButton.Checked = false;
                    break;
            }
                        
            if (_activityData.PolicyConfiguration)
            {
                importAndApplyPolicy_CheckBox.Checked = true;
            }
                        
            if (_activityData.ReportExtraction )
            {
                generateReports_CheckBox.Checked = true;
            }

            switch (_activityData.Browser)
            {
                case BrowserType.InternetExplorer:
                    browserType_ComboBox.SelectedItem = EnumUtil.GetDescription(BrowserType.InternetExplorer);
                    break;
                case BrowserType.GoogleChrome:
                    browserType_ComboBox.SelectedItem = EnumUtil.GetDescription(BrowserType.GoogleChrome);
                    break;
            }
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(lockSmith_FieldValidator.ValidateAll());
        }

        private void numberOfNetworkHops_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            automaticStartIPAddress_IpAddressControl.Enabled = false;
            automaticEndIPAddress_IpAddressControl.Enabled = false;
            networkHop_NumericUpDown.Enabled = true;
        }

        private void iPRange_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            automaticStartIPAddress_IpAddressControl.Enabled = true;
            automaticEndIPAddress_IpAddressControl.Enabled = true;
            networkHop_NumericUpDown.Enabled = false;
            lockSmith_FieldValidator.RequireCustom(automaticStartIPAddress_IpAddressControl, () => (!automaticStartIPAddress_IpAddressControl.Enabled || automaticStartIPAddress_IpAddressControl.IsValidIPAddress()), "Enter Valid IP Address");
            lockSmith_FieldValidator.RequireCustom(automaticEndIPAddress_IpAddressControl, () => (!automaticEndIPAddress_IpAddressControl.Enabled || automaticEndIPAddress_IpAddressControl.IsValidIPAddress()), "Enter Valid IP Address");
        }

        private void discoveryType_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            switch (EnumUtil.GetByDescription<PrinterDiscovery>(discoveryType_ComboBox.SelectedItem.ToString()))
            {
                case PrinterDiscovery.ManualIPAddress:
                    lockSmith_FieldValidator.RequireCustom(manualIPAddress_IpAddressControl, () => (!manualIPAddress_IpAddressControl.Enabled || manualIPAddress_IpAddressControl.IsValidIPAddress()), "Enter Valid IP Address");
                    manualDiscovery_GroupBox.Enabled = true;
                    automaticDiscovery_GroupBox.Enabled = false;
                    addFromFile_GroupBox.Enabled = false;
                    assetSelectionControl.Enabled = false;
                    lockSmith_FieldValidator.Remove(assetSelectionControl);
                    break;

                case PrinterDiscovery.DeviceListFile:
                    lockSmith_FieldValidator.RequireCustom(ipAddressFile_textBox, () => (!ipAddressFileBrowse_button.Enabled || !ipAddressFile_textBox.Enabled || ipAddressFile_textBox.Text != string.Empty), "Device List Path not specified");
                    addFromFile_GroupBox.Enabled = true;
                    manualDiscovery_GroupBox.Enabled = false;
                    automaticDiscovery_GroupBox.Enabled = false;
                    assetSelectionControl.Enabled = false;
                    lockSmith_FieldValidator.Remove(assetSelectionControl);
                    break;

                case PrinterDiscovery.AssetInventory:
                    lockSmith_FieldValidator.RequireAssetSelection(assetSelectionControl, "device", printerDiscovery_CheckBox);
                    assetSelectionControl.Enabled = true;
                    addFromFile_GroupBox.Enabled = false;
                    manualDiscovery_GroupBox.Enabled = false;
                    automaticDiscovery_GroupBox.Enabled = false;
                    break;

                case PrinterDiscovery.AutomaticHops:
                case PrinterDiscovery.AutomaticRange:
                    manualDiscovery_GroupBox.Enabled = false;
                    automaticDiscovery_GroupBox.Enabled = true;
                    addFromFile_GroupBox.Enabled = false;
                    numberOfNetworkHops_RadioButton.Checked = true;
                    assetSelectionControl.Enabled = false;
                    lockSmith_FieldValidator.Remove(assetSelectionControl);
                    break;
            }
        }

        private void ipAddressFileBrowse_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "IPAddress File | *.xml; *.txt";
            DialogResult dr = dialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                ipAddressFile_textBox.Text = dialog.FileName;
            }

        }

        private void importAndApplyPolicy_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            policyConfiguration_GroupBox.Enabled = importAndApplyPolicy_CheckBox.Checked ? true : false;
            tasekType_GroupBox.Enabled = importAndApplyPolicy_CheckBox.Checked ? true : false;
            assessOnly_RadioButton.Checked = assessAndRemediate_RadioButton.Checked ? false : true;
            lockSmith_FieldValidator.RequireCustom(policyPath_TextBox, () => (!importAndApplyPolicy_CheckBox.Checked || existingPolicyName_CheckBox.Checked || policyPath_TextBox.Text != string.Empty), "Provide a Policy path");
            lockSmith_FieldValidator.RequireCustom(policyPassword_TextBox, () => (!importAndApplyPolicy_CheckBox.Checked || existingPolicyName_CheckBox.Checked || policyPassword_TextBox.Text != string.Empty), "Provide password for the policy");
        }

        private void printerDiscovery_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            deviceDiscovery_GroupBox.Enabled = printerDiscovery_CheckBox.Checked ? true : false;
        }

        private void existingPolicyName_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            policyPath_TextBox.Enabled = existingPolicyName_CheckBox.Checked ? false : true;
            validatePolicyPath_CheckBox.Enabled = existingPolicyName_CheckBox.Checked ? false : true;
            policyPassword_TextBox.Enabled = existingPolicyName_CheckBox.Checked ? false : true;
            validatePolicyPath_CheckBox.Checked = existingPolicyName_CheckBox.Checked ? false : true;
            existingPolicyName_TextBox.Enabled = existingPolicyName_CheckBox.Checked ? true : false;
            lockSmith_FieldValidator.RequireCustom(existingPolicyName_CheckBox, () => (!existingPolicyName_CheckBox.Checked || existingPolicyName_TextBox.Text != string.Empty), "Provide a Policy name");
        }
    }
}
