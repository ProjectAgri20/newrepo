using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public partial class ImportDocumentControl : UserControl
    {
        private SortableBindingList<TestDocumentContract> _data = null;
        private TestDocumentResolverForm _form = null;

        /// <summary>
        /// Fires when all unresolved documents have been resolved by the user.
        /// </summary>
        public event EventHandler OnAllItemsResolved;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportDocumentControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            resolveDataGridView.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Loads the TestDocumentContract list from the specified EnterpriseScenarioContract.
        /// </summary>
        /// <param name="contract"></param>
        public void LoadContract(EnterpriseScenarioContract contract)
        {
            _data = new SortableBindingList<TestDocumentContract>(contract.TestDocuments.Where(x => x.ResolutionRequired).ToList());

            resolveDataGridView.DataSource = null;
            resolveDataGridView.DataSource = _data;
        }

        private void resolveDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = resolveDataGridView.Rows[e.RowIndex];

            if (e.ColumnIndex == 0)
            {
                TestDocumentContract data = row.DataBoundItem as TestDocumentContract;

                if (data != null && data.Resolved)
                {
                    e.Value = resolvedImageList.Images["Resolved"];
                }
                else
                {
                    e.Value = resolvedImageList.Images["NotResolved"];
                }

                e.FormattingApplied = true;
            }
        }

        private void resolveDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ResolveEntry();
        }

        private void ResolveEntry()
        {
            if (resolveDataGridView.SelectedRows.Count == 1)
            {
                var contract = resolveDataGridView.SelectedRows[0].DataBoundItem as TestDocumentContract;
                if (_form == null)
                {
                    _form = new TestDocumentResolverForm();
                }

                _form.Initialize(contract);

                if (_form.ShowDialog() == DialogResult.OK)
                {
                    contract.Resolved = true;

                    if (_data.All(x => x.Resolved))
                    {
                        OnAllItemsResolved?.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        private void resolveToolStripButton_Click(object sender, EventArgs e)
        {
            ResolveEntry();
        }

        private void resolveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResolveEntry();
        }
    }
}
