using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ScanToEmail
{
    /// <summary>
    /// Control to configure data for a Scan To Email activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ScanToEmailConfigControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.1";
        private const AssetAttributes DeviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;
        private ScanOptions _scanOptions = new ScanOptions();

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToEmailConfigControl"/> class.
        /// </summary>
        public ScanToEmailConfigControl()
        {
            InitializeComponent();

            fieldValidator.RequireValue(email_ComboBox, emailDestination_Label);
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");

            email_ComboBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            addressSource_comboBox.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
            useOcr_CheckBox.CheckedChanged += useOcr_CheckBox_CheckedChanged;
            pageCount_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            usesDigitalSendServer_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            digitalSendServer_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            emailApp_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            launchFromApp_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            launchFromHome_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            quickSet_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            quickSet_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
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
            ConfigureControls(new ScanToEmailData());

            assetSelectionControl.Initialize(DeviceAttributes);
            lockTimeoutControl.Initialize(new LockTimeoutData(TimeSpan.FromMinutes(2),TimeSpan.FromMinutes(5) ));
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            ScanToEmailData activityData = configuration.GetMetadata<ScanToEmailData>(ConverterProvider.GetMetadataConverters());
            ConfigureControls(activityData);

            assetSelectionControl.Initialize(configuration.Assets, DeviceAttributes);
            assetSelectionControl.AdfDocuments = configuration.Documents;
            lockTimeoutControl.Initialize(activityData.ScanOptions.LockTimeouts);
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            ScanToEmailData data = new ScanToEmailData()
            {
                EmailAddress = email_ComboBox.Text,
                AddressSource = addressSource_comboBox.SelectedItem.ToString(),
                UseOcr = useOcr_CheckBox.Checked,
                
                AutomationPause = assetSelectionControl.AutomationPause,
                
                QuickSetName = quickSet_TextBox.Text,
                UseQuickset = quickSet_RadioButton.Checked,
                LaunchQuicksetFromApp = launchFromApp_RadioButton.Checked,
                ImagePreviewOptions = OptionalRadioButton.Checked ? 0 : GenerateRadioButton.Checked ? 1 : RestrictRadioButton.Checked ? 2 : 0,
                ScanOptions = _scanOptions,
                ApplicationAuthentication = radioButton_ScanToEmail.Checked,
                AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue
            };
            data.ScanOptions.PageCount = (int)pageCount_NumericUpDown.Value;
            data.ScanOptions.LockTimeouts = lockTimeoutControl.Value;
            data.ScanOptions.UseAdf = assetSelectionControl.UseAdf;

            if (data.UseOcr)
            {
                data.ScanOptions.FileType = DeviceAutomation.FileType.SearchablePdfOcr;
            }

            data.DigitalSendServer = usesDigitalSendServer_CheckBox.Checked ? digitalSendServer_TextBox.Text : null;

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

        private void ConfigureControls(ScanToEmailData data)
        {
            email_ComboBox.Text = data.EmailAddress;

            useOcr_CheckBox.Checked = data.UseOcr;
            pageCount_NumericUpDown.Value = data.ScanOptions.PageCount;
            
            assetSelectionControl.AutomationPause = data.AutomationPause;
            assetSelectionControl.UseAdf = data.ScanOptions.UseAdf;

            quickSet_TextBox.Text = data.QuickSetName;
            quickSet_RadioButton.Checked = data.UseQuickset;
            launchFromApp_RadioButton.Checked = data.LaunchQuicksetFromApp;
            launchFromHome_RadioButton.Checked = !data.LaunchQuicksetFromApp;
            OptionalRadioButton.Checked = data.ImagePreviewOptions == 0;
            GenerateRadioButton.Checked = data.ImagePreviewOptions == 1;
            RestrictRadioButton.Checked = data.ImagePreviewOptions == 2;

            email_ComboBox.Items.Clear();

            // Populate the list of email addresses
            foreach (string email in ConfigurationServices.EnvironmentConfiguration.GetOutputMonitorDestinations("OutputEmail"))
            {
                email_ComboBox.Items.Add(email);
            }

            // Determine whether the DSS server should be filled in
            if (!string.IsNullOrEmpty(data.DigitalSendServer))
            {
                usesDigitalSendServer_CheckBox.Checked = true;
                digitalSendServer_TextBox.Text = data.DigitalSendServer;
            }
            else
            {
                usesDigitalSendServer_CheckBox.Checked = false;
            }
            // Check AddressSource
            addressSource_comboBox.SelectedIndex = string.IsNullOrEmpty(data.AddressSource) ? 0 : addressSource_comboBox.Items.IndexOf(data.AddressSource);
            _scanOptions = data.ScanOptions;
            if (data.ApplicationAuthentication)
            {
                radioButton_ScanToEmail.Checked = true;
            }
            else
            {
                radioButton_SignInButton.Checked = true;
            }
            comboBox_AuthProvider.SelectedValue = data.AuthProvider;
        }

        private void usesDigitalSendServer_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            digitalSendServerName_Label.Enabled = digitalSendServer_TextBox.Enabled = usesDigitalSendServer_CheckBox.Checked;
        }


        private void emailApp_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (emailApp_RadioButton.Checked)
            {
                fieldValidator.RequireValue(email_ComboBox, emailDestination_Label);
                fieldValidator.Remove(quickSet_TextBox);
                email_GroupBox.Visible = true;
                quickSet_GroupBox.Visible = false;
            }
            else
            {
                fieldValidator.RequireValue(quickSet_TextBox, quickset_Label, quickSet_RadioButton);
                fieldValidator.Remove(email_ComboBox);
                quickSet_GroupBox.Visible = true;
                email_GroupBox.Visible = false;
            }
        }

        private void ScanOptions_Button_Click(object sender, EventArgs e)
        {
            _scanOptions.ScanJobType = "ScanToEmail";
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
