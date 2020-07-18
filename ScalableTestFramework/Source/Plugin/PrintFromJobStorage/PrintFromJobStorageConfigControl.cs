using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.DeviceAutomation.Authentication;
using System.Collections.Generic;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.PrintFromJobStorage
{
    /// <summary>
    /// Control to configure data for a Print From Job Storage activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class PrintFromJobStorageConfigControl : UserControl, IPluginConfigurationControl
    {
        private const AssetAttributes _deviceAttributes = AssetAttributes.ControlPanel | AssetAttributes.Printer;
        private PrintFromJobStorageActivityData _data;

        public const string Version = "1.0";

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintFromJobStorageConfigControl"/> class.
        /// </summary>
        public PrintFromJobStorageConfigControl()
        {
            InitializeComponent();
            jobStorage_AssetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            printAll_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            pinRequired_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            pin_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            folderName_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            fieldValidator.RequireAssetSelection(jobStorage_AssetSelectionControl);
            fieldValidator.RequireValue(pin_TextBox, pin_Label, ValidationCondition.IfEnabled);

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
            _data.FolderName = folderName_TextBox.Text;
            _data.Pin = pin_TextBox.Text;
            _data.PrintAll = printAll_CheckBox.Checked;
            _data.DeleteJobAfterPrint = deleteJobAfterPrint_Checkbox.Checked;
            _data.IsPinRequired = pinRequired_CheckBox.Checked;
            _data.NumberOfCopies = Copies_NumericUpDown.Enabled ? Convert.ToInt32(!string.IsNullOrEmpty(Copies_NumericUpDown.Text) ? Copies_NumericUpDown.Text : "1") : 1;
            _data.ApplicationAuthentication = radioButton_PrintFromJobStprage.Checked;
            _data.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;

            return new PluginConfigurationData(_data, Version)
            {
                Assets = jobStorage_AssetSelectionControl.AssetSelectionData
            };
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new PrintFromJobStorageActivityData();
            jobStorage_AssetSelectionControl.Initialize(_deviceAttributes);
            ConfigureControls(_data);
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<PrintFromJobStorageActivityData>();
            jobStorage_AssetSelectionControl.Initialize(configuration.Assets, _deviceAttributes);
            folderName_TextBox.Text = _data.FolderName;
            pin_TextBox.Text = _data.Pin;
            printAll_CheckBox.Checked = _data.PrintAll;
            deleteJobAfterPrint_Checkbox.Checked = _data.DeleteJobAfterPrint;
            pinRequired_CheckBox.Checked = _data.IsPinRequired;
            Copies_NumericUpDown.Value = _data.NumberOfCopies < 1 ? 1 : _data.NumberOfCopies;
            ConfigureControls(_data);
        }

        public void ConfigureControls(PrintFromJobStorageActivityData data)
        {
            comboBox_AuthProvider.SelectedValue = _data.AuthProvider;
            if (_data.ApplicationAuthentication)
            {
                radioButton_PrintFromJobStprage.Checked = true;
            }
            else
            {
                radioButton_SignInButton.Checked = true;
            }
        }
        /// <summary>
        /// Validates the activity's configuration data.
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void pinRequired_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pin_TextBox.Enabled = pinRequired_CheckBox.Checked;
            pin_TextBox.Text = pinRequired_CheckBox.Checked ? pin_TextBox.Text : "";
        }

        private void pin_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void EnableDisableNumberOfCopies(object sender, EventArgs e)
        {
            Copies_NumericUpDown.Enabled = printAll_CheckBox.Checked ? false : true;
        }
    }
}
