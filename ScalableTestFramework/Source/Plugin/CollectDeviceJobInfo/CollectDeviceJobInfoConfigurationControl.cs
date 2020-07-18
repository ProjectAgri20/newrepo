using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.CollectDeviceJobInfo
{
    /// <summary>
    /// Control used to configure the PluginCollectDeviceJobInfo execution data.
    /// </summary>
    [ToolboxItem(false)]
    public partial class CollectDeviceJobInfoConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private CollectDeviceJobInfoActivityData _data = null;
        public const string Version = "1.0";

        /// <summary>
        /// Initializes a new instance of this control.
        /// </summary>
        public CollectDeviceJobInfoConfigurationControl()
        {
            InitializeComponent();
            fieldValidator.RequireAssetSelection(assetSelectionControl);
            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
        }

        /// <summary>
        /// Event used to signal a change in configuration data has occurred.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Returns all of the PluginCollectDeviceJobInfo configuration data along with a version string.
        /// </summary>
        /// <returns>Configuration data and version string.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            return new PluginConfigurationData(_data, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData
            };
        }

        /// <summary>
        /// Initializes the configuration control to default values.
        /// </summary>
        /// <param name="environment">Domain and plugin specific environment data.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new CollectDeviceJobInfoActivityData();
            assetSelectionControl.Initialize(Framework.Assets.AssetAttributes.None);            
        }

        /// <summary>
        /// Initializes the configuration control with the supplied configuration settings.
        /// </summary>
        /// <param name="configuration">Pre-configured plugin settings.</param>
        /// <param name="environment">Domain and plugin specific environment data.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Deserialize the plugin specific configuration settings.            
            _data = configuration.GetMetadata<CollectDeviceJobInfoActivityData>();
            assetSelectionControl.Initialize(configuration.Assets, Framework.Assets.AssetAttributes.None);
        }

        /// <summary>
        /// Validate the configuration settings before saving.
        /// </summary>
        /// <returns>Information about the validation results.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Raises the ConfigurationChanged event.
        /// </summary>
        /// <param name="e">Contains any event data that should be sent with the event.</param>
        protected virtual void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }
    }
}
