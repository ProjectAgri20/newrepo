using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.SolutionApps.AutoStore;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.AutoStore
{
    /// <summary>
    /// Implements the Nuance AutoStore capture and route solution
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginConfigurationControl" />
    [ToolboxItem(false)]
    public partial class AutoStoreConfigControl : UserControl, IPluginConfigurationControl
    {
        private AutoStoreActivityData _activityData;
        private ScanOptions _scanOptions = new ScanOptions();

        public const string Version = "1.0";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoStoreConfigControl" /> class.
        /// </summary>
        public AutoStoreConfigControl()
        {
            InitializeComponent();

            fieldValidator.RequireSelection(AutoStoreApps_comboBox, "An AutoStore Application");
            fieldValidator.RequireAssetSelection(assetSelectionControl, "AutoStore Asset");
            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            AutoStoreApps_comboBox.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            checkBox_ImagePreview.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;

            checkBox_OCR.CheckedChanged += checkBox_OCR_CheckedChanged;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new AutoStoreActivityData();
            assetSelectionControl.Initialize(_deviceAttributes);
            lockTimeoutControl.Initialize(_activityData.ScanOptions.LockTimeouts);

            InitAutoStoreApplications();
            ConfigureControls();
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<AutoStoreActivityData>();
            assetSelectionControl.Initialize(configuration.Assets, _deviceAttributes);

            InitAutoStoreApplications();
            ConfigureControls();
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.AutoStoreAuthentication = AutoStoreRadioButton.Checked;
            _activityData.AutoStoreScanButton = AutoStoreApps_comboBox.Text;
            _activityData.DeviceMemoryProfilerConfig = deviceMemoryProfilerControl.SelectedData;
            _activityData.UseOcr = checkBox_OCR.Checked;
            _activityData.ImagePreview = checkBox_ImagePreview.Checked;

            _activityData.ScanOptions = _scanOptions;
            _activityData.ScanOptions.PageCount = int.Parse(pageCount_NumericUpDown.Value.ToString());
            _activityData.ScanOptions.LockTimeouts = lockTimeoutControl.Value;

            if (_activityData.UseOcr)
            {
                _activityData.ScanOptions.FileType = DeviceAutomation.FileType.SearchablePdfOcr;
            }

            return new PluginConfigurationData(_activityData, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData,
            };
        }

        /// <summary>
        /// Initializes the HPCR application types into the combo box.
        /// </summary>
        private void InitAutoStoreApplications()
        {
            Collection<string> scanApps = new Collection<string>();

            // Load combo box allowed items from enum descriptions
            foreach (AutoStoreAppTypes value in Enum.GetValues(typeof(AutoStoreAppTypes)))
            {
                scanApps.Add(value.GetDescription());
            }
            AutoStoreApps_comboBox.DataSource = scanApps;
        }

        /// <summary>
        /// Configures the controls per the activity data either derived from initialization or the saved meta data.
        /// </summary>
        private void ConfigureControls()
        {            
            pageCount_NumericUpDown.Value = _activityData.ScanOptions.PageCount;
            lockTimeoutControl.Initialize(_activityData.ScanOptions.LockTimeouts);
            checkBox_ImagePreview.Checked = _activityData.ImagePreview;
            checkBox_OCR.Checked = _activityData.UseOcr;
            AutoStoreRadioButton.Checked = _activityData.AutoStoreAuthentication;
            deviceMemoryProfilerControl.SelectedData = _activityData.DeviceMemoryProfilerConfig ?? new PluginSupport.MemoryCollection.DeviceMemoryProfilerConfig();

            if (String.IsNullOrEmpty(_activityData.AutoStoreScanButton))
            {
                AutoStoreApps_comboBox.SelectedIndex = 0;
            }
            else
            {
                // look for item in combo box allowed items (enum description)
                var foundIndex = AutoStoreApps_comboBox.Items.IndexOf(_activityData.AutoStoreScanButton);
                if (foundIndex == -1)
                {
                    // see if we can find as value (enum value as string)
                    var enumValue = EnumUtil.GetByDescription<AutoStoreAppTypes>(_activityData.AutoStoreScanButton, true);
                    var alternateValue = enumValue.GetDescription();
                    foundIndex = AutoStoreApps_comboBox.Items.IndexOf(alternateValue);
                }

                AutoStoreApps_comboBox.SelectedIndex = foundIndex;
            }
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

        /// <summary>
        /// Gets the selected scan application.
        /// </summary>
        /// <returns></returns>
        private string GetAutoStoreApplication()
        {
            string scanApp = string.Empty;

            if (AutoStoreApps_comboBox.SelectedIndex >= 0)
            {
                scanApp = AutoStoreApps_comboBox.SelectedItem.ToString();
                AutoStoreScan_label.Text = $"Uses \"{scanApp}\"";
            }
            return scanApp;
        }

        private void checkBox_OCR_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox_OCR.Checked)
            {
                _scanOptions.FileType = DeviceAutomation.FileType.SearchablePdfOcr;
            }
            else
            {
                _scanOptions.FileType = DeviceAutomation.FileType.DeviceDefault;
            }
            ConfigurationChanged(sender, e);
        }

        private void button_ScanOptions_Click(object sender, EventArgs e)
        {
            _scanOptions.ScanJobType = "AutoStore";
        }

        private void AutoStoreApps_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetAutoStoreApplication();
        }
    }
}
