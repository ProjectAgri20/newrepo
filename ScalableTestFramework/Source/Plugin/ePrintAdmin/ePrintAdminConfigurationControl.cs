using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.ePrintAdmin
{
    [ToolboxItem(false)]
    public partial class ePrintAdminConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private ePrintAdminActivityData _activityData;
        private AssetInfoCollection _selectedAssetList = null;
        private HpacInputValues _hpacInputValue = new HpacInputValues();
        private Guid _selectedServer = Guid.Empty;

        public ePrintAdminConfigurationControl()
        {
            InitializeComponent();
            HPAC_checkBox.DataBindings.Add("Enabled", addPrinter_radioButton, "Checked");
            solutioninput_groupBox.DataBindings.Add("Enabled", HPAC_checkBox, "Checked", false);
            importPrinter_groupBox.DataBindings.Add("Enabled", importPrinter_radioButton, "Checked");
            File_Textbox.DataBindings.Add("Enabled", importPrinter_radioButton, "Checked");
            //deviceId_TextBox.DataBindings.Add("Enabled", addPrinter_radioButton, "Checked");
            selectDevice_Button.DataBindings.Add("Enabled", deviceId_TextBox, "Enabled");
            eprint_fieldValidator.RequireSelection(ePrintServer_ComboBox, ePrintServer_label);
            eprint_fieldValidator.RequireValue(adminUser_textBox, adminUser_label);
            eprint_fieldValidator.RequireValue(adminPassword_textBox, adminPassword_label);
            eprint_fieldValidator.RequireValue(deviceId_TextBox, DeviceName_label, ValidationCondition.IfEnabled);
            eprint_fieldValidator.RequireValue(File_Textbox, "Import file", ValidationCondition.IfEnabled);

            eprint_fieldValidator.RequireSelection(hpacServer_ComboBox, "HPAC Server", ValidationCondition.IfEnabled);
            eprint_fieldValidator.RequireValue(printerName_textBox, "Printer Name", ValidationCondition.IfEnabled);
            eprint_fieldValidator.RequireValue(domaniUser_textBox, "Domain User", ValidationCondition.IfEnabled);
            eprint_fieldValidator.RequireValue(domainPassword_textBox, "Domain Password", ValidationCondition.IfEnabled);
            eprint_fieldValidator.RequireValue(queue_textBox, "Queue Name", ValidationCondition.IfEnabled);

            ePrintServer_ComboBox.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            tasks_dataGridView.RowsAdded += (s, e) => ConfigurationChanged(s, e);
            tasks_dataGridView.RowsRemoved += (s, e) => ConfigurationChanged(s, e);
        }

        public event EventHandler ConfigurationChanged;

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.ePrintAdminUser = adminUser_textBox.Text;
            _activityData.ePrintAdminPassword = adminPassword_textBox.Text;

            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = GetAssociatedAsset(),
                Servers = new ServerSelectionData(ePrintServer_ComboBox.SelectedServer)
            };
        }

        public void Initialize(PluginEnvironment environment)
        {
            // Initialize the activity data with a default value
            _activityData = new ePrintAdminActivityData();
            ePrintServer_ComboBox.Initialize("ePrint");
            hpacServer_ComboBox.Initialize();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<ePrintAdminActivityData>();
            _selectedAssetList = ConfigurationServices.AssetInventory.GetAssets(configuration.Assets.SelectedAssets);
            if (_selectedAssetList != null && _selectedAssetList.Any())
            {
                deviceId_TextBox.Text = _selectedAssetList.First().AssetId;
            }
            _selectedServer = configuration.Servers.SelectedServers.FirstOrDefault();
            ePrintServer_ComboBox.Initialize(_selectedServer, "ePrint");

            adminUser_textBox.Text = _activityData.ePrintAdminUser;
            adminPassword_textBox.Text = _activityData.ePrintAdminPassword;

            tasks_dataGridView.DataSource = _activityData.ePrintAdminTasks;
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(eprint_fieldValidator.ValidateAll());
        }

        private AssetSelectionData GetAssociatedAsset()
        {
            if (_selectedAssetList != null)
            {
                return new AssetSelectionData(_selectedAssetList);
            }
            return new AssetSelectionData();
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
                var tempTask = _activityData.ePrintAdminTasks.ElementAt(index);
                _activityData.ePrintAdminTasks.Insert(index - 1, tempTask);
                _activityData.ePrintAdminTasks.RemoveAt(index + 1);
                tasks_dataGridView.DataSource = _activityData.ePrintAdminTasks;
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

            if (index < (_activityData.ePrintAdminTasks.Count - 1))
            {
                tasks_dataGridView.Visible = false;
                tasks_dataGridView.DataSource = null;
                var tempTask = _activityData.ePrintAdminTasks.ElementAt(index);
                _activityData.ePrintAdminTasks.Insert(index + 2, tempTask);
                _activityData.ePrintAdminTasks.RemoveAt(index);
                tasks_dataGridView.DataSource = _activityData.ePrintAdminTasks;
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
            tasks_dataGridView.Visible = false;
            tasks_dataGridView.DataSource = null;
            _activityData.ePrintAdminTasks.RemoveAt(index);
            tasks_dataGridView.DataSource = _activityData.ePrintAdminTasks;
            tasks_dataGridView.Visible = true;
        }

        private void addactivity_button_Click(object sender, EventArgs e)
        {
            if (eprint_fieldValidator.ValidateAll().All(x => x.Succeeded))
            {
                tasks_dataGridView.Visible = false;
                tasks_dataGridView.DataSource = null;
                _activityData.ePrintAdminUser = adminUser_textBox.Text;
                _activityData.ePrintAdminPassword = adminPassword_textBox.Text;
                if (addPrinter_radioButton.Checked)
                {
                    if (HPAC_checkBox.Checked)
                    {
                        _activityData.ePrintAdminTasks.Add(new EprintAdminTask() { Operation = EprintAdminToolOperation.AddPrinterHpac, HpacInputValue = _hpacInputValue, Description = $"Add Printer { (object)printerName_textBox.Text} HPAC" });
                        _hpacInputValue.PrinterName = printerName_textBox.Text;
                        _hpacInputValue.DomainUser = domaniUser_textBox.Text;
                        _hpacInputValue.DomainPassword = domainPassword_textBox.Text;
                        _hpacInputValue.QueueName = queue_textBox.Text;
                    }
                    else if (pjl_checkBox.Checked)
                    {
                        _activityData.ePrintAdminTasks.Add(new EprintAdminTask() { Operation = EprintAdminToolOperation.AddPrinterPJL, HpacInputValue = _hpacInputValue, Description = $"Add Printer { (object)printerName_textBox.Text} SAFECOM " });
                        _hpacInputValue.PrinterName = printerName_textBox.Text;
                        _hpacInputValue.DomainUser = domaniUser_textBox.Text;
                        _hpacInputValue.DomainPassword = domainPassword_textBox.Text;
                        _hpacInputValue.QueueName = queue_textBox.Text;
                    }
                    else
                    {
                        _activityData.ePrintAdminTasks.Add(new EprintAdminTask() { Operation = EprintAdminToolOperation.AddPrinteripv4, TargetObject = deviceId_TextBox.Text, Description = $"Add Printer { (object)deviceId_TextBox.Text}" });
                    }
                }
                else if (deletePrinter_radioButton.Checked)
                {
                    _activityData.ePrintAdminTasks.Add(new EprintAdminTask() { Operation = EprintAdminToolOperation.DeletePrinter, TargetObject = deviceId_TextBox.Text, Description = $"Delete Printer{ (object)deviceId_TextBox.Text}" });
                }
                else if (importPrinter_radioButton.Checked)
                {
                    _activityData.ePrintAdminTasks.Add(new EprintAdminTask() { Operation = EprintAdminToolOperation.ImportPrinter, TargetObject = File_Textbox.Text, Description = $"Import Printer {Path.GetFileName(File_Textbox.Text)}" });
                }
                else if (regularUser_radioButton.Checked)
                {
                    _activityData.ePrintAdminTasks.Add(new EprintAdminTask() { Operation = EprintAdminToolOperation.RegularUser, Description = "Add Regular User" });
                }
                else if (guest_radioButton.Checked)
                {
                    _activityData.ePrintAdminTasks.Add(new EprintAdminTask() { Operation = EprintAdminToolOperation.GuestUser, Description = "Add Guest User " });
                }
                else if (sendJob_radioButton.Checked)
                {
                    _activityData.ePrintAdminTasks.Add(new EprintAdminTask() { Operation = EprintAdminToolOperation.SendPrintJob, Description = "Send Print Job" });
                }
                tasks_dataGridView.DataSource = _activityData.ePrintAdminTasks;
                tasks_dataGridView.Visible = true;
            }
        }

        private void FileBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Filter = "CSV files (*.csv)|*.csv|Excel files (*.xls,*.xlsx)|*.xls;*.xlsx";
                    dialog.Multiselect = false;
                    dialog.Title = "Add CSV/Excel file";
                    dialog.CheckFileExists = true;
                    dialog.CheckPathExists = true;
                    dialog.ValidateNames = true;

                    if (DialogResult.OK == dialog.ShowDialog())
                    {
                        File_Textbox.Text = dialog.FileName;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void selectDevice_Button_Click(object sender, EventArgs e)
        {
            try
            {
                using (AssetSelectionForm printerSelectionForm = new AssetSelectionForm(AssetAttributes.Printer, deviceId_TextBox.Text, true))
                {
                    printerSelectionForm.ShowDialog(this);
                    if (printerSelectionForm.DialogResult == DialogResult.OK)
                    {
                        _selectedAssetList = printerSelectionForm.SelectedAssets;

                        if (_selectedAssetList != null)
                        {
                            deviceId_TextBox.Text = _selectedAssetList.First().AssetId;
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void addPrinter_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            ipv4_groupBox.DataBindings.Clear();
            hpac_groupBox.DataBindings.Clear();
            ipv4_groupBox.DataBindings.Add("Enabled", addPrinter_radioButton, "Checked");
            hpac_groupBox.DataBindings.Add("Enabled", addPrinter_radioButton, "Checked");
        }

        private void deletePrinter_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            ipv4_groupBox.DataBindings.Clear();
            ipv4_groupBox.DataBindings.Add("Enabled", deletePrinter_radioButton, "Checked");
        }

        private void HPAC_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            solutioninput_groupBox.DataBindings.Clear();
            solutioninput_groupBox.DataBindings.Add("Enabled", HPAC_checkBox, "Checked");
            ipv4_groupBox.Enabled = !HPAC_checkBox.Checked;
        }

        private void ePrintserverselectionchanged(object sender, EventArgs e)
        {
        }

        private void HPACServerselectionchanged(object sender, EventArgs e)
        {
            _hpacInputValue.NetworkAddress = hpacServer_ComboBox.SelectedServer.Address;
        }

        private void ePrintAdminConfigurationControl_Load(object sender, EventArgs e)
        {
            if (_selectedServer == Guid.Empty)
                ePrintServer_ComboBox.Initialize("ePrint");
            else
                ePrintServer_ComboBox.Initialize(_selectedServer, "ePrint");

            if (_selectedAssetList != null && _selectedAssetList.Any())
            {
                deviceId_TextBox.Text = _selectedAssetList.First().AssetId;
            }

            hpacServer_ComboBox.Initialize("HPAC");
            ePrintServer_ComboBox.SelectionChanged += ePrintserverselectionchanged;
            hpacServer_ComboBox.SelectionChanged += HPACServerselectionchanged;


        }

        private void pjl_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            solutioninput_groupBox.DataBindings.Clear();
            solutioninput_groupBox.DataBindings.Add("Enabled", pjl_checkBox, "Checked");
            ipv4_groupBox.Enabled = !pjl_checkBox.Checked;
        }
    }
}