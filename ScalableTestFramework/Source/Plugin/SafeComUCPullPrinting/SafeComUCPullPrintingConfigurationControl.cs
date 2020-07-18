using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.SafeComUC;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.SafeComUCPullPrinting
{
    [ToolboxItem(false)]
    public partial class SafeComUCPullPrintingConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.0";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Printer | AssetAttributes.ControlPanel;
        private PluginConfigurationData _pluginConfigurationData;
        private SafeComUCPullPrintingActivityData _activityData;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeComUCPullPrintingConfigurationControl" /> class.
        /// </summary>
        public SafeComUCPullPrintingConfigurationControl()
        {
            InitializeComponent();

            // Set up Auth Provider Combobox
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.SafeComUC, AuthenticationProvider.SafeComUC.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
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
            radioButton_Print.Tag = SafeComUCPullPrintAction.Print;
            radioButton_Retain.Tag = SafeComUCPullPrintAction.PrintRetain;
            radioButton_Unretain.Tag = SafeComUCPullPrintAction.PrintUnretain;
            radioButton_Delete.Tag = SafeComUCPullPrintAction.Delete;
            radioButton_PrintAll.Tag = SafeComUCPullPrintAction.PrintAll;
            radioButton_PrintAllApp.Tag = SafeComUCPullPrintAction.PrintAllApp;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new SafeComUCPullPrintingActivityData();
            assetSelectionControl.Initialize(_deviceAttributes);
            printingTaskConfigurationControl.Initialize();
            SetConfiguration();
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<SafeComUCPullPrintingActivityData>();
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
                case SafeComUCPullPrintAction.Print:
                    radioButton_Print.Checked = true;
                    break;
                case SafeComUCPullPrintAction.PrintAll:
                    radioButton_PrintAll.Checked = true;
                    break;
                case SafeComUCPullPrintAction.PrintRetain:
                case SafeComUCPullPrintAction.PrintRetainAll:
                    radioButton_Retain.Checked = true;                    
                    break;
                case SafeComUCPullPrintAction.PrintUnretain:
                case SafeComUCPullPrintAction.PrintUnretainAll:
                    radioButton_Unretain.Checked = true;
                    break;
                case SafeComUCPullPrintAction.Delete:
                case SafeComUCPullPrintAction.DeleteAll:
                    radioButton_Delete.Checked = true;
                    break;
                case SafeComUCPullPrintAction.PrintAllApp:
                    radioButton_PrintAllApp.Checked = true;
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
            SafeComUCPullPrintAction action = (SafeComUCPullPrintAction)selected.Tag;

            if (checkBox_SelectAll.Checked)
            {
                switch (action)
                {
                    case SafeComUCPullPrintAction.Delete:
                        action = SafeComUCPullPrintAction.DeleteAll;
                        break;
                    case SafeComUCPullPrintAction.PrintAll:
                        // Don't allow Select All for PrintAll action
                        checkBox_SelectAll.Checked = false;
                        break;
                    case SafeComUCPullPrintAction.PrintRetain:
                    case SafeComUCPullPrintAction.PrintRetainAll:
                        action = SafeComUCPullPrintAction.PrintRetain;
                        break;
                    case SafeComUCPullPrintAction.PrintUnretain:
                    case SafeComUCPullPrintAction.PrintUnretainAll:
                        action = SafeComUCPullPrintAction.PrintUnretain;
                        break;
                    case SafeComUCPullPrintAction.PrintAllApp:
                        action = SafeComUCPullPrintAction.PrintAllApp;
                        checkBox_SelectAll.Checked = false;
                        break;
                }
            }

            checkBox_SelectAll.Enabled = ((action != SafeComUCPullPrintAction.PrintAll) && (action!= SafeComUCPullPrintAction.PrintAllApp));
            numericUpDown_Copies.Enabled = (action != SafeComUCPullPrintAction.PrintAllApp);

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
