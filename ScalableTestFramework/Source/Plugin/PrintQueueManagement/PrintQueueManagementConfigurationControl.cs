using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.PrintQueueManagement
{
    /// <summary>
    /// Print Management Plugin's edit control
    /// </summary>
    [ToolboxItem(false)]
    public partial class PrintQueueManagementConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private PrintQueueManagementActivityData _activityData;
        private DynamicLocalPrintQueueInfo _localPrintQueueInfo;
        private DocumentSelectionData _documentSelectionData;
        private PrintDriverInfo _printDriver;

        /// <summary>
        /// print queue preference member
        /// </summary>
        private PrintQueuePreferences _preference;

        /// <summary>
        /// constructor
        /// </summary>
        public PrintQueueManagementConfigurationControl()
        {
            InitializeComponent();
            tasks_dataGridView.AutoGenerateColumns = false;

            pqm_fieldValidator.RequireValue(device_textBox, "Device Textbox");
            pqm_fieldValidator.RequireValue(document_comboBox, document_label, ValidationCondition.IfEnabled);
            pqm_fieldValidator.RequireCustom(tasks_dataGridView, () => tasks_dataGridView.RowCount > 0, "Please add a task");
            upgrade_printDriverSelectionControl.Initialize();
            upgrade_printDriverSelectionControl.DataBindings.Add("Enabled", upgrade_radioButton, "Checked");
            configure_button.DataBindings.Add("Enabled", configure_radioButton, "Checked");
            tasks_dataGridView.AutoGenerateColumns = false;
        }

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new PrintQueueManagementActivityData();
            _documentSelectionData = new DocumentSelectionData();

            LoadDocuments();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration?.GetMetadata<PrintQueueManagementActivityData>();
            _documentSelectionData = configuration?.Documents;

            DynamicLocalPrintQueueDefinition definition = configuration?.PrintQueues.SelectedPrintQueues.First() as DynamicLocalPrintQueueDefinition;
            IPrinterInfo printDevice = ConfigurationServices.AssetInventory.GetAsset(definition.AssetId) as IPrinterInfo;
            _printDriver = ConfigurationServices.AssetInventory.AsInternal().GetPrintDrivers().First(n => n.PrintDriverId == definition.PrintDriverId);
            _localPrintQueueInfo = new DynamicLocalPrintQueueInfo(printDevice, _printDriver, definition.PrinterPort, definition.PrintDriverConfiguration);

            LoadDocuments();
            LoadUi();
        }

        public PluginConfigurationData GetConfiguration()
        {
            if (_activityData.ActivityPacing != pacing_timeSpanControl.Value)
            {
                _activityData.ActivityPacing = pacing_timeSpanControl.Value;
            }
            _activityData.IsDefaultPrinter = defaultprinter_checkBox.Checked;

            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = new AssetSelectionData(ConfigurationServices.AssetInventory.GetAsset(_localPrintQueueInfo.AssociatedAssetId)),
                PrintQueues = new PrintQueueSelectionData(_localPrintQueueInfo),
                Documents = _documentSelectionData
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(pqm_fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Called when validate interface triggers
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void printerSelect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(device_textBox.Text))
            {
                using (DynamicLocalPrintQueueForm localQueueForm = new DynamicLocalPrintQueueForm(false))
                {
                    localQueueForm.ShowDialog(this);
                    if (localQueueForm.DialogResult == DialogResult.OK)
                    {
                        _localPrintQueueInfo = localQueueForm.PrintQueues.First();
                        device_textBox.Text = _localPrintQueueInfo.AssociatedAssetId + ", " + _localPrintQueueInfo.PrintDriver.DriverName;
                    }
                }
            }
            else
            {
                using (DynamicLocalPrintQueueForm localQueueForm = new DynamicLocalPrintQueueForm(_localPrintQueueInfo, false))
                {
                    localQueueForm.ShowDialog(this);
                    {
                        if (localQueueForm.DialogResult == DialogResult.OK)
                        {
                            _localPrintQueueInfo = localQueueForm.PrintQueues.First();
                            device_textBox.Text = _localPrintQueueInfo.AssociatedAssetId + ", " + _localPrintQueueInfo.PrintDriver.DriverName;
                        }
                    }
                }
            }
        }

        private void LoadDocuments()
        {
            var allExtensions = ConfigurationServices.DocumentLibrary.GetExtensions();
            var documentExtension = allExtensions.Where(n => n.Extension.Equals("docx", StringComparison.OrdinalIgnoreCase) || n.Extension.Equals("xlsx", StringComparison.OrdinalIgnoreCase) || n.Extension.Equals("pptx", StringComparison.OrdinalIgnoreCase)
            || n.Extension.Equals("pdf", StringComparison.OrdinalIgnoreCase) || n.Extension.Equals("doc", StringComparison.OrdinalIgnoreCase) || n.Extension.Equals("xls", StringComparison.OrdinalIgnoreCase) || n.Extension.Equals("ppt", StringComparison.OrdinalIgnoreCase));
            var docs = ConfigurationServices.DocumentLibrary.GetDocuments(documentExtension);

            document_comboBox.DisplayMember = "FileName";
            document_comboBox.ValueMember = "DocumentId";
            document_comboBox.DataSource = docs;
        }

        private void LoadUi()
        {
            localcache_checkBox.Checked = _activityData.LocalCacheInstall;
            defaultprinter_checkBox.Checked = _activityData.IsDefaultPrinter;

            device_textBox.Text = _localPrintQueueInfo.AssociatedAssetId + @", " +
                                  _printDriver.DriverName;

            tasks_dataGridView.DataSource = _activityData.PrintQueueTasks;
            pacing_timeSpanControl.Value = _activityData.ActivityPacing;
        }

        private void addtask_button_Click(object sender, EventArgs e)
        {
            if (_activityData.PrintQueueTasks.Count == 0)
            {
                if (!install_radioButton.Checked)
                {
                    MessageBox.Show(@"Please install the device", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (upgrade_radioButton.Checked)
            {
                if (upgrade_printDriverSelectionControl.SelectedPrintDriver == null || upgrade_printDriverSelectionControl.SelectedPrintDriver.PrintDriverId.Equals(Guid.Empty))
                {
                    return;
                }
            }

            tasks_dataGridView.Visible = false;
            tasks_dataGridView.DataSource = null;
            if (install_radioButton.Checked)
            {
                _activityData.PrintQueueTasks.Add(new PrintQueueManagementTask() { Operation = PrintQueueOperation.Install, TargetObject = string.Empty, Description = "Install the Print Queue" });
            }
            else if (upgrade_radioButton.Checked)
            {
                if (upgrade_printDriverSelectionControl.SelectedPrintDriver.PrintDriverId == _localPrintQueueInfo.PrintDriver.PrintDriverId)
                {
                    MessageBox.Show(@"Please select a different driver for upgrade");
                    tasks_dataGridView.DataSource = _activityData.PrintQueueTasks;
                    tasks_dataGridView.Visible = true;
                    return;
                }
                _activityData.PrintQueueTasks.Add(new PrintQueueManagementTask() { Operation = PrintQueueOperation.Upgrade, TargetObject = upgrade_printDriverSelectionControl.SelectedPrintDriver, Description = $"Upgrade the driver to version: { (object)upgrade_printDriverSelectionControl.SelectedPrintDriver.DriverName}" });
            }
            else if (uninstall_radioButton.Checked)
            {
                _activityData.PrintQueueTasks.Add(new PrintQueueManagementTask() { Operation = PrintQueueOperation.Uninstall, TargetObject = string.Empty, Description = "Uninstall the Print Queue" });
            }
            else if (print_radioButton.Checked)
            {
                var docId = Guid.Parse(document_comboBox.SelectedValue.ToString());
                if (!_documentSelectionData.SelectedDocuments.Contains(docId))
                {
                    _documentSelectionData.SelectedDocuments.Add(docId);
                }

                _activityData.PrintQueueTasks.Add(new PrintQueueManagementTask() { Operation = PrintQueueOperation.Print, TargetObject = docId, Description = $"Print the document: { (object)document_comboBox.Text}" });
            }
            else if (canceljob_radioButton.Checked)
            {
                var docId = Guid.Parse(document_comboBox.SelectedValue.ToString());
                if (!_documentSelectionData.SelectedDocuments.Contains(docId))
                {
                    _documentSelectionData.SelectedDocuments.Add(docId);
                }
                _activityData.PrintQueueTasks.Add(new PrintQueueManagementTask() { Operation = PrintQueueOperation.Cancel, TargetObject = docId, Description = $"Print and Cancel the document: { (object)document_comboBox.Text} with delay of { (object)canceldelay_numericUpDown.Value}", Delay = (int)canceldelay_numericUpDown.Value });
            }
            else
            {
                if (_preference != null)
                {
                    _activityData.PrintQueueTasks.Add(new PrintQueueManagementTask() { Operation = PrintQueueOperation.Configure, TargetObject = string.Empty, Preference = _preference, Description = $"Configure Print Properties: {_preference}" });
                }
            }

            tasks_dataGridView.DataSource = _activityData.PrintQueueTasks;
            tasks_dataGridView.Visible = true;
        }

        private void moveup_button_Click(object sender, EventArgs e)
        {
            if (tasks_dataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            int index = tasks_dataGridView.CurrentCell.RowIndex;

            if (index > 1)
            {
                tasks_dataGridView.Visible = false;
                tasks_dataGridView.DataSource = null;
                var tempTask = _activityData.PrintQueueTasks.ElementAt(index);
                _activityData.PrintQueueTasks.Insert(index - 1, tempTask);
                _activityData.PrintQueueTasks.RemoveAt(index + 1);
                tasks_dataGridView.DataSource = _activityData.PrintQueueTasks;
                tasks_dataGridView.Visible = true;

                tasks_dataGridView.ClearSelection();
                tasks_dataGridView.Rows[index - 1].Selected = true;
                tasks_dataGridView.CurrentCell = tasks_dataGridView.Rows[index - 1].Cells[0];
            }
        }

        private void movedown_button_Click(object sender, EventArgs e)
        {
            if (tasks_dataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            int index = tasks_dataGridView.CurrentCell.RowIndex;
            if (index == 0)
            {
                return;
            }

            if (index < (_activityData.PrintQueueTasks.Count - 1))
            {
                tasks_dataGridView.Visible = false;
                tasks_dataGridView.DataSource = null;
                var tempTask = _activityData.PrintQueueTasks.ElementAt(index);
                _activityData.PrintQueueTasks.Insert(index + 2, tempTask);
                _activityData.PrintQueueTasks.RemoveAt(index);
                tasks_dataGridView.DataSource = _activityData.PrintQueueTasks;
                tasks_dataGridView.Visible = true;

                tasks_dataGridView.ClearSelection();
                tasks_dataGridView.Rows[index + 1].Selected = true;
                tasks_dataGridView.CurrentCell = tasks_dataGridView.Rows[index + 1].Cells[0];
            }
        }

        private void remove_button_Click(object sender, EventArgs e)
        {
            if (tasks_dataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            int index = tasks_dataGridView.CurrentCell.RowIndex;

            if (_activityData.PrintQueueTasks.ElementAt(index).Operation == PrintQueueOperation.Cancel ||
                _activityData.PrintQueueTasks.ElementAt(index).Operation == PrintQueueOperation.Print)
            {
                {
                    if (
                        _activityData.PrintQueueTasks.Count(
                            x => x.TargetObject.ToString() == _activityData.PrintQueueTasks.ElementAt(index).TargetObject.ToString()) <= 1)
                    {
                        Guid docId;
                        if (Guid.TryParse(_activityData.PrintQueueTasks.ElementAt(index).TargetObject.ToString(),
                            out docId))
                        {
                            _documentSelectionData.SelectedDocuments.Remove(docId);
                        }
                    }
                }
            }
            tasks_dataGridView.Visible = false;
            tasks_dataGridView.DataSource = null;
            _activityData.PrintQueueTasks.RemoveAt(index);
            tasks_dataGridView.DataSource = _activityData.PrintQueueTasks;
            tasks_dataGridView.Visible = true;
        }

        private void localcache_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            _activityData.LocalCacheInstall = localcache_checkBox.Checked;
        }

        private void print_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            document_comboBox.Enabled = print_radioButton.Checked;
        }

        private void canceljob_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            document_comboBox.Enabled = canceljob_radioButton.Checked;
        }

        private void tasks_dataGridView_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tasks_dataGridView.RowCount == 0)
            {
                e.Cancel = true;
            }
        }

        private void configure_button_Click(object sender, EventArgs e)
        {
            using (ConfigureQueueForm configureQueueForm = new ConfigureQueueForm())
            {
                if (DialogResult.OK == configureQueueForm.ShowDialog())
                {
                    _preference = configureQueueForm.PrintPreference;
                }
            }
        }
    }
}