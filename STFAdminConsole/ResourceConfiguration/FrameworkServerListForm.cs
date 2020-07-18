using System;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI;
using Telerik.WinControls.UI;
using System.Linq;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// List form showing STF Server entries
    /// </summary>
    public partial class FrameworkServerListForm : Form
    {
        FrameworkServerController _controller = null;
        bool _unsavedChanges = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkServerListForm"/> class.
        /// </summary>
        public FrameworkServerListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(server_RadGridView, GridViewStyle.ReadOnly);
        }

        private void FrameworkServerListForm_Load(object sender, System.EventArgs e)
        {
            _controller = new FrameworkServerController();
            
            server_RadGridView.DataSource = _controller.ServersList;

            server_RadGridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            server_RadGridView.BestFitColumns();

            server_RadGridView.Refresh();
        }

        private void ok_Button_Click(object sender, System.EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _controller.Commit();
            Cursor = Cursors.Default;

            Close();
        }

        private void add_Button_Click(object sender, System.EventArgs e)
        {
            FrameworkServerProxy proxy = new FrameworkServerProxy();

            // Prompt for the hostname
            string inputText = InputDialog.Show("Server Hostname: ", "Add Server");
            if (string.IsNullOrWhiteSpace(inputText))
            {
                // User cancelled
                return;
            }

            // Determine if this hostname already exists
            if (_controller.HostNameExists(inputText))
            {
                MessageBox.Show("This hostname already exists", "Hostname Exists", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            proxy.HostName = inputText;
            string error = string.Empty;

            Cursor = Cursors.WaitCursor;
            bool foundHost = _controller.QueryServer(proxy, out error);
            Cursor = Cursors.Default;

            // If there is a failure reading server and the user doesn't want to continue, then return
            if (!foundHost && !ContinueEditing(proxy, error))
            {
                return;
            }

            if (EditEntry(proxy) == DialogResult.OK)
            {
                FrameworkServer server = new FrameworkServer() { FrameworkServerId = SequentialGuid.NewGuid() };
                proxy.CopyTo(server);
                _controller.ServersList.Add(server);
            }

            server_RadGridView.Refresh();
        }

        private static bool ContinueEditing(FrameworkServerProxy proxy, string error)
        {
            var result = MessageBox.Show
                (
                    "Unable to connect to server.  {0} {1}{2}Enter server properties manually?".FormatWith(error, Environment.NewLine, Environment.NewLine),
                    "Connection Error", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Error
                );

            if (result == System.Windows.Forms.DialogResult.No)
            {
                return false;
            }

            proxy.Architecture = WindowsSystemInfo.ArchitectureX64;
            proxy.Cores = 1;
            proxy.Memory = 2000;

            return true;
        }

        private DialogResult EditEntry(FrameworkServerProxy proxy)
        {
            DialogResult result = DialogResult.None;

            using (FrameworkServerEditForm form = new FrameworkServerEditForm(_controller, proxy))
            {
                result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _unsavedChanges = true;
                }
            }

            return result;
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            if (_unsavedChanges)
            {
                var result = MessageBox.Show("You have unsaved changes that will be lost.  Do you want to continue?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Close();
                    return;
                }
            }
            else
            {
                Close();
                return;
            }
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return server_RadGridView.SelectedRows.FirstOrDefault();
        }

        private void edit_Button_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var server = row.DataBoundItem as FrameworkServer;

                FrameworkServerProxy proxy = new FrameworkServerProxy(server);

                if (EditEntry(proxy) == DialogResult.OK)
                {
                    proxy.CopyTo(server);
                }
            }
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _controller.Commit();
            Cursor = Cursors.Default;

            _unsavedChanges = false;
        }

        private void delete_Button_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var server = row.DataBoundItem as FrameworkServer;

                if (server.ServerTypes.Select(n => n.Name).Any(n => n == ServerType.Print.ToString() || n == ServerType.ePrint.ToString()))
                {
                    RemovePrintServer(server);
                }
                else
                {
                    _controller.Remove(server);
                }

                _unsavedChanges = true;
            }
        }

        private void RemovePrintServer(FrameworkServer server)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            // Determine if there are any queues associated with this server and if they are in use
            if (_controller.SelectQueuesInUse(server).Count > 0)
            {
                MessageBox.Show
                    (
                        "This server is configured as a Print Server and has Print Queues associated with Test Scenarios.  " +
                        "You must use the 'Print Servers' configuration option to manage this server.",
                        "Delete Print Server",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation
                    );
                return;
            }

            var dialogResult = MessageBox.Show
                (
                    "This server is configured as a Print Server and will be removed from the STF configuration database.  Do you want to continue?",
                    "Delete Print Server",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question

                );

            if (dialogResult == DialogResult.Yes)
            {                
                _controller.Remove(server);
            }
        }

        private void server_RadGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var server = row.DataBoundItem as FrameworkServer;

                FrameworkServerProxy proxy = new FrameworkServerProxy(server);

                if (EditEntry(proxy) == DialogResult.OK)
                {
                    proxy.CopyTo(server);
                }
            }
        }

        private void toolStripButton_Types_Click(object sender, EventArgs e)
        {
            using (FrameworkServerTypeForm typesForm = new FrameworkServerTypeForm(_controller))
            {
                typesForm.ShowDialog(this);
            }
        }
    }
}
