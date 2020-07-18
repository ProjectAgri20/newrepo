using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;

namespace $safenamespace$
{
    /// <summary>
    /// Control used to configure the $safeclassprefix$ execution data.
    /// </summary>
    public partial class $safeclassprefix$ConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private $safeclassprefix$ActivityData _data = null;

        /// <summary>
        /// Initializes a new instance of this control.
        /// </summary>
        public $safeclassprefix$ConfigurationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event used to signal a change in configuration data has occurred.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Returns all of the $safeclassprefix$ configuration data along with a version string.
        /// </summary>
        /// <returns>Configuration data and version string.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            return new PluginConfigurationData(_data, "1.0");
        }

        /// <summary>
        /// Initializes the configuration control to default values.
        /// </summary>
        /// <param name="environment">Domain and plugin specific environment data.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new $safeclassprefix$ActivityData();
        }

        /// <summary>
        /// Initializes the configuration control with the supplied configuration settings.
        /// </summary>
        /// <param name="configuration">Pre-configured plugin settings.</param>
        /// <param name="environment">Domain and plugin specific environment data.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Deserialize the plugin specific configuration settings.
            _data = configuration.GetMetadata<$safeclassprefix$ActivityData>();
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
