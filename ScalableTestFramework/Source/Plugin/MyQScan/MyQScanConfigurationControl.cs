using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Plugin.MyQScan.MyQScanActivityData;

namespace HP.ScalableTest.Plugin.MyQScan
{
    [ToolboxItem(false)]
    public partial class MyQScanConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "2.0";
        private PluginConfigurationData _pluginConfigurationData;
        private MyQScanActivityData _data;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyQScanConfigurationControl" /> class.
        /// </summary>
        public MyQScanConfigurationControl()
        {
            InitializeComponent();

            fieldValidator.RequireAssetSelection(assetSelectionControl1, "device");
            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            assetSelectionControl1.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;

            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.MyQ, AuthenticationProvider.MyQ.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.DSS, AuthenticationProvider.DSS.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Auto, AuthenticationProvider.Auto.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            comboBox_AuthMethod.DataSource = authProviders;

            radioButton_Email.CheckedChanged += OnConfigurationChanged;
            radioButton_Folder.CheckedChanged += OnConfigurationChanged;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new MyQScanActivityData();
            assetSelectionControl1.Initialize(AssetAttributes.None);
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
            _data = configuration.GetMetadata<MyQScanActivityData>();
            _pluginConfigurationData = configuration;
            assetSelectionControl1.Initialize(_pluginConfigurationData.Assets, AssetAttributes.None);
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
            _data.AuthProvider = (AuthenticationProvider)comboBox_AuthMethod.SelectedValue;
            if (radioButton_Email.Checked)
            {
                _data.ScanType = MyQScanType.Email;
            }
            else if (radioButton_Folder.Checked)
            {
                _data.ScanType = MyQScanType.Folder;
            }

            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = assetSelectionControl1.AssetSelectionData,
            };
        }

        /// <summary>
        /// Set configuration by activity data
        /// </summary>
        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_data.LockTimeouts);
            deviceMemoryProfilerControl.SelectedData = _data.DeviceMemoryProfilerConfig;
            comboBox_AuthMethod.SelectedValue = _data.AuthProvider;
            if (MyQScanType.Email.Equals(_data.ScanType))
            {
                radioButton_Email.Checked = true;
            }
            else if (MyQScanType.Folder.Equals(_data.ScanType))
            {
                radioButton_Folder.Checked = true;
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
    }
}
