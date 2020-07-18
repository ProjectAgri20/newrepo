using System;
using System.Collections.ObjectModel;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.WindowsAutomation;

namespace HP.ScalableTest.LabConsole
{
    public partial class ActiveDirectoryGroupManagementListForm : Form
    {
        private Collection<GroupPrincipal> _masterItemsList = null;

        private SortableBindingList<ActiveDirectoryGroup> _selectedItemsList = null;
        private EnterpriseTestContext _context = null;
        private Collection<ActiveDirectoryGroup> _deletedItems = new Collection<ActiveDirectoryGroup>();
        bool _unsavedChanges = false;

        public ActiveDirectoryGroupManagementListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialogWithHelp);

            activeDirectory_DataGridView.AutoGenerateColumns = false;

            _selectedItemsList = new SortableBindingList<ActiveDirectoryGroup>();
            _context = new EnterpriseTestContext();
        }

        private void ActiveDirectoryGroupManagementListForm_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, GlobalSettings.Items[Setting.DnsDomain]);
                var allGroups = ActiveDirectoryController.RetrieveGroups(domainContext).OrderBy(n => n.Name);
                _masterItemsList = new Collection<GroupPrincipal>(allGroups.ToList());
            }
            catch (Exception)
            {
                MessageBox.Show
                    (
                    "This User ({0}) does not have privs to communicate with Active Directory.  You will need to give this user privs before proceeding",
                    "Unable to Communicate with Active Directory",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                    );
                Close();
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            foreach (var item in _context.ActiveDirectoryGroups)
            {
                if (_masterItemsList.Any(x => x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    _selectedItemsList.Add(item);
                }
            }

            activeDirectory_DataGridView.DataSource = _selectedItemsList;
        }

        private void activeDirectory_DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (activeDirectory_DataGridView.SelectedRows.Count == 1)
            {
                var group = activeDirectory_DataGridView.SelectedRows[0].DataBoundItem as ActiveDirectoryGroup;
                
                if (EditEntry(group) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    activeDirectory_DataGridView.Refresh();
                }
            }
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
                _context.ActiveDirectoryGroups.DeleteObject(entity);
            }

            foreach (var item in _selectedItemsList)
            {
                switch (item.EntityState)
                {
                    case EntityState.Added:
                        {
                            _context.ActiveDirectoryGroups.AddObject(item);
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
            ActiveDirectoryGroup group = new ActiveDirectoryGroup();

            if (EditEntry(group) == DialogResult.OK)
            {
                _context.AddToActiveDirectoryGroups(group);
                _selectedItemsList.Add(group);

                int index = activeDirectory_DataGridView.Rows.Count - 1;

                activeDirectory_DataGridView.Rows[index].Selected = true;

                //In case if you want to scroll down as well.
                activeDirectory_DataGridView.FirstDisplayedScrollingRowIndex = index;
            }
        }

        private void edit_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (activeDirectory_DataGridView.SelectedRows.Count == 1)
            {
                var group = activeDirectory_DataGridView.SelectedRows[0].DataBoundItem as ActiveDirectoryGroup;

                if (EditEntry(group) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    activeDirectory_DataGridView.Refresh();
                }
            }
        }

        private void delete_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (activeDirectory_DataGridView.SelectedRows.Count == 1)
            {
                var group = activeDirectory_DataGridView.SelectedRows[0].DataBoundItem as ActiveDirectoryGroup;

                var dialogResult = MessageBox.Show
                    (
                        "Removing group {0}.  Do you want to continue?".FormatWith(group.Name),
                        "Delete User",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question

                    );

                if (dialogResult == DialogResult.Yes)
                {
                    _unsavedChanges = true;
                    _deletedItems.Add(group);
                    _selectedItemsList.Remove(group);
                }
            }
        }

        private DialogResult EditEntry(ActiveDirectoryGroup group)
        {
            DialogResult result = DialogResult.None;

            using (var form = new ActiveDirectoryGroupManagementForm(_masterItemsList, _selectedItemsList, group))
            {
                result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _unsavedChanges = true;
                }
            }

            return result;
        }
    }
}
