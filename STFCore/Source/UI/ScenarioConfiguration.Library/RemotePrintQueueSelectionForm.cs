using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    public partial class RemotePrintQueueSelectionForm : Form
    {

        private readonly BindingList<RemotePrintQueueInfo> _printQueueList = new BindingList<RemotePrintQueueInfo>();
        private readonly Collection<string> _selectedPrintQueueIds = new Collection<string>();

        /// <summary>
        /// Creates an instance of Remote Print Queue Selection, which allows you to select queues for various functions.
        /// </summary>
        public RemotePrintQueueSelectionForm()
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(this, ScalableTest.Framework.UI.FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(printQueue_GridView, ScalableTest.Framework.UI.GridViewStyle.ReadOnly);

        }

        /// <summary>
        /// Sets the list values in the print queue selection form.
        /// </summary>
        /// <param name="allowMultipleSelection"></param>
        public RemotePrintQueueSelectionForm(bool allowMultipleSelection)
        : this(){
            printQueue_GridView.MultiSelect = allowMultipleSelection;
            var remoteQueues = ConfigurationServices.AssetInventory.GetRemotePrintQueues().ToList();

            foreach (RemotePrintQueueInfo remote in remoteQueues.Where(x => x.GetType() == typeof(RemotePrintQueueInfo)))
            {
                var queue = remoteQueues.FirstOrDefault(n => n.PrintQueueId == remote.PrintQueueId);
                if (queue != null)
                {
                    _printQueueList.Add(queue);
                }
            }
            //printQueue_GridView.Rows.
            printQueue_GridView.DataSource = _printQueueList;


        }

        /// <summary>
        /// Collection of selected print queues as  a print queue info collection.
        /// </summary>
        
        public PrintQueueInfoCollection SelectedPrintQueues
        {
            get {
                Collection<PrintQueueInfo> queueCollection = new Collection<PrintQueueInfo>();
                foreach (var queue in printQueue_GridView.SelectedRows.Select(x => x.DataBoundItem).Cast<PrintQueueInfo>())
                {
                    queueCollection.Add(queue);
                }
                return new PrintQueueInfoCollection(queueCollection);
            }
        }


        private void RemotePrintQueueSelectionForm_Load(object sender, EventArgs e)
        {

            if (_selectedPrintQueueIds.Count > 0 && printQueue_GridView.RowCount > 0)
            {
                printQueue_GridView.Rows[0].IsCurrent = false;

                bool scrollToFirst = true;
                foreach (string queueId in _selectedPrintQueueIds)
                {
                    GridViewRowInfo row = printQueue_GridView.Rows.Where(r => (string)r.Cells[0].Value == queueId).FirstOrDefault();
                    if (row != null)
                    {
                        row.IsSelected = true;
                        if (scrollToFirst)
                        {
                            printQueue_GridView.TableElement.ScrollTo(row.Index, 0);
                            scrollToFirst = false;
                        }
                    }
                }
            }
            else if (printQueue_GridView.RowCount > 0)
            {
                printQueue_GridView.Rows.First().IsSelected = true;
            }
        }

        private void RemotePrintQueueSelectionForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                DialogResult = DialogResult.OK;
            }
        }

        private void printQueue_GridView_CellDoubleClick(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DialogResult = DialogResult.OK;
            }
        }



    }
}
