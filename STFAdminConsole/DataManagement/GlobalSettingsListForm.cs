using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Core.UI;
using Telerik.WinControls.UI;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.LabConsole
{
    public partial class GlobalSettingsListForm : Form
    {
        private SortableBindingList<SystemSetting> _settings = null;
        private EnterpriseTestContext _context = null;
        private Collection<SystemSetting> _deletedItems = new Collection<SystemSetting>();
        private SettingType _settingType = SettingType.SystemSetting;
        private string _subType = string.Empty;
        bool _unsavedChanges = false;

        /// <summary>
        /// Displays list of SystemSettings (all or by type).
        /// </summary>
        /// <param name="settingType"></param>
        /// <param name="subType"></param>
        public GlobalSettingsListForm(SettingType settingType, string subType = "")
        {
            InitializeComponent();

            _settings = new SortableBindingList<SystemSetting>();
            _context = new EnterpriseTestContext();
            _settingType = settingType;
            _subType = subType;

            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            ConfigureSettingsGrid();

            //this.Text = Regex.Replace(_settingType.ToString(), "([A-Z])", " $1").Trim() + "s Management";            
            this.Text = _settingType.GetDescription() + "s Management";

            ToolStripControlHost toolStripHost = new ToolStripControlHost(viewAll_CheckBox);
            toolStripHost.Alignment = ToolStripItemAlignment.Left;
            toolStripHost.ToolTipText = "View Settings Type";
            toolStripHost.Visible = UserManager.CurrentUser.HasPrivilege(UserRole.Administrator);
            toolStrip1.Items.Insert(toolStrip1.Items.Count, toolStripHost);
        }

        private void GlobalSettingsListForm_Load(object sender, EventArgs e)
        {
            using (new BusyCursor())
            {
                BindSettingsToGrid();
            }
        }

        private void ConfigureSettingsGrid()
        {
            UserInterfaceStyler.Configure(settings_RadGridView, GridViewStyle.ReadOnly);
            settings_RadGridView.AllowRowResize = true;

            GridViewColumn subType_Column = settings_RadGridView.MasterTemplate.Columns["subType_Column"];
            switch (_settingType)
            {
                case SettingType.PluginSetting:
                    subType_Column.HeaderText = "Plugin";
                    break;
                case SettingType.ServerSetting:
                    subType_Column.HeaderText = "Server";
                    break;
                case SettingType.SystemSetting:
                    break;
                default:
                    GridViewColumn type_Column = settings_RadGridView.MasterTemplate.Columns["type_Column"];
                    type_Column.IsVisible = true;
                    break;
            }
        }

        private void RefreshSettings()
        {
            _settings.Clear();

            string settingType = _settingType.ToString();
            foreach (SystemSetting item in _context.SystemSettings.Where(n => n.Type.Equals(settingType)))
            {
                _settings.Add(item);
            }
        }

        private void BindSettingsToGrid()
        {
            RefreshSettings();

            settings_RadGridView.DataSource = null;
            settings_RadGridView.DataSource = _settings;

            settings_RadGridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            settings_RadGridView.BestFitColumns();

            if (settings_RadGridView.Rows.Count > 0)
            {
                settings_RadGridView.TableElement.ScrollToRow(0);
            }
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return settings_RadGridView.SelectedRows.FirstOrDefault();
        }

        private void settings_RadGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var setting = row.DataBoundItem as SystemSetting;

                if (EditEntry(setting) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    settings_RadGridView.Refresh();
                }
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            Commit();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            if (_unsavedChanges)
            {
                var result = MessageBox.Show("You have unsaved changes that will be lost.  Do you want to continue?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Close();
                    DialogResult = DialogResult.Cancel;
                    return;
                }
            }
            else
            {
                Close();
                DialogResult = DialogResult.Cancel;
                return;
            }
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        public void Commit()
        {
            Cursor = Cursors.WaitCursor;

            foreach (var entity in _deletedItems)
            {
                _context.SystemSettings.DeleteObject(entity);
            }

            foreach (var item in _settings)
            {
                switch (item.EntityState)
                {
                    case EntityState.Added:
                        {
                            _context.SystemSettings.AddObject(item);
                            break;
                        }
                }
            }

            _context.SaveChanges();
            _deletedItems.Clear();

            Cursor = Cursors.Default;
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Commit();
            Cursor = Cursors.Default;

            _unsavedChanges = false;
        }

        private void add_ToolStripButton_Click(object sender, EventArgs e)
        {
            SystemSetting setting = new SystemSetting()
            {
                Type = _settingType.ToString(),
                SubType = _subType,
                Name = string.Empty,
                Value = string.Empty
            };

            if (EditEntry(setting) == DialogResult.OK)
            {
                AddSetting(setting);

                int index = settings_RadGridView.Rows.Count - 1;
                settings_RadGridView.Rows[index].IsSelected = true;
                settings_RadGridView.TableElement.ScrollToRow(index);
            }
        }

        private void edit_ToolStripButton_Click(object sender, EventArgs e)
        {
            EditEntry();
        }

        private void EditEntry()
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                SystemSetting setting = row.DataBoundItem as SystemSetting;

                if (EditEntry(setting) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    settings_RadGridView.Refresh();
                }
            }
        }

        private DialogResult EditEntry(SystemSetting setting)
        {
            DialogResult result = DialogResult.None;

            using (GlobalSettingsEditForm form = CreateSettingsEditForm(setting))
            {
                result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    AddLinkedSettings(form.AddedItems);
                    _unsavedChanges = true;
                }
            }

            return result;
        }

        private void AddLinkedSettings(List<SystemSetting> addedItems)
        {
            foreach (SystemSetting setting in addedItems)
            {
                AddSetting(setting);
            }
        }

        private void AddSetting(SystemSetting setting)
        {
            if (! _settings.Contains(setting))
            {
                _context.AddToSystemSettings(setting);
                _settings.Add(setting);
            }
        }

        private void delete_ToolStripButton_Click(object sender, EventArgs e)
        {
            RemoveEntry();
        }

        private void RemoveEntry()
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var setting = row.DataBoundItem as SystemSetting;

                var dialogResult = MessageBox.Show
                    (
                        "Removing setting {0}.  Do you want to continue?".FormatWith(setting.Name),
                        "Delete Setting",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question

                    );

                if (dialogResult == DialogResult.Yes)
                {
                    _unsavedChanges = true;
                    _deletedItems.Add(setting);
                    _settings.Remove(setting);
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditEntry();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveEntry();
        }

        private void reloadToolStripButton_Click(object sender, EventArgs e)
        {
            using (new BusyCursor())
            {
                GlobalSettings.Refresh();
                BindSettingsToGrid();
            }
        }

        private void viewAll_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            GridViewColumn type_Column = settings_RadGridView.MasterTemplate.Columns["type_Column"];
            type_Column.IsVisible = viewAll_CheckBox.Checked;
        }

        private GlobalSettingsEditForm CreateSettingsEditForm(SystemSetting setting)
        {
            if (_settingType == SettingType.PluginSetting)
            {
                return new GlobalSettingsEditForm(setting, _context, _settings.Where(s => s.Name.Equals(setting.Name) && s.Value.Equals(setting.Value)).ToList());
            }

            return new GlobalSettingsEditForm(setting, _context);
        }
    }
}
