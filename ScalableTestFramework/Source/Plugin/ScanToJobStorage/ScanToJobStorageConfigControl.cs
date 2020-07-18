using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Framework.Synchronization;
using System.ComponentModel;
using HP.ScalableTest.PluginSupport.Scan;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ScanToJobStorage
{
    /// <summary>
    /// Control to configure data for a Scan To Job Storage activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ScanToJobStorageConfigControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.1";
        private const AssetAttributes DeviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;
        //The scanoptions  will allow to set the settings for jobstorage job
        private ScanOptions _scanOptions = new ScanOptions();

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToJobStorageConfigControl"/> class.
        /// </summary>
        public ScanToJobStorageConfigControl()
        {
            InitializeComponent();
            fieldValidator.RequireAssetSelection(assetSelectionControl);
            fieldValidator.RequireValue(pin_TextBox, pin_Label, ValidationCondition.IfEnabled);
            pinRequired_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            pin_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            pageCount_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);

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
            ConfigureControls(new ScanToJobStorageData());
            assetSelectionControl.Initialize(DeviceAttributes);
        }
        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            ScanToJobStorageData activityData = configuration.GetMetadata<ScanToJobStorageData>(ConverterProvider.GetMetadataConverters());
            ConfigureControls(activityData);
            assetSelectionControl.Initialize(configuration.Assets, DeviceAttributes);
            assetSelectionControl.AdfDocuments = configuration.Documents;
        }
        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            ScanToJobStorageData data = new ScanToJobStorageData()
            {
                Pin = pin_TextBox.Text,
                IsPinRequired = pinRequired_CheckBox.Checked,
                
                AutomationPause = assetSelectionControl.AutomationPause,
                
                ScanOptions = _scanOptions,
                ApplicationAuthentication = radioButton_ScanToJobStorage.Checked,
                AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue
            };

            data.ScanOptions.UseAdf = assetSelectionControl.UseAdf;
            data.ScanOptions.LockTimeouts = lockTimeoutControl.Value;
            data.ScanOptions.PageCount = (int)pageCount_NumericUpDown.Value;

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

        private void ConfigureControls(ScanToJobStorageData data)
        {
            pin_TextBox.Text = data.Pin;
            pinRequired_CheckBox.Checked = data.IsPinRequired;
            lockTimeoutControl.Initialize(data.ScanOptions.LockTimeouts);
            pageCount_NumericUpDown.Value = data.ScanOptions.PageCount;
            assetSelectionControl.AutomationPause = data.AutomationPause;
            assetSelectionControl.UseAdf = data.ScanOptions.UseAdf;
            _scanOptions = data.ScanOptions;
            if (data.ApplicationAuthentication)
            {
                radioButton_ScanToJobStorage.Checked = true;
            }
            else
            {
                radioButton_SignInButton.Checked = true;
            }
            comboBox_AuthProvider.SelectedValue = data.AuthProvider;
        }

        private void pinRequired_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pin_TextBox.Enabled = pinRequired_CheckBox.Checked;
        }

        private void pin_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void ScanOptions_Button_Click(object sender, EventArgs e)
        {
            _scanOptions.ScanJobType = "ScanToJobStorage";
            using (var scanOptionsForm = new ScanOptionsForm(_scanOptions))
            {
                if (scanOptionsForm.ShowDialog() == DialogResult.OK)
                {
                    _scanOptions = scanOptionsForm.ScanOption;
                }
            }
        }
    }
}
