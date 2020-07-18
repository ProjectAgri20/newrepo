using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.PluginSupport.PullPrint
{
    public partial class PrintingTabConfigControl : UserControl
    {
        private readonly List<PrintQueueInfo> _printQueues = new List<PrintQueueInfo>();
        private readonly BindingList<PrintQueueRow> _printQueueRows = new BindingList<PrintQueueRow>();

        private int delayAfterPrint;
        private bool shuffleDocuments;
        private bool usePrintServerNotification;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of <see cref="PrintingTabConfigControl" />
        /// </summary>
        public PrintingTabConfigControl()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(printQueues_GridView, GridViewStyle.ReadOnly);
            printQueues_GridView.DataSource = _printQueueRows;

            fieldValidator.RequireDocumentSelection(documentSelectionControl);
            fieldValidator.RequireCustom(printQueues_GridView, () => printQueues_GridView.Rows.Any(), "At least one print queue must be selected.");

            SetConfigurationMonitors();
        }

        private void SetConfigurationMonitors()
        {
            shuffle_CheckBox.CheckedChanged += (s, e) => OnConfigurationChanged(s, e);
            documentSelectionControl.SelectionChanged += (s, e) => OnConfigurationChanged(s, e);
            numericUpDownDelayAfterPrint.ValueChanged += (s, e) => OnConfigurationChanged(s, e);
            printServerNotificationcheckBox.CheckedChanged += (s, e) => OnConfigurationChanged(s, e);
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        public void Initialize()
        {
            delayAfterPrint = 10;
            shuffleDocuments = false;
            usePrintServerNotification = false;    

            ConfigureControls(delayAfterPrint, shuffleDocuments, usePrintServerNotification);

            documentSelectionControl.Initialize();
            _printQueues.Clear();
            RefreshQueueDataGrid();
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="DocumentSelectionData" />.
        /// </summary>
        public void Initialize(DocumentSelectionData docData, PrintQueueSelectionData queueData, int delay, bool shuffle, bool usePrintServerNotification)
        {
            ConfigureControls(delay, shuffle, usePrintServerNotification);
            documentSelectionControl.Initialize(docData);
            LoadPrintQueues(queueData.SelectedPrintQueues);
            RefreshQueueDataGrid();
        }

        /// <summary>
        /// Gets the delay.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetDelay() { return (int)numericUpDownDelayAfterPrint.Value; }

        /// <summary>
        /// Gets the shuffle.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool GetShuffle() { return shuffle_CheckBox.Checked; }
        
        /// <summary>
        /// Gets the print server notification.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool GetPrintServerNotification() { return printServerNotificationcheckBox.Checked; }

        /// <summary>
        /// Gets the print queues.
        /// </summary>
        /// <returns>PrintQueueSelectionData.</returns>
        public PrintQueueSelectionData GetPrintQueues() { return new PrintQueueSelectionData(_printQueues); }

        /// <summary>
        /// Gets the documents.
        /// </summary>
        /// <returns>DocumentSelectionData.</returns>
        public DocumentSelectionData GetDocuments() { return documentSelectionControl.DocumentSelectionData; }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        public IEnumerable<ValidationResult> ValidateConfiguration() => fieldValidator.ValidateAll();

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, EventArgs.Empty);
            if (sender is CheckBox)
            {
                if (((CheckBox)sender).Checked)
                {
                    numericUpDownDelayAfterPrint.Enabled = false;
                }
                else
                {
                    if (!numericUpDownDelayAfterPrint.Enabled)
                    {
                        numericUpDownDelayAfterPrint.Enabled = true;                        
                    }
                }
            }
        }

        private void ConfigureControls(int delay, bool shuffle, bool usePrintServerNotification)
        {
            shuffleDocuments = shuffle;
            shuffle_CheckBox.Checked = shuffle;
            // old metadata will not have a value
            delayAfterPrint = delay;
            numericUpDownDelayAfterPrint.Value = (delay > 0) ? delay : 10;
            printServerNotificationcheckBox.Checked = usePrintServerNotification;
        }

        private void LoadPrintQueues(PrintQueueDefinitionCollection printQueueDefinitions)
        {
            _printQueues.Clear();

            foreach (LocalPrintQueueDefinition local in printQueueDefinitions.OfType<LocalPrintQueueDefinition>())
            {
                _printQueues.Add(new LocalPrintQueueInfo(local.QueueName, local.AssociatedAssetId));
            }

            var remotePrintQueues = ConfigurationServices.AssetInventory.GetRemotePrintQueues().ToList();
            foreach (RemotePrintQueueDefinition remote in printQueueDefinitions.OfType<RemotePrintQueueDefinition>())
            {
                var queue = remotePrintQueues.FirstOrDefault(n => n.PrintQueueId == remote.PrintQueueId);
                if (queue != null)
                {
                    _printQueues.Add(queue);
                }
            }
        }

        private void RefreshQueueDataGrid()
        {
            _printQueueRows.Clear();
            foreach (PrintQueueInfo info in _printQueues)
            {
                _printQueueRows.Add(new PrintQueueRow(info));
            }
        }

        private void localQueues_ToolStripButton_Click(object sender, EventArgs e)
        {
            var originalLocalPrintQueues = _printQueues.Except(_printQueues.OfType<RemotePrintQueueInfo>()).ToList();
            using (LocalQueueSelectionForm form = new LocalQueueSelectionForm(originalLocalPrintQueues))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    ReplaceQueues(originalLocalPrintQueues, form.SelectedQueues);
                    RefreshQueueDataGrid();
                    OnConfigurationChanged(this, EventArgs.Empty);
                }
            }
        }

        private void remoteQueues_ToolStripButton_Click(object sender, EventArgs e)
        {
            var originalRemotePrintQueues = _printQueues.OfType<RemotePrintQueueInfo>().ToList();
            using (RemoteQueueSelectionForm form = new RemoteQueueSelectionForm(originalRemotePrintQueues))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    ReplaceQueues(originalRemotePrintQueues, form.SelectedQueues);
                    RefreshQueueDataGrid();
                    OnConfigurationChanged(this, EventArgs.Empty);
                }
            }
        }

        private void ReplaceQueues(IEnumerable<PrintQueueInfo> oldQueues, IEnumerable<PrintQueueInfo> newQueues)
        {
            foreach (PrintQueueInfo queue in oldQueues)
            {
                _printQueues.Remove(queue);
            }
            _printQueues.AddRange(newQueues);
        }


        private class PrintQueueRow
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used in data grid binding.")]
            public string QueueName { get; private set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used in data grid binding.")]
            public string PrintServer { get; private set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used in data grid binding.")]
            public string QueueType { get; private set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used in data grid binding.")]
            public string Device { get; private set; }

            public PrintQueueRow(PrintQueueInfo queue)
            {
                QueueName = queue.QueueName;
                PrintServer = "Client";
                QueueType = "Local";
                Device = queue.AssociatedAssetId;

                RemotePrintQueueInfo remoteQueue = queue as RemotePrintQueueInfo;
                if (remoteQueue != null)
                {
                    PrintServer = remoteQueue.ServerHostName;
                    QueueType = "Remote";
                }
            }
        }
    }
}
