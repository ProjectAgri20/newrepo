using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using System.Linq;
using System.ComponentModel;
using HP.ScalableTest.PluginSupport.Scan;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;
using HP.ScalableTest.PluginSupport.Print;

namespace HP.ScalableTest.Plugin.Fax
{
    /// <summary>
    /// Control used to configure the Fax Plugin execution data.
    /// </summary>
    [ToolboxItem(false)]
    public partial class FaxConfigControl : UserControl, IPluginConfigurationControl
    {
        private readonly List<PrintQueueInfo> _printQueues = new List<PrintQueueInfo>();

        private readonly BindingList<PrintQueueRow> _printQueueRows = new BindingList<PrintQueueRow>();

        public const string Version = "1.1";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;
        //The _fax options will allow to set the settings for fax job
        private ScanOptions _faxOptions = new ScanOptions();

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanFaxConfigControl" /> class.
        /// </summary>
        public FaxConfigControl()
        {
            InitializeComponent();

            fieldValidator.RequireValue(email_ComboBox, "Notification email address", notify_CheckBox);
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            fieldValidator.RequireValue(comboBoxFaxType, "Fax Type");

            notify_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            email_ComboBox.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
            pageCount_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            usesDigitalSendServer_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            digitalSendServer_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            comboBoxFaxType.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
            faxNumber_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            timeSpanControlFaxReceive.ValueChanged += (s, e) => ConfigurationChanged(s, e);

            foreach (RadioButton button in groupBoxFaxOperation.Controls.OfType<RadioButton>())
            {
                button.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            }

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
            ConfigureControls(new FaxActivityData());
            assetSelectionControl.Initialize(_deviceAttributes);
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            FaxActivityData activityData = configuration.GetMetadata<FaxActivityData>(ConverterProvider.GetMetadataConverters());
            ConfigureControls(activityData);

            jobseparator_checkBox.Checked = activityData.PrintJobSeparator;
            LoadPrintQueues(configuration.PrintQueues.SelectedPrintQueues);
            assetSelectionControl.Initialize(configuration.Assets, _deviceAttributes);
            assetSelectionControl.AdfDocuments = configuration.Documents;
            RefreshQueueDataGrid();
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            FaxActivityData data = new FaxActivityData()
            {
                EnableNotification = notify_CheckBox.Checked,
                NotificationEmail = email_ComboBox.Text,
                
                AutomationPause = assetSelectionControl.AutomationPause,
               
                FaxType = (FaxConfiguration)Enum.Parse(typeof(FaxConfiguration), comboBoxFaxType.Text),
                FaxNumber = faxNumber_textBox.Text.ToString(),
                FaxReceiveTimeout = timeSpanControlFaxReceive.Value,
                ImagePreviewOptions = OptionalRadioButton.Checked ? 0 : GenerateRadioButton.Checked ? 1 : RestrictRadioButton.Checked ? 2 : 0,
                PIN = PIN_textBox.Text,
                UseSpeedDial = useSpeedDial_Checkbox.Checked,
                ScanOptions = _faxOptions,
                ApplicationAuthentication = radioButton_Fax.Checked,
                AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue,
                PrintJobSeparator = jobseparator_checkBox.Checked
            };
            data.ScanOptions.PageCount = (int)pageCount_NumericUpDown.Value;
            data.ScanOptions.LockTimeouts = lockTimeoutControl.Value;
            data.ScanOptions.UseAdf = assetSelectionControl.UseAdf;
            data.ScanOptions.ScanJobType = "ScanToFax";

            if (usesDigitalSendServer_CheckBox.Checked)
            {
                data.DigitalSendServer = digitalSendServer_TextBox.Text;
            }
            else
            {
                data.DigitalSendServer = null;
            }

            data.UseSpeedDial = useSpeedDial_Checkbox.Checked;

            if (radioFaxReceive.Checked)
                data.FaxOperation = FaxTask.ReceiveFax;
            else if (radioFaxSend.Checked)
                data.FaxOperation = FaxTask.SendFax;

            return new PluginConfigurationData(data, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData,
                Documents = assetSelectionControl.AdfDocuments,
                PrintQueues = new PrintQueueSelectionData(_printQueues)
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

        private void ConfigureControls(FaxActivityData data)
        {
            pageCount_NumericUpDown.Value = data.ScanOptions.PageCount;
            lockTimeoutControl.Initialize(data.ScanOptions.LockTimeouts);

            assetSelectionControl.AutomationPause = data.AutomationPause;
            assetSelectionControl.UseAdf = data.ScanOptions.UseAdf;

            notify_CheckBox.Checked = data.EnableNotification;
            email_ComboBox.Text = data.NotificationEmail;

            email_ComboBox.Items.Clear();
            // Populate the list of email addresses
            foreach (string email in ConfigurationServices.EnvironmentConfiguration.GetOutputMonitorDestinations("DigitalSendNotification"))
            {
                email_ComboBox.Items.Add(email);
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
            foreach (FaxConfiguration type in (FaxConfiguration[])Enum.GetValues(typeof(FaxConfiguration)))
            {
                comboBoxFaxType.Items.Add(type);
            }
            comboBoxFaxType.Text = data.FaxType.ToString();
            faxNumber_textBox.Text = data.FaxNumber;
            timeSpanControlFaxReceive.Value = data.FaxReceiveTimeout;

            if (data != null)
            {
                switch (data.FaxOperation)
                {
                    case FaxTask.SendFax:
                        radioFaxSend.Checked = true;
                        break;
                    case FaxTask.ReceiveFax:
                        radioFaxReceive.Checked = true;
                        break;
                    default:
                        break;
                }
            }

            useSpeedDial_Checkbox.Checked = data.UseSpeedDial;

            OptionalRadioButton.Checked = data.ImagePreviewOptions == 0;
            GenerateRadioButton.Checked = data.ImagePreviewOptions == 1;
            RestrictRadioButton.Checked = data.ImagePreviewOptions == 2;

            PIN_textBox.Text = data.PIN;
            _faxOptions = data.ScanOptions;
            comboBox_AuthProvider.SelectedValue = data.AuthProvider;
            if (data.ApplicationAuthentication)
            {
                radioButton_Fax.Checked = true;
            }
            else
            {
                radioButton_SignInButton.Checked = true;
            }
        }

        private void radioFaxReceive_CheckedChanged(object sender, EventArgs e)
        {
            if(radioFaxReceive.Checked)
            {
                timeSpanControlFaxReceive.Visible = true;
                labelFaxTimeout.Visible = true;
                labelFaxTimeoutHelp.Visible = true;
                labelFaxTimeoutHelp.Text = "Wait time for Fax arrival before retrieving the Fax report";
            }
            else if (radioFaxSend.Checked)
            {
                timeSpanControlFaxReceive.Visible = false;
                labelFaxTimeout.Visible = false;
                labelFaxTimeoutHelp.Visible = false;
                labelFaxTimeoutHelp.Text = "";
            }
        }

        private void radioFaxSend_CheckedChanged(object sender, EventArgs e)
        {
            if (radioFaxSend.Checked)
            {
                timeSpanControlFaxReceive.Visible = false;
                labelFaxTimeout.Visible = false;
                labelFaxTimeoutHelp.Visible = false;
                labelFaxTimeoutHelp.Text = "";
            }
            else if (radioFaxReceive.Checked)
            {
                timeSpanControlFaxReceive.Visible = true;
                labelFaxTimeout.Visible = true;
                labelFaxTimeoutHelp.Visible = true;
                labelFaxTimeoutHelp.Text = "Wait time for Fax arrival before retrieving the Fax report";
            }
        }


        private void PIN_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)44)
            {
                e.Handled = true;
            }
        }

