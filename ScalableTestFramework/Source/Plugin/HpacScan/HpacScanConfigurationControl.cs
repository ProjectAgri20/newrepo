using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Plugin;
using System.Collections.Generic;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Utility;
using HP.ScalableTest.DeviceAutomation.Enums;
using HP.ScalableTest.DeviceAutomation.HpacScan;

namespace HP.ScalableTest.Plugin.HpacScan
{
    [ToolboxItem(false)]
    public partial class HpacScanConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.0";
        private PluginConfigurationData _pluginConfigurationData;
        private HpacScanActivityData _data;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="HpacScanConfigurationControl" /> class.
        /// </summary>
        public HpacScanConfigurationControl()
        {
            InitializeComponent();

            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;

            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpacScan, AuthenticationProvider.HpacScan.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.DSS, AuthenticationProvider.DSS.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Auto, AuthenticationProvider.Auto.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));
            comboBox_AuthProvider.DataSource = authProviders;

            List<KeyValuePair<PaperSupplyType, string>> paperSupply = new List<KeyValuePair<PaperSupplyType, string>>();
            paperSupply.Add(new KeyValuePair<PaperSupplyType, string>(PaperSupplyType.Simplex, PaperSupplyType.Simplex.GetDescription()));
            paperSupply.Add(new KeyValuePair<PaperSupplyType, string>(PaperSupplyType.Duplex, PaperSupplyType.Duplex.GetDescription()));
            comboBox_PaperSupply.DataSource = paperSupply;

            List<KeyValuePair<ColorModeType, string>> colorMode = new List<KeyValuePair<ColorModeType, string>>();
            colorMode.Add(new KeyValuePair<ColorModeType, string>(ColorModeType.Color, ColorModeType.Color.GetDescription()));
            colorMode.Add(new KeyValuePair<ColorModeType, string>(ColorModeType.Greyscale, ColorModeType.Greyscale.GetDescription()));
            colorMode.Add(new KeyValuePair<ColorModeType, string>(ColorModeType.Monochrome, ColorModeType.Monochrome.GetDescription()));
            comboBox_ColorMode.DataSource = colorMode;

            List<KeyValuePair<QualityType, string>> quality = new List<KeyValuePair<QualityType, string>>();
            quality.Add(new KeyValuePair<QualityType, string>(QualityType.Low, QualityType.Low.GetDescription()));
            quality.Add(new KeyValuePair<QualityType, string>(QualityType.Normal, QualityType.Normal.GetDescription()));
            quality.Add(new KeyValuePair<QualityType, string>(QualityType.High, QualityType.High.GetDescription()));
            comboBox_Quality.DataSource = quality;
            
            checkBox_JobBuild.CheckedChanged += OnConfigurationChanged;
            jobBuildPageCount_numericUpDown.ValueChanged += OnConfigurationChanged;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new HpacScanActivityData();
            assetSelectionControl.Initialize(AssetAttributes.None);
            lockTimeoutControl.Initialize(_data.LockTimeouts);
            SetConfiguration();
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<HpacScanActivityData>();
            _pluginConfigurationData = configuration;
            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, AssetAttributes.None);
            lockTimeoutControl.Initialize(_data.LockTimeouts);
            SetConfiguration();
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data.LockTimeouts = lockTimeoutControl.Value;
            _data.DeviceMemoryProfilerConfig = deviceMemoryProfilerControl.SelectedData;
            _data.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;
            _data.PaperSupplyType = (PaperSupplyType)comboBox_PaperSupply.SelectedValue;
            _data.ColorModeType = (ColorModeType)comboBox_ColorMode.SelectedValue;
            _data.QualityType = (QualityType)comboBox_Quality.SelectedValue;
            _data.JobBuild = checkBox_JobBuild.Checked;
            _data.ScanCount = (int)jobBuildPageCount_numericUpDown.Value;

            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
            };
        }

        /// <summary>
        /// Set configuration by activity data
        /// </summary>
        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_data.LockTimeouts);
            deviceMemoryProfilerControl.SelectedData = _data.DeviceMemoryProfilerConfig;
            comboBox_AuthProvider.SelectedValue = _data.AuthProvider;
            comboBox_PaperSupply.SelectedValue = _data.PaperSupplyType;
            comboBox_ColorMode.SelectedValue = _data.ColorModeType;
            comboBox_Quality.SelectedValue = _data.QualityType;
            checkBox_JobBuild.Checked = _data.JobBuild;
            jobBuildPageCount_numericUpDown.Value = _data.ScanCount;
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
        /// set numberUpDown enable or not
        /// </summary>
        private void checkBox_JobBuild_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox_PaperSupply.SelectedValue.Equals(PaperSupplyType.Simplex) && !checkBox_JobBuild.Checked)
            {
                jobBuildPageCount_numericUpDown.Enabled = false;
            }
            else
            {
                jobBuildPageCount_numericUpDown.Enabled = true;
            }
        }
    }
}
