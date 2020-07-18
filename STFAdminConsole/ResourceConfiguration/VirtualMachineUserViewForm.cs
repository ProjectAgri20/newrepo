using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.LabConsole
{
    public partial class VirtualMachineUserViewForm : Form
    {
        private SortableBindingList<User> _users = new SortableBindingList<User>();
        private SortableBindingList<VirtualMachine> _machines = new SortableBindingList<VirtualMachine>();
        private SortableBindingList<UserGroup> _groups = new SortableBindingList<UserGroup>();
        private EnterpriseTestEntities _entities = null;

        private User _selectedUser = null;

        private readonly object _lockObject = new object();
        Semaphore _semaphore = new Semaphore(0, 2);

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualMachinePlatformConfigForm"/> class.
        /// </summary>
        public VirtualMachineUserViewForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            _entities = new EnterpriseTestContext();

            platformFilter_ToolStripComboBox.ComboBox.DisplayMember = "Name";
            platformFilter_ToolStripComboBox.ComboBox.ValueMember = "FrameworkClientPlatformId";
        }


        #region Form Load and Unload

        /// <summary>
        /// Handles the Load event of the VirtualMachinePlatformConfigForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void VirtualMachineGroupConfigForm_Load(object sender, EventArgs e)
        {
            users_DataGridView.AutoGenerateColumns = false;
            machines_DataGridView.AutoGenerateColumns = false;
            groups_DataGridView.AutoGenerateColumns = false;

            Cursor = Cursors.WaitCursor;

            platformFilter_ToolStripComboBox.Items.Add("All Platforms");
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (var item in context.FrameworkClientPlatforms.Where(x => x.Active == true))
                {
                    platformFilter_ToolStripComboBox.Items.Add(item);
                }
            }

            platformFilter_ToolStripComboBox.SelectedIndex = 0;

            // Spin off a thread to load each piece of data, then have then sync up through a 
            // semaphore and come back together in the loadPlatformWorker_RunWorkerCompleted() call
            // where a bit more initialization will occur
            BackgroundWorker loadMachinesWorker = new BackgroundWorker();
            loadMachinesWorker.DoWork += new DoWorkEventHandler(LoadMachines_DoWork);
            loadMachinesWorker.RunWorkerAsync();

            BackgroundWorker loadGroupsWorker = new BackgroundWorker();
            loadGroupsWorker.DoWork += new DoWorkEventHandler(LoadGroups_DoWork);
            loadGroupsWorker.RunWorkerAsync();

            BackgroundWorker loadUsersWorker = new BackgroundWorker();
            loadUsersWorker.DoWork += new DoWorkEventHandler(LoadUsers_DoWork);
            loadUsersWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadUsers_RunWorkerCompleted);
            loadUsersWorker.RunWorkerAsync();

            platformFilter_ToolStripComboBox.SelectedIndexChanged += platformFilter_ToolStripComboBox_SelectedIndexChanged;
        }

        #endregion

        #region Load Platform and Virtual Machine Grids

        /// <summary>
        /// Handles the RunWorkerCompleted event of the loadPlatformWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void LoadUsers_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (users_DataGridView.Rows.Count > 0)
            {
                users_DataGridView.Rows[0].Selected = true;
                _selectedUser = users_DataGridView.Rows[0].DataBoundItem as Data.EnterpriseTest.Model.User;

                Task.Factory.StartNew(() => groups_DataGridView.Invoke(new MethodInvoker(() => PopulateGroupCheckboxes(_selectedUser))));
                Task.Factory.StartNew(() => machines_DataGridView.Invoke(new MethodInvoker(() => PopulateMachineCheckboxes(_selectedUser))));

            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Handles the DoWork event of the loadPlatformWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void LoadUsers_DoWork(object sender, DoWorkEventArgs e)
        {
            users_DataGridView.Invoke(new MethodInvoker(PopulateUsersGrid));
            _semaphore.WaitOne();
        }

        /// <summary>
        /// Populates the platform grid.
        /// </summary>
        private void PopulateUsersGrid()
        {
            _users.Clear();

            IEnumerable<User> users = null;
            lock (_lockObject)
            {
                users = _entities.Users.OrderBy(f => f.UserName);
            }

            foreach (User user in users)
            {
                _users.Add(user);
            }

            users_DataGridView.DataSource = null;
            users_DataGridView.DataSource = _users;
        }

        /// <summary>
        /// Handles the DoWork event of the loadPlatformWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void LoadGroups_DoWork(object sender, DoWorkEventArgs e)
        {
            groups_DataGridView.Invoke(new MethodInvoker(PopulateGroupsGrid));
            _semaphore.Release();
        }

        /// <summary>
        /// Populates the platform grid.
        /// </summary>
        private void PopulateGroupsGrid()
        {
            _groups.Clear();

            IEnumerable<UserGroup> groups = null;
            lock (_lockObject)
            {
                groups = _entities.UserGroups.OrderBy(f => f.GroupName);
            }
            foreach (UserGroup group in groups)
            {
                _groups.Add(group);
            }

            groups_DataGridView.DataSource = null;
            groups_DataGridView.DataSource = _groups;
        }

        /// <summary>
        /// Handles the DoWork event of the loadVirtualMachinesWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void LoadMachines_DoWork(object sender, DoWorkEventArgs e)
        {
            machines_DataGridView.Invoke(new MethodInvoker(PopulateMachinesGrid));
            _semaphore.Release();
        }

        /// <summary>
        /// Populates the virtual machine grid.
        /// </summary>
        private void PopulateMachinesGrid()
        {
            _machines.Clear();

            IEnumerable<VirtualMachine> machines = null;
            lock (_lockObject)
            {
                machines = VirtualMachine.SelectAll().OrderBy(m => m.Name);
            }

            foreach (VirtualMachine machine in machines)
            {
                _machines.Add(machine);
            }

            machines_DataGridView.DataSource = null;
            machines_DataGridView.DataSource = _machines;
        }

        private void PopulateMachinesGrid(string platformId)
        {
            IEnumerable<VirtualMachine> machines = VirtualMachine.Select(platform: platformId);
            var query =
                (
                    from m in machines
                    from p in m.Platforms
                    where platformId.Equals(p.FrameworkClientPlatformId)
                    select m
                ).Distinct();

            _machines.Clear();
            foreach (var item in query)
            {
                _machines.Add(item);
            }

            machines_DataGridView.DataSource = null;
            machines_DataGridView.DataSource = _machines;
        }

        #endregion

        #region Platform DataGrid Handlers

        /// <summary>
        /// Handles the CellClick event of the domainGroup_DataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void users_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                machines_DataGridView.ClearSelection();

                if (users_DataGridView.SelectedRows.Count == 1)
                {
                    _selectedUser = users_DataGridView.SelectedRows[0].DataBoundItem as User;
                    Task.Factory.StartNew(() => groups_DataGridView.Invoke(new MethodInvoker(() => PopulateGroupCheckboxes( _selectedUser))));
                    Task.Factory.StartNew(() => machines_DataGridView.Invoke(new MethodInvoker(() => PopulateMachineCheckboxes(_selectedUser))));
                }
            }
        }

        private void groups_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (groups_DataGridView.SelectedRows.Count == 1)
                {
                    var selectedGroup = groups_DataGridView.SelectedRows[0].DataBoundItem as UserGroup;

                    if (_selectedUser != null)
                    {
                        var group = _entities.UserGroups.FirstOrDefault(x => x.GroupName.Equals(selectedGroup.GroupName));
                        var any = group != null && group.Users.Any(x => x.UserName.Equals(_selectedUser.UserName));

                        if (group != null && any)
                        {
                            PopulateMachineCheckboxes(selectedGroup);
                        }
                        else
                        {
                            ClearMachineBackground();
                        }
                    }
                    else
                    {
                        ClearMachineBackground();
                    }
                }
            }
        }


        /// <summary>
        /// Handles the RowValidating event of the domainGroup_DataGridView control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void group_DataGridView_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Row Validating " + e.RowIndex + " PlatformCount= " + _users.Count);

            e.Cancel = !IsGroupRowValid
            (
                users_DataGridView.Rows[e.RowIndex].Cells[0].FormattedValue as string,
                users_DataGridView.Rows.Count - 1 == e.RowIndex
            );
        }

        /// <summary>
        /// If PlatformId is null and we are on the last row, we consider it valid because nothing was added to the grid.
        /// Otherwise, we check both platformId and Description fields for data.
        /// </summary>
        /// <param name="platformId"></param>
        /// <param name="description"></param>
        /// <param name="isRowLast"></param>
        /// <returns></returns>
        private bool IsGroupRowValid(string groupName, bool isRowLast)
        {
            bool isValid = true;

            if (!isRowLast && string.IsNullOrEmpty(groupName))
            {
                DisplayValidationError("Group name must have a value.");
                isValid = false;
            }

            return isValid;
        }

        private void DisplayValidationError(string message)
        {
            MessageBox.Show(this, message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Handles the UserDeletingRow event of the domainGroup_DataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewRowCancelEventArgs"/> instance containing the event data.</param>
        private void groups_DataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // This will need a check to see if any virtual resources
            var group = e.Row.DataBoundItem as UserGroup;
            e.Cancel = DeleteUserGroup(group);
        }

        /// <summary>
        /// Deletes the platform entry.
        /// </summary>
        /// <param name="group">The platform.</param>
        /// <returns></returns>
        private bool DeleteUserGroup(UserGroup group)
        {
            bool cancel = false;
            Cursor = Cursors.WaitCursor;
            if (group.EntityState != EntityState.Detached)
            {
                _entities.UserGroups.DeleteObject(group);
            }
            Cursor = Cursors.Default;

            return cancel;
        }

#endregion

        #region Virtual Machine DataGrid Handlers

        private void ClearMachineBackground()
        {
            foreach (DataGridViewRow row in machines_DataGridView.Rows)
            {
                row.Cells[0].Style.BackColor = Color.White;
            }
        }

        private void PopulateMachineCheckboxes(UserGroup selectedGroup)
        {
            Cursor = Cursors.WaitCursor;
            UserGroup currentGroup = _groups.FirstOrDefault(f => f.GroupName == selectedGroup.GroupName);
            if (currentGroup != null)
            {
                List<string> hostNames = (from u in currentGroup.VirtualMachineGroupAssocs select u.MachineName).ToList();
                if (hostNames.Count == 0)
                {
                    ClearMachineBackground();
                }
                else
                {
                    foreach (DataGridViewRow row in machines_DataGridView.Rows)
                    {
                        bool check = hostNames.Contains((string)row.Cells["hostNameColumn"].Value);
                        if (check)
                        {
                            row.Cells[0].Style.BackColor = Color.Yellow;
                        }
                        else
                        {
                            row.Cells[0].Style.BackColor = Color.White;
                        }
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void PopulateMachineCheckboxes(Data.EnterpriseTest.Model.User selectedUser)
        {
            Cursor = Cursors.WaitCursor;
            User currentUser = _users.FirstOrDefault(f => f.UserName == selectedUser.UserName);
            if (currentUser != null)
            {
                var hostNames = currentUser.UserGroups.SelectMany(e => e.VirtualMachineGroupAssocs).Select(e => e.MachineName).Distinct();

                if (hostNames.Count() == 0)
                {
                    foreach (DataGridViewRow row in machines_DataGridView.Rows)
                    {
                        row.Cells[0].Value = machine_ImageList.Images["Blank"];
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in machines_DataGridView.Rows)
                    {
                        bool check = hostNames.Contains((string)row.Cells["hostNameColumn"].Value);
                        if (check)
                        {
                            row.Cells[0].Value = machine_ImageList.Images["Tick"];
                        }
                        else
                        {
                            row.Cells[0].Value = machine_ImageList.Images["Blank"];
                        }
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void PopulateGroupCheckboxes(Data.EnterpriseTest.Model.User selectedUser)
        {
            Cursor = Cursors.WaitCursor;
            var currentUser = _users.FirstOrDefault(f => f.UserName == selectedUser.UserName);
            if (currentUser != null)
            {
                var groupNames = currentUser.UserGroups.Select(e => e.GroupName).Distinct();

                if (groupNames.Count() == 0)
                {
                    foreach (DataGridViewRow row in groups_DataGridView.Rows)
                    {
                        row.Cells[0].Value = machine_ImageList.Images["Blank"];
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in groups_DataGridView.Rows)
                    {
                        bool check = groupNames.Contains((string)row.Cells["groupNameColumn"].Value);
                        row.Cells[0].Value = check ? machine_ImageList.Images["Tick"] : machine_ImageList.Images["Blank"];
                    }
                }
            }

            if (groups_DataGridView.Rows.Count > 0)
            {
                groups_DataGridView.Rows[0].Selected = true;
                var selectedGroup = groups_DataGridView.SelectedRows[0].DataBoundItem as UserGroup;

                var group = _entities.UserGroups.FirstOrDefault(x => x.GroupName.Equals(selectedGroup.GroupName));
                var any = group != null && group.Users.Any(x => x.UserName.Equals(selectedUser.UserName));

                if (group != null && any)
                {
                    PopulateMachineCheckboxes(selectedGroup);
                }
                else
                {
                    ClearMachineBackground();
                }
            }

            Cursor = Cursors.Default;
        }


#endregion

        /// <summary>
        /// Handles the Click event of the ok_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ok_Button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void platformFilter_ToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string name = (string)platformFilter_ToolStripComboBox.ComboBox.Text;
            if (name.Equals("All Platforms"))
            {
                PopulateMachinesGrid();
            }
            else
            {
                name = ((FrameworkClientPlatform)platformFilter_ToolStripComboBox.ComboBox.SelectedItem).FrameworkClientPlatformId;
                PopulateMachinesGrid(name);
            }

            PopulateMachineCheckboxes(_selectedUser);

            if (groups_DataGridView.SelectedRows.Count == 1)
            {
                var selectedGroup = groups_DataGridView.SelectedRows[0].DataBoundItem as UserGroup;

                if (_selectedUser != null)
                {
                    var group = _entities.UserGroups.FirstOrDefault(x => x.GroupName.Equals(selectedGroup.GroupName));
                    var any = group != null && group.Users.Any(x => x.UserName.Equals(_selectedUser.UserName));

                    if (group != null && any)
                    {
                        PopulateMachineCheckboxes(selectedGroup);
                    }
                    else
                    {
                        ClearMachineBackground();
                    }
                }
                else
                {
                    ClearMachineBackground();
                }
            }
        }
    }
}
