using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ScanToHpcr
{
    /// <summary>
    /// Control to configure data for a Scan To Email activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ScanToHpcrConfigControl : UserControl, IPluginConfigurationControl
    {
        private ScanToHpcrActivityData _activityData;
        private List<string> _appsUsingDestination = new List<string>();
        public const string Version = "1.0";
        private const Framework.Assets.AssetAttributes _deviceAttributes = Framework.Assets.AssetAttributes.Scanner | Framework.Assets.AssetAttributes.ControlPanel;

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToHpcrConfigControl"/> class.
        /// </summary>
        public ScanToHpcrConfigControl()
        {
            InitializeComponent();

            // Set up Auth Provider Combobox
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpacDra, AuthenticationProvider.HpacDra.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpacIrm, AuthenticationProvider.HpacIrm.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.SafeCom, AuthenticationProvider.SafeCom.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.DSS, AuthenticationProvider.DSS.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Auto, AuthenticationProvider.Auto.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));
            comboBox_AuthProvider.DataSource = authProviders;

            _appsUsingDestination = new List<string>()
            {
                HpcrAppTypes.ScanToFolder.GetDescription(),
                HpcrAppTypes.PersonalDistributions.GetDescription(),
                HpcrAppTypes.PublicDistributions.GetDescription()
            };

            fieldValidator.RequireSelection(HpcrApps_comboBox, "An HPCR application");
            fieldValidator.RequireAssetSelection(assetSelectionControl, "HPCR Asset");

            fieldValidator.RequireValue(distribution_textbox, labelDistribution, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(destination_textBox, lblDestinationName, ValidationCondition.IfEnabled);

            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            HpcrApps_comboBox.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
            distribution_textbox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            destination_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            pageCount_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            checkBoxImagePreview.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            comboBox_AuthProvider.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;
        }

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new ScanToHpcrActivityData();

            InitHpcrApplications();
            ConfigureControls();

            assetSelectionControl.Initialize(_deviceAttributes);
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<ScanToHpcrActivityData>();
            InitHpcrApplications();
            ConfigureControls();

            assetSelectionControl.Initialize(configuration.Assets, _deviceAttributes);
        }
        /// <summary>
        /// Initializes the HPCR application types into the combo box.
        /// </summary>
        private void InitHpcrApplications()
        {
            Collection<string> scanApps = new Collection<string>();

            // Load combo box allowed items from enum descriptions
            foreach (HpcrAppTypes value in Enum.GetValues(typeof(HpcrAppTypes)))
            {
                scanApps.Add(value.GetDescription());
            }
            HpcrApps_comboBox.DataSource = scanApps;
        }
        /// <summary>
        /// Retrieves the user settings from the controls and sets the activity data class
        /// </summary>
        /// <returns>PluginConfigurationData</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.LockTimeouts = lockTimeoutControl.Value;

            _activityData.HpcrScanButton = HpcrApps_comboBox.SelectedItem.ToString();
            _activityData.ImagePreview = checkBoxImagePreview.Checked;
            _activityData.ScanDestination = destination_textBox.Text;
            _activityData.ScanDistribution = distribution_textbox.Text;
            _activityData.PageCount = int.Parse(pageCount_NumericUpDown.Value.ToString());
            _activityData.AutomationPause = assetSelectionControl.AutomationPause;
            _activityData.ApplicationAuthentication = radioButtonHpcr.Checked;
            _activityData.DeviceMemoryProfilerConfig = deviceMemoryProfilerControl.SelectedData;
            _activityData.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;

            return new PluginConfigurationData(_activityData, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData,
            };
        }
        /// <summary>
        /// Configures the controls per the activity data either derived from initialization or the saved meta data.
        /// </summary>
        private void ConfigureControls()
        {
            pageCount_NumericUpDown.Value = _activityData.PageCount;
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);

            destination_textBox.Text = _activityData.ScanDestination;
            distribution_textbox.Text = _activityData.ScanDistribution;

            assetSelectionControl.AutomationPause = _activityData.AutomationPause;
            checkBoxImagePreview.Checked = _activityData.ImagePreview;
            radioButtonHpcr.Checked = _activityData.ApplicationAuthentication;
            radioButtonDevice.Checked = !_activityData.ApplicationAuthentication;

            deviceMemoryProfilerControl.SelectedData = _activityData.DeviceMemoryProfilerConfig ?? new PluginSupport.MemoryCollection.DeviceMemoryProfilerConfig();

            comboBox_AuthProvider.SelectedValue = _activityData.AuthProvider;

            if (String.IsNullOrEmpty(_activityData.HpcrScanButton))
            {
                HpcrApps_comboBox.SelectedIndex = 0;
            }
            else
            {
                // look for item in combo box allowed items (enum description)
                var foundIndex = HpcrApps_comboBox.Items.IndexOf(_activityData.HpcrScanButton);
                if (foundIndex == -1)
                {
                    // see if we can find as value (enum value as string)
                    var enumValue = EnumUtil.GetByDescription<HpcrAppTypes>(_activityData.HpcrScanButton, true);
                    var alternateValue = enumValue.GetDescription();
                    foundIndex = HpcrApps_comboBox.Items.IndexOf(alternateValue);
                }

                HpcrApps_comboBox.SelectedIndex = foundIndex;
            }
        }

        public PluginValidationResult ValidateConfiguration()
        {
            PluginValidationResult pvr = new PluginValidationResult(fieldValidator.ValidateAll());

            return pvr;
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the HpcrApps_comboBox control.
        /// Determines the enablement of the distribution and destination text boxes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void HpcrApps_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string scanApp = GetScanApplication();

            if (_appsUsingDestination.Contains(scanApp, StringComparer.OrdinalIgnoreCase))
            {
                distribution_textbox.Enabled = true;
                if (scanApp.Equals("Public Distributions"))
                {
                    // just so I can find it!
                    lblDestinationName.Text = "Collection Name";
                    destination_textBox.Enabled = true;
                }
                else if (scanApp.Equals("Personal Distributions"))
                {
                    lblDestinationName.Text = "Destination Name";
                    destination_textBox.Enabled = false;
                    destination_textBox.Clear();
                }
                else if (scanApp.Equals("Scan To Folder"))
                {

                    distribution_textbox.Clear();
                    distribution_textbox.Enabled = false;

                    lblDestinationName.Text = "Destination Name";
                    destination_textBox.Enabled = true;
                }
            }
            else
            {
                SetControlDefaults();
            }
        }

        private void SetControlDefaults()
        {
            destination_textBox.Enabled = false;
            distribution_textbox.Enabled = false;
            lblDestinationName.Text = "Destination Name";
            destination_textBox.Clear();
            distribution_textbox.Clear();
        }
        /// <summary>
        /// Gets the selected scan application.
        /// </summary>
        /// <returns></returns>
        private string GetScanApplication()
        {
            string scanApp = string.Empty;

            if (HpcrApps_comboBox.SelectedIndex >= 0)
            {
                scanApp = HpcrApps_comboBox.SelectedItem.ToString();
                hpcrScan_label.Text = $"Uses HPCR \"{scanApp}\"";
            }
            return scanApp;
        }


    }
}
