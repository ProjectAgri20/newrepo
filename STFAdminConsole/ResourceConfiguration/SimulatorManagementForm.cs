using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.UI.SessionExecution;
using HP.ScalableTest.Virtualization;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.LabConsole.ResourceConfiguration
{
    public partial class SimulatorManagementForm : Form
    {
        private const int SelectedColumn = 0;
        private SimReservedList _masterList;

        public SimulatorManagementForm()
        {
            InitializeComponent();
            GetMasterList();
            BindSimMgtGrid(_masterList);
            BindCboReservedFor();
        }

        #region Form and Control events

        private void close_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BulkUpdateSimulatorCheckbox(true);
        }

        private void uncheckAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BulkUpdateSimulatorCheckbox(false);
        }

        private void simMgt_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 0)
            {
                DataGridViewCell cell = simMgt_Grid.Rows[e.RowIndex].Cells["selected_Column"];
                ((SimReserved)simMgt_Grid.Rows[e.RowIndex].DataBoundItem).IsSelected = !((bool)cell.Value);

            }
        }

        private void launch_ToolStripButton_Click(object sender, EventArgs e)
        {
            List<SimReserved> selected = GetSelectedRows();

            if (selected.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(LaunchSelected, selected);
            }
            else
            {
                MessageBox.Show("Please select one or more devices.", "Launch", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void powerOff_ToolStripButton_Click(object sender, EventArgs e)
        {
            List<SimReserved> selected = GetSelectedRows();

            if (selected.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(PowerOffSelected, selected);
            }
            else
            {
                MessageBox.Show("Please select one or more devices.", "Power Off", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void powerOn_ToolStripButton_Click(object sender, EventArgs e)
        {
            List<SimReserved> selected = GetSelectedRows();

            if (selected.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(PowerOnSelected, selected);
            }
            else
            {
                MessageBox.Show("Please select one or more devices.", "Power On", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void revert_ToolStripButton_Click(object sender, EventArgs e)
        {
            List<SimReserved> selected = GetSelectedRows();

            if (selected.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(RevertSelected, selected);
            }
            else
            {
                MessageBox.Show("Please select one or more devices.", "Revert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void selectAll_ToolStripButton_Click(object sender, EventArgs e)
        {
            SimMgtGridSelection(true);
        }

        private void deselectAll_ToolStripButton_Click(object sender, EventArgs e)
        {
            SimMgtGridSelection(false);
        }

        private void reservedFor_ToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string reservedFor = " all available reservations.";

            // Uncheck all items so that there's no hidden selected items
            BulkUpdateSimulatorCheckbox(false);

            if (reservedFor_ToolStripComboBox.SelectedIndex != 0)
            {
                reservedFor = reservedFor_ToolStripComboBox.SelectedItem.ToString();
                var matches = (from sr in _masterList
                               where sr.ReservedFor.Equals(reservedFor)
                               select sr);

                SimReservedList lst = new SimReservedList();
                foreach (SimReserved item in matches)
                {
                    lst.Add(item);
                }
                BindSimMgtGrid(lst);

            }
            else
            {
                BindSimMgtGrid(_masterList);
            }

            SetReservedForMessage(reservedFor);
        }

        #endregion

        #region Helper methods

        private void BindCboReservedFor()
        {
            reservedFor_ToolStripComboBox.Items.Clear();
            List<string> keys = new List<string> { "Select..." };
            keys.AddRange(ConfigurationServices.AssetInventory.GetAssets().Select(n => n.ReservationKey).Distinct().Where(n => !string.IsNullOrEmpty(n)));

            foreach (string item in keys)
            {
                reservedFor_ToolStripComboBox.Items.Add(item);
            }
            reservedFor_ToolStripComboBox.SelectedIndex = 0;
        }

        private void BindSimMgtGrid(SimReservedList listSimReserved)
        {
            simReservedListBindingSource.DataSource = listSimReserved;
            simReservedListBindingSource.ResetBindings(false);
        }

        private void BulkUpdateSimulatorCheckbox(bool isChecked)
        {
            Cursor = Cursors.WaitCursor;
            foreach (DataGridViewRow row in simMgt_Grid.SelectedRows)
            {
                row.Cells[SelectedColumn].Value = isChecked;
                ((SimReserved)row.DataBoundItem).IsSelected = isChecked;
            }
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Determines whether an operation can be performed on the selected sims.
        /// If no session IDs are involved, the operation can be performed without further checks.
        /// If more than one session ID is involved, the operation cannot be performed.
        /// If any of the selected sims are associated with a single session ID, we need to check user credentials.
        /// If the user is the owner of the session, or an admin, they will be allowed to reset the sims.
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        private bool CanReset(List<SimReserved> selected)
        {
            string sessionId = string.Empty;
            var sessionIds = (from s in selected
                              where s.SessionId.Length > 0
                              select s.SessionId).Distinct();

            switch (sessionIds.Count())
            {
                case 0:
                    return true;
                case 1:
                    sessionId = sessionIds.FirstOrDefault();
                    break;
                default:
                    MessageBox.Show("Unable to perform operations on more than one session at a time.  Please modify your selection.", "Session", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
            }

            // At least one sim is associated with a single session ID.
            SessionResetManager resetManager = new SessionResetManager();
            bool result = resetManager.IsAuthorized(sessionId);
            if (result)
            {
                resetManager.LogSessionReset(sessionId);
            }
            return result;

        }

        private void PowerOnSelected(object state)
        {
            event_StatusLabel.Text = "Powering On simulators...";

            List<SimReserved> selected = (List<SimReserved>)state;
            Parallel.ForEach<SimReserved>(selected, item => PowerOn(item));

            event_StatusLabel.Text = "Ready.";
        }

        private void PowerOn(SimReserved simulator)
        {
            Thread.CurrentThread.SetName(simulator.AssetId);

            UpdateItemStatus(simulator, "Checking power status...");
            if (VMController.IsPoweredOn(simulator.VirtualMachine))
            {
                UpdateItemStatus(simulator, "Already powered on.");
            }
            else
            {
                UpdateItemStatus(simulator, "Powering On VM...");
                VMController.PowerOnMachine(simulator.VirtualMachine);
                VMInventoryManager.SetInUse(simulator.VirtualMachine);
                CancellationTokenSource task = new CancellationTokenSource();
                VMController.WaitOnMachineAvailable(simulator.VirtualMachine, task.Token);
                UpdateItemStatus(simulator, "Power On complete.");
            }
        }

        private void PowerOffSelected(object state)
        {
            event_StatusLabel.Text = "Powering Off simulators...";

            List<SimReserved> selected = (List<SimReserved>)state;
            if (CanReset(selected))
            {
                Parallel.ForEach<SimReserved>(selected, item => PowerOff(item));
            }

            event_StatusLabel.Text = "Ready.";
        }

        private void PowerOff(SimReserved simulator)
        {
            Thread.CurrentThread.SetName(simulator.AssetId);

            UpdateItemStatus(simulator, "Checking power status...");
            if (VMController.IsPoweredOn(simulator.VirtualMachine))
            {
                if (JediSimulatorManager.IsSimulatorReady(simulator.HostAddress))
                {
                    UpdateItemStatus(simulator, "Shutting down simulator...");
                    JediSimulatorManager.ShutdownSimulator(simulator.VirtualMachine);
                    UpdateItemStatus(simulator, "Simulator Shutdown complete.");
                }

                UpdateItemStatus(simulator, "Powering Off VM...");
                VMController.Shutdown(simulator.VirtualMachine);
                UpdateItemStatus(simulator, "Power Off complete.");
            }
            else
            {
                UpdateItemStatus(simulator, "Already powered off.");
            }
        }

        private void LaunchSelected(object state)
        {
            event_StatusLabel.Text = "Launching simulators...";

            List<SimReserved> selected = (List<SimReserved>)state;
            Parallel.ForEach<SimReserved>(selected, item => Launch(item));

            event_StatusLabel.Text = "Ready.";
        }

        private void Launch(SimReserved simulator)
        {
            Thread.CurrentThread.SetName(simulator.AssetId);

            PowerOn(simulator);

            if (!JediSimulatorManager.IsSimulatorReady(simulator.HostAddress))
            {
                UpdateItemStatus(simulator, "Launching Simulator...");
                JediSimulatorManager.LaunchSimulator(simulator.VirtualMachine, simulator.Product, simulator.HostAddress);
                UpdateItemStatus(simulator, "Launch complete.");
            }
            else
            {
                UpdateItemStatus(simulator, "Simulator is already running.");
            }
        }

        private void RevertSelected(object state)
        {
            event_StatusLabel.Text = "Reverting simulators...";

            List<SimReserved> selected = (List<SimReserved>)state;
            if (CanReset(selected))
            {
                Parallel.ForEach<SimReserved>(selected, item => Revert(item));
            }

            event_StatusLabel.Text = "Ready.";
        }

        private void Revert(SimReserved simulator)
        {
            Thread.CurrentThread.SetName(simulator.AssetId);

            UpdateItemStatus(simulator, "Reverting...");
            VMController.Revert(simulator.VirtualMachine);
            UpdateItemStatus(simulator, "Revert complete.");
        }

        /// <summary>
        /// Retrieves the simulators listed in table Assets and if available what their reservations are.
        /// </summary>
        private void GetMasterList()
        {
            _masterList = new SimReservedList();

            using (AssetInventoryContext aie = DbConnect.AssetInventoryContext())
            {
                var matches = (from ds in aie.Assets.OfType<DeviceSimulator>()
                               join ar in aie.AssetReservations on ds.AssetId equals ar.AssetId into mgtGroup
                               from item in mgtGroup.DefaultIfEmpty()
                               select new
                               {
                                   ds.AssetId,
                                   ds.Product,
                                   ds.Address,
                                   ds.VirtualMachine,
                                   SessionId = item.SessionId == null ? "" : item.SessionId,
                                   ReservedFor = item.ReservedFor == null ? "" : item.ReservedFor
                               });

                foreach (var m in matches)
                {
                    SimReserved ent = new SimReserved();
                    ent.AssetId = m.AssetId;
                    ent.Product = m.Product;
                    ent.HostAddress = m.Address;
                    ent.VirtualMachine = m.VirtualMachine;
                    ent.SessionId = m.SessionId;
                    ent.ReservedFor = m.ReservedFor;

                    _masterList.Add(ent);
                }
            }
        }

        private List<SimReserved> GetSelectedRows()
        {
            simMgt_Grid.EndEdit();

            return (from DataGridViewRow row in simMgt_Grid.Rows
                    where ((bool)row.Cells["selected_Column"].Value) == true
                    select (SimReserved)row.DataBoundItem).ToList();

        }

        private void SetReservedForMessage(string reservedFor)
        {
            if (!string.IsNullOrWhiteSpace(reservedFor))
            {
                event_StatusLabel.Text = "Simulator Management reserved for " + reservedFor;
            }
            else
            {
                event_StatusLabel.Text = "Non associated devices";
            }
        }

        private void SimMgtGridSelection(bool select)
        {
            foreach (DataGridViewRow row in simMgt_Grid.Rows)
            {
                row.Cells["selected_Column"].Value = select;

                ((SimReserved)row.DataBoundItem).IsSelected = select;
            }
        }

        private void UpdateItemStatus(SimReserved item, string message)
        {
            item.Status = message;
            RefreshGrid(null, EventArgs.Empty);
        }

        private void RefreshGrid(object sender, EventArgs e)
        {
            if (simMgt_Grid.InvokeRequired)
            {
                simMgt_Grid.Invoke(new EventHandler(RefreshGrid), e);
            }
            else
            {
                simMgt_Grid.Refresh();
            }
        }

        #endregion

    }
    #region Grid class and list for maintain assets and reservations
    internal class SimReserved
    {
        public string AssetId { get; set; }
        public string Product { get; set; }
        public string VirtualMachine { get; set; }
        public string ReservedFor { get; set; }
        public string SessionId { get; set; }
        public string HostAddress { get; set; }
        public string Status { get; set; }

        public bool IsSelected { get; set; }

        public SimReserved()
        {
            IsSelected = false;
            AssetId = string.Empty;
            Product = string.Empty;
            VirtualMachine = string.Empty;
            SessionId = string.Empty;
            ReservedFor = string.Empty;
            HostAddress = string.Empty;
        }
    }

    internal class SimReservedList : List<SimReserved>
    {
    }
    #endregion

}
