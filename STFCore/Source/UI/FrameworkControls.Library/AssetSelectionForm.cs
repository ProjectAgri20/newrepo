using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.Framework
{
    /// <summary>
    /// Form that displays a list of assets for selection.
    /// </summary>
    public partial class AssetSelectionForm : Form
    {
        private BindingList<AssetInfo> _assetList = new BindingList<AssetInfo>();
        private IEnumerable<string> _selectedAssetIds = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionForm"/> class.
        /// </summary>
        public AssetSelectionForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(assets_GridView, GridViewStyle.ReadOnly);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionForm"/> class.
        /// </summary>
        /// <param name="capabilities">The capabilities.</param>
        public AssetSelectionForm(AssetAttributes capabilities)
            : this()
        {
            // Get the pools from SystemSettings
            string poolString = GlobalSettings.Items[Setting.AssetInventoryPools];
            List<string> pools = poolString.Split(',').ToList();

            AssetInventoryConfiguration configuration = new AssetInventoryConfiguration();
            pools.ForEach(n => configuration.AssetPools.Add(n));

            AssetInventoryController controller = new AssetInventoryController(DbConnect.AssetInventoryConnectionString, configuration);
            foreach (AssetInfo asset in controller.GetAssets(capabilities))
            {
                _assetList.Add(asset);
            }
            assets_GridView.DataSource = _assetList;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionForm"/> class,
        /// with the specified asset id selected.
        /// </summary>
        /// <param name="capabilities">The capabilities.</param>
        /// <param name="assetId">The asset id.</param>
        public AssetSelectionForm(AssetAttributes capabilities, string assetId)
            : this(capabilities)
        {
            if (! string.IsNullOrEmpty(assetId))
            {
                _selectedAssetIds = new string[] { assetId };
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Select button is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the select button is visible; otherwise, <c>false</c>.
        /// </value>
        public bool SelectButtonVisible
        {
            get { return select_Button.Visible; }
            set { select_Button.Visible = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allow selection of multiple assets.
        /// </summary>
        /// <value>
        ///   <c>true</c> if MultiSelect is set; otherwise, <c>false</c>.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi",
            Justification = "Matches underlying GridView property name, and can't suppress this by adding 'Multi' to the custom CA dictionary.")]
        public bool MultiSelect
        {
            get { return assets_GridView.MultiSelect; }
            set { assets_GridView.MultiSelect = value; }
        }

        /// <summary>
        /// Gets the selected asset descriptions.
        /// </summary>
        public Collection<AssetInfo> SelectedAssets
        {
            get
            {
                Collection<AssetInfo> items = new Collection<AssetInfo>();
                foreach (GridViewRowInfo row in assets_GridView.SelectedRows)
                {
                    AssetInfo description = row.DataBoundItem as AssetInfo;
                    if (description != null)
                    {
                        items.Add(description);
                    }
                }
                return items;
            }
        }

        /// <summary>
        /// Gets the selected asset ids.
        /// </summary>
        public IEnumerable<string> SelectedAssetIds
        {
            get { return SelectedAssets.Select(n => n.AssetId); }
            set 
            {
                _selectedAssetIds = value;
                SetSelectedAssets();
            }
        }

        private void AssetSelectionForm_Load(object sender, EventArgs e)
        {
            // If there is an initial selection of asset Ids, set the selection
            SetSelectedAssets();
        }

        private void SetSelectedAssets()
        {
            if (_selectedAssetIds != null && assets_GridView.RowCount > 0)
            {
                //IsCurrent makes the first row look like it's selected.  Turn it off before setting the selection.
                assets_GridView.Rows[0].IsCurrent = false;

                bool scrollToFirst = true;
                foreach (string assetId in _selectedAssetIds)
                {
                    GridViewRowInfo row = assets_GridView.Rows.Where(r => (string)r.Cells[0].Value == assetId).FirstOrDefault();
                    if (row != null)
                    {
                        row.IsSelected = (row != null);
                        if (scrollToFirst)
                        {
                            assets_GridView.TableElement.ScrollTo(row.Index, 0);
                            scrollToFirst = false;
                        }
                    }
                }
            }
            else if (assets_GridView.Rows.Count > 0)
            {
                assets_GridView.Rows.First().IsSelected = true;
            }
        }

        /// <summary>
        /// Forces form to display only printers with the specified inventoryIds.
        /// </summary>
        /// <param name="assetIds">The asset ids.</param>
        public void FilterByIds(IEnumerable<string> assetIds)
        {
            SortableBindingList<AssetInfo> filteredAssets = new SortableBindingList<AssetInfo>();
            foreach (AssetInfo asset in _assetList)
            {
                if (assetIds.Contains(asset.AssetId))
                {
                    filteredAssets.Add(asset);
                }
            }
            assets_GridView.DataSource = filteredAssets;
        }

        private void AssetSelectionForm_KeyDown(object sender, KeyEventArgs e)
        {
            // When enter is pressed, close the form rather than moving to the next item in the grid
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void select_Button_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void assets_GridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            // Make sure we clicked on a row and not one of the headers
            if (e.RowIndex > -1)
            {
                select_Button.PerformClick();
            }
        }

        private void usage_Button_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                if (assets_GridView.SelectedRows.Count > 0)
                {
                    Collection<AssetInfo> items = new Collection<AssetInfo>();

                    foreach (GridViewRowInfo item in assets_GridView.SelectedRows)
                    {
                        items.Add(item.DataBoundItem as AssetInfo);
                    }

                    using (AssetUsageForm form = new AssetUsageForm(items))
                    {
                        form.ShowDialog();
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
