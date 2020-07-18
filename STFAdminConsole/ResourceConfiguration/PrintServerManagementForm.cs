using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.LabConsole.Properties;
using Telerik.WinControls.UI;
using AssetSelectionForm = HP.ScalableTest.UI.Framework.AssetSelectionForm;

namespace HP.ScalableTest.LabConsole
{
    public partial class PrintServerManagementForm : Form
    {
        private SortableBindingList<FrameworkServer> _printServers = new SortableBindingList<FrameworkServer>();
        private SortableBindingList<RemotePrintQueue> _printQueues = new SortableBindingList<RemotePrintQueue>();
        private FrameworkServerController _controller = null;
        private ServerType _serverType = ServerType.Print;

        private struct QueueColumns
        {
            public const string Active = "active_GridViewColumn";
            public const string InventoryId = "inventoryId_GridViewColumn";
            public const string LookupButton = "lookupButton_GridViewColumn";
            public const string DeviceType = "deviceType_GridViewColumn";
            public const string Name = "name_GridViewColumn";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueSelectionForm"/> class.
        /// </summary>
        public PrintServerManagementForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(printServer_GridView, GridViewStyle.ReadOnly);
            UserInterfaceStyler.Configure(printQueue_GridView, GridViewStyle.Display);
        }

        public ServerType ServerType 
        {
            set
            {
                _serverType = value;
                ConfigureDisplay();
            }
        }

        /// <summary>
        /// Handles the Load event of the PrintQueueSelectionForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PrintServerManagementForm_Load(object sender, EventArgs e)
        {
            _controller = new FrameworkServerController();

            RefreshServerDisplay();

            if (printServer_GridView.SelectedRows.Count > 0)
            {
                printServerSelected(printServer_GridView.CurrentRow.Index);
            }
        }

        private void printServer_GridView_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            //Configure grid lines for all cells
            ConfigureGridLines(e.CellElement, Color.LightGray);
        }

        private void printQueue_GridView_CellFormatting(object sender, CellFormattingEventArgs e)
        {            
            if (e.CellElement is GridCommandCellElement)
            {
                GridCommandCellElement buttonCell = e.CellElement as GridCommandCellElement;

                //Manually set the button size.  If AutoSize is used, it will not size the button correctly.
                buttonCell.CommandButton.AutoSize = false;
                buttonCell.CommandButton.Size = new System.Drawing.Size(printQueue_GridView.Columns[buttonCell.ColumnIndex].Width - 8, printQueue_GridView.TableElement.RowHeight - 8);
            }

            //Configure grid lines for all cells
            ConfigureGridLines(e.CellElement, Color.LightGray);
        }

        private static void ConfigureGridLines(GridCellElement cellElement, Color lineColor)
        {
            cellElement.BorderBottomWidth = 1;
            cellElement.BorderBottomColor = lineColor;

            cellElement.BorderRightWidth = 1;
            cellElement.BorderRightColor = lineColor;
        }

        private GridViewRowInfo GetFirstPrintServerSelectedRow()
        {
            return printServer_GridView.SelectedRows.FirstOrDefault();
        }

