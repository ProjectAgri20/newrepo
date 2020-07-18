using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.LabConsole
{
    public partial class UserGroupsConfigForm : Form
    {
        private SortableBindingList<UserGroup> _groups = new SortableBindingList<UserGroup>();
        private SortableBindingList<User> _users = new SortableBindingList<User>();
        private EnterpriseTestContext _context = null;
        private bool _unsavedChanges = false;
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private int _numberOfThreads = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualMachinePlatformConfigForm"/> class.
        /// </summary>
        public UserGroupsConfigForm()
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);

            _context = new EnterpriseTestContext();
        }

        /// <summary>
        /// Handles the Load event of the VirtualMachinePlatformConfigForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void UserGroupsConfigForm_Load(object sender, EventArgs e)
        {
            groups_RadGridView.AutoGenerateColumns = false;
            user_RadGridView.AutoGenerateColumns = false;

            using (new BusyCursor())
            {
                BackgroundWorker loadUsersWorker = new BackgroundWorker();
                loadUsersWorker.DoWork += new DoWorkEventHandler(loadUsersWorker_DoWork);
                loadUsersWorker.RunWorkerAsync();

                BackgroundWorker loadGroupsWorker = new BackgroundWorker();
                loadGroupsWorker.DoWork += new DoWorkEventHandler(loadGroupsWorker_DoWork);
                loadGroupsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadGroupsWorker_RunWorkerCompleted);
                loadGroupsWorker.RunWorkerAsync();
            }
        }

        private GridViewRowInfo GetFirstSelectedGroupRow()
        {
            return groups_RadGridView.SelectedRows.FirstOrDefault();
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the loadPlatformWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void loadGroupsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var row = GetFirstSelectedGroupRow();
            if (row != null)
            {
                var selectedGroup = row.DataBoundItem as UserGroup;
                PopulateCheckboxes(selectedGroup);
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Handles the DoWork event of the loadPlatformWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void loadGroupsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            groups_RadGridView.Invoke(new MethodInvoker(PopulateUserGroupGrid));
            _resetEvent.WaitOne();
        }

        /// <summary>
        /// Populates the platform grid.
        /// </summary>
        private void PopulateUserGroupGrid()
        {
            foreach (UserGroup group in _context.UserGroups.OrderBy(f => f.GroupName))
            {
                _groups.Add(group);
            }

            groups_RadGridView.DataSource = null;
            groups_RadGridView.DataSource = _groups;

            groups_RadGridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            groups_RadGridView.BestFitColumns();
        }

        /// <summary>
        /// Handles the DoWork event of the loadVirtualMachinesWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void loadUsersWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            user_RadGridView.Invoke(new MethodInvoker(PopulateUserGrid));

            // If this thread is the last, the set the wait event
            if (Interlocked.Decrement(ref _numberOfThreads) == 0)
            {
                _resetEvent.Set();
            }
        }

        /// <summary>
        /// Populates the virtual machine grid.
        /// </summary>
        private void PopulateUserGrid()
        {
            foreach (User user in _context.Users.OrderBy(f => f.UserName))
            {
                _users.Add(user);
            }

            user_RadGridView.DataSource = null;
            user_RadGridView.DataSource = _users;

            user_RadGridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            user_RadGridView.BestFitColumns();
        }

        private void groups_RadGridView_CellClick(object sender, GridViewCellEventArgs e)
        {
            UpdateUserSelections();
        }

        private void UpdateUserSelections()
        {
            user_RadGridView.ClearSelection();

            var row = GetFirstSelectedGroupRow();
            if (row != null)
            {
                var group = row.DataBoundItem as UserGroup;

                if (group != null)
                {
                    PopulateCheckboxes(group);
                }
            }
        }

        private void groups_RadGridView_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            foreach (var row in user_RadGridView.Rows)
            {
                row.Cells[0].Value = false;
            }
        }

        /// <summary>
        /// Populates the checkboxes.
        /// </summary>
        /// <param name="selectedGroup">The selected platform.</param>
        private void PopulateCheckboxes(UserGroup selectedGroup)
        {
            using (new BusyCursor())
            {
                var userNames = selectedGroup.Users.Select(x => x.UserName);
                if (userNames.Count() == 0)
                {
                    foreach (var row in user_RadGridView.Rows)
                    {
                        row.Cells[0].Value = false;
                    }
                }
                else
                {
                    foreach (var row in user_RadGridView.Rows)
                    {
                        bool check = userNames.Contains((string)row.Cells["userNameColumn"].Value);
                        row.Cells[0].Value = check;
                    }
                }
            }
        }

        private void user_RadGridView_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    // The boolean value here is swapped, because the checked value of the control is 
                    // opposite of where it is heading.
                    bool isChecked = !(bool)user_RadGridView.Rows[e.RowIndex].Cells["userSelectedColumn"].Value;
                    UserGroup currentGroup = GetFirstSelectedGroupRow().DataBoundItem as UserGroup;
                    User user = user_RadGridView.Rows[e.RowIndex].DataBoundItem as User;

                    UpdateUserList(isChecked, currentGroup, user);

                    _unsavedChanges = true;
                }
            }
        }

        /// <summary>
        /// Bulk updates the virtual machine checkbox.
        /// </summary>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        private void BulkUpdateUserCheckbox(bool isChecked)
        {
            using (new BusyCursor())
            {
                var currentGroup = GetFirstSelectedGroupRow().DataBoundItem as UserGroup;
                foreach (var row in user_RadGridView.SelectedRows)
                {
                    if ((bool)row.Cells[0].Value != isChecked)
                    {
                        row.Cells[0].Value = isChecked;

                        var user = row.DataBoundItem as User;
                        UpdateUserList(isChecked, currentGroup, user);
                    }
                }

                _unsavedChanges = true;
            }
        }

        /// <summary>
        /// Updates the virtual machine list.
        /// </summary>
        /// <param name="isChecked">if set to <c>true</c> the checkbox is checked.</param>
        /// <param name="currentGroup">The current platform.</param>
        /// <param name="user">The virtual machine.</param>
        private void UpdateUserList(bool isChecked, UserGroup currentGroup, User user)
        {
            // If the virtual machine isn't NULL, then update the virtual machine list to either
            // include or remove the virtual machine, depending on the check setting.
            if (user != null && currentGroup != null)
            {
                if (isChecked && !currentGroup.Users.Contains(user))
                {
                    currentGroup.Users.Add(user);
                }
                else
                {
                    currentGroup.Users.Remove(user);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the cancel_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cancel_Button_Click(object sender, EventArgs e)
        {
            if (_unsavedChanges)
            {
                var result = MessageBox.Show
                    (
                        "You have unsaved changes that will be lost.  Do you want to continue?",
                        "Unsaved Changes",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                if (result == DialogResult.Yes)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Handles the Click event of the ok_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ok_Button_Click(object sender, EventArgs e)
        {
            SaveChanges();
            Close();
        }

        /// <summary>
        /// Handles the Click event of the apply_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void apply_Button_Click(object sender, EventArgs e)
        {
            SaveChanges();
            _unsavedChanges = false;
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        private void SaveChanges()
        {
            using (new BusyCursor())
            {
                foreach (UserGroup group in _groups)
                {
                    if (group.EntityState == EntityState.Added || group.EntityState == EntityState.Detached)
                    {
                        _context.UserGroups.AddObject(group);
                    }
                }
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Handles the Click event of the selectAllToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BulkUpdateUserCheckbox(true);
        }

        /// <summary>
        /// Handles the Click event of the uncheckAllToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uncheckAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BulkUpdateUserCheckbox(false);
        }

        /// <summary>
        /// Handles the Click event of the deleteToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveGroup();
        }

        private void addGroupToolStripButton_Click(object sender, EventArgs e)
        {
            UserGroup group = new UserGroup();

            if (EditEntry(group) == DialogResult.OK)
            {
                _context.AddToUserGroups(group);
                _groups.Add(group);

                //int index = user_RadGridView.Rows.Count - 1;

                //user_RadGridView.Rows[index].IsSelected = true;

                //In case if you want to scroll down as well.
                user_RadGridView.TableElement.ScrollToRow(0);

                foreach (var row in user_RadGridView.Rows)
                {
                    row.Cells[0].Value = false;
                }
            }
        }

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
            EditUserGroup();
        }
        
        private void EditUserGroup()
        {
            var row = GetFirstSelectedGroupRow();
            if (row != null)
            {
                var group = row.DataBoundItem as UserGroup;
                EditEntry(group);
            }
        }

        private void removeToolStripButton_Click(object sender, EventArgs e)
        {
            RemoveGroup();
        }

        private void RemoveGroup()
        {
            var row = GetFirstSelectedGroupRow();

            if (row != null)
            {
                var group = row.DataBoundItem as UserGroup;

                var dialogResult = MessageBox.Show
                    (
                        "Removing Group {0}.  Do you want to continue?".FormatWith(group.GroupName),
                        "Delete User Group",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question

                    );

                if (dialogResult == DialogResult.Yes)
                {
                    _unsavedChanges = true;
                    _groups.Remove(group);

                    UpdateUserSelections();
                }
            }
        }

        private DialogResult EditEntry(UserGroup group)
        {
            DialogResult result = DialogResult.None;

            using (UserGroupEditForm form = new UserGroupEditForm(group, _context))
            {
                result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _unsavedChanges = true;
                }
            }

            return result;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditUserGroup();
        }
    }
}
