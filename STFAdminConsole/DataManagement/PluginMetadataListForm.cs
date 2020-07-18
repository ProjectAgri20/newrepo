using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.LabConsole.DataManagement;
using HP.ScalableTest.UI;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.LabConsole
{
    public partial class PluginMetadataListForm : Form
    {
        private SortableBindingList<MetadataType> _pluginTypes = null;
        private EnterpriseTestContext _context = null;
        private Collection<MetadataType> _deletedItems = new Collection<MetadataType>();
        bool _unsavedChanges = false;

        public PluginMetadataListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(plugin_RadGridView, GridViewStyle.ReadOnly);

            _pluginTypes = new SortableBindingList<MetadataType>();
            _context = new EnterpriseTestContext();
        }

        private void PluginMetadataManagementForm_Load(object sender, EventArgs e)
        {
            using (new BusyCursor())
            {
                foreach (var item in _context.MetadataTypes)
                {
                    item.JoinTypes();
                    _pluginTypes.Add(item);
                }

                plugin_RadGridView.DataSource = _pluginTypes;

                plugin_RadGridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                plugin_RadGridView.BestFitColumns();
            }
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return plugin_RadGridView.SelectedRows.FirstOrDefault();
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            Commit();
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
                    return;
                }
            }
            else
            {
                Close();
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
                _context.MetadataTypes.DeleteObject(entity);
            }

            foreach (var item in _pluginTypes)
            {
                switch (item.EntityState)
                {
                    case EntityState.Added:
                        {
                            _context.MetadataTypes.AddObject(item);
                            break;
                        }

                    case EntityState.Modified:
                        {
                            _context.MetadataTypes.ApplyCurrentValues(item);
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

        private void addPlugin_ToolStripButton_Click(object sender, EventArgs e)
        {
            PluginMetadataNewForm newForm = new PluginMetadataNewForm();
            MetadataType newType = new MetadataType();


            var dialogResult = newForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                newType.Title = newForm.PluginDisplayName;
                newType.Name = newForm.PluginName;
                newType.AssemblyName = newForm.PluginAssemblyName;

                if (newForm.PluginIcon != null)
                {
                    newType.Icon = newForm.PluginIcon.ToByteArray();
                }
                else
                {
                    newType.Icon = null;
                }

            }
            else if (dialogResult == DialogResult.Cancel)
            {
                // User Cancelled
                return;
            }


            if (EditEntry(newType, true) == DialogResult.OK)
            {
                _context.AddToMetadataTypes(newType);
                _pluginTypes.Add(newType);

                int index = plugin_RadGridView.Rows.Count - 1;
                //int nColumnIndex = 3;

                plugin_RadGridView.Rows[index].IsSelected = true;

                //In case if you want to scroll down as well.
                plugin_RadGridView.TableElement.ScrollToRow(index);
            }
        }

        private void edit_ToolStripButton_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var metadataType = row.DataBoundItem as MetadataType;

                if (EditEntry(metadataType, false) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    _context.MetadataTypes.ApplyCurrentValues(metadataType);
                    for (int i = 0; i < _pluginTypes.Count; i++)
                    {
                        if (_pluginTypes[i].Name.Equals(metadataType.Name))
                        {
                            _pluginTypes[i] = metadataType;
                        }
                    }
                    plugin_RadGridView.Refresh();
                }
            }
        }

        private void delete_ToolStripButton_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var metadataType = row.DataBoundItem as MetadataType;

                var dialogResult = MessageBox.Show
                    (
                        "Removing this entry will prevent users from selecting it during test design.  Do you want to continue?",
                        "Delete Plugin",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question

                    );

                if (dialogResult == DialogResult.Yes)
                {
                    _unsavedChanges = true;
                    _deletedItems.Add(metadataType);
                    _pluginTypes.Remove(metadataType);
                }
            }
        }

        private DialogResult EditEntry(MetadataType metadataType, bool isNewEntry)
        {
            DialogResult result = DialogResult.None;

            using (PluginMetadataEditForm form = new PluginMetadataEditForm(metadataType, _context, isNewEntry))
            {
                result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _unsavedChanges = true;
                }
            }

            return result;
        }

        private void PluginMetadataListForm_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            using (HelpDialog dialog = new HelpDialog(Properties.Resources.PluginMetadataHelpPage))
            {
                dialog.Title = "Plugin Reference Help";
                dialog.ShowDialog();
            }
        }

        private void plugin_RadGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var metadataType = row.DataBoundItem as MetadataType;

                if (EditEntry(metadataType, false) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    plugin_RadGridView.Refresh();
                }
            }
        }

    }
}
