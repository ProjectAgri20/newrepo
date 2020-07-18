using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.PSPullPrint
{
    [ToolboxItem(false)]
    public partial class PSPullPrintConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private const AssetAttributes _deviceAttributes = AssetAttributes.Printer | AssetAttributes.ControlPanel;
        private PSPullPrintActivityData _activityData;
        public event EventHandler ConfigurationChanged;
        public PSPullPrintConfigurationControl()
        {
            InitializeComponent();
            pullprint_assetSelectionControl.SelectionChanged += ConfigurationChanged;
            tasks_dataGridView.RowsAdded += (s, e) => ConfigurationChanged(s, e);
            tasks_dataGridView.RowsRemoved += (s, e) => ConfigurationChanged(s, e);
            psp_fieldValidator.RequireAssetSelection(pullprint_assetSelectionControl);
            psp_fieldValidator.RequireCustom(tasks_dataGridView, ValidateTasks, "Please add a task");
        }

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new PSPullPrintActivityData();
            pullprint_assetSelectionControl.Initialize(_deviceAttributes);
            PopulateUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<PSPullPrintActivityData>();
            pullprint_assetSelectionControl.Initialize(configuration.Assets, _deviceAttributes);

            PopulateUI();
        }

        private void PopulateUI()
        {
            solutionApp_comboBox.DataSource = EnumUtil.GetDescriptions<PullPrintingSolution>().ToList();
            tasks_dataGridView.AutoGenerateColumns = false;

            if (_activityData.PullPrintTasks.Count > 0)
            {
                tasks_dataGridView.DataSource = _activityData.PullPrintTasks;
                solutionApp_comboBox.SelectedIndex = solutionApp_comboBox.Items.IndexOf(EnumUtil.GetDescription(_activityData.PullPrintingSolution));
            }
            pacing_timeSpanControl.Value = _activityData.ActivityPacing;
            lockTimeoutControl.Initialize(_activityData.LockTimeouts ?? new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10)));
            PopulateOperations();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.SafecomPin = safecomPin_textBox.Text;
            _activityData.ActivityPacing = pacing_timeSpanControl.Value;
            _activityData.LockTimeouts = lockTimeoutControl.Value;
            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = pullprint_assetSelectionControl.AssetSelectionData
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(psp_fieldValidator.ValidateAll());
        }

        #region AddActivity

        /// <summary>
        /// Adds an activity to the tasks list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addactivity_button_Click(object sender, EventArgs e)
        {
            tasks_dataGridView.Visible = false;
            tasks_dataGridView.DataSource = null;

            SolutionOperation selectedOperation = EnumUtil.GetByDescription<SolutionOperation>(operations_comboBox.SelectedItem.ToString());
            _activityData.PullPrintingSolution = EnumUtil.GetByDescription<PullPrintingSolution>(solutionApp_comboBox.SelectedItem.ToString());

            switch (selectedOperation)
            {
                case SolutionOperation.Print:
                    {
                        _activityData.PullPrintTasks.Add(new PrintManagementTask() { Operation = SolutionOperation.Print, Description = $"Print one job using { (object)solutionApp_comboBox.SelectedItem.ToString()} App" });
                    }
                    break;

                case SolutionOperation.PrintAll:
                    {
                        _activityData.PullPrintTasks.Add(new PrintManagementTask() { Operation = SolutionOperation.PrintAll, Description = $"Print All jobs using { (object)solutionApp_comboBox.SelectedItem.ToString()} App" });
                    }
                    break;

                case SolutionOperation.Delete:
                    {
                        _activityData.PullPrintTasks.Add(new PrintManagementTask() { Operation = SolutionOperation.Delete, Description = $"Delete one job using { (object)solutionApp_comboBox.SelectedItem.ToString()} App" });
                    }
                    break;

                case SolutionOperation.Cancel:
                    {
                        _activityData.PullPrintTasks.Add(new PrintManagementTask() { Operation = SolutionOperation.Cancel, Description = $"Cancel one job using { (object)solutionApp_comboBox.SelectedItem.ToString()} App" });
                    }
                    break;

                case SolutionOperation.PrintDelete:
                    {
                        _activityData.PullPrintTasks.Add(new PrintManagementTask() { Operation = SolutionOperation.PrintDelete, Description = $"Print and Delete job using { (object)solutionApp_comboBox.SelectedItem.ToString()} App" });
                    }
                    break;

                case SolutionOperation.PrintKeep:
                    {
                        _activityData.PullPrintTasks.Add(new PrintManagementTask() { Operation = SolutionOperation.PrintKeep, Description = $"Print and Retain job using { (object)solutionApp_comboBox.SelectedItem.ToString()} App" });
                    }
                    break;

                case SolutionOperation.UIValidation:
                    {
                        _activityData.PullPrintTasks.Add(new PrintManagementTask() { Operation = SolutionOperation.UIValidation, Description = $"Refresh Toggle Button UI Validation" });
                    }
                    break;

                case SolutionOperation.SignOut:
                    {
                        _activityData.PullPrintTasks.Add(new PrintManagementTask() { Operation = SolutionOperation.SignOut, Description = "Sign out the current logged in user" });
                    }
                    break;
            }
            tasks_dataGridView.DataSource = _activityData.PullPrintTasks;
            tasks_dataGridView.Visible = true;
        }

        #endregion AddActivity

        private void moveup_button_Click(object sender, EventArgs e)
        {
            if (tasks_dataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            int index = tasks_dataGridView.CurrentCell.RowIndex;

            if (index >= 1)
            {
                tasks_dataGridView.Visible = false;
                tasks_dataGridView.DataSource = null;
                var tempTask = _activityData.PullPrintTasks.ElementAt(index);
                _activityData.PullPrintTasks.Insert(index - 1, tempTask);
                _activityData.PullPrintTasks.RemoveAt(index + 1);
                tasks_dataGridView.DataSource = _activityData.PullPrintTasks;
                tasks_dataGridView.Visible = true;
            }
        }

        private void movedown_button_Click(object sender, EventArgs e)
        {
            if (tasks_dataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            int index = tasks_dataGridView.CurrentCell.RowIndex;

            if (index < (_activityData.PullPrintTasks.Count - 1))
            {
                tasks_dataGridView.Visible = false;
                tasks_dataGridView.DataSource = null;
                var tempTask = _activityData.PullPrintTasks.ElementAt(index);
                _activityData.PullPrintTasks.Insert(index + 2, tempTask);
                _activityData.PullPrintTasks.RemoveAt(index);
                tasks_dataGridView.DataSource = _activityData.PullPrintTasks;
                tasks_dataGridView.Visible = true;
            }
        }

        private void remove_button_Click(object sender, EventArgs e)
        {
            if (tasks_dataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            int index = tasks_dataGridView.CurrentCell.RowIndex;

            tasks_dataGridView.Visible = false;
            tasks_dataGridView.DataSource = null;
            _activityData.PullPrintTasks.RemoveAt(index);
            tasks_dataGridView.DataSource = _activityData.PullPrintTasks;
            tasks_dataGridView.Visible = true;
        }

        private void PopulateOperations()
        {
            List<String> printOperations = new List<String>();

            if (solutionApp_comboBox.SelectedItem.ToString().Equals("Safecom"))
            {
                printOperations.Add(EnumUtil.GetDescription(SolutionOperation.Print));
                printOperations.Add(EnumUtil.GetDescription(SolutionOperation.PrintAll));
                printOperations.Add(EnumUtil.GetDescription(SolutionOperation.Delete));
                printOperations.Add(EnumUtil.GetDescription(SolutionOperation.SignOut));
                if (!string.IsNullOrEmpty((_activityData.SafecomPin)))
                {
                    safecomPin_textBox.Text = _activityData.SafecomPin;
                }
            }
            else if (solutionApp_comboBox.SelectedItem.ToString().Equals("HP Access Control"))
            {
                printOperations.Add(EnumUtil.GetDescription(SolutionOperation.Print));
                printOperations.Add(EnumUtil.GetDescription(SolutionOperation.Delete));

                printOperations.Add(EnumUtil.GetDescription(SolutionOperation.PrintAll));
                printOperations.Add(EnumUtil.GetDescription(SolutionOperation.Cancel));

                printOperations.Add(EnumUtil.GetDescription(SolutionOperation.PrintDelete));
                printOperations.Add(EnumUtil.GetDescription(SolutionOperation.PrintKeep));
                printOperations.Add(EnumUtil.GetDescription(SolutionOperation.UIValidation));
                printOperations.Add(EnumUtil.GetDescription(SolutionOperation.SignOut));
            }
            operations_comboBox.DataSource = printOperations;
        }

        private void SolutionApp_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {                                                                                //solutionApp_comboBox.Items.IndexOf(EnumUtil.GetDescription(_activityData.PullPrintingSolution));
            if (tasks_dataGridView.Rows.Count > 0 && solutionApp_comboBox.SelectedIndex != solutionApp_comboBox.Items.IndexOf(EnumUtil.GetDescription(_activityData.PullPrintingSolution)))
            {
                MessageBox.Show(@"Solution App should not be modified", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                solutionApp_comboBox.SelectedIndex = solutionApp_comboBox.Items.IndexOf(EnumUtil.GetDescription(_activityData.PullPrintingSolution));
            }
        }

        public bool ValidateTasks()
        {
            return tasks_dataGridView.RowCount > 0;
        }




    }
}