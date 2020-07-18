using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.HpacSimulation
{
    [ToolboxItem(false)]
    public partial class HpacSimulationConfigControl : UserControl, IPluginConfigurationControl
    {
        private PluginConfigurationData _configData = null;
        private HpacSimulationActivityData _activityData = null;

        public HpacSimulationConfigControl()
        {
            InitializeComponent();

            // For this to work, the tag of each radio button must be set to the corresponding HpacAuthenticationMode.
            domain_RadioButton.Tag = HpacAuthenticationMode.DomainCredentials;
            pic_RadioButton.Tag = HpacAuthenticationMode.Pic;
            smartCard_RadioButton.Tag = HpacAuthenticationMode.SmartCard;

            // Set up Validation
            fieldValidator.RequireSelection(hpac_ServerComboBox, hpacServer_Label);
            fieldValidator.RequireAssetSelection(assetSelectionControl);
            fieldValidator.SetIconAlignment(assetSelectionControl, ErrorIconAlignment.TopRight);

            // Set up Change notification
            hpac_ServerComboBox.SelectionChanged += OnConfigurationChanged;
            printAll_CheckBox.CheckedChanged += OnConfigurationChanged;
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
        }

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new HpacSimulationActivityData();
            assetSelectionControl.Initialize(AssetAttributes.Printer);
            hpac_ServerComboBox.Initialize("HPAC");
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Initialize the activity data by deserializing it from the configuration information.
            _configData = configuration;
            _activityData = _configData.GetMetadata<HpacSimulationActivityData>();

            hpac_ServerComboBox.Initialize(_configData.Servers.SelectedServers.FirstOrDefault(), "HPAC");
            assetSelectionControl.Initialize(_configData.Assets, AssetAttributes.Printer);

            // Brute force initialization
            printAll_CheckBox.Checked = _activityData.PullAllDocuments;
            foreach (RadioButton button in authentication_GroupBox.Controls)
            {
                button.Checked = ((HpacAuthenticationMode)button.Tag == _activityData.HpacAuthenticationMode);
            }
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.PullAllDocuments = printAll_CheckBox.Checked;

            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
                Servers = new ServerSelectionData(hpac_ServerComboBox.SelectedServer)
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Updates the underlying data source with the selected authentication mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HpacAuthenticationMode_RadioButton_Click(object sender, EventArgs e)
        {
            _activityData.HpacAuthenticationMode = (HpacAuthenticationMode)(sender as RadioButton).Tag;
            OnConfigurationChanged(sender, e);
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(this, e);
                //System.Diagnostics.Debug.WriteLine("Config Changed.");
            }
        }

    }
}
