using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI;
using HP.ScalableTest.UI.SessionExecution;
using HP.ScalableTest.Virtualization;
using Telerik.WinControls.UI;
using System.Threading;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.LabConsole
{
    public partial class VMInventoryForm : Form
    {
        private class GridColumns
        {
            internal const string NAME_GRID_COLUMN = "name_GridViewColumn";
            internal const string ENVIRONMENT_GRID_COLUMN = "environment_GridViewColumn";
            internal const string SESSIONID_GRID_COLUMN = "sessionId_GridViewColumn";
            internal const string HOLDID_GRID_COLUMN = "holdId_GridViewColumn";
            internal const string USAGESTATE_GRID_COLUMN = "usageState_GridViewColumn";
        }

        private SortableBindingList<VirtualMachineData> _machineData = new SortableBindingList<VirtualMachineData>();

        private class VirtualMachineData
        {
            public string HoldId { get; set;}                    
            public DateTime? LastUpdated {get; set; }
            public string Name { get; set; }
            public string PlatformUsage {get; set; }
            public string PowerState { get; set; }
            public string SessionId { get; set; }
            public int SortOrder { get; set; }
            public string UsageState { get; set; }
            public string Owner { get; set; }
            public string Status { get; set; }
            public DateTime? StartDate { get; set; }
            public string Environment { get; set; }
        }

        private class SessionReleaseInfo
        {
            public string Environment { get; set; }
            public string SessionId { get; set; }

            public GridViewRowInfo Row { get; set; }
        }


        /// <summary>
        /// UI for viewing and managing VM Inventory.
        /// </summary>
        public VMInventoryForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(vm_GridView, GridViewStyle.ReadOnly);
            
            var env = GlobalSettings.Items[Setting.Environment];
            Text += " for {0}".FormatWith(env);
            releaseSession_ToolStripButton.Text += " ({0} only)".FormatWith(env);
            releaseSession_ToolStripMenuItem.Text += " ({0} only)".FormatWith(env);

            SessionClient.Instance.ClearSessionRequestReceived += controller_SessionResetComplete;
        }

        private void VMInventoryForm_Load(object sender, EventArgs e)
        {
            synchronizeToolStripButton.Visible = UserManager.CurrentUser.HasPrivilege(UserRole.Administrator);

            RefreshGrid();
            STFDispatcherManager.DispatcherChanged += StfDispatcherManager_DispatcherChanged;
        }

        private void StfDispatcherManager_DispatcherChanged(object sender, EventArgs e)
        {
            SessionClient.Instance.ClearSessionRequestReceived -= controller_SessionResetComplete;
            SessionClient.Instance.ClearSessionRequestReceived += controller_SessionResetComplete;
        }

        private static object GetProperty(object target, string name)
        {
            return target.GetType().GetProperty(name).GetValue(target, null);
        }

        private void RefreshGrid()
        {
            if (vm_GridView.InvokeRequired) //Make sure we're on the UI thread
            {
                vm_GridView.Invoke(new MethodInvoker(this.RefreshGrid));
            }
            else
            {
                lock (this)
                {
                    try
                    {
                        // Store the current location so we can restore it
                        int currentLocation = vm_GridView.SelectedRows.Any() ? vm_GridView.SelectedRows[0].Index : 0;

                        vm_GridView.DataSource = null;

                        _machineData.Clear();
                        foreach (var item in VirtualMachine.SelectVirtualMachineSessionInfo())
                        {
                            VirtualMachineData data = new VirtualMachineData()
                            {
                                HoldId = GetProperty(item, "HoldId"),
                                Name = GetProperty(item, "Name"),
                                LastUpdated = GetProperty(item, "LastUpdated"),
                                Owner = GetProperty(item, "Owner"),
                                PlatformUsage = GetProperty(item, "PlatformUsage"),
                                PowerState = GetProperty(item, "PowerState"),
                                SessionId = GetProperty(item, "SessionId"),
                                SortOrder = GetProperty(item, "SortOrder"),
                                StartDate = GetProperty(item, "StartDate"),
                                Status = GetProperty(item, "Status"),
                                UsageState = GetProperty(item, "UsageState"),
                                Environment = GetProperty(item, "Environment")
                            };

                            _machineData.Add(data);
                        }

                        vm_GridView.DataSource = _machineData;

                        Task.Factory.StartNew(UpdateRowColor);

                        //Restore the current location
                        if (vm_GridView.Rows.Any())
                        {
                            if (vm_GridView.Rows.Count <= currentLocation)
                            {
                                currentLocation = vm_GridView.Rows.Count - 1;
                            }

                            vm_GridView.TableElement.ScrollToRow(currentLocation);
                            vm_GridView.Rows[currentLocation].IsCurrent = true;
                        }
                    }
                    catch //Intentionally left blank.  We don't want any exceptions crashing the process.
                    {
                    }
                }
            }
        }

        private void UpdateRowColor()
        {
            while (vm_GridView.ChildRows.Count < _machineData.Count)
            {
                Thread.Sleep(100);
            }

            var env = GlobalSettings.Items[Setting.Environment];

            foreach (GridViewRowInfo row in vm_GridView.Rows)
            {
                var cell = row.Cells[GridColumns.ENVIRONMENT_GRID_COLUMN];
                var color = (cell?.Value?.ToString().Equals(env) == true) ? Color.DarkBlue : Color.Black;

                foreach (GridViewCellInfo item in row.Cells)
                {
                    if (item.HasStyle && item.Style != null)
                    {
                        item.Style.ForeColor = color;
                    }
                }
            }
        }

        private void reset_Button_Click(object sender, EventArgs e)
        {
        }

        private IEnumerable<string> GetSelectedHostNames()
        {
            return vm_GridView.SelectedRows.Select(n => n.Cells[GridColumns.NAME_GRID_COLUMN].Value.ToString());
        }


        private void vm_GridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableSessionReleaseMenu();
        }

        private void mainContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EnableSessionReleaseMenu();
            EnableHoldsMenu();
        }

        private void holdToolStripDropDownButton_DropDownOpening(object sender, EventArgs e)
        {            
            EnableHoldsMenu();
        }

        private void EnableSessionReleaseMenu()
        {
            var releaseableSessionsSelected = ReleaseableSessionsSelected();
            releaseSession_ToolStripMenuItem.Enabled = releaseableSessionsSelected;
            releaseSession_ToolStripButton.Enabled = releaseableSessionsSelected;
        }

        private bool ReleaseableSessionsSelected()
        {
            // Determine if all selected rows contain releaseable sessions
            if (vm_GridView.SelectedRows.Count == 0)
            {
                return false;
            }
            else
            {
                // Get all the SessionId values currently selected in the grid by pulling them out
                // of the appropriate column
                var validSessions = GetReleasableSessions(vm_GridView.SelectedRows);
                return validSessions.Any();
            }
        }

        private IEnumerable<SessionReleaseInfo> GetReleasableSessions(IEnumerable<GridViewRowInfo> rows)
        {
           return         
            (from r in rows
             select new SessionReleaseInfo
             {
                 SessionId = (r.Cells[GridColumns.SESSIONID_GRID_COLUMN].Value ?? string.Empty).ToString(),
                 Environment = (r.Cells[GridColumns.ENVIRONMENT_GRID_COLUMN].Value ?? string.Empty).ToString(),
                 Row = r
             }
                )
                .Where(x =>
                        !string.IsNullOrEmpty(x.SessionId)
                        && !string.IsNullOrEmpty(x.Environment)
                        && x.Environment.Equals(GlobalSettings.Environment.ToString())
                    );
        }

        private IEnumerable<SessionReleaseInfo> GetSessionReleaseInfoBySession(string inSession)
        {
            return GetReleasableSessions(vm_GridView.Rows).Where(x => x.SessionId.EqualsIgnoreCase(inSession));
        }
        
        private void releaseSession_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReleaseSessions();
        }

        private void releaseSession_ToolStripButton_Click(object sender, EventArgs e)
        {
            ReleaseSessions();
        }

        private void ReleaseSessions()
        {
            // Get the session Ids from the selected rows.
            var releaseableSessions = GetReleasableSessions(vm_GridView.SelectedRows);
            var sessionIds = releaseableSessions.Select(x => x.SessionId).Distinct().ToList();

            string msg = "This will abort the selected sessions (if they are currently running) and release all machines and other assets used in this test.  Are you sure you want to release the following Sessions?" 
                + Environment.NewLine + string.Join(Environment.NewLine, sessionIds.ToArray());
            var result = MessageBox.Show(msg, "Release Session Resources", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            if (STFDispatcherManager.Dispatcher == null && STFDispatcherManager.ConnectToDispatcher() == false)
            {
                //The user canceled the connect dialog
                return;
            }
                
            SessionResetManager resetManager = new SessionResetManager();
            ReleaseSession(resetManager, sessionIds);
        }

        private void ReleaseSession(SessionResetManager resetManager, IEnumerable<string> inSessions)
        {
            if (inSessions.Count() > 0)
            {
                bool released = false;
                foreach (var sessionId in inSessions)
                {
                    if (!string.IsNullOrEmpty(sessionId) && resetManager.IsAuthorized(sessionId))
                    {
                        // Indicate which session (and rows) are currently being released
                        SetControls(true, sessionId);
                        // Get all rows in the grid that have this session id entered.
                        resetManager.LogSessionReset(sessionId);
                        SessionClient.Instance.SetUserCredential(UserManager.CurrentUser);
                        SessionClient.Instance.Close(sessionId);
                        released = true;

                        statusLabel.InvokeIfRequired(c =>
                        {
                            statusLabel.Text = "Releasing session {0} and cleaning up associated virtual machines and resources...".FormatWith(sessionId);
                        });

                        Task.Factory.StartNew(() =>
                        {
                            // return session (and rows) to unlocked state for grid display after set time regardless
                            Thread.Sleep(5000);
                            SetControls(false, sessionId);
                        });
                    }
                }

                if (released)
                {
                    Task.Factory.StartNew(() =>
                    {
                        // return session (and rows) to unlocked state for grid display after set time regardless
                        Thread.Sleep(5000);
                        SetControls(false);
                        RefreshGrid();
                    });
                }
            }
        }        

        private void SetControls(bool lockDown, string sessionId = null)
        {
            this.InvokeIfRequired(c =>
                {
                    IEnumerable<GridViewRowInfo> sessionRows = null;
                    if (string.IsNullOrEmpty(sessionId))
                    {
                        sessionRows = vm_GridView.Rows;
                    }
                    else
                    {
                        sessionRows = GetSessionReleaseInfoBySession(sessionId).Select(x => x.Row);
                    }

                    if (sessionRows != null && sessionRows.Any())
                    {
                        foreach (GridViewRowInfo row in sessionRows)
                        {
                            foreach (GridViewCellInfo cell in row.Cells)
                            {
                                if (cell != null && cell.Style != null)
                                {
                                    cell.Style.ForeColor = lockDown ? Color.Red : Color.Black;
                                }
                            }
                        }
                    }
                    statusLabel.Text = string.Empty;
                }
            );
        }


        private void controller_SessionResetComplete(object sender, Framework.Dispatcher.SessionIdEventArgs e)
        {
            //Enable the button if this form made the reset call.
            if (!string.IsNullOrEmpty(e.SessionId))
            {
                RefreshGrid();
                SetControls(false, e.SessionId);
                TraceFactory.Logger.Info("Session {0} was released.".FormatWith(e.SessionId));
            }
        }

        private void EnableHoldsMenu()
        {
            if (vm_GridView.SelectedRows.Count == 0)
            {
                removeHoldToolStripMenuItem.Enabled = false;
                removeHoldToolStripMenuItemTop.Enabled = false;
            }
            else
            {
                var holdsExist = vm_GridView.SelectedRows.Select(x => x.Cells[GridColumns.HOLDID_GRID_COLUMN].Value).Cast<string>().Any(x => !string.IsNullOrEmpty(x));
                removeHoldToolStripMenuItem.Enabled = holdsExist;
                removeHoldToolStripMenuItemTop.Enabled = holdsExist;
            }
        }

        private void removeHoldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vm_GridView.SelectedRows.Count == 0)
            {
                return;
            }

            var selectedRows = vm_GridView.SelectedRows;
            var nonEmptyItems = selectedRows.Select(x => x.Cells[GridColumns.HOLDID_GRID_COLUMN].Value).Cast<string>().Where(x => !string.IsNullOrEmpty(x));

            if (!nonEmptyItems.All(x => x.Equals(nonEmptyItems.First())))
            {
                var result = MessageBox.Show("You have selected to remove Holds with different Hold IDs.  Do you really mean to remove all of them?", "Removing Multiple Hold IDs", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    RemoveHolds(selectedRows);
                }
            }
            else
            {
                RemoveHolds(selectedRows);
            }
        }

        private void RemoveHolds(GridViewSelectedRowsCollection selectedRows)
        {
            var hostNames = selectedRows.Select(n => n.Cells[GridColumns.NAME_GRID_COLUMN].Value.ToString());
            try
            {
                this.Cursor = Cursors.WaitCursor;
                VMInventoryManager.RemoveHold(hostNames);

                foreach (var row in selectedRows)
                {
                    row.Cells[GridColumns.HOLDID_GRID_COLUMN].Value = null;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void AddHolds(GridViewSelectedRowsCollection selectedRows)
        {
            string holdId = InputDialog.Show("Enter Hold Id:", "Add Machine Hold");
            if (string.IsNullOrWhiteSpace(holdId))
            {
                // User cancelled
                return;
            }

            var hostNames = selectedRows.Select(n => n.Cells[GridColumns.NAME_GRID_COLUMN].Value.ToString());
            try
            {
                this.Cursor = Cursors.WaitCursor;
                VMInventoryManager.AddHold(hostNames, holdId);

                foreach (var row in selectedRows)
                {
                    row.Cells[GridColumns.HOLDID_GRID_COLUMN].Value = holdId;                    
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void addHoldIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vm_GridView.SelectedRows.Count == 0)
            {
                return;
            }

            var selectedRows = vm_GridView.SelectedRows;
            var nonEmptyItems = selectedRows.Select(x => x.Cells[GridColumns.HOLDID_GRID_COLUMN].Value).Cast<string>().Where(x => !string.IsNullOrEmpty(x));

            if (nonEmptyItems.Count() > 0)
            {
                var result = MessageBox.Show("You selected row(s) with existing Holds. Do you want those Hold IDs updated?", "Updating Existing Holds", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    AddHolds(selectedRows);
                }
            }
            else
            {
                AddHolds(selectedRows);
            }
        }

        private void availableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUsage(VMUsageState.Available);
        }

        private void reservedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUsage(VMUsageState.Reserved);
        }

        private void inUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUsage(VMUsageState.InUse);
        }

        private void doNotScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUsage(VMUsageState.DoNotSchedule);
        }

        private void SetUsage(VMUsageState state)
        {
            if (vm_GridView.SelectedRows.Count == 0)
            {
                return;
            }

            var selectedRows = vm_GridView.SelectedRows;
            var usages = selectedRows.Select(x => x.Cells[GridColumns.USAGESTATE_GRID_COLUMN].Value).Cast<string>();

            if (!usages.All(x => x.Equals(usages.First())))
            {
                var result = MessageBox.Show("You have selected to change usage state on machines that currently have different states.  Do you really want to change all of them?", "Set Machine State", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            SetUsage(selectedRows, state);
        }

        private void SetUsage(GridViewSelectedRowsCollection selectedRows, VMUsageState state)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var hostNames = selectedRows.Select(n => n.Cells[GridColumns.NAME_GRID_COLUMN].Value.ToString());
                VMInventoryManager.SetUsageState(hostNames, state);

                RefreshGrid();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void synchronizeToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Execute this operation on a separate thread so that the Lock Service callback doesn't create a deadlock with the UI thread.
                Task syncTask = Task.Factory.StartNew(() => VMInventoryManager.SyncInventory(UserManager.CurrentUser));
                syncTask.Wait();
                RefreshGrid();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void refresh_ToolStripButton_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }
    }
}
