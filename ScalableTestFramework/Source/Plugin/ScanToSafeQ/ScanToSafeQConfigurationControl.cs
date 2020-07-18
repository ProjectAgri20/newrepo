using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Plugin;
using System.Collections.Generic;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ScanToSafeQ
{
    [ToolboxItem(false)]
    public partial class ScanToSafeQConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private ScanToSafeQActivityData _data;
        private PluginConfigurationData _pluginConfigurationData;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToSafeQConfigurationControl" /> class.
        /// </summary>
        public ScanToSafeQConfigurationControl()
        {
            InitializeComponent();

            // Set up Auth Provider Combobox
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.SafeQ, AuthenticationProvider.SafeQ.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Auto, AuthenticationProvider.Auto.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.DSS, AuthenticationProvider.DSS.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));
            comboBox_AuthProvider.DataSource = authProviders;

            // Set up field validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            fieldValidator.RequireValue(path_textBox, path_label);

            //Set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            radioButton_SignInButton.CheckedChanged += OnConfigurationChanged;
            radioButton_SafeQScan.CheckedChanged += OnConfigurationChanged;
            comboBox_AuthProvider.SelectedIndexChanged += OnConfigurationChanged;
            jobBuildPageCount_numericUpDown.ValueChanged += OnConfigurationChanged;
            path_textBox.TextChanged += OnConfigurationChanged;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new ScanToSafeQActivityData();
            assetSelectionControl.Initialize(AssetAttributes.None);
            lockTimeoutControl.Initialize(_data.LockTimeouts);
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<ScanToSafeQActivityData>();
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
            _data.SafeQAuthentication = radioButton_SafeQScan.Checked;
            _data.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;
            _data.WorkFlowDescription = path_textBox.Text;
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
            switch (_data.SafeQAuthentication)
            {
                case true:
                    radioButton_SafeQScan.Checked = true;
                    break;
                case false:
                    radioButton_SignInButton.Checked = true;
                    break;
            }
            comboBox_AuthProvider.SelectedValue = _data.AuthProvider;
            jobBuildPageCount_numericUpDown.Value = _data.ScanCount;
            path_textBox.Text = _data.WorkFlowDescription;
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
