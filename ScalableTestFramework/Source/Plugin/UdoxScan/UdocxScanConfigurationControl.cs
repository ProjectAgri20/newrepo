using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.DeviceAutomation.SolutionApps.UdocxScan;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.UdocxScan
{
    [ToolboxItem(false)]
    public partial class UdocxScanConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private UdocxScanActivityData _data;
        private const AssetAttributes DeviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdocxScanConfigurationControl" /> class.
        /// </summary>
        public UdocxScanConfigurationControl()
        {
            InitializeComponent();

            //Set up Auth Provider combobox
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.UdocxScan, AuthenticationProvider.UdocxScan.GetDescription()));
            AuthProvider_comboBox.DataSource = authProviders;

            // Set up field validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            fieldValidator.RequireValue(EmailAddress_textBox, EmailAddress_label, ValidationCondition.IfEnabled);

            // Set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            ScantoEmail_radioButton.CheckedChanged += OnConfigurationChanged;
            ScantoOneDrive_radioButton.CheckedChanged += OnConfigurationChanged;
            ScantoSharePoint365_radioButton.CheckedChanged += OnConfigurationChanged;
            EmailAddress_textBox.TextChanged += OnConfigurationChanged;
            AuthProvider_comboBox.SelectedIndexChanged += OnConfigurationChanged;

        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>on
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new UdocxScanActivityData();
            SetConfiguration(_data);

            assetSelectionControl.Initialize(DeviceAttributes);
            lockTimeoutControl.Initialize(_data.LockTimeouts);
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<UdocxScanActivityData>();
            SetConfiguration(_data);

            assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.None);
            lockTimeoutControl.Initialize(_data.LockTimeouts);
        }

        /// <summary>
        /// Set configuration by activity data
        /// </summary>
        private void SetConfiguration(UdocxScanActivityData data)
        {
            lockTimeoutControl.Initialize(data.LockTimeouts);
            EmailAddress_textBox.Text = data.EmailAddress;
            AuthProvider_comboBox.SelectedValue = _data.AuthProvider;

            if (data.JobType.Equals(UdocxScanJobType.ScantoMail))
            {
                ScantoEmail_radioButton.Checked = true;
            }
            else if (data.JobType.Equals(UdocxScanJobType.ScantoOneDrive))
            {
                ScantoOneDrive_radioButton.Checked = true;
            }
            else//data.JobType.Equals(UdocxScanJobType.ScantoSharePoint365
            {
                ScantoSharePoint365_radioButton.Checked = true;
            }
            lockTimeoutControl.Initialize(data.LockTimeouts);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data.LockTimeouts = lockTimeoutControl.Value;
            _data.EmailAddress = EmailAddress_textBox.Text;
            _data.AuthProvider = (AuthenticationProvider)AuthProvider_comboBox.SelectedValue;

            if (ScantoEmail_radioButton.Checked)
            {
                _data.JobType = UdocxScanJobType.ScantoMail;
            }
            else//ScantoSharePoint365_radioButton.Checked or ScantoOneDrive_radioButton.Checked
            {
                _data.JobType = UdocxScanJobType.ScantoOneDrive;
            }
            
            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
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

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        private void ScantoEmail_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ScantoEmail_radioButton.Checked)
            {
                EmailAddress_textBox.Enabled = true;
            }
            else//ScantoSharePoint365_radioButton.Checked or ScantoOneDrive_radioButton.Checked
            {
                EmailAddress_textBox.Enabled = false;
            }
        }
    }
}
