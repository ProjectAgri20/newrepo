using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.SafeQ;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.SafeQPullPrinting
{
    [ToolboxItem(false)]
    public partial class SafeQPullPrintingConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.0";
        private SafeQPullPrintingActivityData _activityData;
        private PluginConfigurationData _pluginConfigurationData;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeQPrintConfigurationControl" /> class.
        /// </summary>
        public SafeQPullPrintingConfigurationControl()
        {
            InitializeComponent();

            //GetDataSource 
            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.SafeQ, AuthenticationProvider.SafeQ.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Auto, AuthenticationProvider.Auto.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.DSS, AuthenticationProvider.DSS.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));
            comboBox_AuthProvider.DataSource = authProviders;
            comboBox_ColorMode.DataSource = EnumUtil.GetDescriptions<SafeQPrintColorMode>().ToList();
            comboBox_Sides.DataSource = EnumUtil.GetDescriptions<SafeQPrintSides>().ToList();

            //Set up field validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            fieldValidator.RequireCustom(deviceMemoryProfilerControl, deviceMemoryProfilerControl.ValidateMemoryCollectionSettings);

            //Set up change notification
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
            lockTimeoutControl.ValueChanged += OnConfigurationChanged;
            checkBox_ReleaseOnSignIn.CheckedChanged += OnConfigurationChanged;
            radioButton_SignInButton.CheckedChanged += OnConfigurationChanged;
            radioButton_SafeQPrint.CheckedChanged += OnConfigurationChanged;
            radioButton_PrintAll.CheckedChanged += OnConfigurationChanged;
            radioButton_Print.CheckedChanged += OnConfigurationChanged;
            radioButton_Delete.CheckedChanged += OnConfigurationChanged;
            comboBox_AuthProvider.SelectedIndexChanged += OnConfigurationChanged;
            checkBox_SelectAll.CheckedChanged += OnConfigurationChanged;
            numericUpDown_copies.ValueChanged += OnConfigurationChanged;
            deviceMemoryProfilerControl.SelectionChanged += OnConfigurationChanged;
            printingConfigurationControl.ConfigurationChanged += OnConfigurationChanged;

            //Set Enums on Radio buttons
            radioButton_PrintAll.Tag = SafeQPrintPullPrintAction.PrintAll;
            radioButton_Print.Tag = SafeQPrintPullPrintAction.Print;
            radioButton_Delete.Tag = SafeQPrintPullPrintAction.Delete;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new SafeQPullPrintingActivityData();
            assetSelectionControl.Initialize(AssetAttributes.None);
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
            _activityData = configuration.GetMetadata<SafeQPullPrintingActivityData>();
            _pluginConfigurationData = configuration;

            assetSelectionControl.Initialize(_pluginConfigurationData.Assets, AssetAttributes.None);
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
            _activityData.SafeQPrintAuthentication = radioButton_SafeQPrint.Checked;
            _activityData.AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue;

            _activityData.SelectAll = checkBox_SelectAll.Checked;
            _activityData.ReleaseOnSignIn = checkBox_ReleaseOnSignIn.Checked;
            _activityData.DocumentProcessAction = GetPullPrintAction();
            _activityData.NumberOfCopies = (int)numericUpDown_copies.Value;
            //
            _activityData.ColorMode = EnumUtil.GetByDescription<SafeQPrintColorMode>(comboBox_ColorMode.Text);
            _activityData.Sides = EnumUtil.GetByDescription<SafeQPrintSides>(comboBox_Sides.Text);
            //
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
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            if(radioButton_PrintAll.Checked == true || checkBox_SelectAll.Checked == true)
            {
                comboBox_ColorMode.Enabled = false;
                comboBox_Sides.Enabled = false;
            }
            else
            {
                comboBox_ColorMode.Enabled = true;
                comboBox_Sides.Enabled = true;
            }

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

            switch (_activityData.SafeQPrintAuthentication)
            {
                case true:
                    radioButton_SafeQPrint.Checked = true;
                    break;
                case false:
                    radioButton_SignInButton.Checked = true;
                    break;
            }

            comboBox_AuthProvider.SelectedValue = _activityData.AuthProvider;

            SetPullPrintAction();
            numericUpDown_copies.Value = _activityData.NumberOfCopies;
            //
            comboBox_ColorMode.Text = _activityData.ColorMode.GetDescription();
            comboBox_Sides.Text = _activityData.Sides.GetDescription();
            //

        }

        /// <summary>
        /// Set pull print action: Print or Delete
        /// </summary>
        private void SetPullPrintAction()
        {
            switch (_activityData.DocumentProcessAction)
            {
                case SafeQPrintPullPrintAction.PrintAll:
                    radioButton_PrintAll.Checked = true;
                    break;
                case SafeQPrintPullPrintAction.Print:
                    radioButton_Print.Checked = true;
                    break;
                case SafeQPrintPullPrintAction.Delete:
                    radioButton_Delete.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// Get pull print action: Print or Delete
        /// </summary>
        private SafeQPrintPullPrintAction GetPullPrintAction()
        {
            RadioButton selected = groupBox_PullPrint.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            return (SafeQPrintPullPrintAction)selected.Tag;
        }
    }
}
