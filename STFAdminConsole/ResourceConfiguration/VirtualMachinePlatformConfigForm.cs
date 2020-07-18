using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using VirtualMachine = HP.ScalableTest.Data.EnterpriseTest.VirtualMachine;

namespace HP.ScalableTest.LabConsole
{
    public partial class VirtualMachinePlatformConfigForm : Form
    {
        private SortableBindingList<FrameworkClientPlatform> _platforms = new SortableBindingList<FrameworkClientPlatform>();
        private SortableBindingList<VirtualMachineRow> _virtualMachines = new SortableBindingList<VirtualMachineRow>();
        private SortableBindingList<ResourceTypeRow> _resourceTypes = new SortableBindingList<ResourceTypeRow>();
        private AssetInventoryContext _assetInventoryContext = null;
        private EnterpriseTestContext _enterpriseTestContext = null;

        private ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private int _numberOfThreads = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualMachinePlatformConfigForm"/> class.
        /// </summary>
        public VirtualMachinePlatformConfigForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            _assetInventoryContext = DbConnect.AssetInventoryContext();
            _enterpriseTestContext = DbConnect.EnterpriseTestContext();
        }

        private class VirtualMachineRow
        {
            public VirtualMachineRow(VirtualMachine machine)
            {
                Machine = machine;
            }

            public VirtualMachine Machine { get; private set; }
            public bool Selected { get; set; }
            public string Name { get { return Machine.Name; } }
            public string HoldId { get { return Machine.HoldId; } }
        }

        #region Form Load and Unload

