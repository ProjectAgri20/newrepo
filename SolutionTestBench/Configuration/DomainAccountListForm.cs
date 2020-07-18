using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using Telerik.WinControls.UI;

namespace HP.ScalableTest
{
    /// <summary>
    /// List form showing STF Server entries
    /// </summary>
    public partial class DomainAccountListForm : Form
    {
        private SortableBindingList<DomainAccountPool> _userPools = null;
        private AssetInventoryContext _context = null;
        private Collection<DomainAccountPool> _deletedItems = new Collection<DomainAccountPool>();
        private BindingSource _bindingSource = null;
        bool _unsavedChanges = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainAccountListForm"/> class.
        /// </summary>
        public DomainAccountListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(domainAccount_RadGridView, GridViewStyle.ReadOnly);
            ShowIcon = true;

            domainAccount_RadGridView.AutoGenerateColumns = false;
            domainAccount_RadGridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            domainAccount_RadGridView.BestFitColumns();

            _context = DbConnect.AssetInventoryContext();
            _userPools = new SortableBindingList<DomainAccountPool>();
        }

        private void DomainAccountList_Load(object sender, System.EventArgs e)
        {
            using (new BusyCursor())
            {
                foreach (var item in _context.DomainAccountPools)
                {
                    _userPools.Add(item);
                }

                _bindingSource = new BindingSource();
                _bindingSource.DataSource = _userPools;
                domainAccount_RadGridView.DataSource = _bindingSource;
            }
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return domainAccount_RadGridView.SelectedRows.FirstOrDefault();
        }

        private void ok_Button_Click(object sender, System.EventArgs e)
        {
            if (base.ValidateChildren())
            {
                Commit();

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        private void Commit()
        {
            using (new BusyCursor())
            {
                try
                {
                    foreach (var item in _deletedItems)
                    {
                        _context.DomainAccountPools.Remove(item);
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

        private DialogResult EditEntry(DomainAccountPool pool)
        {
            DialogResult result = DialogResult.None;

            using (DomainAccountEditForm form = new DomainAccountEditForm(pool, _context))
            {
                result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _bindingSource.ResetBindings(false);
                    //SetDefault();
                    _unsavedChanges = true;
                }
            }

            return result;
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            if (_unsavedChanges)
            {
                var result = MessageBox.Show("You have unsaved changes that will be lost.  Do you want to continue?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            Close();
            return;
        }

        private void edit_Button_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var pool = row.DataBoundItem as DomainAccountPool;

                if (EditEntry(pool) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    domainAccount_RadGridView.Refresh();
                }
            }
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            Commit();
            _unsavedChanges = false;
        }

        private void add_Button_Click(object sender, EventArgs e)
        {
            var pool = new DomainAccountPool();

            if (EditEntry(pool) == DialogResult.OK)
            {
                AddPool(pool);
            }
        }

        private void AddPool(DomainAccountPool pool)
        {
            if (!_context.DomainAccountPools.Any(x => x.DomainAccountKey.Equals(pool.DomainAccountKey, StringComparison.OrdinalIgnoreCase)))
            {
                _context.DomainAccountPools.Add(pool);
                _userPools.Add(pool);

                int index = domainAccount_RadGridView.Rows.Count - 1;

                domainAccount_RadGridView.Rows[index].IsSelected = true;

                //In case if you want to scroll down as well.
                domainAccount_RadGridView.TableElement.ScrollToRow(index);
            }
            else
            {
                MessageBox.Show("A user pool with the name '{0}' already exists in the database.".FormatWith(pool.DomainAccountKey), "Unable to Add", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }        

        private void remove_Button_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var pool = row.DataBoundItem as DomainAccountPool;

                var dialogResult = MessageBox.Show
                    (
                        "Removing Account Pool {0}. Do you want to continue?".FormatWith(pool.DomainAccountKey),
                        "Delete Account Pool",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question

                    );

                if (dialogResult == DialogResult.Yes)
                {
                    _unsavedChanges = true;
                    _deletedItems.Add(pool);
                    _userPools.Remove(pool);
                }
            }
        }

        private void domainAccount_RadGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var pool = row.DataBoundItem as DomainAccountPool;

                if (EditEntry(pool) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    domainAccount_RadGridView.Refresh();
                }
            }
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
                var pool = row.DataBoundItem as DomainAccountPool;

                using (var form = new DomainAccountReservationListForm(pool))
                {
                    form.ShowDialog();
                }
            }
        }
    }
}
