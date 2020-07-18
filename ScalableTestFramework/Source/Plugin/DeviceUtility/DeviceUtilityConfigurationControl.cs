using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.DeviceUtility
{
    [ToolboxItem(false)]
    public partial class DeviceUtilityConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private const string Version = "1.0";
        private PluginConfigurationData _pluginConfigurationData;
        private RebootDeviceActivityData _activityData;

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Creates a new HpacPullPrintingConfigControl
        /// </summary>
        public DeviceUtilityConfigurationControl()
        {
            InitializeComponent();

            PluginActionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            PluginActionComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            // Only the value AutoCompleteMode.None can be used when DropDownStyle is ComboBoxStyle.DropDownList and AutoCompleteSource is not AutoCompleteSource.ListItems.
            PluginActionComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            PluginActionComboBox.DataSource = Enum.GetValues(typeof(DeviceUtilityAction))
                .Cast<DeviceUtilityAction>()
                .Select(enumVal => new { Key = enumVal, Display = enumVal.ToString() })
                .ToList();
            PluginActionComboBox.DisplayMember = "Display";
            PluginActionComboBox.ValueMember = "Key";

            PaperlessModeAfterRebootComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            PaperlessModeAfterRebootComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            // Only the value AutoCompleteMode.None can be used when DropDownStyle is ComboBoxStyle.DropDownList and AutoCompleteSource is not AutoCompleteSource.ListItems.
            PaperlessModeAfterRebootComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            PaperlessModeAfterRebootComboBox.DataSource = Enum.GetValues(typeof(JobMediaModeDesired))
                .Cast<JobMediaModeDesired>()
                .Select(enumVal => new { Key = enumVal, Display = enumVal.ToString() })
                .ToList();
            PaperlessModeAfterRebootComboBox.DisplayMember = "Display";
            PaperlessModeAfterRebootComboBox.ValueMember = "Key";

            // Set up field validation
            FieldValidator.RequireAssetSelection(AssetSelectionControl, "device");

            // Set up change notification
            PluginActionComboBox.SelectedIndexChanged += (s, e) => OnConfigurationChanged(s, e);
            AssetSelectionControl.SelectionChanged += (s, e) => OnConfigurationChanged(s, e);
            LockTimeoutControl.ValueChanged += (s, e) => OnConfigurationChanged(s, e);
            WaitForReadyCheckBox.CheckedChanged += (s, e) => OnConfigurationChanged(s, e);
            PaperlessModeAfterRebootComboBox.SelectedIndexChanged += (s, e) => OnConfigurationChanged(s, e);
        }

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new RebootDeviceActivityData();
            AssetSelectionControl.Initialize(AssetAttributes.None);
            Initialize(_activityData);
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _pluginConfigurationData = configuration;
            _activityData = configuration.GetMetadata<RebootDeviceActivityData>();
            AssetSelectionControl.Initialize(configuration.Assets, AssetAttributes.None);
            Initialize(_activityData);
        }

        private void Initialize(RebootDeviceActivityData activityData)
        {
            // Cannot set the SelectedValue in a ListControl with an empty ValueMember.
            PluginActionComboBox.SelectedValue = activityData.DeviceUtilityAction;

            // Plugin task-specific settings
            WaitForReadyCheckBox.Checked = activityData.ShouldWaitForReady;

            // Plugin behavior
            LockTimeoutControl.Initialize(_activityData.LockTimeouts);
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.LockTimeouts = LockTimeoutControl.Value;

            return new PluginConfigurationData(_activityData, Version)
            {
                Assets = AssetSelectionControl.AssetSelectionData,
            };
        }

        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(FieldValidator.ValidateAll());

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            PaperlessModeAfterRebootComboBox.Enabled = WaitForReadyCheckBox.Checked;
            ConfigurationChanged?.Invoke(this, e);
        }
    }
}
