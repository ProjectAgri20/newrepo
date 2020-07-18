using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.PerfMonCollector;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    public partial class PerfMonCollectorCounterControl : ScenarioConfigurationControlBase
    {
        private const string PrintQueueCategory = "Print Queue";
        private List<ResourceWindowsCategory> _printQueueCounterNames = new List<ResourceWindowsCategory>();
        private TreeNode _printQueueNode = new TreeNode(PrintQueueCategory);
        private VirtualResourceMetadata _metadata = null;
        private PerfMonCounterData _selected = new PerfMonCounterData();

        public PerfMonCollectorCounterControl()
        {
            InitializeComponent();
            _printQueueNode.Name = PrintQueueCategory;
        }

        internal override EntityObject EntityObject
        {
            get { return _metadata; }
        }

        public override void Initialize()
        {
            Initialize(new VirtualResourceMetadata("PerfMonCollector", "PerfMonCounter"));
        }

        public override void Initialize(object entity)
        {
            _metadata = entity as VirtualResourceMetadata;
            if (_metadata == null)
            {
                throw new EditorTypeMismatchException(entity, typeof(VirtualResourceMetadata));
            }

            string server = (_metadata.VirtualResource as PerfMonCollector).HostName;
            List<ResourceWindowsCategory> categories = null;

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                //Load categories from the database.
                categories = ResourceWindowsCategory.Select(context, "PerfMon").ToList();
            }

            // Get the top node. It will have a blank name.
            ResourceWindowsCategory top = categories.FirstOrDefault(c => string.IsNullOrEmpty(c.Name));
            foreach (ResourceWindowsCategory category in top.Children)
            {
                if (category.Name == PrintQueueCategory)
                {
                    LoadPrintQueueCounterNames(category.Children);
                }
                else
                {
                    BuildTree(category);
                }
            }
            LoadPrintQueues(server);

            SelectedCounter = _metadata.Metadata;

            // Data Bindings
            hostNameDisplay_Label.Text = server;
            category_TextBox.DataBindings.Add("Text", _selected, "Category");
            instance_TextBox.DataBindings.Add("Text", _selected, "Instance");
            counter_TextBox.DataBindings.Add("Text", _selected, "Counter");
            collect_CheckBox.DataBindings.Add("Checked", _selected, "CollectAtStart");
            //Interval Binding requires special formatting.
            Binding intervalBinding = new Binding("Text", _selected, "CollectionInterval");
            intervalBinding.Format += new ConvertEventHandler(IntervalBinding_Format);
            intervalBinding.Parse += new ConvertEventHandler(IntervalBinding_Parse);
            interval_TextBox.DataBindings.Add(intervalBinding);

            SyncTreeChecked(_selected.Key);

            available_TreeView.BeforeCheck += new TreeViewCancelEventHandler(Available_TreeView_BeforeCheck);
            available_TreeView.AfterCheck += new TreeViewEventHandler(Available_TreeView_AfterCheck);

            // Validate the data coming in
            ValidateInterval(_selected.CollectionInterval);
            ValidateSelection();
        }

        public override void FinalizeEdit()
        {
            // Modify focus so that any data bindings will update
            title_Label.Focus();

            _metadata.Metadata = SelectedCounter;
        }

        private void IntervalBinding_Parse(object sender, ConvertEventArgs e)
        {
            string formattedValue = "00:{0}".FormatWith(e.Value);
            int val = (int)formattedValue.ToTimeSpan(TimeSpanFormat.HourMinuteSecond).TotalSeconds;
            ValidateInterval(val);
            e.Value = val;
        }

        private void IntervalBinding_Format(object sender, ConvertEventArgs e)
        {
            e.Value = ((int)e.Value).ToTimeSpanString();
        }

        private string SelectedCounter
        {
            get { return _selected.ToXml(); }
            set { _selected = value.ToObject<PerfMonCounterData>(); }
        }

        private void BuildTree(ResourceWindowsCategory categoryItem)
        {
            BuildTree(categoryItem, null);
        }

        private void BuildTree(ResourceWindowsCategory categoryItem, TreeNode parentNode)
        {
            TreeNode thisNode = null;
            if (parentNode == null)
            {
                thisNode = available_TreeView.Nodes.Add(categoryItem.Name);
            }
            else
            {
                thisNode = parentNode.Nodes.Add(categoryItem.Name);
            }

            thisNode.Name = GenerateNodeKey(thisNode);
            if (categoryItem.Children.Count > 0)
            {
                HideCheckBox(thisNode);
            }
            foreach (ResourceWindowsCategory child in categoryItem.Children)
            {
                BuildTree(child, thisNode);
            }
        }

        /// <summary>
        /// Custom key builder for TreeNode key.
        /// Would have used the FullPath property except it throws an exception when
        /// called on a node that has not been added to a TreeView control.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        private static string GenerateNodeKey(TreeNode node)
        {
            StringBuilder key = new StringBuilder();

            if (node.Parent != null)
            {
                if (node.Parent.Parent != null)
                {
                    key.Append(node.Parent.Parent.Text).Append("/");
                }
                key.Append(node.Parent.Text).Append("/");
            }
            key.Append(node.Text);

            return key.ToString();
        }

        private void LoadPrintQueueCounterNames(IEnumerable<ResourceWindowsCategory> counters)
        {
            foreach (ResourceWindowsCategory counter in counters)
            {
                _printQueueCounterNames.Add(counter);
            }
        }

        private void LoadPrintQueues(string serverName)
        {
            List<RemotePrintQueue> queues = null;
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                queues = RemotePrintQueue.SelectByPrintServerName(context, serverName).ToList();
            }

            _printQueueNode.Nodes.Clear();
            foreach (RemotePrintQueue queue in queues)
            {
                TreeNode queueNode = _printQueueNode.Nodes.Add(queue.Name);
                foreach (ResourceWindowsCategory counter in _printQueueCounterNames)
                {
                    BuildTree(counter, queueNode);
                }
            }

            // Only display the Print Queue Node if the selected server has queues.
            if (_printQueueNode.Nodes.Count > 0)
            {
                available_TreeView.Nodes.Add(_printQueueNode);
            }
        }

        private void Available_TreeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            // Only execute if a user caused the checked state to change and if the node is being checked.
            if (e.Action != TreeViewAction.Unknown && e.Node.Checked == false)
            {
                //System.Diagnostics.Debug.WriteLine("Calling ClearAllNodes");
                ClearAllChecked(available_TreeView.Nodes);
            }
        }

        private void Available_TreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // Only execute if a user caused the checked state to change.  This prevents a stack overflow error.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count == 0 && e.Node.Checked)
                {
                    UpdateSelected(e.Node);
                    SelectNode(e.Node);
                    ValidateSelection();
                }
                else
                {
                    UpdateSelected(string.Empty, string.Empty, string.Empty);
                    SelectNode(null);
                }
            }
        }

        private void UpdateSelected(TreeNode node)
        {
            UpdateSelected(node.Parent.Parent.Text, node.Parent.Text, node.Text);
        }

        private void UpdateSelected(string category, string instance, string counter)
        {
            _selected.Category = category;
            _selected.Instance = instance;
            _selected.Counter = counter;

            if (_metadata.Name != _selected.Key)
            {
                _metadata.Name = _selected.Key;
            }
        }

        private void SyncTreeChecked(string nodeKey)
        {
            TreeNode[] found = available_TreeView.Nodes.Find(nodeKey, true);
            if (found.Length > 0)
            {
                found[0].Checked = true;
                found[0].EnsureVisible();
                SelectNode(found[0]);
            }
        }

        private void SelectNode(TreeNode node)
        {
            available_TreeView.SelectedNode = node;
            available_TreeView.Focus();
        }

        private void ClearAllChecked(TreeNodeCollection nodeCollection)
        {
            foreach (TreeNode node in nodeCollection)
            {
                if (node.Checked)
                {
                    node.Checked = false;
                }
                if (node.Nodes.Count > 0)
                {
                    ClearAllChecked(node.Nodes);
                }
            }
        }

        private bool ValidateSelection()
        {
            if (counter_TextBox.Text.Length == 0)
            {
                fieldValidator.SetError(counter_TextBox, "A counter must be selected.");
                return false;
            }

            fieldValidator.SetError(counter_TextBox, string.Empty);
            return true;
        }

        private bool ValidateInterval(int intervalValue)
        {
            if (intervalValue < 1)
            {
                fieldValidator.SetError(mmss_Label, "Interval must be at least one second and match the format mm:ss.");
                return false;
            }

            fieldValidator.SetError(mmss_Label, string.Empty);
            return true;
        }


        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;

        /// <summary>
        /// Hides the checkbox for the specified node on a TreeView control.
        /// </summary>
        private void HideCheckBox(TreeNode node)
        {
            TreeViewItem tvi = new TreeViewItem();
            tvi.hItem = node.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            SendMessage(available_TreeView.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TreeViewItem lParam);

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TreeViewItem
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }
    }
}
