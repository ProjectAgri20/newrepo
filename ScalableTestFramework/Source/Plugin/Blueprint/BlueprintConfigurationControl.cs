using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.Assets;
using System.Linq;

namespace HP.ScalableTest.Plugin.Blueprint
{
    [ToolboxItem(false)]
    public partial class BlueprintConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.0";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Printer | AssetAttributes.ControlPanel;
        private BlueprintActivityData _activityData;
        private PluginConfigurationData _pluginConfigurationData;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlueprintConfigurationControl" /> class.
        /// </summary>
        public BlueprintConfigurationControl()
        {
            InitializeComponent();

            // Set up Auth Provider Combobox
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();

            //authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Blueprint, AuthenticationProvider.Blueprint.GetDescription()));
	        authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));

            comboBox_AuthProvider.DataSource = authProviders;

            //Set up field Validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            //set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;
            radioButton_Blueprint.CheckedChanged += OnConfigurationChanged;
            radioButton_SignInButton.CheckedChanged += OnConfigurationChanged;
            printingTaskConfigurationControl.ConfigurationChanged += (s, e) => OnConfigurationChanged(s, e);
            comboBox_AuthProvider.SelectedIndexChanged += OnConfigurationChanged;

            //Set Enums on Radio Buttons
            radioButton_Print.Tag = BlueprintPullPrintAction.Print;
            radioButton_Delete.Tag = BlueprintPullPrintAction.Delete;
            radioButton_PrintAll.Tag = BlueprintPullPrintAction.PrintAll;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new BlueprintActivityData();
            assetSelectionControl.Initialize(_deviceAttributes);
            printingTaskConfigurationControl.Initialize();
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<BlueprintActivityData>();
            _pluginConfigurationData = configuration;

            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, _deviceAttributes);
            printingTaskConfigurationControl.Initialize(_pluginConfigurationData.Documents, _pluginConfigurationData.PrintQueues, _activityData.DelayAfterPrint, _activityData.ShuffleDocuments, _activityData.UsePrintServerNotification);
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
            _activityData.BlueprintAuthentication = radioButton_Blueprint.Checked;
            _activityData.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;

            _activityData.DocumentProcessAction = GetAction();
            _activityData.ShuffleDocuments = printingTaskConfigurationControl.GetShuffle();
            _activityData.DelayAfterPrint = printingTaskConfigurationControl.GetDelay();
            _activityData.UsePrintServerNotification = printingTaskConfigurationControl.GetPrintServerNotification();

            return new PluginConfigurationData(_activityData, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData,
                PrintQueues = printingTaskConfigurationControl.GetPrintQueues(),
                Documents = printingTaskConfigurationControl.GetDocuments()
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

        private BlueprintPullPrintAction GetAction()
        {
            RadioButton selected = groupBox_PullPrintConfig.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            return (BlueprintPullPrintAction)selected.Tag;
        }

        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            deviceMemoryProfilerControl.SelectedData = _activityData.DeviceMemoryProfilerConfig;

            switch (_activityData.BlueprintAuthentication)
            {
                case true:
                    radioButton_Blueprint.Checked = true;
                    break;
                case false:
                    radioButton_SignInButton.Checked = true;
                    break;
            }

            comboBox_AuthProvider.SelectedValue = _activityData.AuthProvider;

            switch (_activityData.DocumentProcessAction)
            {
                case BlueprintPullPrintAction.Delete:
                    radioButton_Delete.Checked = true;
                    break;
                case BlueprintPullPrintAction.PrintAll:
                    radioButton_PrintAll.Checked = true;
                    break;
                case BlueprintPullPrintAction.Print:
                    radioButton_Print.Checked = true;
                    break;
            }
        }

        private void RadioButton_Action_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = (RadioButton)sender;

            //This filters out one of the two events that will fire from a CheckedChanged event.

            OnConfigurationChanged(this, e);
        }

        private void comboBox_AuthProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_AuthProvider.Text == AuthenticationProvider.HpacAgentLess.GetDescription())
            {
                radioButton_SignInButton.Checked = true;
                radioButton_Blueprint.Enabled = false;
            }
            else if (!radioButton_Blueprint.Enabled)
            {
                radioButton_Blueprint.Enabled = true;
            }
        }
    }
}
