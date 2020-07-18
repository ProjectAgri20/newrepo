using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.ImportExport;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public partial class ImportResolutionControl : UserControl
    {

        public event EventHandler OnAllItemsResolved;

        public ImportResolutionControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            resolveDataGridView.AutoGenerateColumns = false;
        }

        void resolveMenuItem_Click(object sender, EventArgs e)
        {
            ResolveEntry();
        }

        public void LoadContract(EnterpriseScenarioContract contract)
        {
        }

        private void resolveDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = resolveDataGridView.Rows[e.RowIndex];

            if (e.ColumnIndex == 0)
            {
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
