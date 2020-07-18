using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.LabConsole
{
    public partial class VirtualMachineGroupConfigForm : Form
    {
        private SortableBindingList<UserGroup> _groups = new SortableBindingList<UserGroup>();
        private SortableBindingList<VirtualMachine> _machines = new SortableBindingList<VirtualMachine>();
        private EnterpriseTestEntities _entities = null;

        private ManualResetEvent _resetEvent = new ManualResetEvent(false);

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualMachinePlatformConfigForm"/> class.
        /// </summary>
        public VirtualMachineGroupConfigForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            _entities = new EnterpriseTestContext();
            //bind the virtual machine list to the combobox, the display will always the name
            // and the value will be the platformId
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
            groups_DataGridView.AutoGenerateColumns = false;
            machines_DataGridView.AutoGenerateColumns = false;

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
            loadGroupsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadGroups_RunWorkerCompleted);
            loadGroupsWorker.RunWorkerAsync();            
        }

        #endregion

        #region Load Platform and Virtual Machine Grids

        /// <summary>
        /// Handles the RunWorkerCompleted event of the loadPlatformWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void LoadGroups_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (groups_DataGridView.Rows.Count > 0)
            {
                groups_DataGridView.Rows[0].Selected = true;
                var selectedGroup = groups_DataGridView.Rows[0].DataBoundItem as UserGroup;
                PopulateMachineCheckboxes(selectedGroup);
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Handles the DoWork event of the loadPlatformWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void LoadGroups_DoWork(object sender, DoWorkEventArgs e)
        {
            groups_DataGridView.Invoke(new MethodInvoker(PopulateGroupsGrid));
            _resetEvent.WaitOne();
        }

        /// <summary>
        /// Populates the platform grid.
        /// </summary>
        private void PopulateGroupsGrid()
        {
            _groups.Clear();

            foreach (UserGroup group in _entities.UserGroups.OrderBy(f => f.GroupName))
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
            _resetEvent.Set();
        }

        /// <summary>
        /// Populates the virtual machine grid.
        /// </summary>
        private void PopulateMachinesGrid()
        {
            _machines.Clear();
            foreach (VirtualMachine machine in VirtualMachine.SelectAll().OrderBy(f => f.Name))
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
        private void groups_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                machines_DataGridView.ClearSelection();

                if (groups_DataGridView.SelectedRows.Count == 1)
                {
                    var selectedGroup = groups_DataGridView.SelectedRows[0].DataBoundItem as UserGroup;

                    if (selectedGroup != null)
                    {
                        PopulateMachineCheckboxes(selectedGroup);
                    }
                }
            }
        }

