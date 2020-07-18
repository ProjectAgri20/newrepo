using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// List form showing all STB Mobile Device entries
    /// </summary>
    public partial class MobileDeviceListForm : Form
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MobileDeviceListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(radGrid_Devices, GridViewStyle.ReadOnly);
            ShowIcon = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RefreshItems();
            radGrid_Devices.BestFitColumns();
        }

        private void RefreshItems()
        {
            int index = (radGrid_Devices.CurrentRow != null ? radGrid_Devices.CurrentRow.Index : 0);
            using (new BusyCursor())
            {
                SortableBindingList<MobileDevice> mobileDevices = new SortableBindingList<MobileDevice>();

                foreach (MobileDevice device in GetMobileDevices())
                {
                    mobileDevices.Add(device);
                }

                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = mobileDevices;
                radGrid_Devices.DataSource = bindingSource;

                GridViewRowInfo foundRow = null;
                while (radGrid_Devices.RowCount > 0)
                {
                    foundRow = radGrid_Devices.Rows.FirstOrDefault(x => x.Index == index);
                    if (foundRow != null)
                    {
                        radGrid_Devices.CurrentRow = foundRow;
                        break;
                    }
                    index--;
                }
            }
        }

        private static List<MobileDevice> GetMobileDevices()
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                return context.Assets.Include(n => n.Reservations).OfType<MobileDevice>().ToList();
            }
        }

        private void new_ToolButton_Click(object sender, EventArgs e)
        {
            MobileDevice newDevice = new MobileDevice();
            using (MobileDeviceEditForm dialog = new MobileDeviceEditForm(newDevice))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshItems();
                }
            }
        }

        private void edit_ToolButton_Click(object sender, EventArgs e)
        {
            if (radGrid_Devices.SelectedRows.Count == 1)
            {
                MobileDevice selectedDevice = GetFirstSelectedDataItem();

                using (BusyCursor bc = new BusyCursor())
                {
                    using (MobileDeviceEditForm form = new MobileDeviceEditForm(selectedDevice.AssetId))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            RefreshItems();
                        }
                    }
                }
            }
        }

        private void remove_ToolButton_Click(object sender, EventArgs e)
        {
            MobileDevice mobileDevice = GetFirstSelectedDataItem();
            if (mobileDevice != null)
            {
                DialogResult dialogResult = MessageBox.Show($"Removing Mobile Device '{mobileDevice.AssetId.Trim()}'.  Do you want to continue?", "Delete Mobile Device", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                    {
                        Asset asset = context.Assets.FirstOrDefault(n => n.AssetId == mobileDevice.AssetId);
                        context.Assets.Remove(asset);
                        context.SaveChanges();
                    }
                    RefreshItems();
                }
            }
        }

        private MobileDevice GetFirstSelectedDataItem()
        {
            return radGrid_Devices.SelectedRows.FirstOrDefault()?.DataBoundItem as MobileDevice;
        }

        private void radGrid_Devices_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            GridViewRowInfo row = e.Row;
            if (row.IsCurrent && row.IsSelected)
            {
                edit_ToolButton_Click(sender, EventArgs.Empty);
            }
        }

        private void reservations_ToolButton_Click(object sender, EventArgs e)
        {
            MobileDevice mobileDevice = GetFirstSelectedDataItem();
            if (mobileDevice != null)
            {
                using (var form = new AssetReservationListForm<MobileDevice>(mobileDevice))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        RefreshItems();
                    }
                }
            }
        }
    }
}
