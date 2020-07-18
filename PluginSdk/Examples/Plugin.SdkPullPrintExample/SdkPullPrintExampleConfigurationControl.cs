using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;

namespace Plugin.SdkPullPrintExample
{
    /// <summary>
    /// Control used to configure the LocalPullPrintExample execution data.
    /// </summary>
    public partial class SdkPullPrintExampleConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private SdkPullPrintExampleActivityData _data = null;

        /// <summary>
        /// Initializes a new instance of this control.
        /// </summary>
        public SdkPullPrintExampleConfigurationControl()
        {
            InitializeComponent();

            // Initialize the asset selection control to only show assets (devices) that have a control panel
            assetSelectionControl.Initialize(HP.ScalableTest.Framework.Assets.AssetAttributes.ControlPanel);

            // Monitor changes to selected asset
            assetSelectionControl.SelectionChanged += (s, e) => OnConfigurationChanged(e);

            // Require that at least one asset be selected
            fieldValidator.RequireAssetSelection(assetSelectionControl);

            // Require that the top level button name is defined
            fieldValidator.RequireValue(textBoxButtonName, labelButtonName);
        }

        /// <summary>
        /// Event used to signal a change in configuration data has occurred.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Returns all of the LocalPullPrintExample configuration data along with a version string.
        /// </summary>
        /// <returns>Configuration data and version string.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            // Update data object with button name specified in UI
            _data.TopLevelButtonName = textBoxButtonName.Text;

            // Add the selected assets to the plugin configuration data 
            return new PluginConfigurationData(_data, "1.0")
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
            _data = new SdkPullPrintExampleActivityData();
        }

        /// <summary>
        /// Initializes the configuration control with the supplied configuration settings.
        /// </summary>
        /// <param name="configuration">Pre-configured plugin settings.</param>
        /// <param name="environment">Domain and plugin specific environment data.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Deserialize the plugin specific configuration settings.
            _data = configuration.GetMetadata<SdkPullPrintExampleActivityData>();

            // Initialize the asset selection control with current selection stored in data
            assetSelectionControl.Initialize(configuration.Assets, HP.ScalableTest.Framework.Assets.AssetAttributes.None);

            // Set the top level button name from the configuration data
            textBoxButtonName.Text = _data.TopLevelButtonName;
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
        protected virtual void OnConfigurationChanged(EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }
    }
}
