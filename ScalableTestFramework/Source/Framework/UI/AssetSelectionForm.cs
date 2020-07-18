using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Form that displays a list of assets for selection.
    /// </summary>
    public partial class AssetSelectionForm : Form
    {
        private readonly BindingList<AssetInfo> _assetList = null;
        private readonly Collection<string> _selectedAssetIds = new Collection<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionForm" /> class.
        /// </summary>
        private AssetSelectionForm()
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(assets_GridView, GridViewStyle.ReadOnly);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionForm" /> class.
        /// </summary>
        /// <param name="allowMultipleSelection">if set to <c>true</c> allow multiple assets to be selected.</param>
        public AssetSelectionForm(bool allowMultipleSelection)
            : this()
        {
            assets_GridView.MultiSelect = allowMultipleSelection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionForm" /> class.
        /// </summary>
        /// <param name="assetAttributes">The attributes of the assets to be displayed.</param>
        /// <param name="allowMultipleSelection">if set to <c>true</c> allow multiple assets to be selected.</param>
        public AssetSelectionForm(AssetAttributes assetAttributes, bool allowMultipleSelection)
            : this(allowMultipleSelection)
        {
            AssetInfoCollection assetList = ConfigurationServices.AssetInventory.GetAssets(assetAttributes);
            _assetList = new BindingList<AssetInfo>(assetList.ToList());
            assets_GridView.DataSource = _assetList;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionForm" /> class.
        /// </summary>
        /// <param name="assetAttributes">The attributes of the assets to be displayed.</param>
        /// <param name="assetFilter">A function to apply that acts as an additional filter to the displayed assets.</param>
        /// <param name="allowMultipleSelection">if set to <c>true</c> allow multiple assets to be selected.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetFilter" /> is null.</exception>
        public AssetSelectionForm(AssetAttributes assetAttributes, Func<AssetInfoCollection, AssetInfoCollection> assetFilter, bool allowMultipleSelection)
            : this(allowMultipleSelection)
        {
            if (assetFilter == null)
            {
                throw new ArgumentNullException(nameof(assetFilter));
            }

            AssetInfoCollection assetList = ConfigurationServices.AssetInventory.GetAssets(assetAttributes);
            AssetInfoCollection filteredAssetList = assetFilter(assetList);
            _assetList = new BindingList<AssetInfo>(filteredAssetList.ToList());
            assets_GridView.DataSource = _assetList;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionForm" /> class
        /// with the specified asset ID selected.
        /// </summary>
        /// <param name="assetAttributes">The attributes of the assets to be displayed.</param>
        /// <param name="assetId">The asset ID of the asset to be initially selected.</param>
        /// <param name="allowMultipleSelection">if set to <c>true</c> allow multiple assets to be selected.</param>
        public AssetSelectionForm(AssetAttributes assetAttributes, string assetId, bool allowMultipleSelection)
            : this(assetAttributes, allowMultipleSelection)
        {
            if (!string.IsNullOrEmpty(assetId))
            {
                _selectedAssetIds.Add(assetId);
            }
        }

        /// <summary>
        /// Gets the assets selected in the form.
        /// </summary>
        public AssetInfoCollection SelectedAssets
        {
            get
            {
                Collection<AssetInfo> deviceCollection = new Collection<AssetInfo>();
                foreach (AssetInfo device in assets_GridView.SelectedRows.Select(x => x.DataBoundItem).Cast<AssetInfo>())
                {
                    deviceCollection.Add(device);
                }

                return new AssetInfoCollection(deviceCollection);
            }
        }

        private void AssetSelectionForm_Load(object sender, EventArgs e)
        {
            if (_selectedAssetIds.Count > 0 && assets_GridView.RowCount > 0)
            {
                // IsCurrent makes the first row look like it's selected.  Turn it off before setting the selection.
                assets_GridView.Rows[0].IsCurrent = false;

                bool scrollToFirst = true;
                foreach (string assetId in _selectedAssetIds)
                {
                    GridViewRowInfo row = assets_GridView.Rows.Where(r => (string)r.Cells[0].Value == assetId).FirstOrDefault();
                    if (row != null)
                    {
                        row.IsSelected = true;
                        if (scrollToFirst)
                        {
                            assets_GridView.TableElement.ScrollTo(row.Index, 0);
                            scrollToFirst = false;
                        }
                    }
                }
            }
            else if (assets_GridView.RowCount > 0)
            {
                assets_GridView.Rows.First().IsSelected = true;
            }
        }

        private void AssetSelectionForm_KeyDown(object sender, KeyEventArgs e)
        {
            // When enter is pressed, close the form rather than moving to the next item in the grid
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                DialogResult = DialogResult.OK;
            }
        }

        private void assets_GridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            // Make sure we clicked on a row and not one of the headers
            if (e.RowIndex > -1)
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
