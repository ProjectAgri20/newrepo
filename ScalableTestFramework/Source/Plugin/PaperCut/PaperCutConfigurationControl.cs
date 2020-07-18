using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.PaperCut
{
    /// <summary>
    /// Manages plugin configuration data for PaperCut automated operations.
    /// </summary>
    public partial class PaperCutConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.0";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Printer | AssetAttributes.ControlPanel;
        private PluginConfigurationData _pluginConfigurationData;
        private PaperCutActivityData _activityData;

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Creates a new instance of <see cref="PaperCutPullPrintingConfigurationControl" />.
        /// </summary>
        public PaperCutConfigurationControl()
        {
            InitializeComponent();

            // Set up Auth Provider Combobox
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.PaperCut, AuthenticationProvider.PaperCut.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.DSS, AuthenticationProvider.DSS.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Auto, AuthenticationProvider.Auto.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));

            comboBox_AuthProvider.DataSource = authProviders;

            // Set up field validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");

            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            // Set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;
            radioButton_PaperCut.CheckedChanged += OnConfigurationChanged;
            radioButton_SignInButton.CheckedChanged += OnConfigurationChanged;
            checkBox_SelectAll.CheckedChanged += OnConfigurationChanged;
            checkBox_ReleaseOnSignIn.CheckedChanged += OnConfigurationChanged;
            printingConfigurationControl.ConfigurationChanged += OnConfigurationChanged;
            comboBox_AuthProvider.SelectedIndexChanged += OnConfigurationChanged;

            // Set Enums on Radio buttons
            radioButton_Print.Tag = PaperCutPullPrintAction.Print;
            radioButton_Delete.Tag = PaperCutPullPrintAction.Delete;
        }

        /// <summary>
        /// Initializes this instance of <see cref="PaperCutPullPrintingConfigurationControl" /> with default data.
        /// </summary>
        /// <param name="environment">The plugin environment</param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new PaperCutActivityData();
            assetSelectionControl.Initialize(_deviceAttributes);
            printingConfigurationControl.Initialize();
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            SetPullPrintAction();
        }

        /// <summary>
        /// Initializes this instance of <see cref="PaperCutPullPrintingConfigurationControl" /> with the specified plugin configuration data.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment">The plugin environment</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<PaperCutActivityData>();
            _pluginConfigurationData = configuration;

            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, _deviceAttributes);
            printingConfigurationControl.Initialize(_pluginConfigurationData.Documents, _pluginConfigurationData.PrintQueues, _activityData.DelayAfterPrint, _activityData.ShuffleDocuments, _activityData.UsePrintServerNotification);
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            SetConfiguration();
        }

        /// <summary>
        /// Gets the current Plugin configuration data.
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.LockTimeouts = lockTimeoutControl.Value;
            _activityData.DeviceMemoryProfilerConfig = deviceMemoryProfilerControl.SelectedData;
            _activityData.PaperCutAuthentication = radioButton_PaperCut.Checked;
            _activityData.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;

            _activityData.SelectAll = checkBox_SelectAll.Checked;
            _activityData.DocumentProcessAction = GetPullPrintAction();
            _activityData.ReleaseOnSignIn = checkBox_ReleaseOnSignIn.Checked;
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

        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            deviceMemoryProfilerControl.SelectedData = _activityData.DeviceMemoryProfilerConfig;
            checkBox_SelectAll.Checked = _activityData.SelectAll;
            checkBox_ReleaseOnSignIn.Checked = _activityData.ReleaseOnSignIn;

            switch (_activityData.PaperCutAuthentication)
            {
                case true:
                    radioButton_PaperCut.Checked = true;
                    break;
                case false:
                    radioButton_SignInButton.Checked = true;
                    break;
            }

            comboBox_AuthProvider.SelectedValue = _activityData.AuthProvider;

            SetPullPrintAction();
        }

        private void SetPullPrintAction()
        {
            switch (_activityData.DocumentProcessAction)
            {
                case PaperCutPullPrintAction.Print:
                    radioButton_Print.Checked = true;
                    break;
                case PaperCutPullPrintAction.Delete:
                    radioButton_Delete.Checked = true;
                    break;
            }
        }

        private PaperCutPullPrintAction GetPullPrintAction()
        {
            RadioButton selected = groupBox_PullPrintConfig.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            return (PaperCutPullPrintAction)selected.Tag;
        }
    }
}
