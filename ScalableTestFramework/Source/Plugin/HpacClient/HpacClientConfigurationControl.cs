using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.HpacClient
{
    /// <summary>
    /// UI Configuration Control Class for HPAC Client Plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class HpacClientConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private HpacClientActivityData _activityData;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Event handler to be called whenever the activity's configuration data changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Constructor for the UI configuration class of HPAC Client Plugin
        /// </summary>
        public HpacClientConfigurationControl()
        {
            InitializeComponent();
            printDriverSelectionControl.Initialize();

            // Initializing the field Validation controls
            fieldValidator.RequireSelection(serverComboBoxHpac, serverIplabel);
            fieldValidator.RequireCustom(printDriverSelectionControl, () => printDriverSelectionControl.HasSelection, "A print driver must be selected.");
            fieldValidator.RequireValue(lprQueuetextBox, lprQueuelabel, checkBoxInstallClient);
            fieldValidator.RequireValue(textBoxInstallerPath, clientInstallerPathlabel, checkBoxInstallClient);
            fieldValidator.RequireValue(textBoxJAServerName, labelJaServerName, checkBoxInstallClient);
            fieldValidator.RequireValue(textBoxIPMServerName, labelIpmServerName, checkBoxInstallClient);
            fieldValidator.RequireValue(textBoxPullPrintServerName, labelPullPrintServerName, checkBoxInstallClient);

            //Event Handling for Controls for selection change
            radioButtonEnableBidi.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            radioButtonDisableBidi.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            radioButtonPrintAfterSpooling.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            radioButtonPrintImmediately.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            checkBoxDefaultPrinter.CheckedChanged += (s, e) => ConfigurationChanged(s, e);

            lprQueuetextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            textBoxInstallerPath.TextChanged += (s, e) => ConfigurationChanged(s, e);
            textBoxJAServerName.TextChanged += (s, e) => ConfigurationChanged(s, e);
            textBoxIPMServerName.TextChanged += (s, e) => ConfigurationChanged(s, e);
            textBoxPullPrintServerName.TextChanged += (s, e) => ConfigurationChanged(s, e);
            checkBoxQuota.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            checkBoxDelegate.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            checkBoxIpm.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            checkBoxJobStorage.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            checkBoxInstallClient.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            printDriverSelectionControl.SelectionChanged += OnConfigurationChanged;
            serverComboBoxHpac.SelectionChanged += OnConfigurationChanged;
        }

        /// <summary>
        /// Initializer for HPAC Client
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new HpacClientActivityData();
            serverComboBoxHpac.Initialize("HPAC");
            printDriverSelectionControl.Initialize();
            LoadUi();
        }

        /// <summary>
        /// Initializer for HPAC Client with  Plugin Configuration Data
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<HpacClientActivityData>();

            serverComboBoxHpac.Initialize(configuration.Servers.SelectedServers.FirstOrDefault(), "HPAC");
            printDriverSelectionControl.Initialize(_activityData.PrintDriver);

            LoadUi();
        }

        /// <summary>
        /// Retrieves all Plugin Configuration and returns the Metadata of  HPAC Client Plugin
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.PrintDriver = printDriverSelectionControl.SelectedPrintDriver;
            _activityData.LprQueueName = lprQueuetextBox.Text;
            _activityData.IsDefaultPrinter = checkBoxDefaultPrinter.Checked;
            _activityData.EnableBidi = radioButtonEnableBidi.Checked;
            _activityData.PrintAfterSpooling = radioButtonPrintAfterSpooling.Checked;

            _activityData.InstallHpacClient = checkBoxInstallClient.Checked;
            _activityData.HpacClientInstallerPath = textBoxInstallerPath.Text;
            _activityData.HpacJAServerName = textBoxJAServerName.Text;
            _activityData.HpacIPMServerName = textBoxIPMServerName.Text;
            _activityData.HpacPullPrintServerName = textBoxPullPrintServerName.Text;
            _activityData.Quota = checkBoxQuota.Checked;
            _activityData.Ipm = checkBoxIpm.Checked;
            _activityData.Delegate = checkBoxDelegate.Checked;
            _activityData.LocalJobStorage = checkBoxJobStorage.Checked;

            return new PluginConfigurationData(_activityData, "1.0")
            {
                Servers = new ServerSelectionData(serverComboBoxHpac.SelectedServer),
            };
        }

        /// <summary>
        /// Initialize UI Components with Activity Data Value
        /// </summary>
        private void LoadUi()
        {
            textBoxInstallerPath.Text = _activityData.HpacClientInstallerPath;
            lprQueuetextBox.Text = _activityData.LprQueueName;
            checkBoxDefaultPrinter.Checked = _activityData.IsDefaultPrinter;

            radioButtonEnableBidi.Checked = _activityData.EnableBidi;
            radioButtonDisableBidi.Checked = !_activityData.EnableBidi;

            radioButtonPrintAfterSpooling.Checked = _activityData.PrintAfterSpooling;
            radioButtonPrintImmediately.Checked = !_activityData.PrintAfterSpooling;

            checkBoxInstallClient.Checked = _activityData.InstallHpacClient;
            hpacClientInstallGroupBox.Visible = _activityData.InstallHpacClient;

            textBoxJAServerName.Text = _activityData.HpacJAServerName;
            textBoxIPMServerName.Text = _activityData.HpacIPMServerName;
            textBoxPullPrintServerName.Text = _activityData.HpacPullPrintServerName;

            checkBoxQuota.Checked = _activityData.Quota;
            checkBoxIpm.Checked = _activityData.Ipm;
            checkBoxDelegate.Checked = _activityData.Delegate;
            checkBoxJobStorage.Checked = _activityData.LocalJobStorage;
        }

        /// <summary>
        /// Validates the Plugin Configuration
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        /// <summary>
        /// HPAC Client Installer Selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallerPathButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select HPAC Client Installer";
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Setup File (*.msi)|*.msi";
            fdlg.DefaultExt = "*.msi";
            fdlg.Multiselect = false;
            fdlg.RestoreDirectory = true;
            fdlg.CheckFileExists = true; // To prevent mistypes in editbox
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBoxInstallerPath.Text = fdlg.FileName;
            }
        }

        /// <summary>
        /// On Chceked changed for Install Client Checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxInstallClient_CheckedChanged(object sender, EventArgs e)
        {
            hpacClientInstallGroupBox.Visible = checkBoxInstallClient.Checked;
        }
    }
}
