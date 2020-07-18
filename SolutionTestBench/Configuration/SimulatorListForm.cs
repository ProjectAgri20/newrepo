using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework.UI;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Form displaying list of Device Simulator entries.
    /// </summary>
    public partial class SimulatorListForm : Form
    {
        private SortableBindingList<DeviceSimulator> _simulators = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorListForm"/>.
        /// </summary>
        public SimulatorListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            ShowIcon = true;

            simulator_DataGridView.AutoGenerateColumns = false;
            _simulators = new SortableBindingList<DeviceSimulator>();
        }

        private void SimulatorListForm_Load(object sender, System.EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            using (new BusyCursor())
            {
                Collection<DeviceSimulator> simulators = GetSimulators();

                _simulators.Clear();
                foreach (DeviceSimulator item in simulators)
                {
                    _simulators.Add(item);
                }

                simulator_DataGridView.DataSource = _simulators;
            }
        }

        private static Collection<DeviceSimulator> GetSimulators()
        {
            Collection<DeviceSimulator> result = new Collection<DeviceSimulator>();

            // retain only the pools that match AssetInventoryPools SystemSetting
            string[] assetPools = GetAssetPoolsFromSettings();

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                if (assetPools.Count() == 0 || assetPools.Any(x => x.Equals("DEFAULT", StringComparison.OrdinalIgnoreCase)))
                {
                    assetPools = context.AssetPools.Select(x => x.Name).ToArray();
                }

                if (assetPools.Any())
                {
                    foreach (DeviceSimulator simulator in context.Assets.Include(n => n.Reservations).OfType<DeviceSimulator>().Where(n => assetPools.Contains(n.Pool.Name)))
                    {
                        result.Add(simulator);
                    }
                }
            }
            return result;
        }

        private static string[] GetAssetPoolsFromSettings()
        {
            return (GlobalSettings.Items != null && GlobalSettings.Items.ContainsKey(Setting.AssetInventoryPools) ? GlobalSettings.Items[Setting.AssetInventoryPools] : string.Empty)
                .Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        private void toolStripButton_New_Click(object sender, EventArgs e)
        {
            //Create a new Simulator
            DeviceSimulator simulator = new DeviceSimulator()
            {
                AssetId = string.Empty,
                AssetType = "DeviceSimulator",
                Capability = 0, //Default to no capabilities selected
                Address = string.Empty,
                VirtualMachine = string.Empty,
                FirmwareVersion = string.Empty,
                SimulatorType = string.Empty
            };

            //Display the new Simulator for editing.
            using (SimulatorEditForm form = new SimulatorEditForm(simulator))
            {
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    RefreshData();
                }
            }
        }

        private void toolStripButton_Edit_Click(object sender, EventArgs e)
        {
            if (simulator_DataGridView.SelectedRows.Count == 1)
            {
                DeviceSimulator simulator = simulator_DataGridView.SelectedRows[0].DataBoundItem as DeviceSimulator;

                //Display the Simulator for editing.
                using (SimulatorEditForm form = new SimulatorEditForm(simulator.AssetId))
                {
                    DialogResult result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        RefreshData();
                    }
                }
            }
        }

        private void toolStripButton_Remove_Click(object sender, EventArgs e)
        {
            if (simulator_DataGridView.SelectedRows.Count == 1)
            {
                DeviceSimulator simulator = simulator_DataGridView.SelectedRows[0].DataBoundItem as DeviceSimulator;

                var dialogResult = MessageBox.Show
                    (
                        $"Removing Device Simulator '{simulator.AssetId}'.  Do you want to continue?",
                        "Delete Device Simulator",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question

                    );

                if (dialogResult == DialogResult.Yes)
                {
                    using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                    {
                        Asset asset = context.Assets.FirstOrDefault(n => n.AssetId == simulator.AssetId);
                        context.Assets.Remove(asset);
                        context.SaveChanges();
                    }
                    RefreshData();
                }
            }
        }

        private void toolStripButton_Reservation_Click(object sender, EventArgs e)
        {
            if (simulator_DataGridView.SelectedRows.Count == 1)
            {
                DeviceSimulator simulator = simulator_DataGridView.SelectedRows[0].DataBoundItem as DeviceSimulator;

                using (var form = new AssetReservationListForm<DeviceSimulator>(simulator))
                {
                    form.ShowDialog();
                }
            }
        }

        private void simulator_DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                toolStripButton_Edit_Click(sender, EventArgs.Empty);
            }
        }
    }
}
