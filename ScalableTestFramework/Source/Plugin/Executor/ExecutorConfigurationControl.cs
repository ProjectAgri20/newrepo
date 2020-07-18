using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.Executor
{
    /// <summary>
    /// UI Editor control
    /// </summary>
    [ToolboxItem(false)]
    public partial class ExecutorConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private ExecutorActivityData _activityData;

        /// <summary>
        /// constructor
        /// </summary>
        public ExecutorConfigurationControl()
        {
            InitializeComponent();
            dataGridView_executables.AutoGenerateColumns = false;

            executor_fieldValidator.RequireCustom(dataGridView_executables, () => dataGridView_executables.RowCount > 0, "Please select an executable.");
            if (ConfigurationChanged != null)
            {
                dataGridView_executables.RowsAdded += (s, e) => ConfigurationChanged(s, e);
                dataGridView_executables.RowsRemoved += (s, e) => ConfigurationChanged(s, e);
            }
        }



        #region Events

        private void browseInstaller_button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = "*.bat";
                dialog.Filter = @"All Executables (*.exe)(*.bat)(*.msi)|*.exe;*.bat;*.msi";
                dialog.Multiselect = false;
                dialog.Title = @"Select the software installer";

                if (DialogResult.OK == dialog.ShowDialog())
                {
                    setupPath_textBox.Text = dialog.FileName;
                }
            }
        }

        private void moveup_button_Click(object sender, EventArgs e)
        {
            if (dataGridView_executables.SelectedRows.Count == 0)
            {
                return;
            }

            int index = dataGridView_executables.CurrentCell.RowIndex;

            if (index > 0)
            {
                dataGridView_executables.Visible = false;
                dataGridView_executables.DataSource = null;
                var tempTask = _activityData.Executables.ElementAt(index);
                _activityData.Executables.Insert(index - 1, tempTask);
                _activityData.Executables.RemoveAt(index + 1);
                dataGridView_executables.DataSource = _activityData.Executables;
                dataGridView_executables.Visible = true;
            }
        }

        private void movedown_button_Click(object sender, EventArgs e)
        {
            if (dataGridView_executables.SelectedRows.Count == 0)
            {
                return;
            }

            int index = dataGridView_executables.CurrentCell.RowIndex;

            if (index < (_activityData.Executables.Count - 1))
            {
                dataGridView_executables.Visible = false;
                dataGridView_executables.DataSource = null;
                var tempTask = _activityData.Executables.ElementAt(index);
                _activityData.Executables.Insert(index + 2, tempTask);
                _activityData.Executables.RemoveAt(index);
                dataGridView_executables.DataSource = _activityData.Executables;
                dataGridView_executables.Visible = true;
            }
        }

        private void addPlugin_ToolStripButton_Click(object sender, EventArgs e)
        {
            Executable newExecutable = new Executable();


            if (EditEntry(newExecutable) == DialogResult.OK)
            {
                dataGridView_executables.Visible = false;
                dataGridView_executables.DataSource = null;
                _activityData.Executables.Add(newExecutable);
                dataGridView_executables.DataSource = _activityData.Executables;
                dataGridView_executables.Visible = true;
                dataGridView_executables.Refresh();
            }
        }

        private void edit_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView_executables.SelectedRows.Count == 0)
            {
                return;
            }

            var executable = dataGridView_executables.SelectedRows[0].DataBoundItem as Executable;

            if (EditEntry(executable) == DialogResult.OK)
            {
                dataGridView_executables.Refresh();
            }
        }

        private void delete_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView_executables.SelectedRows.Count == 0)
            {
                return;
            }

            int index = dataGridView_executables.CurrentCell.RowIndex;

            dataGridView_executables.Visible = false;
            dataGridView_executables.DataSource = null;
            _activityData.Executables.RemoveAt(index);
            dataGridView_executables.DataSource = _activityData.Executables;
            dataGridView_executables.Visible = true;
        }
        #endregion

        private static DialogResult EditEntry(Executable executable)
        {
            DialogResult result;

            using (ExecutorManagementForm form = new ExecutorManagementForm(executable))
            {
                result = form.ShowDialog();
            }

            return result;
        }

        #region IPluginConfigurationControl

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            // Initialize the activity data with a default value
            _activityData = new ExecutorActivityData();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Initialize the activity data by deserializing it from an existing copy of configuration information.
            _activityData = configuration?.GetMetadata<ExecutorActivityData>();
            if (_activityData != null)
            {
                dataGridView_executables.DataSource = _activityData.Executables;
                setupPath_textBox.Text = _activityData.SetupFileName;
                copydir_checkBox.Checked = _activityData.CopyDirectory;
            }

        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.SetupFileName = setupPath_textBox.Text;
            _activityData.CopyDirectory = copydir_checkBox.Checked;
            return new PluginConfigurationData(_activityData, "1.0");

        }

        public PluginValidationResult ValidateConfiguration()
        {
            // This is where you can add any validation for your UI and then
            // return the appropriate validation result when saving the data. 

            return new PluginValidationResult(executor_fieldValidator.ValidateAll());
        }

        #endregion
    }
}
