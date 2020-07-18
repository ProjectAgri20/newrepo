using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.SafeComPullPrinting
{
    [ToolboxItem(false)]
    public partial class SafeComPullPrintingConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.2";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Printer | AssetAttributes.ControlPanel;
        private PluginConfigurationData _pluginConfigurationData;
        private SafeComActivityData _activityData;

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Creates a new SafeComPullPrintingConfigControl.
        /// </summary>
        public SafeComPullPrintingConfigurationControl()
        {
            InitializeComponent();

            // Set up Auth Provider Combobox
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.SafeCom, AuthenticationProvider.SafeCom.GetDescription()));
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
            radioButton_SafeCom.CheckedChanged += OnConfigurationChanged;
            radioButton_SignInButton.CheckedChanged += OnConfigurationChanged;
            checkBoxReleaseOnSignIn.CheckedChanged += ConfigurationChanged;
            numericUpDown_Copies.ValueChanged += OnConfigurationChanged;
            printingTaskConfigurationControl.ConfigurationChanged += (s, e) => OnConfigurationChanged(s, e);
            comboBox_AuthProvider.SelectedIndexChanged += OnConfigurationChanged;
            checkBox_SelectAll.CheckedChanged += OnConfigurationChanged;

            // Set Enums on Radio buttons
            radioButton_Print.Tag = SafeComPullPrintAction.Print;
            radioButton_Retain.Tag = SafeComPullPrintAction.PrintRetain;
            radioButton_Delete.Tag = SafeComPullPrintAction.Delete;
            radioButton_PrintAll.Tag = SafeComPullPrintAction.PrintAll;
        }

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new SafeComActivityData();
            assetSelectionControl.Initialize(_deviceAttributes);
            printingTaskConfigurationControl.Initialize();
            SetConfiguration();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<SafeComActivityData>(ConverterProvider.GetMetadataConverters());
            _pluginConfigurationData = configuration;

            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, _deviceAttributes);
            printingTaskConfigurationControl.Initialize(_pluginConfigurationData.Documents, _pluginConfigurationData.PrintQueues, _activityData.DelayAfterPrint, _activityData.ShuffleDocuments, _activityData.UsePrintServerNotification);
            SetConfiguration();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.LockTimeouts = lockTimeoutControl.Value;
            _activityData.DeviceMemoryProfilerConfig = deviceMemoryProfilerControl.SelectedData;
            _activityData.SafeComAuthentication = radioButton_SafeCom.Checked;
            _activityData.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;
            _activityData.SelectAll = checkBox_SelectAll.Checked;
            _activityData.ReleaseOnSignIn = checkBoxReleaseOnSignIn.Checked;
            _activityData.NumberOfCopies = (int)numericUpDown_Copies.Value;
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

        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            deviceMemoryProfilerControl.SelectedData = _activityData.DeviceMemoryProfilerConfig;
            radioButton_SafeCom.Checked = _activityData.SafeComAuthentication;
            comboBox_AuthProvider.SelectedValue = _activityData.AuthProvider;
            checkBox_SelectAll.Checked = _activityData.SelectAll;
            checkBoxReleaseOnSignIn.Checked = _activityData.ReleaseOnSignIn;
            numericUpDown_Copies.Value = _activityData.NumberOfCopies;

            switch (_activityData.DocumentProcessAction)
            {
                case SafeComPullPrintAction.Print:
                    radioButton_Print.Checked = true;
                    break;
                case SafeComPullPrintAction.PrintAll:
                    radioButton_PrintAll.Checked = true;
                    break;
                case SafeComPullPrintAction.PrintRetain:
                case SafeComPullPrintAction.PrintRetainAll:
                    radioButton_Retain.Checked = true;
                    checkBox_SelectAll.Checked = false;
                    break;
                case SafeComPullPrintAction.Delete:
                case SafeComPullPrintAction.DeleteAll:
                    radioButton_Delete.Checked = true;
                    break;
            }
            if (_activityData.SelectAll != checkBox_SelectAll.Checked)
            {
                _activityData.SelectAll = checkBox_SelectAll.Checked;
                OnConfigurationChanged(this, EventArgs.Empty);
            }
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        private void RadioButton_Action_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = (RadioButton)sender;
            SafeComPullPrintAction action = (SafeComPullPrintAction)selected.Tag;

            if (checkBox_SelectAll.Checked)
            {
                switch (action)
                {
                    case SafeComPullPrintAction.PrintRetain:
                        //action = SafeComPullPrintAction.PrintRetainAll; dwa - 05/25/2017 retain all is not implemented at this time
                        checkBox_SelectAll.Checked = false;
                        break;
                    case SafeComPullPrintAction.Delete:
                        action = SafeComPullPrintAction.DeleteAll;
                        break;
                    case SafeComPullPrintAction.PrintAll:
                        // Don't allow Select All for PrintAll action
                        checkBox_SelectAll.Checked = false;
                        break;
                    case SafeComPullPrintAction.PrintRetainAll:
                        action = SafeComPullPrintAction.PrintRetain;
                        checkBox_SelectAll.Checked = false;
                        break;
                }
            }

            checkBox_SelectAll.Enabled = (action != SafeComPullPrintAction.PrintAll && action != SafeComPullPrintAction.PrintRetain);
            numericUpDown_Copies.Enabled = (action != SafeComPullPrintAction.PrintRetain);

            _activityData.DocumentProcessAction = action;
            _activityData.SelectAll = checkBox_SelectAll.Checked;

            OnConfigurationChanged(this, e);

        }

        private void checkBoxReleaseOnSignIn_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxAuthentication.Enabled = !checkBoxReleaseOnSignIn.Checked;
            groupBoxSafeComConfiguration.Enabled = !checkBoxReleaseOnSignIn.Checked;
        }

    }
}