        private void FaxOptions_Button_Click(object sender, EventArgs e)
        {
            _faxOptions.ScanJobType = "ScanToFax";
            using (var scanOptionsForm = new ScanOptionsForm(_faxOptions))
            {
                if (scanOptionsForm.ShowDialog() == DialogResult.OK)
                {
                    _faxOptions = scanOptionsForm.ScanOption;
                }
            }
        }

        private void localQueue_button_Click(object sender, EventArgs e)
        {
            var originalLocalPrintQueues = _printQueues.Except(_printQueues.OfType<RemotePrintQueueInfo>()).ToList();
            using (LocalQueueSelectionForm form = new LocalQueueSelectionForm(originalLocalPrintQueues))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {

                    ReplaceQueues(originalLocalPrintQueues, form.SelectedQueues);
                    RefreshQueueDataGrid();
                    ConfigurationChanged(this, EventArgs.Empty);
                }
            }
        }

        private void ReplaceQueues(IEnumerable<PrintQueueInfo> oldQueues, IEnumerable<PrintQueueInfo> newQueues)
        {
            foreach (PrintQueueInfo queue in oldQueues)
            {
                _printQueues.Remove(queue);
            }
            _printQueues.AddRange(newQueues);
        }

        private void RefreshQueueDataGrid()
        {
            _printQueueRows.Clear();
            foreach (PrintQueueInfo info in _printQueues)
            {
                _printQueueRows.Add(new PrintQueueRow(info));
            }
        }
        private void LoadPrintQueues(PrintQueueDefinitionCollection printQueueDefinitions)
        {
            _printQueues.Clear();

            foreach (LocalPrintQueueDefinition local in printQueueDefinitions.OfType<LocalPrintQueueDefinition>())
            {
                _printQueues.Add(new LocalPrintQueueInfo(local.QueueName, local.AssociatedAssetId));
            }
        }

        private class PrintQueueRow
        {
            public string QueueName { get; private set; }
            public string PrintServer { get; private set; }
            public string QueueType { get; private set; }
            public string Device { get; private set; }

            public PrintQueueRow(PrintQueueInfo queue)
            {
                QueueName = queue.QueueName;
                PrintServer = "Client";
                QueueType = "Local";
                Device = queue.AssociatedAssetId;
            }
        }
    }
}