        /// <summary>
        /// Handles the Load event of the VirtualMachinePlatformConfigForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void VirtualMachinePlatformConfigForm_Load(object sender, EventArgs e)
        {
            resourceType_DataGridView.AutoGenerateColumns = false;
            platform_DataGridView.AutoGenerateColumns = false;
            virtualMachine_DataGridView.AutoGenerateColumns = false;

            Cursor = Cursors.WaitCursor;

            // Spin off a thread to load each piece of data, then have then sync up through a 
            // semaphore and come back together in the loadPlatformWorker_RunWorkerCompleted() call
            // where a bit more initialization will occur
            BackgroundWorker loadVirtualMachinesWorker = new BackgroundWorker();
            loadVirtualMachinesWorker.DoWork += new DoWorkEventHandler(loadVirtualMachinesWorker_DoWork);
            loadVirtualMachinesWorker.RunWorkerAsync();

            BackgroundWorker loadResourceTypesWorker = new BackgroundWorker();
            loadResourceTypesWorker.DoWork += new DoWorkEventHandler(loadResourceTypesWorker_DoWork);
            loadResourceTypesWorker.RunWorkerAsync();

            BackgroundWorker loadPlatformWorker = new BackgroundWorker();
            loadPlatformWorker.DoWork += new DoWorkEventHandler(loadPlatformWorker_DoWork);
            loadPlatformWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadPlatformWorker_RunWorkerCompleted);
            loadPlatformWorker.RunWorkerAsync();            
        }

        /// <summary>
        /// Handles the DoWork event of the loadResourceTypesWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void loadResourceTypesWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            resourceType_DataGridView.Invoke(new MethodInvoker(PopulateResourceTypeGrid));

            // If this thread is the last, the set the wait event
            if (Interlocked.Decrement(ref _numberOfThreads) == 0)
            {
                _resetEvent.Set();
            }
        }

        /// <summary>
        /// Populates the resource type grid.
        /// </summary>
        private void PopulateResourceTypeGrid()
        {
            foreach (ResourceType resourceType in _enterpriseTestContext.ResourceTypes)
            {
                _resourceTypes.Add(new ResourceTypeRow(resourceType));
            }
            resourceType_DataGridView.DataSource = null;
            resourceType_DataGridView.DataSource = _resourceTypes;
        }

        #endregion

        #region Load Platform and Virtual Machine Grids

        /// <summary>
        /// Handles the RunWorkerCompleted event of the loadPlatformWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void loadPlatformWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (platform_DataGridView.Rows.Count > 1)
            {
                var selectedPlatform = platform_DataGridView.Rows[0].DataBoundItem as FrameworkClientPlatform;
                PopulateCheckboxes(selectedPlatform);
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Handles the DoWork event of the loadPlatformWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void loadPlatformWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            platform_DataGridView.Invoke(new MethodInvoker(PopulatePlatformGrid));
            _resetEvent.WaitOne();
        }

        /// <summary>
        /// Populates the platform grid.
        /// </summary>
        private void PopulatePlatformGrid()
        {
            foreach (FrameworkClientPlatform platform in _assetInventoryContext.FrameworkClientPlatforms.OrderBy(f => f.FrameworkClientPlatformId))
            {
                _platforms.Add(platform);
            }
            platform_DataGridView.DataSource = null;
            platform_DataGridView.DataSource = _platforms;
        }

        /// <summary>
        /// Handles the DoWork event of the loadVirtualMachinesWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void loadVirtualMachinesWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            platform_DataGridView.Invoke(new MethodInvoker(PopulateVirtualMachineGrid));

            // If this thread is the last, the set the wait event
            if (Interlocked.Decrement(ref _numberOfThreads) == 0)
            {
                _resetEvent.Set();
            }
        }

        /// <summary>
        /// Populates the virtual machine grid.
        /// </summary>
        private void PopulateVirtualMachineGrid()
        {
            foreach (VirtualMachine machine in VirtualMachine.SelectAll().OrderBy(f => f.SortOrder))
            {
                _virtualMachines.Add(new VirtualMachineRow(machine));
            }
            virtualMachine_DataGridView.DataSource = null;
            virtualMachine_DataGridView.DataSource = _virtualMachines;
        }

        #endregion

        #region Platform DataGrid Handlers

        private void platform_DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            virtualMachine_DataGridView.ClearSelection();
            resourceType_DataGridView.ClearSelection();

            if (platform_DataGridView.SelectedRows.Count == 1)
            {
                FrameworkClientPlatform selectedPlatform = platform_DataGridView.SelectedRows[0].DataBoundItem as FrameworkClientPlatform;

                if (selectedPlatform != null)
                {
                    PopulateCheckboxes(selectedPlatform);
                }
            }
        }
        
        /// <summary>
        /// Handles the CellClick event of the platform_DataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void platform_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                platform_DataGridView.Rows[e.RowIndex].Cells[0].ReadOnly = true;
                if (e.ColumnIndex >= 0 && platform_DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    platform_DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
                }
            }
        }

        /// <summary>
        /// Handles the RowValidating event of the platform_DataGridView control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void platform_DataGridView_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!platform_DataGridView.IsCurrentRowDirty)
            {
                return;
            }

            e.Cancel = !IsRowValid
            (
                platform_DataGridView.Rows[e.RowIndex].Cells[0].FormattedValue as string,
                platform_DataGridView.Rows[e.RowIndex].Cells[2].FormattedValue as string,
                platform_DataGridView.Rows.Count - 1 == e.RowIndex
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
        private bool IsRowValid(string platformId, string description, bool isRowLast)
        {
            bool isValid = true;

            if (!string.IsNullOrEmpty(platformId))
            {
                if (string.IsNullOrEmpty(description))
                {
                    DisplayValidationError("Description must have a value.  This value will be displayed when selecting a platform.");
                    isValid = false;
                }
            }
            else if (!isRowLast)
            {
                DisplayValidationError("Platform Id must have a value.");
                isValid = false;
            }
            return isValid;
        }

        private void DisplayValidationError(string message)
        {
            MessageBox.Show(this, message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Handles the UserDeletingRow event of the platform_DataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewRowCancelEventArgs"/> instance containing the event data.</param>
        private void platform_DataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // This will need a check to see if any virtual resources
            var platform = e.Row.DataBoundItem as FrameworkClientPlatform;
            e.Cancel = DeletePlatformEntry(platform);
        }

        /// <summary>
        /// Deletes the platform entry.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns></returns>
        private bool DeletePlatformEntry(FrameworkClientPlatform platform)
        {
            bool cancel = false;
            if (_enterpriseTestContext.VirtualResources.Any(f => f.Platform == platform.FrameworkClientPlatformId))
            {
                MessageBox.Show
                    (
                        this,
                        "You cannot delete this Platform Association as it is currently being referenced by Virtual Resource entries in the database.",
                        "Cannot Delete Platform Association",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop
                    );
                cancel = true;
            }
            else
            {
                _assetInventoryContext.FrameworkClientPlatforms.Remove(platform);
            }

            return cancel;
        }

        /// <summary>
        /// Handles the UserAddedRow event of the platform_DataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewRowEventArgs"/> instance containing the event data.</param>
        private void platform_DataGridView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            foreach (DataGridViewRow row in virtualMachine_DataGridView.Rows)
            {
                row.Cells[0].Value = false;
            }

            foreach (DataGridViewRow row in resourceType_DataGridView.Rows)
            {
                row.Cells[0].Value = false;
            }
        }

#endregion

        #region Virtual Machine DataGrid Handlers

        /// <summary>
        /// Populates the checkboxes.
        /// </summary>
        /// <param name="selectedPlatform">The selected platform.</param>
        private void PopulateCheckboxes(FrameworkClientPlatform selectedPlatform)
        {
            Cursor = Cursors.WaitCursor;
            if (selectedPlatform != null)
            {
                foreach (DataGridViewRow row in virtualMachine_DataGridView.Rows)
                {
                    bool check = ((VirtualMachineRow)row.DataBoundItem).Machine.Platforms.Any(n => n.FrameworkClientPlatformId == selectedPlatform.FrameworkClientPlatformId);
                    row.Cells[0].Value = check;
                }

                foreach (DataGridViewRow row in resourceType_DataGridView.Rows)
                {
                    bool check = ((ResourceTypeRow)row.DataBoundItem).ResourceType.FrameworkClientPlatforms.Any(n => n.FrameworkClientPlatformId == selectedPlatform.FrameworkClientPlatformId);
                    row.Cells[0].Value = check;
                }
            }
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Handles the CellClick event of the virtualMachine_DataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void resourceType_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    // The boolean value here is swapped, because the checked value of the control is 
                    // opposite of where it is heading.
                    bool isChecked = !(bool)resourceType_DataGridView.Rows[e.RowIndex].Cells["resourceTypeSelectedColumn"].Value;
                    var currentPlatform = platform_DataGridView.SelectedRows[0].DataBoundItem as FrameworkClientPlatform;
                    var resourceTypeRow = resourceType_DataGridView.Rows[e.RowIndex].DataBoundItem as ResourceTypeRow;

                    UpdateResourceTypeList(isChecked, currentPlatform, resourceTypeRow.ResourceType);
                }
            }
        }

        private void virtualMachine_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    // The boolean value here is swapped, because the checked value of the control is 
                    // opposite of where it is heading.
                    bool isChecked = !(bool)virtualMachine_DataGridView.Rows[e.RowIndex].Cells["virtualMachineSelectedColumn"].Value;
                    FrameworkClientPlatform currentPlatform = platform_DataGridView.SelectedRows[0].DataBoundItem as FrameworkClientPlatform;
                    VirtualMachineRow virtualMachineRow = virtualMachine_DataGridView.Rows[e.RowIndex].DataBoundItem as VirtualMachineRow;

                    UpdateVirtualMachineList(isChecked, currentPlatform, virtualMachineRow);

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
            var currentPlatform = platform_DataGridView.SelectedRows[0].DataBoundItem as FrameworkClientPlatform;
            foreach (DataGridViewRow row in virtualMachine_DataGridView.SelectedRows)
            {
                row.Cells[0].Value = isChecked;

                VirtualMachineRow virtualMachineRow = row.DataBoundItem as VirtualMachineRow;
                UpdateVirtualMachineList(isChecked, currentPlatform, virtualMachineRow);
            }
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Updates the resource type list.
        /// </summary>
        /// <param name="isChecked">if set to <c>true</c> the checkbox is checked.</param>
        /// <param name="currentPlatform">The current platform.</param>
        /// <param name="resourceType">Type of the resource.</param>
        private static void UpdateResourceTypeList(bool isChecked, FrameworkClientPlatform currentPlatform, ResourceType resourceType)
        {
            if (resourceType != null)
            {
                ResourceTypeFrameworkClientPlatformAssociation existingAssociation = resourceType.FrameworkClientPlatforms.FirstOrDefault(n => n.FrameworkClientPlatformId == currentPlatform.FrameworkClientPlatformId);
                if (isChecked && existingAssociation == null)
                {
                    resourceType.FrameworkClientPlatforms.Add(new ResourceTypeFrameworkClientPlatformAssociation { FrameworkClientPlatformId = currentPlatform.FrameworkClientPlatformId, ResourceTypeName = resourceType.Name });
                }
                else
                {
                    resourceType.FrameworkClientPlatforms.Remove(existingAssociation);
                }
            }
        }

        private void UpdateVirtualMachineList(bool isChecked, FrameworkClientPlatform currentPlatform, VirtualMachineRow virtualMachineRow)
        {
            FrameworkClient frameworkClient = _assetInventoryContext.FrameworkClients.Find(virtualMachineRow.Machine.Name);
            if (isChecked && !frameworkClient.Platforms.Contains(currentPlatform))
            {
                frameworkClient.Platforms.Add(currentPlatform);
            }
            else
            {
                frameworkClient.Platforms.Remove(currentPlatform);
            }
        }

        private void virtualMachine_DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (virtualMachine_DataGridView.IsCurrentCellDirty)
            {
                virtualMachine_DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
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
            foreach (FrameworkClientPlatform platform in _platforms)
            {
                if (_assetInventoryContext.FrameworkClientPlatforms.Find(platform.FrameworkClientPlatformId) == null)
                {
                    _assetInventoryContext.FrameworkClientPlatforms.Add(platform);
                }
            }

            _assetInventoryContext.SaveChanges();
            _enterpriseTestContext.SaveChanges();

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

        /// <summary>
        /// Handles the Click event of the deleteToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var row = platform_DataGridView.SelectedRows[0];
            var platform = row.DataBoundItem as FrameworkClientPlatform;
            bool cancel = DeletePlatformEntry(platform);
            if (!cancel)
            {
                platform_DataGridView.Rows.Remove(row);
            }
            foreach (DataGridViewRow virtualMachineRow in virtualMachine_DataGridView.Rows)
            {
                virtualMachineRow.Cells["virtualMachineSelectedColumn"].Value = false;
            }
            foreach (DataGridViewRow resourceTypeRow in resourceType_DataGridView.Rows)
            {
                resourceTypeRow.Cells["resourceTypeSelectedColumn"].Value = false;
            }
        }

        #endregion

        private class ResourceTypeRow
        {
            /// <summary>
            /// Helper class for displaying Resource Types in a grid.
            /// </summary>
            /// <param name="resourceType"></param>
            public ResourceTypeRow(ResourceType resourceType)
            {
                ResourceType = resourceType;
            }

            public ResourceType ResourceType { get; private set; }
            public bool Selected { get; set; }
            public string Name { get { return ResourceType.Name; } }
        }
    }
}
