using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.UI.Framework;
using System;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
	/// <summary>
	/// User control for swapping out devices used within a scenario
	/// </summary>
	public partial class BulkDeviceControl : UserControl
	{
		private BulkDeviceList _bulkDeviceList;
		private string _selectedAssetId = string.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="BulkDeviceControl"/> class.
		/// </summary>
		/// <param name="bulkDeviceList">The bulk device list.</param>
		public BulkDeviceControl(BulkDeviceList bulkDeviceList)
		{
			_bulkDeviceList = bulkDeviceList;
			InitializeComponent();
			BindBulkDeviceGrid();
		}
		/// <summary>
		/// Gets a value indicating whether [bulk device list change].
		/// </summary>
		/// <value>
		/// <c>true</c> if [bulk device list change]; otherwise, <c>false</c>.
		/// </value>
		public bool BulkDeviceListChange
		{
			get
			{
				bool changed = false;
				foreach(BulkDeviceEnt ent in _bulkDeviceList)
				{
					if(ent.DeviceChanged)
					{
						changed = true;
						break;
					}
				}
				return changed;
			}
		}
		/// <summary>
		/// Binds the bulk device grid.
		/// </summary>
		private void BindBulkDeviceGrid()
		{
			this.bulkDeviceListBindingSource.DataSource = _bulkDeviceList;
			this.bulkDeviceListBindingSource.ResetBindings(true);
		}

		/// <summary>
		/// Handles the CommandCellClick event of the RadGridView button column control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void MasterTemplate_CommandCellClick(object sender, EventArgs e)
		{ 
			GetNewResource();
		}

		/// <summary>
		/// Shows the Asset selection form. If a device is selected will start the process for adding it to the grid.
		/// </summary>
		private void GetNewResource()
		{
			string newResourceId = string.Empty;

            using (AssetSelectionForm form = new AssetSelectionForm(AssetAttributes.Printer & AssetAttributes.Scanner | AssetAttributes.ControlPanel))
			{
				form.MultiSelect = false;
				form.ShowDialog(this);
				if (form.DialogResult == DialogResult.OK)
				{
					newResourceId = form.SelectedAssetIds.FirstOrDefault();
					BulkDeviceUpdate(newResourceId);
				}
			}
		}

		/// <summary>
		/// Updates the bulk device list and grid
		/// </summary>
		/// <param name="newResourceId">The new resource identifier.</param>
		private void BulkDeviceUpdate(string newResourceId)
		{
			BulkDeviceEnt bulkDeviceSelected = BulkDeviceRadGridView.CurrentRow.DataBoundItem as BulkDeviceEnt;

			bulkDeviceSelected.DeviceChanged = true;
			bulkDeviceSelected.NewDevice = newResourceId;
			BindBulkDeviceGrid();
		}
    }
}
