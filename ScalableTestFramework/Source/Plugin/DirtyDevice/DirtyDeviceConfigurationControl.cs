using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    [ToolboxItem(false)]
    public partial class DirtyDeviceConfigurationControl : UserControl, IPluginConfigurationControl
    {
        public const string Version = "1.1";
        private PluginConfigurationData _pluginConfigurationData;

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Creates a new DirtyDeviceConfigurationControl
        /// </summary>
        public DirtyDeviceConfigurationControl()
        {
            InitializeComponent();

            // Set up field validation
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");

            // Set up change notification
            assetSelectionControl.SelectionChanged += ConfigurationChanged;
            lockTimeoutControl.ValueChanged += ConfigurationChanged;
        }

        public void Initialize(PluginEnvironment environment)
        {
            dirtyDeviceSettings.Value = new DirtyDeviceActivityData();
            assetSelectionControl.Initialize(AssetAttributes.None);
            Initialize(dirtyDeviceSettings.Value);
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _pluginConfigurationData = configuration;
            assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.None);
            Initialize(configuration.GetMetadata<DirtyDeviceActivityData>(new[] { new DirtyDeviceDataConverter1_1() }));
        }

        private void Initialize(DirtyDeviceActivityData activityData)
        {
            // Copy plugin task-specific settings from activity data to control here...
            dirtyDeviceSettings.Value = activityData;
            // Plugin behavior
            lockTimeoutControl.Initialize(activityData.LockTimeouts);
        }

        public PluginConfigurationData GetConfiguration()
        {
            var activityData = dirtyDeviceSettings.Value;
            activityData.LockTimeouts = lockTimeoutControl.Value;

            return new PluginConfigurationData(activityData, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData,
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(
                fieldValidator.ValidateAll()
                .Union(dirtyDeviceSettings.ValidateConfiguration()));
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }
    }
}
