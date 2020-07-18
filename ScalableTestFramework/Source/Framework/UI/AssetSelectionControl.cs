using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Control for selecting a list of devices.
    /// </summary>
    public partial class AssetSelectionControl : UserControl
    {
        private readonly BindingList<AssetSelectionRow> _assetRows = new BindingList<AssetSelectionRow>();
        private AssetAttributes _assetAttributes;

        /// <summary>
        /// Occurs when this control's selection is changed.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when the selection of the control changes.")]
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionControl" /> class.
        /// </summary>
        public AssetSelectionControl()
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(asset_GridView, GridViewStyle.Display);
            asset_GridView.DataSource = _assetRows;
        }

        /// <summary>
        /// Gets the <see cref="Assets.AssetSelectionData" /> from this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AssetSelectionData AssetSelectionData
        {
            get
            {
                var selected = _assetRows.Where(n => n.Enabled).Select(n => n.Asset);
                var inactive = _assetRows.Where(n => !n.Enabled).Select(n => n.Asset);
                return new AssetSelectionData(selected, inactive);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has selected (and active) assets.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasSelection
        {
            get
            {
                asset_GridView.EndEdit();
                return _assetRows.Any(n => n.Enabled);
            }
        }

        /// <summary>
        /// Initializes the asset selection control for selecting assets with the specified attributes.
        /// </summary>
        /// <param name="attributes">The attributes of assets that can be selected.</param>
        public void Initialize(AssetAttributes attributes)
        {
            _assetAttributes = attributes;
            OnDisplayedAssetsChanged();
        }

        /// <summary>
        /// Initializes the asset selection control for selecting assets with the specified attributes
        /// and adds the specified assets to the grid.
        /// </summary>
        /// <param name="data">The asset selection data.</param>
        /// <param name="attributes">The attributes of assets that can be selected.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data" /> is null.</exception>
        public void Initialize(AssetSelectionData data, AssetAttributes attributes)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _assetAttributes = attributes;
            _assetRows.Clear();

            var allAssetIds = data.SelectedAssets.Union(data.InactiveAssets);
            AssetInfoCollection assets = ConfigurationServices.AssetInventory.GetAssets(allAssetIds);
            foreach (AssetInfo asset in assets)
            {
                _assetRows.Add(new AssetSelectionRow(asset, data.SelectedAssets.Contains(asset.AssetId)));
            }

            asset_GridView.Refresh();
            OnDisplayedAssetsChanged();
        }

        private void add_ToolStripButton_Click(object sender, EventArgs e)
        {
            bool changeMade = false;

            using (AssetSelectionForm form = new AssetSelectionForm(_assetAttributes, ApplySelectionFilter, true))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (AssetInfo item in form.SelectedAssets)
                    {
                        if (!_assetRows.Any(n => n.AssetId == item.AssetId))
                        {
                            _assetRows.Add(new AssetSelectionRow(item));
                            changeMade = true;
                        }
                    }
                }
            }

            asset_GridView.Refresh();

            if (changeMade)
            {
                OnSelectionChanged();
                OnDisplayedAssetsChanged();
            }
        }

        private void remove_ToolStripButton_Click(object sender, EventArgs e)
        {
            var selectedRows = asset_GridView.SelectedRows.Select(n => n.DataBoundItem).Cast<AssetSelectionRow>().ToList();
            foreach (AssetSelectionRow row in selectedRows)
            {
                _assetRows.Remove(row);
            }

            OnSelectionChanged();
            OnDisplayedAssetsChanged();
        }

        private void enableAll_ToolStripButton_Click(object sender, EventArgs e)
        {
            bool newState = enableAll_ToolStripButton.Text == "Enable All";

            foreach (AssetSelectionRow row in _assetRows)
            {
                row.Enabled = newState;
            }

            ConfigureEnableAllButton();
            OnSelectionChanged();
        }

        private void ConfigureEnableAllButton()
        {
            if (_assetRows.All(n => n.Enabled))
            {
                enableAll_ToolStripButton.Image = enableImages.Images["Disable"];
                enableAll_ToolStripButton.Text = "Disable All";
            }
            else
            {
                enableAll_ToolStripButton.Image = enableImages.Images["Enable"];
                enableAll_ToolStripButton.Text = "Enable All";
            }
        }

        private void quickEntry_ToolStripButton_Click(object sender, EventArgs e)
        {
            bool changeMade = false;

            var currentAssetIds = _assetRows.Select(n => n.AssetId);
            using (AssetQuickEntryForm form = new AssetQuickEntryForm(currentAssetIds))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    // Determine which assets were added and which were removed
                    List<string> addedAssetIds = form.EnteredAssetIds.Where(n => !currentAssetIds.Contains(n)).ToList();
                    List<string> removedAssetIds = currentAssetIds.Where(n => !form.EnteredAssetIds.Contains(n)).ToList();
                    changeMade = addedAssetIds.Any() || removedAssetIds.Any();

                    // Retrieve asset information for all added assets that meet the attribute requirements
                    AssetInfoCollection assetsWithAttribute = ConfigurationServices.AssetInventory.GetAssets(_assetAttributes);
                    AssetInfoCollection filteredAssets = ApplySelectionFilter(assetsWithAttribute);
                    AssetInfoCollection requestedAssets = ConfigurationServices.AssetInventory.GetAssets(addedAssetIds);
                    var allowedAssets = requestedAssets.Where(n => filteredAssets.Any(m => m.AssetId == n.AssetId));

                    // Add new assets and remove old ones
                    allowedAssets.ToList().ForEach(n => _assetRows.Add(new AssetSelectionRow(n)));
                    _assetRows.Where(n => removedAssetIds.Contains(n.AssetId)).ToList().ForEach(n => _assetRows.Remove(n));

                    // Show a dialog box if there are any that could not be added
                    var notAdded = addedAssetIds.Where(n => !_assetRows.Any(m => m.AssetId == n));
                    if (notAdded.Any())
                    {
                        MessageBox.Show($"The following asset(s) could not be added, either because they were not found " +
                                        $"or do not have the capabilities required for this activity:{Environment.NewLine}" +
                                         string.Join(Environment.NewLine, notAdded),
                                        "Devices Not Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            asset_GridView.Refresh();
            if (changeMade)
            {
                OnSelectionChanged();
                OnDisplayedAssetsChanged();
            }
        }

        private void OnSelectionChanged()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnDisplayedAssetsChanged()
        {
            AssetInfoCollection displayedAssets = new AssetInfoCollection(_assetRows.Select(n => n.Asset).ToList());
            OnDisplayedAssetsChanged(displayedAssets);
        }

        /// <summary>
        /// Called when there is a change in the displayed assets in this control.
        /// Can be overridden by inheriting classes to customize behavior.
        /// </summary>
        /// <param name="displayedAssets">The displayed assets.</param>
        protected virtual void OnDisplayedAssetsChanged(AssetInfoCollection displayedAssets)
        {
            // Nothing to do here; provided for child classes to override if desired
        }

        /// <summary>
        /// Applies additional filtering to assets that can be selected using this control.
        /// Can be overridden by inheriting classes to customize behavior.
        /// </summary>
        /// <param name="selectableAssets">The assets selectable based on the standard filter behavior.</param>
        /// <returns>The filtered list of assets after custom modifications are applied.</returns>
        protected virtual AssetInfoCollection ApplySelectionFilter(AssetInfoCollection selectableAssets)
        {
            // By default, perform no extra filtering.
            return selectableAssets;
        }

        private sealed class AssetSelectionRow : INotifyPropertyChanged
        {
            private bool _enabled;

            public event PropertyChangedEventHandler PropertyChanged;
            public AssetInfo Asset { get; }

            public AssetSelectionRow(AssetInfo info, bool enabled = true)
            {
                Asset = info;
                _enabled = enabled;
            }

            public bool Enabled
            {
                get
                {
                    return _enabled;
                }
                set
                {
                    _enabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Enabled"));
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used in data grid binding.")]
            public string AssetId => Asset.AssetId;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used in data grid binding.")]
            public string AssetType => Asset.AssetType;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used in data grid binding.")]
            public string Description => Asset.Description;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used in data grid binding.")]
            public string ReservationKey => Asset.ReservationKey;
        }
    }
}

