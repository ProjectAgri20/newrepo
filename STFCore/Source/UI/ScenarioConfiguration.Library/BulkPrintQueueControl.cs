using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Framework;
using System;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// User control for swapping out print queues used within a scenario
    /// </summary>
    public partial class BulkPrintQueueControl : UserControl
    {
        private BulkPrintQueueList _bulkPrintQueueList;
        private string _selectedQueueId = string.Empty;
        /// <summary>
        /// Initializes a new instance of the <see cref="BulkPrintQueueControl"/>class
        /// </summary>
        /// <param name="bulkPrintQueueList"></param>
        public BulkPrintQueueControl(BulkPrintQueueList bulkPrintQueueList)
        {
            _bulkPrintQueueList = bulkPrintQueueList;
            InitializeComponent();
            BindBulkPrintQueueGrid();
        }

        /// <summary>
        /// Gets a value indicating whether PrintQueues changed
        /// </summary>
        public bool BulkPrintQueueListChange
        {
            get
            {
                bool changed = false;
                foreach (BulkPrintQueueEnt ent in _bulkPrintQueueList)
                {
                    if (ent.QueueChanged)
                    {
                        changed = true;
                        break;
                    }
                }
                return changed;
            }
        }

        private void BindBulkPrintQueueGrid()
        {
            bulkPrintQueueListBindingSource.DataSource = _bulkPrintQueueList;
            bulkPrintQueueListBindingSource.ResetBindings(true);
        }

        private void MasterTemplate_CommandCellClick(object sender, EventArgs e)
        {
            GetNewResource();
        }

        private void GetNewResource()
        {
            string newResourceId = string.Empty;
            string serverName = string.Empty;
            //using (AssetSelectionForm form = new AssetSelectionForm(DeviceCapabilities.Print))
            using (RemotePrintQueueSelectionForm form = new RemotePrintQueueSelectionForm(false))
            {
                //form.MultiSelect = false;
                form.ShowDialog(this);
                if (form.DialogResult == DialogResult.OK)
                {
                    var temp = form.SelectedPrintQueues.FirstOrDefault();
                    RemotePrintQueueInfo info = ((RemotePrintQueueInfo)temp);
                    //resourceId = form.SelectedPrintQueues.FirstOrDefault().QueueName;
                    //serverName = form.SelectedPrintQueues.FirstOrDefault();
                    newResourceId = info.QueueName;
                    serverName = info.ServerHostName;


                    //newResourceId = form.SelectedPrintQueues.FirstOrDefault().QueueName;
                    BulkPrintQueueUpdate(newResourceId, serverName);
                }
            }
        }

        private void BulkPrintQueueUpdate(string newRourceId, string serverName)
        {
            BulkPrintQueueEnt bulkPrintQueueSelected = BulkPrintQueueRadView.CurrentRow.DataBoundItem as BulkPrintQueueEnt;

            bulkPrintQueueSelected.QueueChanged = true;
            bulkPrintQueueSelected.NewQueue = newRourceId;
            bulkPrintQueueSelected.NewHostName = serverName;
            BindBulkPrintQueueGrid();
        }

        private void EditBlank_Button_Click(object sender, EventArgs e)
        {
            string resourceId = string.Empty;
            string serverName = string.Empty;

            using (RemotePrintQueueSelectionForm form = new RemotePrintQueueSelectionForm(false))
            {
                form.ShowDialog(this);
                if (form.DialogResult == DialogResult.OK)
                {

                    var temp = form.SelectedPrintQueues.FirstOrDefault();
                    RemotePrintQueueInfo info = ((RemotePrintQueueInfo)temp);
                    //resourceId = form.SelectedPrintQueues.FirstOrDefault().QueueName;
                    //serverName = form.SelectedPrintQueues.FirstOrDefault();
                    resourceId = info.QueueName;
                    serverName = info.ServerHostName;
                    UpdateBlankPrintQueues(resourceId, serverName);
                }
                form.Refresh();
            }
        }

        private void UpdateBlankPrintQueues(string resourceId, string serverName)
        {
            BulkPrintQueueRadView.Rows.AddNew();
            BulkPrintQueueEnt bulkPrintQueueRow = BulkPrintQueueRadView.Rows.Last().DataBoundItem as BulkPrintQueueEnt;
            bulkPrintQueueRow.Active = true;
            bulkPrintQueueRow.NewHostName = serverName;
            bulkPrintQueueRow.CurrentQueue = "N/A";
            bulkPrintQueueRow.QueueChanged = true;
            bulkPrintQueueRow.NewQueue = resourceId;
            bulkPrintQueueRow.VirtualResourceMetadataId = new Guid("00000000000000000000000000000000");
            BulkPrintQueueRadView.Focus();
            BulkPrintQueueRadView.Refresh();

        }


    }
}
