using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.iSecStarPullPrinting
{
    [ToolboxItem(false)]
    public partial class iSecStarPullPrintingConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.0";
        private PluginConfigurationData _pluginConfigurationData;
        private iSecStarActivityData _activityData;

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Creates a new ISecStarPullPrintingConfigControl
        /// </summary>
        public iSecStarPullPrintingConfigurationControl()
        {
            InitializeComponent();

            // Set up Auth Provider Combobox
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.ISecStar, AuthenticationProvider.ISecStar.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));
            comboBox_AuthProvider.DataSource = authProviders;

            // Set up field validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            // Set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;
            radioButton_iSecStar.CheckedChanged += OnConfigurationChanged;
            radioButton_SignInButton.CheckedChanged += OnConfigurationChanged;
            checkBox_SelectAll.CheckedChanged += OnConfigurationChanged;
            checkBoxReleaseOnSignIn.CheckedChanged += OnConfigurationChanged;
            printingTaskConfigurationControl.ConfigurationChanged += OnConfigurationChanged;
            comboBox_AuthProvider.SelectedIndexChanged += OnConfigurationChanged;
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;

            // Set Enums on Radio buttons

            radioButton_RePrint.Tag = iSecStarPullPrintAction.Reprint;
            radioButton_Print.Tag = iSecStarPullPrintAction.Print;
            radioButton_Delete.Tag = iSecStarPullPrintAction.Delete;
        }

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new iSecStarActivityData();
            assetSelectionControl.Initialize(AssetAttributes.None);
            printingTaskConfigurationControl.Initialize();
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<iSecStarActivityData>();
            _pluginConfigurationData = configuration;

            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, AssetAttributes.None);
            printingTaskConfigurationControl.Initialize(_pluginConfigurationData.Documents, _pluginConfigurationData.PrintQueues, _activityData.DelayAfterPrint, _activityData.ShuffleDocuments, _activityData.UsePrintServerNotification);
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            SetConfiguration();
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>Configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.LockTimeouts = lockTimeoutControl.Value;
            _activityData.DeviceMemoryProfilerConfig = deviceMemoryProfilerControl.SelectedData;
            _activityData.ISecStarAuthentication = radioButton_iSecStar.Checked;
            _activityData.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;

            _activityData.SelectAll = checkBox_SelectAll.Checked;
            _activityData.DocumentProcessAction = GetAction();
            _activityData.ReleaseOnSignIn = checkBoxReleaseOnSignIn.Checked;
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
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            deviceMemoryProfilerControl.SelectedData = _activityData.DeviceMemoryProfilerConfig;
            checkBox_SelectAll.Checked = _activityData.SelectAll;
            checkBoxReleaseOnSignIn.Checked = _activityData.ReleaseOnSignIn;

            switch (_activityData.ISecStarAuthentication)
            {
                case true:
                    radioButton_iSecStar.Checked = true;
                    break;
                case false:
                    radioButton_SignInButton.Checked = true;
                    break;
            }

            comboBox_AuthProvider.SelectedValue = _activityData.AuthProvider;

            switch (_activityData.DocumentProcessAction)
            {
                case iSecStarPullPrintAction.Reprint:
                    radioButton_RePrint.Checked = true;
                    break;
                case iSecStarPullPrintAction.Print:
                    radioButton_Print.Checked = true;
                    break;
                case iSecStarPullPrintAction.Delete:
                    radioButton_Delete.Checked = true;
                    break;
            }
        }

        private iSecStarPullPrintAction GetAction()
        {
            RadioButton selected = groupBox_PullPrintConfig.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            return (iSecStarPullPrintAction)selected.Tag;
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        private void RadioButton_Action_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = (RadioButton)sender;
            OnConfigurationChanged(this, e);
        }

        private void checkBoxReleaseOnSignIn_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxAuthentication.Enabled = !checkBoxReleaseOnSignIn.Checked;
            groupBox_PullPrintConfig.Enabled = !checkBoxReleaseOnSignIn.Checked;
        }

        private void comboBox_AuthProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_AuthProvider.Text == AuthenticationProvider.ISecStar.GetDescription())
            {
                radioButton_SignInButton.Checked = true;
                radioButton_iSecStar.Enabled = true;
            }
        }
    }
}
