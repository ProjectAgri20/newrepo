using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.HpacPullPrinting
{
    [ToolboxItem(false)]
    public partial class HpacPullPrintingConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.2";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Printer | AssetAttributes.ControlPanel;
        private PluginConfigurationData _pluginConfigurationData;
        private HpacActivityData _activityData;

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Creates a new HpacPullPrintingConfigControl
        /// </summary>
        public HpacPullPrintingConfigurationControl()
        {
            InitializeComponent();

            // Set up Authentication Provider Combobox
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpacDra, AuthenticationProvider.HpacDra.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpacIrm, AuthenticationProvider.HpacIrm.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpacAgentLess, AuthenticationProvider.HpacAgentLess.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpacPic, AuthenticationProvider.HpacPic.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpacWindows, AuthenticationProvider.HpacWindows.GetDescription()));
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
            radioButton_Hpac.CheckedChanged += OnConfigurationChanged;
            radioButton_SignInButton.CheckedChanged += OnConfigurationChanged;
            checkBox_SelectAll.CheckedChanged += OnConfigurationChanged;
            checkBoxReleaseOnSignIn.CheckedChanged += OnConfigurationChanged;
            printingTaskConfigurationControl.ConfigurationChanged += OnConfigurationChanged;
            comboBox_AuthProvider.SelectedIndexChanged += OnConfigurationChanged;
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;

            // Set Enums on Radio buttons
            radioButton_PrintDelete.Tag = HpacPullPrintAction.PrintDelete;
            radioButton_PrintKeep.Tag = HpacPullPrintAction.PrintKeep;
            radioButton_Delete.Tag = HpacPullPrintAction.Delete;
            radioButton_PrintAll.Tag = HpacPullPrintAction.PrintAll;
        }

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new HpacActivityData();
            assetSelectionControl.Initialize(_deviceAttributes);
            printingTaskConfigurationControl.Initialize();
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<HpacActivityData>(ConverterProvider.GetMetadataConverters());
            _pluginConfigurationData = configuration;

            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, _deviceAttributes);
            printingTaskConfigurationControl.Initialize(_pluginConfigurationData.Documents, _pluginConfigurationData.PrintQueues, _activityData.DelayAfterPrint, _activityData.ShuffleDocuments, _activityData.UsePrintServerNotification);
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            SetConfiguration();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.LockTimeouts = lockTimeoutControl.Value;
            _activityData.DeviceMemoryProfilerConfig = deviceMemoryProfilerControl.SelectedData;
            _activityData.HpacAuthentication = radioButton_Hpac.Checked;
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

        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            deviceMemoryProfilerControl.SelectedData = _activityData.DeviceMemoryProfilerConfig;
            checkBox_SelectAll.Checked = _activityData.SelectAll;
            checkBoxReleaseOnSignIn.Checked = _activityData.ReleaseOnSignIn;

            switch (_activityData.HpacAuthentication)
            {
                case true:
                    radioButton_Hpac.Checked = true;
                    break;
                case false:
                    radioButton_SignInButton.Checked = true;
                    break;
            }

            comboBox_AuthProvider.SelectedValue = _activityData.AuthProvider;

            switch (_activityData.DocumentProcessAction)
            {
                case HpacPullPrintAction.Delete:
                    radioButton_Delete.Checked = true;
                    break;
                case HpacPullPrintAction.PrintAll:
                    radioButton_PrintAll.Checked = true;
                    break;
                case HpacPullPrintAction.PrintDelete:
                    radioButton_PrintDelete.Checked = true;
                    break;
                case HpacPullPrintAction.PrintKeep:
                    radioButton_PrintKeep.Checked = true;
                    break;
            }
        }

        private HpacPullPrintAction GetAction()
        {
            RadioButton selected = groupBox_PullPrintConfig.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            return (HpacPullPrintAction)selected.Tag;
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        private void RadioButton_Action_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = (RadioButton)sender;

            //This filters out one of the two events that will fire from a CheckedChanged event.
            if (selected.Checked)
            {
                checkBox_SelectAll.Checked = checkBox_SelectAll.Checked && (!radioButton_PrintAll.Checked);
                checkBox_SelectAll.Enabled = !radioButton_PrintAll.Checked;
            }

            OnConfigurationChanged(this, e);
        }

        private void checkBoxReleaseOnSignIn_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxAuthentication.Enabled = !checkBoxReleaseOnSignIn.Checked;
            groupBox_PullPrintConfig.Enabled = !checkBoxReleaseOnSignIn.Checked;
        }

        private void comboBox_AuthProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_AuthProvider.Text == AuthenticationProvider.HpacAgentLess.GetDescription())
            {
                radioButton_SignInButton.Checked = true;
                radioButton_Hpac.Enabled = false;
            }
            else if(!radioButton_Hpac.Enabled)
            {
                radioButton_Hpac.Enabled = true;                
            }
        }
    }
}
