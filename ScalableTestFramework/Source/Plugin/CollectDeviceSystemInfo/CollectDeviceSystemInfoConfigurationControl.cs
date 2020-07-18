using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.CollectDeviceSystemInfo
{
    /// <summary>
    /// Control used to configure the PluginCollectDeviceSystemInfo execution data.
    /// </summary>
    [ToolboxItem(false)]
    public partial class CollectDeviceSystemInfoConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private CollectDeviceSystemInfoActivityData _data = null;
        public const string Version = "1.1";

        /// <summary>
        /// Initializes a new instance of this control.
        /// </summary>
        public CollectDeviceSystemInfoConfigurationControl()
        {
            InitializeComponent();
            label_MemoryCollectionNotes.Text = "NOTE: Minimum collection interval is 4 minutes, regardless of officeworker pacing. \n\rOfficeworker pacing interval greater than 4 minutes is recommended.";

            assetSelectionControl.SelectionChanged += OnConfigurationChanged;
        }

        /// <summary>
        /// Event used to signal a change in configuration data has occurred.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Returns all of the PluginCollectDeviceSystemInfo configuration data along with a version string.
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
            _data = new CollectDeviceSystemInfoActivityData();
            assetSelectionControl.Initialize(HP.ScalableTest.Framework.Assets.AssetAttributes.None);
        }

        /// <summary>
        /// Initializes the configuration control with the supplied configuration settings.
        /// </summary>
        /// <param name="configuration">Pre-configured plugin settings.</param>
        /// <param name="environment">Domain and plugin specific environment data.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Deserialize the plugin specific configuration settings.
            _data = configuration.GetMetadata<CollectDeviceSystemInfoActivityData>(ConverterProvider.GetMetadataConverters());
            assetSelectionControl.Initialize(configuration.Assets, HP.ScalableTest.Framework.Assets.AssetAttributes.None);
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
