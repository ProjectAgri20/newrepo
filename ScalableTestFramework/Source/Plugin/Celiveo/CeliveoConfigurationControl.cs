using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.Celiveo
{
    [ToolboxItem(false)]
    public partial class CeliveoConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.0";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Printer | AssetAttributes.ControlPanel;
        private CeliveoActivityData _activityData;
        private PluginConfigurationData _pluginConfigurationData;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="CeliveoConfigurationControl" /> class.
        /// </summary>
        public CeliveoConfigurationControl()
        {
            InitializeComponent();

            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Celiveo, AuthenticationProvider.Celiveo.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Auto, AuthenticationProvider.Auto.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.DSS, AuthenticationProvider.DSS.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));
            comboBox_AuthProvider.DataSource = authProviders;

            // Set up field validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            // Set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;
            radioButton_SignInButton.CheckedChanged += OnConfigurationChanged;
            radioButton_Celiveo.CheckedChanged += OnConfigurationChanged;
            numericUpDown_copies.ValueChanged += OnConfigurationChanged;
            checkBox_SelectAll.CheckedChanged += OnConfigurationChanged;
            checkBox_ReleaseOnSignIn.CheckedChanged += OnConfigurationChanged;
            printingConfigurationControl.ConfigurationChanged += OnConfigurationChanged;
            comboBox_AuthProvider.SelectedIndexChanged += OnConfigurationChanged;

            // Set Enums on Radio buttons
            radioButton_PrintBW.Tag = CeliveoPullPrintAction.PrintBW;
            radioButton_Print.Tag = CeliveoPullPrintAction.Print;
            radioButton_Delete.Tag = CeliveoPullPrintAction.Delete;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new CeliveoActivityData();
            assetSelectionControl.Initialize(_deviceAttributes);
            printingConfigurationControl.Initialize();
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            SetPullPrintAction();
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<CeliveoActivityData>();
            _pluginConfigurationData = configuration;

            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, _deviceAttributes);
            printingConfigurationControl.Initialize(_pluginConfigurationData.Documents, _pluginConfigurationData.PrintQueues, _activityData.DelayAfterPrint, _activityData.ShuffleDocuments, _activityData.UsePrintServerNotification);
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            SetConfiguration();
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.LockTimeouts = lockTimeoutControl.Value;
            _activityData.DeviceMemoryProfilerConfig = deviceMemoryProfilerControl.SelectedData;
            _activityData.CeliveoAuthentication = radioButton_Celiveo.Checked;
            _activityData.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;

            _activityData.SelectAll = checkBox_SelectAll.Checked;
            _activityData.ReleaseOnSignIn = checkBox_ReleaseOnSignIn.Checked;
            _activityData.DocumentProcessAction = GetPullPrintAction();
            _activityData.NumberOfCopies = (int)numericUpDown_copies.Value;
            _activityData.ShuffleDocuments = printingConfigurationControl.GetShuffle();
            _activityData.DelayAfterPrint = printingConfigurationControl.GetDelay();
            _activityData.UsePrintServerNotification = printingConfigurationControl.GetPrintServerNotification();

            return new PluginConfigurationData(_activityData, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData,
                PrintQueues = printingConfigurationControl.GetPrintQueues(),
                Documents = printingConfigurationControl.GetDocuments()
            };
        }

        /// <summary>
        /// Validates the current plugin configuration.
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Set configuration by activity data
        /// </summary>
        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            deviceMemoryProfilerControl.SelectedData = _activityData.DeviceMemoryProfilerConfig;
            checkBox_SelectAll.Checked = _activityData.SelectAll;
            checkBox_ReleaseOnSignIn.Checked = _activityData.ReleaseOnSignIn;
            
            switch (_activityData.CeliveoAuthentication)
            {
                case true:
                    radioButton_Celiveo.Checked = true;
                    break;
                case false:
                    radioButton_SignInButton.Checked = true;
                    break;
            }

            comboBox_AuthProvider.SelectedValue = _activityData.AuthProvider;

            SetPullPrintAction();
            numericUpDown_copies.Value = _activityData.NumberOfCopies;
        }

        /// <summary>
        /// Set pull print action: Print or Delete
        /// </summary>
        private void SetPullPrintAction()
        {
            switch (_activityData.DocumentProcessAction)
            {                
                case CeliveoPullPrintAction.Print:
                    radioButton_Print.Checked = true;
                    break;
                case CeliveoPullPrintAction.PrintBW:
                    radioButton_PrintBW.Checked = true;
                    break;
                case CeliveoPullPrintAction.Delete:
                    radioButton_Delete.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// Get pull print action: Print or Delete
        /// </summary>
        private CeliveoPullPrintAction GetPullPrintAction()
        {
            RadioButton selected = groupBox_PullPrint.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            return (CeliveoPullPrintAction)selected.Tag;
        }
    }
}
