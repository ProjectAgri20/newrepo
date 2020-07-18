using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public partial class ImportDevicePoolControl : UserControl
    {
        private AssetContractCollection<PrinterContract> _printers = null;

        public event EventHandler OnAllItemsResolved;

        public ImportDevicePoolControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            resolveDataGridView.AutoGenerateColumns = false;

            resolveDataGridView.CellValueChanged += ResolveDataGridView_CellValueChanged;
            resolveDataGridView.CurrentCellDirtyStateChanged += ResolveDataGridView_CurrentCellDirtyStateChanged;
        }

        private void ResolveDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (resolveDataGridView.IsCurrentCellDirty)
            {
                if (resolveDataGridView.CurrentCell.EditedFormattedValue != null)
                {
                    resolveDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }

        private void ResolveDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var printer = resolveDataGridView.Rows[e.RowIndex].DataBoundItem as PrinterContract;
            if (printer != null)
            {
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    var entity = context.Assets.FirstOrDefault(x => x.AssetId.Equals(printer.AssetId));
                    if (entity == null)
                    {
                        context.Assets.Add(ContractFactory.Create(printer, context));
                    }
                    else
                    {
                        entity.Pool = context.AssetPools.FirstOrDefault(n => n.Name == printer.PoolName);
                    }

                    context.SaveChanges();
                }
            }

            CheckItemsResolved();
            resolveDataGridView.Invalidate();
        }

        public void Reset()
        {
            _printers = null;
            resolveDataGridView.DataSource = null;
        }

        public void LoadPrinters(AssetContractCollection<PrinterContract> printers)
        {
            _printers = printers;

            bool changesMade = false;

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (var printer in _printers)
                {
                    if (!context.Assets.Any(x => x.Pool.Name.Equals(printer.PoolName)))
                    {
                        if (context.AssetPools.Count() > 0)
                        {
                            printer.PoolName = context.AssetPools.First().Name;
                            if (!context.Assets.Any(x => x.AssetId.Equals(printer.AssetId)))
                            {
                                context.Assets.Add(ContractFactory.Create(printer, context));
                                changesMade = true;
                            }
                        }
                    }
                }

                if (changesMade)
                {
                    context.SaveChanges();
                }
            }

            resolveDataGridView.DataSource = _printers;

            CheckItemsResolved();
        }

        private void CheckItemsResolved()
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                // Determine if all printers in this list have been added
                // to the database.  If so, send the event to indicate that
                // It's complete.
                var values = _printers.Select(x => x.AssetId);
                var source = context.Assets.Select(x => x.AssetId);
                if (values.All(y => source.Contains(y)))
                {
                    if (OnAllItemsResolved != null)
                    {
                        OnAllItemsResolved(this, new EventArgs());
                    }
                }
            }
        }

        private void ImportDevicePoolControl_Load(object sender, EventArgs e)
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (var pool in context.AssetPools.OrderBy(x => x.Name))
                {
                    poolNameColumn.Items.Add(pool.Name);
                }
            }
        }

        private void resolveDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }
    }
}
