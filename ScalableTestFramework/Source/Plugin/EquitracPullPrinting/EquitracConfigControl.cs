using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.EquitracPullPrinting
{
    [ToolboxItem(false)]
    public partial class EquitracConfigControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.3";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Printer | AssetAttributes.ControlPanel;
        private PluginConfigurationData _pluginConfigurationData;
        private EquitracActivityData _activityData;

        public event EventHandler ConfigurationChanged;

        public EquitracConfigControl()
        {
            InitializeComponent();

            // Set up Auth Provider Combobox
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Equitrac, AuthenticationProvider.Equitrac.GetDescription()));            
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.EquitracWindows, AuthenticationProvider.EquitracWindows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Auto, AuthenticationProvider.Auto.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));

            comboBox_AuthProvider.DataSource = authProviders;

            // Set up field validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            // Set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;
            radioButtonEquitrac.CheckedChanged += OnConfigurationChanged;
            radioButtonSignInButton.CheckedChanged += OnConfigurationChanged;
            numericUpDownCopies.ValueChanged += OnConfigurationChanged;
            checkBoxSelectAll.CheckedChanged += OnConfigurationChanged;
            printingTaskConfigurationControl.ConfigurationChanged += (s, e) => OnConfigurationChanged(s, e);
            comboBox_AuthProvider.SelectedIndexChanged += OnConfigurationChanged;

            // Set Enums on Radio buttons
            radioButtonPrint.Tag = EquitracPullPrintAction.Print;
            radioButtonPrintSave.Tag = EquitracPullPrintAction.PrintSave;
            radioButtonDelete.Tag = EquitracPullPrintAction.Delete;

        }

        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            deviceMemoryProfilerControl.SelectedData = _activityData.DeviceMemoryProfilerConfig;
            checkBoxSelectAll.Checked = _activityData.SelectAll;
            numericUpDownCopies.Value = _activityData.NumberOfCopies;

            switch (_activityData.EquitracAuthentication)
            {
                case true:
                    radioButtonEquitrac.Checked = true;
                    break;
                case false:
                    radioButtonSignInButton.Checked = true;
                    break;
            }

            comboBox_AuthProvider.SelectedValue = _activityData.AuthProvider;

            switch (_activityData.DocumentProcessAction)
            {
                case EquitracPullPrintAction.Print:
                    radioButtonPrint.Checked = true;
                    break;
                case EquitracPullPrintAction.PrintSave:
                    radioButtonPrintSave.Checked = true;
                    break;
                case EquitracPullPrintAction.Delete:
                    radioButtonDelete.Checked = true;
                    break;
            }
            //Equitrac doesn't like the new convention
            //printingTaskConfigurationControl.Initialize(_activityData.PrintingTaskData, _pluginConfigurationData.Documents, _pluginConfigurationData.PrintQueues);
            //printingTaskConfigurationControl.Initialize(_activityData.PrintingTaskData);
            checkBoxServerSelection.Checked = false;

        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.LockTimeouts = lockTimeoutControl.Value;
            _activityData.DeviceMemoryProfilerConfig = deviceMemoryProfilerControl.SelectedData;
            _activityData.EquitracAuthentication = radioButtonEquitrac.Checked;
            _activityData.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;
            _activityData.EquitracServer = string.Empty;
            _activityData.SelectAll = checkBoxSelectAll.Checked;
            _activityData.NumberOfCopies = (int)numericUpDownCopies.Value;

            _activityData.ShuffleDocuments = printingTaskConfigurationControl.GetShuffle();
            _activityData.DelayAfterPrint = printingTaskConfigurationControl.GetDelay();
            _activityData.UsePrintServerNotification = printingTaskConfigurationControl.GetPrintServerNotification();
          

            return new PluginConfigurationData(_activityData, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData,
                PrintQueues = printingTaskConfigurationControl.GetPrintQueues(),
                Documents = printingTaskConfigurationControl.GetDocuments()
                //PrintQueues = _activityData.PrintingTaskData.PrintQueueSelectionData,
                //Documents = _activityData.PrintingTaskData.DocumentSelectionData
            };
        }
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new EquitracActivityData();
            assetSelectionControl.Initialize(_deviceAttributes);
            printingTaskConfigurationControl.Initialize();
            SetConfiguration();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<EquitracActivityData>(ConverterProvider.GetMetadataConverters());
            _pluginConfigurationData = configuration;
            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, _deviceAttributes);

            printingTaskConfigurationControl.Initialize(_pluginConfigurationData.Documents, _pluginConfigurationData.PrintQueues, _activityData.DelayAfterPrint, _activityData.ShuffleDocuments, _activityData.UsePrintServerNotification);

            SetConfiguration();
        }

        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        private void RadioButtonDocumentProcessCheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = (RadioButton)sender;

            _activityData.DocumentProcessAction = (EquitracPullPrintAction)selected.Tag;
            OnConfigurationChanged(this, e);
        }

    }
}
