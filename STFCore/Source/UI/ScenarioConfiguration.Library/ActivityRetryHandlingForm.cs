using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Provides an interface for setting activity exception handling options.
    /// </summary>
    internal partial class ActivityRetryHandlingForm : Form
    {
        private const int ACTION_COLUMN = 1;
        private const int RETRY_LIMIT_COLUMN = 2;
        private const int RETRY_DELAY_COLUMN = 3;
        private const int RETRY_ACTION_COLUMN = 4;
        private const int ERROR_ROW = 3;
        private const int SKIP_ROW = 1;
        private const int FAIL_ROW = 2;

        private string RETRY = EnumUtil.GetDescription(PluginRetryAction.Retry);
        private string CONTINUE = EnumUtil.GetDescription(PluginRetryAction.Continue);

        private List<WorkerActivityConfiguration> _configurations;

        public ActivityRetryHandlingForm(IEnumerable<WorkerActivityConfiguration> configurations)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);

            for (int i = 1; i < main_TableLayoutPanel.RowCount; i++)
            {
                LoadActionItems((ComboBox)main_TableLayoutPanel.GetControlFromPosition(ACTION_COLUMN, i));
                LoadRetryActionItems((ComboBox)main_TableLayoutPanel.GetControlFromPosition(RETRY_ACTION_COLUMN, i));
            }

            LoadDefaultDefinitions();

            _configurations = configurations.ToList();

            List<VirtualResourceMetadataRetrySetting> failedDefinitions = new List<VirtualResourceMetadataRetrySetting>();
            List<VirtualResourceMetadataRetrySetting> skippedDefinitions = new List<VirtualResourceMetadataRetrySetting>();
            List<VirtualResourceMetadataRetrySetting> errorDefinitions = new List<VirtualResourceMetadataRetrySetting>();

            bool addedNew = false; // Determine if a new activity was added
            foreach (var config in _configurations)
            {
                if (config.Metadata.VirtualResourceMetadataRetrySettings == null || config.Metadata.VirtualResourceMetadataRetrySettings.Count == 0) // For unconfigured activities, instantiate a new list
                {
                    addedNew = true;
                }
                else // If they are not null, add them to the list
                {
                    failedDefinitions.AddRange(config.Metadata.VirtualResourceMetadataRetrySettings.Where(x => x.State == PluginResult.Failed.ToString()));
                    skippedDefinitions.AddRange(config.Metadata.VirtualResourceMetadataRetrySettings.Where(x => x.State == PluginResult.Skipped.ToString()));
                    errorDefinitions.AddRange(config.Metadata.VirtualResourceMetadataRetrySettings.Where(x => x.State == PluginResult.Error.ToString()));
                }
            }

            if (!addedNew)
            {
                var skipPrototype = skippedDefinitions.First();
                var failPrototype = failedDefinitions.First();
                var errorPrototype = errorDefinitions.First();

                // Check if all actvities are configured the same, then we can edit them all at the same time
                if (_configurations.Count == 1 || (skippedDefinitions.All(s => s == skipPrototype) && failedDefinitions.All(f => f == failPrototype) && errorDefinitions.All(e => e == errorPrototype)))
                {
                    RefreshRow(FAIL_ROW, failPrototype);
                    RefreshRow(SKIP_ROW, skipPrototype);
                    RefreshRow(ERROR_ROW, errorPrototype);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private static void LoadActionItems(ComboBox comboBox)
        {
            comboBox.Items.Add(EnumUtil.GetDescription(PluginRetryAction.Continue));
            comboBox.Items.Add(EnumUtil.GetDescription(PluginRetryAction.Halt));
            comboBox.Items.Add(EnumUtil.GetDescription(PluginRetryAction.Retry));
        }

        private static void LoadRetryActionItems(ComboBox comboBox)
        {
            comboBox.Items.Add(EnumUtil.GetDescription(PluginRetryAction.Continue));
            comboBox.Items.Add(EnumUtil.GetDescription(PluginRetryAction.Halt));
        }

        private void LoadDefaultDefinitions()
        {
            RefreshRow(ERROR_ROW, null);
            RefreshRow(SKIP_ROW, null);
            RefreshRow(FAIL_ROW, null);
        }

        private void RefreshRow(int rowIndex, VirtualResourceMetadataRetrySetting definition)
        {
            ComboBox actionCombo = (ComboBox)main_TableLayoutPanel.GetControlFromPosition(ACTION_COLUMN, rowIndex);
            TextBox retryTextBox = (TextBox)main_TableLayoutPanel.GetControlFromPosition(RETRY_LIMIT_COLUMN, rowIndex);
            var delayTimeSpan = (HP.ScalableTest.Framework.UI.TimeSpanControl)main_TableLayoutPanel.GetControlFromPosition(RETRY_DELAY_COLUMN, rowIndex);
            ComboBox retryActionCombo = (ComboBox)main_TableLayoutPanel.GetControlFromPosition(RETRY_ACTION_COLUMN, rowIndex);

            bool setRetryDefaults = true;
            if (definition != null)
            {
                actionCombo.Text = definition.Action;
                if (definition.Action.Equals(PluginRetryAction.Retry.ToString()))
                {
                    setRetryDefaults = false;
                    retryTextBox.Text = definition.RetryLimit.ToString(CultureInfo.CurrentCulture);
                    delayTimeSpan.Value = new TimeSpan(0,0, definition.RetryDelay);
                    retryActionCombo.Text = definition.LimitExceededAction;
                }
            }
            else
            {
                actionCombo.SelectedIndex = 0;
            }

            if (setRetryDefaults)
            {
                retryTextBox.Text = "0";
                delayTimeSpan.Value = new TimeSpan(0, 0, 0);
                retryActionCombo.SelectedIndex = 0;
            }
        }

        private void Action_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            TableLayoutPanelCellPosition position = main_TableLayoutPanel.GetPositionFromControl(comboBox);
            bool retrySelected = RETRY.Equals((string)comboBox.SelectedItem, StringComparison.OrdinalIgnoreCase);
            ToggleRetryControls(position.Row, retrySelected);
            if (sender == errorAction_ComboBox)
            {
                errorRetryWarningLabel.Visible = retrySelected;
            }
        }

        private void ToggleRetryControls(int rowIndex, bool enabled)
        {
            ToggleControls(RETRY_LIMIT_COLUMN, rowIndex, enabled);
        }

        private void ToggleControls(int colIndex, int rowIndex, bool enabled)
        {
            for (int col = colIndex; col < main_TableLayoutPanel.ColumnCount; col++)
            {
                main_TableLayoutPanel.GetControlFromPosition(col, rowIndex).Enabled = enabled;
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            // Clear all previous exception definitions
            foreach (var c in _configurations)
            {               
                var settings = c.Metadata.VirtualResourceMetadataRetrySettings.ToList();
                foreach(var setting in settings)
                {
                    setting.VirtualResourceMetadataId = Guid.Empty;
                }
                c.Metadata.VirtualResourceMetadataRetrySettings.Clear();
            }

            UpdateDefinitions();

            this.DialogResult = DialogResult.OK;
        }

        private void UpdateDefinitions()
        {
            UpdateDefinition(ERROR_ROW, PluginResult.Error);
            UpdateDefinition(SKIP_ROW, PluginResult.Skipped);
            UpdateDefinition(FAIL_ROW, PluginResult.Failed);
        }

        private void UpdateDefinition(int rowIndex, PluginResult activityUpdateType)
        {
            ComboBox actionCombo = (ComboBox)main_TableLayoutPanel.GetControlFromPosition(ACTION_COLUMN, rowIndex);
            TextBox retryTextBox = (TextBox)main_TableLayoutPanel.GetControlFromPosition(RETRY_LIMIT_COLUMN, rowIndex);
            var delayTimeSpan = (HP.ScalableTest.Framework.UI.TimeSpanControl)main_TableLayoutPanel.GetControlFromPosition(RETRY_DELAY_COLUMN, rowIndex);
            ComboBox retryActionCombo = (ComboBox)main_TableLayoutPanel.GetControlFromPosition(RETRY_ACTION_COLUMN, rowIndex);

            // Add it to all of the configurations
            foreach (var c in _configurations)
            {                
                VirtualResourceMetadataRetrySetting retrySetting = new VirtualResourceMetadataRetrySetting();
                retrySetting.SettingId = SequentialGuid.NewGuid();
                retrySetting.State = activityUpdateType.ToString();
                retrySetting.Action = actionCombo.Text;
                int limitValue = 0;
                if (!int.TryParse(retryTextBox.Text, out limitValue))
                {
                    limitValue = 0;
                }
                retrySetting.RetryLimit = limitValue;
                retrySetting.RetryDelay = Convert.ToInt32(delayTimeSpan.Value.TotalSeconds);
                retrySetting.LimitExceededAction = retryActionCombo.Text;

                c.Metadata.VirtualResourceMetadataRetrySettings.Add(retrySetting);
            }
        }
    }
}
