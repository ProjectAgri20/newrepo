using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Runtime;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using HP.ScalableTest.Print;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.UI;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// Tree view for displaying details about enterprise scenario execution.
    /// </summary>
    public partial class SessionExecutionTreeView : RadTreeView
    {
        List<SessionMapElement> _orphans = new List<SessionMapElement>();
        SessionExecutionManager _sessionManager;

        /// <summary>
        /// Gets or sets the <see cref="SessionExecutionManager"/>.
        /// </summary>
        public SessionExecutionManager SessionManager
        {
            get { return _sessionManager; }
            set
            {
                _sessionManager = value;
            }
        }

        /// <summary>
        /// Occurs when a session map element is selected.
        /// </summary>
        public event EventHandler<SessionMapElementEventArgs> SessionMapElementSelected;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionExecutionTreeView"/> class.
        /// </summary>
        public SessionExecutionTreeView()
        {
            InitializeComponent();

            this.ThemeClassName = typeof(RadTreeView).FullName;
        }

        /// <summary>
        /// Initializes this instance by subscribing to <see cref="SessionClient"/> events.
        /// </summary>
        public void Initialize(SessionExecutionManager manager = null)
        {
            // Need the Invoke call to prevent cross-thread exceptions
            SessionClient.Instance.SessionMapElementReceived += (s, e) => Invoke(new Action(() => SessionMapElementReceived(e.MapElement)));
            SessionClient.Instance.ClearSessionRequestReceived += (s, e) => Invoke(new Action(() => ClearSession(e.SessionId)));

            SessionManager = manager; 
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            RootNode.Nodes.Clear();
            _orphans.Clear();

            // If a session manager is present make sure we display nodes for the sessions it's managing
            if (SessionManager != null)
            {
                var mapElements = SessionManager.GetManagedSessions().Select(x => x.MapElement);
                LoadElementsIntoTree(mapElements);
            }
        }

        internal void LoadElementsIntoTree(IEnumerable<SessionMapElement> elements)
        {
            // Some of the icons are loaded from the database, so this can't be initialized in the constructor.
            if (this.ImageList == null)
            {
                this.ImageList = IconManager.Instance.ExecutionIcons;
            }

            foreach (var element in elements)
            {
                SessionMapElementReceived(element);
            }           
        }

        /// <summary>
        /// The root node (first) of this instance.
        /// this.Nodes.FirstOrDefault();
        /// </summary>
        public RadTreeNode RootNode
        {
            get { return this.Nodes.FirstOrDefault(); }
        }

        private void SessionMapElementReceived(SessionMapElement element)
        {
            if (element == null)
            {
                return;
            }

            // If this node already exists, update it
            RadTreeNode existingNode = FindNode(element.Id);
            if (existingNode != null)
            {
                ConfigureNode(existingNode, element);
            }
            else
            {
                // If there is no parent, add it as a root node
                if (element.ParentId == Guid.Empty)
                {
                    RadTreeNode node = CreateElementNode(element);
                    RootNode.Nodes.Add(node);
                    ProcessOrphans(node, element);
                }
                else
                {
                    // Look for the parent
                    RadTreeNode parentNode = FindNode(element.ParentId);
                    if (parentNode != null)
                    {
                        RadTreeNode node = AddChild(parentNode, element);
                        if (node != null)
                        {
                            ProcessOrphans(node, element);
                        }
                    }
                    else
                    {
                        // This is an orphan
                        _orphans.Add(element);
                    }
                }
            }
        }

        private void ProcessOrphans(RadTreeNode node, SessionMapElement element)
        {
            lock (_orphans)
            {
                // Find any nodes that should be connected to the node we just added
                List<SessionMapElement> children = _orphans.Where(n => n.ParentId == element.Id).ToList();
                foreach (SessionMapElement child in children)
                {
                    AddChild(node, child);
                    _orphans.Remove(child);
                }
            }
        }

        private RadTreeNode AddChild(RadTreeNode node, SessionMapElement childElement)
        {
            // If this is a metadata item, determine whether we should actually show it
            if (childElement.ElementType == ElementType.Activity)
            {
                string subtype = (node.Tag as SessionMapElement).ElementSubtype;
                VirtualResourceType resourceType = EnumUtil.Parse<VirtualResourceType>(subtype);
                if (!resourceType.UsesPlugins())
                {
                    return null;
                }
            }

            RadTreeNode childNode = CreateElementNode(childElement);
            node.Nodes.Add(childNode);
            return childNode;
        }

        private void ClearSession(string sessionId)
        {            
            // LINQ gets messy, so do this the old fashioned way
            RadTreeNode subRootNode = null;
            foreach (RadTreeNode node in RootNode.Nodes.ToList())
            {
                SessionMapElement element = node.Tag as SessionMapElement;
                if (element != null && element.SessionId == sessionId)
                {
                    subRootNode = node;
                    break;
                }
            }

            if (subRootNode != null)
            {
                RootNode.Nodes.Remove(subRootNode);
            }
        }

        private void SessionExecutionTreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                // Get the element information for the selected node
                SessionMapElement element = e.Node.Tag as SessionMapElement;

                SessionMapElementSelected?.Invoke(this, new SessionMapElementEventArgs(element));

            }
        }

        #region Context Menu

        private SessionMapElement _contextMenuElement;

        private void SessionExecutionTreeView_ContextMenuOpening(object sender, TreeViewContextMenuOpeningEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null)
            {
                e.Cancel = true;
                return;
            }

            SessionMapElement element = e.Node.Tag as SessionMapElement;
            switch (element.ElementType)
            {
                // Virtual hosts can show the menu if they are starting up
                case ElementType.Machine:
                    e.Cancel = (element.State != RuntimeState.Starting);
                    break;

                // Simulators can show the menu if they are starting up
                case ElementType.Device:
                    if (element.ElementSubtype == "JediSimulator")
                    {
                        e.Cancel = (element.State != RuntimeState.Starting);
                    }
                    else
                    {
                        // Only display if it's Running or Offline
                        e.Cancel = (element.State != RuntimeState.Running && element.State != RuntimeState.Offline);
                    }
                    break;

                // Worker type virtual resources can show the menu if they are running or paused
                case ElementType.Worker:
                    List<string> workerResourceTypes = new List<string>() { "OfficeWorker", "AdminWorker", "CitrixWorker" };
                    if (workerResourceTypes.Contains(element.ElementSubtype))
                    {
                        e.Cancel = (element.State != RuntimeState.Running && element.State != RuntimeState.Paused);
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    break;

                // No menus for anything else
                default:
                    e.Cancel = true;
                    break;
            }

            // If we are going to allow the menu to be shown, configure it and save the selected element for later
            if (!e.Cancel)
            {
                ConfigureContextMenu(element);
                _contextMenuElement = element;
            }
        }

        private void ConfigureContextMenu(SessionMapElement element)
        {
            switch (element.ElementType)
            {
                case ElementType.Device:
                    switch (element.State)
                    {
                        case RuntimeState.Running:
                            suspendAsset_MenuItem.Visibility = ElementVisibility.Visible;
                            resumeAsset_MenuItem.Visibility = ElementVisibility.Collapsed;
                            break;
                        default:
                            suspendAsset_MenuItem.Visibility = ElementVisibility.Collapsed;
                            resumeAsset_MenuItem.Visibility = ElementVisibility.Visible;
                            break;
                    }
                    break;
                case ElementType.Machine:
                    restartVM_MenuItem.Visibility = ElementVisibility.Visible;
                    pauseWorker_MenuItem.Visibility = ElementVisibility.Collapsed;
                    resumeWorker_MenuItem.Visibility = ElementVisibility.Collapsed;
                    haltWorker_MenuItem.Visibility = ElementVisibility.Collapsed;
                    break;
                case ElementType.Worker:
                    if (element.State == RuntimeState.Running)
                    {
                        pauseWorker_MenuItem.Visibility = ElementVisibility.Visible;
                        resumeWorker_MenuItem.Visibility = ElementVisibility.Collapsed;
                    }
                    else
                    {
                        resumeWorker_MenuItem.Visibility = ElementVisibility.Visible;
                        pauseWorker_MenuItem.Visibility = ElementVisibility.Collapsed;
                    }

                    haltWorker_MenuItem.Visibility = ElementVisibility.Visible;
                    restartVM_MenuItem.Visibility = ElementVisibility.Collapsed;
                    break;
                default:
                    break;
            }
        }

        private void restartVM_MenuItem_Click(object sender, EventArgs e)
        {
            switch (_contextMenuElement.ElementType)
            {
                case ElementType.Machine:
                    SessionClient.Instance.RestartMachine(_contextMenuElement.SessionId, _contextMenuElement.Name, false);
                    break;

                case ElementType.Device:
                    SessionClient.Instance.RestartAsset(_contextMenuElement.SessionId, _contextMenuElement.Name);
                    break;
            }

            _contextMenuElement = null;
        }

        private void pauseWorker_MenuItem_Click(object sender, EventArgs e)
        {
            SessionClient.Instance.PauseWorker(_contextMenuElement.SessionId, _contextMenuElement.Name);
            _contextMenuElement = null;
        }

        private void resumeWorker_MenuItem_Click(object sender, EventArgs e)
        {
            SessionClient.Instance.ResumeWorker(_contextMenuElement.SessionId, _contextMenuElement.Name);
            _contextMenuElement = null;
        }

        private void haltWorker_MenuItem_Click(object sender, EventArgs e)
        {
            SessionClient.Instance.HaltWorker(_contextMenuElement.SessionId, _contextMenuElement.Name);
            _contextMenuElement = null;
        }

        private void suspendAsset_MenuItem_Click(object sender, EventArgs e)
        {
            SessionClient.Instance.TakeAssetOffline(_contextMenuElement.SessionId, _contextMenuElement.Name);
            _contextMenuElement = null;
        }

        private void resumeAsset_MenuItem_Click(object sender, EventArgs e)
        {
            SessionClient.Instance.BringAssetOnline(_contextMenuElement.SessionId, _contextMenuElement.Name);
            _contextMenuElement = null;
        }
        /// <summary>
        /// Handles the Click event of the enableCRC_MenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void enablePaperless_MenuItem_Click(object sender, EventArgs e)
        {            
            SetDeviceCRC(true);
            _contextMenuElement = null;
        }

        /// <summary>
        /// Handles the Click event of the disableCRC_MenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void disablePaperless_MenuItem_Click(object sender, EventArgs e)
        {            
            SetDeviceCRC(false);
            _contextMenuElement = null;
        }
        /// <summary>
        /// Sets the device CRC with the given Boolean.
        /// </summary>
        /// <param name="crcOn">if set to <c>true</c> [CRC on].</param>
        private void SetDeviceCRC(bool crcOn)
        {
            TraceFactory.Logger.Debug("Setting CRC mode of {0}".FormatWith(crcOn.ToString()));
            SessionClient.Instance.SetCrc(_contextMenuElement.SessionId, _contextMenuElement.Name, crcOn);
        }
        #endregion

        #region Node Operations

        private RadTreeNode FindNode(Guid nodeId)
        {
            return Find(n => n.Tag != null && (n.Tag as SessionMapElement).Id == nodeId);
        }

        private RadTreeNode CreateElementNode(SessionMapElement element)
        {
            RadTreeNode node = new RadTreeNode();
            ConfigureNode(node, element);

            switch (element.ElementType)
            {
                case ElementType.Session:
                case ElementType.Workers:
                case ElementType.Assets:
                case ElementType.RemotePrintQueues:
                    node.Expanded = true;
                    break;
            }

            return node;
        }

        /// <summary>
        /// Updates an existing node with new data from a SessionMapElement.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="element">The element.</param>
        private void ConfigureNode(RadTreeNode node, SessionMapElement element)
        {
            node.Tag = element;
            node.ImageKey = GetImageKey(element);
            SetContextMenu(node, element.ElementType);

            if (!string.IsNullOrWhiteSpace(element.Message))
            {
                node.Text = "{0} ({1})".FormatWith(GetName(element), element.Message);
            }
            else
            {
                node.Text = GetName(element);
            }
        }

        private string GetName(SessionMapElement element)
        {
            switch (element.ElementType)
            {
                case ElementType.Session:
                    return element.Name;

                case ElementType.Workers:
                    return "Virtual Resources";

                case ElementType.Assets:
                    return "Test Assets";

                case ElementType.RemotePrintQueues:
                    return "Remote Print Queues";

                default:
                    return element.Name;
            }
        }

        private static string GetImageKey(SessionMapElement element)
        {
            switch (element.ElementType)
            {
                case ElementType.Session:
                    return "Session";

                case ElementType.Workers:
                    return "SessionResources";

                case ElementType.Assets:
                    return "SessionAssets";

                case ElementType.Machine:
                    return "Computer";

                case ElementType.RemotePrintQueues:
                    return "PrintServer";

                case ElementType.RemotePrintQueue:
                    return "RemotePrinter";

                case ElementType.Device:
                case ElementType.Worker:
                case ElementType.Activity:
                    return element.ElementSubtype;

                default:
                    return null;
            }
        }

        private void SetContextMenu(RadTreeNode node, ElementType elementType)
        {
            if (elementType == ElementType.Device)
            {
                node.ContextMenu = asset_ContextMenu;
            }
        }

        #endregion

    }
}
