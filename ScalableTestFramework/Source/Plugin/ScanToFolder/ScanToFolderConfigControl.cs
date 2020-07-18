using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.Scan;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ScanToFolder
{
    /// <summary>
    /// Control to configure data for a Scan To Folder activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ScanToFolderConfigControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.1";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;
        private ScanOptions _scanOptions = new ScanOptions();

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToFolderConfigControl"/> class.
        /// </summary>
        public ScanToFolderConfigControl()
        {
            InitializeComponent();

            fieldValidator.RequireValue(folder_TextBox, "Network Folder", networkFolder_RadioButton);
            fieldValidator.RequireValue(quickSet_TextBox, "Named QuickSet", quickSet_RadioButton);
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");

            networkFolder_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            folder_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            quickSet_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            useOcr_CheckBox.CheckedChanged += useOcr_CheckBox_CheckedChanged;
            applyCredential_Checkbox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            pageCount_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            singleFolder_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            multipleFoldersAuto_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            multipleFoldersExact_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            destinations_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            usesDigitalSendServer_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            digitalSendServer_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            OptionalRadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            GenerateRadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            RestrictRadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);

            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpacDra, AuthenticationProvider.HpacDra.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpacIrm, AuthenticationProvider.HpacIrm.GetDescription()));            
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.DSS, AuthenticationProvider.DSS.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Auto, AuthenticationProvider.Auto.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.SafeCom, AuthenticationProvider.SafeCom.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Equitrac, AuthenticationProvider.Equitrac.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));
            comboBox_AuthProvider.DataSource = authProviders;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            ConfigureControls(new ScanToFolderData());

            assetSelectionControl.Initialize(_deviceAttributes);
            
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            ScanToFolderData activityData = configuration.GetMetadata<ScanToFolderData>(ConverterProvider.GetMetadataConverters());
            ConfigureControls(activityData);

            assetSelectionControl.Initialize(configuration.Assets, _deviceAttributes);
            assetSelectionControl.AdfDocuments = configuration.Documents;
            
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            ScanToFolderData data = new ScanToFolderData()
            {
                FolderPath = folder_TextBox.Text,
                QuickSetName = quickSet_TextBox.Text,
                UseQuickset = quickSet_RadioButton.Checked,
                UseOcr = useOcr_CheckBox.Checked,
                ApplyCredentialsOnVerification = applyCredential_Checkbox.Checked,
                AutomationPause = assetSelectionControl.AutomationPause,
               
                ImagePreviewOptions = OptionalRadioButton.Checked ? 0 : GenerateRadioButton.Checked ? 1 : RestrictRadioButton.Checked ? 2 : 0,
                ScanOptions=_scanOptions,
                ApplicationAuthentication = radioButton_ScanToFolder.Checked,
                AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue
            };
            data.ScanOptions.PageCount = (int)pageCount_NumericUpDown.Value;
            data.ScanOptions.LockTimeouts = lockTimeoutControl.Value;
            data.ScanOptions.UseAdf = assetSelectionControl.UseAdf;
            if (data.UseOcr)
            {
                data.ScanOptions.FileType = DeviceAutomation.FileType.SearchablePdfOcr;
            }

            if (singleFolder_RadioButton.Checked)
            {
                data.DestinationType = "Folder";
                data.DestinationCount = 1;
            }
            else
            {
                data.DestinationType = "Folder Multi";
                data.DestinationCount = (multipleFoldersAuto_RadioButton.Checked ? 0 : (int)destinations_NumericUpDown.Value);
            }

            if (usesDigitalSendServer_CheckBox.Checked)
            {
                data.DigitalSendServer = digitalSendServer_TextBox.Text;
            }
            else
            {
                data.DigitalSendServer = null;
            }

            return new PluginConfigurationData(data, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData,
                Documents = assetSelectionControl.AdfDocuments
            };
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        private void ConfigureControls(ScanToFolderData data)
        {
            folder_TextBox.Text = data.FolderPath;
            quickSet_TextBox.Text = data.QuickSetName;
            quickSet_RadioButton.Checked = data.UseQuickset;
            networkFolder_RadioButton.Checked = !data.UseQuickset;
            useOcr_CheckBox.Checked = data.UseOcr;
            applyCredential_Checkbox.Checked = data.ApplyCredentialsOnVerification;
            pageCount_NumericUpDown.Value = data.ScanOptions.PageCount;
            
            OptionalRadioButton.Checked = data.ImagePreviewOptions == 0;
            GenerateRadioButton.Checked = data.ImagePreviewOptions == 1;
            RestrictRadioButton.Checked = data.ImagePreviewOptions == 2;

            lockTimeoutControl.Initialize(data.ScanOptions.LockTimeouts);

            assetSelectionControl.AutomationPause = data.AutomationPause;
            assetSelectionControl.UseAdf = data.ScanOptions.UseAdf;

            // Determine what logging option should be selected
            if (data.DestinationType == "Folder Multi")
            {
                if (data.DestinationCount > 0)
                {
                    multipleFoldersExact_RadioButton.Checked = true;
                    destinations_NumericUpDown.Value = data.DestinationCount;
                }
                else
                {
                    multipleFoldersAuto_RadioButton.Checked = true;
                }
            }
            else
            {
                singleFolder_RadioButton.Checked = true;
            }

            // Determine whether the DSS server should be filled in
            if (!string.IsNullOrEmpty(data.DigitalSendServer))
            {
                digitalSendServer_TextBox.Text = data.DigitalSendServer;
                usesDigitalSendServer_CheckBox.Checked = true;
            }
            else
            {
                usesDigitalSendServer_CheckBox.Checked = false;
            }
            _scanOptions = data.ScanOptions;
            
            if (data.ApplicationAuthentication)
            {
                radioButton_ScanToFolder.Checked = true;
            }
            else
            {
                radioButton_SignInButton.Checked = true;
            }
            comboBox_AuthProvider.SelectedValue = data.AuthProvider;
        }

        private void networkFolder_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            folder_TextBox.Enabled = networkFolder_RadioButton.Checked;
            quickSet_TextBox.Enabled = !networkFolder_RadioButton.Checked;
        }

        private void multipleFoldersExact_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            destinations_NumericUpDown.Enabled = multipleFoldersExact_RadioButton.Checked;
        }

        private void usesDigitalSendServer_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            digitalSendServerName_Label.Enabled = digitalSendServer_TextBox.Enabled = usesDigitalSendServer_CheckBox.Checked;
        }

        private void ScanPreferences_Button_Click(object sender, EventArgs e)
            {
            using (var scanOptionsForm = new ScanOptionsForm(_scanOptions))
                {
                if (scanOptionsForm.ShowDialog() == DialogResult.OK)
                    {
                    _scanOptions = scanOptionsForm.ScanOption;
                    }
                }
            }

        private void useOcr_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (useOcr_CheckBox.Checked)
            {
                _scanOptions.FileType = DeviceAutomation.FileType.SearchablePdfOcr;
            }
            else
            {
                _scanOptions.FileType = DeviceAutomation.FileType.DeviceDefault;
            }

            ConfigurationChanged(sender, e);
        }
    }
}
