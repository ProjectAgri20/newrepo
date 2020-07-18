using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.DeviceAutomation.Authentication;
using System.Collections.Generic;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ScanToUsb
{
    [ToolboxItem(false)]
    public partial class ScanToUsbConfigControl : UserControl, IPluginConfigurationControl
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
        public ScanToUsbConfigControl()
        {
            InitializeComponent();

            fieldValidator.RequireValue(usb_TextBox, "USB", usbName_RadioButton);
            fieldValidator.RequireValue(quickSet_TextBox, "Named QuickSet", quickSet_RadioButton);
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");

            usbName_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            usb_TextBox.TextChanged += useOcr_CheckBox_CheckedChanged;
            quickSet_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            useOcr_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            pageCount_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            usesDigitalSendServer_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            digitalSendServer_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);

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
            ConfigureControls(new ScanToUsbData());

            assetSelectionControl.Initialize(_deviceAttributes);
            
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            ConfigureControls(configuration.GetMetadata<ScanToUsbData>(ConverterProvider.GetMetadataConverters()));

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
            ScanToUsbData data = new ScanToUsbData()
            {
                UsbName = usb_TextBox.Text,
                QuickSetName = quickSet_TextBox.Text,
                UseQuickset = quickSet_RadioButton.Checked,
                UseOcr = useOcr_CheckBox.Checked,
               
                AutomationPause = assetSelectionControl.AutomationPause,
                
                ScanOptions = _scanOptions,
                ApplicationAuthentication = radioButton_ScanToUSB.Checked,
                AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue
            };
            data.ScanOptions.PageCount = (int)pageCount_NumericUpDown.Value;
            data.ScanOptions.LockTimeouts = lockTimeoutControl.Value;
            data.ScanOptions.UseAdf = assetSelectionControl.UseAdf;
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
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void ConfigureControls(ScanToUsbData data)
        {
            usb_TextBox.Text = data.UsbName;
            quickSet_TextBox.Text = data.QuickSetName;
            quickSet_RadioButton.Checked = data.UseQuickset;
            usbName_RadioButton.Checked = !data.UseQuickset;
            useOcr_CheckBox.Checked = data.UseOcr;
            pageCount_NumericUpDown.Value = data.ScanOptions.PageCount;

            lockTimeoutControl.Initialize(data.ScanOptions.LockTimeouts);

            assetSelectionControl.AutomationPause = data.AutomationPause;
            assetSelectionControl.UseAdf = data.ScanOptions.UseAdf;

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
                radioButton_ScanToUSB.Checked = true;
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
        private void usbName_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            usb_TextBox.Enabled = usbName_RadioButton.Checked;
            quickSet_TextBox.Enabled = !usbName_RadioButton.Checked;
        }

        private void ScanOptions_Button_Click(object sender, EventArgs e)
        {
            _scanOptions.ScanJobType = "ScanToUsb";
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
