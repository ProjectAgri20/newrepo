using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.SyncPoint
{
    [ToolboxItem(false)]
    public partial class SyncPointConfigurationControl : UserControl, IPluginConfigurationControl
    {
        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncPointConfigurationControl" /> class.
        /// </summary>
        public SyncPointConfigurationControl()
        {
            InitializeComponent();

            fieldValidator.RequireValue(eventName_TextBox, "Sync event name");

            eventName_TextBox.TextChanged += (s, e) => ConfigurationChanged?.Invoke(this, EventArgs.Empty);
            signalEvent_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged?.Invoke(this, EventArgs.Empty);
            waitForEvent_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            eventName_TextBox.Clear();
            signalEvent_RadioButton.Checked = true;
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            SyncPointActivityData data = configuration.GetMetadata<SyncPointActivityData>();

            eventName_TextBox.Text = data.EventName;
            if (data.Action == SyncPointAction.Signal)
            {
                signalEvent_RadioButton.Checked = true;
            }
            else
            {
                waitForEvent_RadioButton.Checked = true;
            }
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            SyncPointActivityData activityData = new SyncPointActivityData
            {
                EventName = eventName_TextBox.Text,
                Action = signalEvent_RadioButton.Checked ? SyncPointAction.Signal : SyncPointAction.Wait
            };

            return new PluginConfigurationData(activityData, "1.0");
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
    }
}
