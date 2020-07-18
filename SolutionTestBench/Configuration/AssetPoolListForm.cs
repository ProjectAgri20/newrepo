using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI;
using Telerik.WinControls.UI;

namespace HP.ScalableTest
{
    /// <summary>
    /// List form showing STB Asset Pools
    /// </summary>
    public partial class AssetPoolListForm : Form
    {
        private SortableBindingList<AssetPool> _assetPools = null;
        private AssetInventoryContext _context = null;
        private Collection<AssetPool> _deletedItems = new Collection<AssetPool>();
        private BindingSource _bindingSource = null;
        bool _unsavedChanges = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetPoolListForm"/>
        /// </summary>
        public AssetPoolListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(assetPool_RadGridView, GridViewStyle.ReadOnly);
            ShowIcon = true;


            assetPool_RadGridView.AutoGenerateColumns = false;
            assetPool_RadGridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            assetPool_RadGridView.BestFitColumns();

            _context = DbConnect.AssetInventoryContext();
            _assetPools = new SortableBindingList<AssetPool>();
        }

        private void AssetPoolList_Load(object sender, EventArgs e)
        {
            using (new BusyCursor())
            {
                foreach (var item in _context.AssetPools)
                {
                    _assetPools.Add(item);
                }

                _bindingSource = new BindingSource();
                _bindingSource.DataSource = _assetPools;
                assetPool_RadGridView.DataSource = _bindingSource;
            }
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return assetPool_RadGridView.SelectedRows.FirstOrDefault();
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (base.ValidateChildren())
            {
                Commit();

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private DialogResult EditEntry(AssetPool pool)
        {
            DialogResult result = DialogResult.None;
            using (AssetPoolEditForm form = new AssetPoolEditForm(pool, _context))
            {
                result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _bindingSource.ResetBindings(false);
                    _unsavedChanges = true;
                }

            }

            return result;
        }

        private void Commit()
        {
            using (new BusyCursor())
            {
                try
                {
                    foreach (var item in _deletedItems)
                    {
                        _context.AssetPools.Remove(item);
                    }

                    _context.SaveChanges();
                    _deletedItems.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving changes. Check log for more detail. Error: {0}".FormatWith(ex.Message), "Commit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TraceFactory.Logger.Error(ex);
                }
            }
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (_unsavedChanges)
            {
                var result = MessageBox.Show("You have unsaved changes that will be lost. Do you want to continue?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }
            Close();
            return;
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            Commit();
            _unsavedChanges = false;
        }

        private void add_Button_Click(object sender, EventArgs e)
        {
            var pool = new AssetPool();

            if (EditEntry(pool) == DialogResult.OK)
            {
                AddPool(pool);
            }
        }

        private void AddPool(AssetPool pool)
        {
            if (!_context.AssetPools.Any(x => x.Name.Equals(pool.Name, StringComparison.OrdinalIgnoreCase)))
            {
                _context.AssetPools.Add(pool);
                _assetPools.Add(pool);
                int index = assetPool_RadGridView.Rows.Count - 1;

                assetPool_RadGridView.Rows[index].IsSelected = true;

                assetPool_RadGridView.TableElement.ScrollToRow(index);
            }
            else
            {
                MessageBox.Show("An asset pool with the name '{0}' already exists in the database.".FormatWith(pool.Name), "Unable to Add", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }


        private void edit_Button_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var pool = row.DataBoundItem as AssetPool;

                if (EditEntry(pool) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    assetPool_RadGridView.Refresh();
                }
            }
        }

        private void remove_Button_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var pool = row.DataBoundItem as AssetPool;

                var dialogResult = MessageBox.Show
                    (
                    "Removing Asset Pool {0}. Do you want to continue?".FormatWith(pool.Name),
                    "Delete Asset Pool",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                    );

                if (dialogResult == DialogResult.Yes)
                {
                    _unsavedChanges = true;
                    _deletedItems.Add(pool);
                    _assetPools.Remove(pool);
                }
            }
        }

        private void assetPool_RadGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var pool = row.DataBoundItem as AssetPool;

                if (EditEntry(pool) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    assetPool_RadGridView.Refresh();
                }
            }
        }
    }
}