#endregion

        #region Virtual Machine DataGrid Handlers

        /// <summary>
        /// Populates the checkboxes.
        /// </summary>
        /// <param name="selectedGroup">The selected group.</param>
        private void PopulateMachineCheckboxes(UserGroup selectedGroup)
        {
            Cursor = Cursors.WaitCursor;
            UserGroup currentGroup = _groups.FirstOrDefault(f => f.GroupName == selectedGroup.GroupName);
            if (currentGroup != null)
            {
                var hostNames = (from u in currentGroup.VirtualMachineGroupAssocs select u.MachineName).ToList();
                if (hostNames.Count == 0)
                {
                    foreach (DataGridViewRow row in machines_DataGridView.Rows)
                    {
                        row.Cells[0].Value = false;
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in machines_DataGridView.Rows)
                    {
                        bool check = hostNames.Contains((string)row.Cells["hostNameColumn"].Value);
                        row.Cells[0].Value = check;
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void machines_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    // The boolean value here is swapped, because the checked value of the control is 
                    // opposite of where it is heading.
                    bool  isChecked = !(bool)machines_DataGridView.Rows[e.RowIndex].Cells["machineSelectedColumn"].Value;
                    UserGroup currentGroup = groups_DataGridView.SelectedRows[0].DataBoundItem as UserGroup;
                    VirtualMachine machine = machines_DataGridView.Rows[e.RowIndex].DataBoundItem as VirtualMachine;

                    UpdateMachineList(isChecked, currentGroup, GetMachineGroupAssoc(currentGroup.GroupName, machine.Name));
                }
            }
        }

        /// <summary>
        /// Bulk updates the virtual machine checkbox.
        /// </summary>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        private void BulkUpdateVirtualMachineCheckbox(bool isChecked)
        {
            Cursor = Cursors.WaitCursor;
            var currentGroup = groups_DataGridView.SelectedRows[0].DataBoundItem as UserGroup;
            foreach (DataGridViewRow row in machines_DataGridView.SelectedRows)
            {
                row.Cells[0].Value = isChecked;

                VirtualMachine machine = row.DataBoundItem as VirtualMachine;
                UpdateMachineList(isChecked, currentGroup, GetMachineGroupAssoc(currentGroup.GroupName, machine.Name));
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Updates the virtual machine list.
        /// </summary>
        /// <param name="isChecked">if set to <c>true</c> the checkbox is checked.</param>
        /// <param name="currentGroup">The current platform.</param>
        /// <param name="machineAssoc">The virtual machine/group association.</param>
        private void UpdateMachineList(bool isChecked, UserGroup currentGroup, VirtualMachineGroupAssoc machineAssoc)
        {
            // If the virtual machine isn't NULL, then update the virtual machine list to either
            // include or remove the virtual machine, depending on the check setting.
            if (machineAssoc != null)
            {
                var exists = currentGroup.VirtualMachineGroupAssocs.Any(x => x.GroupName.Equals(machineAssoc.GroupName) && x.MachineName.Equals(machineAssoc.MachineName));

                if (isChecked && !exists)
                {
                    // If this association has already been deleted (unchecked) without a save, then create a replacement entry
                    // and add it back into the collection.  Otherwise an exception will occur saying the object is in a deleted
                    // state and can't be added.
                    if (machineAssoc.EntityState == System.Data.EntityState.Deleted)
                    {
                        var replacement = VirtualMachineGroupAssoc.CreateVirtualMachineGroupAssoc(machineAssoc.MachineName, machineAssoc.GroupName);
                        currentGroup.VirtualMachineGroupAssocs.Add(replacement);
                    }
                    else
                    {
                        currentGroup.VirtualMachineGroupAssocs.Add(machineAssoc);
                    }
                }
                else if (!isChecked && exists)
                {
                    var item = currentGroup.VirtualMachineGroupAssocs.First(x => x.GroupName.Equals(machineAssoc.GroupName) && x.MachineName.Equals(machineAssoc.MachineName));
                    currentGroup.VirtualMachineGroupAssocs.Remove(item);
                }
            }
        }

        private VirtualMachineGroupAssoc GetMachineGroupAssoc(string groupName, string machineName)
        {
            VirtualMachineGroupAssoc result = (from a in _entities.VirtualMachineGroupAssocs
                                               where a.MachineName == machineName && a.GroupName == groupName
                                               select a).FirstOrDefault();

            if (result == null)
            {
                result = new VirtualMachineGroupAssoc()
                {
                    MachineName = machineName,
                    GroupName = groupName
                };
            }

            return result;
        }

        private void machines_DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (machines_DataGridView.IsCurrentCellDirty)
            {
                machines_DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

#endregion

        #region Button and Menu Handlers

        /// <summary>
        /// Handles the Click event of the cancel_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cancel_Button_Click(object sender, EventArgs e)
        {
            Close();
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
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        private void SaveChanges()
        {
            Cursor = Cursors.WaitCursor;
            _entities.SaveChanges();
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Handles the Click event of the selectAllToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BulkUpdateVirtualMachineCheckbox(true);
        }

        /// <summary>
        /// Handles the Click event of the uncheckAllToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uncheckAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BulkUpdateVirtualMachineCheckbox(false);
        }

        #endregion

        private void select_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;

            foreach (DataGridViewRow row in machines_DataGridView.Rows)
            {
                row.Cells[0].Value = checkBox.Checked;
            }
        }

        private void platformFilter_ToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //get the current chosen platform
            string name = (string)platformFilter_ToolStripComboBox.ComboBox.Text;
            

            if (name.Equals("All Platforms"))
            {
                PopulateMachinesGrid();
            }
            else
            {
                //send the platformId to populate the grid
                name = ((FrameworkClientPlatform)platformFilter_ToolStripComboBox.ComboBox.SelectedItem).FrameworkClientPlatformId;
                PopulateMachinesGrid(name);
            }

            if (groups_DataGridView.Rows.Count > 1)
            {
                var selectedGroup = groups_DataGridView.SelectedRows[0].DataBoundItem as UserGroup;
                PopulateMachineCheckboxes(selectedGroup);
            }
        }

        private void userAssignments_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (VirtualMachineUserViewForm form = new VirtualMachineUserViewForm())
            {
                form.ShowDialog();
            }
        }
    }
}
