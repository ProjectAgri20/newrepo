using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI;

namespace HP.ScalableTest.LabConsole
{
    public partial class GlobalSettingsManagementDialog : Form
    {
        EnterpriseTestContext _context = null;
        bool _unsavedChangesExist = false;

        public GlobalSettingsManagementDialog()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialogWithHelp);

            _context = new EnterpriseTestContext();
        }

        private void GlobalSettingsManagementDialog_Load(object sender, EventArgs e)
        {
            // Populate the combo box with the names of all available roles
            foreach (string settingType in _context.SystemSettings.Select(n => n.Type).Distinct())
            {
                typeDataGridViewTextBoxColumn.Items.Add(settingType);
            }

            settings_DataGridView.DataSource = _context.SystemSettings;

            int lastRow = settings_DataGridView.Rows.GetLastRow(DataGridViewElementStates.None);
            settings_DataGridView.Rows[lastRow].Cells["nameDataGridViewTextBoxColumn"].ReadOnly = false;

            // Watch for changes to prompt the user if they click on Cancel
            settings_DataGridView.CellValueChanged += new DataGridViewCellEventHandler(settings_DataGridView_CellValueChanged);
            settings_DataGridView.RowsAdded += new DataGridViewRowsAddedEventHandler(settings_DataGridView_RowsAdded);
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (ValidateEntities())
            {
                _context.SaveChanges();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool ValidateEntities()
        {
            foreach (DataGridViewRow row in settings_DataGridView.Rows)
            {
                SystemSetting setting = row.DataBoundItem as SystemSetting;

                if (setting != null && setting.EntityState == EntityState.Added)
                {
                    if (string.IsNullOrEmpty(setting.Type))
                    {
                        MessageBox.Show
                            (
                                "The Type must have a value",
                                "Setting Type Missing Value",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        return false;
                    }

                    if (string.IsNullOrEmpty(setting.Name))
                    {
                        MessageBox.Show
                            (
                                "The Name must not be blank",
                                "Setting Name Missing Value",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        return false;
                    }

                    if (string.IsNullOrEmpty(setting.Value))
                    {
                        MessageBox.Show
                            (
                                "The Value must not be blank",
                                "Setting Value Missing Value",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        return false;
                    }

                    var exists = 
                        (
                            from s in _context.SystemSettings
                            where s.Name.Equals(setting.Name, StringComparison.OrdinalIgnoreCase)
                            select s
                        ).Any();

                    if (exists)
                    {
                        MessageBox.Show
                            (
                                "A setting entry with Name = '{0}' already exists.".FormatWith(setting.Name),
                                "Duplicate Setting Value",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        return false;
                    }

                    _context.AddToSystemSettings(setting);
                }
            }

            return true;
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            if (_unsavedChangesExist)
            {
                var result = MessageBox.Show
                    (
                        "You have unsaved changes that will be lost.  Continue?",
                        "Unsaved changes",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            Close();
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            if (ValidateEntities())
            {
                _context.SaveChanges();
                MessageBox.Show
                    (
                        "Changes have been saved.",
                        "Apply Changes",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
            }
        }

        private void settings_DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            _unsavedChangesExist = true;
        }

        private void delete_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (settings_DataGridView.SelectedRows.Count == 1)
            {
                settings_DataGridView.Rows.RemoveAt(settings_DataGridView.SelectedRows[0].Index);
            }
        }

        private void settings_DataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // A new row was added, so make sure readonly is turned off for this row
            if (e.RowIndex > 0)
            {
                settings_DataGridView.Rows[e.RowIndex - 1].Cells["nameDataGridViewTextBoxColumn"].ReadOnly = false;
            }
        }

        private void GlobalSettingsManagementDialog_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            using (HelpDialog dialog = new HelpDialog(Properties.Resources.GlobalSettingsManagementHelpPage))
            {
                dialog.Title = "Global Settings Help";
                dialog.ShowDialog();
            }
        }
    }
}
