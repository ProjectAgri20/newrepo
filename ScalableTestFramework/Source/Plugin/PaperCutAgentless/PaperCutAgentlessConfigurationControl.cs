using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Plugin.PaperCutAgentless.PaperCutAgentlessActivityData;

namespace HP.ScalableTest.Plugin.PaperCutAgentless
{
    /// <summary>
    /// Manages plugin configuration data for PaperCutAgentless automated operations.
    /// </summary>
    [ToolboxItem(false)]
    public partial class PaperCutAgentlessConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.0";
        private const AssetAttributes _deviceAttributes = AssetAttributes.Printer | AssetAttributes.ControlPanel;
        private PluginConfigurationData _pluginConfigurationData;
        private PaperCutAgentlessActivityData _activityData;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaperCutAgentlessConfigurationControl" /> class.
        /// </summary>
        public PaperCutAgentlessConfigurationControl()
        {
            InitializeComponent();

            //Set up Auth Provider Combobox
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.PaperCutAgentless, AuthenticationProvider.PaperCutAgentless.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.DSS, AuthenticationProvider.DSS.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Auto, AuthenticationProvider.Auto.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));

            comboBox_AuthProvider.DataSource = authProviders;

            // Set up filed validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");

            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            // Set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;
            checkBox_SelectAll.CheckedChanged += OnConfigurationChanged;
            radioButton_Print.CheckedChanged += OnConfigurationChanged;
            radioButton_Delete.CheckedChanged += OnConfigurationChanged;
            checkBox_Force2sided.CheckedChanged += OnConfigurationChanged;
            checkBox_ForceGrayscale.CheckedChanged += OnConfigurationChanged;
            checkBox_UseSingleJobOptions.CheckedChanged += OnConfigurationChanged;
            radioButton_1sided.CheckedChanged += OnConfigurationChanged;
            radioButton_2sided.CheckedChanged += OnConfigurationChanged;
            radioButton_Color.CheckedChanged += OnConfigurationChanged;
            radioButton_GrayScale.CheckedChanged += OnConfigurationChanged;            
            checkBox_ReleaseOnSignIn.CheckedChanged += OnConfigurationChanged;
            printingConfigurationControl.ConfigurationChanged += OnConfigurationChanged;
            comboBox_AuthProvider.SelectedIndexChanged += OnConfigurationChanged;

            // Set Enums on Radio buttons
            radioButton_Print.Tag = PaperCutAgentlessPullPrintAction.Print;
            radioButton_Delete.Tag = PaperCutAgentlessPullPrintAction.Delete;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new PaperCutAgentlessActivityData();
            assetSelectionControl.Initialize(_deviceAttributes);
            printingConfigurationControl.Initialize();
            lockTimeoutControl.Initialize(_activityData.LockTimeouts);
            SetPullPrintAction();
            SetSingleJobOptions();
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<PaperCutAgentlessActivityData>();
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
            _activityData.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;

            _activityData.SelectAll = checkBox_SelectAll.Checked;
            _activityData.Force2sided = checkBox_Force2sided.Checked;
            _activityData.ForceGrayscale = checkBox_ForceGrayscale.Checked;
            _activityData.DocumentProcessAction = GetPullPrintAction();
            _activityData.ReleaseOnSignIn = checkBox_ReleaseOnSignIn.Checked;
            _activityData.ShuffleDocuments = printingConfigurationControl.GetShuffle();
            _activityData.DelayAfterPrint = printingConfigurationControl.GetDelay();
            _activityData.UsePrintServerNotification = printingConfigurationControl.GetPrintServerNotification();

            _activityData.UseSingleJobOptions = checkBox_UseSingleJobOptions.Checked;
            _activityData.SingleJobCopies = (int)numericUpDown_Copies.Value;
            _activityData.SingleJobDuplex = radioButton_2sided.Checked;
            _activityData.SingleJobGrayScale = radioButton_GrayScale.Checked;

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
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
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
            checkBox_ForceGrayscale.Checked = _activityData.ForceGrayscale;
            checkBox_Force2sided.Checked = _activityData.Force2sided;
            checkBox_UseSingleJobOptions.Checked = _activityData.UseSingleJobOptions;
            checkBox_ReleaseOnSignIn.Checked = _activityData.ReleaseOnSignIn;
            comboBox_AuthProvider.SelectedValue = _activityData.AuthProvider;            
            SetPullPrintAction();
            SetSingleJobOptions();
        }

        private void SetPullPrintAction()
        {
            switch (_activityData.DocumentProcessAction)
            {
                case PaperCutAgentlessPullPrintAction.Print:
                    radioButton_Print.Checked = true;
                    break;
                case PaperCutAgentlessPullPrintAction.Delete:
                    radioButton_Delete.Checked = true;
                    break;
            }
        }

        private void SetSingleJobOptions()
        {
            groupBox_SingleJobOptions.Enabled = _activityData.UseSingleJobOptions;

            numericUpDown_Copies.Value = (decimal) _activityData.SingleJobCopies;

            if (_activityData.SingleJobDuplex)
            {
                radioButton_2sided.Checked = true;
            }
            else
            {
                radioButton_1sided.Checked = true;
            }

            if (_activityData.SingleJobGrayScale)
            {
                radioButton_GrayScale.Checked = true;
            }
            else
            {
                radioButton_Color.Checked = true;
            }
            
        }

        private PaperCutAgentlessPullPrintAction GetPullPrintAction()
        {
            RadioButton selected = groupBox_PullPrintConfig.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            return (PaperCutAgentlessPullPrintAction)selected.Tag;
        }

        private void checkBox_UseSingleJobOptions_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_SingleJobOptions.Enabled = checkBox_UseSingleJobOptions.Checked;
            checkBox_SelectAll.Enabled = !checkBox_UseSingleJobOptions.Checked;
            checkBox_Force2sided.Enabled = !checkBox_UseSingleJobOptions.Checked;
            checkBox_ForceGrayscale.Enabled = !checkBox_UseSingleJobOptions.Checked;
        }

        private void radioButton_DocumentAction_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_SingleJobOptions.Enabled = !radioButton_Delete.Checked;            
            checkBox_UseSingleJobOptions.Enabled = !radioButton_Delete.Checked;
            checkBox_Force2sided.Enabled = !radioButton_Delete.Checked;
            checkBox_ForceGrayscale.Enabled = !radioButton_Delete.Checked;

            if (radioButton_Delete.Checked)
            {
                checkBox_SelectAll.Enabled = true;
                checkBox_UseSingleJobOptions.Checked = false;
            }            

        }
    }
}
