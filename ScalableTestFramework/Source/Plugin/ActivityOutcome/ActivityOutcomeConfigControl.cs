using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.ActivityOutcome
{
    /// <summary>
    /// Edit control for a demo Activity Plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class ActivityOutcomeConfigControl : UserControl, IPluginConfigurationControl
    {
        private Dictionary<PluginResult, RadioButton> _outcomeButtons = new Dictionary<PluginResult, RadioButton>();

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityOutcomeConfigControl"/> class.
        /// </summary>
        public ActivityOutcomeConfigControl()
        {
            InitializeComponent();

            _outcomeButtons.Add(PluginResult.Failed, fail_RadioButton);
            _outcomeButtons.Add(PluginResult.Skipped, skip_RadioButton);
            _outcomeButtons.Add(PluginResult.Passed, success_RadioButton);
            _outcomeButtons.Add(PluginResult.Error, error_radioButton);

            foreach (RadioButton radioButton in _outcomeButtons.Values)
            {
                radioButton.CheckedChanged += OnConfigurationChanged;
            }
            randomOutcome_CheckBox.CheckedChanged += OnConfigurationChanged;
        }

        public void Initialize(PluginEnvironment environment)
        {
            success_RadioButton.Checked = true;
            randomOutcome_CheckBox.Checked = false;
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            ActivityOutcomeData data = configuration.GetMetadata<ActivityOutcomeData>();
            _outcomeButtons[data.Result].Checked = true;
            randomOutcome_CheckBox.Checked = data.RandomResult;
        }

        public PluginConfigurationData GetConfiguration()
        {
            ActivityOutcomeData data = new ActivityOutcomeData()
            {
                Result = _outcomeButtons.First(n => n.Value.Checked).Key,
                RandomResult = randomOutcome_CheckBox.Checked
            };

            return new PluginConfigurationData(data, "1.0");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            // This control has no validation
            return new PluginValidationResult(true);
        }

        private void CheckBoxRandomCheckedChanged(object sender, EventArgs e)
        {
            groupBoxOutcome.Enabled = !randomOutcome_CheckBox.Checked;
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(this, EventArgs.Empty);
            }
        }
    }
}