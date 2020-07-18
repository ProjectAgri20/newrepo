using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.GeniusBytes;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Utility;
using System.Collections.Generic;

namespace HP.ScalableTest.Plugin.GeniusBytesScan
{
    [ToolboxItem(false)]
    public partial class GeniusBytesScanConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.0";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Printer | AssetAttributes.ControlPanel;
        private GeniusBytesScanActivityData _activityData;
        private PluginConfigurationData _pluginConfigurationData;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeniusBytesScanConfigurationControl" /> class.
        /// </summary>
        public GeniusBytesScanConfigurationControl()
        {
            InitializeComponent();
            //ScanOptions
            List<KeyValuePair<GeniusByteScanFileTypeOption, string>> ScanFileTypeOptions = new List<KeyValuePair<GeniusByteScanFileTypeOption, string>>();
            ScanFileTypeOptions.Add(new KeyValuePair<GeniusByteScanFileTypeOption, string>(GeniusByteScanFileTypeOption.Multipage_PDF, GeniusByteScanFileTypeOption.Multipage_PDF.GetDescription()));
            ScanFileTypeOptions.Add(new KeyValuePair<GeniusByteScanFileTypeOption, string>(GeniusByteScanFileTypeOption.PDF_A, GeniusByteScanFileTypeOption.PDF_A.GetDescription()));
            ScanFileTypeOptions.Add(new KeyValuePair<GeniusByteScanFileTypeOption, string>(GeniusByteScanFileTypeOption.JPEG, GeniusByteScanFileTypeOption.JPEG.GetDescription()));
            ScanFileTypeOptions.Add(new KeyValuePair<GeniusByteScanFileTypeOption, string>(GeniusByteScanFileTypeOption.Searchable_PDF, GeniusByteScanFileTypeOption.Searchable_PDF.GetDescription()));
            ScanFileTypeOptions.Add(new KeyValuePair<GeniusByteScanFileTypeOption, string>(GeniusByteScanFileTypeOption.Searchable_PDF_A, GeniusByteScanFileTypeOption.Searchable_PDF_A.GetDescription()));
            ScanFileTypeOptions.Add(new KeyValuePair<GeniusByteScanFileTypeOption, string>(GeniusByteScanFileTypeOption.Monopage_TIFF, GeniusByteScanFileTypeOption.Monopage_TIFF.GetDescription()));
            ScanFileTypeOptions.Add(new KeyValuePair<GeniusByteScanFileTypeOption, string>(GeniusByteScanFileTypeOption.Multipage_TIFF, GeniusByteScanFileTypeOption.Multipage_TIFF.GetDescription()));
            comboBox_FileType.DataSource = ScanFileTypeOptions;

            List<KeyValuePair<GeniusByteScanSidesOption, string>> ScanSidesOptions = new List<KeyValuePair<GeniusByteScanSidesOption, string>>();
            ScanSidesOptions.Add(new KeyValuePair<GeniusByteScanSidesOption, string>(GeniusByteScanSidesOption.Simplex, GeniusByteScanSidesOption.Simplex.GetDescription()));
            ScanSidesOptions.Add(new KeyValuePair<GeniusByteScanSidesOption, string>(GeniusByteScanSidesOption.DuplexLongEdge, GeniusByteScanSidesOption.DuplexLongEdge.GetDescription()));
            ScanSidesOptions.Add(new KeyValuePair<GeniusByteScanSidesOption, string>(GeniusByteScanSidesOption.DuplexShortEdge, GeniusByteScanSidesOption.DuplexShortEdge.GetDescription()));
            comboBox_Sides.DataSource = ScanSidesOptions;

            List<KeyValuePair<GeniusByteScanColorOption, string>> ScanColourOptions = new List<KeyValuePair<GeniusByteScanColorOption, string>>();
            ScanColourOptions.Add(new KeyValuePair<GeniusByteScanColorOption, string>(GeniusByteScanColorOption.BlackNWhite, GeniusByteScanColorOption.BlackNWhite.GetDescription()));
            ScanColourOptions.Add(new KeyValuePair<GeniusByteScanColorOption, string>(GeniusByteScanColorOption.Color, GeniusByteScanColorOption.Color.GetDescription()));
            ScanColourOptions.Add(new KeyValuePair<GeniusByteScanColorOption, string>(GeniusByteScanColorOption.Grayscale, GeniusByteScanColorOption.Grayscale.GetDescription()));
            comboBox_Colour.DataSource = ScanColourOptions;

            List<KeyValuePair<GeniusByteScanResolutionOption, string>> ScanResolutionOptions = new List<KeyValuePair<GeniusByteScanResolutionOption, string>>();
            ScanResolutionOptions.Add(new KeyValuePair<GeniusByteScanResolutionOption, string>(GeniusByteScanResolutionOption.DPI200, GeniusByteScanResolutionOption.DPI200.GetDescription()));
            ScanResolutionOptions.Add(new KeyValuePair<GeniusByteScanResolutionOption, string>(GeniusByteScanResolutionOption.DPI300, GeniusByteScanResolutionOption.DPI300.GetDescription()));
            ScanResolutionOptions.Add(new KeyValuePair<GeniusByteScanResolutionOption, string>(GeniusByteScanResolutionOption.DPI400, GeniusByteScanResolutionOption.DPI400.GetDescription()));
            ScanResolutionOptions.Add(new KeyValuePair<GeniusByteScanResolutionOption, string>(GeniusByteScanResolutionOption.DPI600, GeniusByteScanResolutionOption.DPI600.GetDescription()));
            comboBox_Resolution.DataSource = ScanResolutionOptions;

            //set up field validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");

            //set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            checkBox_ReleaseOnSignIn.CheckedChanged += OnConfigurationChanged;
            radioButton_GuestLogin.CheckedChanged += OnConfigurationChanged;
            radioButton_ManualLogin.CheckedChanged += OnConfigurationChanged;
            radioButton_PINLogin.CheckedChanged += OnConfigurationChanged;
            radioButton_ProximityCardLogin.CheckedChanged += OnConfigurationChanged;

            //Scan App Selection
            radioButton_Scan2Home.CheckedChanged += OnConfigurationChanged;
            radioButton_Scan2Mail.CheckedChanged += OnConfigurationChanged;
            radioButton_Scan2ME.CheckedChanged += OnConfigurationChanged;
            jobBuildPageCount_numericUpDown.ValueChanged += OnConfigurationChanged;
            comboBox_FileType.SelectedIndexChanged += OnConfigurationChanged;
            comboBox_Sides.SelectedIndexChanged += OnConfigurationChanged;
            comboBox_Colour.SelectedIndexChanged += OnConfigurationChanged;
            comboBox_Resolution.SelectedIndexChanged += OnConfigurationChanged;
            checkBoxImagePreview.CheckedChanged += OnConfigurationChanged;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new GeniusBytesScanActivityData();
            assetSelectionControl.Initialize(_deviceAttributes);
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<GeniusBytesScanActivityData>();
            _pluginConfigurationData = configuration;
            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, _deviceAttributes);
            SetConfiguration();
        }

        /// <summary>
        /// Set configuration by activity data
        /// </summary>
        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            checkBox_ReleaseOnSignIn.Checked = _activityData.ReleaseOnSignIn;
            checkBoxImagePreview.Checked = _activityData.ImagePreview;

            switch (_activityData.AuthProvider)
            {
                case AuthenticationProvider.GeniusBytesGuest:
                    radioButton_GuestLogin.Checked = true;
                    break;
                case AuthenticationProvider.GeniusBytesManual:
                    radioButton_ManualLogin.Checked = true;
                    break;
                case AuthenticationProvider.GeniusBytesPin:
                    radioButton_PINLogin.Checked = true;
                    break;
                case AuthenticationProvider.Card:
                    radioButton_ProximityCardLogin.Checked = true;
                    break;
            }

            switch(_activityData.AppName)
            {
                case GeniusBytesScanType.Scan2ME:
                    radioButton_Scan2ME.Checked = true;
                    break;
                case GeniusBytesScanType.Scan2Home:
                    radioButton_Scan2Home.Checked = true;
                    break;
                case GeniusBytesScanType.Scan2Mail:
                    radioButton_Scan2Mail.Checked = true;
                    break;
            }
            comboBox_FileType.SelectedValue = _activityData.FileType;
            comboBox_Colour.SelectedValue = _activityData.ColourOption;
            comboBox_Sides.SelectedValue = _activityData.SidesOption;
            comboBox_Resolution.SelectedValue = _activityData.ResolutionOption;
            jobBuildPageCount_numericUpDown.Value = _activityData.ScanCount;
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.LockTimeouts = lockTimeoutControl.Value;
            _activityData.FileType = (GeniusByteScanFileTypeOption)comboBox_FileType.SelectedValue;
            _activityData.ColourOption = (GeniusByteScanColorOption)comboBox_Colour.SelectedValue;
            _activityData.SidesOption = (GeniusByteScanSidesOption)comboBox_Sides.SelectedValue;
            _activityData.ResolutionOption = (GeniusByteScanResolutionOption)comboBox_Resolution.SelectedValue;
            _activityData.ScanCount = (int)jobBuildPageCount_numericUpDown.Value;
            _activityData.ReleaseOnSignIn = checkBox_ReleaseOnSignIn.Checked;
            _activityData.ImagePreview = checkBoxImagePreview.Checked;

            if (radioButton_Scan2ME.Checked)
            {
                _activityData.AppName = GeniusBytesScanType.Scan2ME;
            }
            else if (radioButton_Scan2Home.Checked)
            {
                _activityData.AppName = GeniusBytesScanType.Scan2Home;
            }
            else if(radioButton_Scan2Mail.Checked)
            {
                _activityData.AppName = GeniusBytesScanType.Scan2Mail;
            }


            if (radioButton_GuestLogin.Checked)
            {
                _activityData.AuthProvider = AuthenticationProvider.GeniusBytesGuest;
            }
            else if (radioButton_ManualLogin.Checked)
            {
                _activityData.AuthProvider = AuthenticationProvider.GeniusBytesManual;
            }
            else if (radioButton_PINLogin.Checked)
            {
                _activityData.AuthProvider = AuthenticationProvider.GeniusBytesPin;
            }
            else
            {
                _activityData.AuthProvider = AuthenticationProvider.Card;
            }

            return new PluginConfigurationData(_activityData, Version)
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

    }
}
