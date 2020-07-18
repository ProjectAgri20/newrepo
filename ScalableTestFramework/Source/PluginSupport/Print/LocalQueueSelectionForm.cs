using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.PluginSupport.Print
{
    public partial class LocalQueueSelectionForm : Form
    {
        private readonly List<PrintQueueInfo> _printQueues = new List<PrintQueueInfo>();
        private readonly BindingList<LocalPrintQueueRow> _printQueueRows = new BindingList<LocalPrintQueueRow>();

        public IEnumerable<PrintQueueInfo> SelectedQueues
        {
            get { return _printQueues; }
        }

        private LocalQueueSelectionForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(printQueue_GridView, GridViewStyle.ReadOnly);
        }

        public LocalQueueSelectionForm(IEnumerable<PrintQueueInfo> queues)
            : this()
        {
            _printQueues.AddRange(queues);

            foreach (LocalPrintQueueInfo localQueue in queues.OfType<LocalPrintQueueInfo>())
            {
                _printQueueRows.Add(new LocalPrintQueueRow(localQueue));
            }

            foreach (DynamicLocalPrintQueueInfo dynamicQueue in queues.OfType<DynamicLocalPrintQueueInfo>())
            {
                _printQueueRows.Add(new LocalPrintQueueRow(dynamicQueue));
            }

            printQueue_GridView.DataSource = _printQueueRows;
        }

        /// <summary>
        /// Gets or sets visibility of Add Dynamic Print Queue button
        /// </summary>
        public bool DynamicQueueEnabled
        {
            get { return addDynamic_ToolStripButton.Visible; }
            set { addDynamic_ToolStripButton.Visible = value; }
        }

        private void addExisting_ToolStripButton_Click(object sender, EventArgs e)
        {
            using (ExistingQueueForm form = new ExistingQueueForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    _printQueues.Add(form.PrintQueue);
                    _printQueueRows.Add(new LocalPrintQueueRow(form.PrintQueue));
                }
            }
        }

        private void addDynamic_ToolStripButton_Click(object sender, EventArgs e)
        {
            using (DynamicLocalPrintQueueForm form = new DynamicLocalPrintQueueForm(true))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (DynamicLocalPrintQueueInfo queue in form.PrintQueues)
                    {
                        _printQueues.Add(queue);
                        _printQueueRows.Add(new LocalPrintQueueRow(queue));
                    }
                }
            }
        }

        private void edit_ToolStripButton_Click(object sender, EventArgs e)
        {
            var selectedRow = printQueue_GridView.SelectedRows.FirstOrDefault();
            if (selectedRow != null)
            {
                LocalPrintQueueRow row = selectedRow.DataBoundItem as LocalPrintQueueRow;
                if (row != null)
                {
                    LocalPrintQueueInfo localQueue = row.PrintQueueInfo as LocalPrintQueueInfo;
                    if (localQueue != null)
                    {
                        EditLocalQueue(row, localQueue);
                    }

                    DynamicLocalPrintQueueInfo dynamicQueue = row.PrintQueueInfo as DynamicLocalPrintQueueInfo;
                    if (dynamicQueue != null)
                    {
                        EditDynamicQueue(row, dynamicQueue);
                    }
                }
            }
        }

        private void EditLocalQueue(LocalPrintQueueRow row, LocalPrintQueueInfo localQueue)
        {
            using (ExistingQueueForm form = new ExistingQueueForm(localQueue))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    _printQueueRows.Remove(row);
                    _printQueues.Remove(localQueue);
                    _printQueues.Add(form.PrintQueue);
                    _printQueueRows.Add(new LocalPrintQueueRow(form.PrintQueue));
                }
            }
        }

        private void EditDynamicQueue(LocalPrintQueueRow row, DynamicLocalPrintQueueInfo dynamicQueue)
        {
            using (DynamicLocalPrintQueueForm form = new DynamicLocalPrintQueueForm(dynamicQueue, true))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    _printQueueRows.Remove(row);
                    _printQueues.Remove(dynamicQueue);

                    foreach (DynamicLocalPrintQueueInfo queue in form.PrintQueues)
                    {
                        _printQueues.Add(queue);
                        _printQueueRows.Add(new LocalPrintQueueRow(queue));
                    }
                }
            }
        }

        private void remove_ToolStripButton_Click(object sender, EventArgs e)
        {
            var selectedRows = printQueue_GridView.SelectedRows.Select(n => n.DataBoundItem).Cast<LocalPrintQueueRow>().ToList();
            foreach (var row in selectedRows)
            {
                _printQueueRows.Remove(row);
                _printQueues.Remove(row.PrintQueueInfo);
            }
        }

        private class LocalPrintQueueRow
        {
            public PrintQueueInfo PrintQueueInfo { get; private set; }
            public string QueueName { get; private set; }
            public string QueueType { get; private set; }
            public string Device { get; private set; }

            private LocalPrintQueueRow(PrintQueueInfo queue)
            {
                PrintQueueInfo = queue;
                Device = queue.AssociatedAssetId;
            }

            public LocalPrintQueueRow(LocalPrintQueueInfo queue)
                : this((PrintQueueInfo)queue)
            {
                QueueName = queue.QueueName;
                QueueType = "Existing";
            }

            public LocalPrintQueueRow(DynamicLocalPrintQueueInfo queue)
                : this((PrintQueueInfo)queue)
            {
                QueueName = string.Format("{0}\\{1} on {2}", queue.PrintDriver.PackageName, queue.PrintDriver.DriverName, queue.AssociatedAssetId);
                QueueType = "Dynamic";
            }
        }
    }
}
