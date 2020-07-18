using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.MyQPullPrinting
{
    [ToolboxItem(false)]
    public partial class MyQPullPrintingConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "2.0";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Printer | AssetAttributes.ControlPanel;
        private PluginConfigurationData _pluginConfigurationData;
        private MyQPullPrintingActivityData _data;
        //private PluginEnvironment _environment;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyQPullPrintingConfigurationControl" /> class.
        /// </summary>
        public MyQPullPrintingConfigurationControl()
        {
            InitializeComponent();

            //set up field validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            //set up Auth Provider Combobox
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.MyQ, AuthenticationProvider.MyQ.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.DSS, AuthenticationProvider.DSS.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Auto, AuthenticationProvider.Auto.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));
            comboBox_AuthProvider.DataSource = authProviders;

            //set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            comboBox_AuthProvider.SelectedIndexChanged += OnConfigurationChanged;
            checkBox_SelectAll.CheckedChanged += OnConfigurationChanged;
            radioButton_PrintAll.CheckedChanged += OnConfigurationChanged;
            radioButton_Print.CheckedChanged += OnConfigurationChanged;
            radioButton_Delete.CheckedChanged += OnConfigurationChanged;
            
            //set Enums on Radio Button
            radioButton_PrintAll.Tag = MyQPullPrintAction.PrintAll;
            radioButton_Print.Tag = MyQPullPrintAction.Print;
            radioButton_Delete.Tag = MyQPullPrintAction.Delete;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new MyQPullPrintingActivityData();
            assetSelectionControl.Initialize(_deviceAttributes);
            printingConfigurationControl.Initialize();
            lockTimeoutControl.Initialize(_data.LockTimeouts);
            SetPullPrintAction();
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<MyQPullPrintingActivityData>();
            _pluginConfigurationData = configuration;

            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, _deviceAttributes);
            printingConfigurationControl.Initialize(_pluginConfigurationData.Documents, _pluginConfigurationData.PrintQueues, _data.DelayAfterPrint, _data.ShuffleDocuments, _data.UsePrintServerNotification);
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
            _data.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;
            _data.DocumentProcessAction = GetPullPrintAction();
            _data.SelectAll = checkBox_SelectAll.Checked;
            _data.ShuffleDocuments = printingConfigurationControl.GetShuffle();
            _data.DelayAfterPrint = printingConfigurationControl.GetDelay();
            _data.UsePrintServerNotification = printingConfigurationControl.GetPrintServerNotification();

            return new PluginConfigurationData(_data, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData,
                PrintQueues = printingConfigurationControl.GetPrintQueues(),
                Documents = printingConfigurationControl.GetDocuments()
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

        /// <summary>
        /// Set configuration by activity data
        /// </summary>
        private void SetConfiguration()
        {
            lockTimeoutControl.Initialize(_data.LockTimeouts);
            deviceMemoryProfilerControl.SelectedData = _data.DeviceMemoryProfilerConfig;

            comboBox_AuthProvider.SelectedValue = _data.AuthProvider;
            SetPullPrintAction();
        }

        /// <summary>
        /// Set pull print action: Print or Delete
        /// </summary>
        private void SetPullPrintAction()
        {
            switch (_data.DocumentProcessAction)
            {
                case MyQPullPrintAction.PrintAll:
                    radioButton_PrintAll.Checked = true;
                    break;
                case MyQPullPrintAction.Print:
                    radioButton_Print.Checked = true;
                    break;
                case MyQPullPrintAction.Delete:
                    radioButton_Delete.Checked = true;
                    break;
            }
            if (_data.SelectAll)
            {
                checkBox_SelectAll.Checked = true;
            }
        }

        private MyQPullPrintAction GetPullPrintAction()
        {
            RadioButton selected = groupBox_PullPrintConfig.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            return (MyQPullPrintAction)selected.Tag;
        }

        private void checkBox_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_PrintAll.Checked)
            {
                checkBox_SelectAll.Checked = false;
            }
        }
    }
}
