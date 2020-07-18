using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;
using HP.ScalableTest.PluginSupport.Print;
using System.Linq;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.Copy
{
    /// <summary>
    /// Control to configure data for a Copy activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class CopyConfigControl : UserControl, IPluginConfigurationControl
    {
        private readonly List<PrintQueueInfo> _printQueues = new List<PrintQueueInfo>();

        private readonly BindingList<PrintQueueRow> _printQueueRows = new BindingList<PrintQueueRow>();

        private const AssetAttributes DeviceAttributes = AssetAttributes.Printer | AssetAttributes.Scanner | AssetAttributes.ControlPanel;
        //private CopyPreferences _preferences = new CopyPreferences(); Renamed to CopyOptions 9-August-2017
        private CopyOptions _options = new CopyOptions();

        public const string Version = "1.1";

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyConfigControl"/> class.
        /// </summary>
        public CopyConfigControl()
        {
            InitializeComponent();
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            preferences_button.DataBindings.Add("Enabled", copyApp_RadioButton, "Checked");
            quickSet_GroupBox.DataBindings.Add("Enabled", quickSet_RadioButton, "Checked");
            fieldValidator.RequireValue(quickSet_TextBox, quickset_Label, ValidationCondition.IfEnabled);

            pageCount_NumericUpDown.ValueChanged += ConfigurationChanged;
            assetSelectionControl.SelectionChanged += ConfigurationChanged;
            usesDigitalSendServer_CheckBox.CheckedChanged += ConfigurationChanged;
            digitalSendServer_TextBox.TextChanged += ConfigurationChanged;

            copyApp_RadioButton.CheckedChanged += ConfigurationChanged;
            launchFromApp_RadioButton.CheckedChanged += ConfigurationChanged;
            launchFromHome_RadioButton.CheckedChanged += ConfigurationChanged;
            quickSet_RadioButton.CheckedChanged += ConfigurationChanged;
            quickSet_TextBox.TextChanged += ConfigurationChanged;

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
            ConfigureControls(new CopyData());
            assetSelectionControl.Initialize(DeviceAttributes);            
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            var copyData = configuration.GetMetadata<CopyData>(ConverterProvider.GetMetadataConverters());
            ConfigureControls(copyData);
            _options = copyData.Options;
            jobseparator_checkBox.Checked = copyData.PrintJobSeparator;
            LoadPrintQueues(configuration.PrintQueues.SelectedPrintQueues);
            assetSelectionControl.Initialize(configuration.Assets, DeviceAttributes);
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
            CopyData data = new CopyData()
            {
                Color = _options.Color,
                PageCount = (int)pageCount_NumericUpDown.Value,
                Copies = _options.Copies,
                QuickSetName = quickSet_TextBox.Text,
                UseQuickset = quickSet_RadioButton.Checked,
                LaunchQuicksetFromApp = launchFromApp_RadioButton.Checked,
                Options = _options,
                ApplicationAuthentication = radioButton_Copy.Checked,
                AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue,
                PrintJobSeparator = jobseparator_checkBox.Checked
            };
                     
            data.LockTimeouts = lockTimeoutControl.Value;

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

        private void ConfigureControls(CopyData data)
        {
            pageCount_NumericUpDown.Value = data.PageCount;
            quickSet_TextBox.Text = data.QuickSetName;
            lockTimeoutControl.Initialize(data.LockTimeouts);
            quickSet_RadioButton.Checked = data.UseQuickset;
            launchFromApp_RadioButton.Checked = data.LaunchQuicksetFromApp;
            launchFromHome_RadioButton.Checked = !data.LaunchQuicksetFromApp;
            comboBox_AuthProvider.SelectedValue = data.AuthProvider;

            if (data.ApplicationAuthentication)
            {
                radioButton_Copy.Checked = true;
            }
            else
            {
                radioButton_SignInButton.Checked = true;
            }

        }


        private void preferences_button_Click(object sender, EventArgs e)
        {
            using (var preferences = new CopyOptionsForm(_options))
            {
                if (preferences.ShowDialog() == DialogResult.OK)
                {
                    _options = preferences.CopyOptions;
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
