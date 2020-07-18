using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Control for selecting a list of remote print queues.
    /// </summary>
    public partial class RemotePrintQueueSelectionControl : UserControl
    {
        private readonly Dictionary<Guid, RadTreeNode> _queueTreeNodes = new Dictionary<Guid, RadTreeNode>();
        private bool _suppressSelectionChanged = false;

        /// <summary>
        /// Occurs when this control's selection is changed.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when the selection of the control changes.")]
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePrintQueueSelectionControl" /> class.
        /// </summary>
        public RemotePrintQueueSelectionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the <see cref="PrintQueueSelectionData" /> from this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PrintQueueSelectionData PrintQueueSelectionData
        {
            get { return new PrintQueueSelectionData(SelectedPrintQueues); }
        }

        /// <summary>
        /// Gets the selected print queues from this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PrintQueueInfoCollection SelectedPrintQueues
        {
            get
            {
                var queues = printQueueTreeView.CheckedNodes.Select(n => n.Tag).OfType<PrintQueueInfo>();
                return new PrintQueueInfoCollection(queues.ToList());
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has selected (and active) assets.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasSelection
        {
            get { return printQueueTreeView.CheckedNodes.Any(); }
        }

        /// <summary>
        /// Initializes this control by loading all print queues from the asset inventory.
        /// </summary>
        public void Initialize()
        {
            LoadTree();
        }

        /// <summary>
        /// Initializes this control by loading all print queues from the asset inventory
        /// with the specified queues selected.
        /// </summary>
        /// <param name="selectedQueues">The selected queues.</param>
        /// <exception cref="ArgumentNullException"><paramref name="selectedQueues" /> is null.</exception>
        public void Initialize(IEnumerable<RemotePrintQueueInfo> selectedQueues)
        {
            if (selectedQueues == null)
            {
                throw new ArgumentNullException(nameof(selectedQueues));
            }

            Initialize();
            SetSelectedQueues(selectedQueues.Select(n => n.PrintQueueId));
        }

        /// <summary>
        /// Initializes this control by loading all print queues from the asset inventory
        /// and setting the selected queues based on the specified <see cref="PrintQueueSelectionData" />.
        /// </summary>
        /// <param name="data">The print queue selection data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data" /> is null.</exception>
        public void Initialize(PrintQueueSelectionData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Initialize();
            SetSelectedQueues(data.SelectedPrintQueues.OfType<RemotePrintQueueDefinition>().Select(n => n.PrintQueueId));
        }

        private void LoadTree()
        {
            printQueueTreeView.Nodes.Clear();
            _queueTreeNodes.Clear();

            var serverQueueGroups = ConfigurationServices.AssetInventory.GetRemotePrintQueues().GroupBy(n => n.ServerHostName);
            foreach (var serverQueueGroup in serverQueueGroups)
            {
                RadTreeNode serverNode = new RadTreeNode(serverQueueGroup.Key);
                serverNode.ImageKey = "Server";
                foreach (RemotePrintQueueInfo queue in serverQueueGroup)
                {
                    RadTreeNode queueNode = new RadTreeNode(queue.QueueName);
                    queueNode.Tag = queue;
                    queueNode.ImageKey = "Queue";
                    _queueTreeNodes[queue.PrintQueueId] = queueNode;
                    serverNode.Nodes.Add(queueNode);
                }
                printQueueTreeView.Nodes.Add(serverNode);
            }
        }

        private void SetSelectedQueues(IEnumerable<Guid> selectedQueues)
        {
            _suppressSelectionChanged = true;

            foreach (RadTreeNode node in printQueueTreeView.Nodes)
            {
                node.Checked = false;
            }

            foreach (Guid id in selectedQueues)
            {
                if (_queueTreeNodes.TryGetValue(id, out RadTreeNode node))
                {
                    node.Checked = true;
                }
            }

            foreach (RadTreeNode node in printQueueTreeView.CheckedNodes)
            {
                node.Expand();
            }

            _suppressSelectionChanged = false;
        }

        private void printQueueTreeView_NodeCheckedChanged(object sender, TreeNodeCheckedEventArgs e)
        {
            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            if (!_suppressSelectionChanged)
            {
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
