using System;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.LabConsole
{
    public partial class PrintServerUsageDetails : Form
    {
        private string _serverName = string.Empty;
        private SortableBindingList<PrintQueueInUse> _queuesInUse = null;

        public PrintServerUsageDetails(string serverName, SortableBindingList<PrintQueueInUse> queuesInUse)
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(queues_DataGridView, GridViewStyle.ReadOnly);

            _serverName = serverName;
            _queuesInUse = queuesInUse;
        }

        private void PrintServerUsageDetails_Load(object sender, EventArgs e)
        {
            queues_DataGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            queues_DataGridView.MasterTemplate.BestFitColumns();

            queues_DataGridView.DataSource = _queuesInUse;
        }

        private void close_Button_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
