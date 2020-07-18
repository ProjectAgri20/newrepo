using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.UserIntervention
{
    /// <summary>
    /// Configuration Control Module for the Plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class UserInterventionConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private UserInterventionActivityData _pluginData;
        public UserInterventionConfigurationControl()
        {
            InitializeComponent();
        }

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Gets the Configuration Settings from the UI to the Activity Data
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            _pluginData.AlertMessage = AlertTextBox.Text;
            return new PluginConfigurationData(_pluginData, "1.0");
        }

        /// <summary>
        /// Initialize the Control for new MetaData
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            _pluginData = new UserInterventionActivityData();
        }

        /// <summary>
        /// Initializes the Control for the Existing MetaData
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _pluginData = configuration.GetMetadata<UserInterventionActivityData>();
            if (!string.IsNullOrEmpty(_pluginData.AlertMessage))
            {
                AlertTextBox.Text = _pluginData.AlertMessage;
            }
        }

        /// <summary>
        /// Validates the values Input by user in Configuration Control
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(FieldValidator.HasValue(AlertTextBox, "Alert message"));
        }
    }
}
