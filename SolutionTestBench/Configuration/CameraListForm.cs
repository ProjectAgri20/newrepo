using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using Telerik.WinControls.UI;
using System.Collections.Generic;

namespace HP.ScalableTest
{
    /// <summary>
    /// List form showing STB Camera entries
    /// </summary>
    public partial class CameraListForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraListForm"/> class.
        /// </summary>
        public CameraListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(radGridViewCameras, GridViewStyle.ReadOnly);
            ShowIcon = true;
            radGridViewCameras.AutoGenerateColumns = false;
        }

        private void AssetListForm_Load(object sender, System.EventArgs e)
        {
            RefreshItems();
            radGridViewCameras.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridViewCameras.BestFitColumns();
        }

        private void RefreshItems()
        {
            int index = (radGridViewCameras.CurrentRow != null ? radGridViewCameras.CurrentRow.Index : 0);
            using (new BusyCursor())
            {
                SortableBindingList<Camera> cameras = new SortableBindingList<Camera>();
                foreach (Camera camera in GetCameras())
                {
                    cameras.Add(camera);
                }

                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = cameras;
                radGridViewCameras.DataSource = bindingSource;

                GridViewRowInfo foundRow = null;
                while (radGridViewCameras.RowCount > 0)
                {
                    foundRow = radGridViewCameras.Rows.FirstOrDefault(x => x.Index == index);
                    if (foundRow != null)
                    {
                        radGridViewCameras.CurrentRow = foundRow;
                        break;
                    }
                    index--;
                }
            }
        }

        private static List<Camera> GetCameras()
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                return context.Assets.Include(n => n.Reservations).OfType<Camera>().ToList();
            }
        }

        private void edit_Button_Click(object sender, EventArgs e)
        {
            if (radGridViewCameras.SelectedRows.Count == 1)
            {
                Camera selectedCamera = radGridViewCameras.SelectedRows[0].DataBoundItem as Camera;

                using (var bc = new BusyCursor())
                {
                    using (CameraEditForm form = new CameraEditForm(selectedCamera.AssetId))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            RefreshItems();
                        }
                    }
                }
            }
        }

        private void add_Button_Click(object sender, EventArgs e)
        {
            Camera newCamera = new Camera();
            newCamera.AssetType = newCamera.GetType().Name;
            using (CameraEditForm dialog = new CameraEditForm(newCamera))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshItems();
                }
            }
        }

        private void radGridViewCameras_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            GridViewRowInfo row = e.Row;
            if (row.IsCurrent && row.IsSelected)
            {
                edit_Button_Click(sender, EventArgs.Empty);
            }
        }

        private void remove_Button_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                Camera camera = row.DataBoundItem as Camera;
                DialogResult dialogResult = MessageBox.Show($"Removing Camera '{camera.AssetId.Trim()}'.  Do you want to continue?", "Delete Camera", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                    {
                        Asset asset = context.Assets.FirstOrDefault(n => n.AssetId == camera.AssetId);
                        context.Assets.Remove(asset);
                        context.SaveChanges();
                    }
                    RefreshItems();
                }
            }
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return radGridViewCameras.SelectedRows.FirstOrDefault();
        }

        private void reservationToolStripButton_Click(object sender, EventArgs e)
        {
            ManageReservations();
        }

        private void ManageReservations()
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var camera = row.DataBoundItem as Camera;

                using (var form = new AssetReservationListForm<Camera>(camera))
                {
                    form.ShowDialog();
                    RefreshItems();
                }
            }
        }

    }
}