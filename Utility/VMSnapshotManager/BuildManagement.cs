using HP.ScalableTest;
using HP.ScalableTest.Data.AssetInventory;
using HP.ScalableTest.Data.AssetInventory.Model;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Virtualization.VMWare;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace VMSnapshotManager
{
    public partial class BuildManagement : Form
    {
        private string _repoPath;
        private AssetInventoryContext _context;

        public BuildManagement()
        {
            InitializeComponent();
            serverFilter_ToolStripComboBox.ComboBox.DisplayMember = "Name";
            dispatchers_DataGridView.AutoGenerateColumns = false;
        }

        private void BuildManagement_Load(object sender, EventArgs e)
        {
            using (AssetInventoryContext context = new AssetInventoryContext())
            {
                serverFilter_ToolStripComboBox.ComboBox.DataSource = FrameworkServerType.SelectAll(context);
            }
        }

        private void browse_button_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbdBrowserDialog = new FolderBrowserDialog())
            {
                fbdBrowserDialog.Description = "Select the repository path..";
                fbdBrowserDialog.ShowNewFolderButton = false;
                var type = fbdBrowserDialog.GetType();

                FieldInfo fieldInfo = type.GetField("rootFolder", BindingFlags.NonPublic | BindingFlags.Instance);
                fieldInfo.SetValue(fbdBrowserDialog, 18);

                if (fbdBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    if (!Directory.Exists(Path.Combine(fbdBrowserDialog.SelectedPath, "VirtualResource")) && !fbdBrowserDialog.SelectedPath.Contains("VirtualResource"))
                    {
                        MessageBox.Show("Repository doesn't contain VirtualResource folder", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _repoPath = fbdBrowserDialog.SelectedPath;

                    repo_textBox.Text = _repoPath;
                    if (_repoPath.StartsWith("\\"))
                    {
                        _repoPath = _repoPath.Substring(2);
                    }
                    if (_repoPath.EndsWith("VirtualResource", StringComparison.OrdinalIgnoreCase))
                    {
                        _repoPath = _repoPath.Substring(0, _repoPath.LastIndexOf('\\'));
                    }
                }
            }
        }

        private void serverFilter_ToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedServer = serverFilter_ToolStripComboBox.ComboBox.SelectedItem as FrameworkServerType;
            dispatchers_DataGridView.DataSource = null;
            if (_context == null)
            {
                _context = new AssetInventoryContext();
            }

            if (selectedServer.Name == "Dispatcher")
            {
                var dispatchers =
                    FrameworkServer.Select(_context, ServerType.Dispatcher, GlobalSettings.Environment).ToList();
                dispatchers_DataGridView.DataSource = dispatchers;
            }
            else if (selectedServer.Name == "Print")
            {
                var printServers =
                    FrameworkServer.Select(_context, ServerType.Print, GlobalSettings.Environment).ToList();
                dispatchers_DataGridView.DataSource = printServers;
            }
            else if (selectedServer.Name == "Citrix")
            {
                var citrixServers =
                    FrameworkServer.Select(_context, ServerType.Citrix, GlobalSettings.Environment).ToList();
                dispatchers_DataGridView.DataSource = citrixServers;
            }
        }

        private void deploy_button_Click(object sender, EventArgs e)
        {
            if (dispatchers_DataGridView.SelectedRows.Count == 0 || string.IsNullOrEmpty(_repoPath))
            {
                return;
            }

            foreach (DataGridViewRow row in dispatchers_DataGridView.SelectedRows)
            {
                var server = row.DataBoundItem as FrameworkServer;
                var types = server.FrameworkServerTypes.Select(x => x.Name).ToList();
                if (server.IsPrintServer && !types.Contains(ServerType.Citrix.ToString()))
                {
                    DeployPrintMonitorService(server.HostName, server.DatabaseHostName);
                }
                else
                {
                    if (types.Contains(ServerType.Dispatcher.ToString()))
                    {
                        DeployDispatcher(server.HostName, server.DatabaseHostName);
                    }
                    else if (types.Contains(ServerType.Citrix.ToString()))
                    {
                        DeployCitrixServer(server.HostName, server.DatabaseHostName);
                    }
                }
                Application.DoEvents();
            }
        }

        private void DeployDispatcher(string dispatcher, string database)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                var sessions =
                    SessionInfo.SelectAll(context)
                        .Where(
                            x =>
                                x.Dispatcher == dispatcher && (x.ShutdownState != "Complete" && x.ShutdownState != "ManualReset" && x.ShutdownState != "Partial") && x.ShutdownDate != null);
                if (sessions.Any())
                {
                    SessionClient.Instance.Initialize(dispatcher);
                    ShutdownOptions shutdown = new ShutdownOptions();
                    shutdown.PowerOffOption = VMPowerOffOption.PowerOff;
                    shutdown.PowerOff = true;
                    shutdown.ReleaseDeviceReservation = true;

                    foreach (var session in sessions)
                    {
                        SessionClient.Instance.Shutdown(session.SessionId, shutdown);
                    }
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }
            }
            KillRemoteTask(dispatcher, "sessionproxy.exe");
            KillRemoteTask(dispatcher, "HP.STF.AdminConsole.exe");

            string argument = @"/c cd C:\VirtualResource\Deployment && C:\VirtualResource\Deployment\stf.installdispatcher.cmd {0} {1} /d".FormatWith(_repoPath, database);
            var pid = ExecuteGuestProcess(dispatcher, argument);

            if (pid <= 0)
            {
                UpdateStatus("Deployment failed on {0}".FormatWith(dispatcher));
                return;
            }

            UpdateStatus("Deployed on {0}".FormatWith(dispatcher));
        }

        private void DeployCitrixServer(string hostName, string databaseHostName)
        {
            string argument =
                @"/c cd C:\VirtualResource\Deployment && C:\VirtualResource\Deployment\stf.installCitrix.cmd {0} {1} /d"
                    .FormatWith(_repoPath, databaseHostName);

            var pid = ExecuteGuestProcess(hostName, argument);

            if (pid <= 0)
            {
                UpdateStatus("Deployment failed on {0}".FormatWith(hostName));
                return;
            }

            UpdateStatus("Deployed on {0}".FormatWith(hostName));
        }

        private void DeployPrintMonitorService(string hostName, string databaseHostName)
        {
            string argument =
                    @"/c cd C:\VirtualResource\Deployment && C:\VirtualResource\Deployment\stf.installprintmonitorservice.cmd {0} {1} /d"
                        .FormatWith(_repoPath, databaseHostName);
            var pid = ExecuteGuestProcess(hostName, argument);

            if (pid <= 0)
            {
                UpdateStatus("Deployment failed on {0}".FormatWith(hostName));
                return;
            }

            UpdateStatus("Deployed on {0}".FormatWith(hostName));
        }

        private int ExecuteGuestProcess(string hostName, string argument)
        {
            using (var virtualCenter = GetVirtualCenter())
            {
                var machine = virtualCenter.GetVirtualMachine(hostName);

                string executable = "CMD.EXE";

                var pid = machine.RunGuestProcess(GlobalSettings.Items[Setting.DomainAdminUserName],
                    GlobalSettings.Items[Setting.DomainAdminPassword], executable, argument, true);

                return (int)pid;
            }
        }

        private void KillRemoteTask(string machine, string taskname)
        {
            string argument = @"/c taskkill /f /im {0}".FormatWith(taskname);

            var pid = ExecuteGuestProcess(machine, argument);

            if (pid <= 0)
            {
                UpdateStatus("failed to kill {0} on {1}".FormatWith(taskname, machine));
                return;
            }

            UpdateStatus("Killed {0} on {1}".FormatWith(taskname, machine));
        }

        private static VirtualCenter GetVirtualCenter()
        {
            string serverUri = GlobalSettings.Items[Setting.VMWareServerUri];
            return new VirtualCenter
                (
                    new Uri(serverUri),
                    "{0}\\{1}".FormatWith(UserManager.LoggedInUser.Domain, UserManager.LoggedInUser.UserName),
                    BasicEncryption.Decrypt(UserManager.LoggedInUser.Password, Resource.Key)
                );
        }

        private void UpdateStatus(string text)
        {
            // If an invoke is required, this will call right back to itself through the
            // Invoke which will put it in the UI thread and then the else will come into
            // play and the control will be updated.
            if (log_textBox.InvokeRequired)
            {
                log_textBox.Invoke(new MethodInvoker(() => UpdateStatus(text)));
            }
            else
            {
                log_textBox.Text += text + System.Environment.NewLine;
            }
        }

        private void BuildManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            _context?.Dispose();
        }
    }
}