        private int GetDataBoundItemIndex(Guid frameworkServerId)
        {
            foreach (GridViewRowInfo row in printServer_GridView.Rows)
            {
                FrameworkServer dataItem = (FrameworkServer)row.DataBoundItem;
                if (dataItem.FrameworkServerId == frameworkServerId)
                {
                    return row.Index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the selected queue.
        /// </summary>
        /// <value>
        /// The queue data.
        /// </value>
        private RemotePrintQueue SelectedQueue
        {
            get 
            {
                GridViewRowInfo row = printQueue_GridView.SelectedRows.FirstOrDefault();
                if (row != null)
                {
                    return row.DataBoundItem as RemotePrintQueue;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the selected server.
        /// </summary>
        /// <value>
        /// The server data.
        /// </value>
        private FrameworkServer SelectedServer
        {
            get
            {
                GridViewRowInfo rowInfo = GetFirstPrintServerSelectedRow();
                return (rowInfo != null ? (rowInfo.DataBoundItem as FrameworkServer) : null);
            }
            set
            {
                int index = GetDataBoundItemIndex(value.FrameworkServerId);
                if (index >= 0)
                {
                    printServer_GridView.TableElement.ScrollToRow(index);
                    printServer_GridView.Rows[index].IsSelected = true;
                }
            }
        }

        private void ConfigureDisplay()
        {
            string stringValue = _serverType.ToString();
            this.Text = "{0} Server Management".FormatWith(stringValue);
            servers_ToolStripLabel.Text = "{0} Servers".FormatWith(stringValue);
            queues_ToolStripLabel.Text = "{0} Queues".FormatWith(stringValue);

            // Set Users ability to automatically refresh queues
            newServer_ToolStripButton.Enabled = 
                refresh_ToolStripButton.Enabled = 
                (_serverType != ServerType.ePrint);

            // Set Users ability to manually edit Queues
            newQueue_ToolStripButton.Visible = 
                removeQueue_ToolStripButton.Visible = 
                editQueue_ToolStripButton.Visible = 
                (_serverType == ServerType.ePrint);

        }

        private void RefreshServerDisplay()
        {
            printServer_GridView.DataSource = null;
            _printServers.Clear();

            foreach (FrameworkServer item in _controller.GetServersByType(_serverType))
            {
                _printServers.Add(item);
            }

            printServer_GridView.DataSource = _printServers;
            printServer_GridView.BestFitColumns();
        }

        private void SetSelectedIndex(RadGridView radGridView, int selectedIndex)
        {
            GridViewRowInfo foundRow = null;
            while (radGridView.RowCount > 0)
            {
                foundRow = radGridView.Rows.FirstOrDefault(x => x.Index == selectedIndex);
                if (foundRow != null)
                {
                    radGridView.TableElement.ScrollToRow(selectedIndex);
                    radGridView.CurrentRow = foundRow;
                    break;
                }
                selectedIndex--;
            }
        }

        /// <summary>
        /// Displays the print queue data for the specified server.
        /// </summary>
        /// <param name="server">The server</param>
        private void RefreshQueueDisplay(FrameworkServer server)
        {
            printQueue_GridView.DataSource = null;
            _printQueues.Clear();

            foreach (RemotePrintQueue queue in _controller.Context.RemotePrintQueues.Where(n => n.PrintServerId == server.FrameworkServerId).OrderBy(x => x.Name))
            {
                _printQueues.Add(queue);
            }

            printQueue_GridView.DataSource = _printQueues;
            printQueue_GridView.BestFitColumns();
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        private void SaveChanges()
        {
            SetWaitCursor();
            try
            {
                _controller.SaveChanges();
            }
            finally
            {
                SetDefaultCursor();
            }
        }

        /// <summary>
        /// Handles the Click event of the apply_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void close_Button_Click(object sender, EventArgs e)
        {
            _controller.SaveChanges();
            Close();
        }
        
        /// <summary>
        /// Creates a new print queue from the provided name, parsing the inventory id and inferring the queue type.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        private static RemotePrintQueue CreatePrintQueue(string queueName)
        {
            // Try to parse the printer id out of the queue name.
            // For virtual printers, look for the name to start in the form VA1 00000.
            // The corresponding printer's inventory id is VP01-0000.
            Regex virtualPrinterPattern = new Regex(@"^V\w*(\d{1,2}) (\d{5})");
            if (virtualPrinterPattern.IsMatch(queueName))
            {
                Match match = virtualPrinterPattern.Match(queueName);
                int serverNumber = int.Parse(match.Groups[1].ToString(), CultureInfo.InvariantCulture);
                string inventoryId = "VP{0}-{1}".FormatWith(serverNumber.ToString("00", CultureInfo.InvariantCulture), match.Groups[2]);
                return CreatePrintQueue(queueName, "Virtual", inventoryId);
            }

            // For physical printers, look for the inventory id in the form AAA-00000.
            Regex physicalPrinterPattern = new Regex(@"\b\w{3}-\d{4,5}\b");
            if (physicalPrinterPattern.IsMatch(queueName))
            {
                string inventoryId = physicalPrinterPattern.Match(queueName).Groups[0].ToString();
                return CreatePrintQueue(queueName, "Physical", inventoryId);
            }

            // If we didn't match anything, just leave the inventory id blank
            return CreatePrintQueue(queueName, "Physical", "");
        }

        private static RemotePrintQueue CreatePrintQueue(string name, string platform, string inventoryId)
        {
            RemotePrintQueue queue = new RemotePrintQueue();
            queue.RemotePrintQueueId = SequentialGuid.NewGuid();
            queue.Name = name;
            queue.PrinterId = inventoryId;
            queue.Active = true;

            return queue;
        }

        /// <summary>
        /// Sets the wait cursor.
        /// </summary>
        private void SetWaitCursor()
        {
            Cursor = Cursors.WaitCursor;
        }

        /// <summary>
        /// Sets the default cursor.
        /// </summary>
        private void SetDefaultCursor()
        {
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Processes queues that are resident on the Print Server but missing from
        /// the database.  The user has the option of adding these new queues to the
        /// database.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="queuesOnServer">The queues on server.</param>
        /// <returns></returns>
        private bool FindMissingDatabaseQueues(FrameworkServer server, Collection<string> queuesOnServer)
        {
            bool changesExist = false;

            // Determine if any of the queues found above for the print server are missing
            // from the database.
            Collection<string> queuesMissingInDatabase = new Collection<string>();
            foreach (string name in queuesOnServer)
            {
                // If the queue name doesn't already exist in the queues for the server then add it.
                // Note: We don't need to use n.Name.Equals(name, StringComparison.OrdinalIgnoreCase) because the LINQ is translated into SQL which is case insensitive 
                bool exists = _controller.Context.RemotePrintQueues.Any(n => n.PrintServerId == server.FrameworkServerId && n.Name == name); 

                if (!exists)
                {
                    queuesMissingInDatabase.Add(name);
                }
            }

            if (queuesMissingInDatabase.Count > 0)
            {
                changesExist = true;

                // If there are queues missing in the database, the user is prompted to select which ones to add to it.
                using (PrintQueueRefreshForm form = new PrintQueueRefreshForm(queuesMissingInDatabase, Properties.Resources.AddQueues, "Add", "Add Missing Queues To Database"))
                {
                    DialogResult result = form.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        foreach (string queue in form.SelectedQueues)
                        {
                            RemotePrintQueue newQueue = CreatePrintQueue(queue);
                            newQueue.PrintServer = server;

                            _controller.Context.RemotePrintQueues.Add(newQueue);
                        }

                        SaveChanges();
                        RefreshQueueDisplay(server);
                    }
                }
            }

            return changesExist;
        }

        /// <summary>
        /// Processes queues that are in the database but don't show up on the server anymore.
        /// The user is prompted to remove these queues if they choose, but if the queues
        /// are still referenced within an activity then they can't be deleted.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="queuesOnServer">The queues on server.</param>
        /// <returns></returns>
        private bool FindMissingServerQueues(FrameworkServer server, Collection<string> queuesOnServer)
        {
            bool changesExist = false;

            // There are queues that no longer exist on the server, ask the user what they want
            // to do.  Should they keep the information around, but inactive, or should they
            // remove the queue entry from the database?
            Collection<RemotePrintQueue> queuesMissingOnServer = new Collection<RemotePrintQueue>();
            foreach (var item in _controller.Context.RemotePrintQueues.Where(n => n.PrintServerId == server.FrameworkServerId))
            {
                if (!queuesOnServer.Contains(item.Name))
                {
                    queuesMissingOnServer.Add(item);
                }
            }

            SortableBindingList<PrintQueueInUse> queuesInUse = _controller.SelectQueuesInUse(server);
            Collection<Tuple<string, string>> queuesAndScenarios = new Collection<Tuple<string, string>>();

            foreach (string queueName in queuesMissingOnServer.Select(q => q.Name))
            {
                PrintQueueInUse queue = queuesInUse.Where(q => q.QueueName == queueName).FirstOrDefault();

                if (queue != null)
                {
                    queuesAndScenarios.Add(new Tuple<string, string>(queueName, queue.ScenarioName));
                }
                else
                {
                    queuesAndScenarios.Add(new Tuple<string, string>(queueName, string.Empty));
                }
            }

            if (queuesMissingOnServer.Count > 0)
            {
                changesExist = true;

                // If there are queues missing on the server, the user selects which queues to forcefully remove from the database, EVEN IF THEY ARE BEING USED IN A SCENARIO
                using (PrintQueueRefreshForm form = new PrintQueueRefreshForm(queuesAndScenarios, Properties.Resources.RemoveQueues.FormatWith('\n'), "Remove", "Remove Missing Queues From Database", "Force Remove"))
                {
                    DialogResult result = form.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        foreach (string queue in form.SelectedQueues)
                        {
                            RemotePrintQueue remoteQueue = queuesMissingOnServer.Where(q => q.Name == queue).FirstOrDefault();
                            _controller.Context.RemotePrintQueues.Remove(remoteQueue);
                        }

                        SaveChanges();
                        RefreshQueueDisplay(server);
                    }
                }
            }

            return changesExist;
        }

        /// <summary>
        /// Refreshes the queue list.
        /// </summary>
        /// <param name="server">The server data.</param>
        private void RefreshQueueList(FrameworkServer server)
        {
            Collection<string> queues = null;

            try
            {
                bool changesMade = false;               

                SetWaitCursor();
                string serverName = $"{server.HostName}.{GlobalSettings.Items[Setting.DnsDomain]}";
                TraceFactory.Logger.Debug("Getting queue names from: {0}".FormatWith(serverName));
                queues = GetPrintQueueNames(serverName);

                if (queues != null)
                {
                    if (FindMissingDatabaseQueues(server, queues))
                    {
                        changesMade = true;
                    }

                    if (FindMissingServerQueues(server, queues))
                    {
                        changesMade = true;
                    }
                }

                if (!changesMade)
                {
                    MessageBox.Show
                    (
                        Properties.Resources.PrintServerUpToDate,
                        "Refresh Print Queues",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Error Retrieving Print Queues", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TraceFactory.Logger.Error("Error Retrieving Print Queues", ex);
                return;
            }
            finally
            {
                SetDefaultCursor();
            }
        }

        private void associateAsset_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridViewRowInfo row = printQueue_GridView.SelectedRows.FirstOrDefault();

            if (row != null)
            {
                AddInventoryId(row);
            }
        }

        private void removeAssociation_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridViewRowInfo row = printQueue_GridView.SelectedRows.FirstOrDefault();

            if (row != null)
            {
                GridViewCellInfo otherCell = row.Cells[QueueColumns.Name];

                RemotePrintQueue queue = _controller.Context.RemotePrintQueues.Where(x => x.Name == otherCell.Value.ToString()).FirstOrDefault();
                _controller.Context.RemotePrintQueues.Remove(queue);
                _controller.Context.SaveChanges();

                row.Delete();
                
                printQueue_GridView.Refresh();
            }
        }

        private void AddInventoryId(GridViewRowInfo rowToUpdate)
        {
            GridViewCellInfo targetCell = rowToUpdate.Cells[QueueColumns.InventoryId];
            if (SelectedQueue != null)
            {
                object valueObject = targetCell.Value;
                string currentValue = string.Empty;
                if (valueObject != null)
                {
                    currentValue = valueObject.ToString();
                } 

                using (AssetSelectionForm printerSelectionForm = new AssetSelectionForm(AssetAttributes.Printer, currentValue))
                {
                    printerSelectionForm.ShowDialog(this);
                    if (printerSelectionForm.DialogResult == DialogResult.OK)
                    {
                        targetCell.Value = printerSelectionForm.SelectedAssetIds.FirstOrDefault();
                        printQueue_GridView.Refresh();
                    }
                }
            }
        }

        private void RemoveServer()
        {
            FrameworkServer server = SelectedServer;
            if (server != null)
            {
                // Determine all the queues that are currently in use for the print server
                var queuesInUse = _controller.SelectQueuesInUse(server);

                string message = string.Empty;
                bool canRemoveServer = false;
                if (queuesInUse.Count > 0)
                {
                    // If there are queues in use, then let the user know they can't remove the
                    // print server but they can have any unused queues removed.  If the user
                    // says no to the prompt, then nothing will be deleted.
                    message = Properties.Resources.RemoveUnusedQueues;
                }
                else
                {
                    // Since no queues for the print server are in use, then prompt the 
                    // user to delete the server and all queues.  Then determine what
                    // queues can be deleted.  If the user says no to the prompt, then
                    // nothing will be deleted.
                    message = Properties.Resources.RemovePrintServer.FormatWith(server.HostName);
                    canRemoveServer = true;
                }

                // Confirm with the user what they want to do.
                DialogResult dialogResult = MessageBox.Show
                (
                    message,
                    "Remove Print Server",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

                if (dialogResult == DialogResult.OK)
                {
                    // Determine if any queues can be deleted and prompt the user to confirm
                    var queuesNotInUse = _controller.GetQueuesNotInUse(server, queuesInUse);

                    // If there are queues to delete, then iterate over each one, mark it for delete and
                    // set the parent to the update state.  In addition remove that queue from the data
                    // grid so it's not visible any more.
                    if (queuesNotInUse.Count > 0)
                    {
                        foreach (var queue in queuesNotInUse)
                        {
                            _controller.Context.RemotePrintQueues.Remove(queue);
                            _printQueues.Remove(queue);
                        }
                    }

                    // The server can be removed if all queues can be removed. This was determined higher up
                    // If the server can be removed, then mark it for delete and remove it from the grid view.
                    if (canRemoveServer)
                    {
                        _controller.Context.FrameworkServers.Remove(server);
                        _printServers.Remove(server);
                    }

                    SaveChanges();
                }
            }
        }

        /// <summary>
        /// Cell Click handler for the print server data grid.
        /// </summary>
        /// <param name="sender">The Cell Element</param>
        /// <param name="e">The GridViewCellEventArgs</param>
        private void printServer_GridView_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                printServerSelected(e.RowIndex);
            }
        }

        /// <summary>
        /// Handles both initial grid load and cell click.
        /// </summary>
        /// <param name="rowIndex"></param>
        private void printServerSelected(int rowIndex)
        {
            FrameworkServer selectedServer = printServer_GridView.Rows[rowIndex].DataBoundItem as FrameworkServer;
            RefreshQueueDisplay(selectedServer);
        }

        /// <summary>
        /// Handles the Click event of the removeServer_ToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void removeServer_ToolStripButton_Click(object sender, EventArgs e)
        {
            RemoveServer();
            RefreshServerDisplay();
        }

        /// <summary>
        /// Handles the Click event of the refreshQueueList_ToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void refresh_ToolStripButton_Click(object sender, EventArgs e)
        {
            RefreshQueueList(SelectedServer);
        }

        /// <summary>
        /// Handles the Click event of the usage_ToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void usage_ToolStripButton_Click(object sender, EventArgs e)
        {
            FrameworkServer server = SelectedServer;
            if (server != null)
            {
                var queuesInUse = _controller.SelectQueuesInUse(server);
                PrintServerUsageDetails details = new PrintServerUsageDetails(server.HostName, queuesInUse);
                details.ShowDialog();
            }
        }

        private static bool ContinueEditing(FrameworkServerProxy server, string error)
        {
            DialogResult result = MessageBox.Show
            (
                $"Unable to connect to {server.HostName}.  {error} {Environment.NewLine}{Environment.NewLine}Enter server properties manually?",
                "Connection Error",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error
            );

            if (result == DialogResult.No)
            {
                return false;
            }

            server.Architecture = WindowsSystemInfo.ArchitectureX64;
            server.Cores = 1;
            server.Memory = 2000;

            return true;
        }

        private void newServer_ToolStripButton_Click(object sender, EventArgs e)
        {
            bool result = false;

            // Prompt for the hostname
            string serverName = InputDialog.Show("Server Hostname:", "Add Server");
            if (string.IsNullOrEmpty(serverName))
            {
                // User cancelled
                return;
            }

            if (serverName.StartsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                serverName = serverName.TrimStart(new char[] { '\\' });
            }

            try
            {
                Guid serverId = Guid.Empty;
                if (_controller.HostNameExists(serverName))
                {
                    FrameworkServer existingServer = _controller.Select(serverName);
                    serverId = existingServer.FrameworkServerId;
                    result = UpdateServerProperties(existingServer);
                }
                else
                {
                    serverId = SequentialGuid.NewGuid();
                    result = AddNewServer(serverName, serverId);
                }

                if (result)
                {
                    //Refesh The entire UI
                    RefreshServerDisplay();
                    int index = GetDataBoundItemIndex(serverId);
                    SetSelectedIndex(printServer_GridView, index);

                    if (_printServers.Count > 0)
                    {
                        RefreshQueueDisplay((FrameworkServer)printServer_GridView.CurrentRow.DataBoundItem);
                    }
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Add Print Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TraceFactory.Logger.Error("Error Adding Print Server with associated Queues", ex);
            }
            finally
            {
                SetDefaultCursor();
            }
        }

        /// <summary>
        /// Updates the server properties of an existing FrameworkServer to a Print Server.
        /// Scans the server for Print Queues and adds them to the database.
        /// </summary>
        private bool UpdateServerProperties(FrameworkServer server)
        {
            // Before displaying the server properties, add the Print Property to this server entry if it's not already there.
            if (!server.ServerTypes.Any(x => x.Name.Equals(ServerType.Print.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                server.ServerTypes.Add(_controller.GetServerType(ServerType.Print));
                SaveChanges();
            }

            // Instantiate the proxy class against the server, which will be used to edit the properties
            FrameworkServerProxy proxy = new FrameworkServerProxy(server);

            using (FrameworkServerEditForm form = new FrameworkServerEditForm(_controller, proxy))
            {
                if (form.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }

                // Copy the working copy (proxy) data back to the server object
                proxy.CopyTo(server);
                //At this point we want to save in case the refreshing of the queues fails for some reason
                SaveChanges();  
            }

            // If the grid isn't already showing the FrameworkServer, then add it.
            int index = GetDataBoundItemIndex(server.FrameworkServerId);
            if (index == -1)
            {
                _printServers.Add(server);
            }

            // The server was not a print server before, so we will need to refresh the print queues. 
            AddServerQueues(server);

            return true;
        }

        /// <summary>
        /// Adds a new Framework Server with Print properties and retrieves it's print queues.
        /// </summary>
        private bool AddNewServer(string serverName, Guid serverId)
        {
            // The server doesn't exist in configuration, so try to query it and load
            // that information so it can be reviewed and edited by the user.
            Cursor = Cursors.WaitCursor;
            string error = string.Empty;
            FrameworkServerProxy proxy = new FrameworkServerProxy(serverName);
            bool success = _controller.QueryServer(proxy, out error);
            Cursor = Cursors.Default;

            // If there is a failure reading server and/or the user doesn't want to continue, then return
            if (!success && !ContinueEditing(proxy, error))
            {
                return false;
            }

            FrameworkServer server = new FrameworkServer() { FrameworkServerId = serverId };

            // Add Print as a server type for this server before editing
            FrameworkServerType serverType = _controller.GetServerType(ServerType.Print);
            proxy.ServerTypes.Add(serverType);

            using (FrameworkServerEditForm form = new FrameworkServerEditForm(_controller, proxy))
            {
                if (form.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }

                proxy.CopyTo(server);
                //Save the new server to the database at this point.  If there is an error getting
                //print queues, the server data will be preserved.
                _controller.AddNewServer(server);
                _printServers.Add(server);
                SaveChanges();
            }

            // Scan the server for queues.
            AddServerQueues(server);

            return true;
        }

        /// <summary>
        /// Adds Queues for the specified server.  Only adds new queues to the database,
        /// does not check for PrintQueue database records that don't exist on the server.
        /// </summary>
        /// <param name="server">The Framework Server.</param>
        private bool AddServerQueues(FrameworkServer server)
        {
            Collection<string> queueNames = null;
            try
            {
                // Get the queues for the print server
                SetWaitCursor();
                queueNames = GetPrintQueueNames($"{server.HostName}.{GlobalSettings.Items[Setting.DnsDomain]}");
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Failed to get print queues: {0}".FormatWith(ex.Message), "Print Queue Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TraceFactory.Logger.Error("Failed to get print queues", ex);
                return false;
            }
            finally
            {
                SetDefaultCursor();
            }

            if (queueNames.Count > 0)
            {
                // Prompt the user to choose the queues they want to add from the Server.
                return FindMissingDatabaseQueues(server, queueNames);
            }

            MessageBox.Show(string.Format(Resources.NoQueuesAvailable, server.HostName), "Refresh Print Queues", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
        }

        /// <summary>
        /// Manually add new print queue name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newQueue_ToolStripButton_Click(object sender, EventArgs e)
        {
            FrameworkServer server = SelectedServer;
            string queueName = string.Empty;

            if (server == null)
            {
                MessageBox.Show("Please select a Print Server.", "Print Server not selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TraceFactory.Logger.Error("Error Adding Print Queue. No Print Server selected");
                return;
            }

            // Determine if this server is categorized as an ePrint server.  If it is, then prompt the user
            // to enter ePrint related information.  Otherwise it will treat it as a standard print server.
            string ePrintType = ServerType.ePrint.ToString();
            if (server.ServerTypes.Any(x => x.Name.Equals(ePrintType, StringComparison.OrdinalIgnoreCase)))
            {
                queueName = InputDialog.Show("Enter an ePrint email address", "Add ePrint Email Address");
                if (!string.IsNullOrEmpty(queueName))
                {
                    ManuallyAddQueue(server, queueName);
                }
            }

        }

        /// <summary>
        /// Manually remove a print queue from a print server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeQueue_ToolStripButton_Click(object sender, EventArgs e)
        {
            RemotePrintQueue queue = SelectedQueue;
            if (queue != null)
            {
                if (CanRemoveQueue(queue))
                {
                    DialogResult dialogResult = MessageBox.Show
                    (
                        Properties.Resources.RemoveQueueManually.FormatWith(queue.Name),
                        "Remove Queue",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question
                    );

                    if (dialogResult == DialogResult.OK)
                    {
                        RemoveQueue(queue);
                        SaveChanges();
                    }
                }
            }
        }

        private void editQueue_ToolStripButton_Click(object sender, EventArgs e)
        {
            RemotePrintQueue queue = SelectedQueue;
            FrameworkServer server = SelectedServer;
            if (server == null)
            {
                MessageBox.Show("Please select a Print Server.", "Print Server not selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TraceFactory.Logger.Error("No Print Server selected for editing queue");
                return;
            }
            if (queue == null)
            {
                MessageBox.Show("Please select a Print Queue.", "Print Queue Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TraceFactory.Logger.Error("Failed to get print queue. No Print Queue selected");
                return;
            }

            // Determine if this server is categorized as an ePrint server.  If it is, then allow the user
            // to edit ePrint related information.  Otherwise it will treat it as a standard print server.
            string ePrintType = ServerType.ePrint.ToString();
            
            if (server.ServerTypes.Any(x => x.Name.Equals(ePrintType, StringComparison.OrdinalIgnoreCase)))
            {
                string queueName = InputDialog.Show("ePrint email address", "Edit ePrint Email Address", queue.Name);
                if (!string.IsNullOrEmpty(queueName))
                {
                    EditQueue(queue, SelectedServer, queueName);
                }
            }
        }

        private bool CanRemoveQueue(RemotePrintQueue queue)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                List<string> queueUsages = GetScenariosUsingQueue(context, queue.RemotePrintQueueId.ToString()).ToList();

                if (queueUsages.Any())
                {
                    StringBuilder listString = new StringBuilder("\n");
                    listString.Append(string.Join(",\n", queueUsages));
                    listString.Append("\n\n");
                    MessageBox.Show(Resources.QueuesInUse.FormatWith(queue.Name, listString), "Remove Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                return true;
            }
        }

        private static IEnumerable<string> GetScenariosUsingQueue(EnterpriseTestEntities entities, string queueId)
        {
            return from mpqu in entities.VirtualResourceMetadataPrintQueueUsages.AsEnumerable()
                   where mpqu.PrintQueueSelectionData.Contains(queueId)
                   join vrm in entities.VirtualResourceMetadataSet
                       on mpqu.VirtualResourceMetadataId equals vrm.VirtualResourceMetadataId into Metadata
                   from m in Metadata
                   join vr in entities.VirtualResources
                       on m.VirtualResourceId equals vr.VirtualResourceId into Resources
                   from r in Resources
                   join es in entities.EnterpriseScenarios
                       on r.EnterpriseScenarioId equals es.EnterpriseScenarioId into Scenarios
                   from s in Scenarios
                   select s.Name;
        }

        private bool ManuallyAddQueue(FrameworkServer server, string queueName)
        {
            RemotePrintQueue queue = CreatePrintQueue(queueName);
            queue.PrintServerId = server.FrameworkServerId;
            _controller.Context.RemotePrintQueues.Add(queue);

            try
            {
                //This will throw if there is already an existing queue with the same name for the selected server.
                SaveChanges();
            }
            catch (UpdateException)
            {
                MessageBox.Show("'{0}' already exists.".FormatWith(queueName), "Add {0} Queue".FormatWith(_serverType.ToString()), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _controller.Context.RemotePrintQueues.Remove(queue);
                SetDefaultCursor();
                return false;
            }
            RefreshQueueDisplay(server);
            return true;
        }

        private void RemoveQueue(RemotePrintQueue queue)
        {
            _controller.Context.RemotePrintQueues.Remove(queue);
            _printQueues.Remove(queue);
        }

        /// <summary>
        /// Because Queue name is part of the primary key, we can't just change the name.
        /// We have to delete the old one and add it new.  This will work as long as the
        /// new name doesn't already exist.
        /// </summary>
        /// <param name="queueToDelete"></param>
        /// <param name="server"></param>
        /// <param name="newQueueName"></param>
        private void EditQueue(RemotePrintQueue queueToDelete, FrameworkServer server, string newQueueName)
        {
            if (CanRemoveQueue(queueToDelete))
            {
                RemoveQueue(queueToDelete);
                if (!ManuallyAddQueue(server, newQueueName))
                {
                    //At this point the data context is in a bad state.  Reload.
                    ReloadForm();
                }
            }
        }

        /// <summary>
        /// Disposes the existing data context and gets a new one.
        /// Reloads all lists.
        /// </summary>
        private void ReloadForm()
        {
            printServer_GridView.DataSource = null;
            printQueue_GridView.DataSource = null;
            _controller.Dispose();
            _printServers.Clear();
            _printQueues.Clear();

            _controller = new FrameworkServerController();

            foreach (var item in _controller.GetServersByType(_serverType))
            {
                _printServers.Add(item);
            }

            printServer_GridView.DataSource = _printServers;
            printServer_GridView.BestFitColumns();

            if (_printServers.Count > 0)
            {
                RefreshQueueDisplay(_printServers[0]);
                printServer_GridView.TableElement.ScrollToRow(0);
            }
        }

        private void printServer_GridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editServer_ToolStripButton_Click(sender, EventArgs.Empty);
        }

        private void editServer_ToolStripButton_Click(object sender, EventArgs e)
        {
            int index = (printServer_GridView.CurrentRow != null ? printServer_GridView.CurrentRow.Index : 0);
            EditEntry();
            RefreshServerDisplay();
            SetSelectedIndex(printServer_GridView, index);
        }

        private void EditEntry()
        {
            var row = GetFirstPrintServerSelectedRow();
            if (row != null)
            {
                var server = row.DataBoundItem as FrameworkServer;

                FrameworkServerProxy proxy = new FrameworkServerProxy(server);

                // The user will now edit the print properties of this server.
                using (FrameworkServerEditForm form = new FrameworkServerEditForm(_controller, proxy))
                {
                    if (form.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        // Copy the working copy (proxy) data back to the server object, and then
                        // save these changes to the database.
                        proxy.CopyTo(server);
                        SaveChanges();
                    }
                }
            }
        }

        private void editServer_ContextMenuItem_Click(object sender, EventArgs e)
        {
            editServer_ToolStripButton_Click(sender, e);
        }

        private void removeServer_ContextMenuItem_Click(object sender, EventArgs e)
        {
            removeServer_ToolStripButton_Click(sender, e);
        }

        private static Collection<string> GetPrintQueueNames(string serverName)
        {
            Collection<string> queues = new Collection<string>();

            using (PrintServer server = new PrintServer(@"\\" + serverName))
            {
                EnumeratedPrintQueueTypes[] queueTypes = new EnumeratedPrintQueueTypes[] { EnumeratedPrintQueueTypes.Shared };

                foreach (PrintQueue queue in server.GetPrintQueues(queueTypes))
                {
                    queues.Add(queue.Name);
                }
            }

            return queues;
        }
    }
}
